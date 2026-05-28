1.	Architecture layers (project & responsibilities)
•	Presentation (InventoryAPI.Api)
•	ASP.NET Core Web API controllers, request/response DTOs, API versioning, Swagger, input model validation.
•	Keep controllers thin—delegate to Application services.
•	Application (InventoryAPI.Application)
•	Use cases, commands/queries, orchestration of domain operations, input/output DTOs, interfaces for repositories & integrations.
•	Domain (InventoryAPI.Domain)
•	Entities, value objects, domain services, domain events, invariants and business rules.
•	Data Access (InventoryAPI.Data)
•	Repository & Unit of Work interfaces and Dapper/EF implementations. Mapping between domain and persistence models.
•	Infrastructure (InventoryAPI.Infrastructure)
•	Concrete implementations: email, storage (Blob), caching (Redis), logging sinks (Serilog), Key Vault integration, external API clients.
•	Integration (InventoryAPI.Integration)
•	Message brokers, external services, adapters (e.g., Azure Service Bus, Kafka, 3rd-party suppliers).
Project layout recommendation (solution)
•	InventoryAPI.sln
•	InventoryAPI.Api (Presentation)
•	InventoryAPI.Application
•	InventoryAPI.Domain
•	InventoryAPI.Data (Dapper/EF implementations)
•	InventoryAPI.Infrastructure
•	InventoryAPI.Integration
•	InventoryAPI.Tests (unit, integration)
•	InventoryAPI.Contracts (shared DTOs if needed)
2.	Design patterns
•	Dependency Injection (built-in DI): register interfaces in Program.cs with scoped/singleton/lifetime based on responsibility.
•	Repository + Unit of Work: abstract persistence; UnitOfWork for transactional operations spanning repos.
•	CQRS: separate read models (fast queries) from write models when read/write complexity grows; start with simple command/query separation in Application layer.
•	Domain-Driven Design (DDD): bounded contexts for different domains (Products, Suppliers, Inventory), entities/value objects, domain events.
•	Mediator (MediateR): use for decoupling request handlers (commands/queries).
•	Adapter/Facade: for 3rd-party systems and infrastructure concerns.
3.	Standards & best practices
•	SOLID principles and single responsibility per class.
•	Naming: PascalCase for types and methods, camelCase for parameters, async suffix for async methods (GetProductAsync).
•	DTOs separate from domain entities; map with Mapster/AutoMapper only in Application/Data layer.
•	Keep persistence models internal to Data project.
•	Error handling: central exception middleware; return standardized error envelopes (RFC 7807 Problem Details).
•	Use appsettings.{Environment}.json + secrets for env-specific config; no secrets in source.
•	Enforce code quality: analyzers (Roslyn), StyleCop, EditorConfig.
4.	Security
•	Authentication/Authorization
•	JWT access tokens for API; refresh tokens if long sessions needed.
•	For enterprise SSO: OAuth2/OIDC (Azure AD) for interactive users.
•	Role-based and policy-based authorization inside controllers/services.
•	Secrets & keys
•	Use Azure Key Vault or OS-level secret store; do NOT store keys in appsettings.json.
•	Input validation
•	Use FluentValidation in Application layer; strong model validation attributes in API.
•	Encryption
•	Transport: TLS enforced.
•	Data: encrypt sensitive columns; use transparent data encryption or application-level encryption for PII.
•	Defense
•	Rate limiting, anti-forgery for browser endpoints, validate file uploads, limit payload sizes.
5.	Scalability & performance
•	Caching
•	Use Redis distributed cache for hot read models, frequent lookups.
•	Async/await everywhere for I/O-bound work. Use ConfigureAwait(false) in libraries as needed.
•	DB optimizations
•	Proper indexes, pagination, parameterized queries (Dapper protects vs SQL injection).
•	Use read replicas and read routing for heavy read workloads.
•	Logging & observability
•	Structured logging with Serilog, include correlation IDs.
•	OpenTelemetry tracing, export to Application Insights / Prometheus + Jaeger.
•	Rate limits and throttling per-client.
•	Bulk/Batch operations for imports.
6.	DevOps & deployment
•	CI/CD
•	GitHub Actions pipelines: build, run unit tests, run lint/analyzers, build Docker image, push to registry, deploy to staging, run integration tests, promote to production.
•	Gate deployments on approvals and passing tests.
•	Containerization
•	Single responsibility images; multi-stage Dockerfile; run as non-root.
•	Orchestration
•	Kubernetes with Helm charts for manifests; use liveness/readiness probes, HPA.
•	Environment config
•	Use feature flags and per-environment secrets. Use Kubernetes Secrets, Azure Key Vault or HashiCorp Vault.
•	Example GitFlow branching: feature/* → develop → release/* → main → hotfix/*.
7.	Testing strategy
•	Unit tests with xUnit + Moq; test only units, mock external dependencies.
•	Integration tests
•	Use Testcontainers for SQL Server, Redis; run real Dapper queries.
•	End-to-end / Contract tests
•	Contract tests for external integrations (Pact).
•	Regression/Smoke tests in pipeline; create test data builders and seeders; isolate test DB.
•	Coverage thresholds on PRs.
8.	Documentation & governance
•	API docs: Swagger/OpenAPI with XML comments; expose staged docs for staging/prod with auth.
•	API versioning: path versioning /api/v1/, semantic versioning for packages.
•	Code reviews: require PR reviews, automated checks (linters, tests).
•	Security reviews and periodic dependency scanning (Dependabot).
•	Observability runbooks and SLOs/SLIs.
9.	Future growth (microservices & event-driven)
•	Start modular monolith (clear layer boundaries). When scale needed, split by bounded context (Products, Inventory, Ordering).
•	Introduce event-driven architecture with events (Outbox pattern) and message broker (Kafka/Azure Service Bus).
•	Use Saga orchestration for cross-service transactions and eventual consistency; idempotency keys and deduplication for events.
•	Contract-first APIs and API gateway for routing, security, and rate limiting.
Step-by-step feature delivery flow (example)
1.	Create feature branch: feature/INV-123-add-ingredient-source (follow GitFlow).
2.	Update Domain (add entity/value object).
3.	Implement Application command/query + MediatR handler + validation.
4.	Add Repository method signature in Data abstraction.
5.	Implement persistence in Data (Dapper query) and Infrastructure changes (if needed).
6.	Add unit tests for Domain & Application, integration tests hitting test DB container.
7.	Run local linter/tests; create PR.
8.	CI runs: build, tests, static analyzers, container build.
9.	Deploy to staging; run integration & smoke tests.
10.	Manual/automated acceptance; merge to release/main; deploy to production.
Why these choices (brief justification)
•	Layered + DDD + CQRS gives maintainability and clear separation of concerns — necessary for enterprise change velocity.
•	DI, repositories, unit of work simplify testing and swapping implementations (EF, Dapper).
•	JWT/OAuth and secret stores meet enterprise security requirements.
•	Containerization + Kubernetes plus CI/CD provide scalable, repeatable deployments and DevOps maturity.
•	Observability + structured logging + tracing enable operational excellence.
If you want, I can:
•	Generate a starting solution skeleton with the project-per-layer layout (C# .NET 8) and example files (DI registration in Program.cs, repository/interface examples), or
•	Produce a GitHub Actions CI workflow and a sample Dockerfile/Helm chart tuned for your repo.
Which would you like me to generate first?
