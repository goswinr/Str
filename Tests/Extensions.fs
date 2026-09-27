namespace Tests

module Extensions =

    open Str
    open Str.ExtensionsString
    open System

    open Scriptorium.Nib.Assertion
    open type Scriptorium.Quill.Test

#nowarn "44" // to test the obsolete Slice alias
    let private obsoleteSlice (s: string) startIdx endIdx : string = s.Slice(startIdx, endIdx)
#warnon "44"

    let tests = testList ("String extensions tests", [

        test ("DoesNotContain with substring", fun _ ->
            let s = "Hello, world!"
            assertThat (s.DoesNotContain("Goodbye")) (tag "Expected string not to contain 'Goodbye'" >> isTrue)
        )

        test ("DoesNotContain with char", fun _ ->
            let s = "Hello, world!"
            assertThat (s.DoesNotContain('x')) (tag "Expected string not to contain 'x'" >> isTrue)
        )

        test ("Contains with char", fun _ ->
            let s = "Hello, world!"
            assertThat (s.Contains('H')) (tag "Expected string to contain 'H'" >> isTrue)
        )

        test ("Split with char", fun _ ->
            let s = "Hello, world!"
            let split = s.Split(',')
            assertThat split.Length (tag "Expected split to have 2 elements" >> isEqualTo 2)
            assertThat split.[0] (tag "Expected first element to be 'Hello'" >> isEqualTo "Hello")
            assertThat split.[1] (tag "Expected second element to be ' world!'" >> isEqualTo " world!")
        )

        test ("LastIndex", fun _ ->
            let s = "Hello, world!"
            let result = s.LastIndex
            assertThat result (tag "Expected last index to be 12" >> isEqualTo 12)
        )

        test ("Last", fun _ ->
            let s = "Hello, world!"
            let result = s.Last
            assertThat result (tag "Expected last character to be '!'" >> isEqualTo '!')
        )

        test ("SecondLast", fun _ ->
            let s = "Hello, world!"
            let result = s.SecondLast
            assertThat result (tag "Expected second last character to be 'd'" >> isEqualTo 'd')
        )

        test ("ThirdLast", fun _ ->
            let s = "Hello, world!"
            let result = s.ThirdLast
            assertThat result (tag "Expected third last character to be 'l'" >> isEqualTo 'l')
        )

        test ("LastX", fun _ ->
            let s = "Hello, world!"
            let result = s.LastX 5
            assertThat result (tag "Expected last 5 characters to be 'orld!'" >> isEqualTo "orld!")
        )

        test ("First", fun _ ->
            let s = "Hello, world!"
            let result = s.First
            assertThat result (tag "Expected first character to be 'H'" >> isEqualTo 'H')
        )

        test ("Second", fun _ ->
            let s = "Hello, world!"
            let result = s.Second
            assertThat result (tag "Expected second character to be 'e'" >> isEqualTo 'e')
        )

        test ("Third", fun _ ->
            let s = "Hello, world!"
            let result = s.Third
            assertThat result (tag "Expected third character to be 'l'" >> isEqualTo 'l')
        )

        test ("GetNeg", fun _ ->
            let s = "Hello, world!"
            let result = s.GetNeg -1
            assertThat result (tag "Expected character at index -1 to be '!'" >> isEqualTo '!')
        )

        test ("GetLooped with positive index", fun _ ->
            let s = "Hello, world!"
            let result = s.GetLooped 13
            assertThat result (tag "Expected character at looped index 13 to be 'H'" >> isEqualTo 'H')
        )

        test ("GetLooped with negative index", fun _ ->
            let s = "Hello, world!"
            let result = s.GetLooped -1
            assertThat result (tag "Expected character at looped index -1 to be '!'" >> isEqualTo '!')
        )

        test ("SliceNeg with positive indices", fun _ ->
            let s = "Hello, world!"
            let result = s.SliceNeg(0, 4)
            assertThat result (tag "Expected slice from index 0 to 5 to be 'Hello'" >> isEqualTo "Hello")
        )

        test ("SliceNeg with negative indices", fun _ ->
            let s = "Hello, world!"
            let result = s.SliceNeg(-6, -1)
            assertThat result (tag "Expected slice from index -6 to -1 to be 'world'" >> isEqualTo "world!")
        )

        test ("ReplaceFirst", fun _ ->
            let s = "Hello-XT-world-XT!"
            let result = s.ReplaceFirst("XT", "000")
            assertThat result (tag "Expected first occurrence of 'XT' to be replaced with '000'" >> isEqualTo "Hello-000-world-XT!")
        )

        test ("ReplaceLast", fun _ ->
            let s = "Hello-XT-world-XT!"
            let result = s.ReplaceLast("XT", "000")
            assertThat result (tag "Expected last occurrence of 'XT' to be replaced with '000'" >> isEqualTo "Hello-XT-world-000!")
        )

        test ("ReplaceLast casing", fun _ ->
            let s = "Hello-xT-world-Xt!"
            let result = s.ReplaceLast("XT", "000")
            assertThat result (tag "Expected no occurrence of 'XT' to be replaced with '000'" >> isEqualTo "Hello-xT-world-Xt!")
        )

        // ============ str.IsWhite tests ============
        test ("str.IsWhite should return true for empty string", fun _ ->
            let s = ""
            assertThat s.IsWhite (tag "Expected empty string to be white" >> isTrue)
        )

        test ("str.IsWhite should return true for whitespace only", fun _ ->
            let s = "   \t\n"
            assertThat s.IsWhite (tag "Expected whitespace to be white" >> isTrue)
        )

        test ("str.IsWhite should return false for non-whitespace", fun _ ->
            let s = "Hello"
            assertThat s.IsWhite (tag "Expected text to not be white" >> isFalse)
        )

        test ("str.IsWhite should return false for text with whitespace", fun _ ->
            let s = "  Hello  "
            assertThat s.IsWhite (tag "Expected text with spaces to not be white" >> isFalse)
        )

        // ============ str.IsNotWhite tests ============
        test ("str.IsNotWhite should return false for empty string", fun _ ->
            let s = ""
            assertThat s.IsNotWhite (tag "Expected empty string to be white" >> isFalse)
        )

        test ("str.IsNotWhite should return false for whitespace only", fun _ ->
            let s = "   \t\n"
            assertThat s.IsNotWhite (tag "Expected whitespace to be white" >> isFalse)
        )

        test ("str.IsNotWhite should return true for non-whitespace", fun _ ->
            let s = "Hello"
            assertThat s.IsNotWhite (tag "Expected text to not be white" >> isTrue)
        )

        // ============ str.IsEmpty tests ============
        test ("str.IsEmpty should return true for empty string", fun _ ->
            let s = ""
            assertThat s.IsEmpty (tag "Expected empty string to be empty" >> isTrue)
        )

        test ("str.IsEmpty should return false for whitespace", fun _ ->
            let s = "   "
            assertThat s.IsEmpty (tag "Expected whitespace to not be empty" >> isFalse)
        )

        test ("str.IsEmpty should return false for text", fun _ ->
            let s = "Hello"
            assertThat s.IsEmpty (tag "Expected text to not be empty" >> isFalse)
        )

        // ============ str.IsNotEmpty tests ============
        test ("str.IsNotEmpty should return false for empty string", fun _ ->
            let s = ""
            assertThat s.IsNotEmpty (tag "Expected empty string to be empty" >> isFalse)
        )

        test ("str.IsNotEmpty should return true for whitespace", fun _ ->
            let s = "   "
            assertThat s.IsNotEmpty (tag "Expected whitespace to not be empty" >> isTrue)
        )

        test ("str.IsNotEmpty should return true for text", fun _ ->
            let s = "Hello"
            assertThat s.IsNotEmpty (tag "Expected text to not be empty" >> isTrue)
        )

        // ============ str.Get tests ============
        test ("str.Get should return character at index 0", fun _ ->
            let s = "Hello"
            assertThat (s.Get 0) (tag "Expected first char to be 'H'" >> isEqualTo 'H')
        )

        test ("str.Get should return character at middle index", fun _ ->
            let s = "Hello"
            assertThat (s.Get 2) (tag "Expected char at index 2 to be 'l'" >> isEqualTo 'l')
        )

        test ("str.Get should return character at last index", fun _ ->
            let s = "Hello"
            assertThat (s.Get 4) (tag "Expected last char to be 'o'" >> isEqualTo 'o')
        )

        test ("str.Get should throw for negative index", fun _ ->
            let s = "Hello"
            assertThat (fun _ -> s.Get -1 |> ignore) (tag "Expected exception for negative index" >> throws)
        )

        test ("str.Get should throw for out of range index", fun _ ->
            let s = "Hello"
            assertThat (fun _ -> s.Get 10 |> ignore) (tag "Expected exception for out of range" >> throws)
        )

        test ("str.Get should throw for empty string", fun _ ->
            let s = ""
            assertThat (fun _ -> s.Get 0 |> ignore) (tag "Expected exception for empty string" >> throws)
        )

        // ============ str.Idx tests ============
        test ("str.Idx should return character at index 0", fun _ ->
            let s = "Hello"
            assertThat (s.Idx 0) (tag "Expected first char to be 'H'" >> isEqualTo 'H')
        )

        test ("str.Idx should return character at middle index", fun _ ->
            let s = "Hello"
            assertThat (s.Idx 2) (tag "Expected char at index 2 to be 'l'" >> isEqualTo 'l')
        )

        test ("str.Idx should throw for negative index", fun _ ->
            let s = "Hello"
            assertThat (fun _ -> s.Idx -1 |> ignore) (tag "Expected exception for negative index" >> throws)
        )

        test ("str.Idx should throw for out of range index", fun _ ->
            let s = "Hello"
            assertThat (fun _ -> s.Idx 10 |> ignore) (tag "Expected exception for out of range" >> throws)
        )

        // ============ Edge cases for existing tests ============
        test ("First should throw for empty string", fun _ ->
            let s = ""
            assertThat (fun _ -> s.First |> ignore) (tag "Expected exception for empty string" >> throws)
        )

        test ("Last should throw for empty string", fun _ ->
            let s = ""
            assertThat (fun _ -> s.Last |> ignore) (tag "Expected exception for empty string" >> throws)
        )

        test ("Second should throw for single char string", fun _ ->
            let s = "A"
            assertThat (fun _ -> s.Second |> ignore) (tag "Expected exception for single char" >> throws)
        )

        test ("SecondLast should throw for single char string", fun _ ->
            let s = "A"
            assertThat (fun _ -> s.SecondLast |> ignore) (tag "Expected exception for single char" >> throws)
        )

        test ("Third should throw for two char string", fun _ ->
            let s = "AB"
            assertThat (fun _ -> s.Third |> ignore) (tag "Expected exception for two char string" >> throws)
        )

        test ("ThirdLast should throw for two char string", fun _ ->
            let s = "AB"
            assertThat (fun _ -> s.ThirdLast |> ignore) (tag "Expected exception for two char string" >> throws)
        )

        test ("LastX should throw when x > length", fun _ ->
            let s = "Hi"
            assertThat (fun _ -> s.LastX 5 |> ignore) (tag "Expected exception when x > length" >> throws)
        )

        test ("LastX should throw when x is negative", fun _ ->
            let s = "Hi"
            assertThat (fun _ -> s.LastX -1 |> ignore) (tag "Expected exception when x is negative" >> throws)
        )

        test ("GetLooped should throw for empty string", fun _ ->
            let s = ""
            assertThat (fun _ -> s.GetLooped 0 |> ignore) (tag "Expected exception for empty string" >> throws)
        )

        test ("SliceNeg should throw for invalid range", fun _ ->
            let s = "Hello"
            assertThat (fun _ -> s.SliceNeg(3, 1) |> ignore) (tag "Expected exception for invalid range" >> throws)
        )

        test ("ReplaceFirst and ReplaceLast return the input for an empty oldValue", fun _ ->
            assertThat ("abc".ReplaceFirst("", "x")) (tag "ReplaceFirst" >> isEqualTo "abc")
            assertThat ("abc".ReplaceLast("", "x")) (tag "ReplaceLast" >> isEqualTo "abc")
        )

        test ("SliceNeg error for out of range end index reports the end index", fun _ ->
            let msg = try "abc".SliceNeg(0, 10) |> ignore; "" with e -> e.Message
            assertThat (msg.Contains "End index 10") (tag $"Message should name end index 10, got: {msg}" >> isTrue)
        )

        test ("SliceNeg error for out of range negative end index reports the end index", fun _ ->
            let msg = try "abcde".SliceNeg(1, -99) |> ignore; "" with e -> e.Message
            assertThat (msg.Contains "End index -99 is out of range") (tag $"Message should name end index -99, got: {msg}" >> isTrue)
            let msg6 = try "abcde".SliceNeg(1, -6) |> ignore; "" with e -> e.Message
            assertThat (msg6.Contains "End index -6 is out of range") (tag $"Message should name end index -6, got: {msg6}" >> isTrue)
            let msgStart = try "abcde".SliceNeg(3, 1) |> ignore; "" with e -> e.Message
            assertThat (msgStart.Contains "Start index 3 is bigger than end index 1") (tag $"Message should name both indices, got: {msgStart}" >> isTrue)
        )

        test ("SliceNeg returns empty when end index is one less than start index", fun _ ->
            assertThat ("abcde".SliceNeg(3, 2)) (tag "SliceNeg 3 2" >> isEqualTo "")
            assertThat ("abcde".SliceNeg(0, -6)) (tag "SliceNeg 0 -6" >> isEqualTo "")
        )

        test ("obsolete Slice still works like SliceNeg", fun _ ->
            assertThat (obsoleteSlice "Hello" -3 -1) (tag "Slice(-3, -1)" >> isEqualTo "llo")
        )

        test ("SliceIdx uses an inclusive end index", fun _ ->
            let s = "01234"
            assertThat (s.SliceIdx(0, 0)) (tag "first char" >> isEqualTo "0")
            assertThat (s.SliceIdx(1, 3)) (tag "indices 1 through 3" >> isEqualTo "123")
            assertThat (s.SliceIdx(0, 4)) (tag "full range" >> isEqualTo "01234")
            assertThat (s.SliceIdx(4, 4)) (tag "last char" >> isEqualTo "4")
        )

        test ("SliceIdx rejects invalid ranges", fun _ ->
            let s = "01234"
            assertThat (fun _ -> s.SliceIdx(-1, 2) |> ignore) (tag "negative start" >> throws)
            assertThat (fun _ -> s.SliceIdx(0, -1) |> ignore) (tag "negative end" >> throws)
            assertThat (fun _ -> s.SliceIdx(5, 5) |> ignore) (tag "start at Length" >> throws)
            assertThat (fun _ -> s.SliceIdx(0, 5) |> ignore) (tag "end past Length" >> throws)
            assertThat (fun _ -> s.SliceIdx(3, 2) |> ignore) (tag "start greater than end" >> throws)
            let msg = try s.SliceIdx(-1, 2) |> ignore; "" with e -> e.Message
            assertThat (msg.Contains "str.SliceIdx: Start index -1 is out of range") (tag $"Message should name start index -1, got: {msg}" >> isTrue)
            let msgEmpty = try "".SliceIdx(0, 0) |> ignore; "" with e -> e.Message
            assertThat (msgEmpty.Contains "Can't slice an empty string") (tag $"Message should say the string is empty, got: {msgEmpty}" >> isTrue)
        )

        test ("SliceLooped normalizes indices with modulo", fun _ ->
            let s = "abc"
            assertThat (s.SliceLooped(-1, 0)) (tag "-1..0 is empty" >> isEqualTo "")
            assertThat (s.SliceLooped(-1, -1)) (tag "-1..-1 is the last char" >> isEqualTo "c")
            assertThat (s.SliceLooped(3, 4)) (tag "3..4 loops to 0..1" >> isEqualTo "ab")
            assertThat (s.SliceLooped(-3, -1)) (tag "-3..-1 is all chars" >> isEqualTo "abc")
            assertThat ("".SliceLooped(0, 5)) (tag "empty input gives empty result" >> isEqualTo "")
        )
        ])
