using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudderStep : MonoBehaviour
{
    public float increment = 0.0625f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
		{
            transform.position = new Vector3(transform.position.x, transform.position.y + increment, transform.position.z);
		}
    }
}
