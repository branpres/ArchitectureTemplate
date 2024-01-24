# ArchitectureTemplate

A .NET 8 Web API solution taking into account my experience with various "architectures" over the years and coming to the conclusion that we, as developers, create a lot of problems for ourselves. I am not a fan of abstractions that hold no real value IMO (looking at you, repository pattern), pointless indirection, gigantic god services, core business logic not being centralized, overuse of dependency injection (e.g. classes containing code that could have just as easily have been simple extension methods), mocks (with regards to testing - I much prefer to test from the outer most public interface for the most part so that my tests end up using the real implementations and so that code can more easily be refactored), projects where it's just hard to tell at a glance what the code does (i.e. what features does the software have). In short, I'm not a fan of things that just end up turning your code into a big mess.

I like various aspects of clean/onion/hexagonal architecture, vertical slice architecture, even N-tier architecture. I like keeping the HTTP specific code separate from the business logic specific code. However, I think we can go overboard with the number of projects in the solution. Hence why this template has just one production code project. The WebAPI project contains everything you need: HTTP related stuff and business logic in a vertical slice setup. You could potentially have multiple projects in a more, clean architecture, style solution: Web API, application, infrastructure, and domain projects. However, after having built and worked on several projects structured like that, I had to ask...why? For the life of me, I cannot think of a good reason to keep doing that. During the process of building out this template, at the beginning I had 3 separate projects. However, I eventually realized that it was silly to do that and just moved all the code into the one WebAPI project. After doing so, I found that organizationlly, things just tended to fit together better. You don't need separate projects for this stuff, separation via folders works just as well, if not better. 

Long story short, this template is a demonstration of my latest personal opinions on how to structure a .NET Web API solution using a vertical slice architecture, a structure that is simple (IMO anyway), pragmatic, and more easily reveals the intent of what the software actually does. It's definitely not exhaustive by any stretch, but hopefully a decent enough starting point.

### Some tech/styles/patterns used

- Minimal API
- SQLite
- IExceptionHandler
- Swagger/Open API (still learning how to use it to document the API)
- FluentValidation
- EF Core
- Testcontainers (for spinning up a docker container for integration testing against a real MySQL database)
- HATEOAS (various actions that can be performed on a resource that are returned as links with an endpoint response)
- CQRS (ish)
- DDD (still learning how to apply some of the concepts - perhaps DDD lite at best)
- Domain Events
- Vertical Slices
- Outbox pattern (maybe a bit of a crude implementation of it)
- Result pattern (all responses return whether it was successful or whether it failed, eliminates throwing exceptions as a way of handling errors in normal control flow)
- Integration tests
