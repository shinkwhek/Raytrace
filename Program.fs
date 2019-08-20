open System
open System.IO
open Vec
open Ray
open Object

let color(r: Ray) : Vec =
    let sphereCenter = Vec3(0., 0., -1.)
    match Sphere(sphereCenter, 0.5).Hit(r) with
    | Some t ->
        let N = (r.Point(t) - sphereCenter).Unit
        Vec3(N.X+1., N.Y+1., N.Z+1.) * 0.5
    | None ->
        let a = r.Direction.Unit
        let t = 0.5 * (a.Y + 1.)
        Vec3(1.,1.,1.)*(1.-t) + Vec3(0.5, 0.7, 1.)*t


[<EntryPoint>]
let main argv =
    let w = 200
    let h = 100
    let body = seq {
        yield "P3"
        yield (string w) + " " + (string h)
        yield "255"
        
        let lowerLeftCorner = Vec3(-2., -1., -1.)
        let horizontal = Vec3(4., 0., 0.)
        let vertical = Vec3(0., 2., 0.)
        let origin = Vec3(0., 0., 0.)

        for k in List.rev [0..h-1] do
        for i in [0..w-1] do
            let u = horizontal * ((float i)/float w)
            let v =vertical * ((float k)/float h)
            let r = Ray3(origin, lowerLeftCorner + u + v)
            let col = color(r)
            let ir = int(255.99 * col.X)
            let ig = int(255.99 * col.Y)
            let ib = int(255.99 * col.Z)
            
            yield (string ir) + " " + (string ig) + " " + (string ib)
    }
    File.WriteAllLines (@"./result.ppm", body) |> ignore
    0 // return an integer exit code
