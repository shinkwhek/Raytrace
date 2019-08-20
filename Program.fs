open System
open System.IO
open Vec
open Ray
open Object
open Camera

[<Literal>]
let TMax = System.Double.MaxValue
[<Literal>]
let TMin = 0.

let color(r: Ray, w: ObjList) =
    match w.Hit(r, TMax, 0.) with
    | Some h ->
        Vec3(h.N.X+1., h.N.Y+1., h.N.Z+1.) * 0.5
    | None ->
        let a = r.Direction.Unit
        let t = 0.5 * (a.Y + 1.)
        Vec3(1.,1.,1.)*(1.-t) + Vec3(0.5, 0.7, 1.)*t


[<EntryPoint>]
let main argv =
    let w = 200
    let h = 100
    let ns = 100
    let rand = System.Random ()

    let body = seq {
        yield "P3"
        yield (string w) + " " + (string h)
        yield "255"

        let s1 = Sphere(Vec3(0., 0., -1.), 0.5)
        let s2 = Sphere(Vec3(0., -100.5, -1.), 100.)
        let world = World [s1; s2]

        for k in List.rev [0..h-1] do
        for i in [0..w-1] do
            let col = seq {
                for _ in [0..ns-1] do
                let u = ((float i)+rand.NextDouble()) / float w
                let v = ((float k)+rand.NextDouble()) /float h
                let r = Camera().GetRay(u, v)
                yield color(r, world)
            }
            let col = (col|>Seq.sum) / (float ns)
            let ir = int(255.99 * col.X)
            let ig = int(255.99 * col.Y)
            let ib = int(255.99 * col.Z)
            yield (string ir) + " " + (string ig) + " " + (string ib)
    }
    File.WriteAllLines (@"./result.ppm", body) |> ignore
    0 // return an integer exit code
