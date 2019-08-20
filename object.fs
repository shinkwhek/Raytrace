module Object
open System
open Vec
open Ray

type HitRecord(t: float, v: Vec, n: Vec) =
  struct
    member this.T = t
    member this.P = v
    member this.N = n
  end

type Obj = Sphere of Vec * float
    with
        member this.Hit(r: Ray, tMax, tMin) =
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
      let rec hitIter(l: Obj list, r:Ray, tMax, tMin, record) =
        match l with
        | h::tl ->
          match h.Hit(r, tMax, tMin) with
          | Some a -> hitIter(tl, r, a.T, tMin, Some a)
          | None -> hitIter(tl, r, tMax, tMin, record)
        | [] ->
          match record with
          | Some a -> Some a
          | None -> record

      match this with
      | World(lst) -> hitIter(lst, r, tMax, tMin, None)
        
        
      