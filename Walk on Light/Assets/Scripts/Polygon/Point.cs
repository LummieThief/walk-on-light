using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : Polygon
{
    void Awake()
    {
        List<Vector2> list = new List<Vector2>();
        list.Add(transform.position);
        SetCorners(list);
    }

}
