using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LightSource))]
public class SetupLight : MonoBehaviour
{
    public bool setUpOnLoad;
	private bool setUp;

	// Update is called once per frame
	void LateUpdate()
    {
        if(setUpOnLoad)
		{
			SetUp();
			setUpOnLoad = false;
		}
    }

	public void SetUp()
	{
		if(!setUp)
		{
			setUp = true;
			GetComponent<LightSource>().Recalculate();
		}
	}
}
