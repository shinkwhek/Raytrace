open System
open System.IO

type Vec = Vec3 of float * float * float
    with
        member this.X =
            match this with
            | Vec3(x,_,_) -> x
        member this.Y =
            match this with
            | Vec3(_,y,_) -> y
        member this.Z =
            match this with
            | Vec3(_,_,z) -> z

        static member ( + )(l, r) =
            match l,r with
            | Vec3(x1,y1,z1), Vec3(x2,y2,z2) -> Vec3(x1+x2, y1+y2, z1+z2)

        static member ( - )(l, r) =
            match l,r with
            | Vec3(x1,y1,z1), Vec3(x2,y2,z2) -> Vec3(x1-x2, y1-y2, z1-z2)

        static member ( * )(l, r) =
            match l,r with
            | Vec3(x,y,z),r -> Vec3(x*r, y*r, z*r)

        static member ( / )(l, r) =
            match l with
            | Vec3(x,y,z) -> Vec3(x/r, y/r, z/r)

        member this.Minus =
            match this with
            | Vec3(x,y,z) -> Vec3(-x,-y,-z)

        static member Dot(a, b) =
            match a,b with 
            | Vec3(x1, y1, z1), Vec3(x2, y2, z2) -> x1*x2 + y1*y2 + z1*z2

        static member Cross(a, b) =
            match a,b with
            | Vec3(x1, y1, z1), Vec3(x2, y2, z2) ->
                Vec3(y1*z2 - y2*z1,
                     z1*x2 - z2*x1,
                     x1*y2 - x2*y2)

        member this.Normarize =
            match this with
            | v -> v / sqrt(Vec.Dot(v, v)) 

type Ray = Ray3 of Vec * Vec
    with
        member this.Origin =
            match this with
            | Ray3(a,_) -> a

        member this.Direction =
            match this with
            | Ray3(_,b) -> b

        member this.Point(t) =
            match this with
            | Ray3(a,b) -> a + b*t

[<EntryPoint>]
let main argv =
    let w = 200
    let h = 100
    let body = seq {
        yield "P3\n"
        yield (string w) + " " + (string h)
        yield "\n255\n"
        for k in [ 0..h ] do
            for i in [0..w] do
                let col = Vec3((float i) % float w, (float (h-k)) % float h, 0.2)
                let ir = 255.99 * col.X
                let ig = 255.99 * col.Y
                let ib = 255.99 * col.Z
            
                yield (string ir) + " " + (string ig) + " " + (string ib) + "\n"
    }
    File.WriteAllLines (@"./result.ppm", body) |> ignore
    0 // return an integer exit code
