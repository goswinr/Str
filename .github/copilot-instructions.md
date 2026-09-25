# Project Guidelines

## Code Style
- Use F# idioms consistent with files in `Src/` and keep changes small and targeted.
- Preserve the compile order defined in `Src/Str.fsproj`; do not reorder source files unless explicitly required.
- Keep public API behavior aligned across .NET and Fable targets when touching code behind conditional compilation.
- Prefer existing exception and message patterns (`StrException` and `Format.truncated` in `Src/Extensions.fs`) for argument and bounds errors.

## Architecture
- The library is intentionally layered by compile order:
  1. `Src/StringBuilder.fs`
  2. `Src/ComputationalExpression.fs`
  3. `Src/Extensions.fs`
  4. `Src/Module.fs`
- Namespace `Str` exposes:
  - static class `Str` for string utilities,
  - computation expression `str`,
  - auto-opened string and `StringBuilder` extensions.
- JavaScript/TypeScript compatibility is implemented with `#if FABLE_COMPILER_*` blocks (notably in `Src/Module.fs`). Maintain parity with .NET behavior.

## Build and Test
- Build from repository root:
  - `dotnet build Str.sln`
  - `dotnet build Src/Str.fsproj --configuration Release`
- Run tests from `Tests/` (required):
  - .NET tests (Scriptorium): `dotnet run`
  - JS tests (Scriptorium, via `dotnet fable --runScript`): `npm test`
  - JS tests + TypeScript check: `npm run testTS`
- For first JS test runs or clean environments, run `dotnet tool restore` and install Node dependencies in `Tests/`.

## Conventions
- Tests use Scriptorium (`Scriptorium.Quill` for `testList`/`test` and `Runner.runTests`, `Scriptorium.Nib` for `assertThat`). The single entry point in `Tests/Main.fs` runs on both .NET and JS; there is no `--filter`, use `ftest`/`xtest` temporarily instead.
- Keep test definitions cross-platform: shared test logic lives in `Tests/Module.fs`, `Tests/Extensions.fs`, and `Tests/StringBuilder.fs`.
- If you modify behavior in Fable-specific branches, update or add tests that validate parity across .NET and JS paths.
- Prefer linking to canonical docs instead of duplicating long explanations:
  - `README.md` for usage and API examples.
  - `.claude/CLAUDE.md` for architecture and workflow details.