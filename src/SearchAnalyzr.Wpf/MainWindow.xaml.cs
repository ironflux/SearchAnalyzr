using Microsoft.VisualBasic;
using SearchAnalyzr.Wpf.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;

namespace SearchAnalyzr.Wpf
{
    public partial class MainWindow : Window
    {
        private readonly IHttpClientFactory httpClientFactory;
        public MainWindow(IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();
            this.httpClientFactory = httpClientFactory;
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                lblResultsHeader.Content = "Processing ....";

                HttpClient client = httpClientFactory.CreateClient();

                client.BaseAddress = new Uri("http://localhost:5000");

                HttpResponseMessage response = await client.PostAsJsonAsync("/api/submit", new SearchParams
                {
                    Keywords = txtKeywords.Text,
                    Url = txtUrl.Text
                });

                AnalyzrResult result = await response.Content.ReadFromJsonAsync<AnalyzrResult>();

                txtResults.Text = Constants.vbTab + (result.Positions.Count > 0 ? string.Join(Constants.vbTab, result.Positions) : "0");

                lblResultsHeader.Content = $"Displaying results for keywords '{txtKeywords.Text}' and URL '{txtUrl.Text}'";
            }
            catch(Exception ex)
            {
                lblResultsHeader.Content = ex.Message;
            }
        }
    }
}
