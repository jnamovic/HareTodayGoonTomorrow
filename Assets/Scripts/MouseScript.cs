using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class MouseScript : MonoBehaviour
{
    public bool kod;

    public Material deadMaterial;
    public Material reviveMaterial;

    private Material baseMaterial;

    private ShootBabyShoot shootScript;
    // ADDED
    private BasePlayerMovementScript baseScript;

    public int numAPressesRequired = 5;
    public int numAPressesGiven;

    private bool healedThisFrame;

    // ADDED
    private bool onePlayerIn;
    private bool twoPlayerIn;
    private bool checkedForMe;

    private GameObject carry;

    public int xzForce = 2000;
    public int yForce = 4000;

    public int throwForce = 100;

    // Start is called before the first frame update
    void Start()
    {
        baseMaterial = transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material;
        shootScript = GetComponent<ShootBabyShoot>();
        //ADDED
        baseScript = GetComponent<BasePlayerMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (kod)
            GameManager.S.UpdatePlayerColor(gameObject, deadMaterial);
        else
            GameManager.S.UpdatePlayerColor(gameObject, baseMaterial);

        if (XCI.GetButton(XboxButton.X, baseScript.playerNumber))
        {
            Drop(true);
        }

        // CHANGED
        GetComponent<Animator>().SetBool("alive", !kod);
    }

    // EDITED
    private void OnTriggerStay(Collider other)
    {
        // Assuming this is covered in the hammer script
        if (!kod &&
            !other.isTrigger &&
            other.gameObject.tag == "Mouse" &&
            other.gameObject.GetComponent<MouseScript>().kod)
        {
            // CHANGED
            //if (XCI.GetButtonDown(XboxButton.A, baseScript.playerNumber))
            if (XCI.GetButton(XboxButton.A, baseScript.playerNumber))
            {
                // ADDED
                if (other.gameObject.GetComponent<MouseScript>().onePlayerIn && !checkedForMe)
                {
                    other.gameObject.GetComponent<MouseScript>().twoPlayerIn = true;
                    checkedForMe = true;
                }
                else if (!other.gameObject.GetComponent<MouseScript>().onePlayerIn && !checkedForMe)
                {
                    other.gameObject.GetComponent<MouseScript>().onePlayerIn = true;
                    checkedForMe = true;
                }

                other.gameObject.GetComponent<MouseScript>().Help();
            }
            else
            {
                if (!other.gameObject.GetComponent<MouseScript>().twoPlayerIn && checkedForMe)
                {
                    other.gameObject.GetComponent<MouseScript>().numAPressesGiven = 0;
                    other.gameObject.GetComponent<MouseScript>().onePlayerIn = false;
                    checkedForMe = false;
                }
                else if (other.gameObject.GetComponent<MouseScript>().twoPlayerIn && checkedForMe)
                {
                    other.gameObject.GetComponent<MouseScript>().twoPlayerIn = false;
                    checkedForMe = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Piece")
        {
            //GameManager.S.SetNumPiecesAssembled(1);

            PickUp(collision.transform.gameObject);

        }

        if (collision.transform.tag == "Rock Pile")
        {
            shootScript.RefillRocks();
            GameManager.S.RemoveRock(collision.gameObject);
            Destroy(collision.gameObject);
            SoundManager.S.MakeMousePickUpAmmoSound();
        }
        if (collision.transform.tag == "Bonus")
        {
            Deposit();

        }

    }

    private void OnTriggerExit(Collider other)
    {
        // ADDED
        if (!kod &&
            !other.isTrigger &&
            other.gameObject.tag == "Mouse" &&
            other.gameObject.GetComponent<MouseScript>().kod)
        {
            if (!other.gameObject.GetComponent<MouseScript>().twoPlayerIn && checkedForMe)
            {
                other.gameObject.GetComponent<MouseScript>().numAPressesGiven = 0;
                other.gameObject.GetComponent<MouseScript>().onePlayerIn = false;
                checkedForMe = false;
            }
            else if (other.gameObject.GetComponent<MouseScript>().twoPlayerIn && checkedForMe)
            {
                other.gameObject.GetComponent<MouseScript>().twoPlayerIn = false;
                checkedForMe = false;
            }
        }

        if ((!other.isTrigger && other.gameObject.tag == "Mouse" && other.gameObject.GetComponent<MouseScript>().kod))
            GameManager.S.UpdatePlayerColor(gameObject, deadMaterial);
    }

    private void Help()
    {
        numAPressesGiven++;
        print(numAPressesGiven);
        if (numAPressesGiven >= numAPressesRequired)
        {
            // CHANGED
            // numAPressesRequired = (int) Mathf.Round(1.5f * numAPressesRequired);
            numAPressesGiven = 0;
            Revive();
            GameManager.S.players.Add(this.gameObject);
            GameManager.S.UpdateTargets();
        }
    }

    public void KO()
    {
        if (!kod)
            GameManager.S.SetNumKOd(1);
        kod = true;
        GetComponent<BasePlayerMovementScript>().enabled = false;
        GetComponent<ShootBabyShoot>().enabled = false;
        GameManager.S.players.Remove(this.gameObject);
        GameManager.S.UpdateTargets();

        GameManager.S.UpdatePlayerStatus(baseScript.playerNumber, true);

        Drop(false);
        // Too long
        //SoundManager.S.MakeMouseDeathSound();
    }

    public void Revive()
    {
        if (GameManager.S.GetNumKOd() > 0)
            GameManager.S.SetNumKOd(-1);
        kod = false;
        GetComponent<BasePlayerMovementScript>().enabled = true;
        GetComponent<ShootBabyShoot>().enabled = true;

        if (!GameManager.S.firstFrame)
        {
            GetComponent<ShootBabyShoot>().RefillRocks();
            GameManager.S.UpdatePlayerStatus(baseScript.playerNumber, false);
        }

    }

    private void PickUp(GameObject picked)
    {
        if (carry == null && !kod)
        {
            picked.transform.SetParent(this.transform);
            picked.GetComponent<Rigidbody>().isKinematic = true;
            picked.GetComponent<Collider>().enabled = false;
            picked.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .5f, this.transform.position.z);
            baseScript.MouseSlowDown();
            carry = picked;
            SoundManager.S.MakeMousePickUpWalkieSound();
        }
    }
    private void Drop(bool itJustWorks)
    {

        if (carry != null)
        {
            Vector3 dropPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

            System.Random xzDist = new System.Random();

            // CHANGED
            Vector3 randomForce = new Vector3(xzDist.Next(-400, 400), 200, xzDist.Next(-400, 400));

            carry.transform.SetParent(null);
            carry.transform.position = dropPosition;
            carry.GetComponent<Rigidbody>().isKinematic = false;

            carry.GetComponent<Collider>().enabled = true;


            // CHANGED
            if (itJustWorks)
                carry.GetComponent<Rigidbody>().AddForce(Vector3.up * throwForce);
            else
                carry.GetComponent<Rigidbody>().AddForce(randomForce);

            baseScript.MouseSpeedUp();
            carry = null;
            SoundManager.S.MakeMiceDropWalkieSound();
        }
    }

    private void Deposit()
    {

        if (carry != null)
        {
            GameManager.S.AddBonus(carry);
            baseScript.MouseSpeedUp();
            carry = null;
            SoundManager.S.MakePlaceWalkieOnAlterSound();
        }
    }
}