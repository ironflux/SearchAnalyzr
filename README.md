# Search Analyzr

A simple fun experiment with ASP.NET Core REST API as backend and WPF as frontend. The app takes in two parameters: 'Keywords' and a 'URL'; performs a Google search and returns the position(s) of the specified URL in the search results.

## How to run

You need to first make sure you have **[.NET 5.0](https://dotnet.microsoft.com/download)** and **[Git](https://git-scm.com/downloads)** installed on your machine and then follow the steps below to get the projects up and running. 

### Run the following commands on your favourite CLI tool

- Clone project to a directory in you local machine

```bash
  git clone https://github.com/ironflux/SearchAnalyzr.git
```

- Navigate to the Web API project (*replace [your local directory] with your local directory path to the cloned repo*)

```bash
  cd [your local directory]\SearchAnalyzr\src\SearchAnalyzr.WebApi
```

- Start the Web API

```bash
  dotnet run
```

### The Web API backend will be accessible via the link below:

[http://localhost:5000/index.html](http://localhost:5000/index.html)

The REST API endpoint can be tested via the Swagger UI interface. 

### A published version WPF UI executable can be found here:

```bash
[your local directory]\SearchAnalyzr\Repo\src\SearchAnalyzr.Wpf\bin\publish\SearchAnalyzr.Wpf.exe
```
Navigate to the executable file via the file explorer and double-click to run.

Note: Make sure that backend is up and running before submitting requests via the WPF app, as the app is configured to post requests to the backend at http://localhost:5000/.