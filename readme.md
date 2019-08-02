# Favorite Stations API

## Introduction
Favorite Stations API is a .NET Core Web API which persists an authenticated user's "favorite" internet radio stations to an underlying SQL Server database via Entity Framework Core.  CRUD operations for station, tag, and station-tag are provided in a RESTful manner.

It's primarily intended for use by [Browninglogic Radio](https://github.com/pfbrowning/ng-radio), but it's intentionally decoupled in order to be of use to any other consumers which might also find it useful.

This is a work in progress in its early stages, thus it's still very incomplete.

## Running Migrations

* `dotnet ef migrations add MigrationName`
* `dotnet ef database update`

## Todo
* Stations CRUD
* Tags CRUD
* Station Tags CRUD
* Swagger
* CORS
* JWT Authentication
* Consistent error responses
* Logging
* Azure Devops build / release pipelines
* Azure App Service hosting
* Code coverage badge
* Write tests
* Write proper readme