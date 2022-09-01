using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polaroid : MonoBehaviour
{
    public LightSource lightSource;
    public MeshRenderer lightMesh;
    public Transform player;
    public MeshRenderer focusLightMesh;
    private int flashDuration = 40; //60 = 1 second

    public Material flashOnMat;
    public Material flashOffMat;

    private float flashDelay = 0.25f;
	// Start is called before the first frame update
	private void Start()
	{
        //lightSource = Room.activeRoom.lightSource;
        //lightMesh = Room.activeRoom.lightMesh;
	}

	// Update is called once per frame
	void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1))
		{
            StartFocussing();
		}
        if(Input.GetKeyUp(KeyCode.Mouse1))
		{
            StopFocussing();
		}
        if(Input.GetKeyDown(KeyCode.Mouse0))
		{
            StopCoroutine("TakePicture");
            StartCoroutine("TakePicture");
		}
    }

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
        //lightSource = newRoom.lightSource;
        lightMesh = newRoom.lightMesh;
	}

    private void StartFocussing()
	{
        focusLightMesh.enabled = true;
    }

    private void StopFocussing()
	{
        focusLightMesh.enabled = false;
    }
    IEnumerator TakePicture()
	{
        StopFocussing();
        yield return new WaitForSeconds(flashDelay);
        lightSource.transform.position = player.position;
        lightSource.Recalculate();
        StopCoroutine("Flash");
        StartCoroutine("Flash");
        
    }

    IEnumerator Flash()
    {
        lightMesh.sharedMaterial.Lerp(flashOnMat, flashOnMat, 0);
        for(int i = 0; i < flashDuration; i++)
		{
            lightMesh.sharedMaterial.Lerp(flashOnMat, flashOffMat, Mathf.Pow((i / (float)flashDuration), 1.4f));
            yield return new WaitForFixedUpdate();
		}
    }
}
