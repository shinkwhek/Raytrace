module Object
open System
open Vec
open Ray

[<Literal>]
let TMax = System.Double.MaxValue
[<Literal>]
let TMin = 0.

type HitRecord(t: float, v: Vec, n: Vec) =
  struct
    member this.T = t
    member this.P = v
    member this.N = n
  end

type Material = Lambertian of Vec
  with
    static member RandInUnitSphere() =
      let rnd = System.Random()
      let rec iter() =
        let p = Vec3(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble())*2. - Vec3(1.,1.,1.)
        if Vec.Dot(p,p) >= 1.
        then iter()
        else p
      iter()

    member this.Scatter(r: Ray, record: HitRecord) =
      match this with
      | Lambertian a ->
        let target = record.P + record.N + Material.RandInUnitSphere()
        let scattered = Ray3(record.P, target-record.P)
        let attenuation = a
        Some ( scattered, attenuation )

type Obj = Sphere of Vec * float * Material
    with
        member this.M =
          match this with
          | Sphere(_,_,m) -> m

        member this.Hit(r: Ray, tMax, tMin) =
            match this with
            | Sphere(center, radius, _) ->
                // C: Sphere center,
                // Dot(p(t)-C,p(t)-C) = R^2
                // Dot(A+tB-C,A+tB-C) = R^2
                // t^2 B^2 + 2t B*(A-C) + (A-C)^2 - R^2 = 0
                let oc = r.Origin - center
                let a = Vec.Dot(r.Direction, r.Direction)
                let b = 2. * Vec.Dot(r.Direction, oc)
                let c = Vec.Dot(oc, oc) - radius*radius
                let discriminant = b*b - 4.*a*c
                if discriminant < 0.
                then None
                else
                  let t = (-b - sqrt(discriminant))/(2.*a)
                  if (tMin < t) && (t < tMax)
                  then
                    let p = r.Point(t)
                    Some(HitRecord(t, p, (p-center)/radius))
                  else
                    let t = (-b + sqrt(discriminant))/(2.*a)
                    if (tMin < t) && (t < tMax)
                    then
                      let p = r.Point(t)
                      Some(HitRecord(t, p, (p-center)/radius))
                    else None

type ObjList = World of Obj list
  with
    member this.Hit(r: Ray, tMax, tMin) =
      let rec hitIter(l: Obj list, r:Ray, tMax, tMin, record, material) =
        match l with
        | h::tl ->
          match h.Hit(r, tMax, tMin) with
          | Some a -> hitIter(tl, r, a.T, tMin, Some a, Some h.M)
          | None -> hitIter(tl, r, tMax, tMin, record, material)
        | [] ->
          match record with
          | Some a -> Some a, material
          | None -> record, material

      match this with
      | World(lst) -> hitIter(lst, r, tMax, tMin, None, None)
        
        
      