using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooFooControlScript : MonoBehaviour
{
    public int hp = 15;
    private int baseHP;

    public float maxHurtTime = 0.75f;
    private float hurtTime;

    private bool hurt;
    public bool stunned;

    private int pieceCount = 1;

    public GameObject piece1;
    public GameObject piece2;
    public GameObject piece3;

    public int xzForce = 1;
    public int yForce = 2;

    public Material baseMaterial;
    public Material red;

    void Awake()
    {
        baseHP = hp;
    }

    // Start is called before the first frame update
    void Start()
    {
        baseHP = hp;

        baseMaterial = transform.GetChild(1).GetChild(1).GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (hurt)
        {
            if (hurtTime >= maxHurtTime)
            {
                //GetComponent<Renderer>().material = baseMaterial;
                GameManager.S.UpdatePlayerColor(gameObject, baseMaterial);
                hurtTime = 0.0f;
                hurt = false;
            }
            else
            {
                //GetComponent<Renderer>().material = red;
                GameManager.S.UpdatePlayerColor(gameObject, red);
                hurtTime += Time.deltaTime;
            }
        }
    }

    private void DropPiece(int pieceNum)
    {
        Vector3 dropPosition = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);

        System.Random xzDist = new System.Random();

        Vector3 randomForce = new Vector3(xzDist.Next(0, xzForce), yForce, xzDist.Next(0, xzForce));


        GameObject DropPiece;


        switch (pieceNum)
        {
            case 1:
                // ADDED
                GameManager.S.UpdatePieces(piece1, true);
                DropPiece = Instantiate(piece1);
                break;
            case 2:
                // ADDED
                GameManager.S.UpdatePieces(piece2, true);
                DropPiece = Instantiate(piece2);
                break;
            default:
                // ADDED
                GameManager.S.UpdatePieces(piece3, true);
                DropPiece = Instantiate(piece3);
                break;
        }

        GameManager.S.PieceInPlay(DropPiece);
        SoundManager.S.MakeFooFooDropWalkieSound();
        DropPiece.transform.position = dropPosition;
        DropPiece.GetComponent<Rigidbody>().AddForce(randomForce);

        // REMOVED
        // GetComponent<BasePlayerMovementScript>().FooFooSpeedUp();
    }

    public void LoseHP()
    {
        if (hp != 0)
        {
            hp--;
            hurt = true;
            if (hp % 3 == 0)
            {
                DropPiece(pieceCount);
                pieceCount++;
                //GameManager.S.SetNumPiecesAssembled(1);
                //print(GameManager.S.GetNumPiecesAssembled());
            }
        }
        else
        {
            StartCoroutine(stun());
        }
    }

    public void Reset()
    {
        if (baseHP != 0)
            hp = baseHP;
        pieceCount = 1;
        GetComponent<BasePlayerMovementScript>().FooFooSlowDownUp();
    }

    IEnumerator stun()
    {
        stunned = true;
        yield return new WaitForSeconds(.05f);
        stunned = false;
    }
}
