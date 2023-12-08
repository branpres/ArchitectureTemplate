# ArchitectureTemplate

A .NET 8 Web API solution taking into account my experience with various "architectures" over the years and coming to the conclusion that we, as developers, create a lot of problems for ourselves. I am not a fan of abstractions that hold no real value IMO (looking at you, repository pattern), pointless indirection, gigantic god services, core business logic not being centralized, overuse of dependency injection (e.g. classes containing code that could have just as easily have been simple extension methods), mocks (with regards to testing - I much prefer to test from the outer most public interface for the most part so that my tests end up using the real implementations), projects where it's just hard to tell at a glance what the code does (i.e. what features does the software have). In short, I'm not a fan of things that just end up turning your code into a big mess.

I like various aspects of clean/onion/hexagonal architecture, vertical slice architecture, even N-tier architecture. I like keeping the HTTP specific code separate from the "application" specific code. However, I think we can go overboard with the number of projects in the solution. Hence why this template has just two projects. One is the Web API project that defines the endpoints along with any other web related stuff. The other is where the business logic resides. You could potentially have 4 projects in a more, clean architecture, style solution: Web API, application, infrastructure, and domain projects. However, after having built and worked on several projects structured like that, I had to ask...why? For the life of me, I cannot think of a good reason to keep doing that. During the process of building out this template, at the beginning I had a separate Domain project. However, I eventually noticed that it was silly to have that and just moved that code into the Application project. After doing so, some classes fit even better in other folders than just simply keeping the same folder structure I was using in the Domain project.

Long story short, this template is a demonstration of my latest personal opinions on how to structure a .NET Web API solution, a structure that is simple (IMO anyway, debatable), pragmatic, and reveals more easily the intent of what the software actually does. It's definitely not exhaustive by any stretch, but hopefully a decent enough starting point.

### Some tech/styles/patterns used

- Minimal API
- SQLite
- IExceptionHandler
- Swagger (not much, still learning how to use it)
- FluentValidation
- EF Core
- Testcontainers (for spinning up a docker container for integration testing against a real MySQL database)
- HATEOAS (not happy with how I'm currently providing links in responses, but hopefully can eventually find a decent way of doing this generically)
- CQRS (ish)
- DDD (still learning how to apply some of the concepts - perhaps DDD lite at best)
- Domain Events
- Outbox pattern (maybe a bit of a crude implementation of it)
- Result pattern (all responses return whether it was successful or whether it failed, eliminates throwing exceptions as a way of handling errors in normal control flow)
- Integration tests
