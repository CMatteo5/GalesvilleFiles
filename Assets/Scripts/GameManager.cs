using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    // The one GameManager that survived across scenes. Scene objects use this
    // instead of an Inspector reference, because a scene's own copy of the
    // GameManager gets destroyed as a duplicate when you arrive from elsewhere.
    public static GameManager Instance { get; private set; }

    //Individual Line Data Class
    [System.Serializable]
    public class lineData
    {
        public string direction;
        public int leftCharacterID;
        public int rightCharacterID;
        public string sfx;
        public string displayLine;
    }

    //Script Data Class
    [System.Serializable]
    public class scriptData
    {
        public int scriptID;
        public int finalLine;
        public List<lineData> lines;
        public string button1text;
        public string button2text;
        public string button3text;
        public string button4text;
        public int button1mode;
        public int button2mode;
        public int button3mode;
        public int button4mode;
        public int followUpScriptID1;
        public int followUpScriptID2;
        public int followUpScriptID3;
        public int followUpScriptID4;
    }

    public scriptData currentScript;
    TextAsset currentScriptFile;

    // ---------- Persistent references (these objects travel with the GameManager) ----------
    [Header("Persistent objects (GameUI, Player, Main Camera)")]
    public Dialogue dialogueHandler;
    public AudioSource music;
    public AudioSource sfx;

    public Button optionButton1;
    public Button optionButton2;
    public Button optionButton3;
    public Button optionButton4;

    public GameObject player;
    public CameraFollow cameraFollow;   // the Main Camera (with CameraFollow on it)

    public GameObject mapStuff;
    public Image mapBackground;
    public Button townsqaureButton;
    public Button mainStreetButton;
    public Button outskirtsButton;
    public Image introBackground;
    public TextMeshProUGUI introText;
    public TextMeshProUGUI introText2;

    // ---------- Found automatically in each scene ----------
    [Header("Found automatically when a scene loads")]
    public AreaBounds currentArea;      // the area the player is in right now
    public SpawnPoint currentSpawn;

    PlayerMovement playerMovement;
    Camera mainCam;
    bool finishedLastFrame;
    bool sceneSetUp;

    // ---------- Game state (survives scene loads because GameManager does) ----------
    [Header("Game state")]
    public bool foundBenny = false;
    public bool mapVisible = false;
    public float playerX;
    public float playerY;
    public SpriteRenderer playerSprite;
    public bool playerFacingRight;
    public Texture2D fingerCursor;
    public Texture2D glassCursor;
    public Vector2 hotSpot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    // ---------- Music ----------
    [Header("Music")]
    public AudioClip morgueSong;

    // 1 = Wednesday, 2 = Thursday, 3 = Friday, 4 = Saturday, 5 = Sunday
    public int day = 1;
    // 1 = Dawn, 2 = Day, 3 = Dusk, 4 = Night
    public int timeOfDay = 1;

    // 1 = Town Square, 2 = Main Street, 3 = Outskirts, 4 = Hotel
    public int mainPOI;

    // 1 = Streets (Applies for All), 2 = Hospital/General Store/Church respectively, 3 = Police Station/Butchery/Miller House, 4 = Mayoral Office/ Ice Cream Parlor / Backwood Still, 5 = Morgue/ NA / NA / Hotel Room
    public int subPOI;


    readonly HashSet<string> usedInteractables = new HashSet<string>();


    void Awake()
    {
        timeOfDay = 1;
        day = 1;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    void Start()
    {

        EnsurePersistentRefs();
        if (cameraFollow != null && player != null)
        {
            cameraFollow.target = player.transform;
        }
        if (!sceneSetUp)
        {
            SetUpScene();
        }

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetUpScene();
        handleIntroText();
        if (currentSpawn.playerStartsLookingRight)
        {
            playerFacingRight = true;
            playerSprite.flipX = false;
        }
        else
        {
            playerFacingRight = false;
            playerSprite.flipX = true;
        }
    }


    void SetUpScene()
    {
        sceneSetUp = true;
        EnsurePersistentRefs();

        HideMap();

        currentArea = FindFirstObjectByType<AreaBounds>();
        if (currentArea == null)
        {
            Debug.LogError("GameManager: scene '" + SceneManager.GetActiveScene().name + "' has no AreaBounds.");
        }

        currentSpawn = FindFirstObjectByType<SpawnPoint>();

        if (player != null)
        {
            if (playerMovement != null)
            {
                playerMovement.Stop();   // don't keep walking toward a spot from the last scene
            }

            Vector3 pos = player.transform.position;
            if (currentSpawn != null)
            {
                pos.x = currentSpawn.transform.position.x;
                pos.y = currentSpawn.transform.position.y;
            }
            else if (currentArea != null && currentArea.IsValid)
            {
                Debug.LogWarning("GameManager: no SpawnPoint in this scene, using the middle of the AreaBounds.");
                pos.x = currentArea.CenterX;
            }
            player.transform.position = pos;
        }

        if (cameraFollow != null && player != null)
        {
            cameraFollow.target = player.transform;
        }
        mainPOI = currentArea.areaID;
        subPOI = currentArea.areaSubID;
        SetArea(currentArea);
    }
    void EnsurePersistentRefs()
    {
        if (player == null)
        {
            PlayerMovement found = FindFirstObjectByType<PlayerMovement>();
            if (found != null)
            {
                player = found.gameObject;
            }
        }

        if (player != null && playerMovement == null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("GameManager: the player object needs a PlayerMovement component.");
            }
        }
        else if (player == null)
        {
            Debug.LogError("GameManager: no Player found. Assign it in the Player slot.");
        }

        if (cameraFollow == null)
        {
            cameraFollow = FindFirstObjectByType<CameraFollow>();
            if (cameraFollow == null)
            {
                Debug.LogError("GameManager: assign the Main Camera (with CameraFollow) to the Camera Follow slot.");
            }
        }

        if (mainCam == null)
        {
            mainCam = cameraFollow != null ? cameraFollow.GetComponent<Camera>() : Camera.main;
        }

        if (dialogueHandler == null)
        {
            dialogueHandler = FindFirstObjectByType<Dialogue>(FindObjectsInactive.Include);
        }
    }


    void Update()
    {
        if (player == null || dialogueHandler == null)
        {
            return;
        }

        playerX = player.transform.position.x;
        playerY = player.transform.position.y;
        if (dialogueHandler.scriptFinished)
        {
            bool overUI = EventSystem.current.IsPointerOverGameObject();
            Interactable hoveredInteractable = null;
            Vector3 worldPos = Vector3.zero;

            if (!overUI && mainCam != null)
            {
                worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

                // OverlapPointAll so the player's own collider can't hide an interactable behind them
                Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);
                foreach (Collider2D hit in hits)
                {
                    Interactable found = hit.GetComponent<Interactable>();
                    if (found != null && IsInteractableAvailable(found))
                    {
                        hoveredInteractable = found;
                        break;
                    }
                }
            }

            if (hoveredInteractable != null)
            {
                Cursor.SetCursor(glassCursor, hotSpot, cursorMode);
            }
            else
            {
                Cursor.SetCursor(fingerCursor, hotSpot, cursorMode);
            }

            if (finishedLastFrame && Input.GetMouseButtonDown(0) && !overUI)
            {
                if (playerMovement != null && mainCam != null)
                {
                    if (worldPos.x > playerX)
                    {
                        if (!playerFacingRight)
                        {
                            playerFacingRight = true;
                            playerSprite.flipX = false;
                        }
                    }
                    else
                    {
                        if (playerFacingRight)
                        {
                            playerFacingRight = false;
                            playerSprite.flipX = true;
                        }
                    }

                    if (hoveredInteractable == null)
                    {
                        playerMovement.MoveTo(worldPos.x);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                toggleMap();
            }
        }

        finishedLastFrame = dialogueHandler.scriptFinished;
    }

    // Mirrors Interactable.OnMouseDown: reclickable ones are always available,
    // one-time ones are available until they've been used.
    bool IsInteractableAvailable(Interactable interactable)
    {
        return interactable.reclickable || !IsInteractableUsed(interactable.interactableID);
    }

    public void SetArea(AreaBounds newArea)
    {
        currentArea = newArea;

        if (playerMovement != null)
        {
            playerMovement.area = newArea;
        }

        if (cameraFollow != null)
        {
            cameraFollow.area = newArea;
            cameraFollow.SnapToTarget();
        }
    }

    public void changeScript(string IDstring)
    {
        hideButtons();
        currentScriptFile = Resources.Load<TextAsset>(IDstring);
        if (currentScriptFile == null)
        {
            Debug.LogError("GameManager: no script file named '" + IDstring + "' found in a Resources folder.");
            return;
        }
        currentScript = JsonUtility.FromJson<scriptData>(currentScriptFile.text);
        assignButtonFunction();
        dialogueHandler.StartDialogue();
    }

    public void showButtons()
    {
        if (currentScript.button1mode != 0)
        {
            optionButton1.gameObject.SetActive(true);
        }
        if (currentScript.button2mode != 0)
        {
            optionButton2.gameObject.SetActive(true);
        }
        if (currentScript.button3mode != 0)
        {
            optionButton3.gameObject.SetActive(true);
        }
        if (currentScript.button4mode != 0)
        {
            optionButton4.gameObject.SetActive(true);
        }
        SetButtonText(optionButton1, currentScript.button1text);
        SetButtonText(optionButton2, currentScript.button2text);
        SetButtonText(optionButton3, currentScript.button3text);
        SetButtonText(optionButton4, currentScript.button4text);
    }

    void SetButtonText(Button button, string label)
    {
        TextMeshProUGUI tmp = button.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmp != null)
        {
            tmp.text = label;
            return;
        }

        Text legacy = button.GetComponentInChildren<Text>(true);
        if (legacy != null)
        {
            legacy.text = label;
            return;
        }

        Debug.LogWarning("GameManager: no text component found under button '" + button.name + "'.");
    }


    public void assignButtonFunction()
    {
        optionButton1.onClick.RemoveAllListeners();
        optionButton2.onClick.RemoveAllListeners();
        optionButton3.onClick.RemoveAllListeners();
        optionButton4.onClick.RemoveAllListeners();

        if (currentScript.button1mode == 1)
        {
            string target = currentScript.followUpScriptID1.ToString();
            optionButton1.onClick.AddListener(() => changeScript(target));
        }
        if (currentScript.button2mode == 1)
        {
            string target = currentScript.followUpScriptID2.ToString();
            optionButton2.onClick.AddListener(() => changeScript(target));
        }
        if (currentScript.button3mode == 1)
        {
            string target = currentScript.followUpScriptID3.ToString();
            optionButton3.onClick.AddListener(() => changeScript(target));
        }
        if (currentScript.button4mode == 1)
        {
            string target = currentScript.followUpScriptID4.ToString();
            optionButton4.onClick.AddListener(() => changeScript(target));
        }
    }

    public bool allButtonsInactive()
    {
        return !optionButton1.gameObject.activeSelf && !optionButton2.gameObject.activeSelf && !optionButton3.gameObject.activeSelf && !optionButton4.gameObject.activeSelf;
    }


    public void hideButtons()
    {
        optionButton1.gameObject.SetActive(false);
        optionButton2.gameObject.SetActive(false);
        optionButton3.gameObject.SetActive(false);
        optionButton4.gameObject.SetActive(false);
    }

    // Called by a SubPOIButton: records the location, closes the map and loads its scene.
    public void changeScene(int deltaMainPOI, int deltaSubPOI, string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("GameManager: this sub-POI button has no Scene Name set.");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("GameManager: scene '" + sceneName + "' isn't in the build's scene list. " +
                "Add it under File > Build Profiles > Scene List.");
            return;
        }

        mainPOI = deltaMainPOI;
        subPOI = deltaSubPOI;
        HideMap();
        SceneManager.LoadScene(sceneName);
    }

    public void toggleMap()
    {
        if (mapVisible)
        {
            HideMap();
        }
        else
        {
            Debug.Log("Showing map");
            mapVisible = true;
            mapBackground.gameObject.SetActive(true);
            mainStreetButton.gameObject.SetActive(true);
            townsqaureButton.gameObject.SetActive(true);
            outskirtsButton.gameObject.SetActive(true);
        }
    }

    public void HideMap()
    {
        mapVisible = false;
        if (mapStuff == null)
        {
            return;
        }
        foreach (Transform child in mapStuff.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public bool IsInteractableUsed(string id)
    {
        return usedInteractables.Contains(id);
    }

    public void MarkInteractableUsed(string id)
    {
        usedInteractables.Add(id);
    }

    public void handleIntroText()
    {
        string introText1String = "";
        string introText2String = "";

        //Time of Day
        if (timeOfDay == 1)
        {
            introText1String += "Dawn";
        }
        else if (timeOfDay == 2)
        {
            introText1String += "Day";
        }
        else if (timeOfDay == 3)
        {
            introText1String += "Dusk";
        }
        else if (timeOfDay == 4)
        {
            introText1String += "Night";
        }

        //Day of the Week
        if (day == 1)
        {
            introText1String += ", Wednesday October 12th, 1927";
        }
        else if (day == 2)
        {
            introText1String += ", Thursday October 13th, 1927";
        }
        else if (day == 3)
        {
            introText1String += ", Friday October 14th, 1927";
        }
        else if (day == 4)
        {
            introText1String += ", Saturday October 15th, 1927";
        }
        else if (day == 5)
        {
            introText1String += ", Sunday October 16th, 1927";
        }

        //Sub POI
        if (subPOI == 1)
        {
            if (mainPOI == 1)
                introText2String += "Town Square Streets, Galesville, Illinois";
            if (mainPOI == 2)
                introText2String += "Main Street Streets, Galesville, Illinois";
            if (mainPOI == 3)
                introText2String += "Outskirts Streets, Galesville, Illinois";
        }
        if (subPOI == 2)
        {
            if (mainPOI == 1)
                introText2String += "Rockwell Hospital, Galesville, Illinois";
            if (mainPOI == 2)
                introText2String += "Galesville Feed and Supply, Galesville, Illinois";
            if (mainPOI == 3)
                introText2String += "Saint Matthew's Lutheran Church, Galesville, Illinois";
        }
        if (subPOI == 3)
        {
            if (mainPOI == 1)
                introText2String += "Galesville Police Station, Galesville, Illinois";
            if (mainPOI == 2)
                introText2String += "Anderson Butchery, Galesville, Illinois";
            if (mainPOI == 3)
                introText2String += "Miller Homestead, Unincorporated Galesville, Illinois";
        }
        if (subPOI == 4)
        {
            if (mainPOI == 1)
                introText2String += "Galesville Mayoral Office, Galesville, Illinois";
            if (mainPOI == 2)
                introText2String += "Betty's Ice Cream and Soda Fountain, Galesville, Illinois";
            if (mainPOI == 3)
                introText2String += "Miller Woods, Unincorporated Galesville, Illinois";
        }
        if (subPOI == 5)
        {
            if (mainPOI == 4)
                introText2String += "Galesville Hostel Room 3, Galesville, Illinois";
            if (mainPOI == 1)
            {
                introText2String += "Rockwell Hospital - Morgue, Galesville, Illinois";
                music.PlayOneShot(morgueSong);
            }
        }

        introText.text = introText1String;
        introText2.text = introText2String;
        StartCoroutine(fadeOut(introBackground.gameObject, 2f, 5f));
        StartCoroutine(FadeInThenOut(introText.gameObject, introText2.gameObject, 2f, 2f));

    }
    IEnumerator FadeInThenOut(GameObject target1, GameObject target2, float duration, float waitTime)
    {
        CanvasGroup canvasGroup = target1.GetComponent<CanvasGroup>();
        CanvasGroup canvasGroup2 = target2.GetComponent<CanvasGroup>();
        canvasGroup2.alpha = 0f;
        if (canvasGroup == null)
        {
            yield return null;
        }

        float halfDuration = duration / 2f;
        float elapsedTime = 0f;

        // Fade in
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / halfDuration);
            yield return null;
        }

        // Ensure fully visible
        canvasGroup.alpha = 1f;

        // Wait for a moment before fading out
        yield return new WaitForSeconds(waitTime);

        elapsedTime = 0f;

        // Fade out
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / halfDuration));
            yield return null;
        }

        // Ensure fully invisible
        canvasGroup.alpha = 0f;

        canvasGroup = target2.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            yield return null;
        }

         halfDuration = duration / 2f;
         elapsedTime = 0f;

        // Fade in
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / halfDuration);
            yield return null;
        }

        // Ensure fully visible
        canvasGroup.alpha = 1f;

        // Wait for a moment before fading out
        yield return new WaitForSeconds(waitTime);

        elapsedTime = 0f;

        // Fade out
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / halfDuration));
            yield return null;
        }

        // Ensure fully invisible
        canvasGroup.alpha = 0f;


    }

    IEnumerator fadeOut(GameObject target, float duration, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            yield return null;
        }

        float elapsedTime = 0f;

        // Fade out
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / duration));
            yield return null;
        }

        // Ensure fully invisible
        canvasGroup.alpha = 0f;
    }


}