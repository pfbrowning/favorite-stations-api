# Favorite Stations API
[![Coverage Status](https://coveralls.io/repos/github/pfbrowning/favorite-stations-api/badge.svg?branch=master)](https://coveralls.io/github/pfbrowning/favorite-stations-api?branch=master)

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

## Roadmap To 1.0.0
* CORS
* Swagger
* Business operation response mapping
* Write proper readme

## Subsequent Goals
* Don't serialize empty Extensions in ProblemDetails responses
* Tags CRUD
* Station Tags CRUD