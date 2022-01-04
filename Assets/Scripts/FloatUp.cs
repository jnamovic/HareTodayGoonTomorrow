using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUp : MonoBehaviour
{
    private float speed = 100.0f;

    private float counter;
    private float maxTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counter < maxTime)
            counter += Time.deltaTime;
        else
            transform.localPosition += Vector3.up * speed * Time.deltaTime;
    }
}
