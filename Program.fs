open System
open System.IO
open Vec
open Ray


[<EntryPoint>]
let main argv =
    let w = 200
    let h = 100
    let body = seq {
        yield "P3\n"
        yield (string w) + " " + (string h)
        yield "\n255\n"
        
        for k in [0..h] do
        for i in [0..w] do
            let col = Vec3((float i) % float w, (float (h-k)) % float h, 0.2)
            let ir = 255.99 * col.X
            let ig = 255.99 * col.Y
            let ib = 255.99 * col.Z
            
            yield (string ir) + " " + (string ig) + " " + (string ib) + "\n"
    }
    File.WriteAllLines (@"./result.ppm", body) |> ignore
    0 // return an integer exit code
