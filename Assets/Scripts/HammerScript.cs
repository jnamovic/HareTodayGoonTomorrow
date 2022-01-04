using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && other.gameObject.tag == "Mouse")
        {
            other.gameObject.GetComponent<MouseScript>().KO();
            // ADDED
            SoundManager.S.MakeHammerHitSound();
        }

        // ADDED
        // Used to destroy non-wall trees, but this logic needs refining once we merge Austin's stuff
        
        else if ( other.gameObject.tag == "Hammerable" || other.gameObject.tag == "Rock Pile" || (!other.isTrigger && other.gameObject.tag == "Untagged" && other.transform.parent.transform.parent.transform.parent == null))
        {
            other.gameObject.SetActive(false);
            // ADDED
            SoundManager.S.MakeHammerHitSound();
        }

        else if (!other.isTrigger && other.gameObject.tag == "Rock Pile")
        {
            GameManager.S.RemoveRock(other.gameObject);
            Destroy(other.gameObject);
            
            // ADDED
            SoundManager.S.MakeHammerHitSound();
        }

    }
}
