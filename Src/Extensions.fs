namespace Str
open System



/// This module provides utilities for trimming strings.
/// mostly for internal error messages
[<RequireQualifiedAccess>]
module internal Format =
    open System.Text

    /// Joins string into one line.
    /// Replaces line break with a space character.
    /// Skips leading whitespace on each line.
    /// Collapses consecutive whitespace into a single space.
    /// If string is null returns *null string*
    /// Does not include surrounding quotes.
    let inOneLine (s:string) : string =
        if isNull s then
            "*null string*"
        else
            let sb = StringBuilder(s.Length)
            let rec loop addNextWhite i =
                if i<s.Length then
                    let c = s.[i]
                    if Char.IsWhiteSpace c then
                        if addNextWhite then sb.Append(' ') |> ignore<StringBuilder> // to have at least on space separating new lines
                        loop false (i+1)
                    else
                        sb.Append(c) |> ignore<StringBuilder>
                        loop true (i+1)
            loop false 0
            // TODO delete trailing space if there is one??
            sb.ToString()

    /// Formats a string for display using at most maxCharCount content characters, with a minimum of 8.
    /// Surrounding quotation marks add two characters to the returned string.
    /// Depending on maxCharCount, skipped characters are represented by (..), (...), or a message such as ( ... and 123 more chars.).
    /// If input is null, returns *null* when maxCharCount is below 15 and *null string* otherwise.
    let truncated (maxCharCount:int) (s:string) : string =
        let maxChar = max 8 maxCharCount
        if isNull s then
            if maxChar >= 15 then
                "*null string*"
            else
                "*null*"
        elif s.Length <= maxChar  then
            str{ "\"" ; s ; "\"" }
        else
            let len = s.Length
            if   maxChar <= 10 then str{ "\"" ;  s.Substring(0, maxChar-2-2) ; "(..)"                           ; "\"" }
            elif maxChar <= 20 then str{ "\"" ;  s.Substring(0, maxChar-3-2) ; "(..)"  ; s.Substring(len-1, 1)  ; "\"" }
            elif maxChar <= 35 then str{ "\"" ;  s.Substring(0, maxChar-5-2) ; "(...)" ; s.Substring(len-2, 2)  ; "\"" }
            else
                let suffixLen = 1 + maxChar / 20 // using 5% for end of string
                let counterLen = "[< ..99 more chars.. >]".Length
                str{
                    "\""
                    s.Substring(0, maxChar-counterLen-suffixLen)
                    "( ... and "; len - maxChar+counterLen  ; " more chars.)"
                    s.Substring(len-suffixLen, suffixLen)
                    "\""
                    }


    /// Limits a string to maxLineCount logical lines and adds a note such as (... and 3 more lines.) when truncated.
    /// Recognizes CR, LF, and CRLF line endings. Truncated results are enclosed in quotation marks; unchanged results are not.
    /// maxLineCount is treated as at least 1. If the string is null, returns *null string*.
    let truncatedToMaxLines (maxLineCount:int) (s:string) : string =
        let maxLines = max 1 maxLineCount
        if isNull s then
            "*null string*"
        else
            let lineBreakStarts = ResizeArray<int>()
            let mutable i = 0
            while i < s.Length do
                if s.[i] = '\r' then
                    lineBreakStarts.Add i
                    if i+1 < s.Length && s.[i+1] = '\n' then
                        i <- i + 2
                    else
                        i <- i + 1
                elif s.[i] = '\n' then
                    lineBreakStarts.Add i
                    i <- i + 1
                else
                    i <- i + 1

            let lineCount = lineBreakStarts.Count + 1
            if lineCount > maxLines then
                let stopPos = lineBreakStarts.[maxLines-1]
                let trimmedLineCount = lineCount - maxLines
                let lineWord = if trimmedLineCount = 1 then " line.)\"" else " lines.)\""
                str{
                    "\""
                    s.Substring(0,stopPos)
                    "(... and "
                    trimmedLineCount
                    " more"
                    lineWord
                    }
            else
                s



/// Extension methods for System.String.
/// Like DoesNotContain(str),..,
/// This module is automatically opened when the namespace Str is opened.
[<AutoOpen>]
module AutoOpenExtensionsString =

    // This type extension should be always available that is why it is in this Auto-open module
    type System.String with

        /// s.IndexOf(subString,StringComparison.Ordinal) = -1
        member inline s.DoesNotContain(subString:string) : bool =
            s.IndexOf(subString, StringComparison.Ordinal) = -1 // in Fable the StringComparison arg is ignored. TODO Fable should issue a warning for that !

        /// s.IndexOf(chr) = -1
        member inline s.DoesNotContain(chr:char) : bool =
            s.IndexOf(chr) = -1

        /// s.IndexOf(char) <> -1
        member inline s.Contains(chr:char) : bool =  // this overload does not exist by default
            s.IndexOf(chr) <> -1

        /// Splits a string into substrings.
        /// Empty entries are included.
        /// s.Split( [|chr|] )
        member inline s.Split(chr:char) : string[] =  // this overload does not exist by default
            s.Split([|chr|])


        /// Calls not(String.IsNullOrWhiteSpace(str))
        member inline s.IsNotWhite : bool =
            not(String.IsNullOrWhiteSpace s )

        /// Calls String.IsNullOrWhiteSpace(str)
        member inline s.IsWhite : bool =
            String.IsNullOrWhiteSpace s

        /// Calls not(String.IsNullOrEmpty(str))
        member inline s.IsNotEmpty : bool =
            not(String.IsNullOrEmpty s )

        /// Calls String.IsNullOrEmpty(str)
        member inline s.IsEmpty : bool =
            String.IsNullOrEmpty s

/// Extension methods for System.String.
/// Adds extension members on System.String.
/// E.G. .First, .Second, .Last and similar indices.
/// Also adds functionality for negative indices, and s.SliceNeg, s.SliceIdx and s.SliceLooped with an inclusive end index.
/// This module is NOT automatically opened when the namespace Str is opened.
module ExtensionsString =

    /// For string formatting in exceptions. Including surrounding quotes
    let exnf s : string = s |> Format.truncated 100

    /// An Exception for the string functions defined in Str
    type StrException(txt:string)=
        inherit Exception(txt)
        /// Raise the exception with F# printf string formatting
        static member Raise msg =
            Printf.kprintf (fun s -> raise (new StrException(s))) msg

    type System.String with

        /// Gets a character at an index, the same as this.[index] or this.Idx(index).
        /// Throws a descriptive Exception if the index is out of range.
        /// (Use this.GetNeg(i) member if you want to use negative indices too)
        member inline str.Get index : char =
            if index < 0 then StrException.Raise $"Str.ExtensionsString: str.Get({index}) failed for string of {str.Length} chars, use str.GetNeg method if you want negative indices too:{Environment.NewLine}{exnf str}"
            if index >= str.Length then StrException.Raise $"Str.ExtensionsString: str.Get({index}) failed for string of {str.Length} chars:{Environment.NewLine}{exnf str}"
            str.[index]

        /// Gets a character at an index, the same as this.[index] or this.Get(index).
        /// Throws a descriptive Exception if the index is out of range.
        /// (Use this.GetNeg(i) member if you want to use negative indices too)
        member inline str.Idx index : char =
            if index < 0 then StrException.Raise $"Str.ExtensionsString: str.Idx({index}) failed for string of {str.Length} chars, use str.GetNeg method if you want negative indices too:{Environment.NewLine}{exnf str}"
            if index >= str.Length then StrException.Raise $"Str.ExtensionsString: str.Idx({index}) failed for string of {str.Length} chars:{Environment.NewLine}{exnf str}"
            str.[index]


        /// Returns the last valid index in the string
        /// same as: s.Length - 1
        member inline str.LastIndex : int =
            str.Length - 1

        /// Returns the last character of the string
        /// Fails if the string is empty.
        member inline str.Last : char =
            if str.Length = 0 then StrException.Raise "Str.ExtensionsString: str.Last: Failed to get last character of empty String"
            str.[str.Length - 1]

        /// Returns the second last character of the string
        /// Fails if the string has fewer than two characters.
        member inline str.SecondLast : char =
            if str.Length < 2 then StrException.Raise "Str.ExtensionsString: str.SecondLast: Failed to get second last character of '%s'" (exnf str)
            str.[str.Length - 2]

        /// Returns the third last character of the string
        /// Fails if the string has fewer than three characters.
        member inline str.ThirdLast : char =
            if str.Length < 3 then StrException.Raise "Str.ExtensionsString: str.ThirdLast: Failed to get third last character of '%s'" (exnf str)
            str.[str.Length - 3]

        /// Returns the last x characters of the string.
        /// x must be between zero and the string length.
        member inline str.LastX x : string =
            if x < 0 then StrException.Raise "Str.ExtensionsString: str.LastX: x can't be negative: %d" x
            if str.Length < x then StrException.Raise "Str.ExtensionsString: str.LastX: Failed to get last %d character of too short String '%s' " x (exnf str)
            str.Substring(str.Length-x,x)

        /// Returns the first character of the string
        /// Fails if the string is empty.
        member inline str.First : char =
            if str.Length = 0 then StrException.Raise "Str.ExtensionsString: str.First: Failed to get first character of empty String"
            str.[0]

        /// Returns the second character of the string
        /// Fails if the string has fewer than two characters.
        member inline str.Second : char =
            if str.Length < 2 then StrException.Raise "Str.ExtensionsString: str.Second: Failed to get second character of '%s'" (exnf str)
            str.[1]

        /// Returns the third character of the string
        /// Fails if the string has fewer than three characters.
        member inline str.Third : char =
            if str.Length < 3 then StrException.Raise "Str.ExtensionsString: str.Third: Failed to get third character of '%s'" (exnf str)
            str.[2]


        /// Gets an item in the string by index.
        /// Allows negative indexes too (-1 is the last item, as in Python).
        /// (With LangVersion preview, F# also supports indexing from the end with the '^' prefix, e.g. str.[^0] for the last item.)
        member str.GetNeg index : char =
            let len = str.Length
            let ii =  if index < 0 then len + index else index
            if ii<0 || ii >= len then StrException.Raise "Str.ExtensionsString: str.GetNeg: Failed to get character at index %d from string of %d items: %s" index str.Length (exnf str)
            str.[ii]

        /// Any index returns a value for a non-empty string.
        /// The string is treated as an endless loop in both positive and negative directions.
        /// Throws StrException if the string is empty.
        member str.GetLooped index : char =
            let len = str.Length
            if len=0 then StrException.Raise "Str.ExtensionsString: str.GetLooped: Failed to get character at index %d from string of 0 items" index
            let t = index % len
            let ii = if t >= 0 then t  else t + len
            str.[ii]


        /// <summary>Allows for negative indices too. ( -1 is the last character, like Python)
        /// The resulting string includes the end index.
        /// If the end index is one less than the start index an empty string is returned.
        /// For example, str.SliceNeg(0,-3) trims the last two characters from the string.
        /// To reject negative indices use SliceIdx, to normalize any index with modulo use SliceLooped.
        /// (With LangVersion preview, F# also supports slicing from the end with the '^' prefix, e.g. str.[1..^1] skips the first and last character.)</summary>
        /// <param name="startIdx">The start index (inclusive, can be negative).</param>
        /// <param name="endIdx">The end index (inclusive, can be negative).</param>
        /// <returns>A new string containing the sliced characters.</returns>
        /// <exception cref="T:Str.ExtensionsString.StrException">Thrown when either index is out of range or the start index is after the end index.</exception>
        member str.SliceNeg(startIdx:int , endIdx:int) : string =
            // overrides of existing methods are unfortunately silently ignored and not possible. see https://github.com/dotnet/fsharp/issues/3692#issuecomment-334297164
            let count = str.Length
            if count = 0 then
                StrException.Raise "Str.ExtensionsString: str.SliceNeg: Can't slice an empty string. startIdx: %d endIdx: %d" startIdx endIdx
            let st  = if startIdx < 0 then count + startIdx else startIdx
            let en  = if endIdx   < 0 then count + endIdx   else endIdx
            let len = en - st + 1 // zero if end is one less than start

            if st < 0 || st > count - 1 then
                StrException.Raise "Str.ExtensionsString: str.SliceNeg: Start index %d is out of range. Allowed values are -%d up to %d for String %s of %d chars" startIdx count (count-1) (exnf str) count

            if en > count - 1 || (len < 0 && en < 0) then
                StrException.Raise "Str.ExtensionsString: str.SliceNeg: End index %d is out of range. Allowed values are -%d up to %d for String %s of %d chars" endIdx count (count-1) (exnf str) count

            if len < 0 then
                StrException.Raise "Str.ExtensionsString: str.SliceNeg: Start index %d is bigger than end index %d for String %s of %d chars" startIdx endIdx (exnf str) count

            str.Substring(st, len)

        /// <summary>Use str.SliceNeg(startIdx, endIdx) instead.
        /// Slice the string given an inclusive start and end index. Allows for negative indices too. ( -1 is the last character, like Python)</summary>
        /// <param name="startIdx">The start index (inclusive, can be negative).</param>
        /// <param name="endIdx">The end index (inclusive, can be negative).</param>
        /// <returns>A new string containing the sliced characters.</returns>
        [<Obsolete("Use str.SliceNeg(startIdx, endIdx) instead. The name Slice is avoided because in .NET the .Slice method of some collections, like List<'T> and Span<'T>, takes a start index and a length, not an inclusive end index.")>]
        member str.Slice(startIdx:int , endIdx:int) : string =
            str.SliceNeg(startIdx, endIdx)

        /// <summary>Returns a new string containing the characters between the specified inclusive start and end indices.
        /// This member rejects negative and out-of-bounds indices, while the F# slicing notation str.[1..3] does not.
        /// To allow negative indices use SliceNeg, to normalize any index with modulo use SliceLooped.</summary>
        /// <param name="startIdx">The inclusive start index of the slice.</param>
        /// <param name="endIdx">The inclusive end index of the slice.</param>
        /// <returns>A new string containing the requested range.</returns>
        /// <exception cref="T:Str.ExtensionsString.StrException">Thrown when either index is outside the string or startIdx is greater than endIdx.</exception>
        member str.SliceIdx(startIdx:int , endIdx:int) : string =
            let count = str.Length
            if count = 0 then
                StrException.Raise "Str.ExtensionsString: str.SliceIdx: Can't slice an empty string. startIdx: %d endIdx: %d" startIdx endIdx
            if startIdx < 0 || startIdx >= count then
                StrException.Raise "Str.ExtensionsString: str.SliceIdx: Start index %d is out of range. Allowed values are 0 through %d for String %s of %d chars" startIdx (count-1) (exnf str) count
            if endIdx < 0 || endIdx >= count then
                StrException.Raise "Str.ExtensionsString: str.SliceIdx: End index %d is out of range. Allowed values are 0 through %d for String %s of %d chars" endIdx (count-1) (exnf str) count
            if startIdx > endIdx then
                StrException.Raise "Str.ExtensionsString: str.SliceIdx: Start index %d is bigger than end index %d for String %s of %d chars" startIdx endIdx (exnf str) count
            str.Substring(startIdx, endIdx - startIdx + 1)

        /// <summary>Returns a new string containing the characters between the specified start and end indices after normalizing both indices with modulo.
        /// Both indices are inclusive, and negative and out-of-range indices are allowed.
        /// If the normalized start index is greater than the normalized end index, an empty string is returned.
        /// For an empty input string, an empty string is returned.</summary>
        /// <param name="startIdx">The inclusive start index to normalize.</param>
        /// <param name="endIdx">The inclusive end index to normalize.</param>
        /// <returns>A new string containing the requested range.</returns>
        member str.SliceLooped(startIdx:int , endIdx:int) : string =
            let count = str.Length
            if count = 0 then
                ""
            else
                let s = startIdx % count
                let e = endIdx % count
                let st = if s >= 0 then s else s + count
                let en = if e >= 0 then e else e + count
                let len = en - st + 1
                if len < 0 then
                    ""
                else
                    str.Substring(st, len)


        /// Returns a new string in which only the first occurrence of a specified string in the current instance is replaced with another specified string.
        /// (Will return the same instance if text to replace is not found)
        /// An empty oldValue is treated as a no-op and returns the input unchanged.
        member txt.ReplaceFirst (oldValue:string, newValue:string) : string =
            if isNull oldValue then StrException.Raise "str.ReplaceFirst: oldValue is null. (newValue:%s)  (txt:%s) " (exnf newValue) (exnf txt)
            if isNull newValue then StrException.Raise "str.ReplaceFirst: newValue is null. (oldValue:%s)  (txt:%s) " (exnf oldValue) (exnf txt)
            let idx = if oldValue.Length = 0 then -1 else txt.IndexOf(oldValue, StringComparison.Ordinal)
            if idx < 0 then txt
            else txt.Substring(0, idx) + newValue + txt.Substring(idx + oldValue.Length)


        /// Returns a new string in which only the last occurrence of a specified string in the current instance is replaced with another specified string.
        /// (Will return the same instance if text to replace is not found)
        /// An empty oldValue is treated as a no-op and returns the input unchanged.
        member txt.ReplaceLast (oldValue:string, newValue:string) : string =
            if isNull oldValue then StrException.Raise "str.ReplaceLast: oldValue is null. (newValue:%s)  (txt:%s) " (exnf newValue) (exnf txt)
            if isNull newValue then StrException.Raise "str.ReplaceLast: newValue is null. (oldValue:%s)  (txt:%s) " (exnf oldValue) (exnf txt)
            let idx = if oldValue.Length = 0 then -1 else txt.LastIndexOf(oldValue, StringComparison.Ordinal)
            if idx < 0 then txt
            else txt.Substring(0, idx) + newValue + txt.Substring(idx + oldValue.Length)


