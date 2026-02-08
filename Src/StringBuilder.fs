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

        /// Like list.IndexOf but for StringBuilder, returns -1 if not found
        /// Throws ArgumentOutOfRangeException if 'from' is negative.
        member sb.IndexOf (c:char, from:int ) : int =
            if from < 0 then
                ArgumentOutOfRangeException("from",$"StringBuilder.IndexOf: from ({from}) must be non-negative") |> raise
            let len = sb.Length
            // if from >= len then
            //     ArgumentOutOfRangeException("from",$"StringBuilder.IndexOf: from ({from}) must be less than StringBuilder length ({len})") |> raise
            let rec find i =
                if i = len then
                    -1
                elif sb.[i] = c then
                    i
                else
                    find (i+1)
            find(from)

        /// Like list.IndexOf but for StringBuilder, returns -1 if not found
        /// Always uses StringComparison.Ordinal
        /// Throws ArgumentOutOfRangeException if 'from' is negative.
        member sb.IndexOf (t:string, from:int) : int =
            // could in theory be improved be using a rolling hash value
            // see also Array.findArray implementation
            // or https://stackoverflow.com/questions/12261344/fastest-search-method-in-stringbuilder
            let lenBuilder = sb.Length
            let lenText = t.Length
            if from < 0 then
                ArgumentOutOfRangeException("from",$"StringBuilder.IndexOf: from ({from}) must be non-negative") |> raise

            // if from >= lenBuilder then
            //     ArgumentOutOfRangeException("from",$"StringBuilder.IndexOf: from ({from}) must be less than StringBuilder length ({lenBuilder})") |> raise

            let rec find idxBuilder idxText = // index in StringBuilder and index in search string
                if idxBuilder > lenBuilder-lenText+idxText then
                    -1 // not found! not enough chars left in StringBuilder to match remaining search string
                elif sb.[idxBuilder] = t.[idxText]  then
                    if idxText = lenText-1 then
                        idxBuilder - lenText + 1 // found !
                    else
                        find (idxBuilder+1) (idxText+1)
                else
                    find (idxBuilder+1-idxText) 0
            find from 0

        /// Like list.IndexOf but for StringBuilder, returns -1 if not found
        member inline sb.IndexOf (c:char) : int =
            sb.IndexOf(c,0)

        /// Like list.IndexOf but for StringBuilder, returns -1 if not found
        /// always StringComparison.Ordinal
        member inline sb.IndexOf (t:string) : int =
            sb.IndexOf(t,0)

        /// Checks if StringBuilder contains the given character
        member inline sb.Contains (c:char) : bool =
            sb.IndexOf c <> -1

        /// Checks if StringBuilder contains the given string
        member inline sb.Contains (s:string) : bool =
            sb.IndexOf s <> -1
