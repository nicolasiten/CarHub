# CarHub

# Projects
## CarHub.Core
- Business/Application Model
- Business Logic

## CarHub.Infrastructure
- Data Access Logic
	- EF Migrations
	- EF DbContext and model design

## CarHub.Web
- Presentation Logic

## *.Tests
- Unit and Integration Tests

# Comments
## DB Access
For the DB Access there is a generic Repository (EfRepository) in the Infrastructe project. There must be one class that implements the AbstractValidator (FluentValidation) for each entity. Entities get validated on each Add/Update.

## DB Configuration
For each entity there is a Configuration class that implements the IEntityTypeConfigurator.

## Mappings
Mappings between Entities and Models are done with the AutoMapper.
