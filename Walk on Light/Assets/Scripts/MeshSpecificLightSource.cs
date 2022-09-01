using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSpecificLightSource : LightSource
{
	public GameObject lightMesh;
	protected override void Start()
	{
		meshObject = lightMesh;
		filter = meshObject.GetComponent<MeshFilter>();
		edges = meshObject.GetComponentsInChildren<EdgeCollider2D>();

		base.Start();
	}
}
