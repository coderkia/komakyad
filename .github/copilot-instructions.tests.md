# Copilot instructions for unit tests

## General principles
- Keep tests small, clear, and focused on behavior.
- Prefer deterministic tests with no hidden state or timing issues.
- Test observable behavior, not implementation details.
- Use Arrange, Act, Assert structure consistently.
- Name tests clearly to describe the scenario and expected result.
- Keep each test focused on one behavior or edge case.

## C# / .NET test quality
- Use xUnit conventions for naming and structure.
- Prefer real behavior over over-mocking.
- Mock only external boundaries and infrastructure dependencies when necessary.
- Avoid test-only production hooks or methods.
- Keep test setup minimal and readable.
- Validate both happy-path and failure-path scenarios.
- Avoid brittle assertions based on internal implementation details.
- Prefer meaningful assertions that cover outcomes and contracts.

## Coverage and maintainability
- Add or update unit tests whenever behavior changes or bugs are fixed.
- Keep tests fast and deterministic.
- Split large test classes into focused groups by feature or domain area.
- Extract helper setup methods only when they reduce duplication and improve clarity.
- Avoid huge test methods; prefer short, isolated tests.
- Keep files and test classes under a reasonable size and split when they become unclear.

## Test design guidance
- Prefer one assertion focus per test when possible.
- Use descriptive test names such as "Should_Return_Empty_List_When_No_Records_Exist".
- Cover edge cases, validation failures, null handling, and boundary conditions.
- Keep fixtures and mocks straightforward and easy to understand.
- If a test is too complex, refactor the production code to be simpler or split behavior into smaller units.

## Review checklist
- Is the test readable and maintainable?
- Does it validate behavior rather than implementation?
- Is the setup minimal and understandable?
- Are edge cases covered?
- Is the test deterministic and fast?
- Does it fail for the right reason and pass for the correct behavior?

Prefer simple, reliable tests that protect behavior without overcomplicating the suite.
