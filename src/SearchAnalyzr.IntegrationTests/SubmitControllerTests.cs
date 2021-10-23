using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SearchAnalyzr.IntegrationTests.Fakes;
using SearchAnalyzr.IntegrationTests.Helpers;
using SearchAnalyzr.WebApi;
using SearchAnalyzr.WebApi.Interfaces;
using SearchAnalyzr.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace SearchAnalyzr.IntegrationTests
{
    public class SubmitControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private const string keyword1 = "conveyancing software";
        private const string keyword2 = "smokeball conveyancing";
        public SubmitControllerTests()
        {
            var factory = new WebApplicationFactory<Startup>();

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddHttpClient<ISearchService, FakeSearchService>(c =>
                    {
                        c.BaseAddress = new Uri("https://fakeurl/search");
                    })
                    .ConfigurePrimaryHttpMessageHandler(handler =>
                    new HttpClientHandler()
                    {
                        AutomaticDecompression = DecompressionMethods.GZip
                    });
                });
            }).CreateClient();
        }

        [Fact]
        public async Task PostRequest_WithKeyword1_UrlSmokeball_Response_OK_TestAsync()
        {
            var data = new SearchParams { Keywords = keyword1, Url = "www.smokeball.com.au" };
            
            var response = await _client.PostAsJsonAsync("/api/submit", data, JsonSerializerHelper.DefaultSerialisationOptions);
            var returnValue = await response.Content.ReadFromJsonAsync<AnalyzrResult>();

            returnValue.Should().BeEquivalentTo(new AnalyzrResult { Positions = new List<int> { 2 } });
        }

        [Fact]
        public async Task PostRequest_WithKeyword2_UrlSmokeball_Response_OK_TestAsync()
        {
            var data = new SearchParams { Keywords = keyword2, Url = "www.smokeball.com.au" };

            var response = await _client.PostAsJsonAsync("/api/submit", data, JsonSerializerHelper.DefaultSerialisationOptions);
            var returnValue = await response.Content.ReadFromJsonAsync<AnalyzrResult>();

            returnValue.Should().BeEquivalentTo(new AnalyzrResult { Positions = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 42 } });
        }

        [Fact]
        public async Task PostRequest_WithKeyword1_UrlFacebook_Response_OK_TestAsync()
        {
            var data = new SearchParams { Keywords = keyword1, Url = "www.facebook.com" };

            var response = await _client.PostAsJsonAsync("/api/submit", data, JsonSerializerHelper.DefaultSerialisationOptions);
            var returnValue = await response.Content.ReadFromJsonAsync<AnalyzrResult>();

            returnValue.Should().BeEquivalentTo(new AnalyzrResult { Positions = new List<int> { 25 } });
        }

        [Fact]
        public async Task PostRequest_ToSimulate_Response_Unavailable_TestAsync()
        {
            var data = new SearchParams { Keywords = "random keyword to fake error response", Url = "www.smokeball.com.au" };

            var response = await _client.PostAsJsonAsync("/api/submit", data, JsonSerializerHelper.DefaultSerialisationOptions);
            
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        [Fact]
        public async Task PostRequest_MissingParams_BadRequest_TestAsync()
        {
            var data = new SearchParams();

            var response = await _client.PostAsJsonAsync("/api/submit", data, JsonSerializerHelper.DefaultSerialisationOptions);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
