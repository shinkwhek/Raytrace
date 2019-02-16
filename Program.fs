open System
open System.IO

type V =
    struct
        val X: float
        val Y: float
        val Z: float
        new(v: float) = {X = v; Y = v; Z = v}
        new(x: float, y: float, z: float) = {X = x; Y = y; Z = z}
    end

[<EntryPoint>]
let main argv =
    let w = 1200
    let h = 800
    let body = seq {
        yield "P3\n"
        yield (string w)
        yield (string h)
        yield "\n255\n"
        for i in [ 0..w*h ] do
            yield "255 0 255\n"
    }
    File.WriteAllLines (@"./result.ppm", body) |> ignore
    0 // return an integer exit code
