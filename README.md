# SmartScheduleBackend

This is SmartSchedule application.


# Common Layer

This will contain all cross-cutting concerns.

# Application Layer

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project.
This layer defines interfaces that are implemented by outside layers. 
For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

# Domain Layer

This will contain all entities, enums, exceptions, types and logic specific to the domain.
The Entity Framework related classes are abstract, and should be considered in the same light as .NET Core.
For testing, use an InMemory provider such as InMemory or SqlLite.

# Infrastructure Layer

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on.
These classes should be based on interfaces defined within the application layer.

# Persistence Layer

This layer contains all configuration to databases.

# Presentation Layer

This layer contains API(controllers .etc)
