

## API setup
### immediateplatform
- https://immediateplatform.dev/


## Job schedulling
### Job lib
#### TickerQ
- https://tickerq.net/
Not too active, months since last commit
#### Hangfire
- https://www.hangfire.io/

#### Quartz.net
- https://www.quartz-scheduler.net/

### Messaging lib
#### MassTransit
- https://masstransit.io/

#### Wolverine

## DB
### EFCore

### Marten
- Document storage
- Event Sourcing

## Primary Keys
### UUID
- larger
- more fragmentation
- slower to compare
### int
- will overflow at 2 000 000 000
- fast to compare
- is predictable sequence
### snowfalke
- look into it


## Data Formats
 - https://www.rfc-editor.org/rfc/rfc9457

 
 
# Examples / Templates

- https://github.com/dotnet/eShop
- https://github.com/fullstackhero/dotnet-starter-kit

# Issues

- OpenApi versioning
  - https://github.com/dotnet/aspnetcore/issues/56314
  - https://github.com/dotnet/eShop/blob/main/src/eShop.ServiceDefaults/OpenApiOptionsExtensions.cs
