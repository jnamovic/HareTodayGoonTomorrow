using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

public class IntroManager : MonoBehaviour
{
    public static IntroManager S;

    public bool first;
    public bool second;
    public bool third;
    public bool fourth;
    public bool fifth;

    private bool firstTimer;
    private bool secondTimer;
    private bool thirdTimer;
    private bool fourthTimer;
    private bool fifthTimer;

    public bool mouse1;
    public bool mouse2;
    public bool mouse3;
    public bool foofoo;

    public Canvas c;

    public float halfWidth;

    // Start is called before the first frame update
    void Awake()
    {
        S = this;

        first = true;

        halfWidth = c.GetComponent<RectTransform>().rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        //if (XCI.GetButtonDown(XboxButton.Start))
        //    SceneManager.LoadScene("Character Selection");

        if (mouse1 && mouse2 && mouse3 && foofoo)
        {
            if (first)
            {
                first = false;
                second = true;
            } else if (second)
            {
                second = false;
                third = true;
            } else if (third)
            {
                third = false;
                fourth = true;
            } else if (fourth)
            {
                fourth = false;
                fifth = true;
            } else if (fifth)
            {
                SceneManager.LoadScene("Start");
            }

            mouse1 = false;
            mouse2 = false;
            mouse3 = false;
            foofoo = false;
        }

        if (XCI.GetButton(XboxButton.Start))
        {
            SceneManager.LoadScene("Start");
        }
    }
}
