namespace Tests

open Str

module Main =

    #if FABLE_COMPILER

    open Fable.Mocha
    Mocha.runTests Tests.Extensions.tests |> ignore
    Mocha.runTests Tests.Module.tests |> ignore
    #else

    open Expecto
    [<EntryPoint>]
    let main argv =
        let a = runTestsWithCLIArgs [] [||] Tests.Extensions.tests
        let b = runTestsWithCLIArgs [] [||] Tests.Module.tests
        a|||b

    #endif