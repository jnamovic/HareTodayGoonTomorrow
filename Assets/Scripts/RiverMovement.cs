using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverMovement : MonoBehaviour
{
    private Vector3 direction;
    public float speed;

    // Update is called once per frame
    void Update()
    {

    }

    void onCollisionStay(Collider other)
    {
        if (other.gameObject.tag == "Mouse")
        {
            direction = transform.forward;
            direction = direction * speed;

            Rigidbody mrb = other.GetComponent<Rigidbody>();
            // Add a WORLD force to the other objects
            // Ignore the mass of the other objects so they all go the same speed (ForceMode.Acceleration)
            mrb.AddForce(direction, ForceMode.Acceleration);
        }
    }
}