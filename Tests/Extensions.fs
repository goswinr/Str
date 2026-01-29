namespace Tests

module Extensions =

    open Str
    open Str.ExtensionsString
    open System

    #if FABLE_COMPILER
    open Fable.Mocha
    #else
    open Expecto
    #endif


    let tests = testList "String extensions tests" [

        testCase "DoesNotContain with substring" <| fun _ ->
            let s = "Hello, world!"
            Expect.isTrue (s.DoesNotContain("Goodbye")) "Expected string not to contain 'Goodbye'"

        testCase "DoesNotContain with char" <| fun _ ->
            let s = "Hello, world!"
            Expect.isTrue (s.DoesNotContain('x')) "Expected string not to contain 'x'"

        testCase "Contains with char" <| fun _ ->
            let s = "Hello, world!"
            Expect.isTrue (s.Contains('H')) "Expected string to contain 'H'"

        testCase "Split with char" <| fun _ ->
            let s = "Hello, world!"
            let split = s.Split(',')
            Expect.equal split.Length 2 "Expected split to have 2 elements"
            Expect.equal split.[0] "Hello" "Expected first element to be 'Hello'"
            Expect.equal split.[1] " world!" "Expected second element to be ' world!'"

        testCase "LastIndex" <| fun _ ->
            let s = "Hello, world!"
            let result = s.LastIndex
            Expect.equal result 12 "Expected last index to be 12"

        testCase "Last" <| fun _ ->
            let s = "Hello, world!"
            let result = s.Last
            Expect.equal result '!' "Expected last character to be '!'"

        testCase "SecondLast" <| fun _ ->
            let s = "Hello, world!"
            let result = s.SecondLast
            Expect.equal result 'd' "Expected second last character to be 'd'"

        testCase "ThirdLast" <| fun _ ->
            let s = "Hello, world!"
            let result = s.ThirdLast
            Expect.equal result 'l' "Expected third last character to be 'l'"

        testCase "LastX" <| fun _ ->
            let s = "Hello, world!"
            let result = s.LastX 5
            Expect.equal result "orld!" "Expected last 5 characters to be 'orld!'"

        testCase "First" <| fun _ ->
            let s = "Hello, world!"
            let result = s.First
            Expect.equal result 'H' "Expected first character to be 'H'"

        testCase "Second" <| fun _ ->
            let s = "Hello, world!"
            let result = s.Second
            Expect.equal result 'e' "Expected second character to be 'e'"

        testCase "Third" <| fun _ ->
            let s = "Hello, world!"
            let result = s.Third
            Expect.equal result 'l' "Expected third character to be 'l'"

        testCase "GetNeg" <| fun _ ->
            let s = "Hello, world!"
            let result = s.GetNeg -1
            Expect.equal result '!' "Expected character at index -1 to be '!'"

        testCase "GetLooped with positive index" <| fun _ ->
            let s = "Hello, world!"
            let result = s.GetLooped 13
            Expect.equal result 'H' "Expected character at looped index 13 to be 'H'"

        testCase "GetLooped with negative index" <| fun _ ->
            let s = "Hello, world!"
            let result = s.GetLooped -1
            Expect.equal result '!' "Expected character at looped index -1 to be '!'"

        testCase "Slice with positive indices" <| fun _ ->
            let s = "Hello, world!"
            let result = s.Slice(0, 4)
            Expect.equal result "Hello" "Expected slice from index 0 to 5 to be 'Hello'"

        testCase "Slice with negative indices" <| fun _ ->
            let s = "Hello, world!"
            let result = s.Slice(-6, -1)
            Expect.equal result "world!" "Expected slice from index -6 to -1 to be 'world'"

        testCase "ReplaceFirst" <| fun _ ->
            let s = "Hello-XT-world-XT!"
            let result = s.ReplaceFirst("XT", "000")
            Expect.equal result "Hello-000-world-XT!" "Expected first occurrence of 'XT' to be replaced with '000'"

        testCase "ReplaceLast" <| fun _ ->
            let s = "Hello-XT-world-XT!"
            let result = s.ReplaceLast("XT", "000")
            Expect.equal result "Hello-XT-world-000!" "Expected last occurrence of 'XT' to be replaced with '000'"

        testCase "ReplaceLast casing" <| fun _ ->
            let s = "Hello-xT-world-Xt!"
            let result = s.ReplaceLast("XT", "000")
            Expect.equal result "Hello-xT-world-Xt!" "Expected no occurrence of 'XT' to be replaced with '000'"

        // ============ str.IsWhite tests ============
        testCase "str.IsWhite should return true for empty string" <| fun _ ->
            let s = ""
            Expect.isTrue s.IsWhite "Expected empty string to be white"

        testCase "str.IsWhite should return true for whitespace only" <| fun _ ->
            let s = "   \t\n"
            Expect.isTrue s.IsWhite "Expected whitespace to be white"

        testCase "str.IsWhite should return false for non-whitespace" <| fun _ ->
            let s = "Hello"
            Expect.isFalse s.IsWhite "Expected text to not be white"

        testCase "str.IsWhite should return false for text with whitespace" <| fun _ ->
            let s = "  Hello  "
            Expect.isFalse s.IsWhite "Expected text with spaces to not be white"

        // ============ str.IsNotWhite tests ============
        testCase "str.IsNotWhite should return false for empty string" <| fun _ ->
            let s = ""
            Expect.isFalse s.IsNotWhite "Expected empty string to be white"

        testCase "str.IsNotWhite should return false for whitespace only" <| fun _ ->
            let s = "   \t\n"
            Expect.isFalse s.IsNotWhite "Expected whitespace to be white"

        testCase "str.IsNotWhite should return true for non-whitespace" <| fun _ ->
            let s = "Hello"
            Expect.isTrue s.IsNotWhite "Expected text to not be white"

        // ============ str.IsEmpty tests ============
        testCase "str.IsEmpty should return true for empty string" <| fun _ ->
            let s = ""
            Expect.isTrue s.IsEmpty "Expected empty string to be empty"

        testCase "str.IsEmpty should return false for whitespace" <| fun _ ->
            let s = "   "
            Expect.isFalse s.IsEmpty "Expected whitespace to not be empty"

        testCase "str.IsEmpty should return false for text" <| fun _ ->
            let s = "Hello"
            Expect.isFalse s.IsEmpty "Expected text to not be empty"

        // ============ str.IsNotEmpty tests ============
        testCase "str.IsNotEmpty should return false for empty string" <| fun _ ->
            let s = ""
            Expect.isFalse s.IsNotEmpty "Expected empty string to be empty"

        testCase "str.IsNotEmpty should return true for whitespace" <| fun _ ->
            let s = "   "
            Expect.isTrue s.IsNotEmpty "Expected whitespace to not be empty"

        testCase "str.IsNotEmpty should return true for text" <| fun _ ->
            let s = "Hello"
            Expect.isTrue s.IsNotEmpty "Expected text to not be empty"

        // ============ str.Get tests ============
        testCase "str.Get should return character at index 0" <| fun _ ->
            let s = "Hello"
            Expect.equal (s.Get 0) 'H' "Expected first char to be 'H'"

        testCase "str.Get should return character at middle index" <| fun _ ->
            let s = "Hello"
            Expect.equal (s.Get 2) 'l' "Expected char at index 2 to be 'l'"

        testCase "str.Get should return character at last index" <| fun _ ->
            let s = "Hello"
            Expect.equal (s.Get 4) 'o' "Expected last char to be 'o'"

        testCase "str.Get should throw for negative index" <| fun _ ->
            let s = "Hello"
            Expect.throws (fun _ -> s.Get -1 |> ignore) "Expected exception for negative index"

        testCase "str.Get should throw for out of range index" <| fun _ ->
            let s = "Hello"
            Expect.throws (fun _ -> s.Get 10 |> ignore) "Expected exception for out of range"

        testCase "str.Get should throw for empty string" <| fun _ ->
            let s = ""
            Expect.throws (fun _ -> s.Get 0 |> ignore) "Expected exception for empty string"

        // ============ str.Idx tests ============
        testCase "str.Idx should return character at index 0" <| fun _ ->
            let s = "Hello"
            Expect.equal (s.Idx 0) 'H' "Expected first char to be 'H'"

        testCase "str.Idx should return character at middle index" <| fun _ ->
            let s = "Hello"
            Expect.equal (s.Idx 2) 'l' "Expected char at index 2 to be 'l'"

        testCase "str.Idx should throw for negative index" <| fun _ ->
            let s = "Hello"
            Expect.throws (fun _ -> s.Idx -1 |> ignore) "Expected exception for negative index"

        testCase "str.Idx should throw for out of range index" <| fun _ ->
            let s = "Hello"
            Expect.throws (fun _ -> s.Idx 10 |> ignore) "Expected exception for out of range"

        // ============ Edge cases for existing tests ============
        testCase "First should throw for empty string" <| fun _ ->
            let s = ""
            Expect.throws (fun _ -> s.First |> ignore) "Expected exception for empty string"

        testCase "Last should throw for empty string" <| fun _ ->
            let s = ""
            Expect.throws (fun _ -> s.Last |> ignore) "Expected exception for empty string"

        testCase "Second should throw for single char string" <| fun _ ->
            let s = "A"
            Expect.throws (fun _ -> s.Second |> ignore) "Expected exception for single char"

        testCase "SecondLast should throw for single char string" <| fun _ ->
            let s = "A"
            Expect.throws (fun _ -> s.SecondLast |> ignore) "Expected exception for single char"

        testCase "Third should throw for two char string" <| fun _ ->
            let s = "AB"
            Expect.throws (fun _ -> s.Third |> ignore) "Expected exception for two char string"

        testCase "ThirdLast should throw for two char string" <| fun _ ->
            let s = "AB"
            Expect.throws (fun _ -> s.ThirdLast |> ignore) "Expected exception for two char string"

        testCase "LastX should throw when x > length" <| fun _ ->
            let s = "Hi"
            Expect.throws (fun _ -> s.LastX 5 |> ignore) "Expected exception when x > length"

        testCase "GetLooped should throw for empty string" <| fun _ ->
            let s = ""
            Expect.throws (fun _ -> s.GetLooped 0 |> ignore) "Expected exception for empty string"

        testCase "Slice should throw for invalid range" <| fun _ ->
            let s = "Hello"
            Expect.throws (fun _ -> s.Slice(3, 1) |> ignore) "Expected exception for invalid range"
        ]
