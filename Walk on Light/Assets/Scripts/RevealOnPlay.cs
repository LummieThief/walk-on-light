using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(mr != null)
		{
            mr.enabled = true;
		}
        if(sr != null)
		{
            sr.enabled = true;
		}            
    }
}
