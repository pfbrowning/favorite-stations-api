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
* Unhandled error middleware
* Move test projects into a single project separated by namespace
* CORS
* Swagger
* Code coverage badge
* Business operation response mapping
* Tags CRUD
* Station Tags CRUD
* Write proper readme
* Fill in remaining test coverage