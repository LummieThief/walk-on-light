using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{

	protected GameObject meshObject;
	protected MeshFilter filter;
	protected EdgeCollider2D[] edges;

	public bool canCollide;
	public bool dynamic;

	private List<Vector2> points;
	private float indirectRayRotation = 0.01f;

	protected virtual void Start()
	{
		GetPolys();	
	}
	private void FixedUpdate()
	{
		if(dynamic)
		{
			Recalculate();
		}
	}

	public void Recalculate()
	{
		GetPolys();
		CreateLightMesh();
		if (canCollide)
		{ 
			CreateEdgeCollider();
		}
	}

	private void GetPolys()
	{
		Polygon[] polys = FindObjectsOfType<Polygon>();
		points = new List<Vector2>();
		foreach (Polygon poly in polys)
		{
			foreach (Vector2 point in poly.GetCorners())
			{
				points.Add(point);
			}
		}
	}

	//You dont have to recalculate the triangles array every frame if no vertices are added/removed from the scene,
	//but its just one forloop which shouldnt be too bad.
	private void CreateLightMesh()
	{
		//a list containing the angle between each vertex and this gameobject,
		//in reverse sorted order
		List<float> angleList = new List<float>();
		//a list containing the points corresponding to the angles in the other list.
		List<Vector2> pointList = new List<Vector2>();
		//we only really need the pointList, but in order to sort the pointList by angle,
		//we sort the angleList, then put the points at the corresponding indices.

		foreach (Vector2 point in points)
		{
			RaycastHit hit;
			//the ray that fires directly at the vertex
			Ray direct = new Ray(transform.position, (Vector3)point - transform.position);
			//the ray that fires slightly clockwise from the vertex
			Ray clockwise = new Ray(transform.position,
				RotatePointAroundPivot(point, transform.position, new Vector3(0, 0, indirectRayRotation)) - transform.position);
			//the ray that fires slightly counterclockwise from the vertex
			Ray counterclockwise = new Ray(transform.position,
				RotatePointAroundPivot(point, transform.position, new Vector3(0, 0, -indirectRayRotation)) - transform.position);
			if (Physics.Raycast(direct, out hit)) //direct hit
			{
				ReverseSortedInsertByAngle(hit.point, angleList, pointList);
				Debug.DrawLine(transform.position, hit.point);
			}
			if (Physics.Raycast(clockwise, out hit)) //clockwise hit
			{
				ReverseSortedInsertByAngle(hit.point, angleList, pointList);
				Debug.DrawLine(transform.position, hit.point);
			}
			if (Physics.Raycast(counterclockwise, out hit)) //counterclockwise hit
			{
				ReverseSortedInsertByAngle(hit.point, angleList, pointList);
				Debug.DrawLine(transform.position, hit.point);
			}
		}
		//Debug.Log(leftBound + "   " + rightBound);
		//pointList is now full of all the points sorted in clockwise order
		
		//vertices hold the vertices of all the polygons in the mesh
		Vector3[] vertices = new Vector3[pointList.Count + 2];
		//we +2ed it because we need to store the lights position, and add the first point to 
		//the end of the array so it wraps around.

		//the lights position
		vertices[0] = transform.position;
		for(int i = 0; i < pointList.Count; i++)
		{
			vertices[i + 1] = pointList[i];
		}
		//the first point again so it wraps around
		vertices[vertices.Length - 1] = vertices[1];

		//triangles is an array of vertex indices that will create the polygons.
		//We need to create one triangle for each point, and each triangle has
		//3 vertices, so we multiply pointList.
		int[] triangles = new int[pointList.Count * 3];

		//Each triangle is made up of 3 points.
		//The first point is always the light (this object)
		//The second and third point are always adjacent in the pointList
		for(int i = 0; i < pointList.Count; i ++)
		{
			triangles[3*i] = 0;
			triangles[3*i + 1] = i + 1;
			triangles[3*i + 2] = i + 2;
		}

		//I dont know what the UVs actually do. I just know they are
		//the same as the vertices.
		Vector2[] uv = new Vector2[vertices.Length];
		for(int i = 0; i < vertices.Length; i++)
		{
			uv[i] = vertices[i];
		}

		//Builds the new mesh
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.RecalculateNormals();

		//applies the new mesh to the mask object
		filter.mesh = mesh;
	}

	private void CreateEdgeCollider()
	{
		Vector2[] points = filter.mesh.uv;
		List<Vector2> list = new List<Vector2>();


		bool skipping = false;
		int currentEdge = 0;
		for(int i = 0; i < edges.Length; i++)
		{
			edges[i].points = new Vector2[2];
		}
		for (int i = 0; i < points.Length - 1; i++)
		{
			if (Room.activeRoom.OnBoundary(points[i + 1]))
			{
				if (!skipping)
				{
					list.Add(points[i + 1]);
					skipping = true;
				}
			}
			else
			{
				if(skipping)
				{
					edges[currentEdge].points = list.ToArray();
					currentEdge++;
					list.Clear();
					skipping = false;
					list.Add(points[i]);
				}
				list.Add(points[i + 1]);
			}
			
		}
		edges[currentEdge].points = list.ToArray();
		currentEdge++;
		for(int i = currentEdge; i < edges.Length; i++)
		{
			edges[i].points = new Vector2[2];
		}
		//edge.points = list.ToArray();

	}

	// Rotates the point "point" around the point "pivot" by "angles" degrees
	// (angles.x and angles.y should be 0, and angles.z is the degrees you want)
	private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		return Quaternion.Euler(angles) * (point - pivot) + pivot;
	}


	//Gets the angle between a point "point" and this game objects position, then inserts the angle
	//into "angleList" in reverse-sorted order and uses that index to know where to insert "point"
	//into "pointList".
	private void ReverseSortedInsertByAngle(Vector2 point, List<float> angleList, List<Vector2> pointList)
	{
		float thisAngle = Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x);
		int i = 0;
		//{inv: items [0..i-1] of angleList are less than thisAngle}
		while(i < angleList.Count)
		{
			if (thisAngle > angleList[i])
			{
				break;
			}
			i++;
		}
		angleList.Insert(i, thisAngle);
		pointList.Insert(i, point);
	}
}
