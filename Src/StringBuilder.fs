namespace Str

open System

/// Extension methods for StringBuilder.
/// Like IndexOf(str), append which returns unit,
/// This module is automatically opened when the namespace Str is opened.
[<AutoOpen>]
module AutoOpenExtensionsStringBuilder =
    type Text.StringBuilder with

        /// Like .Append(string) but returning unit
        member inline sb.Add (s:string) : unit = sb.Append(s) |> ignore

        /// Like .Append(char) but returning unit
        member inline sb.Add (c:char) : unit = sb.Append(c) |> ignore

        /// Like .AppendLine(string) but returning unit
        member inline sb.AddLine (s:string) : unit = sb.AppendLine(s) |> ignore

        /// Like .AppendLine() but returning unit
        member inline sb.AddLine() : unit = sb.AppendLine() |> ignore

        // TODO: add overload with length: sb.IndexOf (c:char, from:int, length:int )

        /// Like .IndexOf for strings, returns -1 if not found
        member sb.IndexOf (c:char, from:int ) : int =
            let rec find i =
                if   i = sb.Length then -1
                elif sb.[i] = c    then i
                else find (i+1)
            find(from)

        /// Like .IndexOf for strings, returns -1 if not found
        /// Always uses StringComparison.Ordinal
        member sb.IndexOf (t:string, from:int) : int =
            // could in theory be improved be using a rolling hash value
            // see also Array.findArray implementation
            // or https://stackoverflow.com/questions/12261344/fastest-search-method-in-stringbuilder
            let ls = sb.Length
            let lt = t.Length
            //printfn "sb :%d t:%d" ls lt
            let rec find ib it = // index in StringBuilder and index in search string
                //printfn "Search at ib:%d %c for it:%d %c" ib sb.[ib] it  t.[it]
                if  ib > ls-lt+it then -1 // not found! not enough chars left in StringBuilder to match remaining search string
                elif sb.[ib] = t.[it]  then
                    if it = lt-1 then ib - lt + 1 // found !
                    else find (ib+1) (it+1)
                else find (ib+1-it) 0
            find from 0

        /// Like .IndexOf for strings, returns -1 if not found
        member inline sb.IndexOf (c:char) : int =
            sb.IndexOf(c,0)

        /// Like .IndexOf for strings, returns -1 if not found
        /// always StringComparison.Ordinal
        member inline sb.IndexOf (t:string) : int =
            sb.IndexOf(t,0)

        member inline sb.Contains (c:char) : bool =
            sb.IndexOf c <> -1

        member inline sb.Contains (s:string) : bool =
            sb.IndexOf s <> -1
