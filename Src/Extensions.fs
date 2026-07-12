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

    // This type extension should be alway available that is why it is in this Auto-open module
    type System.String with

        /// s.IndexOf(subString,StringComparison.Ordinal) = -1
        member inline s.DoesNotContain(subString:string) : bool =
            s.IndexOf(subString, StringComparison.Ordinal) = -1 // in Fable the StringComparison ar is ignored. TODO Fable should issue a warning for that !

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
/// Also adds functionality for negative indices and s.Slice(startIdx:int , endIdx: int) that works with negative numbers.
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
        /// (Since F# 5, from-end indexes can also use the '^' prefix in index expressions, e.g. str.[^0] for the last item.)
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


        /// Allows for negative indices too. -1 is last character
        /// Includes end index in string
        /// For example, str.Slice(0,-3) trims the last two characters from the string.
        member str.Slice(startIdx:int , endIdx:int):string =
             // overrides of existing methods are unfortunately silently ignored and not possible. see https://github.com/dotnet/fsharp/issues/3692#issuecomment-334297164
            let count = str.Length
            let st  = if startIdx<0 then count+startIdx else startIdx
            let len = if endIdx<0 then count+endIdx-st+1 else endIdx-st+1

            if st < 0 || st > count-1 then
                StrException.Raise "Str.ExtensionsString: str.GetSlice: Start index %d is out of range. Allowed values are -%d up to %d for String '%s' of %d chars" startIdx count (count-1) (exnf str) count


            if st+len > count then
                StrException.Raise "Str.ExtensionsString: str.GetSlice: End index %d is out of range. Allowed values are -%d up to %d for String '%s' of %d chars" startIdx count (count-1) (exnf str) count


            if len < 0 then
                let en = if endIdx<0 then count+endIdx else endIdx
                StrException.Raise "Str.ExtensionsString: str.GetSlice: Start index '%A' (= %d) is bigger than end index '%A'(= %d) for String '%s' of %d chars" startIdx st endIdx en (exnf str) count

            str.Substring(st,len)


        /// Returns a new string in which only the first occurrence of a specified string in the current instance is replaced with another specified string.
        /// (Will return the same instance if text to replace is not found)
        member txt.ReplaceFirst (oldValue:string, newValue:string) : string =
            if isNull oldValue then StrException.Raise "str.ReplaceFirst: oldValue is null. (newValue:%s)  (txt:%s) " (exnf newValue) (exnf txt)
            if isNull newValue then StrException.Raise "str.ReplaceFirst: newValue is null. (oldValue:%s)  (txt:%s) " (exnf oldValue) (exnf txt)
            let idx = txt.IndexOf(oldValue, StringComparison.Ordinal)
            if idx < 0 then txt
            else txt.Substring(0, idx) + newValue + txt.Substring(idx + oldValue.Length)


        /// Returns a new string in which only the last occurrence of a specified string in the current instance is replaced with another specified string.
        /// (Will return the same instance if text to replace is not found)
        member txt.ReplaceLast (oldValue:string, newValue:string) : string =
            if isNull oldValue then StrException.Raise "str.ReplaceLast: oldValue is null. (newValue:%s)  (txt:%s) " (exnf newValue) (exnf txt)
            if isNull newValue then StrException.Raise "str.ReplaceLast: newValue is null. (oldValue:%s)  (txt:%s) " (exnf oldValue) (exnf txt)
            let idx = txt.LastIndexOf(oldValue, StringComparison.Ordinal)
            if idx < 0 then txt
            else txt.Substring(0, idx) + newValue + txt.Substring(idx + oldValue.Length)


