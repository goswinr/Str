![Logo](https://raw.githubusercontent.com/goswinr/Str/main/Docs/img/logo128.png)
# Str

[![Str on nuget.org](https://img.shields.io/nuget/v/Str)](https://www.nuget.org/packages/Str/)
[![Build Status](https://github.com/goswinr/Str/actions/workflows/build.yml/badge.svg)](https://github.com/goswinr/Str/actions/workflows/build.yml)
[![Docs Build Status](https://github.com/goswinr/Str/actions/workflows/docs.yml/badge.svg)](https://github.com/goswinr/Str/actions/workflows/docs.yml)
[![Test Status](https://github.com/goswinr/Str/actions/workflows/test.yml/badge.svg)](https://github.com/goswinr/Str/actions/workflows/test.yml)
[![license](https://img.shields.io/github/license/goswinr/Str)](LICENSE.md)
![code size](https://img.shields.io/github/languages/code-size/goswinr/Str.svg)

Str is an F# extension and module library for `System.String`
It also works with [Fable](https://fable.io/).

### It Includes:

- A `Str` module that has all methods from the String type as functions, and more. Adapted and extended from [FSharpX](https://github.com/fsprojects/FSharpx.Extras/blob/master/src/FSharpx.Extras/String.fs)
- A  Computational Expressions `str` that can be used build up strings ( using a StringBuilder internally).
- Extension members on `Str` like `.Get` `.First` `.Last` `.SecondLast` and more.
With nicer IndexOutOfRangeExceptions that include the bad index and the actual size.

- Extensive Tests running on both .NET and JS

### Usage
Just open the module

```fsharp
open Str
```

this module contains:
- a static class also called `Str`
- a Computational Expressions called `str`
- this will also auto open the extension members on `System.String`

then you can do:

```fsharp
let hello = // "Hello, World !!!"
    str {
        "Hello"
        ','
        " World "
        for i in 1..3 do
            "!"
    }
```

### Full API Documentation

[https://goswinr.github.io/Str](https://goswinr.github.io/Str/reference/str.html)


### Test
All Tests run in both javascript and dotnet.
go to the tests folder

```bash
cd Tests
```

For testing with .NET using Expecto:

```bash
dotnet run
```

for testing with Fable.Mocha:

```bash
npm test
```

### License
[MIT](https://github.com/goswinr/Str/blob/main/LICENSE.md)

### Changelog
see [CHANGELOG.md](https://github.com/goswinr/Str/blob/main/CHANGELOG.md)