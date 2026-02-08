![Logo](https://raw.githubusercontent.com/goswinr/Str/main/Docs/img/logo128.png)

# Str

[![Str on nuget.org](https://img.shields.io/nuget/v/Str)](https://www.nuget.org/packages/Str/)
[![Build Status](https://github.com/goswinr/Str/actions/workflows/build.yml/badge.svg)](https://github.com/goswinr/Str/actions/workflows/build.yml)
[![Docs Build Status](https://github.com/goswinr/Str/actions/workflows/docs.yml/badge.svg)](https://github.com/goswinr/Str/actions/workflows/docs.yml)
[![Test Status](https://github.com/goswinr/Str/actions/workflows/test.yml/badge.svg)](https://github.com/goswinr/Str/actions/workflows/test.yml)
[![license](https://img.shields.io/github/license/goswinr/Str)](LICENSE.md)
![code size](https://img.shields.io/github/languages/code-size/goswinr/Str.svg)

Str is an F# extension and module library for `System.String`
It compiles to Javascript and Typescript with [Fable](https://fable.io/).

## It Includes

- A `Str` module that has all methods from the String type as functions, and more. Adapted and extended from [FSharpX](https://github.com/fsprojects/FSharpx.Extras/blob/master/src/FSharpx.Extras/String.fs)
- A  Computational Expressions `str` that can be used build up strings ( using a StringBuilder internally).
- Extension members on `System.String` like `.Get` `.First` `.Last` `.SecondLast` and more.
With nicer IndexOutOfRangeExceptions that include the bad index and the actual size.

- Extensive Tests running on both .NET and JS


### Full API Documentation

[goswinr.github.io/Str](https://goswinr.github.io/Str/reference/str.html)

### Use of AI and LLMs in the project
All core function are are written by hand to ensure performance and correctness.<br>
However, AI tools have been used for code review, typo and grammar checking in documentation<br>
and to generate not all but many of the tests.


### Usage
Just open the module

```fsharp
open Str
```

this module contains:
- a static class also called `Str`
- a Computational Expressions called `str`
- this will also auto open the extension members on `System.String`

### The `str` Computation Expression

Build strings using a StringBuilder internally, with support for `string`, `char`, `int`, loops, and sequences:

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

Use `yield!` to append with a trailing newline:

```fsharp
let lines = // "line one\nline two\nline three\n"
    str {
        yield! "line one"
        yield! "line two"
        yield! "line three"
    }
```

You can also yield a sequence of strings (each gets a newline):

```fsharp
let fromSeq = // "a\nb\nc\n"
    str {
        ["a"; "b"; "c"]
    }
```

### Before / After / Between

Extract parts of a string relative to a delimiter. Each operation comes in three variants:
- **throws** if not found (e.g. `before`)
- **try** returns `Option` (e.g. `tryBefore`)
- **orInput** returns the full input string if not found (e.g. `beforeOrInput`)

```fsharp
Str.before  "/"  "hello/world"         // "hello"
Str.after   "/"  "hello/world"         // "world"
Str.between "("  ")" "say (hi) now"    // "hi"

Str.tryBefore "?" "no-question"        // None
Str.tryAfter  "/" "hello/world"        // Some "world"

Str.beforeOrInput "?" "no-question"    // "no-question"
```

Character versions are available too:

```fsharp
Str.beforeChar '/' "hello/world"       // "hello"
Str.afterChar  '/' "hello/world"       // "world"
Str.betweenChars '(' ')' "say (hi) now" // "hi"
```

### Splitting

```fsharp
Str.splitOnce ":"  "key:value"                // ("key", "value")
Str.splitTwice "(" ")" "before(inside)after"  // ("before", "inside", "after")

// Option variants for safe splitting
Str.trySplitOnce ":" "no-colon"               // None

// Split into array (removes empty entries by default)
Str.split "," "a,,b,c"                        // [|"a"; "b"; "c"|]
Str.splitKeep "," "a,,b,c"                    // [|"a"; ""; "b"; "c"|]

// Split by characters
Str.splitChar ',' "a,b,c"                     // [|"a"; "b"; "c"|]
Str.splitChars [|',';';'|] "a,b;c"            // [|"a"; "b"; "c"|]

// Split by line endings (\r\n, \r, \n)
Str.splitLines "line1\nline2\r\nline3"        // [|"line1"; "line2"; "line3"|]
```

### Slicing with Negative Indices

Negative indices count from the end (`-1` is the last character). The end index is **inclusive**.

```fsharp
Str.slice  0   4  "Hello, World!"    // "Hello"
Str.slice  7  11  "Hello, World!"    // "World"
Str.slice  0  -1  "Hello, World!"    // "Hello, World!"
Str.slice -6  -2  "Hello, World!"    // "orld"
```

### Truncate, Skip, and Take

```fsharp
Str.truncate 5 "Hello, World!"    // "Hello"  (safe, returns input if shorter)
Str.take     5 "Hello, World!"    // "Hello"  (fails if input is shorter)
Str.skip     7 "Hello, World!"    // "World!"
```

### Replace Variants

```fsharp
Str.replace      "o" "0" "foo boo"  // "f00 b00"  (all occurrences)
Str.replaceFirst "o" "0" "foo boo"  // "f0o boo"  (first only)
Str.replaceLast  "o" "0" "foo boo"  // "foo bo0"  (last only)
Str.replaceChar  'o' '0' "foo boo"  // "f00 b00"  (all char occurrences)
```

### Delete

```fsharp
Str.delete     "World" "Hello World"  // "Hello "
Str.deleteChar '!'     "Hi!!!"        // "Hi"
```

### Case Functions

```fsharp
Str.up1    "hello"  // "Hello"  (capitalize first letter)
Str.low1   "Hello"  // "hello"  (lowercase first letter)
Str.toUpper "hi"    // "HI"
Str.toLower "HI"    // "hi"
```

### Contains and Comparison

```fsharp
Str.contains           "world" "hello world"  // true
Str.containsIgnoreCase "WORLD" "hello world"  // true
Str.notContains        "xyz"   "hello world"  // true

Str.startsWith  "hello" "hello world"   // true
Str.endsWith    "world" "hello world"   // true
Str.equals      "abc"   "abc"           // true  (ordinal)
Str.equalsIgnoreCase "ABC" "abc"        // true
```

### Counting

```fsharp
Str.countSubString "ab" "ababab"  // 3
Str.countChar      'a'  "banana"  // 3
```

### Whitespace and Emptiness Checks

```fsharp
Str.isWhite    "  \t "   // true
Str.isNotWhite "hello"   // true
Str.isEmpty    ""         // true
Str.isNotEmpty "hello"   // true
```

### Padding, Quoting, and Affixes

```fsharp
Str.padLeft      10 "hi"         // "        hi"
Str.padRightWith 10 '.' "hi"    // "hi........"
Str.addPrefix "pre-"  "fix"     // "pre-fix"
Str.addSuffix "-end"  "start"   // "start-end"
Str.inQuotes       "hi"         // "\"hi\""
Str.inSingleQuotes "hi"         // "'hi'"
```

### Number Formatting

```fsharp
Str.addThousandSeparators '\'' "1234567"       // "1'234'567"
Str.addThousandSeparators ','  "1234567.1234"  // "1,234,567.123,4"
```

### Normalize (Remove Diacritics)

```fsharp
Str.normalize "cafe\u0301"  // "cafe"  (removes combining accent)
Str.normalize "Zurich"      // "Zurich"
```

### Display Formatting

```fsharp
Str.formatInOneLine "hello\n  world"            // "hello world"
Str.formatTruncated 10 "a long string here"     // "\"a lon(..)\"" (truncated with placeholder)
Str.formatTruncatedToMaxLines 2 "a\nb\nc\nd"    // shows first 2 lines + note
```

### Joining

```fsharp
Str.concat ", " ["a"; "b"; "c"]   // "a, b, c"
Str.concatLines  ["a"; "b"; "c"]  // "a\nb\nc" (joined with Environment.NewLine)
```

### Extension Members (auto-opened)

These are available on any `string` as soon as you `open Str`:

```fsharp
let s = "Hello, World!"

s.Contains('W')              // true  (char overload)
s.DoesNotContain("xyz")     // true
s.IsWhite                   // false
s.IsNotEmpty                // true
```

### Extension Members (from ExtensionsString module)

For richer indexing and slicing, also open the `ExtensionsString` module:

```fsharp
open Str.ExtensionsString

let s = "Hello"
s.First        // 'H'
s.Last         // 'o'
s.Second       // 'e'
s.SecondLast   // 'l'
s.ThirdLast    // 'l'
s.LastX 3      // "llo"
s.LastIndex    // 4

s.Get 0        // 'H'  (with descriptive errors on out-of-range)
s.GetNeg(-1)   // 'o'  (negative index, -1 = last)
s.GetLooped 7  // 'e'  (wraps around: 7 % 5 = 2)

s.Slice(0, 2)    // "Hel"  (inclusive end index)
s.Slice(-3, -1)  // "llo"

s.ReplaceFirst("l", "L")  // "HeLlo"  (only first match)
s.ReplaceLast ("l", "L")  // "HelLo"  (only last match)
```

### StringBuilder Extensions (auto-opened)

`open Str` also adds convenience methods to `System.Text.StringBuilder`:

```fsharp
open System.Text

let sb = StringBuilder()
sb.Add "hello"       // Append returning unit (instead of StringBuilder)
sb.Add ','           // Append char returning unit
sb.AddLine " world"  // AppendLine returning unit
sb.Contains "hello"  // true
sb.IndexOf ","       // 5
```

### Tests
All Tests run in both javascript and dotnet.
Successful Fable compilation to typescript is verified too.
Go to the tests folder:

```bash
cd Tests
```

For testing with .NET using Expecto:

```bash
dotnet run
```

for JS testing with Fable.Mocha and TS verification:

```bash
npm test
```

### License
[MIT](https://github.com/goswinr/Str/blob/main/LICENSE.md)

### Changelog
see [CHANGELOG.md](https://github.com/goswinr/Str/blob/main/CHANGELOG.md)
