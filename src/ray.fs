module Ray
open Vec

type Ray = Ray3 of Vec * Vec
    with
        member this.Origin =
            match this with
            | Ray3(a,_) -> a

        member this.Direction =
            match this with
            | Ray3(_,b) -> b

        member this.Point(t) =
            match this with
            | Ray3(a,b) -> a + b*t