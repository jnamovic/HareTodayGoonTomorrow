using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XboxCtrlrInput;
using UnityEngine.UI;
// ADDED
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    public GameObject m1;
    public GameObject m2;
    public GameObject m3;
    public GameObject ff;
    public GameObject rockPile;

    private GameObject mouse1;
    private GameObject mouse2;
    private GameObject mouse3;
    private GameObject fooFoo;

    // EDITED
    public List<GameObject> players;
    private List<GameObject> rockPiles = new List<GameObject>();
    private List<GameObject> pieces = new List<GameObject>();

    //public CameraMultiTarget c;
    private CameraMultiTarget c;

    private float rockTimer;

    private int numKOd = 0;
    private int numPiecesAssembled = 0;

    private int numFooFooWins = 0;
    private int numBoperativeWins = 0;

    //public GameObject mouseSpawnPoints;
    //public GameObject fooFooSpawnPoints;
    private GameObject mouseSpawnPoints;
    private GameObject fooFooSpawnPoints;
    private GameObject rockSpawnPoints;
    private GameObject bonus;

    public bool playing=false;
    public bool started=false;
    public bool firstFrame=true;

    // ADDED
    public bool startScreen;
    public bool endScreen;

    private bool paused;

    // ADDED
    private bool forcePaused;

    private float tScale;

    // CHANGED
    public int[] playerValues;
    private Material[] playerMaterials;

    // ADDED
    private Canvas canvas;
    private int piecesGathered;
    public int numSecondsForMatch = 180;
    private float numSecondsElapsed;

    // ADDED
    private bool observed;

    // ADDED
    public string[] startButtons = { "start", "credits", "help", "quit" };
    private string buttonSelected = "start";

    public int[] playerColorInts;

    // ADDED
    private Transform startCanvas;

    private void Awake()
    {
        GameObject g = GameObject.Find("GameManager");
        if (g != this.gameObject) {
            
            Destroy(g.gameObject);
        }

        S = this;
        

        playing = false;
        firstFrame = true;
        startScreen = false;
        endScreen = false;
        DontDestroyOnLoad(this.gameObject);
        tScale = Time.timeScale;

        // ADDED

        numBoperativeWins = 0;
        numFooFooWins = 0;
        numSecondsElapsed = 0;


        startCanvas = GameObject.Find("Canvas").gameObject.transform;
        
    }

    // Start is called before the first frame update
    void GameStart()
    {
        firstFrame = true;
        GameManager.S.paused = false;
        GameManager.S.forcePaused = false;
        Time.timeScale = tScale;
        c = GameObject.FindWithTag("MainCamera").GetComponent<CameraMultiTarget>();
        canvas.transform.GetChild(9).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = numFooFooWins.ToString();
        canvas.transform.GetChild(9).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = numBoperativeWins.ToString();

        mouseSpawnPoints = GameObject.Find("MouseSpawnPoints");
        fooFooSpawnPoints = GameObject.Find("FooFooSpawnPoints");
        rockSpawnPoints = GameObject.Find("RockSpawnPoints");
        bonus = GameObject.Find("Bonus");

        players = new List<GameObject>();

        mouse1 = Instantiate(m1);
        mouse1.name = "Mouse 1";
        mouse2 = Instantiate(m2);
        mouse2.name = "Mouse 2";
        mouse3 = Instantiate(m3);
        mouse3.name = "Mouse 3";
        fooFoo = Instantiate(ff);
        fooFoo.name = "Foo Foo";

        // ADDED
        mouse1.GetComponent<ShootBabyShoot>().matIndex = playerColorInts[0];
        mouse2.GetComponent<ShootBabyShoot>().matIndex = playerColorInts[1];
        mouse3.GetComponent<ShootBabyShoot>().matIndex = playerColorInts[2];

        SetPlayerMaterials();

        players.Add(mouse1);
        players.Add(mouse2);
        players.Add(mouse3);
        players.Add(fooFoo);

        SetPlayerCharacters();
        

        c.SetTargets(players.ToArray());

        started = true;

        RoundReset();
        firstFrame = false;

        endScreen = false;
        Time.timeScale = tScale;

    }

    // Update is called once per frame
    void Update()
    {
        // ADDED
        if (SceneManager.GetActiveScene().name == "Arena 1" && !started)
            playing = true;

        if (playing)
        {
            rockTimer += Time.deltaTime;
            if (!started)
            {
                // ADDED
                canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

                GameStart();
                SoundManager.S.MakeFooFooMusicSound();
            }
            // CHANGED
            else if (!forcePaused)
            {
                if (XCI.GetButtonDown(XboxButton.Start, XboxController.First) ||
                    XCI.GetButtonDown(XboxButton.Start, XboxController.Second) ||
                    XCI.GetButtonDown(XboxButton.Start, XboxController.Third) ||
                    XCI.GetButtonDown(XboxButton.Start, XboxController.Fourth))
                {
                    paused = !paused;
                    canvas.transform.GetChild(6).gameObject.SetActive(paused);
                }
            }

            // CHANGED
            if (paused || forcePaused)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = tScale;

                // ADDED
                UpdateTime();
            }

            // CHANGED
            if (GetNumKOd() >= 3 && !observed)
            {
                observed = true;
                FooFooWinsRound();
            }
            else if (numPiecesAssembled >= 3 && !observed)
            {
                observed = true;
                BoperativesWinRound();
            }

            if (rockTimer > 5 && rockPiles.Count < 4)
            {

                SpawnRock();
                rockTimer = 0.0f;
            }
        } else if (startScreen)
        {
            // startCanvas = GameObject.Find("Canvas").gameObject.transform;
            if (buttonSelected == "start")
            {
                startCanvas.GetChild(2).GetComponent<Image>().color = Color.yellow;
                if (XCI.GetButtonDown(XboxButton.A))
                {
                    startCanvas.GetChild(2).GetComponent<Image>().color = Color.green;
                    startScreen = false;
                    SceneManager.LoadScene("Character Selection");
                }
                else if (XCI.GetButtonDown(XboxButton.DPadLeft))
                {
                    buttonSelected = "credits";
                    startCanvas.GetChild(2).GetComponent<Image>().color = Color.white;
                }
                else if (XCI.GetButtonDown(XboxButton.DPadDown))
                {
                    buttonSelected = "quit";
                    startCanvas.GetChild(2).GetComponent<Image>().color = Color.white;
                }
                else if (XCI.GetButtonDown(XboxButton.DPadRight))
                {
                    buttonSelected = "help";
                    startCanvas.GetChild(2).GetComponent<Image>().color = Color.white;
                }
            }
            else if (buttonSelected == "credits")
            {
                startCanvas.GetChild(3).GetComponent<Image>().color = Color.yellow;
                if (XCI.GetButtonDown(XboxButton.A))
                {
                    if (startCanvas.GetChild(6).gameObject.active)
                        startCanvas.GetChild(6).gameObject.SetActive(false);
                    else
                        startCanvas.GetChild(6).gameObject.SetActive(true);
                }
                else if (XCI.GetButtonDown(XboxButton.DPadUp))
                {
                    buttonSelected = "start";
                    startCanvas.GetChild(3).GetComponent<Image>().color = Color.white;
                    startCanvas.GetChild(6).gameObject.SetActive(false);
                }
                else if (XCI.GetButtonDown(XboxButton.DPadRight))
                {
                    buttonSelected = "quit";
                    startCanvas.GetChild(3).GetComponent<Image>().color = Color.white;
                    startCanvas.GetChild(6).gameObject.SetActive(false);
                }
            }
            else if (buttonSelected == "help")
            {
                startCanvas.GetChild(4).GetComponent<Image>().color = Color.yellow;
                if (XCI.GetButtonDown(XboxButton.A))
                {
                    if (startCanvas.GetChild(7).gameObject.active)
                        startCanvas.GetChild(7).gameObject.SetActive(false);
                    else
                        startCanvas.GetChild(7).gameObject.SetActive(true);
                }
                else if (XCI.GetButtonDown(XboxButton.DPadUp))
                {
                    buttonSelected = "start";
                    startCanvas.GetChild(4).GetComponent<Image>().color = Color.white;
                    startCanvas.GetChild(7).gameObject.SetActive(false);
                }
                else if (XCI.GetButtonDown(XboxButton.DPadLeft))
                {
                    buttonSelected = "quit";
                    startCanvas.GetChild(4).GetComponent<Image>().color = Color.white;
                    startCanvas.GetChild(7).gameObject.SetActive(false);
                }
            }
            else if (buttonSelected == "quit")
            {
                startCanvas.GetChild(5).GetComponent<Image>().color = Color.yellow;
                if (XCI.GetButtonDown(XboxButton.A))
                {
                    Application.Quit();
                }
                else if (XCI.GetButtonDown(XboxButton.DPadLeft))
                {
                    buttonSelected = "credits";
                    startCanvas.GetChild(5).GetComponent<Image>().color = Color.white;
                }
                else if (XCI.GetButtonDown(XboxButton.DPadUp))
                {
                    buttonSelected = "start";
                    startCanvas.GetChild(5).GetComponent<Image>().color = Color.white;
                }
                else if (XCI.GetButtonDown(XboxButton.DPadRight))
                {
                    buttonSelected = "help";
                    startCanvas.GetChild(5).GetComponent<Image>().color = Color.white;
                }
            }
        }
        else if (endScreen)
        {
            
            
            if (XCI.GetButtonDown(XboxButton.A))
            {
                //endScreen = false;
                //firstFrame = true;
                LoadCharSelection();
            }
            
        }
        else if (SceneManager.GetActiveScene().name == "Start")
        {
            startScreen = true;
        }
        else
        {
            started = false;
            SoundManager.S.MakeMainMenuMusicSound();
            
            // ADDED
            /*if (XCI.GetButtonDown(XboxButton.B) && SelectionLogic.S.playerStates[0] == 1 &&
                SelectionLogic.S.playerStates[1] == 1 && SelectionLogic.S.playerStates[2] == 1
                && SelectionLogic.S.playerStates[3] == 1)
            {
                startScreen = true;
                SceneManager.LoadScene("Start");
            }*/
        }


    }

    public void SetNumKOd(int change)
    {
        numKOd += change;
    }

    public int GetNumKOd()
    {
        return numKOd;
    }

    public void SetNumPiecesAssembled(int change)
    {
        numPiecesAssembled += change;
    }

    public int GetNumPiecesAssembled()
    {
        return numPiecesAssembled;
    }

    private void SpawnRock()
    {
        System.Random r = new System.Random();
        bool spawned = false;
        int count = 0;
        while (!spawned && count < 20)
        {
            count++;
            spawned = true;
            int g1Check = r.Next(0, 7);
            int g2Check = r.Next(0, 2);
            Vector3 location = rockSpawnPoints.transform.GetChild(g1Check).GetChild(g2Check).transform.position;




            var hitColliders = Physics.OverlapSphere(location, .5f);//2 is purely chosen arbitrarly

            if (hitColliders.Length > 1)
            { //You have someone with a collider here
                spawned = false;

            }
            else
            {
                GameObject rocko = Instantiate(rockPile);
                rocko.transform.position = location;
                rockPiles.Add(rocko);
            }
        }
    }

    public void RemoveRock(GameObject rock)
    {
        rockPiles.Remove(rock);
    }

    private void RoundReset()
    {
        // ADDED
        observed = false;
        Time.timeScale = tScale;
        canvas.transform.GetChild(9).gameObject.SetActive(false);
        forcePaused = false;
        paused = false;
        StopAllCoroutines();

        numSecondsForMatch = 180;

        for (int i = 0; i < rockPiles.Count; i++)
        {
            Destroy(rockPiles[i]);
        }
        rockPiles.Clear();

        for (int i = 0; i < pieces.Count; i++)
        {
            Destroy(pieces[i]);
        }
        pieces.Clear();

        if (players.Count < 4)
        {
            players.Add(mouse1);
            players.Add(mouse2);
            players.Add(mouse3);
            UpdateTargets();
        }

        numPiecesAssembled = 0;

        System.Random m = new System.Random();

        int mouseSpawnGroupNum = m.Next(0, mouseSpawnPoints.transform.childCount);

        Transform proxy = mouseSpawnPoints.transform.GetChild(mouseSpawnGroupNum);

        System.Random r = new System.Random();

        mouse1.GetComponent<MouseScript>().Revive();
        mouse1.transform.rotation = Quaternion.identity;
        mouse2.GetComponent<MouseScript>().Revive();
        mouse2.transform.rotation = Quaternion.identity;
        mouse3.GetComponent<MouseScript>().Revive();
        mouse3.transform.rotation = Quaternion.identity;

        fooFoo.GetComponent<FooFooScript>().Reset();
        fooFoo.GetComponent<FooFooControlScript>().Reset();
        fooFoo.transform.rotation = Quaternion.identity;

        int p1Check = r.Next(0, 3);
        int p2Check = r.Next(0, 2);

        if (p1Check == 0)
        {
            mouse1.transform.position = proxy.GetChild(0).transform.position;

            if (p2Check == 0)
            {
                mouse2.transform.position = proxy.GetChild(1).transform.position;
                mouse3.transform.position = proxy.GetChild(2).transform.position;
            }
            else
            {
                mouse2.transform.position = proxy.GetChild(2).transform.position;
                mouse3.transform.position = proxy.GetChild(1).transform.position;
            }
        }
        else if (p1Check == 1)
        {
            mouse1.transform.position = proxy.GetChild(1).transform.position;

            if (p2Check == 0)
            {
                mouse2.transform.position = proxy.GetChild(0).transform.position;
                mouse3.transform.position = proxy.GetChild(2).transform.position;
            }
            else
            {
                mouse2.transform.position = proxy.GetChild(2).transform.position;
                mouse3.transform.position = proxy.GetChild(0).transform.position;
            }
        }
        else
        {
            mouse1.transform.position = proxy.GetChild(2).transform.position;

            if (p2Check == 0)
            {
                mouse2.transform.position = proxy.GetChild(0).transform.position;
                mouse3.transform.position = proxy.GetChild(1).transform.position;
            }
            else
            {
                mouse2.transform.position = proxy.GetChild(1).transform.position;
                mouse3.transform.position = proxy.GetChild(0).transform.position;
            }
        }


        System.Random f = new System.Random();

        int fooFooSpawnPointNum = f.Next(0, fooFooSpawnPoints.transform.childCount);

        fooFoo.transform.position = fooFooSpawnPoints.transform.GetChild(fooFooSpawnPointNum).position;

        SetCanvasColors();
    }

    // ADDED
    private void SetCanvasColors()
    {

        for (int i = 0; i < 4; i++)
        {
            // Only works with TextMeshPro
            canvas.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().color = playerMaterials[playerValues[i]].color;
            if (playerMaterials[playerValues[i]].color == Color.black)
                canvas.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().outlineColor = Color.white;
        }
    }

    private void FooFooWinsRound()
    {
        numFooFooWins += 1;
        // if (numFooFooWins < 3)
        paused = false;
        forcePaused = true;

        StartCoroutine(StatusScreen(0, numFooFooWins < 3));
        SoundManager.S.MakeMiceLoseRoundSound();

        // RoundReset();
        // else
        //    FooFooWinsGame();            
    }

    private void BoperativesWinRound()
    {
        numBoperativeWins += 1;
        // if (numBoperativeWins < 3)
        paused = false;
        forcePaused = true;

        StartCoroutine(StatusScreen(1, numBoperativeWins < 3));
        SoundManager.S.MakeFooFooLoseRoundSound();

        // RoundReset();
        // else
        //    BoperativesWinGame();
    }


    // ADDED
    public IEnumerator StatusScreen(int child, bool cont)
    {
        canvas.transform.GetChild(9).gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(2.0f);

        // a = 0: Foo Foo
        // a = 1: bOperatives
        int a = numFooFooWins;
        if (child == 1)
            a = numBoperativeWins;

        canvas.transform.GetChild(9).GetChild(1).GetChild(child).GetComponent<TextMeshProUGUI>().text = a.ToString();

        // Foo Foo stylings
        if (child == 0)
        {
            canvas.transform.GetChild(9).GetChild(6).gameObject.SetActive(true);
        }
        else
        {
            canvas.transform.GetChild(9).GetChild(4).gameObject.SetActive(true);
            canvas.transform.GetChild(9).GetChild(5).gameObject.SetActive(true);
            if (a == 1)
                canvas.transform.GetChild(9).GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Oh Foo Foo! You mischevious scamp! Come now, leave the poor mice alone.";
            else if (a == 2)
                canvas.transform.GetChild(9).GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Okay, Foo Foo, I warned you once already. I won't warn you again.";
            else if (a == 3)
                canvas.transform.GetChild(9).GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "WHAT DID I LITERALLY JUST SAY?! THAT'S IT! GOON TIME FOR YOU!";
        }

        /*
        if (child == 1)
        {
            if (!SoundManager.S._FooFooLoseRound.isPlaying)
                SoundManager.S._FooFooMusic.UnPause();
        } else
        {
            if (!SoundManager.S._MiceLoseRoundSound.isPlaying)
                SoundManager.S._FooFooMusic.UnPause();
        }
        */


        yield return new WaitForSecondsRealtime(3.0f);

        string nextscene = "Arena ";
        nextscene += (numFooFooWins + numBoperativeWins) % 5 + 1;
        

      

        if (cont)
        {
            // Load scene logic here
            started = false;
            SceneManager.LoadScene(nextscene);
            numSecondsForMatch = 180;
        }
        else
        {
            if (numFooFooWins >= 3)
                FooFooWinsGame();
            else if (numBoperativeWins >= 3)
                BoperativesWinGame();
        }

        //RoundReset();
    }


    private void FooFooWinsGame()
    {
        canvas.transform.GetChild(8).gameObject.SetActive(true);
        UpdatePlayerColor(canvas.transform.GetChild(8).GetChild(3).gameObject, playerMaterials[playerValues[0]]);
        UpdatePlayerColor(canvas.transform.GetChild(8).GetChild(4).gameObject, playerMaterials[playerValues[1]]);
        UpdatePlayerColor(canvas.transform.GetChild(8).GetChild(5).gameObject, playerMaterials[playerValues[2]]);
        UpdatePlayerColor(canvas.transform.GetChild(8).GetChild(6).gameObject, playerMaterials[playerValues[3]]);

        canvas.transform.GetChild(8).GetChild(3).gameObject.GetComponent<Animator>().SetBool("alive", false);
        canvas.transform.GetChild(8).GetChild(4).gameObject.GetComponent<Animator>().SetBool("alive", false);
        canvas.transform.GetChild(8).GetChild(5).gameObject.GetComponent<Animator>().SetBool("alive", false);

        canvas.transform.GetChild(9).gameObject.SetActive(false);

        endScreen = true;
        playing = false;
    }

    private void BoperativesWinGame()
    {
        UpdatePlayerColor(canvas.transform.GetChild(7).GetChild(3).gameObject, playerMaterials[playerValues[0]]);
        UpdatePlayerColor(canvas.transform.GetChild(7).GetChild(4).gameObject, playerMaterials[playerValues[1]]);
        UpdatePlayerColor(canvas.transform.GetChild(7).GetChild(5).gameObject, playerMaterials[playerValues[2]]);

        canvas.transform.GetChild(7).gameObject.SetActive(true);

        canvas.transform.GetChild(9).gameObject.SetActive(false);

        endScreen = true;
        playing = false;
    }

    public void SetPlayerValues()
    {
        playerValues = SelectionLogic.S.GivePlayerValues();
        playerMaterials = SelectionLogic.S.GiveMaterialValues();

        // ADDED
        playerColorInts = SelectionLogic.S.GivePlayerColorInts();

        /*for (int i = 0; i < 4; i ++)
        {
            print(i + " " + playerValues[i]);
        }*/
    }



    public void SetPlayerMaterials()
    {
        // CHANGED
        try
        {
            mouse1.GetComponent<Renderer>().material = playerMaterials[playerValues[0]];
            mouse2.GetComponent<Renderer>().material = playerMaterials[playerValues[1]];
            mouse3.GetComponent<Renderer>().material = playerMaterials[playerValues[2]];
            fooFoo.GetComponent<Renderer>().material = playerMaterials[playerValues[3]];
        }
        catch (Exception e)
        {
            UpdatePlayerColor(mouse1, playerMaterials[playerValues[0]]);
            UpdatePlayerColor(mouse2, playerMaterials[playerValues[1]]);
            UpdatePlayerColor(mouse3, playerMaterials[playerValues[2]]);
            UpdatePlayerColor(fooFoo, playerMaterials[playerValues[3]]);
        }
    }

    // ADDED
    public void UpdatePlayerColor(GameObject obj, Material m)
    {
        Transform player = obj.transform.GetChild(1);

        // Foo Foo
        if (obj.tag == "foofoo")
        {
            player.GetChild(1).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(2).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(3).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(4).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(5).GetChild(0).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(5).GetChild(1).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(5).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(6).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(7).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(8).GetChild(0).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(8).GetChild(1).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(8).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(9).gameObject.GetComponent<Renderer>().material = m;
        }
        // Mouse
        else
        {
            player.GetChild(0).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(1).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(2).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(3).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(4).GetChild(0).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(4).GetChild(1).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(4).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(6).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(7).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(8).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(9).GetChild(0).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(9).GetChild(1).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(9).GetChild(2).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(11).gameObject.GetComponent<Renderer>().material = m;
            player.GetChild(12).gameObject.GetComponent<Renderer>().material = m;
        }
    }

    public void SetPlayerCharacters()
    {
        for (int i = 0; i < 4; i++)
        {
            players[i].GetComponent<BasePlayerMovementScript>().playerNumber = (XboxController)(playerValues[i] + 1);

            // ADDED
            // canvas.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "P" + (playerValues[i] + 1);

            // NOTE: This only works with TextMeshPro
            canvas.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "P" + (playerValues[i] + 1);
        }
    }

    public void UpdateTargets()
    {
        c.SetTargets(players.ToArray());
    }

    // CHANGED
    public void UpdateRocksOfPlayer(XboxController x, int rockNum)
    {
        int pIndex = (int)x;

        for (int i = 6; i < 11; i++)
        {
            if (i < 6 + rockNum)
            {
                canvas.transform.GetChild(playerValues[pIndex - 1]).GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                canvas.transform.GetChild(playerValues[pIndex - 1]).GetChild(i).gameObject.SetActive(false);
            }
        }

        /*
        canvas.transform.GetChild(playerValues[pIndex - 1]).GetChild(3).GetComponent<TextMeshProUGUI>().text = rockNum.ToString();
        // canvas.transform.GetChild(playerValues[pIndex - 1]).GetChild(3).GetComponent<Text>().text = rockNum.ToString();

        if (rockNum == 0)
            canvas.transform.GetChild(playerValues[pIndex - 1]).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.red;
        // canvas.transform.GetChild(playerValues[pIndex - 1]).GetChild(3).GetComponent<Text>().color = Color.red;
        else
            canvas.transform.GetChild(playerValues[pIndex - 1]).GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.black;
        // canvas.transform.GetChild(playerValues[pIndex - 1]).GetChild(3).GetComponent<Text>().color = Color.black;
        */
    }

    // ADDED
    public void UpdatePieces(GameObject piece, bool released)
    {
        if (piece.name.Substring(6, 1) == "S")
            canvas.transform.GetChild(3).GetChild(2).GetChild(0).gameObject.SetActive(released);
        else if (piece.name.Substring(6, 1) == "B")
            canvas.transform.GetChild(3).GetChild(2).GetChild(1).gameObject.SetActive(released);
        else
            canvas.transform.GetChild(3).GetChild(2).GetChild(2).gameObject.SetActive(released);
    }

    // ADDED
    public void UpdatePiecesAssembled(int pieceNum)
    {
        piecesGathered = pieceNum;
        canvas.transform.GetChild(5).GetChild(1).GetComponent<Text>().text = pieceNum.ToString();
    }

    // CHANGED
    public void UpdateTime()
    {
        numSecondsElapsed += Time.deltaTime;

        if (numSecondsElapsed >= 1)
        {
            numSecondsForMatch -= (int)numSecondsElapsed;
            numSecondsElapsed = 0.0f;
        }

        int minutes = numSecondsForMatch / 60;
        int seconds = numSecondsForMatch % 60;

        if (minutes > 0 || seconds > 0)
        {
            if (seconds > 9)
                canvas.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = minutes + ":" + seconds;
            else
                canvas.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = minutes + ":0" + seconds;
        }

        if (minutes == 0 && seconds <= 10)
            canvas.transform.GetChild(4).GetComponent<TextMeshProUGUI>().color = Color.red;

        if (minutes <= 0 && seconds <= 0)
        {
            FooFooWinsRound();
        }
    }

    // ADDED
    public void UpdatePlayerStatus(XboxController x, bool status)
    {
        int pIndex = (int)x;

        canvas.transform.GetChild(playerValues[pIndex - 1]).GetChild(4).gameObject.SetActive(status);
    }

    // ADDED
    public void UpdateDash(float value)
    {
        if (value > 1)
            value = 1;
        canvas.transform.GetChild(3).GetChild(1).GetChild(1).GetComponent<Slider>().value = value;
    }


    public void AddBonus(GameObject piece)
    {
        piece.transform.SetParent(bonus.transform);

        switch (numPiecesAssembled)
        {
            case 0:
                piece.transform.position = new Vector3(bonus.transform.position.x - .5f, bonus.transform.position.y + 1f, bonus.transform.position.z);
                break;
            case 2:
                piece.transform.position = new Vector3(bonus.transform.position.x, bonus.transform.position.y + 1f, bonus.transform.position.z);
                break;
            default:
                piece.transform.position = new Vector3(bonus.transform.position.x + .5f, bonus.transform.position.y + 1f, bonus.transform.position.z);
                break;
        }
        SetNumPiecesAssembled(1);

        // ADDED
        fooFoo.GetComponent<BasePlayerMovementScript>().FooFooSpeedUp();
    }

    public void PieceInPlay(GameObject piece)
    {
        pieces.Add(piece);
    }

    public void LoadCharSelection()
    {
        // ADDED
        GameManager.S.firstFrame = true;
        GameManager.S.started = false;
        GameManager.S.playing = false;
        Debug.Log("i am going back to character select");

        GameManager.S.paused = false;
        GameManager.S.forcePaused = false;
        Time.timeScale = tScale;
        



        SceneManager.LoadScene("Character Selection");
        Awake();
    }
}






//CODE GRAVE YARD






/*if (playing)
{
    rockTimer += Time.deltaTime;
    if (!started)
    {
        // ADDED
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        GameStart();
        SoundManager.S.MakeFooFooMusicSound();
    }
    else
    {
        if (XCI.GetButtonDown(XboxButton.Start, XboxController.First) ||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Second) ||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Third) ||
            XCI.GetButtonDown(XboxButton.Start, XboxController.Fourth))
        {
            paused = !paused;
            canvas.transform.GetChild(6).gameObject.SetActive(paused);
        }
    }

    if (paused)
    {
        Time.timeScale = 0.0f;
    }
    else
    {
        Time.timeScale = tScale;

        // ADDED
        UpdateTime();
    }

    if (GetNumKOd() >= 3)
        FooFooWinsRound();
    else if (numPiecesAssembled >= 3)
        BoperativesWinRound();

    if (rockTimer > 5 && rockPiles.Count < 4)
    {

        SpawnRock();
        rockTimer = 0.0f;
    }
}
else
{
    started = false;
}*/
