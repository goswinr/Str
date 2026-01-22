# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Str is an F# extension and module library for `System.String` that compiles to both .NET and JavaScript/TypeScript via Fable.

## Build Commands

```bash
# Build the library
dotnet build Src/Str.fsproj

# Build in release mode
dotnet build --configuration Release
```

## Testing

Tests run on both .NET and JavaScript platforms:

```bash
# Run .NET tests (using Expecto)
cd Tests
dotnet run

# Run JavaScript tests (using Fable.Mocha)
cd Tests
npm test

# Run JS tests + verify TypeScript compilation
cd Tests
npm run testTS
```

## Architecture

The library is structured in a specific compilation order (defined in `Src/Str.fsproj`):

1. **StringBuilder.fs** - Extension methods for `System.Text.StringBuilder` (IndexOf, Contains, Add methods)
2. **ComputationalExpression.fs** - The `str` computation expression builder using StringBuilder internally
3. **Extensions.fs** - Extension methods for `System.String` (Get, First, Last, Slice, negative indexing, etc.)
4. **Module.fs** - The main `Str` static class with all string manipulation functions

### Key Design Patterns

- The namespace is `Str`, and opening it provides:
  - A static class `Str` with string functions
  - A computation expression `str` for building strings
  - Auto-opened extension members on `System.String` and `StringBuilder`

- **Fable Compatibility**: Uses `#if FABLE_COMPILER` directives for JavaScript-specific implementations (e.g., the Knuth-Morris-Pratt algorithm in `Str.indicesOf` replaces .NET's `IndexOf` with count parameter)

- **Error Handling**: Custom `StrException` type with descriptive messages including truncated string values via `Format.truncated`

### Dual Test Framework

The test project (`Tests/`) uses conditional compilation:
- `#if FABLE_COMPILER` → Fable.Mocha for JS
- `#else` → Expecto for .NET

Both frameworks share the same test definitions in `Tests/Module.fs` and `Tests/Extensions.fs`.
