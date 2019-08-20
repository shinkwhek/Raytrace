module Color
open System
open Vec
open Ray
open Object

[<Literal>]
let TMax = System.Double.MaxValue
[<Literal>]
let TMin = 0.

let randInUnitSphere() =
  let rnd = System.Random()
  let rec iter() =
    let p = Vec3(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble())*2. - Vec3(1.,1.,1.)
    if Vec.Dot(p,p) >= 1.
    then iter()
    else p
  iter()
      

let rec color(r: Ray, w: ObjList) =
    match w.Hit(r, TMax, 0.001) with
    | Some h ->
        let target = h.P + h.N + randInUnitSphere()
        color( Ray3(h.P, target-h.P),w ) * 0.5
    | None ->
        let a = r.Direction.Unit
        let t = 0.5 * (a.Y + 1.)
        Vec3(1.,1.,1.)*(1.-t) + Vec3(0.5, 0.7, 1.)*t
