# Favorite Stations API
[![Build Status](https://toxicbard.visualstudio.com/Browninglogic%20Radio/_apis/build/status/Favorite%20Stations%20API?branchName=master)](https://toxicbard.visualstudio.com/Browninglogic%20Radio/_build/latest?definitionId=3&branchName=master)
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
* Swagger
* Business operation response mapping
* Write proper readme
* Users table: sub + iss identifier
* CORS

## Subsequent Goals
* Tags CRUD
* Station Tags CRUD
* Don't serialize empty Extensions in ProblemDetails responses
* Improve test coverage