namespace Tests

open Str

#if FABLE_COMPILER
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


        #if FABLE_COMPILER
        // error FABLE: Microsoft.FSharp.Core.Operators.ArrayExtensions.String.GetReverseIndex is not supported by Fable
        #else
        // slice
        testCase "neg slice2 should return a substring from the start index to the end index" <| fun _ ->
            let result = "Hello, World!".[1..^1]
            Expect.equal result "ello, World" "Should be equal"
        #endif


























    ]














