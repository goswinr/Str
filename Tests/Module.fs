namespace Tests

open Str

open Scriptorium.Nib.Assertion
open type Scriptorium.Quill.Test

open System

module Module =


 let tests =
  testList ("Module.fs Tests", [

        test ("indicesOf with pattern found", fun _ ->
            let text = "abababab"
            let pattern = "abab"
            let result = Str.indicesOf (text, pattern, 0, text.Length, 10) |> Array.ofSeq
            assertThat result (tag "Expected pattern to be found at indices [0; 2; 4]" >> isEqualTo [|0; 2; 4|])
        )

        test ("indicesOf with pattern found2", fun _ ->
            let text = "abab0abab0"
            let pattern = "abab"
            let result = Str.indicesOf (text, pattern, 0, text.Length, 10) |> Array.ofSeq
            assertThat result (tag "Expected pattern to be found at indices [0; 5]" >> isEqualTo [|0; 5|])
        )

        test ("Str.indicesOf with pattern not found", fun _ ->
            let text = "Hello, world!"
            let pattern = "xyz"
            let result = Str.indicesOf (text, pattern, 0, text.Length, 10)|> Array.ofSeq
            assertThat result (tag "Expected pattern to not be found" >> isEqualTo [||])
        )

        test ("Str.indicesOf with null pattern", fun _ ->
            let text = "Hello, world!"
            let testFunc = fun () -> Str.indicesOf (text, null, 0, text.Length, 10) |> ignore
            assertThat testFunc (tag "Expected an exception when pattern is null" >> throws)
        )

        test ("Str.indicesOf with null text", fun _ ->
            let pattern = "Hello"
            let testFunc = fun () -> Str.indicesOf (null, pattern, 0, 5, 10) |> ignore
            assertThat testFunc (tag "Expected an exception when text is null" >> throws)
        )

        test ("Str.indicesOf with negative searchFromIdx", fun _ ->
            let text = "Hello, world!"
            let pattern = "Hello"
            let testFunc = fun () -> Str.indicesOf (text, pattern, -1, text.Length, 10) |> ignore
            assertThat testFunc (tag "Expected an exception when searchFromIdx is negative" >> throws)
        )

        test ("Str.indicesOf with negative searchLength", fun _ ->
            let text = "Hello, world!"
            let pattern = "Hello"
            let testFunc = fun () -> Str.indicesOf (text, pattern, 0, -1, 10) |> ignore
            assertThat testFunc (tag "Expected an exception when searchLength is negative" >> throws)
        )

        test ("Str.indicesOf with searchFromIdx + searchLength > text.Length", fun _ ->
            let text = "Hello, world!"
            let pattern = "Hello"
            let testFunc = fun () -> Str.indicesOf (text, pattern, 0, text.Length + 1, 10) |> ignore
            assertThat testFunc (tag "Expected an exception when searchFromIdx + searchLength > text.Length" >> throws)
        )

        test ("Str.indicesOf respects a restricted range for a full-text pattern", fun _ ->
            let result = Str.indicesOf ("abc", "abc", 1, 2, 10) |> Array.ofSeq
            assertThat result (tag "Index 0 is outside the requested range" >> isEqualTo [||])
        )

        test ("Str.indicesOf returns no matches for a zero-length search range", fun _ ->
            let result = Str.indicesOf ("abc", "a", 0, 0, 10) |> Array.ofSeq
            assertThat result (tag "A zero-length range cannot contain a non-empty pattern" >> isEqualTo [||])
        )

        test ("Str.indicesOf rejects a negative match limit", fun _ ->
            assertThat (fun () -> Str.indicesOf ("abc", "a", 0, 3, -1) |> ignore) (tag "Expected a negative match limit to throw" >> throws)
        )

        test ("truncate should return the truncated string", fun _ ->
            let result = Str.truncate 5 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello")
        )

        test ("truncate should return the same string if length is greater than string length", fun _ ->
            let result = Str.truncate 50 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("skip should return the string after skipping the specified length", fun _ ->
            let result = Str.skip 5 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo ", World!")
        )

        test ("take should return the string of the specified length", fun _ ->
            let result = Str.take 5 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello")
        )

        test ("delete should return the string after deleting the specified text", fun _ ->
            let result = Str.delete "Hello" "Hello, World!Hello"
            assertThat result (tag "Should be equal" >> isEqualTo ", World!")
        )

        test ("deleteChar should return the string after deleting the specified char", fun _ ->
            let result = Str.deleteChar 'H' "HelloH, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "ello, World!")
        )

        test ("truncate should return an empty string if length is 0", fun _ ->
            let result = Str.truncate 0 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "")
        )

        test ("truncate should throw an exception if length is negative", fun _ ->
            assertThat (fun _ -> Str.truncate -5 "Hello, World!"  |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("skip should return an empty string if length is equal to string length", fun _ ->
            let result = Str.skip 13 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "")
        )

        test ("skip should throw an exception if length is greater than string length", fun _ ->
            assertThat (fun _ -> Str.skip 50 "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("take should return an empty string if length is 0", fun _ ->
            let result = Str.take 0 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "")
        )

        test ("take should throw an exception if length is negative", fun _ ->
            assertThat (fun _ -> Str.take -5 "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("delete should return the same string if text is not found", fun _ ->
            let result = Str.delete "Goodbye" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("deleteChar should return the same string if char is not found", fun _ ->
            let result = Str.deleteChar 'Z' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("delete should return the same string if text is empty", fun _ ->
            let result = Str.delete "" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("delete should throw an exception if text is null", fun _ ->
            assertThat (fun _ -> Str.delete null "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("deleteChar should return the same string if char is null", fun _ ->
            let result = Str.deleteChar '@' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("truncate should return the same string if length is equal to string length", fun _ ->
            let result = Str.truncate 13 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("take should return the same string if length is equal to string length", fun _ ->
            let result = Str.take 12 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World")
        )

        test ("unifyLineEndings should replace all line endings with System.Environment.NewLine", fun _ ->
            let result = Str.unifyLineEndings "Hello\r\nWorld!\rHello\nWorld!"
            assertThat result (tag "Should be equal" >> isEqualTo ("Hello" + Environment.NewLine + "World!" + Environment.NewLine + "Hello" + Environment.NewLine + "World!"))
        )

        test ("unifyLineEndings should throw an exception if input is null", fun _ ->
            assertThat (fun _ -> Str.unifyLineEndings null |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("before should return the string before the splitter", fun _ ->
            let result = Str.before "," "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello")
        )

        test ("before should throw an exception if splitter is not found", fun _ ->
            assertThat (fun _ -> Str.before "Z" "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("before should throw an exception if stringToSearchIn is null", fun _ ->
            assertThat (fun _ -> Str.before "," null |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("before should throw an exception if splitter is null", fun _ ->
            assertThat (fun _ -> Str.before null "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("beforeChar should return the string before the splitter", fun _ ->
            let result = Str.beforeChar ',' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello")
        )

        test ("beforeChar should throw an exception if splitter is not found", fun _ ->
            assertThat (fun _ -> Str.beforeChar 'Z' "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("tryBefore should return Some string before the splitter", fun _ ->
            let result = Str.tryBefore "," "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo (Some "Hello"))
        )

        test ("tryBefore should return None if splitter is not found", fun _ ->
            let result = Str.tryBefore "Z" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo None)
        )

        test ("tryBeforeChar should return Some string before the splitter", fun _ ->
            let result = Str.tryBeforeChar ',' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo (Some "Hello"))
        )

        test ("tryBeforeChar should return None if splitter is not found", fun _ ->
            let result = Str.tryBeforeChar 'Z' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo None)
        )

        test ("beforeOrInput should return the string before the splitter", fun _ ->
            let result = Str.beforeOrInput "," "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello")
        )

        test ("beforeOrInput should return the input string if splitter is not found", fun _ ->
            let result = Str.beforeOrInput "Z" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("beforeCharOrInput should return the string before the splitter", fun _ ->
            let result = Str.beforeCharOrInput ',' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello")
        )

        test ("beforeCharOrInput should return the input string if splitter is not found", fun _ ->
            let result = Str.beforeCharOrInput 'Z' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("after should return the string after the splitter", fun _ ->
            let result = Str.after "," "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo " World!")
        )

        test ("after should throw an exception if splitter is not found", fun _ ->
            assertThat (fun _ -> Str.after "Z" "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("afterChar should return the string after the splitter", fun _ ->
            let result = Str.afterChar ',' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo " World!")
        )

        test ("afterChar should throw an exception if splitter is not found", fun _ ->
            assertThat (fun _ -> Str.afterChar 'Z' "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("tryAfter should return Some string after the splitter", fun _ ->
            let result = Str.tryAfter "," "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo (Some " World!"))
        )

        test ("tryAfter should return None if splitter is not found", fun _ ->
            let result = Str.tryAfter "Z" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo None)
        )

        test ("tryAfterChar should return Some string after the splitter", fun _ ->
            let result = Str.tryAfterChar ',' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo (Some " World!"))
        )

        test ("tryAfterChar should return None if splitter is not found", fun _ ->
            let result = Str.tryAfterChar 'Z' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo None)
        )


        test ("afterOrInput should return the string after the splitter", fun _ ->
            let result = Str.afterOrInput "," "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo " World!")
        )

        test ("afterOrInput should return the input string if splitter is not found", fun _ ->
            let result = Str.afterOrInput "Z" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("afterCharOrInput should return the string after the splitter", fun _ ->
            let result = Str.afterCharOrInput ',' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo " World!")
        )

        test ("afterCharOrInput should return the input string if splitter is not found", fun _ ->
            let result = Str.afterCharOrInput 'Z' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("between should return the string between the splitters", fun _ ->
            let result = Str.between "," "!" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo " World")
        )

        test ("between should throw an exception if splitters are not found", fun _ ->
            assertThat (fun _ -> Str.between "Z" "Y" "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("tryBetween should return Some string between the splitters", fun _ ->
            let result = Str.tryBetween "," "!" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo (Some " World"))
        )

        test ("tryBetween should return None if splitters are not found", fun _ ->
            let result = Str.tryBetween "Z" "Y" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo None)
        )

        test ("betweenOrInput should return the string between the splitters", fun _ ->
            let result = Str.betweenOrInput "," "!" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo " World")
        )

        test ("betweenOrInput should return the input string if splitters are not found", fun _ ->
            let result = Str.betweenOrInput "Z" "Y" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        test ("betweenChars should return the string between the splitters", fun _ ->
            let result = Str.betweenChars ',' '!' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo " World")
        )

        test ("betweenChars should throw an exception if splitters are not found", fun _ ->
            assertThat (fun _ -> Str.betweenChars 'Z' 'Y' "Hello, World!" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        test ("tryBetweenChars should return Some string between the splitters", fun _ ->
            let result = Str.tryBetweenChars ',' '!' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo (Some " World"))
        )

        test ("tryBetweenChars should return None if splitters are not found", fun _ ->
            let result = Str.tryBetweenChars 'Z' 'Y' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo None)
        )

        test ("betweenCharsOrInput should return the string between the splitters", fun _ ->
            let result = Str.betweenCharsOrInput ',' '!' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo " World")
        )

        test ("betweenCharsOrInput should return the input string if splitters are not found", fun _ ->
            let result = Str.betweenCharsOrInput 'Z' 'Y' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )


        test ("splitOnce should split the string once at the splitter", fun _ ->
            let result = Str.splitOnce "," "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo ("Hello", " World!"))
        )

        test ("splitOnce should throw an exception if splitter is not found", fun _ ->
            assertThat (fun _ -> Str.splitOnce "Z" "Hello, World!" |> ignore<string*string>) (tag "Should throw an exception" >> throws)
        )

        test ("splitOnce should throw an exception if stringToSplit is null", fun _ ->
            assertThat (fun _ -> Str.splitOnce "," null |> ignore<string*string>) (tag "Should throw an exception" >> throws)
        )

        test ("splitOnce should throw an exception if splitter is null", fun _ ->
            assertThat (fun _ -> Str.splitOnce null "Hello, World!" |> ignore<string*string>) (tag "Should throw an exception" >> throws)
        )


        test ("splitOnce should return the input string and an empty string if splitter is at the end", fun _ ->
            let result = Str.splitOnce "," "Hello,"
            assertThat result (tag "Should be equal" >> isEqualTo ("Hello", ""))
        )

        test ("splitOnce should return an empty string and the input string if splitter is at the start", fun _ ->
            let result = Str.splitOnce "," ",World!"
            assertThat result (tag "Should be equal" >> isEqualTo ("", "World!"))
        )

        test ("splitOnce should return two empty strings if input string is the splitter", fun _ ->
            let result = Str.splitOnce "," ","
            assertThat result (tag "Should be equal" >> isEqualTo ("", ""))
        )

        test ("splitOnce should throw an exception if input string is empty", fun _ ->
            assertThat (fun _ -> Str.splitOnce "," "" |> ignore<string*string>) (tag "Should throw an exception" >> throws)
        )


        test ("splitTwice should split the string twice at the splitters", fun _ ->
            let result = Str.splitTwice "X" "T" "cXabTk"
            assertThat result (tag "Should be equal" >> isEqualTo ("c", "ab", "k"))
        )

        test ("splitTwice should throw an exception if firstSplitter is not found", fun _ ->
            assertThat (fun _ -> Str.splitTwice "Z" "T" "cXabTk" |> ignore<string*string*string>) (tag "Should throw an exception" >> throws)
        )

        test ("splitTwice should throw an exception if secondSplitter is not found", fun _ ->
            assertThat (fun _ -> Str.splitTwice "X" "Z" "cXabTk" |> ignore<string*string*string>) (tag "Should throw an exception" >> throws)
        )

        test ("splitTwice should throw an exception if stringToSplit is null", fun _ ->
            assertThat (fun _ -> Str.splitTwice "X" "T" null |> ignore<string*string*string>) (tag "Should throw an exception" >> throws)
        )

        test ("splitTwice should throw an exception if firstSplitter is null", fun _ ->
            assertThat (fun _ -> Str.splitTwice null "T" "cXabTk" |> ignore<string*string*string>) (tag "Should throw an exception" >> throws)
        )

        test ("splitTwice should throw an exception if secondSplitter is null", fun _ ->
            assertThat (fun _ -> Str.splitTwice "X" null "cXabTk" |> ignore<string*string*string>) (tag "Should throw an exception" >> throws)
        )


        test ("between should return the input string if splitters are at the start and end", fun _ ->
            let result = Str.between "," "!" ",Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World")
        )

        test ("between should return an empty string if input string is the splitters", fun _ ->
            let result = Str.between "," "!" ",!"
            assertThat result (tag "Should be equal" >> isEqualTo "")
        )

        test ("between should throw an exception if input string is empty", fun _ ->
            assertThat (fun _ -> Str.between "," "!" "" |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        // trySplitOnce
        test ("trySplitOnce should return Some tuple if splitter is found", fun _ ->
            let result = Str.trySplitOnce "," "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo (Some ("Hello", " World!")))
        )

        test ("trySplitOnce should return None if splitter is not found", fun _ ->
            let result = Str.trySplitOnce "Z" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo None)
        )

        // trySplitTwice
        test ("trySplitTwice should return Some tuple if splitters are found", fun _ ->
            let result = Str.trySplitTwice "H" "o" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo (Some ("", "ell", ", World!")))
        )

        test ("trySplitTwice should return None if splitters are not found", fun _ ->
            let result = Str.trySplitTwice "Z" "Y" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo None)
        )

        // up1
        test ("up1 should return the string with the first character uppercased", fun _ ->
            let result = Str.up1 "hello"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello")
        )

        // low1
        test ("low1 should return the string with the first character lowercased", fun _ ->
            let result = Str.low1 "HELLO"
            assertThat result (tag "Should be equal" >> isEqualTo "hELLO")
        )

        // slice
        test ("slice should return a substring from the start index to the end index", fun _ ->
            let result = Str.slice 0 5 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello,")
        )

        // slice
        test ("neg slice should return a substring from the start index to the end index", fun _ ->
            let result = Str.slice -3 -1 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "ld!")
        )

        test ("slice with negative start and end as in README", fun _ ->
            let result = Str.slice -6 -2 "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "World")
        )

        test ("slice error for out of range end index reports the end index", fun _ ->
            let msg = try Str.slice 0 10 "abc" |> ignore; "" with e -> e.Message
            assertThat (msg.Contains "End index 10") (tag $"Message should name end index 10, got: {msg}" >> isTrue)
        )


        // countSubString
        test ("countSubString should return the number of occurrences of the substring", fun _ ->
            let result = Str.countSubString "l" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo 3)
        )

        test ("countSubString counts non-overlapping occurrences", fun _ ->
            let result = Str.countSubString "aa" "aaaaa"
            assertThat result (tag "Expected two non-overlapping occurrences" >> isEqualTo 2)
        )

        test ("countSubString rejects an empty substring", fun _ ->
            assertThat (fun () -> Str.countSubString "" "abc" |> ignore) (tag "Expected an empty substring to throw" >> throws)
        )

        // countChar
        test ("countChar should return the number of occurrences of the character", fun _ ->
            let result = Str.countChar 'l' "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo 3)
        )




        // addSuffix
        test ("addSuffix should add the suffix to the string", fun _ ->
            let result = Str.addSuffix " World!" "Hello,"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        // addPrefix
        test ("addPrefix should add the prefix to the string", fun _ ->
            let result = Str.addPrefix "Hello, " "World!"
            assertThat result (tag "Should be equal" >> isEqualTo "Hello, World!")
        )

        // inQuotes
        test ("inQuotes should add double quotes at the start and end of the string", fun _ ->
            let result = Str.inQuotes "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "\"Hello, World!\"")
        )

        // inSingleQuotes
        test ("inSingleQuotes should add single quotes at the start and end of the string", fun _ ->
            let result = Str.inSingleQuotes "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo "'Hello, World!'")
        )

        // contains
        test ("contains should return true if the string contains the substring", fun _ ->
            let result = Str.contains "World" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // containsIgnoreCase
        test ("containsIgnoreCase should return true if the string contains the substring, ignoring case", fun _ ->
            let result = Str.containsIgnoreCase "world" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // containsIgnoreCase
        test ("containsIgnoreCase should return false if the string not contains the substring, ignoring case", fun _ ->
            let result = Str.containsIgnoreCase "worl1" "Hello, World!"
            assertThat result (tag "Should be false" >> isFalse)
        )

        // notContains
        test ("notContains should return true if the string does not contain the substring", fun _ ->
            let result = Str.notContains "Universe" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // containsChar
        test ("containsChar should return true if the string contains the character", fun _ ->
            let result = Str.containsChar 'H' "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // notContainsChar
        test ("notContainsChar should return true if the string does not contain the character", fun _ ->
            let result = Str.notContainsChar 'Z' "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // compare
        test ("compare should return 0 if the strings are equal", fun _ ->
            let result = Str.compare "Hello, World!" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo 0)
        )

        // compareIgnoreCase
        test ("compareIgnoreCase should return 0 if the strings are equal, ignoring case", fun _ ->
            let result = Str.compareIgnoreCase "hello, world!" "Hello, World!"
            assertThat result (tag "Should be equal" >> isEqualTo 0)
        )

        // compareIgnoreCase
        test ("compareIgnoreCase should not return 0 if the strings are not equal, ignoring case", fun _ ->
            let result = Str.compareIgnoreCase "hello,world!" "Hello, World!"
            assertThat (result= 0) (tag "Should not be equal" >> isFalse)
        )

        // endsWith
        test ("endsWith should return true if the string ends with the substring", fun _ ->
            let result = Str.endsWith "World!" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // endsWithIgnoreCase
        test ("endsWithIgnoreCase should return true if the string ends with the substring, ignoring case", fun _ ->
            let result = Str.endsWithIgnoreCase "world!" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // startsWith
        test ("startsWith should return true if the string starts with the substring", fun _ ->
            let result = Str.startsWith "Hello" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // startsWithIgnoreCase
        test ("startsWithIgnoreCase should return true if the string starts with the substring, ignoring case-3", fun _ ->
            let result = Str.startsWithIgnoreCase "hello" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // equals
        test ("equals should return true if the strings are equal", fun _ ->
            let result = Str.equals "Hello, World!" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // equals
        test ("equals should return false if the strings are not equal", fun _ ->
            let result = Str.equals "Hello, world!" "Hello, World!"
            assertThat result (tag "Should be false" >> isFalse)
        )

        // countChar
        test ("countChar should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.countChar 'l' null |> ignore<int>) (tag "Should throw an exception" >> throws)
        )



        // addSuffix
        test ("addSuffix should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.addSuffix " World!" null |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        // addPrefix
        test ("addPrefix should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.addPrefix "Hello, " null |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        // inQuotes
        test ("inQuotes should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.inQuotes null |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        // inSingleQuotes
        test ("inSingleQuotes should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.inSingleQuotes null |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        // contains
        test ("contains should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.contains "World" null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )

        // containsIgnoreCase
        test ("containsIgnoreCase should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.containsIgnoreCase "world" null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )

        // notContains
        test ("notContains should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.notContains "Universe" null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )

        // containsChar
        test ("containsChar should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.containsChar 'H' null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )

        // notContainsChar
        test ("notContainsChar should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.notContainsChar 'Z' null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )

        // compare
        test ("compare should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.compare "Hello, World!" null |> ignore<int>) (tag "Should throw an exception" >> throws)
        )

        // compareIgnoreCase
        test ("compareIgnoreCase should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.compareIgnoreCase "hello, world!" null |> ignore<int>) (tag "Should throw an exception" >> throws)
        )

        // endsWith
        test ("endsWith should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.endsWith "World!" null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )

        // endsWithIgnoreCase
        test ("endsWithIgnoreCase should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.endsWithIgnoreCase "world!" null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )

        // startsWith
        test ("startsWith should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.startsWith "Hello" null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )

        // startsWithIgnoreCase
        test ("startsWithIgnoreCase should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.startsWithIgnoreCase "hello" null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )

        // equals
        test ("equals should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.equals "Hello, World!" null |> ignore<bool>) (tag "Should throw an exception" >> throws)
        )



        // equalsIgnoreCase
        test ("equalsIgnoreCase should handle null input", fun _ ->
            assertThat (fun _ -> Str.equalsIgnoreCase null "Hello"  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // indexOfChar
        test ("indexOfChar should handle null input", fun _ ->
            assertThat (fun _ -> Str.indexOfChar 'H' null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // indexOfCharFrom
        test ("indexOfCharFrom should handle null input", fun _ ->
            assertThat (fun _ -> Str.indexOfCharFrom 'H' 0 null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // indexOfCharFromFor
        test ("indexOfCharFromFor should handle null input", fun _ ->
            assertThat (fun _ -> Str.indexOfCharFromFor 'H' 0 1 null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // indexOfString
        test ("indexOfString should handle null input", fun _ ->
            assertThat (fun _ -> Str.indexOfString null "Hello"  |> ignore  ) (tag "Should throw exception" >> throws)
            assertThat (fun _ -> Str.indexOfString "Hello" null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // indexOfStringFrom
        test ("indexOfStringFrom should handle null input", fun _ ->
            assertThat (fun _ -> Str.indexOfStringFrom null 0 "Hello"  |> ignore  ) (tag "Should throw exception" >> throws)
            assertThat (fun _ -> Str.indexOfStringFrom "Hello" 0 null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // indexOfStringFromFor
        test ("indexOfStringFromFor should handle null input", fun _ ->
            assertThat (fun _ -> Str.indexOfStringFromFor null 0 1 "Hello"  |> ignore  ) (tag "Should throw exception" >> throws)
            assertThat (fun _ -> Str.indexOfStringFromFor "Hello" 0 1 null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // indexOfAny
        test ("indexOfAny should handle null input", fun _ ->
            assertThat (fun _ -> Str.indexOfAny [|'H'|] null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // indexOfAnyFrom
        test ("indexOfAnyFrom should handle null input", fun _ ->
            assertThat (fun _ -> Str.indexOfAnyFrom [|'H'|] 0 null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // indexOfAnyFromFor
        test ("indexOfAnyFromFor should handle null input", fun _ ->
            assertThat (fun _ -> Str.indexOfAnyFromFor [|'H'|] 0 1 null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // insert
        test ("insert should handle null input", fun _ ->
            assertThat (fun _ -> Str.insert 0 null "Hello"  |> ignore  ) (tag "Should throw exception" >> throws)
            assertThat (fun _ -> Str.insert 0 "Hello" null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // lastIndexOfChar
        test ("lastIndexOfChar should handle null input", fun _ ->
            assertThat (fun _ -> Str.lastIndexOfChar 'H' null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // lastIndexOfCharFrom
        test ("lastIndexOfCharFrom should handle null input", fun _ ->
            assertThat (fun _ -> Str.lastIndexOfCharFrom 'H' 0 null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // lastIndexOfCharFromFor
        // test ("lastIndexOfCharFromFor should handle null input", fun _ ->
        //     assertThat (fun _ -> Str.lastIndexOfCharFromFor 'H' 0 1 null  |> ignore  ) (tag "Should throw exception" >> throws)
        // )

        // lastIndexOfString
        test ("lastIndexOfString should handle null input", fun _ ->
            assertThat (fun _ -> Str.lastIndexOfString null "Hello"  |> ignore  ) (tag "Should throw exception" >> throws)
            assertThat (fun _ -> Str.lastIndexOfString "Hello" null  |> ignore  ) (tag "Should throw exception" >> throws)
        )

        // startsWith
        test ("startsWith should return true if the string starts with the substring,2", fun _ ->
            let result = Str.startsWith "Hello, World!" "Hello"
            assertThat result (tag "Should be false" >> isFalse)
        )

        // startsWithIgnoreCase
        test ("startsWithIgnoreCase should return true if the string starts with the substring, ignoring case", fun _ ->
            let result = Str.startsWithIgnoreCase "hell" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // equals
        test ("equals should return true if the strings are equal -3", fun _ ->
            let result = Str.equals "Hello, World!" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // equals
        test ("equals should return false if the strings are not equal2", fun _ ->
            let result = Str.equals "Hello, world!" "Hello, World!"
            assertThat result (tag "Should be false" >> isFalse)
        )

        // countChar
        test ("countChar should return the correct count", fun _ ->
            let result = Str.countChar 'l' "Hello, World!"
            assertThat result (tag "Should be 3" >> isEqualTo 3)
        )



        // addSuffix
        test ("addSuffix should return the string with the suffix added", fun _ ->
            let result = Str.addSuffix " World!" "Hello,"
            assertThat result (tag "Should be 'Hello, World!'" >> isEqualTo "Hello, World!")
        )

        // addPrefix
        test ("addPrefix should return the string with the prefix added", fun _ ->
            let result = Str.addPrefix "Hello, " "World!"
            assertThat result (tag "Should be 'Hello, World!'" >> isEqualTo "Hello, World!")
        )

        // inQuotes
        test ("inQuotes should return the string in quotes", fun _ ->
            let result = Str.inQuotes "Hello, World!"
            assertThat result (tag "Should be '\"Hello, World!\"'" >> isEqualTo "\"Hello, World!\"")
        )

        // inSingleQuotes
        test ("inSingleQuotes should return the string in single quotes", fun _ ->
            let result = Str.inSingleQuotes "Hello, World!"
            assertThat result (tag "Should be ''Hello, World!''" >> isEqualTo "'Hello, World!'")
        )



        // contains
        test ("contains should return true if the string contains the substring -2", fun _ ->
            let result = Str.contains "World" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // containsIgnoreCase
        test ("containsIgnoreCase should return true if the string contains the substring, ignoring case -2", fun _ ->
            let result = Str.containsIgnoreCase "world" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // notContains
        test ("notContains should return true if the string does not contain the substring -2", fun _ ->
            let result = Str.notContains "Universe" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // containsChar
        test ("containsChar should return true if the string contains the character -2", fun _ ->
            let result = Str.containsChar 'H' "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // notContainsChar
        test ("notContainsChar should return true if the string does not contain the character -2", fun _ ->
            let result = Str.notContainsChar 'Z' "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // compare
        test ("compare should return 0 if the strings are equal -2", fun _ ->
            let result = Str.compare "Hello, World!" "Hello, World!"
            assertThat result (tag "Should be 0" >> isEqualTo 0)
        )

        // compareIgnoreCase
        test ("compareIgnoreCase should return 0 if the strings are equal, ignoring case -2", fun _ ->
            let result = Str.compareIgnoreCase "hello, world!" "Hello, World!"
            assertThat result (tag "Should be 0" >> isEqualTo 0)
        )

        // endsWith
        test ("endsWith should return true if the string ends with the substring -2", fun _ ->
            let result = Str.endsWith "World!" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // endsWithIgnoreCase
        test ("endsWithIgnoreCase should return true if the string ends with the substring, ignoring case -2", fun _ ->
            let result = Str.endsWithIgnoreCase "world!" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // startsWith
        test ("startsWith should return true if the string starts with the substring -2", fun _ ->
            let result = Str.startsWith "Hello" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // startsWithIgnoreCase
        test ("startsWithIgnoreCase should return true if the string starts with the substring, ignoring case -2", fun _ ->
            let result = Str.startsWithIgnoreCase "hello" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        // equals
        test ("equals should return true if the strings are equal -2", fun _ ->
            let result = Str.equals "Hello, World!" "Hello, World!"
            assertThat result (tag "Should be true" >> isTrue)
        )

        test ("Test equalsIgnoreCase function", fun _ ->
            let result1 = Str.equalsIgnoreCase "test" "TEST"
            let result2 = Str.equalsIgnoreCase "test" "Test1"
            assertThat result1 (tag "Should be true" >> isTrue)
            assertThat result2 (tag "Should be false" >> isFalse)
        )

        test ("Test indexOfChar function", fun _ ->
            let result1 = Str.indexOfChar 'e' "test"
            let result2 = Str.indexOfChar 'z' "test"
            assertThat result1 (tag "Should be 1" >> isEqualTo 1)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test indexOfCharFrom function", fun _ ->
            let result1 = Str.indexOfCharFrom 's' 1 "test"
            let result2 = Str.indexOfCharFrom 'z' 1 "test"
            assertThat result1 (tag "Should be 2" >> isEqualTo 2)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test indexOfCharFromFor function", fun _ ->
            let result1 = Str.indexOfCharFromFor 's' 1 2 "test"
            let result2 = Str.indexOfCharFromFor 'z' 1 2 "test"
            assertThat result1 (tag "Should be 2" >> isEqualTo 2)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test indexOfString function", fun _ ->
            let result1 = Str.indexOfString "es" "test"
            let result2 = Str.indexOfString "zz" "test"
            assertThat result1 (tag "Should be 1" >> isEqualTo 1)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test indexOfStringFrom function", fun _ ->
            let result1 = Str.indexOfStringFrom "es" 1 "test"
            let result2 = Str.indexOfStringFrom "zz" 1 "test"
            assertThat result1 (tag "Should be 1" >> isEqualTo 1)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test indexOfStringFromFor function", fun _ ->
            let result1 = Str.indexOfStringFromFor "es" 1 3 "ttesttt"
            let result2 = Str.indexOfStringFromFor "tt" 1 3 "testtt"
            let result3 = Str.indexOfStringFromFor "zz" 1 3 "zzszzz"
            assertThat result1 (tag "Should be 2" >> isEqualTo 2)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
            assertThat result3 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test indexOfAny function", fun _ ->
            let result1 = Str.indexOfAny [|'e'; 's'|] "test"
            let result2 = Str.indexOfAny [|'z'; 'x'|] "test"
            assertThat result1 (tag "Should be 1" >> isEqualTo 1)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test indexOfAnyFrom function", fun _ ->
            let result1 = Str.indexOfAnyFrom [|'e'; 's'|] 1 "test"
            let result2 = Str.indexOfAnyFrom [|'z'; 'x'|] 1 "test"
            assertThat result1 (tag "Should be 1" >> isEqualTo 1)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test indexOfAnyFromFor function", fun _ ->
            let result1 = Str.indexOfAnyFromFor [|'e'; 's'|] 1 2 "test"
            let result2 = Str.indexOfAnyFromFor [|'z'; 'x'|] 1 2 "test"
            assertThat result1 (tag "Should be 1" >> isEqualTo 1)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test insert function", fun _ ->
            let result1 = Str.insert 1 "es" "tt"
            let result2 = Str.insert 1 "zz" "tst"
            assertThat result1 (tag "Should be 'test'" >> isEqualTo "test")
            assertThat result2 (tag "Should be 'tzzst'" >> isEqualTo "tzzst")
        )

        test ("Test lastIndexOfChar function", fun _ ->
            let result1 = Str.lastIndexOfChar 't' "test"
            let result2 = Str.lastIndexOfChar 'z' "test"
            assertThat result1 (tag "Should be 3" >> isEqualTo 3)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test lastIndexOfCharFrom function", fun _ ->
            let result1 = Str.lastIndexOfCharFrom 't' 2 "test"
            let result2 = Str.lastIndexOfCharFrom 'z' 2 "test"
            assertThat result1 (tag "Should be 0" >> isEqualTo 0)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        // test ("Test lastIndexOfCharFromFor function", fun _ ->
        //     let result1 = Str.lastIndexOfCharFromFor 't' 2 3 "test"
        //     let result2 = Str.lastIndexOfCharFromFor 'z' 2 3 "test"
        //     assertThat result1 (tag "Should be 0" >> isEqualTo 0)
        //     assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        // )

        test ("Test lastIndexOfString function", fun _ ->
            let result1 = Str.lastIndexOfString "es" "testes"
            let result2 = Str.lastIndexOfString "zz" "test"
            assertThat result1 (tag "Should be 4" >> isEqualTo 4)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        test ("Test lastIndexOfStringFrom function", fun _ ->
            let result1 = Str.lastIndexOfStringFrom "es" 3 "testes"
            let result2 = Str.lastIndexOfStringFrom "zz" 1 "test"
            assertThat result1 (tag "Should be 1" >> isEqualTo 1)
            assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        )

        // test ("Test lastIndexOfStringFromFor function", fun _ ->
        //     let result1 = Str.lastIndexOfStringFromFor "es" 2 2 "testes"
        //     let result2 = Str.lastIndexOfStringFromFor "zz" 1 2 "test"
        //     assertThat result1 (tag "Should be 1" >> isEqualTo 1)
        //     assertThat result2 (tag "Should be -1" >> isEqualTo -1)
        // )

        test ("Test padLeft function", fun _ ->
            let result = Str.padLeft 10 "test"
            assertThat result (tag "Should be '      test'" >> isEqualTo "      test")
        )

        test ("Test padLeftWith function", fun _ ->
            let result = Str.padLeftWith 10 '*' "test"
            assertThat result (tag "Should be '******test'" >> isEqualTo "******test")
        )

        test ("Test padRight function", fun _ ->
            let result = Str.padRight 10 "test"
            assertThat result (tag "Should be 'test      '" >> isEqualTo "test      ")
        )

        test ("Test padRightWith function", fun _ ->
            let result = Str.padRightWith 10 '*' "test"
            assertThat result (tag "Should be 'test******'" >> isEqualTo "test******")
        )

        test ("Test remove function", fun _ ->
            let result = Str.remove 1 "test"
            assertThat result (tag "Should be 't'" >> isEqualTo "t")
        )

        test ("Test removeFrom function", fun _ ->
            let result = Str.removeFrom 1 2 "test"
            assertThat result (tag "Should be 'tt'" >> isEqualTo "tt")
        )

        test ("Test replaceChar function", fun _ ->
            let result = Str.replaceChar 'e' 'a' "teste"
            assertThat result (tag "Should be 'tast'" >> isEqualTo "tasta")
        )

        test ("Test replace function", fun _ ->
            let result = Str.replace "es" "ar" "test"
            assertThat result (tag "Should be 'tart'" >> isEqualTo "tart")
        )

        test ("Test replacefirst function", fun _ ->
            let result = Str.replaceFirst "es" "ar" "testes"
            assertThat result (tag "Should be 'tartes'" >> isEqualTo "tartes")
        )

        test ("Test replacefirst function bad case ", fun _ ->
            let result = Str.replaceFirst "eS" "ar" "testes"
            assertThat result (tag "Should be still be 'testes'" >> isEqualTo "testes")
        )

        test ("Test replaceLast function", fun _ ->
            let result = Str.replaceLast "es" "ar" "testes"
            assertThat result (tag "Should be 'testar'" >> isEqualTo "testar")
        )

        test ("Test replaceLast function bad case ", fun _ ->
            let result = Str.replaceLast "eS" "ar" "testes"
            assertThat result (tag "Should be still be 'testes'" >> isEqualTo "testes")
        )

        test ("replaceFirst and replaceLast use ordinal matching", fun _ ->
            let decomposed = "e\u0301"
            assertThat (Str.replaceFirst "\u00E9" "X" decomposed) (tag "replaceFirst should not use linguistic equivalence" >> isEqualTo decomposed)
            assertThat (Str.replaceLast "\u00E9" "X" decomposed) (tag "replaceLast should not use linguistic equivalence" >> isEqualTo decomposed)
        )


        test ("Test concat function", fun _ ->
            let result = Str.concat "$" ["line1"; "line2"; "line3"]
            assertThat result (tag "Should be 'line1$line2$line3'" >> isEqualTo "line1$line2$line3")
        )

        test ("Test concatLines function", fun _ ->
            let result = Str.concatLines ["line1"; "line2"; "line3"]
            let nl = System.Environment.NewLine
            assertThat result (tag ("Should be 'line1"+nl+"line2"+nl+"line3'") >> isEqualTo ("line1"+nl+"line2"+nl+"line3"))
        )

        test ("Test splitLines function", fun _ ->
            let result = Str.splitLines "line1\nline2\nline3"
            assertThat result (tag "Should be ['line1'; 'line2'; 'line3']" >> isEqualTo [|"line1"; "line2"; "line3"|])
        )

        test ("Test split function", fun _ ->
            let result = Str.split "," "1,2,3"
            assertThat result (tag "Should be ['1'; '2'; '3']" >> isEqualTo [|"1"; "2"; "3"|])
        )


        test ("Test split function2", fun _ ->
            let result = Str.split "," "1,2,,3"
            assertThat result (tag "Should be ['1'; '2'; '3']" >> isEqualTo [|"1"; "2"; "3"|])
        )


        test ("Test splitKeep function", fun _ ->
            let result = Str.splitKeep "," "1,2,3,,"
            assertThat result (tag "Should be ['1'; '2'; '3'; ''; '']" >> isEqualTo [|"1"; "2"; "3"; ""; ""|])
        )

        test ("Test splitChar function", fun _ ->
            let result = Str.splitChar ',' "1,2,3,,"
            assertThat result (tag "Should be ['1'; '2'; '3']" >> isEqualTo [|"1"; "2"; "3"|])
        )

        test ("Test splitChars function", fun _ ->
            let result = Str.splitChars [|','; ' '|] "1,2 3,,"
            assertThat result (tag "Should be ['1'; '2'; '3']" >> isEqualTo [|"1"; "2"; "3"|])
        )

        test ("Test splitCharKeep function", fun _ ->
            let result = Str.splitCharKeep ',' "1,2,3,,"
            assertThat result (tag "Should be ['1'; '2'; '3'; ''; '']" >> isEqualTo [|"1"; "2"; "3"; ""; ""|])
        )

        test ("Test splitCharsKeep function", fun _ ->
            let result = Str.splitCharsKeep [|','; ' '|] "1,2 3,,"
            assertThat result (tag "Should be ['1'; '2'; '3'; ''; '']" >> isEqualTo [|"1"; "2"; "3"; ""; ""|])
        )

        test ("Test substringFrom function", fun _ ->
            let result = Str.substringFrom 1 "test"
            assertThat result (tag "Should be 'est'" >> isEqualTo "est")
        )

        test ("Test substringFromFor function", fun _ ->
            let result = Str.substringFromFor 1 2 "test"
            assertThat result (tag "Should be 'es'" >> isEqualTo "es")
        )

        test ("Test toCharArray function", fun _ ->
            let result = Str.toCharArray "test"
            assertThat result (tag "Should be ['t'; 'e'; 's'; 't']" >> isEqualTo [|'t'; 'e'; 's'; 't'|])
        )

        test ("Test toCharArrayFromFor function", fun _ ->
            let result = Str.toCharArrayFromFor 1 2 "test"
            assertThat result (tag "Should be ['e'; 's']" >> isEqualTo [|'e'; 's'|])
        )

        test ("Test toLower function", fun _ ->
            let result = Str.toLower "TEST"
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test toUpper function", fun _ ->
            let result = Str.toUpper "test"
            assertThat result (tag "Should be 'TEST'" >> isEqualTo "TEST")
        )

        test ("Test trim function", fun _ ->
            let result = Str.trim "  test  "
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test trimChar function", fun _ ->
            let result = Str.trimChar '*' "*test*"
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test trimChars function", fun _ ->
            let result = Str.trimChars [|' '; '*'|] " *test* "
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test trimEnd function", fun _ ->
            let result = Str.trimEnd "test  "
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test trimEndChar function", fun _ ->
            let result = Str.trimEndChar '*' "test*"
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test trimEndChars function", fun _ ->
            let result = Str.trimEndChars [|' '; '*'|] "test* "
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test trimStart function", fun _ ->
            let result = Str.trimStart "  test"
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test trimStartChar function", fun _ ->
            let result = Str.trimStartChar '*' "*test"
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test trimStartChars function", fun _ ->
            let result = Str.trimStartChars [|' '; '*'|] " *test"
            assertThat result (tag "Should be 'test'" >> isEqualTo "test")
        )

        test ("Test formatInOneLine function", fun _ ->
            let result = Str.formatInOneLine "line1\nline2\nline3"
            assertThat result (tag "Should be 'line1 line2 line3'" >> isEqualTo "line1 line2 line3")
        )

        test ("formatInOneLine collapses Unicode whitespace", fun _ ->
            let result = Str.formatInOneLine "a\u00A0\u00A0b"
            assertThat result (tag "Non-breaking spaces should be collapsed" >> isEqualTo "a b")
        )

        test ("Test formatTruncated function", fun _ ->
            let result = Str.formatTruncated 26 "This is a very long string that should be truncated"
            assertThat result (tag "Should be '\"This is a very long(...)ed\"'" >> isEqualTo "\"This is a very long(...)ed\"")
        )

        test ("Test formatTruncatedToMaxLines function", fun _ ->
            let result = Str.formatTruncatedToMaxLines 2 "line1\r\nline2\r\nline3\r\nline4\r\nline5"
            assertThat result (tag "The result should contain no more than two lines" >> isEqualTo "\"line1\r\nline2(... and 3 more lines.)\"")
        )

        test ("formatTruncatedToMaxLines truncates exactly one additional line", fun _ ->
            let result = Str.formatTruncatedToMaxLines 1 "line1\nline2"
            assertThat result (tag "The note should remain on the final allowed line" >> isEqualTo "\"line1(... and 1 more line.)\"")
        )

        test ("formatTruncatedToMaxLines recognizes standalone CR line endings", fun _ ->
            let result = Str.formatTruncatedToMaxLines 2 "a\rb\rc"
            assertThat result (tag "Standalone CR should delimit lines without adding an extra output line" >> isEqualTo "\"a\rb(... and 1 more line.)\"")
        )

        test ("formatTruncatedToMaxLines leaves an in-range string unchanged", fun _ ->
            let input = "line1\nline2"
            assertThat (Str.formatTruncatedToMaxLines 2 input) (tag "Unchanged results should not be quoted" >> isEqualTo input)
        )



        test ("str builder ", fun _ ->
            let result =
                str{
                    "Hello"
                    " "
                    "World"
                }
            assertThat result (tag "Should be 'Hello World'" >> isEqualTo "Hello World")
        )

        // ============ str computation expression comprehensive tests ============
        test ("str builder with char yield", fun _ ->
            let result = str { 'H'; 'i' }
            assertThat result (tag "Should be 'Hi'" >> isEqualTo "Hi")
        )

        test ("str builder with int yield", fun _ ->
            let result = str { 42 }
            assertThat result (tag "Should be '42'" >> isEqualTo "42")
        )

        test ("str builder appends a newline after every sequence item", fun _ ->
            let result = str { ["a"; "b"] }
            let expected = "a" + Environment.NewLine + "b" + Environment.NewLine
            assertThat result (tag "A yielded string sequence should include a trailing newline" >> isEqualTo expected)
        )

        test ("str builder with Guid yield", fun _ ->
            let g = System.Guid.Empty
            let result = str { g }
            assertThat result (tag "Should be empty guid" >> isEqualTo "00000000-0000-0000-0000-000000000000")
        )

        test ("str builder with yield! for string (with newline)", fun _ ->
            let result = str { yield! "Hello" ; "World" }
            assertThat (result.Contains("Hello")) (tag "Should contain Hello" >> isTrue)
            assertThat (result.Contains("World")) (tag "Should contain World" >> isTrue)
            assertThat (result.Contains("\n") || result.Contains("\r")) (tag "Should contain newline" >> isTrue)
        )

        test ("str builder with yield! for char (with newline)", fun _ ->
            let result = str { yield! 'A' ; 'B' }
            assertThat (result.Contains("A")) (tag "Should contain A" >> isTrue)
            assertThat (result.Contains("B")) (tag "Should contain B" >> isTrue)
        )

        test ("str builder with yield! for int (with newline)", fun _ ->
            let result = str { yield! 1 ; 2 }
            assertThat (result.Contains("1")) (tag "Should contain 1" >> isTrue)
            assertThat (result.Contains("2")) (tag "Should contain 2" >> isTrue)
        )

        test ("str builder with seq of strings", fun _ ->
            let lines = ["Line1"; "Line2"; "Line3"]
            let result = str { yield lines }
            assertThat (result.Contains("Line1")) (tag "Should contain Line1" >> isTrue)
            assertThat (result.Contains("Line2")) (tag "Should contain Line2" >> isTrue)
            assertThat (result.Contains("Line3")) (tag "Should contain Line3" >> isTrue)
        )

        test ("str builder with for loop", fun _ ->
            let result = str {
                for i in 1..3 do
                    yield i.ToString()
            }
            assertThat result (tag "Should be '123'" >> isEqualTo "123")
        )

        test ("str builder with for loop and strings", fun _ ->
            let items = ["A"; "B"; "C"]
            let result = str {
                for item in items do
                    yield item
            }
            assertThat result (tag "Should be 'ABC'" >> isEqualTo "ABC")
        )

        test ("str builder with while loop", fun _ ->
            let mutable count = 0
            let result = str {
                while count < 3 do
                    yield "X"
                    count <- count + 1
            }
            assertThat result (tag "Should be 'XXX'" >> isEqualTo "XXX")
        )

        test ("str builder empty", fun _ ->
            let result = str { () }
            assertThat result (tag "Should be empty string" >> isEqualTo "")
        )

        test ("str builder mixed types", fun _ ->
            let result = str {
                "Count: "
                42
                ", Char: "
                'X'
            }
            assertThat result (tag "Should combine different types" >> isEqualTo "Count: 42, Char: X")
        )



        // normalize
        test ("normalize should throw an exception if input string is null", fun _ ->
            assertThat (fun _ -> Str.normalize null |> ignore<string>) (tag "Should throw an exception" >> throws)
        )

        // normalize
        test ("normalize should return the normalized string", fun _ ->
            let result = Str.normalize "Héllò, Wörld!"
            assertThat result (tag "Should be 'Hello, World!'" >> isEqualTo "Hello, World!")
        )

        // normalize
        test ("normalize", fun _ ->
            let result = Str.normalize "crème brûlée"
            assertThat result (tag "Should be equal" >> isEqualTo "creme brulee")
        )

        // normalize
        test ("normalize2", fun _ ->
            let result = Str.normalize "crèmeö brûlée"
            assertThat result (tag "Should be equal" >> isEqualTo "cremeo brulee")
        )


        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        // error FABLE: Microsoft.FSharp.Core.Operators.ArrayExtensions.String.GetReverseIndex is not supported by Fable
        #else
        // slice
        test ("neg slice2 should return a substring from the start index to the end index", fun _ ->
            let result = "Hello, World!".[1..^1]
            assertThat result (tag "Should be equal" >> isEqualTo "ello, World")
        )
        #endif



        // addThousandSeparators tests
        test ("addThousandSeparators should format integer with apostrophe separator", fun _ ->
            let result = Str.addThousandSeparators '\'' "1234567"
            assertThat result (tag "Should be '1'234'567'" >> isEqualTo "1'234'567")
        )

        test ("addThousandSeparators should format integer with comma separator", fun _ ->
            let result = Str.addThousandSeparators ',' "1234567"
            assertThat result (tag "Should be '1,234,567'" >> isEqualTo "1,234,567")
        )

        test ("addThousandSeparators should format small integer without separator", fun _ ->
            let result = Str.addThousandSeparators '\'' "123"
            assertThat result (tag "Should be '123'" >> isEqualTo "123")
        )

        test ("addThousandSeparators should format negative integer", fun _ ->
            let result = Str.addThousandSeparators '\'' "-1234567"
            assertThat result (tag "Should be '-1'234'567'" >> isEqualTo "-1'234'567")
        )

        test ("addThousandSeparators should format float with decimal part", fun _ ->
            let result = Str.addThousandSeparators '\'' "1234567.89"
            assertThat result (tag "Should be '1'234'567.89'" >> isEqualTo "1'234'567.89")
        )

        test ("addThousandSeparators should format float with long decimal part", fun _ ->
            let result = Str.addThousandSeparators '\'' "1234.5678901234"
            assertThat result (tag "Should be '1'234.567'890'123'4'" >> isEqualTo "1'234.567'890'123'4")
        )

        test ("addThousandSeparators should format negative float", fun _ ->
            let result = Str.addThousandSeparators '\'' "-9876543.21"
            assertThat result (tag "Should be '-9'876'543.21'" >> isEqualTo "-9'876'543.21")
        )

        test ("addThousandSeparators should handle single digit", fun _ ->
            let result = Str.addThousandSeparators '\'' "5"
            assertThat result (tag "Should be '5'" >> isEqualTo "5")
        )

        test ("addThousandSeparators should handle zero", fun _ ->
            let result = Str.addThousandSeparators '\'' "0"
            assertThat result (tag "Should be '0'" >> isEqualTo "0")
        )

        test ("addThousandSeparators should format exactly 1000", fun _ ->
            let result = Str.addThousandSeparators '\'' "1000"
            assertThat result (tag "Should be '1'000'" >> isEqualTo "1'000")
        )

        test ("addThousandSeparators should format exactly 1 million", fun _ ->
            let result = Str.addThousandSeparators '\'' "1000000"
            assertThat result (tag "Should be '1'000'000'" >> isEqualTo "1'000'000")
        )

        test ("addThousandSeparators should handle decimal only number", fun _ ->
            let result = Str.addThousandSeparators '\'' "0.123456789"
            assertThat result (tag "Should be '0.123'456'789'" >> isEqualTo "0.123'456'789")
        )

        test ("addThousandSeparators should format negative with scientific notation", fun _ ->
            let result = Str.addThousandSeparators '\'' "-123456.789e-5"
            assertThat result (tag "Should be '-123'456.789e-5'" >> isEqualTo "-123'456.789e-5")
        )

        test ("addThousandSeparators should  format negative with scientific notation", fun _ ->
            let result = Str.addThousandSeparators '\'' "-1.23456789e-5"
            assertThat result (tag "Should be '-1.234'567'89e-5'" >> isEqualTo "-1.234'567'89e-5")
        )

        test ("addThousandSeparators should work with underscore separator", fun _ ->
            let result = Str.addThousandSeparators '_' "123456789"
            assertThat result (tag "Should be '123_456_789'" >> isEqualTo "123_456_789")
        )

        test ("addThousandSeparators should work with space separator", fun _ ->
            let result = Str.addThousandSeparators ' ' "123456789"
            assertThat result (tag "Should be '123 456 789'" >> isEqualTo "123 456 789")
        )

        test ("addThousandSeparators should work after decimal point", fun _ ->
            let result = Str.addThousandSeparators '_' "12345.6789"
            assertThat result (tag "Should be '12_345.678_9'" >> isEqualTo "12_345.678_9")
        )


        test ("addThousandSeparators should format with scientific notation", fun _ ->
            let result = Str.addThousandSeparators '\'' "1234567e10"
            assertThat result (tag "Should be '1234567e10'" >> isEqualTo "1'234'567e10")
        )

        test ("addThousandSeparators should format with scientific notation neg", fun _ ->
            let result = Str.addThousandSeparators '\'' "-1234567e10"
            assertThat result (tag "Should be '-1234567e10'" >> isEqualTo "-1'234'567e10")
        )

        test ("addThousandSeparators handles a trailing decimal point", fun _ ->
            assertThat (Str.addThousandSeparators '\'' "1.") (tag "A trailing decimal point should be preserved" >> isEqualTo "1.")
        )

        test ("addThousandSeparators handles a leading decimal point", fun _ ->
            assertThat (Str.addThousandSeparators '\'' ".5") (tag "A leading decimal point should be preserved" >> isEqualTo ".5")
        )

        test ("addThousandSeparators handles a decimal point before an exponent", fun _ ->
            assertThat (Str.addThousandSeparators '\'' "1.e3") (tag "A decimal point before an exponent should be preserved" >> isEqualTo "1.e3")
        )

        test ("addThousandSeparators rejects empty and null input", fun _ ->
            assertThat (fun () -> Str.addThousandSeparators '\'' "" |> ignore) (tag "Expected empty input to throw" >> throws)
            assertThat (fun () -> Str.addThousandSeparators '\'' null |> ignore) (tag "Expected null input to throw" >> throws)
        )

        // ============ Str.get tests ============
        test ("Str.get should return character at index", fun _ ->
            let result = Str.get 0 "Hello"
            assertThat result (tag "Should be 'H'" >> isEqualTo 'H')
        )

        test ("Str.get should return character at middle index", fun _ ->
            let result = Str.get 2 "Hello"
            assertThat result (tag "Should be 'l'" >> isEqualTo 'l')
        )

        test ("Str.get should return character at last index", fun _ ->
            let result = Str.get 4 "Hello"
            assertThat result (tag "Should be 'o'" >> isEqualTo 'o')
        )

        test ("Str.get should throw for null string", fun _ ->
            assertThat (fun _ -> Str.get 0 null |> ignore) (tag "Should throw for null" >> throws)
        )

        test ("Str.get should throw for negative index", fun _ ->
            assertThat (fun _ -> Str.get -1 "Hello" |> ignore) (tag "Should throw for negative index" >> throws)
        )

        test ("Str.get should throw for index out of range", fun _ ->
            assertThat (fun _ -> Str.get 10 "Hello" |> ignore) (tag "Should throw for out of range" >> throws)
        )

        test ("Str.get should throw for empty string", fun _ ->
            assertThat (fun _ -> Str.get 0 "" |> ignore) (tag "Should throw for empty string" >> throws)
        )

        // ============ Str.isWhite tests ============
        test ("Str.isWhite should return true for null", fun _ ->
            let result = Str.isWhite null
            assertThat result (tag "Should be true for null" >> isTrue)
        )

        test ("Str.isWhite should return true for empty string", fun _ ->
            let result = Str.isWhite ""
            assertThat result (tag "Should be true for empty" >> isTrue)
        )

        test ("Str.isWhite should return true for whitespace only", fun _ ->
            let result = Str.isWhite "   "
            assertThat result (tag "Should be true for whitespace" >> isTrue)
        )

        test ("Str.isWhite should return true for tabs and newlines", fun _ ->
            let result = Str.isWhite "\t\n\r"
            assertThat result (tag "Should be true for tabs/newlines" >> isTrue)
        )

        test ("Str.isWhite should return false for non-whitespace", fun _ ->
            let result = Str.isWhite "Hello"
            assertThat result (tag "Should be false for text" >> isFalse)
        )

        test ("Str.isWhite should return false for whitespace with text", fun _ ->
            let result = Str.isWhite "  Hello  "
            assertThat result (tag "Should be false for text with whitespace" >> isFalse)
        )

        // ============ Str.isNotWhite tests ============
        test ("Str.isNotWhite should return false for null", fun _ ->
            let result = Str.isNotWhite null
            assertThat result (tag "Should be false for null" >> isFalse)
        )

        test ("Str.isNotWhite should return false for empty string", fun _ ->
            let result = Str.isNotWhite ""
            assertThat result (tag "Should be false for empty" >> isFalse)
        )

        test ("Str.isNotWhite should return false for whitespace only", fun _ ->
            let result = Str.isNotWhite "   "
            assertThat result (tag "Should be false for whitespace" >> isFalse)
        )

        test ("Str.isNotWhite should return true for non-whitespace", fun _ ->
            let result = Str.isNotWhite "Hello"
            assertThat result (tag "Should be true for text" >> isTrue)
        )

        test ("Str.isNotWhite should return true for whitespace with text", fun _ ->
            let result = Str.isNotWhite "  Hello  "
            assertThat result (tag "Should be true for text with whitespace" >> isTrue)
        )

        // ============ Str.isEmpty tests ============
        test ("Str.isEmpty should return true for null", fun _ ->
            let result = Str.isEmpty null
            assertThat result (tag "Should be true for null" >> isTrue)
        )

        test ("Str.isEmpty should return true for empty string", fun _ ->
            let result = Str.isEmpty ""
            assertThat result (tag "Should be true for empty" >> isTrue)
        )

        test ("Str.isEmpty should return false for whitespace", fun _ ->
            let result = Str.isEmpty "   "
            assertThat result (tag "Should be false for whitespace (not empty)" >> isFalse)
        )

        test ("Str.isEmpty should return false for non-empty string", fun _ ->
            let result = Str.isEmpty "Hello"
            assertThat result (tag "Should be false for text" >> isFalse)
        )

        // ============ Str.isNotEmpty tests ============
        test ("Str.isNotEmpty should return false for null", fun _ ->
            let result = Str.isNotEmpty null
            assertThat result (tag "Should be false for null" >> isFalse)
        )

        test ("Str.isNotEmpty should return false for empty string", fun _ ->
            let result = Str.isNotEmpty ""
            assertThat result (tag "Should be false for empty" >> isFalse)
        )

        test ("Str.isNotEmpty should return true for whitespace", fun _ ->
            let result = Str.isNotEmpty "   "
            assertThat result (tag "Should be true for whitespace (not empty)" >> isTrue)
        )

        test ("Str.isNotEmpty should return true for non-empty string", fun _ ->
            let result = Str.isNotEmpty "Hello"
            assertThat result (tag "Should be true for text" >> isTrue)
        )

        // ============================================================
        // Extensive tests for functions with FABLE_COMPILER directives
        // to ensure JS and .NET runtimes behave the same way
        // ============================================================

        // ============ containsIgnoreCase - extensive tests ============

        test ("containsIgnoreCase: exact match same case", fun _ ->
            assertThat (Str.containsIgnoreCase "Hello" "Hello") (tag "exact match" >> isTrue)
        )

        test ("containsIgnoreCase: all uppercase needle in lowercase haystack", fun _ ->
            assertThat (Str.containsIgnoreCase "HELLO" "hello world") (tag "upper in lower" >> isTrue)
        )

        test ("containsIgnoreCase: all lowercase needle in uppercase haystack", fun _ ->
            assertThat (Str.containsIgnoreCase "hello" "HELLO WORLD") (tag "lower in upper" >> isTrue)
        )

        test ("containsIgnoreCase: mixed case needle", fun _ ->
            assertThat (Str.containsIgnoreCase "hElLo" "Hello World") (tag "mixed case" >> isTrue)
        )

        test ("containsIgnoreCase: empty needle in non-empty haystack", fun _ ->
            assertThat (Str.containsIgnoreCase "" "Hello") (tag "empty needle always found" >> isTrue)
        )

        test ("containsIgnoreCase: empty needle in empty haystack", fun _ ->
            assertThat (Str.containsIgnoreCase "" "") (tag "empty in empty" >> isTrue)
        )

        test ("containsIgnoreCase: non-empty needle in empty haystack", fun _ ->
            assertThat (Str.containsIgnoreCase "x" "") (tag "can't find in empty" >> isFalse)
        )

        test ("containsIgnoreCase: needle longer than haystack", fun _ ->
            assertThat (Str.containsIgnoreCase "Hello World!" "Hello") (tag "needle longer" >> isFalse)
        )

        test ("containsIgnoreCase: single char match", fun _ ->
            assertThat (Str.containsIgnoreCase "a" "A") (tag "single char case insensitive" >> isTrue)
        )

        test ("containsIgnoreCase: single char no match", fun _ ->
            assertThat (Str.containsIgnoreCase "z" "A") (tag "single char no match" >> isFalse)
        )

        test ("containsIgnoreCase: substring at start", fun _ ->
            assertThat (Str.containsIgnoreCase "hel" "Hello") (tag "at start" >> isTrue)
        )

        test ("containsIgnoreCase: substring at end", fun _ ->
            assertThat (Str.containsIgnoreCase "LLO" "Hello") (tag "at end" >> isTrue)
        )

        test ("containsIgnoreCase: substring in middle", fun _ ->
            assertThat (Str.containsIgnoreCase "LL" "Hello") (tag "in middle" >> isTrue)
        )

        test ("containsIgnoreCase: with digits", fun _ ->
            assertThat (Str.containsIgnoreCase "abc123" "xABC123y") (tag "digits are case insensitive irrelevant" >> isTrue)
        )

        test ("containsIgnoreCase: with special characters", fun _ ->
            assertThat (Str.containsIgnoreCase "!@#" "hello!@#world") (tag "special chars" >> isTrue)
        )

        test ("containsIgnoreCase: whitespace matters", fun _ ->
            assertThat (Str.containsIgnoreCase "hello world" "helloworld") (tag "whitespace matters" >> isFalse)
        )

        test ("containsIgnoreCase: repeated pattern", fun _ ->
            assertThat (Str.containsIgnoreCase "aa" "AAA") (tag "repeated pattern" >> isTrue)
        )

        test ("containsIgnoreCase: null haystack throws", fun _ ->
            assertThat (fun _ -> Str.containsIgnoreCase "x" null |> ignore<bool>) (tag "null haystack" >> throws)
        )

        test ("containsIgnoreCase: null needle throws", fun _ ->
            assertThat (fun _ -> Str.containsIgnoreCase null "hello" |> ignore<bool>) (tag "null needle" >> throws)
        )

        test ("containsIgnoreCase: both null throws", fun _ ->
            assertThat (fun _ -> Str.containsIgnoreCase null null |> ignore<bool>) (tag "both null" >> throws)
        )

        test ("containsIgnoreCase: unicode letters", fun _ ->
            assertThat (Str.containsIgnoreCase "über" "ÜBER cool") (tag "unicode case" >> isTrue)
        )

        test ("containsIgnoreCase: very long string", fun _ ->
            let haystack = String.replicate 1000 "ab" + "XY" + String.replicate 1000 "cd"
            assertThat (Str.containsIgnoreCase "xy" haystack) (tag "find in long string" >> isTrue)
        )

        test ("containsIgnoreCase: near miss", fun _ ->
            assertThat (Str.containsIgnoreCase "abd" "abc") (tag "near miss" >> isFalse)
        )


        // ============ indexOfCharFromFor - extensive tests ============

        test ("indexOfCharFromFor: find char at start of search range", fun _ ->
            let result = Str.indexOfCharFromFor 'a' 0 3 "abc"
            assertThat result (tag "char at start" >> isEqualTo 0)
        )

        test ("indexOfCharFromFor: find char at end of search range", fun _ ->
            let result = Str.indexOfCharFromFor 'c' 0 3 "abc"
            assertThat result (tag "char at end of range" >> isEqualTo 2)
        )

        test ("indexOfCharFromFor: char outside search range returns -1", fun _ ->
            let result = Str.indexOfCharFromFor 'c' 0 2 "abc"
            assertThat result (tag "char outside range" >> isEqualTo -1)
        )

        test ("indexOfCharFromFor: search from middle", fun _ ->
            let result = Str.indexOfCharFromFor 'b' 1 2 "abc"
            assertThat result (tag "find from middle" >> isEqualTo 1)
        )

        test ("indexOfCharFromFor: char not present returns -1", fun _ ->
            let result = Str.indexOfCharFromFor 'z' 0 3 "abc"
            assertThat result (tag "char not present" >> isEqualTo -1)
        )

        test ("indexOfCharFromFor: duplicate chars finds first in range", fun _ ->
            let result = Str.indexOfCharFromFor 'a' 0 5 "abaca"
            assertThat result (tag "first occurrence" >> isEqualTo 0)
        )

        test ("indexOfCharFromFor: duplicate chars finds first in range starting later", fun _ ->
            let result = Str.indexOfCharFromFor 'a' 1 4 "abaca"
            assertThat result (tag "first occurrence after start" >> isEqualTo 2)
        )

        test ("indexOfCharFromFor: count of 1", fun _ ->
            let result = Str.indexOfCharFromFor 'a' 0 1 "abc"
            assertThat result (tag "count 1 found" >> isEqualTo 0)
        )

        test ("indexOfCharFromFor: count of 1 not found", fun _ ->
            let result = Str.indexOfCharFromFor 'b' 0 1 "abc"
            assertThat result (tag "count 1 not found" >> isEqualTo -1)
        )

        test ("indexOfCharFromFor: search entire string", fun _ ->
            let result = Str.indexOfCharFromFor 'o' 0 13 "Hello, World!"
            assertThat result (tag "search entire string" >> isEqualTo 4)
        )

        test ("indexOfCharFromFor: search with startIndex past char", fun _ ->
            let result = Str.indexOfCharFromFor 'H' 1 5 "Hello, World!"
            assertThat result (tag "start past the char" >> isEqualTo -1)
        )

        test ("indexOfCharFromFor: find second occurrence", fun _ ->
            let result = Str.indexOfCharFromFor 'l' 3 3 "Hello, World!"
            assertThat result (tag "second l" >> isEqualTo 3)
        )

        test ("indexOfCharFromFor: null input throws", fun _ ->
            assertThat (fun _ -> Str.indexOfCharFromFor 'a' 0 1 null |> ignore) (tag "null throws" >> throws)
        )

        test ("indexOfCharFromFor: space character", fun _ ->
            let result = Str.indexOfCharFromFor ' ' 0 7 "Hello, World!"
            assertThat result (tag "find space" >> isEqualTo 6)
        )

        test ("indexOfCharFromFor: search in single char string found", fun _ ->
            let result = Str.indexOfCharFromFor 'x' 0 1 "x"
            assertThat result (tag "single char found" >> isEqualTo 0)
        )

        test ("indexOfCharFromFor: search in single char string not found", fun _ ->
            let result = Str.indexOfCharFromFor 'y' 0 1 "x"
            assertThat result (tag "single char not found" >> isEqualTo -1)
        )

        test ("indexOfCharFromFor: multiple identical chars", fun _ ->
            let result = Str.indexOfCharFromFor 'a' 2 3 "aaaaa"
            assertThat result (tag "finds at start of range in repeated chars" >> isEqualTo 2)
        )

        test ("indexOfCharFromFor: last position in range", fun _ ->
            let result = Str.indexOfCharFromFor 'c' 0 3 "xxc"
            assertThat result (tag "last position in range" >> isEqualTo 2)
        )

        test ("indexOfCharFromFor: newline character", fun _ ->
            let result = Str.indexOfCharFromFor '\n' 0 6 "ab\ncd\n"
            assertThat result (tag "find newline" >> isEqualTo 2)
        )

        test ("indexOfCharFromFor: tab character", fun _ ->
            let result = Str.indexOfCharFromFor '\t' 0 4 "a\tb\t"
            assertThat result (tag "find tab" >> isEqualTo 1)
        )


        // ============ indexOfStringFromFor - extensive tests ============

        test ("indexOfStringFromFor: find at start of range", fun _ ->
            let result = Str.indexOfStringFromFor "ab" 0 3 "abc"
            assertThat result (tag "at start" >> isEqualTo 0)
        )

        test ("indexOfStringFromFor: find at end of range", fun _ ->
            let result = Str.indexOfStringFromFor "cd" 1 4 "abcde"
            assertThat result (tag "at end of range" >> isEqualTo 2)
        )

        test ("indexOfStringFromFor: not found in range", fun _ ->
            let result = Str.indexOfStringFromFor "de" 0 3 "abcde"
            assertThat result (tag "not in range" >> isEqualTo -1)
        )

        test ("indexOfStringFromFor: empty search string", fun _ ->
            let result = Str.indexOfStringFromFor "" 0 3 "abc"
            assertThat result (tag "empty string found at startIndex" >> isEqualTo 0)
        )

        test ("indexOfStringFromFor: exact match whole range", fun _ ->
            let result = Str.indexOfStringFromFor "abc" 0 3 "abc"
            assertThat result (tag "exact match" >> isEqualTo 0)
        )

        test ("indexOfStringFromFor: pattern longer than search range", fun _ ->
            let result = Str.indexOfStringFromFor "abcd" 0 3 "abcde"
            assertThat result (tag "pattern longer than range" >> isEqualTo -1)
        )

        test ("indexOfStringFromFor: single char pattern", fun _ ->
            let result = Str.indexOfStringFromFor "c" 0 3 "abc"
            assertThat result (tag "single char" >> isEqualTo 2)
        )

        test ("indexOfStringFromFor: duplicate patterns finds first in range", fun _ ->
            let result = Str.indexOfStringFromFor "ab" 0 6 "ababab"
            assertThat result (tag "first occurrence" >> isEqualTo 0)
        )

        test ("indexOfStringFromFor: duplicate patterns from offset", fun _ ->
            let result = Str.indexOfStringFromFor "ab" 1 5 "ababab"
            assertThat result (tag "first after offset" >> isEqualTo 2)
        )

        test ("indexOfStringFromFor: overlapping pattern", fun _ ->
            let result = Str.indexOfStringFromFor "aba" 0 5 "ababa"
            assertThat result (tag "overlapping" >> isEqualTo 0)
        )

        test ("indexOfStringFromFor: overlapping pattern from offset", fun _ ->
            let result = Str.indexOfStringFromFor "aba" 1 4 "ababa"
            assertThat result (tag "overlapping from offset" >> isEqualTo 2)
        )

        test ("indexOfStringFromFor: null needle throws", fun _ ->
            assertThat (fun _ -> Str.indexOfStringFromFor null 0 3 "abc" |> ignore) (tag "null needle" >> throws)
        )

        test ("indexOfStringFromFor: null haystack throws", fun _ ->
            assertThat (fun _ -> Str.indexOfStringFromFor "ab" 0 3 null |> ignore) (tag "null haystack" >> throws)
        )

        test ("indexOfStringFromFor: search in single char string", fun _ ->
            let result = Str.indexOfStringFromFor "x" 0 1 "x"
            assertThat result (tag "single char string found" >> isEqualTo 0)
        )

        test ("indexOfStringFromFor: search in single char string not found", fun _ ->
            let result = Str.indexOfStringFromFor "y" 0 1 "x"
            assertThat result (tag "single char string not found" >> isEqualTo -1)
        )

        test ("indexOfStringFromFor: with special characters", fun _ ->
            let result = Str.indexOfStringFromFor "!@" 0 5 "hi!@#"
            assertThat result (tag "special chars" >> isEqualTo 2)
        )

        test ("indexOfStringFromFor: with whitespace pattern", fun _ ->
            let result = Str.indexOfStringFromFor " " 0 6 "a b c "
            assertThat result (tag "whitespace pattern" >> isEqualTo 1)
        )

        test ("indexOfStringFromFor: case sensitive", fun _ ->
            let result = Str.indexOfStringFromFor "AB" 0 3 "abc"
            assertThat result (tag "case sensitive" >> isEqualTo -1)
        )

        test ("indexOfStringFromFor: pattern at exact boundary", fun _ ->
            let result = Str.indexOfStringFromFor "cd" 2 2 "abcde"
            assertThat result (tag "at boundary" >> isEqualTo 2)
        )

        test ("indexOfStringFromFor: start at last position count 1", fun _ ->
            let result = Str.indexOfStringFromFor "e" 4 1 "abcde"
            assertThat result (tag "last position" >> isEqualTo 4)
        )

        test ("indexOfStringFromFor: long pattern in long string", fun _ ->
            let s = String.replicate 100 "ab" + "XYZ" + String.replicate 100 "cd"
            let result = Str.indexOfStringFromFor "XYZ" 0 s.Length s
            assertThat result (tag "long string" >> isEqualTo 200)
        )

        test ("indexOfStringFromFor: long pattern only in narrow range", fun _ ->
            let s = String.replicate 100 "ab" + "XYZ" + String.replicate 100 "cd"
            let result = Str.indexOfStringFromFor "XYZ" 0 200 s
            assertThat result (tag "not in narrow range" >> isEqualTo -1)
        )

        test ("indexOfStringFromFor: repeated single char", fun _ ->
            let result = Str.indexOfStringFromFor "a" 3 2 "aaaaa"
            assertThat result (tag "repeated single" >> isEqualTo 3)
        )


        // ============ normalize - extensive tests ============

        test ("normalize: null throws", fun _ ->
            assertThat (fun _ -> Str.normalize null |> ignore<string>) (tag "null throws" >> throws)
        )

        test ("normalize: empty string", fun _ ->
            let result = Str.normalize ""
            assertThat result (tag "empty stays empty" >> isEqualTo "")
        )

        test ("normalize: plain ASCII unchanged", fun _ ->
            let result = Str.normalize "Hello World"
            assertThat result (tag "ASCII unchanged" >> isEqualTo "Hello World")
        )

        test ("normalize: digits and punctuation unchanged", fun _ ->
            let result = Str.normalize "123!@#$%^&*()"
            assertThat result (tag "digits and punctuation" >> isEqualTo "123!@#$%^&*()")
        )

        test ("normalize: acute accent e", fun _ ->
            let result = Str.normalize "é"
            assertThat result (tag "acute e" >> isEqualTo "e")
        )

        test ("normalize: grave accent e", fun _ ->
            let result = Str.normalize "è"
            assertThat result (tag "grave e" >> isEqualTo "e")
        )

        test ("normalize: circumflex accent e", fun _ ->
            let result = Str.normalize "ê"
            assertThat result (tag "circumflex e" >> isEqualTo "e")
        )

        test ("normalize: diaeresis accent e", fun _ ->
            let result = Str.normalize "ë"
            assertThat result (tag "diaeresis e" >> isEqualTo "e")
        )

        test ("normalize: tilde n", fun _ ->
            let result = Str.normalize "ñ"
            assertThat result (tag "tilde n" >> isEqualTo "n")
        )

        test ("normalize: umlaut characters", fun _ ->
            let result = Str.normalize "äöü"
            assertThat result (tag "umlauts" >> isEqualTo "aou")
        )

        test ("normalize: uppercase accented", fun _ ->
            let result = Str.normalize "ÀÁÂÃÄÅ"
            assertThat result (tag "uppercase accented A variants" >> isEqualTo "AAAAAA")
        )

        test ("normalize: cedilla", fun _ ->
            let result = Str.normalize "ç"
            assertThat result (tag "cedilla" >> isEqualTo "c")
        )

        test ("normalize: uppercase cedilla", fun _ ->
            let result = Str.normalize "Ç"
            assertThat result (tag "uppercase cedilla" >> isEqualTo "C")
        )

        test ("normalize: mixed accented and plain", fun _ ->
            let result = Str.normalize "café"
            assertThat result (tag "mixed" >> isEqualTo "cafe")
        )

        test ("normalize: full sentence with accents", fun _ ->
            let result = Str.normalize "Les élèves français"
            assertThat result (tag "full sentence" >> isEqualTo "Les eleves francais")
        )

        test ("normalize: crème brûlée", fun _ ->
            let result = Str.normalize "crème brûlée"
            assertThat result (tag "creme brulee" >> isEqualTo "creme brulee")
        )

        test ("normalize: whitespace preserved", fun _ ->
            let result = Str.normalize "  \t\n  "
            assertThat result (tag "whitespace preserved" >> isEqualTo "  \t\n  ")
        )

        test ("normalize: single accented character", fun _ ->
            let result = Str.normalize "ö"
            assertThat result (tag "single accented" >> isEqualTo "o")
        )

        test ("normalize: multiple accents on same base (precomposed)", fun _ ->
            // Vietnamese: ồ = o + combining breve + combining grave
            let result = Str.normalize "ồ"
            // Should at least remove diacritics, result should be plain o
            assertThat result (tag "multiple accents" >> isEqualTo "o")
        )

        test ("normalize: string with no accents is idempotent", fun _ ->
            let input = "The quick brown fox jumps over the lazy dog 1234567890"
            let result = Str.normalize input
            assertThat result (tag "no change for plain ASCII" >> isEqualTo input)
        )

        test ("normalize: accented string normalized twice is same as once", fun _ ->
            let input = "crème brûlée"
            let once = Str.normalize input
            let twice = Str.normalize once
            assertThat twice (tag "idempotent" >> isEqualTo once)
        )

        test ("normalize: Scandinavian characters", fun _ ->
            let result = Str.normalize "Ångström"
            assertThat result (tag "Scandinavian" >> isEqualTo "Angstrom")
        )

        test ("normalize: Spanish text", fun _ ->
            let result = Str.normalize "El niño está aquí"
            assertThat result (tag "Spanish" >> isEqualTo "El nino esta aqui")
        )

        test ("normalize: German text", fun _ ->
            let result = Str.normalize "Ärger über böse Füße"
            assertThat result (tag "German - sharp s preserved" >> isEqualTo "Arger uber bose Fuße")
        )

        test ("normalize: only accents (combining characters)", fun _ ->
            // standalone combining acute accent
            let result = Str.normalize "\u0301"
            assertThat result (tag "standalone combining accent removed" >> isEqualTo "")
        )

        test ("normalize: preserves non-Latin scripts without accents", fun _ ->
            // CJK and numbers should pass through
            let result = Str.normalize "abc123"
            assertThat result (tag "basic latin+digits" >> isEqualTo "abc123")
        )

        // --- additional normalize tests ---

        test ("normalize: brackets and special symbols preserved", fun _ ->
            let input = "[]{}()<>|\\~`_-+=/"
            assertThat (Str.normalize input) (tag "brackets and symbols" >> isEqualTo input)
        )

        test ("normalize: quotes preserved", fun _ ->
            let input = "\"hello\" 'world'"
            assertThat (Str.normalize input) (tag "quotes" >> isEqualTo input)
        )

        test ("normalize: decomposed form (NFD) e + combining acute", fun _ ->
            // e (U+0065) + combining acute (U+0301) = é in NFD
            let input = "e\u0301"
            let result = Str.normalize input
            assertThat result (tag "decomposed acute removed" >> isEqualTo "e")
        )

        test ("normalize: decomposed form a + combining ring above", fun _ ->
            // a (U+0061) + combining ring above (U+030A) = å in NFD
            let input = "a\u030A"
            let result = Str.normalize input
            assertThat result (tag "decomposed ring removed" >> isEqualTo "a")
        )

        test ("normalize: multiple combining marks on one base", fun _ ->
            // a + combining acute + combining tilde
            let input = "a\u0301\u0303"
            let result = Str.normalize input
            assertThat result (tag "multiple combining marks removed" >> isEqualTo "a")
        )

        test ("normalize: precomposed vs decomposed same result", fun _ ->
            let precomposed = Str.normalize "é"      // U+00E9
            let decomposed = Str.normalize "e\u0301" // e + combining acute
            assertThat precomposed (tag "precomposed and decomposed yield same result" >> isEqualTo decomposed)
        )

        test ("normalize: CJK characters preserved", fun _ ->
            let input = "\u4F60\u597D\u4E16\u754C" // 你好世界
            assertThat (Str.normalize input) (tag "CJK unchanged" >> isEqualTo input)
        )

        test ("normalize: Cyrillic without accents preserved", fun _ ->
            let input = "\u041F\u0440\u0438\u0432\u0435\u0442" // Привет
            assertThat (Str.normalize input) (tag "Cyrillic preserved" >> isEqualTo input)
        )

        test ("normalize: Cyrillic with combining accent", fun _ ->
            // и (U+0438) + combining acute (U+0301) → и
            let input = "\u0438\u0301"
            let result = Str.normalize input
            assertThat result (tag "Cyrillic accent removed" >> isEqualTo "\u0438")
        )

        test ("normalize: Greek without accents preserved", fun _ ->
            let input = "\u03B1\u03B2\u03B3" // αβγ
            assertThat (Str.normalize input) (tag "Greek preserved" >> isEqualTo input)
        )

        test ("normalize: Greek with tonos", fun _ ->
            // ά (U+03AC, alpha with tonos) → α
            let result = Str.normalize "\u03AC"
            assertThat result (tag "Greek tonos removed" >> isEqualTo "\u03B1")
        )

        test ("normalize: Polish text", fun _ ->
            let result = Str.normalize "\u0179\u00F3\u0142\u0107"  // Źółć
            // ł (U+0142) is a distinct letter, not a base + combining mark, so it stays
            assertThat result (tag "Polish diacritics removed, ł preserved" >> isEqualTo "Zo\u0142c")
        )

        test ("normalize: Czech text", fun _ ->
            let result = Str.normalize "\u0159\u00E1\u010D\u0161\u0165" // řáčšť
            assertThat result (tag "Czech diacritics removed" >> isEqualTo "racst")
        )

        test ("normalize: Turkish dotted and dotless i", fun _ ->
            // İ (U+0130, capital I with dot) should lose the dot
            let result = Str.normalize "\u0130"
            assertThat result (tag "Turkish capital dotted I" >> isEqualTo "I")
        )

        test ("normalize: Vietnamese text", fun _ ->
            let result = Str.normalize "\u1ED3\u1EA5\u1EBF"  // ồấế
            assertThat result (tag "Vietnamese diacritics removed" >> isEqualTo "oae")
        )

        test ("normalize: single char string", fun _ ->
            assertThat (Str.normalize "a") (tag "single plain char" >> isEqualTo "a")
        )

        test ("normalize: string of only combining marks", fun _ ->
            // two standalone combining marks (acute + grave)
            let result = Str.normalize "\u0301\u0300"
            assertThat result (tag "only combining marks → empty" >> isEqualTo "")
        )

        test ("normalize: accented chars interspersed with numbers", fun _ ->
            let result = Str.normalize "\u00E91\u00E82\u00F63"  // é1è2ö3
            assertThat result (tag "accents removed, digits kept" >> isEqualTo "e1e2o3")
        )

        test ("normalize: tabs and newlines with accents", fun _ ->
            let result = Str.normalize "\u00E9\t\u00E8\n\u00F6"  // é\tè\nö
            assertThat result (tag "whitespace preserved with accents" >> isEqualTo "e\te\no")
        )

        test ("normalize: long string performance", fun _ ->
            let input = String.replicate 1000 "\u00E9"  // 1000 × é
            let result = Str.normalize input
            assertThat result (tag "long accented string" >> isEqualTo (String.replicate 1000 "e"))
        )

        test ("normalize: mixed scripts in one string", fun _ ->
            // Latin accented + CJK + Cyrillic + digits
            let input = "caf\u00E9\u4F60\u597D\u041F\u0440\u0438\u0432\u0435\u044242"
            let result = Str.normalize input
            assertThat result (tag "mixed scripts" >> isEqualTo "cafe\u4F60\u597D\u041F\u0440\u0438\u0432\u0435\u044242")
        )

        test ("normalize: sharp s (ß) preserved", fun _ ->
            // ß (U+00DF) is not a diacritic, should stay
            let result = Str.normalize "\u00DF"
            assertThat result (tag "sharp s unchanged" >> isEqualTo "\u00DF")
        )

        test ("normalize: eth (ð) preserved", fun _ ->
            let result = Str.normalize "\u00F0"
            assertThat result (tag "eth unchanged" >> isEqualTo "\u00F0")
        )

        test ("normalize: thorn (þ) preserved", fun _ ->
            let result = Str.normalize "\u00FE"
            assertThat result (tag "thorn unchanged" >> isEqualTo "\u00FE")
        )

        test ("normalize: copyright and trademark symbols preserved", fun _ ->
            let input = "\u00A9\u00AE\u2122"  // ©®™
            assertThat (Str.normalize input) (tag "symbols preserved" >> isEqualTo input)
        )

        test ("normalize: currency symbols preserved", fun _ ->
            let input = "$\u20AC\u00A3\u00A5"  // $€£¥
            assertThat (Str.normalize input) (tag "currency symbols" >> isEqualTo input)
        )

        test ("normalize: result length <= input length", fun _ ->
            let input = "\u00C0\u00C1\u00C2\u00C3\u00C4\u00C5\u00C7\u00C8\u00C9\u00CA"  // ÀÁÂÃÄÅÇÈÉÊ
            let result = Str.normalize input
            assertThat (result.Length <= input.Length) (tag "result not longer than input" >> isTrue)
        )

        test ("normalize: triple application is same as single", fun _ ->
            let input = "\u00C4rger \u00FCber b\u00F6se F\u00FC\u00DFe"  // Ärger über böse Füße
            let once = Str.normalize input
            let triple = Str.normalize (Str.normalize (Str.normalize input))
            assertThat triple (tag "triple application idempotent" >> isEqualTo once)
        )

    ])














