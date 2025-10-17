Saas application

POC project setup for evaluating C# backend architecture for a SaaS application
The company has a few requirements that need to be met / solutions demonstrated in the POC

The project will have a simple React SPA as the frontend this is not in scope for this POC

# Requirements

- **Multi tennant setup** Single deployment for all customers
- Book API
    - Books Crud
    - Authors Crud
- Email sending
    - Emails must be sent of plains are falling XD
- Start long running tasks really fast
    - Need to start large compute intensive tasks fast, startup should be in the <30ms range
    - some tasks can be in the 1-3min timeframe
    - Live notification when task is done to the frontend
- Eventing setup for backend internal domain events
- User management
    - Role based access control
    - multiple external SSO per customer
- Domain based customer separation
- Public API for external users (api versioning, api keys etc)
- DB migration handling
- Docker files
- K8S deployment
- Code style enforcement
- DB postgresql
- easy mapping between Database entities and domain models and DTOs
- Logging

This is the rough list of requirements that the POC must demonstrate how to best do in c#

# Libs
## Orchestration
### Aspire
Manages what services need to run and how they connect

## Web framework
ASP.NET Core
## Database
Entity Framework Core
## Logging
Serilog (I just like it)

## Mapping library
- https://mapperly.riok.app/



# To be decided
- Background task processing library
- Eventing library
- Email sending library
- User management library

