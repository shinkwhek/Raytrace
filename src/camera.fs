module Camera
open System
open Vec
open Ray

type Camera = Cam of Vec * Vec * Vec * Vec
  with
    member this.LowerLeftCorner = match this with Cam(a,_,_,_) -> a
    member this.Horizonral = match this with Cam(_,a,_,_) -> a
    member this.Vertical = match this with Cam(_,_,a,_) -> a
    member this.Origin = match this with Cam(_,_,_,a) -> a

    static member New(lookfrom: Vec, lookat: Vec, vup: Vec, vfov: float, aspect: float) =
      let theta = vfov * Math.PI/180.
      let halfHeight = Math.Tan(theta/2.)
      let halfWidth = aspect * halfHeight
      let w = (lookfrom - lookat).Unit
      let u = Vec.Cross(vup, w).Unit
      let v = Vec.Cross(w, u)
      Cam (lookfrom - u*halfWidth - v*halfHeight - w,
           u*halfWidth*2.,
           v*halfHeight*2.,
           lookfrom)

    member this.GetRay(u, v) =
      Ray3(this.Origin, this.LowerLeftCorner + this.Horizonral*u + this.Vertical*v - this.Origin)
  