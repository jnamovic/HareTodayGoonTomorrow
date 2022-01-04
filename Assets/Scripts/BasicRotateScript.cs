using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRotateScript : MonoBehaviour
{
    public float rotationSpeed = 10.0f;

    // ADDED
    private float y;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        // ADDED
        if (transform.childCount > 0 && transform.GetChild(0).gameObject.name == "mouse_rig_v01")
        {
            y = 0.0f;
            z = 1.0f;
        }
        else
        {
            y = 1.0f;
            z = 0.0f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // CHANGED
        transform.Rotate(new Vector3(0.0f, y, z) * Time.deltaTime * rotationSpeed);
    }
}