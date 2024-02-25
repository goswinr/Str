

# Str

![code size](https://img.shields.io/github/languages/code-size/goswinr/Str.svg) 
[![license](https://img.shields.io/github/license/goswinr/Str)](LICENSE)

Str is an F# extension and module library for `Str<'T>` ( = `Collection.Generic.List<'T>`)

It also works with [Fable](https://fable.io/).


![Logo](https://raw.githubusercontent.com/goswinr/Str/main/Doc/logo.png)

### It Includes: 

- A `Str` module that has a corseponding functions for  **all**  functions in the  [`Array` module from `FSharp.Core`](https://fsharp.github.io/fsharp-core-docs/reference/fsharp-collections-arraymodule.html). Including those for parallel computing.
- A  Computational Expressions `Str` that can be used like existing ones for `seq`.
- Support for F# slicing operator and indexing from the end. e.g: `items.[ 1 .. ^1]`
- Extension members on `Str` like `.Get` `.Set` `.First` `.Last` `.SecondLast` and more.
With nicer IndexOutOfRangeExceptions that include the bad index and the actual size.

- **All** Tests from the from `FSharp.Core`'s `Array` module ported and adapted to run in both javascript and dotnet.

### Usage
Just open the namespace

```fsharp
open Str
```
this namespace contains:
- a module also called `Str`
- a  Computational Expressions called `Str`
- this will also auto open the extension members on `Str<'T>`

then you can do:

```fsharp
let evenNumbers = 
    Str {  // a Computational Expressions like seq
        for i = 0 t 99 do 
            if i % 2 = 0 then 
                i
    }
    
let oddNumbers = evenNumbers |> Str.map (fun x -> x + 1) // Str module

let hundred = oddNumbers.Last // Extension member to access the last item in list 

```

### Why ?
Yes, F# Arrays and (linked) Lists can do these kind of operations on collections too.
But Str (being mutable)  still offers the best performance for collections that expand or shrink and need random access.

In fact FSharp.Core uses [a very similar module internally](https://github.com/dotnet/fsharp/blob/main/src/Compiler/Utilities/Str.fs)


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
`0.16.0`
- add null checks
- add 'partitionBy' functions
- add equality checks for nested Strs
- flip arg order of 'sub' function

`0.15.0`
- implementation ported from `Rarr` type in https://github.com/goswinr/Str/blob/main/Src/RarrModule.fs 