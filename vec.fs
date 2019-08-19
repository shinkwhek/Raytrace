module Vec

type Vec = Vec3 of float * float * float
    with
        member this.X =
            match this with
            | Vec3(x,_,_) -> x
        member this.Y =
            match this with
            | Vec3(_,y,_) -> y
        member this.Z =
            match this with
            | Vec3(_,_,z) -> z

        static member ( + )(l, r) =
            match l,r with
            | Vec3(x1,y1,z1), Vec3(x2,y2,z2) -> Vec3(x1+x2, y1+y2, z1+z2)

        static member ( - )(l, r) =
            match l,r with
            | Vec3(x1,y1,z1), Vec3(x2,y2,z2) -> Vec3(x1-x2, y1-y2, z1-z2)

        static member ( * )(l, r) =
            match l,r with
            | Vec3(x,y,z),r -> Vec3(x*r, y*r, z*r)

        static member ( / )(l, r) =
            match l with
            | Vec3(x,y,z) -> Vec3(x/r, y/r, z/r)

        member this.Minus =
            match this with
            | Vec3(x,y,z) -> Vec3(-x,-y,-z)

        static member Dot(l, r) =
            match l,r with 
            | Vec3(x1, y1, z1), Vec3(x2, y2, z2) -> x1*x2 + y1*y2 + z1*z2

        static member Cross(l, r) =
            match l,r with
            | Vec3(x1, y1, z1), Vec3(x2, y2, z2) ->
                Vec3(y1*z2 - y2*z1,
                     z1*x2 - z2*x1,
                     x1*y2 - x2*y2)

        member this.Unit =
            match this with
            | v -> v / sqrt(Vec.Dot(v, v)) 