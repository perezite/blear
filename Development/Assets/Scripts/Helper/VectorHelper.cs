using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VectorHelper
{
    public static bool Approximately(Vector2 a, Vector2 b)
    {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
    }

    public static Vector2 Reflect(Vector2 v, Vector2 n)
    {
        Vector3 r3D = Vector3.Reflect(v, n).normalized;
        return new Vector2(r3D.x, r3D.y);
    }

    public static Vector2? ScreenPointToWorldPoint(Vector3 screenPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        Plane plane = new Plane(new Vector3(0f, 0f, -1f), Vector3.zero);
        float distance = 0;

        if (plane.Raycast(ray, out distance))
        {
            Vector2 location = ray.GetPoint(distance);
            return location;
        }

        return null;
    }

    public static Vector3 SmoothStep(Vector3 from, Vector3 to, float t)
    {
        return new Vector3(
            Mathf.SmoothStep(from.x, to.x, t),
            Mathf.SmoothStep(from.y, to.y, t),
            Mathf.SmoothStep(from.z, to.z, t)); 
    }

    public static Vector3 Sum(this List<Vector3> vecs)
    {
        return new Vector3(vecs.Select(v => v.x).Sum(), vecs.Select(v => v.y).Sum(), vecs.Select(v => v.z).Sum());
    } 

    public static Vector2 Sum(this List<Vector2> vecs)
    {
        return new Vector3(vecs.Select(v => v.x).Sum(), vecs.Select(v => v.y).Sum());
    }
}