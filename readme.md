# Favorite Stations API

## Introduction
Favorite Stations API is a .NET Core Web API which persists an authenticated user's "favorite" internet radio stations to an underlying SQL Server database via Entity Framework Core.  CRUD operations for station, tag, and station-tag are provided in a RESTful manner.

It's primarily intended for use by [Browninglogic Radio](https://github.com/pfbrowning/ng-radio), but it's intentionally decoupled in order to be of use to any other consumers which might also find it useful.

This is a work in progress in its early stages, thus it's still very incomplete.

## Requirements
* .NET Core 3.0 SDK
* .NET Ef Tools: `dotnet tool install --global dotnet-ef`

## Running Migrations
In the FavoriteStations.API directory:
* `dotnet ef migrations add MigrationName`
* `dotnet ef database update`

## Todo
* Stations CRUD endpoints
* Model validation
* Consistent error responses
* CORS
* Swagger
* Logging
* Azure Devops build / release pipelines
* Azure App Service hosting
* Code coverage badge
* Versioning
* Tags CRUD
* Station Tags CRUD
* Fill in remaining test coverage
* Write proper readme