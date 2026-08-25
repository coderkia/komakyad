# Copilot instructions for KomakYad

## Architecture and boundaries
- Keep the solution layered and explicit.
- API projects should contain controllers, request/response handling, startup configuration, and HTTP concerns.
- Domain projects should contain business rules, interfaces, entities, and core logic.
- Data access projects should contain DbContext configuration, repository implementations, persistence logic, and migrations.
- Shared/common projects should contain reusable helpers, configuration, and cross-cutting concerns.
- Dependencies should flow inward. Domain logic should not depend on controllers, UI, or persistence infrastructure.
- Keep controllers thin. Do not put business logic in controllers.

## Design patterns and SOLID
- Follow SOLID principles consistently.
- Apply design patterns only when they solve a clear problem and reduce complexity.
- Prefer single responsibility over broad, multi-purpose classes.
- Keep interfaces small, explicit, and focused on one consumer need.
- Favor composition over inheritance.
- Prefer dependency inversion: depend on abstractions, not concrete implementations.
- Design for extension without forcing large changes to stable code.
- Write code that is easy to understand, change, and test.

## Dependency injection
- Use dependency injection for services, repositories, configuration consumers, and infrastructure dependencies.
- Prefer constructor injection over static access, service locators, and hidden global state.
- Register dependencies in the composition root, not inside domain logic or controllers.
- Keep constructors simple and explicit.
- Avoid creating concrete dependencies directly inside business logic.
- Use interfaces to isolate external systems and make code testable.

## C# and .NET code quality
- Use modern C# conventions and consistent naming.
- Prefer file-scoped namespaces.
- Use semicolons after namespace declarations in file-scoped style.
- Keep each file focused on one responsibility.
- Keep files smaller than 300 lines when practical.
- Split large files into smaller, cohesive files when they grow beyond a reasonable size.
- Keep methods small and single-purpose.
- Avoid long methods with many nested branches.
- Extract private helper methods instead of growing a method into a procedural block.
- Prefer readability and intent over cleverness.
- Use clear names and explicit flows.

## Testability
- Write code that is easy to test.
- Keep business logic separate from infrastructure, HTTP, and persistence concerns.
- Design around interfaces and abstractions.
- Prefer deterministic logic over hidden mutable state.
- Avoid static mutable state and hard-coded environment coupling.
- Do not add test-only production hooks just for unit tests.
- Keep tests focused on observable behavior.
- Add or update tests for behavior changes and bug fixes.
- Prefer unit tests with Arrange, Act, Assert structure.

## Implementation guidance
- Prefer the smallest correct change.
- Avoid speculative abstractions and unnecessary refactoring.
- Keep validation at the boundary and business rules in the appropriate domain/service layer.
- Use DTOs to separate public contracts from internal domain models when needed.
- Keep repository methods focused on data access and domain-level queries.
- Keep API actions thin and orchestration-focused.
- Use async/await correctly; avoid blocking calls and sync-over-async patterns.
- Pass cancellation tokens when supported by the underlying APIs.

## Clean code expectations
- Use clear naming for classes, methods, parameters, and variables.
- Use guard clauses and early returns to reduce nesting.
- Keep logic explicit and easy to follow.
- Avoid duplication by extracting reusable logic.
- Handle edge cases and validation explicitly.
- Keep exception handling meaningful and at the correct layer.
- Do not introduce unnecessary dependencies or frameworks.
- Preserve existing project conventions unless a change clearly requires a new pattern.

## Review checklist before finishing a change
- Does the code follow SOLID principles?
- Are responsibilities separated cleanly?
- Are dependencies injected properly?
- Are files and methods small enough to stay maintainable?
- Is the code easy to test?
- Does it respect existing layer boundaries?
- Is the implementation clear, explicit, and minimal?

This repository values clean architecture, maintainable code, and testable design. Prefer code that is simple, readable, and easy to verify.
