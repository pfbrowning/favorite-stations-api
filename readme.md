# Favorite Stations API
[![Build Status](https://toxicbard.visualstudio.com/Browninglogic%20Radio/_apis/build/status/Favorite%20Stations%20API?branchName=master)](https://toxicbard.visualstudio.com/Browninglogic%20Radio/_build/latest?definitionId=3&branchName=master)
[![Coverage Status](https://coveralls.io/repos/github/pfbrowning/favorite-stations-api/badge.svg?branch=master)](https://coveralls.io/github/pfbrowning/favorite-stations-api?branch=master)

## Introduction
Favorite Stations API is a .NET Core Web API which persists an authenticated user's "favorite" internet radio stations to an underlying SQL Server database via Entity Framework Core.  CRUD operations for station, tag, and station-tag are provided in a RESTful manner.

It's primarily intended for use by [Browninglogic Radio](https://github.com/pfbrowning/ng-radio), but it's intentionally decoupled in order to be of use to any other consumers which might also find it useful.

## Requirements
* .NET Core 3.1 SDK
* .NET Ef Tools: `dotnet tool install --global dotnet-ef`

## Running Migrations
You'll need to run EF migrations in order to configure your database before running the API.  In the FavoriteStations.API directory:
* `dotnet ef migrations add MigrationName`
* `dotnet ef database update`

## CORS
TODO Fill this in

## Authentication
This API is designed to use standard Oauth2 + OpenID Connect bearer token authentication.  You'll need to configure your own identity provider if you want to run your own instance of the API.  It's based on a standard OIDC setup, so it should be fairly straightforward to configure with the following in mind:
* You'll need your bearer token to be configured as a JWT
* The API uses the OIDC Standard ["sub" and "iss"](https://openid.net/specs/openid-connect-core-1_0.html#IDToken) claims as a [unique user identifier](https://openid.net/specs/openid-connect-core-1_0.html#ClaimStability), thus you'll need to be sure that these claims are included in your bearer token.
* I recommend using RS256 and the OIDC metadata endpoint.  Depending on your identity provider of choice, doing this should make configuring the API as simple as configuring an authority and audience within your `appsettings.json`.

## Roadmap To 1.0.0
* Fix "StatusCode cannot be set because the response has already started."
* CORS
* Finish readme

## Subsequent Goals
* Tags CRUD
* Station Tags CRUD
* Don't serialize empty Extensions in ProblemDetails responses
* Improve test coverage