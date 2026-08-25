# Copilot instructions for the Angular app

## Architecture and boundaries
- Keep the Angular app organized by feature and responsibility.
- Prefer feature folders or domain-oriented modules over large flat folder structures.
- Separate presentation, state, services, models, and utilities cleanly.
- Keep components focused on rendering and user interaction.
- Move business logic, transformations, and API orchestration into services or state logic.
- Keep configuration and environment-specific values in dedicated config files.

## Angular and TypeScript quality
- Use clean TypeScript conventions and avoid unnecessary complexity.
- Prefer small, focused components and simple service contracts.
- Keep files reasonably small and split large files when they become hard to follow.
- Keep methods short and single-purpose.
- Avoid long lifecycle methods with too much behavior.
- Prefer clear naming for components, services, models, and utility functions.
- Use strict typing and avoid `any` unless unavoidable.

## Dependency injection and state
- Use Angular dependency injection for services and shared dependencies.
- Keep services focused on one concern such as API access, validation, or state management.
- Avoid hidden global state and shared mutable singletons when a service or state store is enough.
- Keep component constructors simple and explicit.
- Prefer centralized state management patterns for cross-cutting data flows when needed.

## Testing guidance
- Write unit tests for components, services, and logic that affect user-visible behavior.
- Prefer real behavior assertions over implementation-heavy tests.
- Keep tests isolated and deterministic.
- Use Arrange, Act, Assert structure consistently.
- Mock external HTTP calls and browser APIs only at the boundary.
- Avoid brittle tests tied to CSS class names or implementation details when a behavior-based test is possible.

## Clean code expectations
- Keep logic explicit and readable.
- Prefer small helper functions over large inline logic blocks.
- Handle loading, error, and empty states clearly.
- Avoid duplication by extracting reusable logic into shared utilities or services.
- Keep RxJS streams readable; avoid overly complex chains when smaller steps are clearer.
- Use simple, maintainable patterns over clever technical workarounds.

## Review checklist before finishing a change
- Does the code follow Angular/TypeScript conventions?
- Are components and services small and focused?
- Are dependencies injected properly?
- Is the logic easy to test and maintain?
- Are large files or long methods avoided?
- Does the implementation respect feature boundaries and keep the app organized?

Prefer maintainable Angular code that is easy to understand, easy to test, and easy to extend.
