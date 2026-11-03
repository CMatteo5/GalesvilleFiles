using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Individual Line Data Class
    [System.Serializable]
    public class lineData
    {
        public string direction;
        public int leftCharacterID;
        public int rightCharacterID;
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

    public Dialogue dialogueHandler;

    public Button optionButton1;
    public Button optionButton2;
    public Button optionButton3;
    public Button optionButton4;

    public GameObject player;
    public GameObject startPoint;

    //Movement + camera
    public CameraFollow cameraFollow;   // the Main Camera (with CameraFollow on it)
    public AreaBounds currentArea;      // the area the player is in right now
    PlayerMovement playerMovement;
    Camera mainCam;
    bool finishedLastFrame;

    public GameObject[] interactibles;
    public bool mapVisible = false;
    public float playerX;
    public float playerY;

    // 1 = Wednesday, 2 = Thursday, 3 = Friday, 4 = Saturday, 5 = Sunday
    public int day;
    // 1 = Dawn, 2 = Day, 3 = Dusk, 4 = Night
    public int timeOfDay;

    // 1 = Town Square, 2 = Main Street, 3 = Outskirts, 4 = Hotel
    public int mainPOI;

    // 1 = Streets (Applies for All), 2 = Hospital/General Store/Church respectively, 3 = Police Station/Butchery/Miller House, 4 = Mayoral Office/ Butchery / Backwood Still, 5 = Hotel Room
    public int subPOI;


    public bool foundBenny;

    void Start()
    {
        mainCam = Camera.main;

        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("GameManager: the player object needs a PlayerMovement component.");
        }

        if (cameraFollow != null)
        {
            cameraFollow.target = player.transform;
        }
        else
        {
            Debug.LogError("GameManager: assign the Main Camera (with CameraFollow) to the Camera Follow slot.");
        }

        SetArea(currentArea);
        changeScript("ExampleScript");
    }


    void Update()
    {
        playerX = player.transform.position.x;
        playerY = player.transform.position.y;
        if (dialogueHandler.scriptFinished)
        {
            if (finishedLastFrame && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (playerMovement != null)
                {
                    Vector3 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    playerMovement.MoveTo(worldPos.x);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                showMap();
            }
        }

        finishedLastFrame = dialogueHandler.scriptFinished;
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

    void changeScript(string IDstring)
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

    public void showMap()
    {
        if (!mapVisible)
        {
            mapVisible = true;
        }
        else
        {
            mapVisible = false;
        }
    }
}