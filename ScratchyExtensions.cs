using UnityEngine;
using System.Collections;


public static class ScratchyExtensions
{
    public static Bounds Intersect(this Bounds a, Bounds b)
    {
        float x1 = Mathf.Max(a.min.x, b.min.x);
        float x2 = Mathf.Min(a.max.x, b.max.x);
        float y1 = Mathf.Max(a.min.y, b.min.y);
        float y2 = Mathf.Min(a.max.y, b.max.y);

        var bounds = new Bounds();
        if (x2 >= x1 && y2 >= y1)
        {
            bounds.SetMinMax(new Vector3(x1, y1, 0), new Vector3(x2, y2, 0));
        }
        return bounds;
    }


}

