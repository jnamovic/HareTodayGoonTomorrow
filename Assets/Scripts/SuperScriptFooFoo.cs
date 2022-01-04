using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperScriptFooFoo : MonoBehaviour
{
    public Material m;

    public float fastSpeed = 10.0f;
    public float slowSpeed = 5.0f;

    public float multiplier = 3.0f;

    private float maxWaitTime = 6.5f;
    private float waitTimer;

    private float maxSwingWaitTime = 1.0f;
    private float swingWaitTimeCounter;

    private bool taken;

    // Start is called before the first frame update
    void Start()
    {
        Transform player = transform.GetChild(1);

        player.GetChild(1).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(2).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(3).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(4).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(5).GetChild(0).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(5).GetChild(1).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(5).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(6).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(7).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(8).GetChild(0).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(8).GetChild(1).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(8).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
        player.GetChild(9).gameObject.GetComponent<Renderer>().material = m;
    }

    // Update is called once per frame
    void Update()
    {
        if (IntroManager.S.first)
        {
            IntroManager.S.foofoo = true;
        }
        else if (IntroManager.S.second)
        {
            // transform.eulerAngles = new Vector3(0, 90, 0);
            if (transform.localPosition.x < -150)
            {
                Move(slowSpeed);
            }
            else
            {
                GetComponent<Animator>().SetBool("moving", false);
                //transform.localPosition = new Vector3(transform.localPosition.x, -30, -200);
                if (taken)
                    GetComponent<Animator>().SetBool("attacking", false);
                else
                    GetComponent<Animator>().SetBool("attacking", true);

                if (swingWaitTimeCounter < maxSwingWaitTime)
                {
                    swingWaitTimeCounter += Time.deltaTime;
                    if (swingWaitTimeCounter >= maxSwingWaitTime / 3)
                    {
                        taken = true;
                        IntroManager.S.c.transform.GetChild(6).gameObject.SetActive(false);
                        IntroManager.S.c.transform.GetChild(7).gameObject.SetActive(true);
                        IntroManager.S.c.transform.GetChild(8).gameObject.SetActive(true);
                        IntroManager.S.c.transform.GetChild(9).gameObject.SetActive(true);
                    }
                }
                else
                {
                    //transform.localPosition = new Vector3(transform.localPosition.x, -30, -266);
                    if (transform.localPosition.x < 300)
                    {
                        GetComponent<Animator>().SetBool("attacking", false);
                        Move(slowSpeed);
                    }
                    else {
                        IntroManager.S.foofoo = true;
                    }
                }
            }
        }
        else if (IntroManager.S.third)
        {
            // transform.localPosition = new Vector3(transform.localPosition.x, -30, -266);
            GetComponent<Animator>().SetBool("moving", true);
            GetComponent<Animator>().SetBool("attacking", false);
            if (transform.eulerAngles.y > 0 && transform.eulerAngles.y <= 261)
                transform.Rotate(0, 10, 0);
            else
                IntroManager.S.foofoo = true;
            /*
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, -120, -266);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
                IntroManager.S.foofoo = true;
            }*/
        }
        else if (IntroManager.S.fourth)
        {
            // transform.localPosition = new Vector3(transform.localPosition.x, -120, -266);
            /*print("Q");
            if (transform.eulerAngles.y > 0)
            {
                if (transform.eulerAngles.y > 0)
                    transform.Rotate(0, -10, 0);
                else
                    IntroManager.S.foofoo = true;
            }*/
            //else
            Move(slowSpeed);
            if (transform.localPosition.x <= -IntroManager.S.halfWidth)
                IntroManager.S.foofoo = true;
        }
        else if (IntroManager.S.fifth)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
            if (waitTimer < maxWaitTime)
                waitTimer += Time.deltaTime;
            else
            {
                //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -266);
                Move(fastSpeed);
                if (transform.localPosition.x >= IntroManager.S.halfWidth + 40)
                    IntroManager.S.foofoo = true;
            }
        }
    }

    public void Move(float speed)
    {
        //transform.localPosition = new Vector3(transform.localPosition.x, -120, -266);
        GetComponent<Animator>().SetBool("moving", true);
        GetComponent<Animator>().SetBool("attacking", false);
        transform.Translate(Vector3.forward * multiplier * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
    }
}
