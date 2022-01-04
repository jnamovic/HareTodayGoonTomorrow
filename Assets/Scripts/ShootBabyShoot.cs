using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class ShootBabyShoot : MonoBehaviour
{
    public float timeToWaitBeforeFiring = 1.5f;
    public float timeToWaitBeforeMovingAgain = 1.5f;

    private float counter;

    private bool firing;
    private bool fired;

    public int rocks = 5;
    public int matIndex;

    private BasePlayerMovementScript b;

    private float oldSpeed;
    private float oldRotationSpeed;

    private XboxController playerNumber;

    private bool attackButtonDown;

    public GameObject pebble;

    public float attackSpeed = 5.0f;

    //private Quaternion baseRotation;

    // Start is called before the first frame update
    void Start()
    {
        b = GetComponent<BasePlayerMovementScript>();

        oldSpeed = b.speed;
        oldRotationSpeed = b.rotationSpeed;
        playerNumber = b.playerNumber;
        //baseRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // ADDED
        if (rocks > 0)
            GetComponent<Animator>().SetBool("attacking", attackButtonDown && (fired || firing));

        // Input
        if (b.playerNumber != XboxController.All && b.playerNumber != XboxController.Any)
        {
            attackButtonDown = XCI.GetButton(XboxButton.B, b.playerNumber);
        }
        else
        {
            // DO NOT USE, ONLY USE XBOX
            attackButtonDown = Input.GetButton("Jump");
        }

        //Logic
        if (attackButtonDown && !firing && rocks > 0)
        {
            //baseRotation = transform.rotation;
            //transform.Rotate(new Vector3(-0.5f, 0.0f, 0.0f));

            /*
            b.speed = 0;
            if (counter >= timeToWaitBeforeFiring)
            {
                firing = true;
            }
            else
            {
                counter += Time.deltaTime;
            }
            */
            firing = true;

            SoundManager.S.MakeMouseThrowSuccessSound();
        }
        else if (!attackButtonDown && firing)
        {
            Vector3 spawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z) ;

            GameObject g = Instantiate(pebble, spawnPosition, transform.rotation);
            g.GetComponent<PebbleScript>().matIndex = matIndex;
            g.GetComponent<PebbleScript>().mouseName = name;
            Vector3 launchPos = transform.forward + new Vector3(0, 0.3f, 0);
            g.GetComponent<Rigidbody>().AddForce(launchPos * attackSpeed);
            counter = 0;
            rocks--;
            fired = true;
            firing = false;

            // ADDED
            GameManager.S.UpdateRocksOfPlayer(b.playerNumber, rocks);
        }
        else if (fired)
        {
            firing = false;
            fired = false;

            /*
            b.rotationSpeed = 0;
            if (counter >= timeToWaitBeforeMovingAgain)
            {
                firing = false;
                counter = 0;
                b.speed = oldSpeed;
                b.rotationSpeed = oldRotationSpeed;
                fired = false;
                //transform.rotation = baseRotation;
            }
            else
            {
                counter += Time.deltaTime;
            }
            */
        }
        else if (!attackButtonDown && !firing)
        {
            //transform.rotation = baseRotation;
            fired = false;
            firing = false;
            counter = 0;
            b.speed = oldSpeed;
            b.rotationSpeed = oldRotationSpeed;
        }
    }

    public void RefillRocks()
    {
        // ADDED/CHANGED
        if (rocks < 5)
            rocks += 3;

        if (rocks > 5)
            rocks = 5;

        // ADDED
        GameManager.S.UpdateRocksOfPlayer(playerNumber, rocks);
    }
}
