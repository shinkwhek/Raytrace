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

type Material = Lambertian of Vec | Metal of Vec * float | Dielectric of float
  with
    static member RandInUnitSphere() =
      let rnd = System.Random()
      let rec iter() =
        let p = Vec3(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble())*2. - Vec3(1.,1.,1.)
        if Vec.Dot(p,p) >= 1.
        then iter()
        else p
      iter()

    static member ReflectMetal(v,n) =
      v- n*Vec.Dot(v,n)*2.

    static member ReflectDielectric(v: Vec, n: Vec, niOvernt) =
      let uv = v.Unit
      let dt = Vec.Dot(uv, n)
      let discriminant = 1. - niOvernt*niOvernt*(1.-dt*dt)
      if discriminant > 0.
      then
        Some ((uv-n*dt)*niOvernt - n*sqrt(discriminant))
      else
        None
    
    static member Schlick(cosine, refIdx) =
      let r0 = (1.-refIdx) / (1.+refIdx)
      let r0 = r0*r0
      r0 + (1.-r0)*(1.-cosine)**5.

    member this.Scatter(r: Ray, record: HitRecord) =
      match this with
      | Lambertian a ->
        let target = record.P + record.N + Material.RandInUnitSphere()
        let scattered = Ray3(record.P, target-record.P)
        let attenuation = a
        Some ( scattered, attenuation )

      | Metal (a,f) ->
        let fuzz = if f < 1. then f else 1.
        let reflected = Material.ReflectMetal(r.Direction.Unit, record.N)
        let scattered = Ray3(record.P, reflected+Material.RandInUnitSphere()*fuzz)
        let attenuation = a
        if Vec.Dot(scattered.Direction, record.N) > 0.
        then
          Some ( scattered, attenuation )
        else
          None

      | Dielectric refIdx ->
        let reflacted = Material.ReflectMetal(r.Direction, record.N)
        let attenuation = Vec3(1.,1.,1.)
        let outwardNormal, niOverNt, cosine =
          if Vec.Dot(r.Direction, record.N) > 0.
          then record.N.Minus, refIdx, Vec.Dot(r.Direction,record.N)*refIdx / r.Direction.Length
          else record.N, 1./refIdx, -Vec.Dot(r.Direction,record.N)*refIdx / r.Direction.Length
        let scattered, reflectProb =
          match Material.ReflectDielectric(r.Direction, outwardNormal, niOverNt) with
          | Some a -> Ray3(record.P, a), Material.Schlick(cosine, refIdx)
          | None -> Ray3(record.P, reflacted), 1.
        match Material.ReflectDielectric(r.Direction, outwardNormal, niOverNt) with
        | Some a -> Some(Ray3(record.P, a), attenuation)
        | None -> None

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
        
        
      