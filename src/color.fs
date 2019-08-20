module Color
open System
open Vec
open Ray
open Object

let rec color(r: Ray, w: ObjList, depth) =
    match w.Hit(r, TMax, 0.001) with
    | Some h, Some m ->
      if depth < 50 then
        match m.Scatter(r, h) with
        | Some (s, {X=ax; Y=ay; Z=az}) ->
          match color(s, w, depth+1) with
          | {X=x; Y=y; Z=z} -> Vec.New(ax*x, ay*y, az*z)
        | _ -> Vec.Zero
      else
        Vec.Zero
        
    | _ ->
        let a = r.Direction.Unit
        let t = 0.5 * (a.Y + 1.)
        Vec.New(1.,1.,1.)*(1.-t) + Vec.New(0.5, 0.7, 1.)*t
