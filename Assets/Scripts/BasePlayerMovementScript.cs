using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BasePlayerMovementScript : MonoBehaviour
{
    public XboxController playerNumber;

    public float speed;
    public float rotationSpeed;
    //private CharacterController c;
    private Rigidbody rb;
    private float slowed;
    private float usedSpeed;
    private float hAxis;
    private float vAxis;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slowed = speed / 2 + 2;
        usedSpeed = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerNumber != XboxController.All && playerNumber != XboxController.Any)
        {
            hAxis = XCI.GetAxis(XboxAxis.LeftStickX, playerNumber);
            vAxis = XCI.GetAxis(XboxAxis.LeftStickY, playerNumber);
        }
        else
        {
            hAxis = Input.GetAxis("Horizontal");
            vAxis = Input.GetAxis("Vertical");
        }

        Vector3 mov = Vector3.zero;

        if (Mathf.Abs(hAxis) > 0.1f)
            mov.x = hAxis;
        else
            mov.x = 0;

        if (Mathf.Abs(vAxis) > 0.1f)
            mov.z = vAxis;
        else
            mov.z = 0;

        if (mov != Vector3.zero)
        {
            //GetComponent<Animator>().SetBool("moving", true);

            Vector3 rotation = Quaternion.LookRotation(mov).eulerAngles;

            rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * rotationSpeed);
        }

        if (Mathf.Abs(mov.x) < 0.1f && Mathf.Abs(mov.z) < 0.1f)
        {
            GetComponent<Animator>().SetBool("moving", false);

        }
        else {
            GetComponent<Animator>().SetBool("moving", true);
        }
            

        rb.MovePosition(transform.position + mov * Time.deltaTime * usedSpeed);
    }

    public void MouseSlowDown() {
        usedSpeed = slowed;
    }

    public void MouseSpeedUp() {
        usedSpeed = speed;
    }

    public void FooFooSlowDownUp()
    {
        usedSpeed = speed;
    }

    public void FooFooSpeedUp()
    {
        usedSpeed += 1.5f;
    }

    
}
