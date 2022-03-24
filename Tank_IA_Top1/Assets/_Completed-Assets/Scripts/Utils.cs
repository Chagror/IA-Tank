using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool CompareVectors(Vector3 vector1, Vector3 vector2, float margin)
    {
        return (Mathf.Abs(vector1.x - vector2.x) < margin
                && Mathf.Abs(vector1.z - vector2.z) < margin);
    }
}
