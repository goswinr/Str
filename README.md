# Str

[![Str on nuget.org](https://img.shields.io/nuget/v/Str)](https://www.nuget.org/packages/Str/)
[![Str on fuget.org](https://www.fuget.org/packages/Str/badge.svg)](https://www.fuget.org/packages/Str)
![code size](https://img.shields.io/github/languages/code-size/goswinr/Str.svg)
[![license](https://img.shields.io/github/license/goswinr/Str)](LICENSE)

Str is an F# extension and module library for `System.String`
It also works with [Fable](https://fable.io/).


![Logo](https://raw.githubusercontent.com/goswinr/Str/main/Doc/logo.png)

### It Includes:

- A `Str` module that has all methods from the String type as functions, and more. Adapted and extended from [FSharpX](https://github.com/fsprojects/FSharpx.Extras/blob/master/src/FSharpx.Extras/String.fs)
- A  Computational Expressions `str` that can be used build up strings ( using a StringBuilder internally).
- Extension members on `Str` like `.Get` `.First` `.Last` `.SecondLast` and more.
With nicer IndexOutOfRangeExceptions that include the bad index and the actual size.

- Extensive Tests

### Usage
Just open the namespace

```fsharp
open Str
```
this namespace contains:
- a module also called `Str`
- a  Computational Expressions called `str`
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

### License
[MIT](https://raw.githubusercontent.com/goswinr/Str/main/LICENSE.txt)

### Test
All Tests from the from `FSharp.Core`'s `Array` module ported and adapted to run in both javascript and dotnet.
go to the tests folder

```bash
cd Tests
```

For testing with .NET using Expecto run

```bash
dotnet run
```

for testing with Fable.Mocha run

```bash
npm test
```


### Changelog

`0.15.0`
- implementation ported from [FsEx](https://github.com/goswinr/FsEx/blob/main/Src/StringModule.fs )