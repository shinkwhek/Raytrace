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

  let spheres =
    seq {
      yield Sphere
              (Vec.New(0., -1000., 0.), 1000.,
               Lambertian(Vec.New(0.5, 0.5, 0.5)))
      for a in [ -11..11 ] do
        for b in [ -11..11 ] do
          let matChoise = rand.NextDouble()
          let center =
            Vec.New
              (float a + 0.9 * rand.NextDouble(), 0.2,
               float b + 0.9 * rand.NextDouble())
          if (center - Vec.New(4., 0.2, 0.)).Length > 0.9 then
            yield Sphere
                    (center, 0.2,
                     Lambertian
                       (Vec.New
                          (rand.NextDouble() * rand.NextDouble(),
                           rand.NextDouble() * rand.NextDouble(),
                           rand.NextDouble() * rand.NextDouble())))
          else if (matChoise < 0.95) then
            yield Sphere
                    (center, 0.2,
                     Metal
                       (Vec.New
                          (0.5 * (1. + rand.NextDouble()),
                           0.5 * (1. + rand.NextDouble()),
                           0.5 * (1. + rand.NextDouble())),
                        0.5 * rand.NextDouble()))
          else yield Sphere(center, 0.2, Dielectric(1.5))
        yield Sphere(Vec.New(0., 1., 0.), 1., Dielectric(1.5))
        yield Sphere
                (Vec.New(-4., 1., 0.), 1., Lambertian(Vec.New(0.4, 0.2, 0.1)))
        yield Sphere
                (Vec.New(4., 1., 0.), 1.0, Metal(Vec.New(0.7, 0.6, 0.5), 0.))
    }
  { Objs = (spheres |> Seq.toList) }

[<EntryPoint>]
let main argv =
  let w = 500
  let h = 250
  let ns = 50
  let rand = System.Random()

  let body =
    printfn "init!"
    let initial =
      seq {
        yield "P3"
        yield (string w) + " " + (string h)
        yield "255"
      } |> Seq.toList
    printfn "set initial!"

    let world = randomScene()
    let camera =
      Camera.New
        (Vec.New(-2., 1.5, 1.5), Vec.New(0., 0., -1.), Vec.New(0., 1., 0.),
         60., float (w) / float (h))

    printfn "set world, camera!"

    let colorFetch (k, i) =
      printfn "start %A %A" k i
      async {
        let randPathFetch n =
          async {
            let u = ((float i) + rand.NextDouble()) / float w
            let v = ((float k) + rand.NextDouble()) / float h
            let r = camera.GetRay(u, v)
            return color (r, world, 0)
          }
        let col =
          [ 0..ns-1 ]
          |> Seq.map randPathFetch
          |> Async.Parallel
          |> Async.RunSynchronously

        printfn "---- ---- ---- %A %A done" k i 

        let col = (col |> Seq.sum) / (float ns)
                  |> Vec.Iter sqrt
        let ir, ig, ib = int (255.99 * col.X), int (255.99 * col.Y), int (255.99 * col.Z)
        return (string ir) + " " + (string ig) + " " + (string ib)
      }
      
    let pixel =
      seq {
        for k in List.rev [ 0..h - 1 ] do
          for i in [ 0..w - 1 ] do
            yield (k, i)
      }
      
    pixel
    |> Seq.map colorFetch
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Seq.toList
    |> List.append initial

  printfn "all done"
    
  File.WriteAllLines(@"./result.ppm", body) |> ignore
  0 // return an integer exit code
