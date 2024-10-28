namespace Str

open System
open System.Text

[<Obsolete("not obsolete but hidden, needs to be visible for inlining")>]
module ComputationalExpressions =

    //TODO: optimize with
    // [<InlineIfLambda>] as in https://gist.github.com/Tarmil/afcf5f50e45e90200eb7b01615b0ffc0
    // or https://github.com/fsharp/fslang-design/blob/main/FSharp-6.0/FS-1099-list-collector.md

    let inline private addChr   (b: StringBuilder) (c:char)   = b.Append      c   |> ignore<StringBuilder>
    let inline private addLnChr (b: StringBuilder) (c:char)   = b.Append(c).AppendLine()   |> ignore<StringBuilder>
    let inline private add      (b: StringBuilder) (s:string) = b.Append      s   |> ignore<StringBuilder>
    let inline private addLn    (b: StringBuilder) (s:string) = b.AppendLine  s   |> ignore<StringBuilder>

    /// Computational Expression String Builder:
    /// use 'yield' to append text, or seq of strings separated by a new line
    /// and 'yield!' (with an exclamation mark)  to append text followed by a new line character.
    /// accepts ints, guids and chars too.
    type StringBufferBuilder () =
        // adapted from https://github.com/fsharp/fslang-suggestions/issues/775

        member inline _.Yield (c: char) =      fun (b: StringBuilder) ->  addChr b c
        member inline _.Yield (txt: string) =  fun (b: StringBuilder) ->  add b txt
        member inline _.Yield (i: int) =       fun (b: StringBuilder)  -> add b (i.ToString())
        member inline _.Yield (g: Guid) =      fun (b: StringBuilder)  -> add b (g.ToString())

        member inline _.YieldFrom (txt: string) =  fun (b: StringBuilder) -> addLn b txt
        member inline _.YieldFrom (c: char) =      fun (b: StringBuilder) -> addLnChr b c
        member inline _.YieldFrom (i: int) =       fun (b: StringBuilder) -> addLn b (i.ToString())
        member inline _.YieldFrom (g: Guid) =      fun (b: StringBuilder) -> addLn b (g.ToString())

        member inline _.Yield (strings: seq<string>) =
            fun (b: StringBuilder) -> for s in strings do  addLn b s

        member inline _.Combine (f, g) = fun (b: StringBuilder) -> f b; g b

        member inline _.Delay f = fun (b: StringBuilder) -> (f()) b

        member inline _.Zero () = ignore

        member inline _.For (xs: 'T seq, f: 'T -> StringBuilder -> unit) =
            fun (b: StringBuilder) ->
                use e = xs.GetEnumerator ()
                while e.MoveNext() do (f e.Current) b

        member inline _.While (p: unit -> bool, f: StringBuilder -> unit) =
            fun (b: StringBuilder) ->
                while p () do f b

        member inline _.Run (f: StringBuilder -> unit) =
            let b = StringBuilder()
            do f b
            b.ToString()

        member inline  _.TryWith(body: StringBuilder -> unit, handler: exn ->  StringBuilder -> unit) =
            fun (b: StringBuilder) ->
                try body b with e -> handler e b

        member inline  _.TryFinally(body: StringBuilder -> unit, compensation:  StringBuilder -> unit) =
            fun (b: StringBuilder) ->
                try body b finally compensation  b

        member inline this.Using(disposable: #IDisposable, body: #IDisposable -> StringBuilder -> unit) =
            this.TryFinally(  body disposable ,  fun (_: StringBuilder)  ->
                if not <| Object.ReferenceEquals(disposable,null) then // might be disposed already
                    disposable.Dispose() )


#nowarn "44" //for opening the hidden but not Obsolete module
/// <summary>
/// This module is automatically opened when the namespace Str is opened.
/// It provides a computational expression for building strings called <c>str</c>.
/// </summary>
[<AutoOpen>]
module AutoOpenComputationalExpression  =

    /// Computational Expression String Builder:
    /// use 'yield' to append text, or seq of strings separated by a new line
    /// and 'yield!' (with an exclamation mark)  to append text followed by a new line character.
    /// accepts ints, guids and chars  too.
    let str = ComputationalExpressions.StringBufferBuilder ()





