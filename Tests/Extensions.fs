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
        ]
