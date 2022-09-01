using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Room : MonoBehaviour
{
	public static Room activeRoom;

	[SerializeField]
	private Transform roomFrame;

	[SerializeField]
	private SetupLight startingLight;

	[SerializeField]
	private GameObject polygons;

	//[SerializeField]
	//private bool startingRoom;
	private const int roomOverlap = 1;

	private static int numRooms;
	private Transform cam;
	public int roomID { get; private set; }

	public bool isActiveRoom;
    public bool playerIn { get; private set; }
	public float topBound { get; private set; }
	public float bottomBound { get; private set; }
	public float leftBound { get; private set; }
	public float rightBound { get; private set; }

	//public LightSource lightSource;
	public MeshRenderer lightMesh;


	public delegate void RoomChange(Room newRoom);
	public static event RoomChange OnRoomChange;

	private Room[] rooms;
	
	private void Awake()
	{
		polygons.SetActive(false);
		rooms = FindObjectsOfType<Room>();
		roomID = numRooms;
		Debug.Log(gameObject + " " + roomID);
		numRooms++;
		cam = GameObject.FindGameObjectWithTag("MainCamera").transform;

		
		InitializeBounds();
		//Debug.Log("top:" + topBound + " bottom:" + bottomBound + " left:" + leftBound + " right:" + rightBound);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag.Equals("Player"))
		{
			if(activeRoom == null)
			{
				SetupNewRoom(this);
			}
			Debug.Log("player in");
			playerIn = true;
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag.Equals("Player"))
		{
			playerIn = false;
			isActiveRoom = false;
			polygons.SetActive(false);
			foreach(Room r in rooms)
			{
				if(r.playerIn)
				{
					SetupNewRoom(r);
					break;
				}
			}
		}
	}
	private void SetupNewRoom(Room r)
	{
		Debug.Log("player is in room " + r.roomID);
		r.isActiveRoom = true;
		activeRoom = r;
		r.polygons.SetActive(true);
		cam.transform.position = new Vector3(r.transform.position.x, r.transform.position.y, cam.position.z);
		roomFrame.position = r.transform.position;
		Physics.SyncTransforms();
		r.startingLight.SetUp();
		
		OnRoomChange(r);
	}
	private void InitializeBounds()
	{
		BoxCollider2D col = GetComponent<BoxCollider2D>();
		leftBound = transform.position.x - (col.size.x + roomOverlap) / 2;
		rightBound = transform.position.x + (col.size.x + roomOverlap) / 2;
		topBound = transform.position.y + (col.size.y + roomOverlap) / 2;
		bottomBound = transform.position.y - (col.size.y + roomOverlap) / 2;
	}

	public bool OnBoundary(Vector2 point, float tolerance)
	{
		return (point.x >= leftBound - tolerance && point.x <= leftBound + tolerance ||
				point.x >= rightBound - tolerance && point.x <= rightBound + tolerance ||
				point.y >= topBound - tolerance && point.y <= topBound + tolerance ||
				point.y >= bottomBound - tolerance && point.y <= bottomBound + tolerance);
	}
	public bool OnBoundary(Vector2 point)
	{
		return point.x == leftBound || point.x == rightBound ||
				point.y == topBound || point.y == bottomBound;
	}
}
