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
    /// Joins multiple whitespaces into one.
    /// If string is null returns *null string*
    /// Does not include surrounding quotes.
    let inOneLine (s:string) =
        if isNull s then
            "*null string*"
        else
            let sb = StringBuilder(s.Length)
            let rec loop addNextWhite i =
                if i<s.Length then
                    match s.[i] with
                    |'\r'|'\n' | ' ' | '\t'->
                        if addNextWhite then sb.Append(' ') |> ignore<StringBuilder> // to have at least on space separating new lines
                        loop false (i+1)
                    | c  ->
                        sb.Append(c) |> ignore<StringBuilder>
                        loop true (i+1)
            loop false 0
            // TODO delete trailing space if there is one??
            sb.ToString()

    /// Reduces a string length for display to a maximum Length.
    /// Shows (..) as placeholder for skipped characters if string is longer than maxCharCount.
    /// If maxChars is bigger than 35 the placeholder will include the count of skipped characters: e.g. ( ... and 123 more chars.).
    /// maxCharCount will be set to be minimum 6.
    /// Returned strings are enclosed in quotation marks: '"'.
    /// If input is null it returns *null string*
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


    /// Adds a note about trimmed line count if there are more ( ... and %d more lines.).
    /// Returned strings are enclosed in quotation marks: '"'.
    /// If string is null returns *null string*.
    let truncatedToMaxLines (maxLineCount:int) (s:string) =
        let maxLines = max 1 maxLineCount
        if isNull s then
            "*null string*"
        elif s.Length < 2 then
            s
        else
            let mutable found = if s.[0]= '\n' then 1 else 0
            let mutable i = 0
            let mutable stopPos = 0
            while i >= 0 do
                if i+1=s.Length then // end of string reached with a '\n'
                    i<- -1
                else
                    i <- s.IndexOf('\n', i+1)
                    found <- found + 1
                    if found = maxLines then
                        stopPos <- i

            if stopPos > 0  && found - maxLines > 1 then // if there is just one more line print it instead of the note
                str{
                    "\""
                    s.Substring(0,stopPos+1)
                    "(... and "
                    found - maxLines
                    " more lines.)\""
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
        member inline s.DoesNotContain(subString:string) =
            s.IndexOf(subString,StringComparison.Ordinal) = -1 // in Fable the StringComparison ar is ignored. TODO Fable should issue a warning for that !

        /// s.IndexOf(chr) = -1
        member inline s.DoesNotContain(chr:char) =
            s.IndexOf(chr) = -1

        /// s.IndexOf(char) <> -1

        member inline s.Contains(chr:char) =  // this overload does not exist by default
            s.IndexOf(chr) <> -1

        /// Splits a string into substrings.
        /// Empty entries are included.
        /// s.Split( [|chr|] )
        member inline s.Split(chr:char) =  // this overload does not exist by default
            s.Split([|chr|])


        /// Calls not(String.IsNullOrWhiteSpace(str))
        member inline s.IsNotWhite =
            not(String.IsNullOrWhiteSpace s )

        /// Calls String.IsNullOrWhiteSpace(str)
        member inline s.IsWhite =
            String.IsNullOrWhiteSpace s

        /// Calls not(String.IsNullOrEmpty(str))
        member inline s.IsNotEmpty =
            not(String.IsNullOrEmpty s )

        /// Calls String.IsNullOrEmpty(str)
        member inline s.IsEmpty =
            String.IsNullOrEmpty s

/// Extension methods for System.String.
/// Adds extension members on System.String.
/// E.G. .First, .Second, .Last and similar indices.
/// Also adds functionality for negative indices and s.Slice(startIdx:int , endIdx: int) that works with negative numbers.
/// This module is NOT automatically opened when the namespace Str is opened.
module ExtensionsString =

    /// For string formatting in exceptions. Including surrounding quotes
    let exnf s  = s |> Format.truncated 100

    /// An Exception for the string functions defined in Str
    type StrException(txt:string)=
        inherit Exception(txt)
        /// Raise the exception with F# printf string formatting
        static member Raise msg =
            Printf.kprintf (fun s -> raise (new StrException(s))) msg

    type System.String with

        /// Gets an character at index, same as this.[index] or this.Idx(index)
        /// Throws a descriptive Exception if the index is out of range.
        /// (Use this.GetNeg(i) member if you want to use negative indices too)
        member inline str.Get index =
            if index < 0 then StrException.Raise "Str.ExtensionsString: str.Get(%d) failed for string of %d chars, use str.GetNeg method if you want negative indices too:\r\n%s" index str.Length (exnf str)
            if index >= str.Length then StrException.Raise "Str.ExtensionsString: str.Get(%d) failed for string of %d chars:\r\n%s" index str.Length (exnf str)
            str.[index]

        /// Gets an character at index, same as this.[index] or this.Get(index)
        /// Throws a descriptive Exception if the index is out of range.
        /// (Use this.GetNeg(i) member if you want to use negative indices too)
        member inline str.Idx index =
            if index < 0 then StrException.Raise "Str.ExtensionsString: str.Idx(%d) failed for string of %d chars, use str.GetNeg method if you want negative indices too:\r\n%s" index str.Length (exnf str)
            if index >= str.Length then StrException.Raise "Str.ExtensionsString: str.Idx(%d) failed for string of %d chars:\r\n%s" index str.Length (exnf str)
            str.[index]


        /// Returns the last valid index in the string
        /// same as: s.Length - 1
        member inline str.LastIndex =
            str.Length - 1

        /// Returns the last character of the string
        /// fails if string is empty
        member inline str.Last =
            if str.Length = 0 then StrException.Raise "Str.ExtensionsString: str.Last: Failed to get last character of empty String"
            str.[str.Length - 1]

        /// Returns the second last character of the string
        /// fails if string has less than two characters
        member inline str.SecondLast =
            if str.Length < 2 then StrException.Raise "Str.ExtensionsString: str.SecondLast: Failed to get second last character of '%s'" (exnf str)
            str.[str.Length - 2]

        /// Returns the third last character of the string
        /// fails if string has less than three characters
        member inline str.ThirdLast =
            if str.Length < 3 then StrException.Raise "Str.ExtensionsString: str.ThirdLast: Failed to get third last character of '%s'" (exnf str)
            str.[str.Length - 3]

        /// Returns the last x(int) characters of the string
        /// same as string.LastN
        member inline str.LastX x =
            if str.Length < x then StrException.Raise "Str.ExtensionsString: str.LastX: Failed to get last %d character of too short String '%s' " x (exnf str)
            str.Substring(str.Length-x,x)

        /// Returns then first character of the string
        /// fails if string is empty
        member inline str.First =
            if str.Length = 0 then StrException.Raise "Str.ExtensionsString: str.First: Failed to get first character of empty String"
            str.[0]

        /// Returns the second character of the string
        /// fails if string has less than two characters
        member inline str.Second =
            if str.Length < 2 then StrException.Raise "Str.ExtensionsString: str.Second: Failed to get second character of '%s'" (exnf str)
            str.[1]

        /// Returns the third character of the string
        /// fails if string has less than three characters
        member inline str.Third =
            if str.Length < 3 then StrException.Raise "Str.ExtensionsString: str.Third: Failed to get third character of '%s'" (exnf str)
            str.[2]


        /// Gets an item in the string by index.
        /// Allows for negative index too ( -1 is last item,  like Python)
        /// (from the release of F# 5 on a negative index can also be done with '^' prefix. E.g. ^0 for the last item)
        member str.GetNeg index =
            let len = str.Length
            let ii =  if index < 0 then len + index else index
            if ii<0 || ii >= len then StrException.Raise "Str.ExtensionsString: str.GetNeg: Failed to get character at index %d from string of %d items: %s" index str.Length (exnf str)
            str.[ii]

        /// Any index will return a value.
        /// Rarr is treated as an endless loop in positive and negative direction
        member str.GetLooped index =
            let len = str.Length
            if len=0 then StrException.Raise "Str.ExtensionsString: str.GetLooped: Failed to get character at index %d from string of 0 items" index
            let t = index % len
            let ii = if t >= 0 then t  else t + len
            str.[ii]


        /// Allows for negative indices too. -1 is last character
        /// Includes end index in string
        /// for example str.Slice(0,-3) will trim off the last two character from the string
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

