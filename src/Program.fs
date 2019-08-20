open System
open System.IO
open Vec
open Ray
open Object
open Camera
open Color


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

        let s1 = Sphere(Vec3(0., 0., -1.), 0.5, Lambertian(Vec3(0.8,0.3,0.3)))
        let s2 = Sphere(Vec3(0., -100.5, -1.), 100., Lambertian(Vec3(0.8,0.8,0.)))
        let s3 = Sphere(Vec3(1.,0.,-1.), 0.5, Metal(Vec3(0.8,0.6,0.2), 0.3))
        let s4 = Sphere(Vec3(-1.,0.,-1.), 0.5, Dielectric(1.5))
        let s5 = Sphere(Vec3(-1.,0.,-1.), -0.45, Dielectric(1.5))
        let world = World [s1; s2; s3; s4]

        for k in List.rev [0..h-1] do
        for i in [0..w-1] do
            let col = seq {
                for _ in [0..ns-1] do
                let u = ((float i)+rand.NextDouble()) / float w
                let v = ((float k)+rand.NextDouble()) /float h
                let r = Camera().GetRay(u, v)
                yield color(r, world, 0)
            }
            let col = (col|>Seq.sum) / (float ns)
            let col = Vec3(sqrt(col.X), sqrt(col.Y), sqrt(col.Z))
            let ir = int(255.99 * col.X)
            let ig = int(255.99 * col.Y)
            let ib = int(255.99 * col.Z)
            yield (string ir) + " " + (string ig) + " " + (string ib)
    }
    File.WriteAllLines (@"./result.ppm", body) |> ignore
    0 // return an integer exit code
