using System.Collections.Generic;
using UnityEngine;
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

    // 1 = Wednesday, 2 = Thursday, 3 = Friday, 4 = Saturday, 5 = Sunday
    public int day;
    // 1 = Dawn , 2 = Day, 3 = Dusk, 4 = Night
    public int timeOfDay;


    void Start()
    {
        changeScript("ExampleScript");
    }


    void Update()
    {

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
}