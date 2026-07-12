namespace Str

open System

/// Extension methods for StringBuilder.
/// Provides IndexOf methods and append operations that return unit.
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

        /// Like String.IndexOf but for StringBuilder; returns -1 if not found.
        /// Throws ArgumentOutOfRangeException if 'from' is negative or greater than the StringBuilder length.
        member sb.IndexOf (c:char, from:int ) : int =
            let len = sb.Length
            if from < 0 || from > len then
                ArgumentOutOfRangeException("from",$"StringBuilder.IndexOf: from ({from}) must be between 0 and StringBuilder length ({len})") |> raise
            let rec find i =
                if i = len then
                    -1
                elif sb.[i] = c then
                    i
                else
                    find (i+1)
            find(from)

        /// Like String.IndexOf but for StringBuilder; returns -1 if not found.
        /// Always uses StringComparison.Ordinal.
        /// An empty search string is found at 'from'.
        /// Throws ArgumentNullException if the search string is null.
        /// Throws ArgumentOutOfRangeException if 'from' is negative or greater than the StringBuilder length.
        member sb.IndexOf (t:string, from:int) : int =
            // could in theory be improved be using a rolling hash value
            // see also Array.findArray implementation
            // or https://stackoverflow.com/questions/12261344/fastest-search-method-in-stringbuilder
            let lenBuilder = sb.Length
            if isNull t then
                ArgumentNullException("t", "StringBuilder.IndexOf: search string cannot be null") |> raise
            let lenText = t.Length
            if from < 0 || from > lenBuilder then
                ArgumentOutOfRangeException("from",$"StringBuilder.IndexOf: from ({from}) must be between 0 and StringBuilder length ({lenBuilder})") |> raise

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
            if lenText = 0 then from
            else find from 0

        /// Like String.IndexOf but for StringBuilder; returns -1 if not found.
        member inline sb.IndexOf (c:char) : int =
            sb.IndexOf(c,0)

        /// Like String.IndexOf but for StringBuilder; returns -1 if not found.
        /// Always uses StringComparison.Ordinal.
        member inline sb.IndexOf (t:string) : int =
            sb.IndexOf(t,0)

        /// Checks if StringBuilder contains the given character
        member inline sb.Contains (c:char) : bool =
            sb.IndexOf c <> -1

        /// Checks if StringBuilder contains the given string
        member inline sb.Contains (s:string) : bool =
            sb.IndexOf s <> -1
