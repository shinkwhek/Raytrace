module Object
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
                if discriminant < 0.
                then None
                else Some((-b - sqrt(discriminant))/(2.*a)) // =t
