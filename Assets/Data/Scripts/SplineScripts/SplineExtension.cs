using System.Collections.Generic;
using UnityEngine;


public static class SplineExtension 
{
    public static Vector3[] GetNormals2D(this Spline spline)
    {
        List<Vector3> normals = new List<Vector3>(); 
        
        Vector3 firstDirection = spline[1].Position - spline[0].Position;
        Vector3 firstNormal = new Vector3(-firstDirection.y, firstDirection.x, 0).normalized;
        
        normals.Add(firstNormal);
        
        for (int i = 1; i < spline.Length-1; i++)
        {
            Vector3 source = spline[i].Position;
            Vector3 target = spline[(i + 1) % spline.Length].Position;
			     
            Vector3 direction = target - source;
			     
            Vector3 normal = new Vector3(-direction.y, direction.x, 0).normalized;
			     
            normals.Add(normal);
        }
        
        Vector3 lastDirection = spline[spline.Length-1].Position - spline[spline.Length-2].Position;
        Vector3 lastNormal = new Vector3(-lastDirection.y, lastDirection.x, 0).normalized;
        
        normals.Add(lastNormal);

        return normals.ToArray();
    }
}
