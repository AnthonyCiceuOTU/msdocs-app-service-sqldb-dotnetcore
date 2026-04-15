# Code Quest

Code Quest is an ASP.NET Core 8 MVC learning platform for beginner programming lessons. It includes user accounts, guest access, lesson progression, unit-based content, quizzes, and lesson completion tracking backed by SQL Server and Entity Framework Core.

This repository appears to have started from an Azure App Service + SQL Database sample, but the current app is focused on interactive programming lessons rather than the original CRUD tutorial.

Demo video: https://youtu.be/Xe5Vm_W301o

## Features

- Multi-unit lesson flow with individual lesson pages and controller-based grading
- Account registration and login with cookie authentication
- Guest mode with limited access
- Lesson progress tracking per user
- Profile and progress summaries
- Syntax and pseudocode learning/game content
- Optional AI-assisted grading for some explanation-style questions

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core 8
- SQL Server / LocalDB
- Cookie authentication
- Session state
- Azure Key Vault integration for non-development environments
- Redis cache in non-development environments
- Google Gemini integration for selected short-answer grading

## Project Structure

- `Controllers/` MVC controllers for lessons, login, profile, games, and navigation
- `Views/` Razor views for lesson pages and UI
- `ViewModels/` strongly typed lesson and page view models
- `Models/` entity models and config models
- `Data/` EF Core `DbContext`
- `Migrations/` database migrations
- `Services/` app services such as AI grading
- `wwwroot/` static assets

## Local Development

### Prerequisites

- .NET 8 SDK
- SQL Server LocalDB or a local SQL Server instance
- `dotnet-ef` CLI tool if you need to run migrations

Install EF Core CLI if needed:

```powershell
dotnet tool install --global dotnet-ef
```

### Configuration

Development uses `appsettings.Development.json` and expects this connection string key:

```json
"ConnectionStrings": {
  "MyDbConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CodeQuestDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

If LocalDB is not working on your machine, you can switch to a local SQL Server instance by updating `MyDbConnection`.

The app only loads Azure Key Vault outside Development, so you do not need Azure credentials for normal local work.

### Database Setup

Apply migrations before first run:

```powershell
dotnet ef database update
```

If you get an error like `Cannot open database "CodeQuestDb"`, make sure:

- LocalDB is installed
- the `MSSQLLocalDB` instance exists and is running
- the database has been created by applying migrations

### Run the App

```powershell
dotnet run
```

By default the app starts at:

- `http://localhost:5093`

The default route goes to the login page:

- `/Login/Index`

## Authentication and Access

- Registered users can sign in and save lesson progress
- Guest users can continue without an account
- Guest access is restricted to Unit 1 content
- Most pages require authentication through the app's fallback authorization policy

## AI Grading

Some lessons use an `IAiShortAnswerGrader` implementation backed by Google Gemini.

Relevant config:

- config key: `GeminiAPIKey`
- configured model: `gemini-2.5-flash`

If no Gemini API key is configured, AI-graded features may not work, but the rest of the application can still run.

## Deployment Notes

The app is set up to behave differently outside Development:

- SQL connection is read from `AZURE_SQL_CONNECTIONSTRING`
- Redis cache is read from `AZURE_REDIS_CONNECTIONSTRING`
- Azure Key Vault is loaded from `https://codequest-key-vault.vault.azure.net/`

That means production-style hosting should provide those settings through environment variables or Azure configuration.

## Useful Commands

Build:

```powershell
dotnet build
```

Run migrations:

```powershell
dotnet ef database update
```

Run the app:

```powershell
dotnet run
```

## Contributing

If you're working in this repo with other contributors:

- avoid force-pushing shared branches unless everyone agrees
- check for in-progress lesson changes before editing unit content
- prefer small commits grouped by unit or feature

See [CONTRIBUTING.md](CONTRIBUTING.md) for any repo-specific contribution guidance.

## License

This repository includes a [LICENSE.md](LICENSE.md) file. Review it before reusing or redistributing the project.
