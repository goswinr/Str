namespace Str

open System

/// For formatting floats with adaptive precision.
module NumberFormatting =

    /// Insert thousand separators into a string representing a float or int.
    /// Before and after the decimal point.
    /// Assumes a string that represent a float or int with '.' as decimal separator and no other input formatting.
    let addThousandSeparators (thousandSeparator:char) (number:string) =
        // copied from https://github.com/goswinr/Euclid/blob/main/Src/Format.fs

        let b = Text.StringBuilder(number.Length + number.Length / 3 + 1)
        let inline add (c:char) = b.Append(c) |> ignore

        let inline doBeforeComma st en =
            for i=st to en-1 do // don't go to last one because it shall never get a separator
                let rest = en-i
                add number.[i]
                if rest % 3 = 0 then add thousandSeparator
            add number.[en] //add last (never with sep)

        let inline doAfterComma st en =
            add number.[st] //add first (never with sep)
            for i=st+1 to en do // don't go to last one because it shall never get a separator
                let pos = i-st
                if pos % 3 = 0 then add thousandSeparator
                add number.[i]

        let start =
            if number.[0] = '-' then  add '-'; 1 // add minus if present and move start location
            else 0

        match number.IndexOf('.') with
        | -1 ->
            match number.IndexOf("e") with //, StringComparison.OrdinalIgnoreCase) // not supported by Fable compiler
            | -1 -> doBeforeComma start (number.Length-1)
            | e -> // if float is in scientific notation don't insert comas into it too:
                doBeforeComma start (number.Length-1)
                for ei = e to number.Length-1 do add number.[ei]
        | i ->
            if i>start then
                doBeforeComma start (i-1)
            add '.'
            if i < number.Length then
                match number.IndexOf("e") with //, StringComparison.OrdinalIgnoreCase) with // not supported by Fable compiler
                | -1 -> doAfterComma (i+1) (number.Length-1)
                | e -> // if float is in scientific notation don't insert comas into it too:
                    doAfterComma (i+1) (e-1)
                    for ei = e to number.Length-1 do add number.[ei]

        b.ToString()
