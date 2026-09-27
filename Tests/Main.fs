namespace Tests

open Scriptorium.Quill

module Main =

    // The same entry point runs on .NET (`dotnet run`) and on JS (`dotnet fable --runScript`).
    // On JS, Scriptorium runs the tests asynchronously and sets the process exit code itself.
    [<EntryPoint>]
    let main _argv =
        Runner.runTests [
            Tests.Extensions.tests
            Tests.Module.tests
            Tests.StringBuilder.tests
        ]
