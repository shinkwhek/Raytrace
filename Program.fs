open System
open System.IO

type V(x: float, y: float, z: float) = 
        member this.X = x
        member this.Y = y
        member this.Z = z
        static member ( + )(l: V, r: V) =
            V(l.X + r.X, l.Y + r.Y, l.Z + r.Z)
        static member ( - )(l: V, r: V) =
            V(l.X - r.X, l.Y - r.Y, l.Z - r.Z)
        static member ( * )(l: V, r: V) =
            V(l.X * r.X, l.Y * r.Y, l.Z * r.Z)
        static member ( / )(l: V, r: float) =
            V(l.X / r, l.Y / r, l.Z / r)
        static member  minus(v: V) =
            V(-v.X, -v.Y, -v.Z)
        static member dot(a: V, b: V) =
            a.X * b.X + a.Y * b.Y + a.Z * b.Z
        static member cross(a: V, b: V) =
            V(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X)
        static member normarize(v: V) =
            v / sqrt(V.dot(v, v))

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
