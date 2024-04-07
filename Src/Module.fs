namespace Str

open System
open System.Text
open ExtensionsString // for StrException

#if FABLE_COMPILER
open Fable.Core
open Fable.Core.JsInterop
#else
#endif


/// Functions for transforming strings
[<RequireQualifiedAccess>]
module Str =

    /// Joins string into one line.
    /// Replaces line break with a space character.
    /// Skips leading whitespace on each line.
    /// Joins multiple whitespaces into one.
    /// If string is null returns *null string*
    /// Does not include surrounding quotes.
    let formatInOneLine (s:string) =
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
    let formatTruncated (maxCharCount:int) (s:string) : string =
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
    let formatTruncatedToMaxLines (maxLineCount:int) (s:string) =
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


    /// For string formatting in exceptions below. Including surrounding quotes
    let private exnf s  = s |> formatTruncated 100 //|> formatInOneLine

    ///<summary>
    /// The Knuth-Morris-Pratt Algorithm for finding a pattern within a piece of text.
    /// With complexity O(n + m)
    /// </summary>
    /// <param name="text">The text to search in</param>
    /// <param name="pattern">The pattern to search for</param>
    /// <param name="searchFromIdx">The index to start searching from</param>
    /// <param name="searchLength">The length of the text to search in</param>
    /// <param name="findNoMoreThan">The maximum number of occurrences to find</param>
    /// <returns>A list of the indices of the occurrences of the pattern in the text.
    /// The distance between these indices may be less than the pattern length
    /// Overlapping patterns are returned each time. e.g "abab" in "abababab" will return [0,2,4]</returns>
    let indicesOf (text:string, pattern:string, searchFromIdx,  searchLength, findNoMoreThan): ResizeArray<int> =
        if isNull pattern then StrException.Raise "Str.indicesOf: pattern is null"
        if isNull text    then StrException.Raise "Str.indicesOf: text is null"
        if pattern.Length > text.Length || findNoMoreThan = 0 then
            ResizeArray()
        elif pattern.Length = text.Length && pattern = text then
            let r = ResizeArray(1)
            r.Add(0)
            r
        else
            if pattern.Length = 0 then StrException.Raise "Str.indicesOf: pattern is empty"
            if searchFromIdx < 0 then StrException.Raise "Str.indicesOf: searchFromIdx:%d can't be negative. looking for %s in %s" searchFromIdx (exnf pattern) (exnf text)
            if searchLength  < 0 then StrException.Raise "Str.indicesOf: searchLength:%d can't be negative. looking for %s in %s" searchLength (exnf pattern) (exnf text)
            if searchFromIdx + searchLength > text.Length then StrException.Raise "Str.indicesOf: searchFromIdx:%d + searchLength:%d can't be longer than text:%d. looking for %s in %s" searchFromIdx searchLength text.Length (exnf pattern) (exnf text)
            //adapted from https://gist.github.com/Nabid/fde41e7c2b0b681ac674ccc93c1daeb1
            let M  = pattern.Length
            let lpsArray  = Array.zeroCreate M
            let mutable len = 0
            lpsArray.[0] <- 0
            let mutable i  = 1
            while (i < M) do
                if pattern.[i] = pattern.[len] then
                    len <- len + 1
                    lpsArray.[i] <- len
                    i <- i + 1
                else
                    if len = 0 then
                        lpsArray.[i] <- 0
                        i <- i + 1
                    else
                        len <- lpsArray.[len - 1]

            let N  = searchFromIdx + searchLength  //text.Length
            let matchedIndices  = new ResizeArray<int>()
            i <- searchFromIdx
            let mutable j  = 0
            while (i < N) do
                if text.[i] = pattern.[j] then
                    i <- i + 1
                    j <- j + 1
                if j = M then
                    if matchedIndices.Count < findNoMoreThan then
                        matchedIndices.Add (i - j)
                    else
                        i <- N // to exit loop
                    j <- lpsArray.[j - 1]
                else
                    if i < N && text.[i] <> pattern.[j] then
                        if j <> 0 then
                            j <- lpsArray.[j - 1]
                        else
                            i <- i + 1
            matchedIndices


    /// Takes at most a given amount of chars from string.
    /// If input is shorter than truncateLength returns input string unchanged.
    /// Alternatively use the functions that include formatting:
    /// Str.Format.stringTruncated and
    /// Str.Format.stringInOneLine
    /// Str.Format.stringTruncatedToMaxLines
    let (*inline*) truncate (truncateLength:int) (fromString:string) :string =
        if isNull fromString   then StrException.Raise "Str.truncate: fromString is null (truncateLength:%d)" truncateLength
        if truncateLength < 0  then StrException.Raise "Str.truncate: truncateLength:%d cant be negative(for %s)" truncateLength (exnf fromString)
        if truncateLength >= fromString.Length then fromString
        else fromString.Substring(0,truncateLength)



    /// Remove characters from the starts.
    /// fromString.Substring(skipLength)
    let (*inline*) skip (skipLength:int) (fromString:string) :string =
        if isNull fromString   then StrException.Raise "Str.skip: fromString is null (skipLength:%d)" skipLength
        if skipLength > fromString.Length then StrException.Raise "Str.skip: skipLength:%d is longer than string %s" skipLength (exnf fromString)
        if skipLength < 0 then StrException.Raise "Str.skip: skipLength:%d cant be negative(for  %s)" skipLength (exnf fromString)
        fromString.Substring(skipLength)

    /// Takes a given amount of chars from string.
    /// Fails if input is shorter than takeLength. Use String.truncate instead if you want to avoid failing in that case.
    /// Code: fromString.Substring(0,takeLength)
    let (*inline*) take (takeLength:int) (fromString:string) :string =
        if isNull fromString  then StrException.Raise "Str.take: fromString is null (takeLength:%d)" takeLength
        if takeLength > fromString.Length then StrException.Raise "Str.take: takeLength:%d is longer than string %s. Use String.truncate instead!" takeLength (exnf fromString)
        if takeLength < 0 then StrException.Raise "Str.take: takeLength:%d cant be negative(for  %s)" takeLength (exnf fromString)
        fromString.Substring(0,takeLength)


    /// Removes all occurrences of a substring from a string if it exists. same as:
    /// (Will return the same string instance, if text to remove is not found)
    /// Code:fromString.Replace(textToRemove, "")
    let (*inline*) delete (textToRemove:string) (fromString:string) :string =
        if isNull fromString   then StrException.Raise "Str.delete: fromString is null (textToRemove:%s)" (exnf textToRemove)
        if isNull textToRemove then StrException.Raise "Str.delete: textToRemove is null (fromString:%s)" (exnf fromString)
        if textToRemove="" then // would fail with System.ArgumentException: String cannot be of zero length.
            fromString
        else
            fromString.Replace(textToRemove, "") // will return the same instance if text to remove is not found

    /// Removes character from a string if it exists. same as:
    /// (Will return the same string instance, if char to remove is not found)
    /// Code: fromString.Replace(charToRemove.ToString(), "")
    let (*inline*) deleteChar (charToRemove:char) (fromString:string) :string =
        if isNull fromString   then StrException.Raise "Str.delete: fromString is null (charToRemove:'%c')" charToRemove
        fromString.Replace(charToRemove.ToString(), "") // will return the same instance if text to remove is not found


    /// Ensures all lines end on System.Environment.NewLine
    /// Code: StringBuilder(s).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine).ToString()
    let (*inline*) unifyLineEndings (txt:string) =
        if isNull txt then StrException.Raise "Str.unifyLineEndings: input is null"
        StringBuilder(txt).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine).ToString() // TODO correct but not performance optimized

    /// Returns everything before first occurrence of a given splitting string.
    /// Or fails if splitter is not found.
    /// Uses StringComparison.Ordinal
    let (*inline*) before (splitter:string) (stringToSearchIn:string) :string =
        if isNull stringToSearchIn then StrException.Raise "Str.before: stringToSearchIn is null (splitter:%s)" (exnf splitter)
        if isNull splitter         then StrException.Raise "Str.before: splitter is null (stringToSearchIn:%s)" (exnf stringToSearchIn)
        let start = stringToSearchIn.IndexOf(splitter, StringComparison.Ordinal)
        if start = -1 then StrException.Raise "Str.before: splitter %s not found in stringToSearchIn:%s" (exnf splitter) (exnf stringToSearchIn)
        else stringToSearchIn.Substring(0, start)

    /// Returns everything before first occurrence of a given splitting character.
    /// Or fails if splitter is not found.
    let (*inline*) beforeChar (splitter:char) (stringToSearchIn:string) :string =
        if isNull stringToSearchIn then StrException.Raise "Str.beforeChar: stringToSearchIn is null (splitter:'%c')" (splitter)
        let start = stringToSearchIn.IndexOf(splitter)
        if start = -1 then StrException.Raise "Str.before: splitter '%c' not found in stringToSearchIn:%s" splitter (exnf stringToSearchIn)
        else stringToSearchIn.Substring(0, start)

    /// Returns everything before first occurrence of a given splitting string.
    /// Or None if splitter is not found.
    /// Uses StringComparison.Ordinal
    let (*inline*) tryBefore (splitter:string) (stringToSearchIn:string): option<string> =
        if isNull stringToSearchIn then StrException.Raise "Str.tryBefore: stringToSearchIn is null (splitter:%s)" (exnf splitter)
        if isNull splitter         then StrException.Raise "Str.tryBefore: splitter is null (stringToSearchIn:%s)" (exnf stringToSearchIn)
        let start = stringToSearchIn.IndexOf(splitter, StringComparison.Ordinal)
        if start = -1 then None
        else Some <| stringToSearchIn.Substring(0, start)

    /// Returns everything before first occurrence of a given splitting character.
    /// Or None if splitter is not found.
    let (*inline*) tryBeforeChar (splitter:char) (stringToSearchIn:string): option<string>  =
        if isNull stringToSearchIn then StrException.Raise "Str.tryBeforeChar: stringToSearchIn is null (splitter:'%c')" (splitter)
        let start = stringToSearchIn.IndexOf(splitter)
        if start = -1 then None
        else Some <| stringToSearchIn.Substring(0, start)

    /// Returns everything before first occurrence of a given splitting string.
    /// Or full input string if splitter is not found.
    /// Uses StringComparison.Ordinal
    let (*inline*) beforeOrInput (splitter:string) (stringToSearchIn:string) :string =
        if isNull stringToSearchIn then StrException.Raise "Str.beforeOrInput: stringToSearchIn is null (splitter:%s)" (exnf splitter)
        if isNull splitter         then StrException.Raise "Str.beforeOrInput: splitter is null (stringToSearchIn:%s)" (exnf stringToSearchIn)
        let start = stringToSearchIn.IndexOf(splitter, StringComparison.Ordinal)
        if start = -1 then stringToSearchIn
        else stringToSearchIn.Substring(0, start)

    /// Returns everything before first occurrence of a given splitting character.
    /// Or full input string if splitter is not found.
    let (*inline*) beforeCharOrInput (splitter:char) (stringToSearchIn:string) :string =
        if isNull stringToSearchIn then StrException.Raise "Str.beforeCharOrInput: stringToSearchIn is null (splitter:'%c')" (splitter)
        let start = stringToSearchIn.IndexOf(splitter)
        if start = -1 then stringToSearchIn
        else stringToSearchIn.Substring(0, start)

    /// Returns everything after first occurrence of a given splitting string.
    /// Or fails if splitter is not found.
    /// Uses StringComparison.Ordinal
    let (*inline*) after (splitter:string) (stringToSearchIn:string) :string =
        if isNull stringToSearchIn then StrException.Raise "Str.after: stringToSearchIn is null (splitter:%s)" (exnf splitter)
        if isNull splitter         then StrException.Raise "Str.after: splitter is null (stringToSearchIn:%s)" (exnf stringToSearchIn)
        let start = stringToSearchIn.IndexOf(splitter, StringComparison.Ordinal)
        if start = -1 then StrException.Raise "Str.after: splitter %s not found in stringToSearchIn:%s" (exnf splitter) (exnf stringToSearchIn)
        else stringToSearchIn.Substring(start+splitter.Length)//cant be out of bounds!

    /// Returns everything after first occurrence of a given splitting character.
    /// Or fails if splitter is not found
    let (*inline*) afterChar (splitter:char) (stringToSearchIn:string) :string  =
        if isNull stringToSearchIn then StrException.Raise "Str.afterChar: stringToSearchIn is null (splitter:'%c')" (splitter)
        let start = stringToSearchIn.IndexOf(splitter)
        if start = -1 then StrException.Raise "Str.after: splitter '%c' not found in stringToSearchIn:%s" splitter (exnf stringToSearchIn)
        else stringToSearchIn.Substring(start+1)//cant be out of bounds!

    /// Returns everything after first occurrence of a given splitting string.
    /// Or None if splitter is not found
    /// Uses StringComparison.Ordinal
    let (*inline*) tryAfter (splitter:string) (stringToSearchIn:string): option<string>  =
        if isNull stringToSearchIn then StrException.Raise "Str.tryAfter: stringToSearchIn is null (splitter:%s)" (exnf splitter)
        if isNull splitter         then StrException.Raise "Str.tryAfter: splitter is null (stringToSearchIn:%s)" (exnf stringToSearchIn)
        let start = stringToSearchIn.IndexOf(splitter, StringComparison.Ordinal)
        if start = -1 then None
        else Some <|stringToSearchIn.Substring(start+splitter.Length)//cant be out of bounds!

    /// Returns everything after first occurrence of a given splitting character.
    /// Or None if splitter is not found
    let (*inline*) tryAfterChar (splitter:char) (stringToSearchIn:string) : option<string> =
        if isNull stringToSearchIn then StrException.Raise "Str.tryAfterChar: stringToSearchIn is null (splitter:'%c')" (splitter)
        let start = stringToSearchIn.IndexOf(splitter)
        if start = -1 then None
        else Some <|stringToSearchIn.Substring(start+1)//cant be out of bounds!

    /// Returns everything after first occurrence of a given splitting string.
    /// Or full input string if splitter is not found.
    /// Uses StringComparison.Ordinal
    let (*inline*) afterOrInput (splitter:string) (stringToSearchIn:string) :string =
        if isNull stringToSearchIn then StrException.Raise "Str.afterOrInput: stringToSearchIn is null (splitter:%s)" (exnf splitter)
        if isNull splitter         then StrException.Raise "Str.afterOrInput: splitter is null (stringToSearchIn:%s)" (exnf stringToSearchIn)
        let start = stringToSearchIn.IndexOf(splitter, StringComparison.Ordinal)
        if start = -1 then stringToSearchIn
        else stringToSearchIn.Substring(start+splitter.Length)

    /// Returns everything after first occurrence of a given splitting character.
    /// Or full input string if splitter is not found
    let (*inline*) afterCharOrInput (splitter:char) (stringToSearchIn:string) :string  =
        if isNull stringToSearchIn then StrException.Raise "Str.afterCharOrInput: stringToSearchIn is null (splitter:'%c')" (splitter)
        let start = stringToSearchIn.IndexOf(splitter)
        if start = -1 then stringToSearchIn
        else stringToSearchIn.Substring(start+1)

    /// Finds text between two strings
    /// e.g.: between "X" "T" "cXabTk" =  "ab"
    /// Fails if not both splitters are found.
    /// Delimiters are excluded in the returned strings
    let (*inline*) between (firstSplitter:string) (secondSplitter:string) (stringToSplit:string) :string =
        if isNull stringToSplit  then StrException.Raise "Str.between: stringToSplit is null (firstSplitter:%s, secondSplitter:%s) " (exnf firstSplitter) (exnf secondSplitter)
        if isNull firstSplitter  then StrException.Raise "Str.between: firstSplitter is null (stringToSplit:%s, secondSplitter:%s)" (exnf stringToSplit) (exnf secondSplitter)
        if isNull secondSplitter then StrException.Raise "Str.between: secondSplitter is null (stringToSplit:%s, firstSplitter:%s)" (exnf stringToSplit) (exnf firstSplitter)
        let start = stringToSplit.IndexOf(firstSplitter, StringComparison.Ordinal)
        if start = -1 then StrException.Raise "Str.between: firstSplitter: %s not found in stringToSplit: %s  (secondSplitter: %s)" (exnf firstSplitter) (exnf stringToSplit) (exnf secondSplitter)
        else
            let ende = stringToSplit.IndexOf(secondSplitter, start + firstSplitter.Length, StringComparison.Ordinal)
            if ende = -1 then StrException.Raise "Str.between: secondSplitter: %s not found in stringToSplit: %s  (firstSplitter: %s)" (exnf secondSplitter) (exnf stringToSplit) (exnf firstSplitter)
            else
                stringToSplit.Substring(start + firstSplitter.Length, ende - start - firstSplitter.Length)// finds text between two chars

    /// Finds text between two strings
    /// e.g.: between "X" "T" "cXabTk" =  "ab"
    /// Returns None if not both splitters are found.
    /// Delimiters are excluded in the returned strings
    let (*inline*) tryBetween (firstSplitter:string) (secondSplitter:string) (stringToSplit:string): option<string>  =
        if isNull stringToSplit  then StrException.Raise "Str.tryBetween: stringToSplit is null (firstSplitter:%s, secondSplitter:%s) " (exnf firstSplitter) (exnf secondSplitter)
        if isNull firstSplitter  then StrException.Raise "Str.tryBetween: firstSplitter is null (stringToSplit:%s, secondSplitter:%s)" (exnf stringToSplit) (exnf secondSplitter)
        if isNull secondSplitter then StrException.Raise "Str.tryBetween: secondSplitter is null (stringToSplit:%s, firstSplitter:%s)" (exnf stringToSplit) (exnf firstSplitter)
        let start = stringToSplit.IndexOf(firstSplitter, StringComparison.Ordinal)
        if start = -1 then None
        else
            let ende = stringToSplit.IndexOf(secondSplitter, start + firstSplitter.Length, StringComparison.Ordinal)
            if ende = -1 then None
            else
                Some <|stringToSplit.Substring(start + firstSplitter.Length, ende - start - firstSplitter.Length)// finds text between two chars

    /// Finds text between two strings
    /// e.g.: between "X" "T" "cXabTk" =  "ab"
    /// Returns full input string if not both splitters are found.
    /// Delimiters are excluded in the returned strings
    let (*inline*) betweenOrInput (firstSplitter:string) (secondSplitter:string) (stringToSplit:string): string  =
        if isNull stringToSplit  then StrException.Raise "Str.betweenOrInput: stringToSplit is null (firstSplitter:%s, secondSplitter:%s) " (exnf firstSplitter) (exnf secondSplitter)
        if isNull firstSplitter  then StrException.Raise "Str.betweenOrInput: firstSplitter is null (stringToSplit:%s, secondSplitter:%s)" (exnf stringToSplit) (exnf secondSplitter)
        if isNull secondSplitter then StrException.Raise "Str.betweenOrInput: secondSplitter is null (stringToSplit:%s, firstSplitter:%s)" (exnf stringToSplit) (exnf firstSplitter)
        let start = stringToSplit.IndexOf(firstSplitter, StringComparison.Ordinal)
        if start = -1 then stringToSplit
        else
            let ende = stringToSplit.IndexOf(secondSplitter, start + firstSplitter.Length, StringComparison.Ordinal)
            if ende = -1 then stringToSplit
            else
                stringToSplit.Substring(start + firstSplitter.Length, ende - start - firstSplitter.Length)// finds text between two chars

    /// Finds text between two Characters
    /// e.g.: between 'X' 'T' "cXabTk" =  "ab"
    /// Fails if not both splitters are found.
    /// Delimiters are excluded in the returned strings
    let (*inline*) betweenChars (firstSplitter:Char) (secondSplitter:Char) (stringToSplit:string) :string =
        if isNull stringToSplit  then StrException.Raise "Str.between: stringToSplit is null (firstSplitter: '%c', secondSplitter: '%c') " ( firstSplitter) ( secondSplitter)
        let start = stringToSplit.IndexOf(firstSplitter)
        if start = -1 then StrException.Raise "Str.between: firstSplitter: '%c' not found in stringToSplit: %s  (secondSplitter: '%c')" ( firstSplitter) (exnf stringToSplit) ( secondSplitter)
        else
            let ende = stringToSplit.IndexOf(secondSplitter, start + 1)
            if ende = -1 then StrException.Raise "Str.between: secondSplitter: '%c' not found in stringToSplit: %s  (firstSplitter: '%c')" ( secondSplitter) (exnf stringToSplit) ( firstSplitter)
            else
                stringToSplit.Substring(start + 1, ende - start - 1)// finds text between two chars


    /// Finds text between two Characters
    /// e.g.: between 'X' 'T' "cXabTk" =  "ab"
    /// Returns None if not both splitters are found.
    /// Delimiters are excluded in the returned strings
    let (*inline*) tryBetweenChars (firstSplitter:Char) (secondSplitter:Char) (stringToSplit:string): option<string>  =
        if isNull stringToSplit  then StrException.Raise "Str.tryBetween: stringToSplit is null (firstSplitter: '%c', secondSplitter: '%c') " ( firstSplitter) ( secondSplitter)
        let start = stringToSplit.IndexOf(firstSplitter)
        if start = -1 then None
        else
            let ende = stringToSplit.IndexOf(secondSplitter, start + 1)
            if ende = -1 then None
            else
                Some <|stringToSplit.Substring(start + 1, ende - start - 1)// finds text between two chars

    /// Finds text between two Characters
    /// e.g.: between 'X' 'T' "cXabTk" =  "ab"
    /// Returns full input string if not both splitters are found.
    /// Delimiters are excluded in the returned strings
    let (*inline*) betweenCharsOrInput (firstSplitter:Char) (secondSplitter:Char) (stringToSplit:string): string  =
        if isNull stringToSplit  then StrException.Raise "Str.betweenOrInput: stringToSplit is null (firstSplitter: '%c', secondSplitter: '%c') " ( firstSplitter) ( secondSplitter)
        let start = stringToSplit.IndexOf(firstSplitter)
        if start = -1 then stringToSplit
        else
            let ende = stringToSplit.IndexOf(secondSplitter, start + 1)
            if ende = -1 then stringToSplit
            else
                stringToSplit.Substring(start + 1, ende - start - 1)// finds text between two chars


    /// Split string into two elements,
    /// Fails if  splitter is not found.
    /// The splitter is not included in the two return strings.
    let (*inline*) splitOnce (splitter:string) (stringToSplit:string) : string*string =
        if isNull stringToSplit then StrException.Raise "Str.splitOnce: stringToSplit is null (splitter:%s)" (exnf splitter)
        if isNull splitter         then StrException.Raise "Str.splitOnce: splitter is null (stringToSplit:%s)" (exnf stringToSplit)
        let start = stringToSplit.IndexOf(splitter, StringComparison.Ordinal)
        if start = -1 then StrException.Raise "Str.splitOnce: splitter %s not found in stringToSplit: %s" (exnf splitter) (exnf stringToSplit)
        else               stringToSplit.Substring(0, start), stringToSplit.Substring(start + splitter.Length)

    /// Finds text before, between and after  two strings.
    /// e.g.: between "X" "T" "cXabTk" = "c", "ab", "k"
    /// Fails if not both splitters are found.
    /// Delimiters are excluded in the three returned strings
    let (*inline*) splitTwice (firstSplitter:string) (secondSplitter:string) (stringToSplit:string) : string*string*string =
        if isNull stringToSplit  then StrException.Raise "Str.splitTwice: stringToSplit is null (firstSplitter:%s, secondSplitter:%s) " (exnf firstSplitter) (exnf secondSplitter)
        if isNull firstSplitter  then StrException.Raise "Str.splitTwice: firstSplitter is null (stringToSplit:%s, secondSplitter:%s)" (exnf stringToSplit) (exnf secondSplitter)
        if isNull secondSplitter then StrException.Raise "Str.splitTwice: secondSplitter is null (stringToSplit:%s, firstSplitter:%s)" (exnf stringToSplit) (exnf firstSplitter)
        let start = stringToSplit.IndexOf(firstSplitter, StringComparison.Ordinal)
        if start = -1 then StrException.Raise "Str.splitTwice: firstSplitter: %s not found in stringToSplit: %s  (secondSplitter: %s)" (exnf firstSplitter) (exnf stringToSplit) (exnf secondSplitter)
        else
            let ende = stringToSplit.IndexOf(secondSplitter, start + firstSplitter.Length, StringComparison.Ordinal)
            if ende = -1 then StrException.Raise "Str.splitTwice: secondSplitter: %s not found in stringToSplit: %s  (firstSplitter: %s)" (exnf secondSplitter) (exnf stringToSplit) (exnf firstSplitter)
            else
                stringToSplit.Substring(0, start ),
                stringToSplit.Substring(start + firstSplitter.Length, ende - start - firstSplitter.Length),// finds text between two chars
                stringToSplit.Substring(ende + secondSplitter.Length)

    /// Split string into two elements,
    /// If splitter not found None is returned
    /// Splitter is not included in the two return strings.
    let (*inline*) trySplitOnce (splitter:string) (stringToSplit:string) : option<string*string> =
        if isNull stringToSplit then StrException.Raise "Str.trySplitOnce: stringToSplit is null (splitter:%s)" (exnf splitter)
        if isNull splitter         then StrException.Raise "Str.trySplitOnce: splitter is null (stringToSplit:%s)" (exnf stringToSplit)
        let start = stringToSplit.IndexOf(splitter, StringComparison.Ordinal)
        if start = -1 then None
        else               Some (stringToSplit.Substring(0, start), stringToSplit.Substring(start + splitter.Length))

    /// Finds text before, between and after two strings.
    /// e.g.: between "X" "T" "cXabTk" = "c", "ab", "k"
    /// If not both splitters are found returns None
    /// Delimiters are excluded in the three returned strings
    let (*inline*) trySplitTwice (firstSplitter:string) (secondSplitter:string) (stringToSplit:string) : option<string*string*string>=
        if isNull stringToSplit  then StrException.Raise "Str.trySplitTwice: stringToSplit is null (firstSplitter:%s, secondSplitter:%s)" (exnf firstSplitter) (exnf secondSplitter)
        if isNull firstSplitter  then StrException.Raise "Str.trySplitTwice: firstSplitter is null (stringToSplit:%s, secondSplitter:%s)" (exnf stringToSplit)(exnf secondSplitter)
        if isNull secondSplitter then StrException.Raise "Str.trySplitTwice: secondSplitter is null (stringToSplit:%s, firstSplitter:%s)" (exnf stringToSplit)(exnf firstSplitter)
        let start = stringToSplit.IndexOf(firstSplitter, StringComparison.Ordinal)
        if start = -1 then None
        else
            let ende = stringToSplit.IndexOf(secondSplitter, start + firstSplitter.Length, StringComparison.Ordinal)
            if ende = -1 then None
            else Some(
                    stringToSplit.Substring(0, start ),
                    stringToSplit.Substring(start + firstSplitter.Length, ende - start - firstSplitter.Length),// finds text between two chars
                    stringToSplit.Substring(ende + secondSplitter.Length)
                    )


    /// Makes First letter of string to Uppercase if possible.
    let (*inline*) up1 (txt:string)  =
        if isNull txt then StrException.Raise "Str.up1: string is null"
        if txt="" || Char.IsUpper txt.[0] then txt
        elif Char.IsLetter txt.[0] then  String(Char.ToUpper(txt.[0]),1) + txt.Substring(1)
        else txt

    /// Makes First letter of string to Lowercase if possible.
    let (*inline*) low1 (txt:string) =
        if isNull txt then StrException.Raise "Str.low1: string is null"
        if txt="" || Char.IsLower txt.[0] then txt
        elif Char.IsLetter txt.[0] then  String(Char.ToLower(txt.[0]),1) + txt.Substring(1)
        else txt

    /// Allows for negative indices too. -1 is last character
    /// The resulting string includes the end index.
    let (*inline*) slice startIdx endIdx (txt:string) =
        if isNull txt then StrException.Raise "Str.slice: string is null! startIdx: %d endIdx: %d" startIdx  endIdx
        let count = txt.Length
        let st  = if startIdx<0 then count+startIdx    else startIdx
        let len = if endIdx<0   then count+endIdx-st+1 else endIdx-st+1
        if st < 0 || st > count-1 then
            StrException.Raise "Str.slice: Start index %d is out of range. Allowed values are -%d up to %d for String %s of %d chars" startIdx count (count-1) (exnf txt) count
        if st+len > count then
            StrException.Raise "Str.slice: End index %d is out of range. Allowed values are -%d up to %d for String %s of %d chars" startIdx count (count-1) (exnf txt) count
        if len < 0 then
            let en = if endIdx<0 then count+endIdx else endIdx
            StrException.Raise "Str.slice: Start index '%A' (= %d) is bigger than end index '%A'(= %d) for String %s of %d items" startIdx st endIdx en (exnf txt) count
        txt.Substring(st,len)


    /// Counts how often a substring appears in a string
    /// Uses StringComparison.Ordinal
    let (*inline*) countSubString (subString:string) (textToSearch:string) =
        if isNull textToSearch then StrException.Raise "Str.countSubString: textToSearch is null, subString: %s" (exnf subString)
        if isNull subString    then StrException.Raise "Str.countSubString: subString is null, textToSearch: %s" (exnf textToSearch)
        let rec find (fromIdx:int) k =
            let r = textToSearch.IndexOf(subString, fromIdx, StringComparison.Ordinal)
            if r < 0 then k
            else find (r + subString.Length) (k + 1)
        find 0 0

    /// Counts how often a character appears in a string
    let (*inline*) countChar (chr:char) (textToSearch:string) =
        if isNull textToSearch then StrException.Raise "Str.countChar: textToSearch is null, chr: '%c'" chr
        let rec find (fromIdx:int) k =
            let r = textToSearch.IndexOf(chr, fromIdx)
            if r < 0 then k
            else find (r + 1) (k + 1)
        find 0 0




    /// Add a suffix to string
    let (*inline*) addSuffix (suffix:string) (txt:string) =
        if isNull txt then StrException.Raise "Str.addSuffix: txt is null"
        txt+suffix


    /// Add a prefix to string
    let (*inline*) addPrefix (prefix:string) (txt:string) =
        if isNull txt then StrException.Raise "Str.addPrefix: txt is null"
        prefix+txt

    /// Add a double quote (") at start and end.
    let (*inline*) inQuotes (txt:string) =
        if isNull txt then StrException.Raise "Str.inQuotes: txt is null"
        "\"" + txt + "\""

    /// Add a single quote (') at start and end.
    let (*inline*) inSingleQuotes (txt:string) =
        if isNull txt then StrException.Raise "Str.inSingleQuotes: txt is null"
        "'" + txt + "'"


    //-------------------------------------------------------------------------
    //-------------------------------------------------------------------------
    // taken and adapted from  FSharpx
    // https://raw.githubusercontent.com/fsprojects/FSharpx.Extras/master/src/FSharpx.Extras/String.fs
    //-------------------------------------------------------------------------
    //-------------------------------------------------------------------------

    /// Returns true if a specified substring occurs within this string.
    let (*inline*) contains (stringToFind:string) (stringToSearchIn:string) =
        if isNull stringToSearchIn then StrException.Raise "Str.contains: stringToSearchIn is null, stringToFind: %s"     (exnf stringToFind)
        if isNull stringToFind     then StrException.Raise "Str.contains: stringToFind     is null, stringToSearchIn: %s" (exnf stringToSearchIn)
        stringToSearchIn.Contains(stringToFind)

    /// Returns true if a specified substring occurs within this string ignoring casing.
    /// uses: stringToSearchIn.IndexOf(stringToFind, StringComparison.OrdinalIgnoreCase) <> -1
    let (*inline*) containsIgnoreCase (stringToFind:string) (stringToSearchIn:string) =
        // TODO for JS: use toLowerCase or regex: https://stackoverflow.com/questions/60634324/check-whether-string-contains-substring-non-case-sensitive
        if isNull stringToSearchIn then StrException.Raise "Str.containsIgnoreCase: stringToSearchIn is null, stringToFind: %s"     (exnf stringToFind)
        if isNull stringToFind     then StrException.Raise "Str.containsIgnoreCase: stringToFind     is null, stringToSearchIn: %s" (exnf stringToSearchIn)
        #if FABLE_COMPILER
        stringToSearchIn.ToLower().Contains(stringToFind.ToLower())
        #else
        stringToSearchIn.IndexOf(stringToFind, StringComparison.OrdinalIgnoreCase) <> -1
        #endif


    /// Returns true if a specified substring does NOT occurs within this string.
    let (*inline*) notContains (stringToFind:string) (stringToSearchIn:string) =
        if isNull stringToSearchIn then StrException.Raise "Str.notContains: stringToSearchIn is null, stringToFind: %s"     (exnf stringToFind)
        if isNull stringToFind     then StrException.Raise "Str.notContains: stringToFind     is null, stringToSearchIn: %s" (exnf stringToSearchIn)
        not (stringToSearchIn.Contains(stringToFind))

    /// Returns true if specified character occurs within this string.
    let (*inline*) containsChar (charToFind:char) (stringToSearchIn:string) =
        if isNull stringToSearchIn then StrException.Raise "Str.containsChar: stringToSearchIn is null, char: '%c'" charToFind
        stringToSearchIn.IndexOf(charToFind) <> -1

    /// Returns true if specified character does NOT occurs within this string.
    let (*inline*) notContainsChar (charToFind:char) (stringToSearchIn:string) =
        if isNull stringToSearchIn then StrException.Raise "Str.notContainsChar: stringToSearchIn is null, char: '%c'" charToFind
        stringToSearchIn.IndexOf(charToFind) = -1

    /// Compares two specified String objects and returns an integer that indicates their relative position in the sort order, u
    /// Uses StringComparison.Ordinal.
    let (*inline*) compare strA strB =
        if isNull strA then StrException.Raise "Str.compare: strA is null, strB: %s" (exnf strB)
        if isNull strB then StrException.Raise "Str.compare: strB is null, strA: %s" (exnf strA)
        String.Compare(strA, strB, StringComparison.Ordinal)

    /// Compares two specified String objects and returns an integer that indicates their relative position in the sort order,
    /// Uses StringComparison.OrdinalIgnoreCase.
    let (*inline*) compareIgnoreCase strA strB =
        if isNull strA then StrException.Raise "Str.compareIgnoreCase: strA is null, strB: %s" (exnf strB)
        if isNull strB then StrException.Raise "Str.compareIgnoreCase: strB is null, strA: %s" (exnf strA)
        #if FABLE_COMPILER
        String.Compare(strA.ToLower(), strB.ToLower(), StringComparison.Ordinal)
        #else
        String.Compare(strA, strB, StringComparison.OrdinalIgnoreCase )
        #endif

    /// Determines whether the end of this string instance matches the specified string, using StringComparison.Ordinal.
    let (*inline*) endsWith (stringToFindAtEnd : string) (stringSearchInAtEnd:string)  =
        if isNull stringToFindAtEnd then StrException.Raise "Str.endsWith: stringToFindAtEnd is null. (stringSearchInAtEnd:%s) " (exnf stringSearchInAtEnd)
        if isNull stringSearchInAtEnd then StrException.Raise "Str.endsWith: stringSearchInAtEnd is null. (stringToFindAtEnd:%s) " (exnf stringToFindAtEnd)
        stringSearchInAtEnd.EndsWith(stringToFindAtEnd, StringComparison.Ordinal)

    /// Determines whether the end of this string instance matches the specified string, using StringComparison.OrdinalIgnoreCase.
    let (*inline*) endsWithIgnoreCase (stringToFindAtEnd : string) (stringSearchInAtEnd:string)  =
        if isNull stringToFindAtEnd then StrException.Raise "Str.endsWithIgnoreCase: stringToFindAtEnd is null. (stringSearchInAtEnd:%s) " (exnf stringSearchInAtEnd)
        if isNull stringSearchInAtEnd then StrException.Raise "Str.endsWithIgnoreCase: stringSearchInAtEnd is null. (stringToFindAtEnd:%s) " (exnf stringToFindAtEnd)
        #if FABLE_COMPILER
        stringSearchInAtEnd.ToLower().EndsWith(stringToFindAtEnd.ToLower())
        #else
        stringSearchInAtEnd.EndsWith(stringToFindAtEnd, StringComparison.OrdinalIgnoreCase)
        #endif

    /// Determines whether the beginning of this string instance matches the specified string, using StringComparison.Ordinal..
    let (*inline*) startsWith (stringToFindAtStart:string) (stringToSearchIn:string)  =
        if isNull stringToFindAtStart then StrException.Raise "Str.startsWith: stringToFindAtStart is null. (stringToSearchIn:%s) " (exnf stringToSearchIn)
        if isNull stringToSearchIn then StrException.Raise "Str.startsWith: stringToSearchIn is null. (stringToFindAtStart:%s) " (exnf stringToFindAtStart)
        stringToSearchIn.StartsWith(stringToFindAtStart, StringComparison.Ordinal)

    /// Determines whether the beginning of this string instance matches the specified string when compared using StringComparison.OrdinalIgnoreCase.
    let (*inline*) startsWithIgnoreCase (stringToFindAtStart:string) (stringToSearchIn:string)  =
        if isNull stringToFindAtStart then StrException.Raise "Str.startsWithIgnoreCase: stringToFindAtStart is null.  (stringToSearchIn:%s) "  (exnf stringToSearchIn)
        if isNull stringToSearchIn then StrException.Raise "Str.startsWithIgnoreCase: stringToSearchIn is null. (stringToFindAtStart:%s)  " (exnf stringToFindAtStart)
        #if FABLE_COMPILER
        stringToSearchIn.ToLower().StartsWith(stringToFindAtStart.ToLower())
        #else
        stringToSearchIn.StartsWith(stringToFindAtStart, StringComparison.OrdinalIgnoreCase)
        #endif

    /// Determines whether two specified String objects have the same value, using StringComparison.Ordinal.(=fastest comparison)
    let (*inline*) equals strA strB  =
        if isNull strA then StrException.Raise "Str.equals: strA is null, strB: %s" (exnf strB)
        if isNull strB then StrException.Raise "Str.equals: strB is null, strA: %s" (exnf strA)
        String.Equals(strA, strB, StringComparison.Ordinal)

    /// Determines whether two specified String objects have the same value, using StringComparison.OrdinalIgnoreCase.
    let (*inline*) equalsIgnoreCase strA strB =
        if isNull strA then StrException.Raise "Str.equalsIgnoreCase: strA is null, strB: %s" (exnf strB)
        if isNull strB then StrException.Raise "Str.equalsIgnoreCase: strB is null, strA: %s" (exnf strA)
        #if FABLE_COMPILER
        strA.ToLower() = strB.ToLower()
        #else
        String.Equals(strA, strB, StringComparison.OrdinalIgnoreCase )
        #endif

    /// Reports the zero-based index of the first occurrence of the specified Unicode character in this string.
    let (*inline*) indexOfChar (charToFind:char) (stringToSearchIn:string)  =
        if isNull stringToSearchIn then StrException.Raise "Str.indexOfChar: stringToSearchIn is null. (charToFind:'%c') " charToFind
        stringToSearchIn.IndexOf(charToFind)

    /// Reports the zero-based index of the first occurrence of the specified Unicode character in this string. The search starts at a specified character position.
    let (*inline*) indexOfCharFrom (charToFind:char) startIndex (stringToSearchIn:string)  =
        if isNull stringToSearchIn then StrException.Raise "Str.indexOfCharFrom: stringToSearchIn is null. (charToFind:'%c')  (startIndex:%d) " charToFind startIndex
        stringToSearchIn.IndexOf(charToFind, startIndex)

    /// Reports the zero-based index of the first occurrence of the specified character in this instance.
    /// The search starts at a specified character position and examines a specified number of character positions.
    /// When used in Fable this use the Knuth-Morris-Pratt algorithm via Str.indicesOf
    let (*inline*) indexOfCharFromFor (charToFind:char) startIndex count (stringToSearchIn:string)  =
        if isNull stringToSearchIn then StrException.Raise "Str.indexOfCharFromFor : stringToSearchIn is null. (charToFind:'%c')  (startIndex:%d)  (count:%d) " charToFind startIndex count
        #if FABLE_COMPILER
        let f = indicesOf (stringToSearchIn, string charToFind, startIndex, count, 1)
        if f.Count = 0 then -1 else f.[0]
        #else
        stringToSearchIn.IndexOf(charToFind, startIndex, count)
        #endif

    /// Reports the zero-based index of the first occurrence of the specified string in this instance, using StringComparison.Ordinal.
    let (*inline*) indexOfString (stringToFind:string) (stringToSearchIn:string)  =
        if isNull stringToFind then StrException.Raise "Str.indexOfString: stringToFind is null. (stringToSearchIn:%s) " (exnf stringToSearchIn)
        if isNull stringToSearchIn then StrException.Raise "Str.indexOfString: stringToSearchIn is null. (stringToFind:%s) " (exnf stringToFind)
        stringToSearchIn.IndexOf(stringToFind, StringComparison.Ordinal)

    /// Reports the zero-based index of the first occurrence of the specified string in this instance.
    /// The search starts at a specified character position, using StringComparison.Ordinal.
    let (*inline*) indexOfStringFrom (stringToFind:string) (startIndex:int) (stringToSearchIn:string)  =
        if isNull stringToFind then StrException.Raise "Str.indexOfStringFrom: stringToFind is null. (startIndex:%d)  (stringToSearchIn:%s) " startIndex (exnf stringToSearchIn)
        if isNull stringToSearchIn then StrException.Raise "Str.indexOfStringFrom: stringToSearchIn is null. (stringToFind:%s)  (startIndex:%d) " (exnf stringToFind) startIndex
        stringToSearchIn.IndexOf(stringToFind, startIndex,StringComparison.Ordinal)

    /// Reports the zero-based index of the first occurrence of the specified string in this instance.
    /// The search starts at a specified character position and examines a specified number of character positions, using StringComparison.Ordinal.
    /// When used in Fable this use the Knuth-Morris-Pratt algorithm via Str.indicesOf
    let (*inline*) indexOfStringFromFor (stringToFind:string) (startIndex:int) (count:int) (stringToSearchIn:string)  =
        if isNull stringToFind then StrException.Raise "Str.indexOfStringFromFor: stringToFind is null. (startIndex:%d)  (count:%d)  (stringToSearchIn:%s) " startIndex count (exnf stringToSearchIn)
        if isNull stringToSearchIn then StrException.Raise "Str.indexOfStringFromFor: stringToSearchIn is null. (stringToFind:%s)  (startIndex:%d)  (count:%d) " (exnf stringToFind) startIndex count
        // TODO add check that Count and start Index is withIn string
        #if FABLE_COMPILER
        let f = indicesOf (stringToSearchIn, stringToFind, startIndex, count, 1)
        if f.Count = 0 then -1 else f.[0]
        #else
        stringToSearchIn.IndexOf(stringToFind, startIndex, count, StringComparison.Ordinal)
        #endif

    /// Reports the zero-based index of the first occurrence in this instance of any character in a specified array of Unicode characters.
    let (*inline*) indexOfAny (anyOf:char[]) (stringToSearchIn:string)  =
        if isNull stringToSearchIn then StrException.Raise "Str.indexOfAny: stringToSearchIn is null. (anyOf:%A) " anyOf
        stringToSearchIn.IndexOfAny(anyOf)

    /// Reports the zero-based index of the first occurrence in this instance of any character in a specified array of Unicode characters.
    /// The search starts at a specified character position.
    let (*inline*) indexOfAnyFrom (anyOf:char[]) startIndex (stringToSearchIn:string)  =
        if isNull stringToSearchIn then StrException.Raise "Str.indexOfAnyFrom: stringToSearchIn is null. (anyOf:%A)  (startIndex:%d) " anyOf startIndex
        stringToSearchIn.IndexOfAny(anyOf, startIndex)

    /// Reports the zero-based index of the first occurrence in this instance of any character in a specified array of Unicode characters.
    /// The search starts at a specified character position and examines a specified number of character positions.
    let (*inline*) indexOfAnyFromFor (anyOf:char[]) startIndex count (stringToSearchIn:string)  =
        if isNull stringToSearchIn then StrException.Raise "Str.indexOfAnyFromFor: stringToSearchIn is null. (anyOf:%A)  (startIndex:%d)  (count:%d) " anyOf startIndex count
        stringToSearchIn.IndexOfAny(anyOf, startIndex, count)

    /// Returns a new string in which a specified string is inserted at a specified index position in this instance.
    let (*inline*) insert startIndex (stringToInsert:string) (insertIntoThisString:string)  =
        if isNull stringToInsert then StrException.Raise "Str.insert: stringToInsert is null. (startIndex:%d)  (insertIntoThisString:%s) " startIndex (exnf insertIntoThisString)
        if isNull insertIntoThisString then StrException.Raise "Str.insert: insertIntoThisString is null. (startIndex:%d)  (stringToInsert:%s) " startIndex (exnf stringToInsert)
        insertIntoThisString.Insert(startIndex, stringToInsert)

    /// Returns the zero-based index position of the last occurrence of a specified Unicode character within this instance.
    /// Or -1 if not found.
    let (*inline*) lastIndexOfChar (charToFind:char) (stringToSearchIn:string)  =
        if isNull stringToSearchIn then StrException.Raise "Str.lastIndexOfChar: stringToSearchIn is null. (charToFind:'%c') " charToFind
        stringToSearchIn.LastIndexOf(charToFind)

    /// Returns the zero-based index position of the last occurrence of a specified Unicode character within this instance.
    /// The search starts at a specified character position and proceeds backward toward the beginning of the string.
    /// Or -1 if not found.
    let (*inline*) lastIndexOfCharFrom (charToFind:char) startIndex (stringToSearchIn:string) =
        if isNull stringToSearchIn then StrException.Raise "Str.lastIndexOfCharFrom : stringToSearchIn is null. (charToFind:'%c')  (startIndex:%d) " charToFind startIndex
        stringToSearchIn.LastIndexOf(charToFind, startIndex)

    /// Returns the zero-based index position of the last occurrence of a specified string within this instance, using StringComparison.Ordinal.
    /// Or -1 if not found.
    let (*inline*) lastIndexOfString (stringToFind:string) (stringToSearchIn:string)  =
        if isNull stringToFind then StrException.Raise "Str.lastIndexOfString: stringToFind is null. (stringToSearchIn:%s) " (exnf stringToSearchIn)
        if isNull stringToSearchIn then StrException.Raise "Str.lastIndexOfString: stringToSearchIn is null. (stringToFind:%s) " (exnf stringToFind)
        stringToSearchIn.LastIndexOf(stringToFind, StringComparison.Ordinal)

    (*inline*)
    (*inline*)
    (* // TODO implement indicesOf from end searching

    /// Returns the zero-based index position of the last occurrence of the specified Unicode character in a substring within this instance.
    /// The search starts at a specified character position and proceeds backward toward the beginning of the string for a specified number of character positions.
    /// Or -1 if not found.
    let lastIndexOfCharFromFor  (charToFind:char) startIndex count (stringToSearchIn:string)  =
        if isNull stringToSearchIn then StrException.Raise "Str.lastIndexOfCharFromFor: stringToSearchIn is null. (charToFind:'%c')  (startIndex:%d)  (count:%d) " charToFind startIndex count
        stringToSearchIn.LastIndexOf(charToFind, startIndex, count)

    /// Returns the zero-based index position of the last occurrence of a specified string within this instance.
    /// The search starts at a specified character position and proceeds backward toward the beginning of the string for a specified number of character positions,
    /// using StringComparison.Ordinal.
    /// Or -1 if not found.
    let  lastIndexOfStringFromFor  (stringToFind:string) (startIndex:int) (count:int) (stringToSearchIn:string)  =
        if isNull stringToFind then StrException.Raise "Str.lastIndexOfStringFromFor : stringToFind is null. (startIndex:%d)  (count:%d)  (stringToSearchIn:%s) " startIndex count (exnf stringToSearchIn)
        if isNull stringToSearchIn then StrException.Raise "Str.lastIndexOfStringFromFor : stringToSearchIn is null. (stringToFind:%s)  (startIndex:%d)  (count:%d) " (exnf stringToFind) startIndex count
        // TODO add check that Count and start Index is withIn string
        if count < 0 then StrException.Raise "Str.lastIndexOfStringFromFor : count is negative. (stringToFind:%s)  (startIndex:%d)  (count:%d) " (exnf stringToFind) startIndex count
        if startIndex < 0 then StrException.Raise "Str.lastIndexOfStringFromFor : startIndex is negative. (stringToFind:%s)  (startIndex:%d)  (count:%d) " (exnf stringToFind) startIndex count
        if startIndex + count > stringToSearchIn.Length then StrException.Raise "Str.lastIndexOfStringFromFor : startIndex + count is bigger than string length. (stringToFind:%s)  (startIndex:%d)  (count:%d) " (exnf stringToFind) startIndex count
        stringToSearchIn.LastIndexOf(stringToFind, startIndex, count, StringComparison.Ordinal)
    *)

    /// Returns the zero-based index position of the last occurrence of a specified string within this instance.
    /// The search starts at a specified character position and proceeds backward toward the beginning of the string, using StringComparison.Ordinal.
    /// Or -1 if not found.
    let (*inline*) lastIndexOfStringFrom (stringToFind:string) (startIndex:int) (stringToSearchIn:string)  =
        if isNull stringToFind then StrException.Raise "Str.lastIndexOfStringFrom: stringToFind is null. (startIndex:%d)  (stringToSearchIn:%s) " startIndex (exnf stringToSearchIn)
        if isNull stringToSearchIn then StrException.Raise "Str.lastIndexOfString': stringToSearchIn is null. (stringToFind:%s)  (startIndex:%d) " (exnf stringToFind) startIndex
        stringToSearchIn.LastIndexOf(stringToFind, startIndex, StringComparison.Ordinal)


    /// Returns a new string that right-aligns the characters in this instance by padding them with spaces on the left, for a specified total length.
    let (*inline*) padLeft totalWidth (txt:string)  =
        if isNull txt then StrException.Raise "Str.padLeft: txt is null. (totalWidth:%d) " totalWidth
        txt.PadLeft(totalWidth)

    /// Returns a new string that right-aligns the characters in this instance by padding them on the left with a specified Unicode character, for a specified total length.
    let (*inline*) padLeftWith totalWidth (paddingChar:char) (txt:string)  =
        if isNull txt then StrException.Raise "Str.padLeftWith: txt is null. (totalWidth:%d)  (paddingChar:'%c') " totalWidth paddingChar
        txt.PadLeft(totalWidth, paddingChar)

    /// Returns a new string that left-aligns the characters in this string by padding them with spaces on the right, for a specified total length.
    let (*inline*) padRight totalWidth (txt:string)  =
        if isNull txt then StrException.Raise "Str.padRight: txt is null. (totalWidth:%d) " totalWidth
        txt.PadRight(totalWidth)

    /// Returns a new string that left-aligns the characters in this string by padding them on the right with a specified Unicode character, for a specified total length.
    let (*inline*) padRightWith totalWidth (paddingChar:char) (txt:string)  =
        if isNull txt then StrException.Raise "Str.padRightWith: txt is null. (totalWidth:%d)  (paddingChar:'%c') " totalWidth paddingChar
        txt.PadRight(totalWidth, paddingChar)

    /// Returns a new string in which all the characters in the current instance, beginning at a specified position and continuing through the last position, have been deleted.
    let (*inline*) remove startIndex (txt:string)  =
        if isNull txt then StrException.Raise "Str.remove: txt is null. (startIndex:%d) " startIndex
        txt.Remove(startIndex)

    /// Returns a new string in which a specified number of characters in the current instance beginning at a specified position have been deleted.
    let (*inline*) removeFrom startIndex count (txt:string)  =
        if isNull txt then StrException.Raise "Str.removeFrom : txt is null. (startIndex:%d)  (count:%d) " startIndex count
        txt.Remove(startIndex, count)

    /// Returns a new string in which all occurrences of a specified Unicode character in this instance are replaced with another specified Unicode character.
    let (*inline*) replaceChar (oldChar:char) (newChar:char) (txt:string)  =
        if isNull txt then StrException.Raise "Str.replace': txt is null. (oldChar:'%c')  (newChar:'%c') " oldChar newChar
        txt.Replace(oldChar, newChar) // will return the same instance if char to replace is not found

    /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.
    /// (Will return the same instance if text to replace is not found)
    let (*inline*) replace (oldValue:string) (newValue:string) (txt:string)  =
        if isNull oldValue then StrException.Raise "Str.replace: oldValue is null. (newValue:%s)  (txt:%s) " (exnf newValue) (exnf txt)
        if isNull newValue then StrException.Raise "Str.replace: newValue is null. (oldValue:%s)  (txt:%s) " (exnf oldValue) (exnf txt)
        if isNull txt then StrException.Raise "Str.replace: txt is null. (oldValue:%s)  (newValue:%s) " (exnf oldValue) (exnf newValue)
        txt.Replace(oldValue, newValue) // will return the same instance if text to replace is not found

    /// Concatenates string with Environment.NewLine
    let inline concatLines  (lines:string seq) =
        String.concat Environment.NewLine lines

    /// Concatenates string with given separator
    /// Same as FSharp.Core.String.concat
    let inline concat (separator:string) (lines:string seq) =
        String.concat separator lines


    //-----------split by string overloads --------------

    /// Split string by "\r\n", "\r" and "\n"
    let (*inline*) splitLines  (stringToSplit:string) =
        if isNull stringToSplit then StrException.Raise "Str.splitLines: stringToSplit is null"
        stringToSplit.Split( [| "\r\n"; "\r"; "\n" |] , StringSplitOptions.None)


    /// Split string, Remove Empty Entries
    /// Like: string.Split([| splitter |], StringSplitOptions.RemoveEmptyEntries)
    let (*inline*) split (splitter:string) (stringToSplit:string) =
        if isNull stringToSplit then StrException.Raise "Str.split: stringToSplit is null (splitter:%s)" (exnf splitter)
        if isNull splitter      then StrException.Raise "Str.split: splitter is null (stringToSplit:%s)" (exnf stringToSplit)
        stringToSplit.Split([|splitter|], StringSplitOptions.RemoveEmptyEntries)

    /// Split string, Keep Empty Entries
    /// Like : string.Split([| splitter |], StringSplitOptions.None)
    let (*inline*) splitKeep (splitter:string) (stringToSplit:string) =
        if isNull stringToSplit then StrException.Raise "Str.splitKeep: stringToSplit is null (splitter:%s)" (exnf splitter)
        if isNull splitter      then StrException.Raise "Str.splitKeep: splitter is null (stringToSplit:%s)" (exnf stringToSplit)
        stringToSplit.Split([|splitter|], StringSplitOptions.None)

    //-----------split by Char overloads --------------

    /// Split string by a Char, Remove Empty Entries
    /// Like: string.Split([| splitter |], StringSplitOptions.RemoveEmptyEntries)
    let (*inline*) splitChar (separator:char) (stringToSplit:string)  =
        if isNull stringToSplit then StrException.Raise "Str.splitChar: stringToSplit is null. (separator:'%c') " separator
        stringToSplit.Split([| separator|] , StringSplitOptions.RemoveEmptyEntries)

    /// Split string by any of multiple Chars, Remove Empty Entries
    /// Like: string.Split([| splitter |], StringSplitOptions.RemoveEmptyEntries)
    let (*inline*) splitChars(separators:char[]) (stringToSplit:string)  =
        if isNull stringToSplit then StrException.Raise "Str.splitByChars: stringToSplit is null. (separators:%A)  " separators
        stringToSplit.Split(separators, StringSplitOptions.RemoveEmptyEntries)

    /// Split string by any of multiple Chars, Keep Empty Entries
    /// Like : string.Split([| splitter |], StringSplitOptions.None)
    let (*inline*) splitCharKeep (separator:char) (stringToSplit:string)  =
        if isNull stringToSplit then StrException.Raise "Str.splitCharKeep: stringToSplit is null. (separator:'%c') " separator
        stringToSplit.Split(separator)

    /// Split string by a Char, Keep Empty Entries
    /// Like : string.Split([| splitter |], StringSplitOptions.None)
    let (*inline*) splitCharsKeep (separators:char[]) (stringToSplit:string)  =
        if isNull stringToSplit then StrException.Raise "Str.splitCharsKeep: stringToSplit is null. (separators:%A)" separators
        stringToSplit.Split(separators)

    //---------------------------- end split -----------------


    /// Retrieves a substring from this instance. The substring starts at a specified character position and continues to the end of the string.
    let (*inline*) substringFrom startIndex (txt:string)  =
        if isNull txt then StrException.Raise "Str.substringFrom: txt is null. (startIndex:%d) " startIndex
        txt.Substring(startIndex)

    /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
    let (*inline*) substringFromFor startIndex length (txt:string)  =
        if isNull txt then StrException.Raise "Str.substringFromFor : txt is null. (startIndex:%d)  (length:%d) " startIndex length
        txt.Substring(startIndex, length)

    /// Copies the characters in this instance to a Unicode character array.
    let (*inline*) toCharArray (txt:string)  =
        if isNull txt then StrException.Raise "Str.toCharArray: txt is null."
        txt.ToCharArray()

    /// Copies the characters in a specified substring in this instance to a Unicode character array.
    let (*inline*) toCharArrayFromFor startIndex length (txt:string)  =
        if isNull txt then StrException.Raise "Str.toCharArrayFromFor : txt is null. (startIndex:%d)  (length:%d) " startIndex length
        txt.ToCharArray(startIndex, length)

    /// Returns a copy of this String object converted to lowercase using the casing rules of the invariant culture.
    let (*inline*) toLower (txt:string)  =
        if isNull txt then StrException.Raise "Str.toLower: txt is null."
        txt.ToLowerInvariant()

    /// Returns a copy of this String object converted to uppercase using the casing rules of the invariant culture.
    let (*inline*) toUpper (txt:string)  =
        if isNull txt then StrException.Raise "Str.toUpper: txt is null."
        txt.ToUpperInvariant()

    // -------------------------trim family-------------

    /// Removes all leading and trailing white-space characters from the current String object.
    let (*inline*) trim (txt:string)  =
        if isNull txt then StrException.Raise "Str.trim: txt is null."
        txt.Trim()

    /// Removes all leading and trailing occurrences of a set of characters specified in an array from the current String object.
    let (*inline*) trimChar (trimChar:char) (txt:string)  =
        if isNull txt then StrException.Raise "Str.trimChar: txt is null."
        txt.Trim([|trimChar|])

    /// Removes all leading and trailing occurrences of a set of characters specified in an array from the current String object.
    let (*inline*) trimChars (trimChars:char[]) (txt:string)  =
        if isNull txt then StrException.Raise "Str.trimChars: txt is null. (trimChars:%A) " trimChars
        txt.Trim(trimChars)

    /// Removes all trailing whitespace.
    let (*inline*) trimEnd (txt:string)  =
        if isNull txt then StrException.Raise "Str.trimEnd: txt is null. (trimChars:%A) " trimChars
        txt.TrimEnd()

    /// Removes all trailing occurrences of a  characters specified in an array from the current String object.
    let (*inline*) trimEndChar (trimChar:char) (txt:string)  =
        if isNull txt then StrException.Raise "Str.trimEndChar: txt is null. (trimChars:%A) " trimChars
        txt.TrimEnd([|trimChar|])

    /// Removes all trailing occurrences of a set of characters specified in an array from the current String object.
    let (*inline*) trimEndChars (trimChars:char[]) (txt:string)  =
        if isNull txt then StrException.Raise "Str.trimEndChars: txt is null. (trimChars:%A) " trimChars
        txt.TrimEnd(trimChars)

    /// Removes all leading whitespace.
    let (*inline*) trimStart (txt:string)  =
        if isNull txt then StrException.Raise "Str.trimStart: txt is null. (trimChars:%A) " trimChars
        txt.TrimStart()

    /// Removes all leading occurrences of a characters specified in an array from the current String object.
    let (*inline*) trimStartChar (trimChar:char) (txt:string)  =
        if isNull txt then StrException.Raise "Str.trimStartChar: txt is null. (trimChars:%A) " trimChars
        txt.TrimStart(trimChar)

    /// Removes all leading occurrences of a set of characters specified in an array from the current String object.
    let (*inline*) trimStartChars (trimChars:char[]) (txt:string)  =
        if isNull txt then StrException.Raise "Str.trimStartChars: txt is null. (trimChars:%A) " trimChars
        txt.TrimStart(trimChars)


    /// Removes accents & diacritics from characters
    /// first does txt.Normalize(System.Text.NormalizationForm.FormD)
    /// and then removes all non-spacing marks
    /// eventually returns string.Normalize(NormalizationForm.FormC)
    /// (in Fable this just call txt.normalize("NFKD") instead)
    let normalize (txt:string ) =
        if isNull txt then StrException.Raise "Str.normalize: txt is null"
        #if FABLE_COMPILER
        //https://stackoverflow.com/a/37511463/969070:
        let n :string = txt?normalize("NFD")
        emitJsExpr n """$0.replace(/\p{Diacritic}/gu, "")"""
        #else
        // better: https://github.com/apache/lucenenet/blob/master/src/Lucene.Net.Analysis.Common/Analysis/Miscellaneous/ASCIIFoldingFilter.cs
        // https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
        txt.Normalize(System.Text.NormalizationForm.FormD)
        |> Seq.filter ( fun c -> Globalization.CharUnicodeInfo.GetUnicodeCategory(c) <> Globalization.UnicodeCategory.NonSpacingMark  )
        |> String.Concat
        |> fun s -> s.Normalize(NormalizationForm.FormC)
        #endif