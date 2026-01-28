namespace Tests

open Str

module Main =

    #if FABLE_COMPILER

    open Fable.Mocha
    Mocha.runTests Tests.Extensions.tests |> ignore
    Mocha.runTests Tests.Module.tests |> ignore
    Mocha.runTests Tests.StringBuilder.tests |> ignore
    #else

    open Expecto
    [<EntryPoint>]
    let main argv =
        let a = runTestsWithCLIArgs [] [||] Tests.Extensions.tests
        let b = runTestsWithCLIArgs [] [||] Tests.Module.tests
        let c = runTestsWithCLIArgs [] [||] Tests.StringBuilder.tests
        a|||b|||c

    #endif