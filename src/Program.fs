open System
open System.IO
open Vec
open Ray
open Object
open Camera
open Color

let randomScene() =
    let rand = System.Random()
    let n = 500
    let spheres = seq {
        yield Sphere(Vec3(0.,-1000.,0.), 1000., Lambertian(Vec3(0.5,0.5,0.5)))
        for a in [-11..11] do
        for b in [-11..11] do
            let matChoise = rand.NextDouble()
            let center = Vec3(float a + 0.9*rand.NextDouble(),
                              0.2,
                              float b + 0.9*rand.NextDouble())
            if (center - Vec3(4.,0.2,0.)).Length > 0.9
            then
                yield Sphere(center, 0.2, Lambertian(Vec3(rand.NextDouble()*rand.NextDouble(),
                                                          rand.NextDouble()*rand.NextDouble(),
                                                          rand.NextDouble()*rand.NextDouble())))
            else if (matChoise < 0.95)
                then
                    yield Sphere(center, 0.2, Metal(Vec3(0.5*(1.+rand.NextDouble()),
                                                         0.5*(1.+rand.NextDouble()),
                                                         0.5*(1.+rand.NextDouble())),
                                                         0.5*rand.NextDouble()))
                else
                    yield Sphere(center, 0.2, Dielectric(1.5))
        yield Sphere(Vec3(0.,1.,0.), 1., Dielectric(1.5))
        yield Sphere(Vec3(-4.,1.,0.), 1., Lambertian(Vec3(0.4,0.2,0.1)))
        yield Sphere(Vec3(4.,1.,0.), 1.0, Metal(Vec3(0.7, 0.6, 0.5), 0.))
    }
    World (spheres |> Seq.toList)

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

        let world = randomScene()

        let camera = Camera.New(Vec3(-2.,2.,1.),
                                Vec3(0.,0.,-1.),
                                Vec3(0.,1.,0.),
                                90.,
                                float(w)/float(h))

        for k in List.rev [0..h-1] do
        for i in [0..w-1] do
            let col = seq {
                for _ in [0..ns-1] do
                let u = ((float i)+rand.NextDouble()) / float w
                let v = ((float k)+rand.NextDouble()) /float h
                let r = camera.GetRay(u, v)
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
