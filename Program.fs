open System
open System.IO

type V(x: float, y: float, z: float) = 
    struct
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
    end

type Ray(origin: V, direction: V) =
    struct
        member this.O = origin 
        member this.D = direction
    end

type Sphere(v: V, r: float) =
    struct
        member this.P = v 
        member this.R = r
    end
    
type Scene =
    struct
        member sphere: IVector<Sphere>
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
            let x: float = (float i) % float w
            let y: float = (float i) % float h
            let ray = Ray(V(2.*(x/(float w))-1., 2.*(y/(float h))-1., 5.), V(0., 0., -1.))

            yield "255 0 255\n"
    }
    File.WriteAllLines (@"./result.ppm", body) |> ignore
    0 // return an integer exit code
