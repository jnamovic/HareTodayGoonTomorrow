using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperScriptMouse : MonoBehaviour
{
    public Material m;

    public float fastSpeed = 10.0f;
    public float slowSpeed = 5.0f;

    public float multiplier = 3.0f;

    private float yPos;
    private int jumps;
    public int totalJumps = 2;
    private float maxJumpTime = 0.35f;
    private float jumpTimeCounter;
    private float jumpIncrease = 3.0f;
    public float delayTime;
    private float delayTimeCounter;

    public float animationDelayTime;
    private float animationDelayTimeCounter;

    private bool rotate1;
    private bool rotate2;

    private float maxHoldTime = 1.0f;
    private float holdTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        yPos = transform.localPosition.y;

        Transform player = transform.GetChild(1);

        player.GetChild(0).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(1).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(2).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(3).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(4).GetChild(0).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(4).GetChild(1).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(4).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(6).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(7).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(8).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(9).GetChild(0).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(9).GetChild(1).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(9).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(11).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(12).gameObject.GetComponent<Renderer>().material = m;
    }

    // Update is called once per frame
    void Update()
    {
        if (animationDelayTimeCounter < animationDelayTime)
        {
            GetComponent<Animator>().Rebind();
            animationDelayTimeCounter += Time.deltaTime;
        }
            
        if (IntroManager.S.first)
        {
            if (jumps < totalJumps) {
                Jump();
            }
            else
            {
                UpdateGameManagerBool();
            }
        }
        else if (IntroManager.S.second)
        {
            if (rotate1)
            {
                if (transform.eulerAngles.x < 340 || (transform.eulerAngles.x > 340 && transform.eulerAngles.x < 360))
                    transform.Rotate(new Vector3(-5, 0, 0));
                else
                {
                    transform.eulerAngles = new Vector3(350, transform.eulerAngles.y, transform.eulerAngles.z);
                    rotate2 = true;
                    rotate1 = false;
                }    
            }
            else if (rotate2)
            {
                if (holdTimeCounter < maxHoldTime)
                {
                    holdTimeCounter += Time.deltaTime;
                }
                else
                {
                    if (transform.eulerAngles.x > 345 && transform.eulerAngles.x < 360)
                        transform.Rotate(new Vector3(5, 20, 0));
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 90, 0);
                        Move(fastSpeed);
                        if (transform.localPosition.x >= IntroManager.S.halfWidth)
                        {
                            UpdateGameManagerBool();
                        }
                    }
                }
            }
            else
            {
                if (name == "Mouse2")
                {
                    if (transform.eulerAngles.y < 240.0f)
                    {
                        transform.Rotate(new Vector3(0, 10, 0));
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 240, 0);
                        rotate1 = true;
                    }
                }
                else
                {
                    if (transform.eulerAngles.y < 270.0f)
                    {
                        transform.Rotate(new Vector3(0, 10, 0));
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 270, 0);
                        rotate1 = true;
                    }
                }
            }
        }
        else if (IntroManager.S.third)
        {
            UpdateGameManagerBool();
        }
        else if (IntroManager.S.fourth)
        {
            transform.eulerAngles = new Vector3(0, 270, 0);
            Move(fastSpeed);
            if (transform.localPosition.x <= -IntroManager.S.halfWidth - 40)
                UpdateGameManagerBool();
        }
        else if (IntroManager.S.fifth)
        {
            transform.GetChild(2).gameObject.SetActive(true);
            transform.eulerAngles = new Vector3(0, 90, 0);
            Move(slowSpeed);
            if (transform.localPosition.x >= IntroManager.S.halfWidth)
                UpdateGameManagerBool();
        }
    }

    public void Move(float speed)
    {
        GetComponent<Animator>().SetBool("moving", true);
        transform.Translate(Vector3.forward * multiplier * speed * Time.deltaTime);
    }

    private void UpdateGameManagerBool()
    {
        if (name == "Mouse1")
            IntroManager.S.mouse1 = true;
        else if (name == "Mouse2")
            IntroManager.S.mouse2 = true;
        else if (name == "Mouse3")
            IntroManager.S.mouse3 = true;
    }

    private void Jump()
    {
        if (delayTimeCounter >= delayTime)
        {
            if (jumpTimeCounter < maxJumpTime / 2)
            {
                jumpTimeCounter += Time.deltaTime;
                transform.localPosition += new Vector3(0.0f, jumpIncrease, 0.0f);
            }
            else if (jumpTimeCounter >= maxJumpTime / 2 && jumpTimeCounter < maxJumpTime)
            {
                jumpTimeCounter += Time.deltaTime;
                transform.localPosition -= new Vector3(0.0f, jumpIncrease, 0.0f);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, yPos, transform.localPosition.z);
                jumps += 1;
                jumpTimeCounter = 0.0f;
                delayTimeCounter = 0.0f;
            }
        }
        else
        {
            delayTimeCounter += Time.deltaTime;
        }
    }
}
