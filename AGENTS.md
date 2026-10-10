# Project guidelines

Str is an F# string extension and module library for .NET and JavaScript/TypeScript via Fable. Keep changes small, targeted, and consistent with the F# style in `Src/`. See `README.md` for usage and API examples.

## Architecture and code style

Preserve the compilation order in `Src/Str.fsproj`:

1. `Src/StringBuilder.fs`: `System.Text.StringBuilder` extensions such as `IndexOf`, `Contains`, and `Add`.
2. `Src/ComputationalExpression.fs`: the `str` computation expression, built on `StringBuilder`.
3. `Src/Extensions.fs`: `Format` helpers, basic auto-opened string extensions, and advanced extensions with `StrException` (including indexing and slicing).
4. `Src/Module.fs`: the main static `Str` class and string manipulation functions.

Opening the `Str` namespace exposes the static `Str` class, the `str` computation expression, and auto-opened string and `StringBuilder` extensions.

- Do not reorder source files unless the change requires it.
- Preserve public API behavior across .NET and Fable targets. JavaScript/TypeScript-specific implementations use `#if FABLE_COMPILER_*` directives, especially in `Src/Module.fs`. For example, `Str.indicesOf` uses a Knuth-Morris-Pratt implementation in Fable where .NET uses an `IndexOf` overload, and `Str.normalize` uses JavaScript-specific diacritic removal.
- For argument and bounds errors, follow existing `StrException` messages and use `Format.truncated` from `Src/Extensions.fs` where appropriate.

## Build and test

Run builds from the repository root:

```bash
dotnet build Str.sln
dotnet build Src/Str.fsproj
dotnet build Src/Str.fsproj --configuration Release
```

The library `Src/Str.fsproj` targets `netstandard2.0`. The tests in `Tests/Tests.fsproj` target `net8.0` only, so the .NET 8 runtime is required to run them (any SDK that can build `net8.0`, e.g. .NET 10 SDK, works).

Run tests from `Tests/`:

```bash
dotnet run           # .NET tests
npm test             # JavaScript tests via Fable and Node.js
npm run testTS       # JavaScript tests and TypeScript compilation
npm run watchTS      # Watch mode for TypeScript development
```

For a first JavaScript test run or a clean environment, run `dotnet tool restore` from the repository root and `npm install` in `Tests/`.

## Test conventions

The .NET and JavaScript targets share the test definitions in `Tests/Module.fs`, `Tests/Extensions.fs`, and `Tests/StringBuilder.fs`. Their single entry point is `Tests/Main.fs`. Add or update shared tests when changing behavior in Fable-specific branches to check parity across targets.

Tests use [Scriptorium](https://fable-hub.github.io/Scriptorium/):

- `Scriptorium.Quill` supplies `testList ("name", [ ... ])`, `test ("name", fun _ -> ...)`, and `Runner.runTests`.
- `Scriptorium.Nib` supplies assertions such as `assertThat result (tag "message" >> isEqualTo expected)`, `assertThat (fun () -> ...) (tag "message" >> throws)`, and `assertThat flag isTrue`.
- Test names must be unique within a list; duplicate paths are rejected.
- There is no CLI `--filter`. To run a subset temporarily, use `ftest` / `ftestList` or `xtest` / `xtestList`. Focused tests fail the CI run, so remove focus markers afterward.
