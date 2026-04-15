# Code Quest

## Group Members
  - Mikhail Kamochkin 100875940
  - Victor Ma 100789474
  - Anthony Ciceu 100787198
  - Ajani Walters 100778216

Code Quest is an ASP.NET Core 8 MVC learning platform for beginner programming lessons. It includes user accounts, guest access, lesson progression, unit-based content, quizzes, and lesson completion tracking backed by SQL Server and Entity Framework Core.

Demo video: https://youtu.be/Xe5Vm_W301o

Website Link: https://codequest-enh0b7dhanf4bjeb.canadacentral-01.azurewebsites.net/

## Features

- Multi-unit lesson flow with individual lesson pages and controller-based grading
- Account registration and login with cookie authentication
- Guest mode with limited access
- Lesson progress tracking per user
- Profile and progress summaries
- Syntax and pseudocode learning/game content

## Tech Stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core 8
- SQL Server
- Cookie authentication
- Session state
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
