using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class FooFooScript : MonoBehaviour
{
    public GameObject hammer;

    // EDITED
    private XboxController playerNumber;
    public int speed = 500;

    public float coolDownTime = 1;

    private bool swinging = false;

    private bool driftable = true;

    // ADDED
    private float timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        // ADDED
        playerNumber = GetComponent<BasePlayerMovementScript>().playerNumber;
        hammer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // ADDED
        GetComponent<Animator>().SetBool("attacking", hammer.activeInHierarchy);

        if (swinging||transform.GetComponent<FooFooControlScript>().stunned)
        {
            GetComponent<BasePlayerMovementScript>().enabled = false;
        }
        else
        {
            GetComponent<BasePlayerMovementScript>().enabled = true;
        }

        if (playerNumber != XboxController.All && playerNumber != XboxController.Any)
        {
            //XCI.GetButtonDown(XboxButton.A, playerNumber);

            //}
            //else
            //{
            if (hammer.transform.localPosition.z == 0.6f)
            {
                swinging = false;
                hammer.SetActive(false);
            }

            //do the thing
            if (XCI.GetButtonDown(XboxButton.B, playerNumber) && !swinging)
            {
                // ADDED
                SoundManager.S.MakeHammerSwingSound();
                swinging = true;
                hammer.SetActive(true);
            }
            if (XCI.GetButtonDown(XboxButton.A, playerNumber) && !swinging && driftable)
            {
                Vector3 dash = transform.forward * speed;
                transform.GetComponent<Rigidbody>().AddForce(dash);
                driftable = false;
                StartCoroutine(cooldown());

                // ADDED
                GameManager.S.UpdateDash(0);
            }


        }

        // ADDED
        if (driftable)
        {
            timeElapsed = 0;
        }
        else
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > coolDownTime)
                timeElapsed = coolDownTime;
            GameManager.S.UpdateDash(timeElapsed);
        }
    }
    IEnumerator cooldown()
    {
        yield return new WaitForSeconds(coolDownTime);
        driftable = true;
    }

    public void Reset()
    {
        hammer.SetActive(false);
        StopCoroutine(cooldown());
        driftable = true;
        swinging = false;
    }


}

