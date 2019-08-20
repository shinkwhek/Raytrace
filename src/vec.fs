module Vec

[<Struct>]
type Vec = 
    { X : float
      Y : float
      Z : float }
    with
        static member New(x,y,z) =
            { X=x; Y=y; Z=z }
        static member Zero = Vec.New(0., 0., 0.)

        static member ( + )(l, r) =
            match l,r with
            | {X=x1; Y=y1; Z=z1}, {X=x2; Y=y2; Z=z2} -> Vec.New(x1+x2, y1+y2, z1+z2)

        static member ( - )(l, r) =
            match l,r with
            | {X=x1; Y=y1; Z=z1}, {X=x2; Y=y2; Z=z2} -> Vec.New(x1-x2, y1-y2, z1-z2)

        static member ( * )(l, r) =
            match l,r with
            | {X=x; Y=y; Z=z},r -> Vec.New(x*r, y*r, z*r)

        static member ( / )(l, r) =
            match l with
            | {X=x; Y=y; Z=z} -> Vec.New(x/r, y/r, z/r)

        member this.Minus =
            match this with
            | {X=x; Y=y; Z=z} -> Vec.New(-x,-y,-z)

        static member Dot(l, r) =
            match l,r with 
            | {X=x1; Y=y1; Z=z1}, {X=x2; Y=y2; Z=z2} -> x1*x2 + y1*y2 + z1*z2

        static member Cross(l, r) =
            match l,r with
            | {X=x1; Y=y1; Z=z1}, {X=x2; Y=y2; Z=z2} ->
                Vec.New(y1*z2 - y2*z1,
                     z1*x2 - z2*x1,
                     x1*y2 - x2*y2)
        
        member this.Length =
            match this with
            | v -> sqrt(Vec.Dot(v,v))
        
        member this.Unit =
            match this with
            | v -> v / v.Length 