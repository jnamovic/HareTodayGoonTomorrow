using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSelection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            
                GameManager.S.firstFrame = true;
                GameManager.S.started = false;
                GameManager.S.playing = false;
            Debug.Log("i am going back to character select");
            
                
            SceneManager.LoadScene("Character Selection");
        }
    }
}
