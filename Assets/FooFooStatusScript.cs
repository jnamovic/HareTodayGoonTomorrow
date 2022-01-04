using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooFooStatusScript : MonoBehaviour
{
    private bool move;
    private bool pause;

    private float waitTime = 0.3f;
    private float waitTimeCounter;

    private float waitTime2 = 0.5f;
    private float waitTimeCounter2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x < -470)
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            GetComponent<Animator>().SetBool("moving", true);
        }
        else
        {
            if (waitTimeCounter < waitTime)
            {
                GetComponent<Animator>().SetBool("moving", false);
                GetComponent<Animator>().SetBool("attacking", true);
                waitTimeCounter += Time.deltaTime;
            }
            else
            {
                GetComponent<Animator>().SetBool("attacking", false);
                if (waitTimeCounter2 < waitTime2)
                {
                    waitTimeCounter2 += Time.deltaTime;
                }
                else {
                    transform.Translate(Vector3.forward * 5 * Time.deltaTime);
                    GetComponent<Animator>().SetBool("moving", true);
                }
            }
        }
    }
}
