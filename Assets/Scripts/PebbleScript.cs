using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PebbleScript : MonoBehaviour
{
    public string mouseName;
    public float forceBackSpeed = 50.0f;
    // ADDED
    public int matIndex;

    public Material[] lineMaterials;

    // Start is called before the first frame update
    void Start()
    {
        
        GetComponent<TrailRenderer>().material = lineMaterials[matIndex];
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == mouseName)
            Physics.IgnoreCollision(other, GetComponent<Collider>());
        else
        {
            if (other.gameObject.tag == "Mouse")
                other.GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity.normalized * forceBackSpeed);
            else if (other.gameObject.tag == "foofoo")
                other.GetComponent<FooFooControlScript>().LoseHP();
            else
                // ADDED
                SoundManager.S.MakeMouseThrowFailSound();
            Destroy(this.gameObject);
        }
    }
}