namespace Tests

open Str
open System.Text

#if FABLE_COMPILER
open Fable.Mocha
#else
open Expecto
#endif

module StringBuilder =

    let tests = testList "StringBuilder extensions tests" [

        // ============ sb.Add(string) tests ============
        testCase "sb.Add(string) should append string" <| fun _ ->
            let sb = StringBuilder()
            sb.Add("Hello")
            sb.Add(" World")
            Expect.equal (sb.ToString()) "Hello World" "Should append strings"

        testCase "sb.Add(string) should work with empty string" <| fun _ ->
            let sb = StringBuilder()
            sb.Add("Hello")
            sb.Add("")
            sb.Add("!")
            Expect.equal (sb.ToString()) "Hello!" "Should handle empty string"

        // ============ sb.Add(char) tests ============
        testCase "sb.Add(char) should append char" <| fun _ ->
            let sb = StringBuilder()
            sb.Add('H')
            sb.Add('i')
            Expect.equal (sb.ToString()) "Hi" "Should append chars"

        // ============ sb.AddLine(string) tests ============
        testCase "sb.AddLine(string) should append string with newline" <| fun _ ->
            let sb = StringBuilder()
            sb.AddLine("Hello")
            sb.Add("World")
            let result = sb.ToString()
            Expect.isTrue (result.Contains("Hello")) "Should contain Hello"
            Expect.isTrue (result.Contains("World")) "Should contain World"
            Expect.isTrue (result.Contains("\n") || result.Contains("\r")) "Should contain newline"

        // ============ sb.AddLine() tests ============
        testCase "sb.AddLine() should append just newline" <| fun _ ->
            let sb = StringBuilder()
            sb.Add("Hello")
            sb.AddLine()
            sb.Add("World")
            let result = sb.ToString()
            Expect.isTrue (result.Contains("\n") || result.Contains("\r")) "Should contain newline"

        // ============ sb.IndexOf(char) tests ============
        testCase "sb.IndexOf(char) should find character" <| fun _ ->
            let sb = StringBuilder("Hello World")
            Expect.equal (sb.IndexOf('W')) 6 "Should find 'W' at index 6"

        testCase "sb.IndexOf(char) should return -1 for not found" <| fun _ ->
            let sb = StringBuilder("Hello World")
            Expect.equal (sb.IndexOf('Z')) -1 "Should return -1 for not found"

        testCase "sb.IndexOf(char) should find first occurrence" <| fun _ ->
            let sb = StringBuilder("Hello")
            Expect.equal (sb.IndexOf('l')) 2 "Should find first 'l' at index 2"

        testCase "sb.IndexOf(char) should handle empty StringBuilder" <| fun _ ->
            let sb = StringBuilder()
            Expect.equal (sb.IndexOf('a')) -1 "Should return -1 for empty"

        // ============ sb.IndexOf(char, from) tests ============
        testCase "sb.IndexOf(char, from) should find character from position" <| fun _ ->
            let sb = StringBuilder("Hello")
            Expect.equal (sb.IndexOf('l', 3)) 3 "Should find 'l' at index 3 when starting from 3"

        testCase "sb.IndexOf(char, from) should return -1 when char is before from" <| fun _ ->
            let sb = StringBuilder("Hello")
            Expect.equal (sb.IndexOf('H', 1)) -1 "Should return -1 when 'H' is before start"

        // ============ sb.IndexOf(string) tests ============
        testCase "sb.IndexOf(string) should find substring" <| fun _ ->
            let sb = StringBuilder("Hello World")
            Expect.equal (sb.IndexOf("World")) 6 "Should find 'World' at index 6"

        testCase "sb.IndexOf(string) should return -1 for not found" <| fun _ ->
            let sb = StringBuilder("Hello World")
            Expect.equal (sb.IndexOf("Foo")) -1 "Should return -1 for not found"

        testCase "sb.IndexOf(string) should find at beginning" <| fun _ ->
            let sb = StringBuilder("Hello World")
            Expect.equal (sb.IndexOf("Hello")) 0 "Should find 'Hello' at index 0"

        testCase "sb.IndexOf(string) should find at end" <| fun _ ->
            let sb = StringBuilder("Hello World")
            Expect.equal (sb.IndexOf("orld")) 7 "Should find 'orld' at index 7"

        testCase "sb.IndexOf(string) should handle empty StringBuilder" <| fun _ ->
            let sb = StringBuilder()
            Expect.equal (sb.IndexOf("test")) -1 "Should return -1 for empty"

        testCase "sb.IndexOf(string) should find single char string" <| fun _ ->
            let sb = StringBuilder("Hello")
            Expect.equal (sb.IndexOf("e")) 1 "Should find 'e' at index 1"

        // ============ sb.IndexOf(string, from) tests ============
        testCase "sb.IndexOf(string, from) should find substring from position" <| fun _ ->
            let sb = StringBuilder("Hello Hello")
            Expect.equal (sb.IndexOf("Hello", 1)) 6 "Should find second 'Hello' at index 6"

        testCase "sb.IndexOf(string, from) should return -1 when string is before from" <| fun _ ->
            let sb = StringBuilder("Hello World")
            Expect.equal (sb.IndexOf("Hello", 1)) -1 "Should return -1 when 'Hello' starts before search start"

        // ============ sb.Contains(char) tests ============
        testCase "sb.Contains(char) should return true when char exists" <| fun _ ->
            let sb = StringBuilder("Hello")
            Expect.isTrue (sb.Contains('e')) "Should contain 'e'"

        testCase "sb.Contains(char) should return false when char not exists" <| fun _ ->
            let sb = StringBuilder("Hello")
            Expect.isFalse (sb.Contains('Z')) "Should not contain 'Z'"

        testCase "sb.Contains(char) should return false for empty StringBuilder" <| fun _ ->
            let sb = StringBuilder()
            Expect.isFalse (sb.Contains('a')) "Should not contain anything"

        // ============ sb.Contains(string) tests ============
        testCase "sb.Contains(string) should return true when string exists" <| fun _ ->
            let sb = StringBuilder("Hello World")
            Expect.isTrue (sb.Contains("World")) "Should contain 'World'"

        testCase "sb.Contains(string) should return false when string not exists" <| fun _ ->
            let sb = StringBuilder("Hello World")
            Expect.isFalse (sb.Contains("Foo")) "Should not contain 'Foo'"

        testCase "sb.Contains(string) should return false for empty StringBuilder" <| fun _ ->
            let sb = StringBuilder()
            Expect.isFalse (sb.Contains("test")) "Should not contain anything"

        testCase "sb.Contains(string) should work with partial matches" <| fun _ ->
            let sb = StringBuilder("Hello")
            Expect.isTrue (sb.Contains("ell")) "Should contain 'ell'"
            Expect.isFalse (sb.Contains("ellx")) "Should not contain 'ellx'"
        ]
