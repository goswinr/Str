# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Str is an F# extension and module library for `System.String` that compiles to both .NET and JavaScript/TypeScript via Fable.

## Build Commands

```bash
# Build the library
dotnet build Src/Str.fsproj

# Build in release mode
dotnet build Src/Str.fsproj --configuration Release

# Build entire solution
dotnet build Str.sln
```

## Testing

Tests must be run from the Tests directory. Both .NET and JavaScript tests share the same test definitions.

```bash
# Run .NET tests (using Scriptorium)
cd Tests && dotnet run

# Run JavaScript tests (Fable compiles to Tests/js, then `--runScript` runs it with Node.js)
cd Tests && npm test

# Run JS tests + verify TypeScript compilation
cd Tests && npm run testTS

# Watch mode for TypeScript development
cd Tests && npm run watchTS
```

## Architecture

The library is structured in a specific compilation order (defined in `Src/Str.fsproj`):

1. **StringBuilder.fs** - Extension methods for `System.Text.StringBuilder` (IndexOf, Contains, Add methods)
2. **ComputationalExpression.fs** - The `str` computation expression builder using StringBuilder internally
3. **Extensions.fs** - Contains:
   - `Format` internal module for truncating strings in error messages
   - `AutoOpenExtensionsString` module with basic string extensions (DoesNotContain, IsWhite, etc.)
   - `ExtensionsString` module with `StrException` type and advanced extensions (Get, First, Last, Slice, negative indexing)
4. **Module.fs** - The main `Str` static class with all string manipulation functions

### Key Design Patterns

- The namespace is `Str`, and opening it provides:
  - A static class `Str` with string functions
  - A computation expression `str` for building strings
  - Auto-opened extension members on `System.String` and `StringBuilder`

- **Fable Compatibility**: Uses `#if FABLE_COMPILER` directives for JavaScript-specific implementations (e.g., the Knuth-Morris-Pratt algorithm in `Str.indicesOf` replaces .NET's `IndexOf` with count parameter, `Str.normalize` uses JS-specific diacritic removal)

- **Error Handling**: Custom `StrException` type with descriptive messages including truncated string values via `Format.truncated`

### Test Framework

The test project (`Tests/`) uses [Scriptorium](https://fable-hub.github.io/Scriptorium/) on both .NET and JS, with no conditional compilation for the framework:
- `Scriptorium.Quill` provides the test DSL (`open type Scriptorium.Quill.Test`: `testList ("name", [ ... ])`, `test ("name", fun _ -> ...)`) and the runner (`Runner.runTests` in `Tests/Main.fs`).
- `Scriptorium.Nib` provides the assertions (`open Scriptorium.Nib.Assertion`), e.g. `assertThat result (tag "message" >> isEqualTo expected)`, `assertThat (fun () -> ...) (tag "message" >> throws)`, `assertThat flag isTrue`.
- There is no CLI `--filter`; to run a subset temporarily use `ftest` / `ftestList` (focus) or `xtest` / `xtestList` (pending). Focused tests fail the run on CI.
- Test names must be unique within a list; the runner rejects duplicate test paths.

The same test definitions in `Tests/Module.fs`, `Tests/Extensions.fs` and `Tests/StringBuilder.fs` run on both .NET and JS.
