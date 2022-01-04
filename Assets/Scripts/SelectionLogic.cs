using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ADDED
using TMPro;
using UnityEngine.SceneManagement;
using System;
using XboxCtrlrInput;

public class SelectionLogic : MonoBehaviour
{
    public static SelectionLogic S;

    public Material[] possibleMaterials;
    public Canvas c;

    public int[] playerStates = new int[] { 1, 1, 1, 1 };
    private int[] playerColumns = new int[] { 0, 1, 2, 3 };
    private int[] playerColorInts = new int[4];

    private float moveTimer = 0;

    private readonly int MAX_STATE = 3;

    public float secondsToWait = 2.0f;
    private float counter;

    public bool skip;

    private bool left;
    private bool right;
    private bool up;
    private bool down;
    private bool triggeredX;
    private bool triggeredY;

    void Awake()
    {
        S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        System.Random r = new System.Random();

        playerColorInts[0] = r.Next(0, 12);
        UpdatePlayerColor(0, playerColorInts[0]);

        do
        {
            playerColorInts[1] = r.Next(0, 12);
        } while (playerColorInts[1] == playerColorInts[0]);
        UpdatePlayerColor(1, playerColorInts[1]);

        do
        {
            playerColorInts[2] = r.Next(0, 12);
        } while (playerColorInts[2] == playerColorInts[0] || playerColorInts[2] == playerColorInts[1]);
        UpdatePlayerColor(2, playerColorInts[2]);

        do
        {
            playerColorInts[3] = r.Next(0, 12);
        } while (playerColorInts[3] == playerColorInts[0] || playerColorInts[3] == playerColorInts[1] || playerColorInts[3] == playerColorInts[2]);
        UpdatePlayerColor(3, playerColorInts[3]);

        for (int i = 0; i < 4; i++)
        {
            c.transform.GetChild(playerColumns[i]).GetChild(2).GetChild(playerColorInts[i]).GetComponent<Outline>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (skip)
        {
            GameManager.S.SetPlayerValues();
            GameManager.S.playing = true;
            SceneManager.LoadScene("Arena 1");
        }

        moveTimer += Time.deltaTime;

        // CHANGED
        if ((playerStates[0] == MAX_STATE && playerStates[1] == MAX_STATE &&
            playerStates[2] == MAX_STATE && playerStates[3] == MAX_STATE) ||
            Input.GetKey(KeyCode.Return))
        {
            if (counter >= secondsToWait)
            {
                GameManager.S.SetPlayerValues();
                GameManager.S.playing = true;
                print(GameManager.S.playing);
                SceneManager.LoadScene("Arena 1");
            }
            else
            {
                counter++;
            }
        }
        else
        {
            counter = 0.0f;
            if (moveTimer > .1)
            {
                CheckPlayerInput(XboxController.First);
                CheckPlayerInput(XboxController.Second);
                CheckPlayerInput(XboxController.Third);
                CheckPlayerInput(XboxController.Fourth);
                moveTimer = 0;
            }

        }
    }

    private void CheckPlayerInput(XboxController x)
    {
        print("reached");

        int xInt = (int)x - 1;

        if (XCI.GetAxis(XboxAxis.LeftStickX, x) < -0.1f)
        {
            left = true;
            right = false;

        }
        else if (XCI.GetAxis(XboxAxis.LeftStickX, x) > 0.1f)
        {
            left = false;
            right = true;

        }
        else
        {
            left = false;
            right = false;
            triggeredX = false;
        }

        if (XCI.GetAxis(XboxAxis.LeftStickY, x) < -0.1f)
        {
            down = true;
            up = false;
        }
        else if (XCI.GetAxis(XboxAxis.LeftStickY, x) > 0.1f)
        {
            down = false;
            up = true;
        }
        else
        {
            down = false;
            up = false;
            triggeredY = false;
        }

        if (XCI.GetButtonDown(XboxButton.A, x) && playerStates[xInt] < MAX_STATE)
        {
            if (checkColumns(xInt))
            {
                // print(playerColumns[0] + " " + playerColumns[1] + " " + playerColumns[2] + " " + playerColumns[3]);
                //c.transform.GetChild(playerColumns[xInt]).GetChild(0).GetChild(xInt).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.green;
                //c.transform.GetChild(playerColumns[xInt]).GetChild(0).GetChild(xInt).GetChild(0).GetComponent<Outline>().effectColor = Color.white;

                if (playerStates[xInt] == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (xInt != i)
                        {
                            // CHANGED
                            //if (playerStates[i] > 1 && playerColorInts[i] == playerColorInts[xInt])
                            if (playerStates[i] > 1 && playerColorInts[playerColumns[i]] == playerColorInts[playerColumns[xInt]])
                            {
                                return;
                            }
                        }
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        print(playerColorInts[playerColumns[i]]);
                    }
                }

                playerStates[xInt]++;
                SwitchReady(xInt, true);
            }
        }
        // CHANGED
        else if (XCI.GetButtonDown(XboxButton.B, x) && playerStates[xInt] > 1)
        {
            /*if (playerStates[xInt] == 1)
            {
                c.transform.GetChild(playerColumns[xInt]).GetChild(0).GetChild(xInt).GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
                c.transform.GetChild(playerColumns[xInt]).GetChild(0).GetChild(xInt).GetChild(0).GetComponent<Outline>().effectColor = Color.black;
            }*/
            playerStates[xInt]--;
            SwitchReady(xInt, false);
        }
        // ADDED
        else if (XCI.GetButtonDown(XboxButton.Start))
        {
            playerStates[xInt] = MAX_STATE;
        }

        if ((!triggeredX && left) || XCI.GetDPadDown(XboxDPad.Left, x))
        {
            if (playerStates[xInt] == 0)
                SwitchColumn(xInt, -1);
            else if (playerStates[xInt] == 1)
                SwitchColor(playerColumns[xInt], -1, true);

            triggeredX = true;
        }
        else if ((!triggeredX && right) || XCI.GetDPadDown(XboxDPad.Right, x))
        {
            if (playerStates[xInt] == 0)
                SwitchColumn(xInt, 1);
            else if (playerStates[xInt] == 1)
                SwitchColor(playerColumns[xInt], 1, true);

            triggeredX = true;
        }
        if ((!triggeredY && down) || XCI.GetDPadDown(XboxDPad.Down, x))
        {
            if (playerStates[xInt] == 1)
                SwitchColor(playerColumns[xInt], -1, false);

            triggeredY = true;
        }
        else if ((!triggeredY && up) || XCI.GetDPadDown(XboxDPad.Up, x))
        {
            if (playerStates[xInt] == 1)
                SwitchColor(playerColumns[xInt], 1, false);

            triggeredY = true;
        }
    }

    private void SwitchColumn(int x, int change)
    {
        c.transform.GetChild(playerColumns[x]).GetChild(0).GetChild(x).gameObject.SetActive(false);

        if (playerColumns[x] == 0 && change == -1)
        {
            playerColumns[x] = 3;
        }
        else if (playerColumns[x] == 3 && change == 1)
        {
            playerColumns[x] = 0;
        }
        else
        {
            playerColumns[x] += change;
        }

        c.transform.GetChild(playerColumns[x]).GetChild(0).GetChild(x).gameObject.SetActive(true);
    }

    private void SwitchColor(int x, int change, bool xDir)
    {
        SoundManager.S.MakeUIMoveSound();
        c.transform.GetChild(x).GetChild(2).GetChild(playerColorInts[x]).GetComponent<Outline>().enabled = false;

        if (xDir)
        {
            if (playerColorInts[x] % 6 == 5 && change == 1)
            {
                if (playerColorInts[x] == 5)
                    playerColorInts[x] = 0;
                else
                    playerColorInts[x] = 6;
            }
            else if (playerColorInts[x] % 6 == 0 && change == -1)
            {
                if (playerColorInts[x] == 0)
                    playerColorInts[x] = 5;
                else
                    playerColorInts[x] = 11;
            }
            else
            {
                playerColorInts[x] += change;
            }
        }
        else
        {
            if (playerColorInts[x] > 5 && change == 1)
            {
                playerColorInts[x] -= 6;
            }
            else if (playerColorInts[x] < 6 && change == -1)
            {
                playerColorInts[x] += 6;
            }
            else
            {
                playerColorInts[x] += (change * 6);
            }
        }

        c.transform.GetChild(x).GetChild(2).GetChild(playerColorInts[x]).GetComponent<Outline>().enabled = true;
        UpdatePlayerColor(x, playerColorInts[x]);
    }

    private void SwitchReady(int x, bool ready)
    {
        if (playerStates[x] == 2 && ready)
            c.transform.GetChild(playerColumns[x]).GetChild(3).GetComponent<Image>().color = Color.yellow;
        else if (playerStates[x] == 2 && !ready)
            c.transform.GetChild(playerColumns[x]).GetChild(3).GetComponent<Image>().color = Color.yellow;
        else if (playerStates[x] == 3)
            c.transform.GetChild(playerColumns[x]).GetChild(3).GetComponent<Image>().color = Color.green;
        else
            c.transform.GetChild(playerColumns[x]).GetChild(3).GetComponent<Image>().color = Color.white;
        SoundManager.S.MakeUISelectSound();
    }

    private bool checkColumns(int player)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i != player)
            {
                if (playerColumns[i] == playerColumns[player] && playerStates[i] > 0)
                    return false;
            }
        }
        return true;
    }

    // CHANGED
    private void UpdatePlayerColor(int i, int j)
    {
        // c.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Renderer>().material = possibleMaterials[j];

        Transform player = c.transform.GetChild(i).GetChild(1).GetChild(0).GetChild(1);

        // Foo Foo
        if (i == 3)
        {
            player.GetChild(1).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(2).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(3).GetChild(2).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(4).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(5).GetChild(0).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(5).GetChild(1).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(5).GetChild(2).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(6).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(7).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(8).GetChild(0).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(8).GetChild(1).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(8).GetChild(2).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(9).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
        }
        // Mouse
        else
        {
            player.GetChild(0).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(1).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(2).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(3).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(4).GetChild(0).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(4).GetChild(1).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(4).GetChild(2).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(6).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(7).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(8).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(9).GetChild(0).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(9).GetChild(1).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(9).GetChild(2).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(11).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
            player.GetChild(12).gameObject.GetComponent<Renderer>().material = possibleMaterials[j];
        }
    }

    public int[] GivePlayerValues()
    {
        return playerColumns;
    }

    public Material[] GiveMaterialValues()
    {
        Material[] playerMaterials = new Material[4];

        for (int i = 0; i < 4; i++)
        {
            playerMaterials[i] = possibleMaterials[playerColorInts[playerColumns[i]]];
        }

        return playerMaterials;
    }

    public int[] GivePlayerColorInts() {
        int[] pColorInts = new int[] { playerColorInts[playerColumns[0]], playerColorInts[playerColumns[1]], playerColorInts[playerColumns[2]] };
        return pColorInts;
    }
}



