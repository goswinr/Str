![Logo](https://raw.githubusercontent.com/goswinr/Str/main/Doc/logo128.png)
# Str

[![license](https://img.shields.io/github/license/goswinr/Str)](LICENSE)
![code size](https://img.shields.io/github/languages/code-size/goswinr/Str.svg)
[![Str on fuget.org](https://www.fuget.org/packages/Str/badge.svg)](https://www.fuget.org/packages/Str)

Str is an F# extension and module library for `System.String`
It also works with [Fable](https://fable.io/).



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


### Changelog
`0.17.0`
- change Str to static class from module

`0.16.0`
- fix overloads for if StringComparison
- check TS build in Fable

`0.15.0`
- implementation ported from [FsEx](https://github.com/goswinr/FsEx/blob/main/Src/StringModule.fs )
- added more tests