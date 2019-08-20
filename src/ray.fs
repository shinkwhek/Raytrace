module Ray

open Vec

[<Struct>]
type Ray =
  { Origin : Vec
    Direction : Vec }

  static member New(o : Vec, d : Vec) =
    { Origin = o
      Direction = d }

  member this.Point(t) =
    match this with
    | { Origin = a; Direction = b } -> a + b * t
