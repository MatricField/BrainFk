module Tests

open System.IO
open Xunit
open FsCheck
open FsCheck.Xunit
open BrainFuck
open BrainFuck.Compilers

type private Dummy() =
    class end

module AppResources =
    let private ResourceManager = System.Resources.ResourceManager("BFUnitTests.AppResource", typeof<Dummy>.Assembly)
    let BSort = ResourceManager.GetString("BSort")
    let HelloWorld = ResourceManager.GetString("HelloWorld")


let helloWorldProgram =
    Compiler1.Compile(AppResources.HelloWorld).Create()

let bsortProgram =
    Compiler1.Compile(AppResources.BSort).Create()

[<Fact>]
let ``hello world output``() =
    let expected = "Hello world!"
    let actual =
        let is = new MemoryStream()
        let os = new MemoryStream()
        helloWorldProgram.Execute(is, os)
        os.Seek(0L, SeekOrigin.Begin) |> ignore
        let reader = new StreamReader(os);
        reader.ReadToEnd()
    expected = actual

[<Property>]
let bSortWorks (items: byte[]) =
    let actual =
        let is = new MemoryStream(items)
        let os = new MemoryStream()
        bsortProgram.Execute(is, os);
        os.Seek(0L, SeekOrigin.Begin) |> ignore
        os.GetBuffer() |> Array.take (int os.Length)
    actual
    |>Seq.pairwise
    |>Seq.forall (fun (x, y) -> x <= y)