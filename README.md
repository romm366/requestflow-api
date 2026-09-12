# RequestFlow

RequestFlow is a small REST API for tracking internal support requests.

## Problem

Small teams often receive internal IT requests through email or chat.
This makes it difficult to track open requests, priorities and resolution status.

RequestFlow provides a central place for recording and retrieving support tickets.

## Current Features

- Create support tickets
- Retrieve all support tickets
- Retrieve a ticket by ID
- Validate required ticket titles
- Automatically assign new tickets the status `Open`
- SQLite database persistence
- HTTP status handling:
  - 200 OK
  - 201 Created
  - 400 Bad Request
  - 404 Not Found

## Technologies

- C#
- ASP.NET Core Minimal APIs
- Entity Framework Core
- SQLite
- REST
- JSON
- Git / GitHub

## Architecture

Client
↓
ASP.NET Core REST API
↓
Entity Framework Core
↓
SQLite

## Ticket Model

- Id
- Title
- Description
- Priority
- Status
- CreatedAtUtc

## Planned Next Steps

- Update tickets
- Delete tickets
- Change ticket status
- Filter by status and priority
- Improve validation
- Add automated tests

