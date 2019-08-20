module Camera
open System
open Vec
open Ray

[<Struct>]
type Camera =
  { LowerLeftCorner : Vec
    Horizontal : Vec
    Vertical : Vec
    Origin : Vec }
  with
    static member New(lookfrom: Vec, lookat: Vec, vup: Vec, vfov: float, aspect: float) =
      let theta = vfov * Math.PI/180.
      let halfHeight = Math.Tan(theta/2.)
      let halfWidth = aspect * halfHeight
      let w = (lookfrom - lookat).Unit
      let u = Vec.Cross(vup, w).Unit
      let v = Vec.Cross(w, u)
      { LowerLeftCorner = lookfrom - u*halfWidth - v*halfHeight - w;
        Horizontal = u*halfWidth*2.;
        Vertical = v*halfHeight*2.;
        Origin = lookfrom }

    member this.GetRay(u, v) =
      Ray.New(this.Origin, this.LowerLeftCorner + this.Horizontal*u + this.Vertical*v - this.Origin)
  