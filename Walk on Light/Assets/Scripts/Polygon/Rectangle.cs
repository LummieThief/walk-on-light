using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : Polygon
{
    // Start is called before the first frame update
    void Awake()
    {
        List<Vector2> list = calculateCorners();
        SetCorners(list);
    }

    private List<Vector2> calculateCorners()
	{
        List<Vector2> list = new List<Vector2>();
        SpriteRenderer rendrr = GetComponent<SpriteRenderer>();
        float width = transform.localScale.x * rendrr.sprite.texture.width / rendrr.sprite.pixelsPerUnit;
        float height = transform.localScale.y * rendrr.sprite.texture.height / rendrr.sprite.pixelsPerUnit;

        Vector2 topRight = transform.position;
        Vector2 bottomRight = transform.position;
        Vector2 bottomLeft = transform.position;
        Vector2 topLeft = transform.position;


        topRight.x += width / 2;
        topRight.y += height / 2;
        bottomRight.x += width / 2;
        bottomRight.y -= height / 2;
        bottomLeft.x -= width / 2;
        bottomLeft.y -= height / 2;
        topLeft.x -= width / 2;
        topLeft.y += height / 2;

        topRight = RotatePointAroundPivot(topRight, transform.position, transform.rotation.eulerAngles);
        bottomRight = RotatePointAroundPivot(bottomRight, transform.position, transform.rotation.eulerAngles);
        bottomLeft = RotatePointAroundPivot(bottomLeft, transform.position, transform.rotation.eulerAngles);
        topLeft = RotatePointAroundPivot(topLeft, transform.position, transform.rotation.eulerAngles);

        //topRight.

        list.Add(topRight);
        list.Add(bottomRight);
        list.Add(bottomLeft);
        list.Add(topLeft);

        return list;
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
}
