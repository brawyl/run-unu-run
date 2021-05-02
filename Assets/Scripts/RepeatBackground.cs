using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatLength;

    private void Start()
    {
        startPos = transform.position; // Establish the default starting position 
        repeatLength = GetComponent<BoxCollider>().size.z; // Set repeat length to half of the background
    }

    private void Update()
    {
        // If background moves down by its repeat length, move it back to start position
        if (transform.position.z < startPos.z - repeatLength)
        {
            transform.position = startPos;
        }
    }


}


