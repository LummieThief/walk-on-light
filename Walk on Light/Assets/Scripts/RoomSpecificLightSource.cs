using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpecificLightSource : LightSource
{
	private void OnEnable()
	{
		Room.OnRoomChange += RecieveRoomChange;
	}
	private void OnDisable()
	{
		Room.OnRoomChange -= RecieveRoomChange;
	}

	private void RecieveRoomChange(Room newRoom)
	{
		meshObject = newRoom.lightMesh.gameObject;
		filter = meshObject.GetComponent<MeshFilter>();
		edges = meshObject.GetComponentsInChildren<EdgeCollider2D>();
	}
}
