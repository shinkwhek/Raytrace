open System
open System.IO
open Vec
open Ray

type Obj = Sphere of Vec * float
    with
        member this.Hit(r: Ray) =
            match this with
            | Sphere(center, radius) ->
                // C: Sphere center,
                // Dot(p(t)-C,p(t)-C) = R^2
                // Dot(A+tB-C,A+tB-C) = R^2
                // t^2 B^2 + 2t B*(A-C) + (A-C)^2 - R^2 = 0
                let oc = r.Origin - center
                let a = Vec.Dot(r.Direction, r.Direction)
                let b = 2. * Vec.Dot(r.Direction, oc)
                let c = Vec.Dot(oc, oc) - radius*radius
                let discriminant = b*b - 4.*a*c
                discriminant > 0.

let color(r: Ray) : Vec =
    if Sphere(Vec3(0., 0. ,-1.), 0.5).Hit(r)
    then
        Vec3(1., 0., 0.)
    else
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
