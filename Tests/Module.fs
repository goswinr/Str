namespace Tests

open Str

#if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
open Fable.Mocha
#else
open Expecto
#endif

open System

module Module =


 let tests =
  testList "Module.fs Tests" [

        testCase "indicesOf with pattern found" <| fun _ ->
            let text = "abababab"
            let pattern = "abab"
            let result = Str.indicesOf (text, pattern, 0, text.Length, 10) |> Array.ofSeq
            Expect.equal result [|0; 2; 4|] "Expected pattern to be found at indices [0; 2; 4]"

        testCase "indicesOf with pattern found2" <| fun _ ->
            let text = "abab0abab0"
            let pattern = "abab"
            let result = Str.indicesOf (text, pattern, 0, text.Length, 10) |> Array.ofSeq
            Expect.equal result [|0; 5|] "Expected pattern to be found at indices [0; 5]"

        testCase "Str.indicesOf with pattern not found" <| fun _ ->
            let text = "Hello, world!"
            let pattern = "xyz"
            let result = Str.indicesOf (text, pattern, 0, text.Length, 10)|> Array.ofSeq
            Expect.equal result [||] "Expected pattern to not be found"

        testCase "Str.indicesOf with null pattern" <| fun _ ->
            let text = "Hello, world!"
            let testFunc = fun () -> Str.indicesOf (text, null, 0, text.Length, 10) |> ignore
            Expect.throws testFunc "Expected an exception when pattern is null"

        testCase "Str.indicesOf with null text" <| fun _ ->
            let pattern = "Hello"
            let testFunc = fun () -> Str.indicesOf (null, pattern, 0, 5, 10) |> ignore
            Expect.throws testFunc "Expected an exception when text is null"

        testCase "Str.indicesOf with negative searchFromIdx" <| fun _ ->
            let text = "Hello, world!"
            let pattern = "Hello"
            let testFunc = fun () -> Str.indicesOf (text, pattern, -1, text.Length, 10) |> ignore
            Expect.throws testFunc "Expected an exception when searchFromIdx is negative"

        testCase "Str.indicesOf with negative searchLength" <| fun _ ->
            let text = "Hello, world!"
            let pattern = "Hello"
            let testFunc = fun () -> Str.indicesOf (text, pattern, 0, -1, 10) |> ignore
            Expect.throws testFunc "Expected an exception when searchLength is negative"

        testCase "Str.indicesOf with searchFromIdx + searchLength > text.Length" <| fun _ ->
            let text = "Hello, world!"
            let pattern = "Hello"
            let testFunc = fun () -> Str.indicesOf (text, pattern, 0, text.Length + 1, 10) |> ignore
            Expect.throws testFunc "Expected an exception when searchFromIdx + searchLength > text.Length"

        testCase "truncate should return the truncated string" <| fun _ ->
            let result = Str.truncate 5 "Hello, World!"
            Expect.equal result "Hello" "Should be equal"

        testCase "truncate should return the same string if length is greater than string length" <| fun _ ->
            let result = Str.truncate 50 "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "skip should return the string after skipping the specified length" <| fun _ ->
            let result = Str.skip 5 "Hello, World!"
            Expect.equal result ", World!" "Should be equal"

        testCase "take should return the string of the specified length" <| fun _ ->
            let result = Str.take 5 "Hello, World!"
            Expect.equal result "Hello" "Should be equal"

        testCase "delete should return the string after deleting the specified text" <| fun _ ->
            let result = Str.delete "Hello" "Hello, World!Hello"
            Expect.equal result ", World!" "Should be equal"

        testCase "deleteChar should return the string after deleting the specified char" <| fun _ ->
            let result = Str.deleteChar 'H' "HelloH, World!"
            Expect.equal result "ello, World!" "Should be equal"

        testCase "truncate should return an empty string if length is 0" <| fun _ ->
            let result = Str.truncate 0 "Hello, World!"
            Expect.equal result "" "Should be equal"

        testCase "truncate should throw an exception if length is negative" <| fun _ ->
            Expect.throws (fun _ -> Str.truncate -5 "Hello, World!"  |> ignore<string>) "Should throw an exception"

        testCase "skip should return an empty string if length is equal to string length" <| fun _ ->
            let result = Str.skip 13 "Hello, World!"
            Expect.equal result "" "Should be equal"

        testCase "skip should throw an exception if length is greater than string length" <| fun _ ->
            Expect.throws (fun _ -> Str.skip 50 "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "take should return an empty string if length is 0" <| fun _ ->
            let result = Str.take 0 "Hello, World!"
            Expect.equal result "" "Should be equal"

        testCase "take should throw an exception if length is negative" <| fun _ ->
            Expect.throws (fun _ -> Str.take -5 "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "delete should return the same string if text is not found" <| fun _ ->
            let result = Str.delete "Goodbye" "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "deleteChar should return the same string if char is not found" <| fun _ ->
            let result = Str.deleteChar 'Z' "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "delete should return the same string if text is empty" <| fun _ ->
            let result = Str.delete "" "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "delete should throw an exception if text is null" <| fun _ ->
            Expect.throws (fun _ -> Str.delete null "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "deleteChar should return the same string if char is null" <| fun _ ->
            let result = Str.deleteChar '@' "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "truncate should return the same string if length is equal to string length" <| fun _ ->
            let result = Str.truncate 13 "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "take should return the same string if length is equal to string length" <| fun _ ->
            let result = Str.take 12 "Hello, World!"
            Expect.equal result "Hello, World" "Should be equal"

        testCase "unifyLineEndings should replace all line endings with System.Environment.NewLine" <| fun _ ->
            let result = Str.unifyLineEndings "Hello\r\nWorld!\rHello\nWorld!"
            Expect.equal result ("Hello" + Environment.NewLine + "World!" + Environment.NewLine + "Hello" + Environment.NewLine + "World!") "Should be equal"

        testCase "unifyLineEndings should throw an exception if input is null" <| fun _ ->
            Expect.throws (fun _ -> Str.unifyLineEndings null |> ignore<string>) "Should throw an exception"

        testCase "before should return the string before the splitter" <| fun _ ->
            let result = Str.before "," "Hello, World!"
            Expect.equal result "Hello" "Should be equal"

        testCase "before should throw an exception if splitter is not found" <| fun _ ->
            Expect.throws (fun _ -> Str.before "Z" "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "before should throw an exception if stringToSearchIn is null" <| fun _ ->
            Expect.throws (fun _ -> Str.before "," null |> ignore<string>) "Should throw an exception"

        testCase "before should throw an exception if splitter is null" <| fun _ ->
            Expect.throws (fun _ -> Str.before null "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "beforeChar should return the string before the splitter" <| fun _ ->
            let result = Str.beforeChar ',' "Hello, World!"
            Expect.equal result "Hello" "Should be equal"

        testCase "beforeChar should throw an exception if splitter is not found" <| fun _ ->
            Expect.throws (fun _ -> Str.beforeChar 'Z' "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "tryBefore should return Some string before the splitter" <| fun _ ->
            let result = Str.tryBefore "," "Hello, World!"
            Expect.equal result (Some "Hello") "Should be equal"

        testCase "tryBefore should return None if splitter is not found" <| fun _ ->
            let result = Str.tryBefore "Z" "Hello, World!"
            Expect.equal result None "Should be equal"

        testCase "tryBeforeChar should return Some string before the splitter" <| fun _ ->
            let result = Str.tryBeforeChar ',' "Hello, World!"
            Expect.equal result (Some "Hello") "Should be equal"

        testCase "tryBeforeChar should return None if splitter is not found" <| fun _ ->
            let result = Str.tryBeforeChar 'Z' "Hello, World!"
            Expect.equal result None "Should be equal"

        testCase "beforeOrInput should return the string before the splitter" <| fun _ ->
            let result = Str.beforeOrInput "," "Hello, World!"
            Expect.equal result "Hello" "Should be equal"

        testCase "beforeOrInput should return the input string if splitter is not found" <| fun _ ->
            let result = Str.beforeOrInput "Z" "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "beforeCharOrInput should return the string before the splitter" <| fun _ ->
            let result = Str.beforeCharOrInput ',' "Hello, World!"
            Expect.equal result "Hello" "Should be equal"

        testCase "beforeCharOrInput should return the input string if splitter is not found" <| fun _ ->
            let result = Str.beforeCharOrInput 'Z' "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "after should return the string after the splitter" <| fun _ ->
            let result = Str.after "," "Hello, World!"
            Expect.equal result " World!" "Should be equal"

        testCase "after should throw an exception if splitter is not found" <| fun _ ->
            Expect.throws (fun _ -> Str.after "Z" "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "afterChar should return the string after the splitter" <| fun _ ->
            let result = Str.afterChar ',' "Hello, World!"
            Expect.equal result " World!" "Should be equal"

        testCase "afterChar should throw an exception if splitter is not found" <| fun _ ->
            Expect.throws (fun _ -> Str.afterChar 'Z' "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "tryAfter should return Some string after the splitter" <| fun _ ->
            let result = Str.tryAfter "," "Hello, World!"
            Expect.equal result (Some " World!") "Should be equal"

        testCase "tryAfter should return None if splitter is not found" <| fun _ ->
            let result = Str.tryAfter "Z" "Hello, World!"
            Expect.equal result None "Should be equal"

        testCase "tryAfterChar should return Some string after the splitter" <| fun _ ->
            let result = Str.tryAfterChar ',' "Hello, World!"
            Expect.equal result (Some " World!") "Should be equal"

        testCase "tryAfterChar should return None if splitter is not found" <| fun _ ->
            let result = Str.tryAfterChar 'Z' "Hello, World!"
            Expect.equal result None "Should be equal"


        testCase "afterOrInput should return the string after the splitter" <| fun _ ->
            let result = Str.afterOrInput "," "Hello, World!"
            Expect.equal result " World!" "Should be equal"

        testCase "afterOrInput should return the input string if splitter is not found" <| fun _ ->
            let result = Str.afterOrInput "Z" "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "afterCharOrInput should return the string after the splitter" <| fun _ ->
            let result = Str.afterCharOrInput ',' "Hello, World!"
            Expect.equal result " World!" "Should be equal"

        testCase "afterCharOrInput should return the input string if splitter is not found" <| fun _ ->
            let result = Str.afterCharOrInput 'Z' "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "between should return the string between the splitters" <| fun _ ->
            let result = Str.between "," "!" "Hello, World!"
            Expect.equal result " World" "Should be equal"

        testCase "between should throw an exception if splitters are not found" <| fun _ ->
            Expect.throws (fun _ -> Str.between "Z" "Y" "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "tryBetween should return Some string between the splitters" <| fun _ ->
            let result = Str.tryBetween "," "!" "Hello, World!"
            Expect.equal result (Some " World") "Should be equal"

        testCase "tryBetween should return None if splitters are not found" <| fun _ ->
            let result = Str.tryBetween "Z" "Y" "Hello, World!"
            Expect.equal result None "Should be equal"

        testCase "betweenOrInput should return the string between the splitters" <| fun _ ->
            let result = Str.betweenOrInput "," "!" "Hello, World!"
            Expect.equal result " World" "Should be equal"

        testCase "betweenOrInput should return the input string if splitters are not found" <| fun _ ->
            let result = Str.betweenOrInput "Z" "Y" "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"

        testCase "betweenChars should return the string between the splitters" <| fun _ ->
            let result = Str.betweenChars ',' '!' "Hello, World!"
            Expect.equal result " World" "Should be equal"

        testCase "betweenChars should throw an exception if splitters are not found" <| fun _ ->
            Expect.throws (fun _ -> Str.betweenChars 'Z' 'Y' "Hello, World!" |> ignore<string>) "Should throw an exception"

        testCase "tryBetweenChars should return Some string between the splitters" <| fun _ ->
            let result = Str.tryBetweenChars ',' '!' "Hello, World!"
            Expect.equal result (Some " World") "Should be equal"

        testCase "tryBetweenChars should return None if splitters are not found" <| fun _ ->
            let result = Str.tryBetweenChars 'Z' 'Y' "Hello, World!"
            Expect.equal result None "Should be equal"

        testCase "betweenCharsOrInput should return the string between the splitters" <| fun _ ->
            let result = Str.betweenCharsOrInput ',' '!' "Hello, World!"
            Expect.equal result " World" "Should be equal"

        testCase "betweenCharsOrInput should return the input string if splitters are not found" <| fun _ ->
            let result = Str.betweenCharsOrInput 'Z' 'Y' "Hello, World!"
            Expect.equal result "Hello, World!" "Should be equal"


        testCase "splitOnce should split the string once at the splitter" <| fun _ ->
            let result = Str.splitOnce "," "Hello, World!"
            Expect.equal result ("Hello", " World!") "Should be equal"

        testCase "splitOnce should throw an exception if splitter is not found" <| fun _ ->
            Expect.throws (fun _ -> Str.splitOnce "Z" "Hello, World!" |> ignore<string*string>) "Should throw an exception"

        testCase "splitOnce should throw an exception if stringToSplit is null" <| fun _ ->
            Expect.throws (fun _ -> Str.splitOnce "," null |> ignore<string*string>) "Should throw an exception"

        testCase "splitOnce should throw an exception if splitter is null" <| fun _ ->
            Expect.throws (fun _ -> Str.splitOnce null "Hello, World!" |> ignore<string*string>) "Should throw an exception"


        testCase "splitOnce should return the input string and an empty string if splitter is at the end" <| fun _ ->
            let result = Str.splitOnce "," "Hello,"
            Expect.equal result ("Hello", "") "Should be equal"

        testCase "splitOnce should return an empty string and the input string if splitter is at the start" <| fun _ ->
            let result = Str.splitOnce "," ",World!"
            Expect.equal result ("", "World!") "Should be equal"

        testCase "splitOnce should return two empty strings if input string is the splitter" <| fun _ ->
            let result = Str.splitOnce "," ","
            Expect.equal result ("", "") "Should be equal"

        testCase "splitOnce should throw an exception if input string is empty" <| fun _ ->
            Expect.throws (fun _ -> Str.splitOnce "," "" |> ignore<string*string>) "Should throw an exception"


        testCase "splitTwice should split the string twice at the splitters" <| fun _ ->
            let result = Str.splitTwice "X" "T" "cXabTk"
            Expect.equal result ("c", "ab", "k") "Should be equal"

        testCase "splitTwice should throw an exception if firstSplitter is not found" <| fun _ ->
            Expect.throws (fun _ -> Str.splitTwice "Z" "T" "cXabTk" |> ignore<string*string*string>) "Should throw an exception"

        testCase "splitTwice should throw an exception if secondSplitter is not found" <| fun _ ->
            Expect.throws (fun _ -> Str.splitTwice "X" "Z" "cXabTk" |> ignore<string*string*string>) "Should throw an exception"

        testCase "splitTwice should throw an exception if stringToSplit is null" <| fun _ ->
            Expect.throws (fun _ -> Str.splitTwice "X" "T" null |> ignore<string*string*string>) "Should throw an exception"

        testCase "splitTwice should throw an exception if firstSplitter is null" <| fun _ ->
            Expect.throws (fun _ -> Str.splitTwice null "T" "cXabTk" |> ignore<string*string*string>) "Should throw an exception"

        testCase "splitTwice should throw an exception if secondSplitter is null" <| fun _ ->
            Expect.throws (fun _ -> Str.splitTwice "X" null "cXabTk" |> ignore<string*string*string>) "Should throw an exception"


        testCase "between should return the input string if splitters are at the start and end" <| fun _ ->
            let result = Str.between "," "!" ",Hello, World!"
            Expect.equal result "Hello, World" "Should be equal"

        testCase "between should return an empty string if input string is the splitters" <| fun _ ->
            let result = Str.between "," "!" ",!"
            Expect.equal result "" "Should be equal"

        testCase "between should throw an exception if input string is empty" <| fun _ ->
            Expect.throws (fun _ -> Str.between "," "!" "" |> ignore<string>) "Should throw an exception"

        // trySplitOnce
        testCase "trySplitOnce should return Some tuple if splitter is found" <| fun _ ->
            let result = Str.trySplitOnce "," "Hello, World!"
            Expect.equal result (Some ("Hello", " World!")) "Should be equal"

        testCase "trySplitOnce should return None if splitter is not found" <| fun _ ->
            let result = Str.trySplitOnce "Z" "Hello, World!"
            Expect.equal result None "Should be equal"

        // trySplitTwice
        testCase "trySplitTwice should return Some tuple if splitters are found" <| fun _ ->
            let result = Str.trySplitTwice "H" "o" "Hello, World!"
            Expect.equal result (Some ("", "ell", ", World!")) "Should be equal"

        testCase "trySplitTwice should return None if splitters are not found" <| fun _ ->
            let result = Str.trySplitTwice "Z" "Y" "Hello, World!"
            Expect.equal result None "Should be equal"

        // up1
        testCase "up1 should return the string with the first character uppercased" <| fun _ ->
            let result = Str.up1 "hello"
            Expect.equal result "Hello" "Should be equal"

        // low1
        testCase "low1 should return the string with the first character lowercased" <| fun _ ->
            let result = Str.low1 "HELLO"
            Expect.equal result "hELLO" "Should be equal"

        // slice
        testCase "slice should return a substring from the start index to the end index" <| fun _ ->
            let result = Str.slice 0 5 "Hello, World!"
            Expect.equal result "Hello," "Should be equal"

        // slice
        testCase "neg slice should return a substring from the start index to the end index" <| fun _ ->
            let result = Str.slice -3 -1 "Hello, World!"
            Expect.equal result "ld!" "Should be equal"


        // countSubString
        testCase "countSubString should return the number of occurrences of the substring" <| fun _ ->
            let result = Str.countSubString "l" "Hello, World!"
            Expect.equal result 3 "Should be equal"

        // countChar
        testCase "countChar should return the number of occurrences of the character" <| fun _ ->
            let result = Str.countChar 'l' "Hello, World!"
            Expect.equal result 3 "Should be equal"




        // addSuffix
        testCase "addSuffix should add the suffix to the string" <| fun _ ->
            let result = Str.addSuffix " World!" "Hello,"
            Expect.equal result "Hello, World!" "Should be equal"

        // addPrefix
        testCase "addPrefix should add the prefix to the string" <| fun _ ->
            let result = Str.addPrefix "Hello, " "World!"
            Expect.equal result "Hello, World!" "Should be equal"

        // inQuotes
        testCase "inQuotes should add double quotes at the start and end of the string" <| fun _ ->
            let result = Str.inQuotes "Hello, World!"
            Expect.equal result "\"Hello, World!\"" "Should be equal"

        // inSingleQuotes
        testCase "inSingleQuotes should add single quotes at the start and end of the string" <| fun _ ->
            let result = Str.inSingleQuotes "Hello, World!"
            Expect.equal result "'Hello, World!'" "Should be equal"

        // contains
        testCase "contains should return true if the string contains the substring" <| fun _ ->
            let result = Str.contains "World" "Hello, World!"
            Expect.isTrue result "Should be true"

        // containsIgnoreCase
        testCase "containsIgnoreCase should return true if the string contains the substring, ignoring case" <| fun _ ->
            let result = Str.containsIgnoreCase "world" "Hello, World!"
            Expect.isTrue result "Should be true"

        // containsIgnoreCase
        testCase "containsIgnoreCase should return false if the string not contains the substring, ignoring case" <| fun _ ->
            let result = Str.containsIgnoreCase "worl1" "Hello, World!"
            Expect.isFalse result "Should be false"

        // notContains
        testCase "notContains should return true if the string does not contain the substring" <| fun _ ->
            let result = Str.notContains "Universe" "Hello, World!"
            Expect.isTrue result "Should be true"

        // containsChar
        testCase "containsChar should return true if the string contains the character" <| fun _ ->
            let result = Str.containsChar 'H' "Hello, World!"
            Expect.isTrue result "Should be true"

        // notContainsChar
        testCase "notContainsChar should return true if the string does not contain the character" <| fun _ ->
            let result = Str.notContainsChar 'Z' "Hello, World!"
            Expect.isTrue result "Should be true"

        // compare
        testCase "compare should return 0 if the strings are equal" <| fun _ ->
            let result = Str.compare "Hello, World!" "Hello, World!"
            Expect.equal result 0 "Should be equal"

        // compareIgnoreCase
        testCase "compareIgnoreCase should return 0 if the strings are equal, ignoring case" <| fun _ ->
            let result = Str.compareIgnoreCase "hello, world!" "Hello, World!"
            Expect.equal result 0 "Should be equal"

        // compareIgnoreCase
        testCase "compareIgnoreCase should not return 0 if the strings are not equal, ignoring case" <| fun _ ->
            let result = Str.compareIgnoreCase "hello,world!" "Hello, World!"
            Expect.isFalse (result= 0) "Should not be equal"

        // endsWith
        testCase "endsWith should return true if the string ends with the substring" <| fun _ ->
            let result = Str.endsWith "World!" "Hello, World!"
            Expect.isTrue result "Should be true"

        // endsWithIgnoreCase
        testCase "endsWithIgnoreCase should return true if the string ends with the substring, ignoring case" <| fun _ ->
            let result = Str.endsWithIgnoreCase "world!" "Hello, World!"
            Expect.isTrue result "Should be true"

        // startsWith
        testCase "startsWith should return true if the string starts with the substring" <| fun _ ->
            let result = Str.startsWith "Hello" "Hello, World!"
            Expect.isTrue result "Should be true"

        // startsWithIgnoreCase
        testCase "startsWithIgnoreCase should return true if the string starts with the substring, ignoring case-3" <| fun _ ->
            let result = Str.startsWithIgnoreCase "hello" "Hello, World!"
            Expect.isTrue result "Should be true"

        // equals
        testCase "equals should return true if the strings are equal" <| fun _ ->
            let result = Str.equals "Hello, World!" "Hello, World!"
            Expect.isTrue result "Should be true"

        // equals
        testCase "equals should return false if the strings are not equal" <| fun _ ->
            let result = Str.equals "Hello, world!" "Hello, World!"
            Expect.isFalse result "Should be false"

        // countChar
        testCase "countChar should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.countChar 'l' null |> ignore<int>) "Should throw an exception"



        // addSuffix
        testCase "addSuffix should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.addSuffix " World!" null |> ignore<string>) "Should throw an exception"

        // addPrefix
        testCase "addPrefix should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.addPrefix "Hello, " null |> ignore<string>) "Should throw an exception"

        // inQuotes
        testCase "inQuotes should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.inQuotes null |> ignore<string>) "Should throw an exception"

        // inSingleQuotes
        testCase "inSingleQuotes should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.inSingleQuotes null |> ignore<string>) "Should throw an exception"

        // contains
        testCase "contains should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.contains "World" null |> ignore<bool>) "Should throw an exception"

        // containsIgnoreCase
        testCase "containsIgnoreCase should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.containsIgnoreCase "world" null |> ignore<bool>) "Should throw an exception"

        // notContains
        testCase "notContains should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.notContains "Universe" null |> ignore<bool>) "Should throw an exception"

        // containsChar
        testCase "containsChar should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.containsChar 'H' null |> ignore<bool>) "Should throw an exception"

        // notContainsChar
        testCase "notContainsChar should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.notContainsChar 'Z' null |> ignore<bool>) "Should throw an exception"

        // compare
        testCase "compare should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.compare "Hello, World!" null |> ignore<int>) "Should throw an exception"

        // compareIgnoreCase
        testCase "compareIgnoreCase should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.compareIgnoreCase "hello, world!" null |> ignore<int>) "Should throw an exception"

        // endsWith
        testCase "endsWith should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.endsWith "World!" null |> ignore<bool>) "Should throw an exception"

        // endsWithIgnoreCase
        testCase "endsWithIgnoreCase should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.endsWithIgnoreCase "world!" null |> ignore<bool>) "Should throw an exception"

        // startsWith
        testCase "startsWith should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.startsWith "Hello" null |> ignore<bool>) "Should throw an exception"

        // startsWithIgnoreCase
        testCase "startsWithIgnoreCase should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.startsWithIgnoreCase "hello" null |> ignore<bool>) "Should throw an exception"

        // equals
        testCase "equals should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.equals "Hello, World!" null |> ignore<bool>) "Should throw an exception"



        // equalsIgnoreCase
        testCase "equalsIgnoreCase should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.equalsIgnoreCase null "Hello"  |> ignore  ) "Should throw exception"

        // indexOfChar
        testCase "indexOfChar should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfChar 'H' null  |> ignore  ) "Should throw exception"

        // indexOfCharFrom
        testCase "indexOfCharFrom should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfCharFrom 'H' 0 null  |> ignore  ) "Should throw exception"

        // indexOfCharFromFor
        testCase "indexOfCharFromFor should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfCharFromFor 'H' 0 1 null  |> ignore  ) "Should throw exception"

        // indexOfString
        testCase "indexOfString should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfString null "Hello"  |> ignore  ) "Should throw exception"
            Expect.throws (fun _ -> Str.indexOfString "Hello" null  |> ignore  ) "Should throw exception"

        // indexOfStringFrom
        testCase "indexOfStringFrom should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfStringFrom null 0 "Hello"  |> ignore  ) "Should throw exception"
            Expect.throws (fun _ -> Str.indexOfStringFrom "Hello" 0 null  |> ignore  ) "Should throw exception"

        // indexOfStringFromFor
        testCase "indexOfStringFromFor should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfStringFromFor null 0 1 "Hello"  |> ignore  ) "Should throw exception"
            Expect.throws (fun _ -> Str.indexOfStringFromFor "Hello" 0 1 null  |> ignore  ) "Should throw exception"

        // indexOfAny
        testCase "indexOfAny should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfAny [|'H'|] null  |> ignore  ) "Should throw exception"

        // indexOfAnyFrom
        testCase "indexOfAnyFrom should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfAnyFrom [|'H'|] 0 null  |> ignore  ) "Should throw exception"

        // indexOfAnyFromFor
        testCase "indexOfAnyFromFor should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfAnyFromFor [|'H'|] 0 1 null  |> ignore  ) "Should throw exception"

        // insert
        testCase "insert should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.insert 0 null "Hello"  |> ignore  ) "Should throw exception"
            Expect.throws (fun _ -> Str.insert 0 "Hello" null  |> ignore  ) "Should throw exception"

        // lastIndexOfChar
        testCase "lastIndexOfChar should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.lastIndexOfChar 'H' null  |> ignore  ) "Should throw exception"

        // lastIndexOfCharFrom
        testCase "lastIndexOfCharFrom should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.lastIndexOfCharFrom 'H' 0 null  |> ignore  ) "Should throw exception"

        // lastIndexOfCharFromFor
        // testCase "lastIndexOfCharFromFor should handle null input" <| fun _ ->
        //     Expect.throws (fun _ -> Str.lastIndexOfCharFromFor 'H' 0 1 null  |> ignore  ) "Should throw exception"

        // lastIndexOfString
        testCase "lastIndexOfString should handle null input" <| fun _ ->
            Expect.throws (fun _ -> Str.lastIndexOfString null "Hello"  |> ignore  ) "Should throw exception"
            Expect.throws (fun _ -> Str.lastIndexOfString "Hello" null  |> ignore  ) "Should throw exception"

        // startsWith
        testCase "startsWith should return true if the string starts with the substring,2" <| fun _ ->
            let result = Str.startsWith "Hello, World!" "Hello"
            Expect.isFalse result "Should be false"

        // startsWithIgnoreCase
        testCase "startsWithIgnoreCase should return true if the string starts with the substring, ignoring case" <| fun _ ->
            let result = Str.startsWithIgnoreCase "hell" "Hello, World!"
            Expect.isTrue result "Should be true"

        // equals
        testCase "equals should return true if the strings are equal -3" <| fun _ ->
            let result = Str.equals "Hello, World!" "Hello, World!"
            Expect.isTrue result "Should be true"

        // equals
        testCase "equals should return false if the strings are not equal2" <| fun _ ->
            let result = Str.equals "Hello, world!" "Hello, World!"
            Expect.isFalse result "Should be false"

        // countChar
        testCase "countChar should return the correct count" <| fun _ ->
            let result = Str.countChar 'l' "Hello, World!"
            Expect.equal result 3 "Should be 3"



        // addSuffix
        testCase "addSuffix should return the string with the suffix added" <| fun _ ->
            let result = Str.addSuffix " World!" "Hello,"
            Expect.equal result "Hello, World!" "Should be 'Hello, World!'"

        // addPrefix
        testCase "addPrefix should return the string with the prefix added" <| fun _ ->
            let result = Str.addPrefix "Hello, " "World!"
            Expect.equal result "Hello, World!" "Should be 'Hello, World!'"

        // inQuotes
        testCase "inQuotes should return the string in quotes" <| fun _ ->
            let result = Str.inQuotes "Hello, World!"
            Expect.equal result "\"Hello, World!\"" "Should be '\"Hello, World!\"'"

        // inSingleQuotes
        testCase "inSingleQuotes should return the string in single quotes" <| fun _ ->
            let result = Str.inSingleQuotes "Hello, World!"
            Expect.equal result "'Hello, World!'" "Should be ''Hello, World!''"



        // contains
        testCase "contains should return true if the string contains the substring -2" <| fun _ ->
            let result = Str.contains "World" "Hello, World!"
            Expect.isTrue result "Should be true"

        // containsIgnoreCase
        testCase "containsIgnoreCase should return true if the string contains the substring, ignoring case -2" <| fun _ ->
            let result = Str.containsIgnoreCase "world" "Hello, World!"
            Expect.isTrue result "Should be true"

        // notContains
        testCase "notContains should return true if the string does not contain the substring -2" <| fun _ ->
            let result = Str.notContains "Universe" "Hello, World!"
            Expect.isTrue result "Should be true"

        // containsChar
        testCase "containsChar should return true if the string contains the character -2" <| fun _ ->
            let result = Str.containsChar 'H' "Hello, World!"
            Expect.isTrue result "Should be true"

        // notContainsChar
        testCase "notContainsChar should return true if the string does not contain the character -2" <| fun _ ->
            let result = Str.notContainsChar 'Z' "Hello, World!"
            Expect.isTrue result "Should be true"

        // compare
        testCase "compare should return 0 if the strings are equal -2" <| fun _ ->
            let result = Str.compare "Hello, World!" "Hello, World!"
            Expect.equal result 0 "Should be 0"

        // compareIgnoreCase
        testCase "compareIgnoreCase should return 0 if the strings are equal, ignoring case -2" <| fun _ ->
            let result = Str.compareIgnoreCase "hello, world!" "Hello, World!"
            Expect.equal result 0 "Should be 0"

        // endsWith
        testCase "endsWith should return true if the string ends with the substring -2" <| fun _ ->
            let result = Str.endsWith "World!" "Hello, World!"
            Expect.isTrue result "Should be true"

        // endsWithIgnoreCase
        testCase "endsWithIgnoreCase should return true if the string ends with the substring, ignoring case -2" <| fun _ ->
            let result = Str.endsWithIgnoreCase "world!" "Hello, World!"
            Expect.isTrue result "Should be true"

        // startsWith
        testCase "startsWith should return true if the string starts with the substring -2" <| fun _ ->
            let result = Str.startsWith "Hello" "Hello, World!"
            Expect.isTrue result "Should be true"

        // startsWithIgnoreCase
        testCase "startsWithIgnoreCase should return true if the string starts with the substring, ignoring case -2" <| fun _ ->
            let result = Str.startsWithIgnoreCase "hello" "Hello, World!"
            Expect.isTrue result "Should be true"

        // equals
        testCase "equals should return true if the strings are equal -2" <| fun _ ->
            let result = Str.equals "Hello, World!" "Hello, World!"
            Expect.isTrue result "Should be true"

        testCase "Test equalsIgnoreCase function" <| fun _ ->
            let result1 = Str.equalsIgnoreCase "test" "TEST"
            let result2 = Str.equalsIgnoreCase "test" "Test1"
            Expect.isTrue result1 "Should be true"
            Expect.isFalse result2 "Should be false"

        testCase "Test indexOfChar function" <| fun _ ->
            let result1 = Str.indexOfChar 'e' "test"
            let result2 = Str.indexOfChar 'z' "test"
            Expect.equal result1 1 "Should be 1"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test indexOfCharFrom function" <| fun _ ->
            let result1 = Str.indexOfCharFrom 's' 1 "test"
            let result2 = Str.indexOfCharFrom 'z' 1 "test"
            Expect.equal result1 2 "Should be 2"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test indexOfCharFromFor function" <| fun _ ->
            let result1 = Str.indexOfCharFromFor 's' 1 2 "test"
            let result2 = Str.indexOfCharFromFor 'z' 1 2 "test"
            Expect.equal result1 2 "Should be 2"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test indexOfString function" <| fun _ ->
            let result1 = Str.indexOfString "es" "test"
            let result2 = Str.indexOfString "zz" "test"
            Expect.equal result1 1 "Should be 1"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test indexOfStringFrom function" <| fun _ ->
            let result1 = Str.indexOfStringFrom "es" 1 "test"
            let result2 = Str.indexOfStringFrom "zz" 1 "test"
            Expect.equal result1 1 "Should be 1"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test indexOfStringFromFor function" <| fun _ ->
            let result1 = Str.indexOfStringFromFor "es" 1 3 "ttesttt"
            let result2 = Str.indexOfStringFromFor "tt" 1 3 "testtt"
            let result3 = Str.indexOfStringFromFor "zz" 1 3 "zzszzz"
            Expect.equal result1 2 "Should be 2"
            Expect.equal result2 -1 "Should be -1"
            Expect.equal result3 -1 "Should be -1"

        testCase "Test indexOfAny function" <| fun _ ->
            let result1 = Str.indexOfAny [|'e'; 's'|] "test"
            let result2 = Str.indexOfAny [|'z'; 'x'|] "test"
            Expect.equal result1 1 "Should be 1"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test indexOfAnyFrom function" <| fun _ ->
            let result1 = Str.indexOfAnyFrom [|'e'; 's'|] 1 "test"
            let result2 = Str.indexOfAnyFrom [|'z'; 'x'|] 1 "test"
            Expect.equal result1 1 "Should be 1"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test indexOfAnyFromFor function" <| fun _ ->
            let result1 = Str.indexOfAnyFromFor [|'e'; 's'|] 1 2 "test"
            let result2 = Str.indexOfAnyFromFor [|'z'; 'x'|] 1 2 "test"
            Expect.equal result1 1 "Should be 1"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test insert function" <| fun _ ->
            let result1 = Str.insert 1 "es" "tt"
            let result2 = Str.insert 1 "zz" "tst"
            Expect.equal result1 "test" "Should be 'test'"
            Expect.equal result2 "tzzst" "Should be 'tzzst'"

        testCase "Test lastIndexOfChar function" <| fun _ ->
            let result1 = Str.lastIndexOfChar 't' "test"
            let result2 = Str.lastIndexOfChar 'z' "test"
            Expect.equal result1 3 "Should be 3"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test lastIndexOfCharFrom function" <| fun _ ->
            let result1 = Str.lastIndexOfCharFrom 't' 2 "test"
            let result2 = Str.lastIndexOfCharFrom 'z' 2 "test"
            Expect.equal result1 0 "Should be 0"
            Expect.equal result2 -1 "Should be -1"

        // testCase "Test lastIndexOfCharFromFor function" <| fun _ ->
        //     let result1 = Str.lastIndexOfCharFromFor 't' 2 3 "test"
        //     let result2 = Str.lastIndexOfCharFromFor 'z' 2 3 "test"
        //     Expect.equal result1 0 "Should be 0"
        //     Expect.equal result2 -1 "Should be -1"

        testCase "Test lastIndexOfString function" <| fun _ ->
            let result1 = Str.lastIndexOfString "es" "testes"
            let result2 = Str.lastIndexOfString "zz" "test"
            Expect.equal result1 4 "Should be 4"
            Expect.equal result2 -1 "Should be -1"

        testCase "Test lastIndexOfStringFrom function" <| fun _ ->
            let result1 = Str.lastIndexOfStringFrom "es" 3 "testes"
            let result2 = Str.lastIndexOfStringFrom "zz" 1 "test"
            Expect.equal result1 1 "Should be 1"
            Expect.equal result2 -1 "Should be -1"

        // testCase "Test lastIndexOfStringFromFor function" <| fun _ ->
        //     let result1 = Str.lastIndexOfStringFromFor "es" 2 2 "testes"
        //     let result2 = Str.lastIndexOfStringFromFor "zz" 1 2 "test"
        //     Expect.equal result1 1 "Should be 1"
        //     Expect.equal result2 -1 "Should be -1"

        testCase "Test padLeft function" <| fun _ ->
            let result = Str.padLeft 10 "test"
            Expect.equal result "      test" "Should be '      test'"

        testCase "Test padLeftWith function" <| fun _ ->
            let result = Str.padLeftWith 10 '*' "test"
            Expect.equal result "******test" "Should be '******test'"

        testCase "Test padRight function" <| fun _ ->
            let result = Str.padRight 10 "test"
            Expect.equal result "test      " "Should be 'test      '"

        testCase "Test padRightWith function" <| fun _ ->
            let result = Str.padRightWith 10 '*' "test"
            Expect.equal result "test******" "Should be 'test******'"

        testCase "Test remove function" <| fun _ ->
            let result = Str.remove 1 "test"
            Expect.equal result "t" "Should be 't'"

        testCase "Test removeFrom function" <| fun _ ->
            let result = Str.removeFrom 1 2 "test"
            Expect.equal result "tt" "Should be 'tt'"

        testCase "Test replaceChar function" <| fun _ ->
            let result = Str.replaceChar 'e' 'a' "teste"
            Expect.equal result "tasta" "Should be 'tast'"

        testCase "Test replace function" <| fun _ ->
            let result = Str.replace "es" "ar" "test"
            Expect.equal result "tart" "Should be 'tart'"

        testCase "Test replacefirst function" <| fun _ ->
            let result = Str.replaceFirst "es" "ar" "testes"
            Expect.equal result "tartes" "Should be 'tartes'"

        testCase "Test replacefirst function bad case " <| fun _ ->
            let result = Str.replaceFirst "eS" "ar" "testes"
            Expect.equal result "testes" "Should be still be 'testes'"

        testCase "Test replaceLast function" <| fun _ ->
            let result = Str.replaceLast "es" "ar" "testes"
            Expect.equal result "testar" "Should be 'testar'"

        testCase "Test replaceLast function bad case " <| fun _ ->
            let result = Str.replaceLast "eS" "ar" "testes"
            Expect.equal result "testes" "Should be still be 'testes'"


        testCase "Test concat function" <| fun _ ->
            let result = Str.concat "$" ["line1"; "line2"; "line3"]
            Expect.equal result "line1$line2$line3"    "Should be 'line1$line2$line3'"

        testCase "Test concatLines function" <| fun _ ->
            let result = Str.concatLines ["line1"; "line2"; "line3"]
            let nl = System.Environment.NewLine
            Expect.equal result ("line1"+nl+"line2"+nl+"line3")    ("Should be 'line1"+nl+"line2"+nl+"line3'")

        testCase "Test splitLines function" <| fun _ ->
            let result = Str.splitLines "line1\nline2\nline3"
            Expect.equal result [|"line1"; "line2"; "line3"|] "Should be ['line1'; 'line2'; 'line3']"

        testCase "Test split function" <| fun _ ->
            let result = Str.split "," "1,2,3"
            Expect.equal result [|"1"; "2"; "3"|] "Should be ['1'; '2'; '3']"


        testCase "Test split function2" <| fun _ ->
            let result = Str.split "," "1,2,,3"
            Expect.equal result [|"1"; "2"; "3"|] "Should be ['1'; '2'; '3']"


        testCase "Test splitKeep function" <| fun _ ->
            let result = Str.splitKeep "," "1,2,3,,"
            Expect.equal result [|"1"; "2"; "3"; ""; ""|] "Should be ['1'; '2'; '3'; ''; '']"

        testCase "Test splitChar function" <| fun _ ->
            let result = Str.splitChar ',' "1,2,3,,"
            Expect.equal result [|"1"; "2"; "3"|] "Should be ['1'; '2'; '3']"

        testCase "Test splitChars function" <| fun _ ->
            let result = Str.splitChars [|','; ' '|] "1,2 3,,"
            Expect.equal result [|"1"; "2"; "3"|] "Should be ['1'; '2'; '3']"

        testCase "Test splitCharKeep function" <| fun _ ->
            let result = Str.splitCharKeep ',' "1,2,3,,"
            Expect.equal result [|"1"; "2"; "3"; ""; ""|] "Should be ['1'; '2'; '3'; ''; '']"

        testCase "Test splitCharsKeep function" <| fun _ ->
            let result = Str.splitCharsKeep [|','; ' '|] "1,2 3,,"
            Expect.equal result [|"1"; "2"; "3"; ""; ""|] "Should be ['1'; '2'; '3'; ''; '']"

        testCase "Test substringFrom function" <| fun _ ->
            let result = Str.substringFrom 1 "test"
            Expect.equal result "est" "Should be 'est'"

        testCase "Test substringFromFor function" <| fun _ ->
            let result = Str.substringFromFor 1 2 "test"
            Expect.equal result "es" "Should be 'es'"

        testCase "Test toCharArray function" <| fun _ ->
            let result = Str.toCharArray "test"
            Expect.equal result [|'t'; 'e'; 's'; 't'|] "Should be ['t'; 'e'; 's'; 't']"

        testCase "Test toCharArrayFromFor function" <| fun _ ->
            let result = Str.toCharArrayFromFor 1 2 "test"
            Expect.equal result [|'e'; 's'|] "Should be ['e'; 's']"

        testCase "Test toLower function" <| fun _ ->
            let result = Str.toLower "TEST"
            Expect.equal result "test" "Should be 'test'"

        testCase "Test toUpper function" <| fun _ ->
            let result = Str.toUpper "test"
            Expect.equal result "TEST" "Should be 'TEST'"

        testCase "Test trim function" <| fun _ ->
            let result = Str.trim "  test  "
            Expect.equal result "test" "Should be 'test'"

        testCase "Test trimChar function" <| fun _ ->
            let result = Str.trimChar '*' "*test*"
            Expect.equal result "test" "Should be 'test'"

        testCase "Test trimChars function" <| fun _ ->
            let result = Str.trimChars [|' '; '*'|] " *test* "
            Expect.equal result "test" "Should be 'test'"

        testCase "Test trimEnd function" <| fun _ ->
            let result = Str.trimEnd "test  "
            Expect.equal result "test" "Should be 'test'"

        testCase "Test trimEndChar function" <| fun _ ->
            let result = Str.trimEndChar '*' "test*"
            Expect.equal result "test" "Should be 'test'"

        testCase "Test trimEndChars function" <| fun _ ->
            let result = Str.trimEndChars [|' '; '*'|] "test* "
            Expect.equal result "test" "Should be 'test'"

        testCase "Test trimStart function" <| fun _ ->
            let result = Str.trimStart "  test"
            Expect.equal result "test" "Should be 'test'"

        testCase "Test trimStartChar function" <| fun _ ->
            let result = Str.trimStartChar '*' "*test"
            Expect.equal result "test" "Should be 'test'"

        testCase "Test trimStartChars function" <| fun _ ->
            let result = Str.trimStartChars [|' '; '*'|] " *test"
            Expect.equal result "test" "Should be 'test'"

        testCase "Test formatInOneLine function" <| fun _ ->
            let result = Str.formatInOneLine "line1\nline2\nline3"
            Expect.equal result "line1 line2 line3" "Should be 'line1 line2 line3'"

        testCase "Test formatTruncated function" <| fun _ ->
            let result = Str.formatTruncated 26 "This is a very long string that should be truncated"
            Expect.equal result "\"This is a very long(...)ed\"" "Should be '\"This is a very long(...)ed\"'"

        testCase "Test formatTruncatedToMaxLines function" <| fun _ ->
            let result = Str.formatTruncatedToMaxLines 2 "line1\r\nline2\r\nline3\r\nline4\r\nline5"
            Expect.equal result "\"line1\r\nline2\r\n(... and 3 more lines.)\"" "Should be '\"line1\nline2\n... and 3 more lines.\"'"



        testCase "str builder " <| fun _ ->
            let result =
                str{
                    "Hello"
                    " "
                    "World"
                }
            Expect.equal result "Hello World" "Should be 'Hello World'"

        // ============ str computation expression comprehensive tests ============
        testCase "str builder with char yield" <| fun _ ->
            let result = str { 'H'; 'i' }
            Expect.equal result "Hi" "Should be 'Hi'"

        testCase "str builder with int yield" <| fun _ ->
            let result = str { 42 }
            Expect.equal result "42" "Should be '42'"

        testCase "str builder with Guid yield" <| fun _ ->
            let g = System.Guid.Empty
            let result = str { g }
            Expect.equal result "00000000-0000-0000-0000-000000000000" "Should be empty guid"

        testCase "str builder with yield! for string (with newline)" <| fun _ ->
            let result = str { yield! "Hello" ; "World" }
            Expect.isTrue (result.Contains("Hello")) "Should contain Hello"
            Expect.isTrue (result.Contains("World")) "Should contain World"
            Expect.isTrue (result.Contains("\n") || result.Contains("\r")) "Should contain newline"

        testCase "str builder with yield! for char (with newline)" <| fun _ ->
            let result = str { yield! 'A' ; 'B' }
            Expect.isTrue (result.Contains("A")) "Should contain A"
            Expect.isTrue (result.Contains("B")) "Should contain B"

        testCase "str builder with yield! for int (with newline)" <| fun _ ->
            let result = str { yield! 1 ; 2 }
            Expect.isTrue (result.Contains("1")) "Should contain 1"
            Expect.isTrue (result.Contains("2")) "Should contain 2"

        testCase "str builder with seq of strings" <| fun _ ->
            let lines = ["Line1"; "Line2"; "Line3"]
            let result = str { yield lines }
            Expect.isTrue (result.Contains("Line1")) "Should contain Line1"
            Expect.isTrue (result.Contains("Line2")) "Should contain Line2"
            Expect.isTrue (result.Contains("Line3")) "Should contain Line3"

        testCase "str builder with for loop" <| fun _ ->
            let result = str {
                for i in 1..3 do
                    yield i.ToString()
            }
            Expect.equal result "123" "Should be '123'"

        testCase "str builder with for loop and strings" <| fun _ ->
            let items = ["A"; "B"; "C"]
            let result = str {
                for item in items do
                    yield item
            }
            Expect.equal result "ABC" "Should be 'ABC'"

        testCase "str builder with while loop" <| fun _ ->
            let mutable count = 0
            let result = str {
                while count < 3 do
                    yield "X"
                    count <- count + 1
            }
            Expect.equal result "XXX" "Should be 'XXX'"

        testCase "str builder empty" <| fun _ ->
            let result = str { () }
            Expect.equal result "" "Should be empty string"

        testCase "str builder mixed types" <| fun _ ->
            let result = str {
                "Count: "
                42
                ", Char: "
                'X'
            }
            Expect.equal result "Count: 42, Char: X" "Should combine different types"



        // normalize
        testCase "normalize should throw an exception if input string is null" <| fun _ ->
            Expect.throws (fun _ -> Str.normalize null |> ignore<string>) "Should throw an exception"

        // normalize
        testCase "normalize should return the normalized string" <| fun _ ->
            let result = Str.normalize "Héllò, Wörld!"
            Expect.equal result "Hello, World!" "Should be 'Hello, World!'"

        // normalize
        testCase "normalize" <| fun _ ->
            let result = Str.normalize "crème brûlée"
            Expect.equal result "creme brulee" "Should be equal"

        // normalize
        testCase "normalize2" <| fun _ ->
            let result = Str.normalize "crèmeö brûlée"
            Expect.equal result "cremeo brulee" "Should be equal"


        #if FABLE_COMPILER_JAVASCRIPT || FABLE_COMPILER_TYPESCRIPT
        // error FABLE: Microsoft.FSharp.Core.Operators.ArrayExtensions.String.GetReverseIndex is not supported by Fable
        #else
        // slice
        testCase "neg slice2 should return a substring from the start index to the end index" <| fun _ ->
            let result = "Hello, World!".[1..^1]
            Expect.equal result "ello, World" "Should be equal"
        #endif



        // addThousandSeparators tests
        testCase "addThousandSeparators should format integer with apostrophe separator" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "1234567"
            Expect.equal result "1'234'567" "Should be '1'234'567'"

        testCase "addThousandSeparators should format integer with comma separator" <| fun _ ->
            let result = Str.addThousandSeparators ',' "1234567"
            Expect.equal result "1,234,567" "Should be '1,234,567'"

        testCase "addThousandSeparators should format small integer without separator" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "123"
            Expect.equal result "123" "Should be '123'"

        testCase "addThousandSeparators should format negative integer" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "-1234567"
            Expect.equal result "-1'234'567" "Should be '-1'234'567'"

        testCase "addThousandSeparators should format float with decimal part" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "1234567.89"
            Expect.equal result "1'234'567.89" "Should be '1'234'567.89'"

        testCase "addThousandSeparators should format float with long decimal part" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "1234.5678901234"
            Expect.equal result "1'234.567'890'123'4" "Should be '1'234.567'890'123'4'"

        testCase "addThousandSeparators should format negative float" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "-9876543.21"
            Expect.equal result "-9'876'543.21" "Should be '-9'876'543.21'"

        testCase "addThousandSeparators should handle single digit" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "5"
            Expect.equal result "5" "Should be '5'"

        testCase "addThousandSeparators should handle zero" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "0"
            Expect.equal result "0" "Should be '0'"

        testCase "addThousandSeparators should format exactly 1000" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "1000"
            Expect.equal result "1'000" "Should be '1'000'"

        testCase "addThousandSeparators should format exactly 1 million" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "1000000"
            Expect.equal result "1'000'000" "Should be '1'000'000'"

        testCase "addThousandSeparators should handle decimal only number" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "0.123456789"
            Expect.equal result "0.123'456'789" "Should be '0.123'456'789'"

        testCase "addThousandSeparators should format negative with scientific notation" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "-123456.789e-5"
            Expect.equal result "-123'456.789e-5" "Should be '-123'456.789e-5'"

        testCase "addThousandSeparators should  format negative with scientific notation" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "-1.23456789e-5"
            Expect.equal result "-1.234'567'89e-5" "Should be '-1.234'567'89e-5'"

        testCase "addThousandSeparators should work with underscore separator" <| fun _ ->
            let result = Str.addThousandSeparators '_' "123456789"
            Expect.equal result "123_456_789" "Should be '123_456_789'"

        testCase "addThousandSeparators should work with space separator" <| fun _ ->
            let result = Str.addThousandSeparators ' ' "123456789"
            Expect.equal result "123 456 789" "Should be '123 456 789'"

        testCase "addThousandSeparators should work after decimal point" <| fun _ ->
            let result = Str.addThousandSeparators '_' "12345.6789"
            Expect.equal result "12_345.678_9" "Should be '12_345.678_9'"


        testCase "addThousandSeparators should format with scientific notation" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "1234567e10"
            Expect.equal result "1'234'567e10" "Should be '1234567e10'"

        testCase "addThousandSeparators should format with scientific notation neg" <| fun _ ->
            let result = Str.addThousandSeparators '\'' "-1234567e10"
            Expect.equal result "-1'234'567e10" "Should be '-1234567e10'"

        // ============ Str.get tests ============
        testCase "Str.get should return character at index" <| fun _ ->
            let result = Str.get 0 "Hello"
            Expect.equal result 'H' "Should be 'H'"

        testCase "Str.get should return character at middle index" <| fun _ ->
            let result = Str.get 2 "Hello"
            Expect.equal result 'l' "Should be 'l'"

        testCase "Str.get should return character at last index" <| fun _ ->
            let result = Str.get 4 "Hello"
            Expect.equal result 'o' "Should be 'o'"

        testCase "Str.get should throw for null string" <| fun _ ->
            Expect.throws (fun _ -> Str.get 0 null |> ignore) "Should throw for null"

        testCase "Str.get should throw for negative index" <| fun _ ->
            Expect.throws (fun _ -> Str.get -1 "Hello" |> ignore) "Should throw for negative index"

        testCase "Str.get should throw for index out of range" <| fun _ ->
            Expect.throws (fun _ -> Str.get 10 "Hello" |> ignore) "Should throw for out of range"

        testCase "Str.get should throw for empty string" <| fun _ ->
            Expect.throws (fun _ -> Str.get 0 "" |> ignore) "Should throw for empty string"

        // ============ Str.isWhite tests ============
        testCase "Str.isWhite should return true for null" <| fun _ ->
            let result = Str.isWhite null
            Expect.isTrue result "Should be true for null"

        testCase "Str.isWhite should return true for empty string" <| fun _ ->
            let result = Str.isWhite ""
            Expect.isTrue result "Should be true for empty"

        testCase "Str.isWhite should return true for whitespace only" <| fun _ ->
            let result = Str.isWhite "   "
            Expect.isTrue result "Should be true for whitespace"

        testCase "Str.isWhite should return true for tabs and newlines" <| fun _ ->
            let result = Str.isWhite "\t\n\r"
            Expect.isTrue result "Should be true for tabs/newlines"

        testCase "Str.isWhite should return false for non-whitespace" <| fun _ ->
            let result = Str.isWhite "Hello"
            Expect.isFalse result "Should be false for text"

        testCase "Str.isWhite should return false for whitespace with text" <| fun _ ->
            let result = Str.isWhite "  Hello  "
            Expect.isFalse result "Should be false for text with whitespace"

        // ============ Str.isNotWhite tests ============
        testCase "Str.isNotWhite should return false for null" <| fun _ ->
            let result = Str.isNotWhite null
            Expect.isFalse result "Should be false for null"

        testCase "Str.isNotWhite should return false for empty string" <| fun _ ->
            let result = Str.isNotWhite ""
            Expect.isFalse result "Should be false for empty"

        testCase "Str.isNotWhite should return false for whitespace only" <| fun _ ->
            let result = Str.isNotWhite "   "
            Expect.isFalse result "Should be false for whitespace"

        testCase "Str.isNotWhite should return true for non-whitespace" <| fun _ ->
            let result = Str.isNotWhite "Hello"
            Expect.isTrue result "Should be true for text"

        testCase "Str.isNotWhite should return true for whitespace with text" <| fun _ ->
            let result = Str.isNotWhite "  Hello  "
            Expect.isTrue result "Should be true for text with whitespace"

        // ============ Str.isEmpty tests ============
        testCase "Str.isEmpty should return true for null" <| fun _ ->
            let result = Str.isEmpty null
            Expect.isTrue result "Should be true for null"

        testCase "Str.isEmpty should return true for empty string" <| fun _ ->
            let result = Str.isEmpty ""
            Expect.isTrue result "Should be true for empty"

        testCase "Str.isEmpty should return false for whitespace" <| fun _ ->
            let result = Str.isEmpty "   "
            Expect.isFalse result "Should be false for whitespace (not empty)"

        testCase "Str.isEmpty should return false for non-empty string" <| fun _ ->
            let result = Str.isEmpty "Hello"
            Expect.isFalse result "Should be false for text"

        // ============ Str.isNotEmpty tests ============
        testCase "Str.isNotEmpty should return false for null" <| fun _ ->
            let result = Str.isNotEmpty null
            Expect.isFalse result "Should be false for null"

        testCase "Str.isNotEmpty should return false for empty string" <| fun _ ->
            let result = Str.isNotEmpty ""
            Expect.isFalse result "Should be false for empty"

        testCase "Str.isNotEmpty should return true for whitespace" <| fun _ ->
            let result = Str.isNotEmpty "   "
            Expect.isTrue result "Should be true for whitespace (not empty)"

        testCase "Str.isNotEmpty should return true for non-empty string" <| fun _ ->
            let result = Str.isNotEmpty "Hello"
            Expect.isTrue result "Should be true for text"

        // ============================================================
        // Extensive tests for functions with FABLE_COMPILER directives
        // to ensure JS and .NET runtimes behave the same way
        // ============================================================

        // ============ containsIgnoreCase - extensive tests ============

        testCase "containsIgnoreCase: exact match same case" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "Hello" "Hello") "exact match"

        testCase "containsIgnoreCase: all uppercase needle in lowercase haystack" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "HELLO" "hello world") "upper in lower"

        testCase "containsIgnoreCase: all lowercase needle in uppercase haystack" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "hello" "HELLO WORLD") "lower in upper"

        testCase "containsIgnoreCase: mixed case needle" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "hElLo" "Hello World") "mixed case"

        testCase "containsIgnoreCase: empty needle in non-empty haystack" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "" "Hello") "empty needle always found"

        testCase "containsIgnoreCase: empty needle in empty haystack" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "" "") "empty in empty"

        testCase "containsIgnoreCase: non-empty needle in empty haystack" <| fun _ ->
            Expect.isFalse (Str.containsIgnoreCase "x" "") "can't find in empty"

        testCase "containsIgnoreCase: needle longer than haystack" <| fun _ ->
            Expect.isFalse (Str.containsIgnoreCase "Hello World!" "Hello") "needle longer"

        testCase "containsIgnoreCase: single char match" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "a" "A") "single char case insensitive"

        testCase "containsIgnoreCase: single char no match" <| fun _ ->
            Expect.isFalse (Str.containsIgnoreCase "z" "A") "single char no match"

        testCase "containsIgnoreCase: substring at start" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "hel" "Hello") "at start"

        testCase "containsIgnoreCase: substring at end" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "LLO" "Hello") "at end"

        testCase "containsIgnoreCase: substring in middle" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "LL" "Hello") "in middle"

        testCase "containsIgnoreCase: with digits" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "abc123" "xABC123y") "digits are case insensitive irrelevant"

        testCase "containsIgnoreCase: with special characters" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "!@#" "hello!@#world") "special chars"

        testCase "containsIgnoreCase: whitespace matters" <| fun _ ->
            Expect.isFalse (Str.containsIgnoreCase "hello world" "helloworld") "whitespace matters"

        testCase "containsIgnoreCase: repeated pattern" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "aa" "AAA") "repeated pattern"

        testCase "containsIgnoreCase: null haystack throws" <| fun _ ->
            Expect.throws (fun _ -> Str.containsIgnoreCase "x" null |> ignore<bool>) "null haystack"

        testCase "containsIgnoreCase: null needle throws" <| fun _ ->
            Expect.throws (fun _ -> Str.containsIgnoreCase null "hello" |> ignore<bool>) "null needle"

        testCase "containsIgnoreCase: both null throws" <| fun _ ->
            Expect.throws (fun _ -> Str.containsIgnoreCase null null |> ignore<bool>) "both null"

        testCase "containsIgnoreCase: unicode letters" <| fun _ ->
            Expect.isTrue (Str.containsIgnoreCase "über" "ÜBER cool") "unicode case"

        testCase "containsIgnoreCase: very long string" <| fun _ ->
            let haystack = String.replicate 1000 "ab" + "XY" + String.replicate 1000 "cd"
            Expect.isTrue (Str.containsIgnoreCase "xy" haystack) "find in long string"

        testCase "containsIgnoreCase: near miss" <| fun _ ->
            Expect.isFalse (Str.containsIgnoreCase "abd" "abc") "near miss"


        // ============ indexOfCharFromFor - extensive tests ============

        testCase "indexOfCharFromFor: find char at start of search range" <| fun _ ->
            let result = Str.indexOfCharFromFor 'a' 0 3 "abc"
            Expect.equal result 0 "char at start"

        testCase "indexOfCharFromFor: find char at end of search range" <| fun _ ->
            let result = Str.indexOfCharFromFor 'c' 0 3 "abc"
            Expect.equal result 2 "char at end of range"

        testCase "indexOfCharFromFor: char outside search range returns -1" <| fun _ ->
            let result = Str.indexOfCharFromFor 'c' 0 2 "abc"
            Expect.equal result -1 "char outside range"

        testCase "indexOfCharFromFor: search from middle" <| fun _ ->
            let result = Str.indexOfCharFromFor 'b' 1 2 "abc"
            Expect.equal result 1 "find from middle"

        testCase "indexOfCharFromFor: char not present returns -1" <| fun _ ->
            let result = Str.indexOfCharFromFor 'z' 0 3 "abc"
            Expect.equal result -1 "char not present"

        testCase "indexOfCharFromFor: duplicate chars finds first in range" <| fun _ ->
            let result = Str.indexOfCharFromFor 'a' 0 5 "abaca"
            Expect.equal result 0 "first occurrence"

        testCase "indexOfCharFromFor: duplicate chars finds first in range starting later" <| fun _ ->
            let result = Str.indexOfCharFromFor 'a' 1 4 "abaca"
            Expect.equal result 2 "first occurrence after start"

        testCase "indexOfCharFromFor: count of 1" <| fun _ ->
            let result = Str.indexOfCharFromFor 'a' 0 1 "abc"
            Expect.equal result 0 "count 1 found"

        testCase "indexOfCharFromFor: count of 1 not found" <| fun _ ->
            let result = Str.indexOfCharFromFor 'b' 0 1 "abc"
            Expect.equal result -1 "count 1 not found"

        testCase "indexOfCharFromFor: search entire string" <| fun _ ->
            let result = Str.indexOfCharFromFor 'o' 0 13 "Hello, World!"
            Expect.equal result 4 "search entire string"

        testCase "indexOfCharFromFor: search with startIndex past char" <| fun _ ->
            let result = Str.indexOfCharFromFor 'H' 1 5 "Hello, World!"
            Expect.equal result -1 "start past the char"

        testCase "indexOfCharFromFor: find second occurrence" <| fun _ ->
            let result = Str.indexOfCharFromFor 'l' 3 3 "Hello, World!"
            Expect.equal result 3 "second l"

        testCase "indexOfCharFromFor: null input throws" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfCharFromFor 'a' 0 1 null |> ignore) "null throws"

        testCase "indexOfCharFromFor: space character" <| fun _ ->
            let result = Str.indexOfCharFromFor ' ' 0 7 "Hello, World!"
            Expect.equal result 6 "find space"

        testCase "indexOfCharFromFor: search in single char string found" <| fun _ ->
            let result = Str.indexOfCharFromFor 'x' 0 1 "x"
            Expect.equal result 0 "single char found"

        testCase "indexOfCharFromFor: search in single char string not found" <| fun _ ->
            let result = Str.indexOfCharFromFor 'y' 0 1 "x"
            Expect.equal result -1 "single char not found"

        testCase "indexOfCharFromFor: multiple identical chars" <| fun _ ->
            let result = Str.indexOfCharFromFor 'a' 2 3 "aaaaa"
            Expect.equal result 2 "finds at start of range in repeated chars"

        testCase "indexOfCharFromFor: last position in range" <| fun _ ->
            let result = Str.indexOfCharFromFor 'c' 0 3 "xxc"
            Expect.equal result 2 "last position in range"

        testCase "indexOfCharFromFor: newline character" <| fun _ ->
            let result = Str.indexOfCharFromFor '\n' 0 6 "ab\ncd\n"
            Expect.equal result 2 "find newline"

        testCase "indexOfCharFromFor: tab character" <| fun _ ->
            let result = Str.indexOfCharFromFor '\t' 0 4 "a\tb\t"
            Expect.equal result 1 "find tab"


        // ============ indexOfStringFromFor - extensive tests ============

        testCase "indexOfStringFromFor: find at start of range" <| fun _ ->
            let result = Str.indexOfStringFromFor "ab" 0 3 "abc"
            Expect.equal result 0 "at start"

        testCase "indexOfStringFromFor: find at end of range" <| fun _ ->
            let result = Str.indexOfStringFromFor "cd" 1 4 "abcde"
            Expect.equal result 2 "at end of range"

        testCase "indexOfStringFromFor: not found in range" <| fun _ ->
            let result = Str.indexOfStringFromFor "de" 0 3 "abcde"
            Expect.equal result -1 "not in range"

        testCase "indexOfStringFromFor: empty search string" <| fun _ ->
            let result = Str.indexOfStringFromFor "" 0 3 "abc"
            Expect.equal result 0 "empty string found at startIndex"

        testCase "indexOfStringFromFor: exact match whole range" <| fun _ ->
            let result = Str.indexOfStringFromFor "abc" 0 3 "abc"
            Expect.equal result 0 "exact match"

        testCase "indexOfStringFromFor: pattern longer than search range" <| fun _ ->
            let result = Str.indexOfStringFromFor "abcd" 0 3 "abcde"
            Expect.equal result -1 "pattern longer than range"

        testCase "indexOfStringFromFor: single char pattern" <| fun _ ->
            let result = Str.indexOfStringFromFor "c" 0 3 "abc"
            Expect.equal result 2 "single char"

        testCase "indexOfStringFromFor: duplicate patterns finds first in range" <| fun _ ->
            let result = Str.indexOfStringFromFor "ab" 0 6 "ababab"
            Expect.equal result 0 "first occurrence"

        testCase "indexOfStringFromFor: duplicate patterns from offset" <| fun _ ->
            let result = Str.indexOfStringFromFor "ab" 1 5 "ababab"
            Expect.equal result 2 "first after offset"

        testCase "indexOfStringFromFor: overlapping pattern" <| fun _ ->
            let result = Str.indexOfStringFromFor "aba" 0 5 "ababa"
            Expect.equal result 0 "overlapping"

        testCase "indexOfStringFromFor: overlapping pattern from offset" <| fun _ ->
            let result = Str.indexOfStringFromFor "aba" 1 4 "ababa"
            Expect.equal result 2 "overlapping from offset"

        testCase "indexOfStringFromFor: null needle throws" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfStringFromFor null 0 3 "abc" |> ignore) "null needle"

        testCase "indexOfStringFromFor: null haystack throws" <| fun _ ->
            Expect.throws (fun _ -> Str.indexOfStringFromFor "ab" 0 3 null |> ignore) "null haystack"

        testCase "indexOfStringFromFor: search in single char string" <| fun _ ->
            let result = Str.indexOfStringFromFor "x" 0 1 "x"
            Expect.equal result 0 "single char string found"

        testCase "indexOfStringFromFor: search in single char string not found" <| fun _ ->
            let result = Str.indexOfStringFromFor "y" 0 1 "x"
            Expect.equal result -1 "single char string not found"

        testCase "indexOfStringFromFor: with special characters" <| fun _ ->
            let result = Str.indexOfStringFromFor "!@" 0 5 "hi!@#"
            Expect.equal result 2 "special chars"

        testCase "indexOfStringFromFor: with whitespace pattern" <| fun _ ->
            let result = Str.indexOfStringFromFor " " 0 6 "a b c "
            Expect.equal result 1 "whitespace pattern"

        testCase "indexOfStringFromFor: case sensitive" <| fun _ ->
            let result = Str.indexOfStringFromFor "AB" 0 3 "abc"
            Expect.equal result -1 "case sensitive"

        testCase "indexOfStringFromFor: pattern at exact boundary" <| fun _ ->
            let result = Str.indexOfStringFromFor "cd" 2 2 "abcde"
            Expect.equal result 2 "at boundary"

        testCase "indexOfStringFromFor: start at last position count 1" <| fun _ ->
            let result = Str.indexOfStringFromFor "e" 4 1 "abcde"
            Expect.equal result 4 "last position"

        testCase "indexOfStringFromFor: long pattern in long string" <| fun _ ->
            let s = String.replicate 100 "ab" + "XYZ" + String.replicate 100 "cd"
            let result = Str.indexOfStringFromFor "XYZ" 0 s.Length s
            Expect.equal result 200 "long string"

        testCase "indexOfStringFromFor: long pattern only in narrow range" <| fun _ ->
            let s = String.replicate 100 "ab" + "XYZ" + String.replicate 100 "cd"
            let result = Str.indexOfStringFromFor "XYZ" 0 200 s
            Expect.equal result -1 "not in narrow range"

        testCase "indexOfStringFromFor: repeated single char" <| fun _ ->
            let result = Str.indexOfStringFromFor "a" 3 2 "aaaaa"
            Expect.equal result 3 "repeated single"


        // ============ normalize - extensive tests ============

        testCase "normalize: null throws" <| fun _ ->
            Expect.throws (fun _ -> Str.normalize null |> ignore<string>) "null throws"

        testCase "normalize: empty string" <| fun _ ->
            let result = Str.normalize ""
            Expect.equal result "" "empty stays empty"

        testCase "normalize: plain ASCII unchanged" <| fun _ ->
            let result = Str.normalize "Hello World"
            Expect.equal result "Hello World" "ASCII unchanged"

        testCase "normalize: digits and punctuation unchanged" <| fun _ ->
            let result = Str.normalize "123!@#$%^&*()"
            Expect.equal result "123!@#$%^&*()" "digits and punctuation"

        testCase "normalize: acute accent e" <| fun _ ->
            let result = Str.normalize "é"
            Expect.equal result "e" "acute e"

        testCase "normalize: grave accent e" <| fun _ ->
            let result = Str.normalize "è"
            Expect.equal result "e" "grave e"

        testCase "normalize: circumflex accent e" <| fun _ ->
            let result = Str.normalize "ê"
            Expect.equal result "e" "circumflex e"

        testCase "normalize: diaeresis accent e" <| fun _ ->
            let result = Str.normalize "ë"
            Expect.equal result "e" "diaeresis e"

        testCase "normalize: tilde n" <| fun _ ->
            let result = Str.normalize "ñ"
            Expect.equal result "n" "tilde n"

        testCase "normalize: umlaut characters" <| fun _ ->
            let result = Str.normalize "äöü"
            Expect.equal result "aou" "umlauts"

        testCase "normalize: uppercase accented" <| fun _ ->
            let result = Str.normalize "ÀÁÂÃÄÅ"
            Expect.equal result "AAAAAA" "uppercase accented A variants"

        testCase "normalize: cedilla" <| fun _ ->
            let result = Str.normalize "ç"
            Expect.equal result "c" "cedilla"

        testCase "normalize: uppercase cedilla" <| fun _ ->
            let result = Str.normalize "Ç"
            Expect.equal result "C" "uppercase cedilla"

        testCase "normalize: mixed accented and plain" <| fun _ ->
            let result = Str.normalize "café"
            Expect.equal result "cafe" "mixed"

        testCase "normalize: full sentence with accents" <| fun _ ->
            let result = Str.normalize "Les élèves français"
            Expect.equal result "Les eleves francais" "full sentence"

        testCase "normalize: crème brûlée" <| fun _ ->
            let result = Str.normalize "crème brûlée"
            Expect.equal result "creme brulee" "creme brulee"

        testCase "normalize: whitespace preserved" <| fun _ ->
            let result = Str.normalize "  \t\n  "
            Expect.equal result "  \t\n  " "whitespace preserved"

        testCase "normalize: single accented character" <| fun _ ->
            let result = Str.normalize "ö"
            Expect.equal result "o" "single accented"

        testCase "normalize: multiple accents on same base (precomposed)" <| fun _ ->
            // Vietnamese: ồ = o + combining breve + combining grave
            let result = Str.normalize "ồ"
            // Should at least remove diacritics, result should be plain o
            Expect.equal result "o" "multiple accents"

        testCase "normalize: string with no accents is idempotent" <| fun _ ->
            let input = "The quick brown fox jumps over the lazy dog 1234567890"
            let result = Str.normalize input
            Expect.equal result input "no change for plain ASCII"

        testCase "normalize: accented string normalized twice is same as once" <| fun _ ->
            let input = "crème brûlée"
            let once = Str.normalize input
            let twice = Str.normalize once
            Expect.equal twice once "idempotent"

        testCase "normalize: Scandinavian characters" <| fun _ ->
            let result = Str.normalize "Ångström"
            Expect.equal result "Angstrom" "Scandinavian"

        testCase "normalize: Spanish text" <| fun _ ->
            let result = Str.normalize "El niño está aquí"
            Expect.equal result "El nino esta aqui" "Spanish"

        testCase "normalize: German text" <| fun _ ->
            let result = Str.normalize "Ärger über böse Füße"
            Expect.equal result "Arger uber bose Fuße" "German - sharp s preserved"

        testCase "normalize: only accents (combining characters)" <| fun _ ->
            // standalone combining acute accent
            let result = Str.normalize "\u0301"
            Expect.equal result "" "standalone combining accent removed"

        testCase "normalize: preserves non-Latin scripts without accents" <| fun _ ->
            // CJK and numbers should pass through
            let result = Str.normalize "abc123"
            Expect.equal result "abc123" "basic latin+digits"

        // --- additional normalize tests ---

        testCase "normalize: brackets and special symbols preserved" <| fun _ ->
            let input = "[]{}()<>|\\~`_-+=/"
            Expect.equal (Str.normalize input) input "brackets and symbols"

        testCase "normalize: quotes preserved" <| fun _ ->
            let input = "\"hello\" 'world'"
            Expect.equal (Str.normalize input) input "quotes"

        testCase "normalize: decomposed form (NFD) e + combining acute" <| fun _ ->
            // e (U+0065) + combining acute (U+0301) = é in NFD
            let input = "e\u0301"
            let result = Str.normalize input
            Expect.equal result "e" "decomposed acute removed"

        testCase "normalize: decomposed form a + combining ring above" <| fun _ ->
            // a (U+0061) + combining ring above (U+030A) = å in NFD
            let input = "a\u030A"
            let result = Str.normalize input
            Expect.equal result "a" "decomposed ring removed"

        testCase "normalize: multiple combining marks on one base" <| fun _ ->
            // a + combining acute + combining tilde
            let input = "a\u0301\u0303"
            let result = Str.normalize input
            Expect.equal result "a" "multiple combining marks removed"

        testCase "normalize: precomposed vs decomposed same result" <| fun _ ->
            let precomposed = Str.normalize "é"      // U+00E9
            let decomposed = Str.normalize "e\u0301" // e + combining acute
            Expect.equal precomposed decomposed "precomposed and decomposed yield same result"

        testCase "normalize: CJK characters preserved" <| fun _ ->
            let input = "\u4F60\u597D\u4E16\u754C" // 你好世界
            Expect.equal (Str.normalize input) input "CJK unchanged"

        testCase "normalize: Cyrillic without accents preserved" <| fun _ ->
            let input = "\u041F\u0440\u0438\u0432\u0435\u0442" // Привет
            Expect.equal (Str.normalize input) input "Cyrillic preserved"

        testCase "normalize: Cyrillic with combining accent" <| fun _ ->
            // и (U+0438) + combining acute (U+0301) → и
            let input = "\u0438\u0301"
            let result = Str.normalize input
            Expect.equal result "\u0438" "Cyrillic accent removed"

        testCase "normalize: Greek without accents preserved" <| fun _ ->
            let input = "\u03B1\u03B2\u03B3" // αβγ
            Expect.equal (Str.normalize input) input "Greek preserved"

        testCase "normalize: Greek with tonos" <| fun _ ->
            // ά (U+03AC, alpha with tonos) → α
            let result = Str.normalize "\u03AC"
            Expect.equal result "\u03B1" "Greek tonos removed"

        testCase "normalize: Polish text" <| fun _ ->
            let result = Str.normalize "\u0179\u00F3\u0142\u0107"  // Źółć
            // ł (U+0142) is a distinct letter, not a base + combining mark, so it stays
            Expect.equal result "Zo\u0142c" "Polish diacritics removed, ł preserved"

        testCase "normalize: Czech text" <| fun _ ->
            let result = Str.normalize "\u0159\u00E1\u010D\u0161\u0165" // řáčšť
            Expect.equal result "racst" "Czech diacritics removed"

        testCase "normalize: Turkish dotted and dotless i" <| fun _ ->
            // İ (U+0130, capital I with dot) should lose the dot
            let result = Str.normalize "\u0130"
            Expect.equal result "I" "Turkish capital dotted I"

        testCase "normalize: Vietnamese text" <| fun _ ->
            let result = Str.normalize "\u1ED3\u1EA5\u1EBF"  // ồấế
            Expect.equal result "oae" "Vietnamese diacritics removed"

        testCase "normalize: single char string" <| fun _ ->
            Expect.equal (Str.normalize "a") "a" "single plain char"

        testCase "normalize: string of only combining marks" <| fun _ ->
            // two standalone combining marks (acute + grave)
            let result = Str.normalize "\u0301\u0300"
            Expect.equal result "" "only combining marks → empty"

        testCase "normalize: accented chars interspersed with numbers" <| fun _ ->
            let result = Str.normalize "\u00E91\u00E82\u00F63"  // é1è2ö3
            Expect.equal result "e1e2o3" "accents removed, digits kept"

        testCase "normalize: tabs and newlines with accents" <| fun _ ->
            let result = Str.normalize "\u00E9\t\u00E8\n\u00F6"  // é\tè\nö
            Expect.equal result "e\te\no" "whitespace preserved with accents"

        testCase "normalize: long string performance" <| fun _ ->
            let input = String.replicate 1000 "\u00E9"  // 1000 × é
            let result = Str.normalize input
            Expect.equal result (String.replicate 1000 "e") "long accented string"

        testCase "normalize: mixed scripts in one string" <| fun _ ->
            // Latin accented + CJK + Cyrillic + digits
            let input = "caf\u00E9\u4F60\u597D\u041F\u0440\u0438\u0432\u0435\u044242"
            let result = Str.normalize input
            Expect.equal result "cafe\u4F60\u597D\u041F\u0440\u0438\u0432\u0435\u044242" "mixed scripts"

        testCase "normalize: sharp s (ß) preserved" <| fun _ ->
            // ß (U+00DF) is not a diacritic, should stay
            let result = Str.normalize "\u00DF"
            Expect.equal result "\u00DF" "sharp s unchanged"

        testCase "normalize: eth (ð) preserved" <| fun _ ->
            let result = Str.normalize "\u00F0"
            Expect.equal result "\u00F0" "eth unchanged"

        testCase "normalize: thorn (þ) preserved" <| fun _ ->
            let result = Str.normalize "\u00FE"
            Expect.equal result "\u00FE" "thorn unchanged"

        testCase "normalize: copyright and trademark symbols preserved" <| fun _ ->
            let input = "\u00A9\u00AE\u2122"  // ©®™
            Expect.equal (Str.normalize input) input "symbols preserved"

        testCase "normalize: currency symbols preserved" <| fun _ ->
            let input = "$\u20AC\u00A3\u00A5"  // $€£¥
            Expect.equal (Str.normalize input) input "currency symbols"

        testCase "normalize: result length <= input length" <| fun _ ->
            let input = "\u00C0\u00C1\u00C2\u00C3\u00C4\u00C5\u00C7\u00C8\u00C9\u00CA"  // ÀÁÂÃÄÅÇÈÉÊ
            let result = Str.normalize input
            Expect.isTrue (result.Length <= input.Length) "result not longer than input"

        testCase "normalize: triple application is same as single" <| fun _ ->
            let input = "\u00C4rger \u00FCber b\u00F6se F\u00FC\u00DFe"  // Ärger über böse Füße
            let once = Str.normalize input
            let triple = Str.normalize (Str.normalize (Str.normalize input))
            Expect.equal triple once "triple application idempotent"

    ]














