module Camera
open Vec
open Ray

type Camera =
  struct
    member this.LowerLeftCorner = Vec3(-2., -1., -1.)
    member this.Horizonral = Vec3(4., 0., 0.)
    member this.Vertical = Vec3(0., 2., 0.)
    member this.Origin = Vec3(0., 0., 0.)

    member this.GetRay(u, v) =
      Ray3(this.Origin, this.LowerLeftCorner + this.Horizonral*u + this.Vertical*v )
  end