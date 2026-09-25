namespace Tests

open Str
open System.Text

open Scriptorium.Nib.Assertion
open type Scriptorium.Quill.Test

module StringBuilder =

    let tests = testList ("StringBuilder extensions tests", [

        // ============ sb.Add(string) tests ============
        test ("sb.Add(string) should append string", fun _ ->
            let sb = StringBuilder()
            sb.Add("Hello")
            sb.Add(" World")
            assertThat (sb.ToString()) (tag "Should append strings" >> isEqualTo "Hello World")
        )

        test ("sb.Add(string) should work with empty string", fun _ ->
            let sb = StringBuilder()
            sb.Add("Hello")
            sb.Add("")
            sb.Add("!")
            assertThat (sb.ToString()) (tag "Should handle empty string" >> isEqualTo "Hello!")
        )

        // ============ sb.Add(char) tests ============
        test ("sb.Add(char) should append char", fun _ ->
            let sb = StringBuilder()
            sb.Add('H')
            sb.Add('i')
            assertThat (sb.ToString()) (tag "Should append chars" >> isEqualTo "Hi")
        )

        // ============ sb.AddLine(string) tests ============
        test ("sb.AddLine(string) should append string with newline", fun _ ->
            let sb = StringBuilder()
            sb.AddLine("Hello")
            sb.Add("World")
            let result = sb.ToString()
            assertThat (result.Contains("Hello")) (tag "Should contain Hello" >> isTrue)
            assertThat (result.Contains("World")) (tag "Should contain World" >> isTrue)
            assertThat (result.Contains("\n") || result.Contains("\r")) (tag "Should contain newline" >> isTrue)
        )

        // ============ sb.AddLine() tests ============
        test ("sb.AddLine() should append just newline", fun _ ->
            let sb = StringBuilder()
            sb.Add("Hello")
            sb.AddLine()
            sb.Add("World")
            let result = sb.ToString()
            assertThat (result.Contains("\n") || result.Contains("\r")) (tag "Should contain newline" >> isTrue)
        )

        // ============ sb.IndexOf(char) tests ============
        test ("sb.IndexOf(char) should find character", fun _ ->
            let sb = StringBuilder("Hello World")
            assertThat (sb.IndexOf('W')) (tag "Should find 'W' at index 6" >> isEqualTo 6)
        )

        test ("sb.IndexOf(char) should return -1 for not found", fun _ ->
            let sb = StringBuilder("Hello World")
            assertThat (sb.IndexOf('Z')) (tag "Should return -1 for not found" >> isEqualTo -1)
        )

        test ("sb.IndexOf(char) should find first occurrence", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.IndexOf('l')) (tag "Should find first 'l' at index 2" >> isEqualTo 2)
        )

        test ("sb.IndexOf(char) should handle empty StringBuilder", fun _ ->
            let sb = StringBuilder()
            assertThat (sb.IndexOf('a')) (tag "Should return -1 for empty" >> isEqualTo -1)
        )

        // ============ sb.IndexOf(char, from) tests ============
        test ("sb.IndexOf(char, from) should find character from position", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.IndexOf('l', 3)) (tag "Should find 'l' at index 3 when starting from 3" >> isEqualTo 3)
        )

        test ("sb.IndexOf(char, from) should return -1 when char is before from", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.IndexOf('H', 1)) (tag "Should return -1 when 'H' is before start" >> isEqualTo -1)
        )

        test ("sb.IndexOf(char, from) accepts Length and rejects larger values", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.IndexOf('x', sb.Length)) (tag "Searching from Length should return -1" >> isEqualTo -1)
            assertThat (fun () -> sb.IndexOf('x', sb.Length + 1) |> ignore) (tag "Searching beyond Length should throw" >> throws)
        )

        // ============ sb.IndexOf(string) tests ============
        test ("sb.IndexOf(string) should find substring", fun _ ->
            let sb = StringBuilder("Hello World")
            assertThat (sb.IndexOf("World")) (tag "Should find 'World' at index 6" >> isEqualTo 6)
        )

        test ("sb.IndexOf(string) should return -1 for not found", fun _ ->
            let sb = StringBuilder("Hello World")
            assertThat (sb.IndexOf("Foo")) (tag "Should return -1 for not found" >> isEqualTo -1)
        )

        test ("sb.IndexOf(string) should find at beginning", fun _ ->
            let sb = StringBuilder("Hello World")
            assertThat (sb.IndexOf("Hello")) (tag "Should find 'Hello' at index 0" >> isEqualTo 0)
        )

        test ("sb.IndexOf(string) should find at end", fun _ ->
            let sb = StringBuilder("Hello World")
            assertThat (sb.IndexOf("orld")) (tag "Should find 'orld' at index 7" >> isEqualTo 7)
        )

        test ("sb.IndexOf(string) should handle empty StringBuilder", fun _ ->
            let sb = StringBuilder()
            assertThat (sb.IndexOf("test")) (tag "Should return -1 for empty" >> isEqualTo -1)
        )

        test ("sb.IndexOf(string) should find single char string", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.IndexOf("e")) (tag "Should find 'e' at index 1" >> isEqualTo 1)
        )

        // ============ sb.IndexOf(string, from) tests ============
        test ("sb.IndexOf(string, from) should find substring from position", fun _ ->
            let sb = StringBuilder("Hello Hello")
            assertThat (sb.IndexOf("Hello", 1)) (tag "Should find second 'Hello' at index 6" >> isEqualTo 6)
        )

        test ("sb.IndexOf(string, from) should return -1 when string is before from", fun _ ->
            let sb = StringBuilder("Hello World")
            assertThat (sb.IndexOf("Hello", 1)) (tag "Should return -1 when 'Hello' starts before search start" >> isEqualTo -1)
        )

        test ("sb.IndexOf(string, from) handles an empty search string", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.IndexOf("", 0)) (tag "An empty string should be found at zero" >> isEqualTo 0)
            assertThat (sb.IndexOf("", 3)) (tag "An empty string should be found at the starting index" >> isEqualTo 3)
            assertThat (sb.IndexOf("", sb.Length)) (tag "An empty string should be found at Length" >> isEqualTo sb.Length)
        )

        test ("sb.IndexOf(string, from) validates its arguments", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (fun () -> sb.IndexOf("x", sb.Length + 1) |> ignore) (tag "Searching beyond Length should throw" >> throws)
            assertThat (fun () -> sb.IndexOf(null, 0) |> ignore) (tag "A null search string should throw" >> throws)
        )

        // ============ sb.Contains(char) tests ============
        test ("sb.Contains(char) should return true when char exists", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.Contains('e')) (tag "Should contain 'e'" >> isTrue)
        )

        test ("sb.Contains(char) should return false when char not exists", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.Contains('Z')) (tag "Should not contain 'Z'" >> isFalse)
        )

        test ("sb.Contains(char) should return false for empty StringBuilder", fun _ ->
            let sb = StringBuilder()
            assertThat (sb.Contains('a')) (tag "Should not contain anything" >> isFalse)
        )

        // ============ sb.Contains(string) tests ============
        test ("sb.Contains(string) should return true when string exists", fun _ ->
            let sb = StringBuilder("Hello World")
            assertThat (sb.Contains("World")) (tag "Should contain 'World'" >> isTrue)
        )

        test ("sb.Contains(string) should return false when string not exists", fun _ ->
            let sb = StringBuilder("Hello World")
            assertThat (sb.Contains("Foo")) (tag "Should not contain 'Foo'" >> isFalse)
        )

        test ("sb.Contains(string) should return false for empty StringBuilder", fun _ ->
            let sb = StringBuilder()
            assertThat (sb.Contains("test")) (tag "Should not contain anything" >> isFalse)
        )

        test ("sb.Contains(string) should work with partial matches", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.Contains("ell")) (tag "Should contain 'ell'" >> isTrue)
            assertThat (sb.Contains("ellx")) (tag "Should not contain 'ellx'" >> isFalse)
        )

        test ("sb.Contains(string) should return true for an empty search string", fun _ ->
            let sb = StringBuilder("Hello")
            assertThat (sb.Contains("")) (tag "Every StringBuilder should contain the empty string" >> isTrue)
        )
        ])
