# Crypto Trading Simulator

A cryptocurrency trading simulator backend application built with ASP.NET Core Web API.

## Tech stack

- ASP.NET Core .NET 9
- Entity Framework Core
- Microsoft SQL Server
- AutoMapper
- Swagger

## Setup

1. 'dotnet ef database update'
2. Run 'initdb.sql' in SSMS
3. Start the app
4. Swagger: 'https://localhost:xxxx/swagger'

## Features

- User registration and management
- Virtual wallet with starting balance
- Buy and sell cryptocurrencies
- Automatic price updates every 60 seconds
- Profit/loss calculation
- Transaction history
