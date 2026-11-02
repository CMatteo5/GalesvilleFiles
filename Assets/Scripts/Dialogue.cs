using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public Image leftCharacter;
    public Image rightCharacter;
    //public string[] lines;
    public int[] focus;
    public int people;
    public int focusScale;
    public int unfocusScale;
    public float textSpeed;


    //New Carlos Stuff Driven Via JSON
    public int rightID;
    public int leftID;
    public bool scriptFinished;
    public GameManager gameManagerRef;



    //Sprites for characters
    public Sprite detective;
    public Sprite nurse;
    public Sprite brood;
    public Sprite butcher;
    public Sprite reverend;
    public Sprite mayor;
    public Sprite employee;
    public Sprite sheriff;
    public Sprite deacon;
    public Sprite bootlegger;
    public Sprite mayorsWife;
    public Sprite monster;


    public int index;
    public int preIndex;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Intentionally empty: GameManager is the only thing that starts dialogue
        // (it calls StartDialogue() after it has loaded a script). Don't clear the text
        // here, or it can wipe the first typed letter depending on Start() order.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!scriptFinished)
            {
                if (textComponent.text == gameManagerRef.currentScript.lines[index].displayLine)
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = gameManagerRef.currentScript.lines[index].displayLine;
                }
            }
        }
    }

    public void StartDialogue()
    {
        StopAllCoroutines();
        textComponent.text = string.Empty;

        int lastValidIndex = gameManagerRef.currentScript.lines.Count - 1;
        if (gameManagerRef.currentScript.finalLine > lastValidIndex)
        {
            Debug.LogWarning("Dialogue: script " + gameManagerRef.currentScript.scriptID +
                " has finalLine = " + gameManagerRef.currentScript.finalLine +
                " but its last valid line index is " + lastValidIndex + ". Clamping it. Fix the JSON.");
        }

        index = 0;
        scriptFinished = false;
        rightID = gameManagerRef.currentScript.lines[index].rightCharacterID;
        leftID = gameManagerRef.currentScript.lines[index].leftCharacterID;
        StartCoroutine(TypeLine());
        PeopleCheck();
        CheckCharacter();
    }

    void CheckCharacter()
    {
        if (gameManagerRef.currentScript.lines[index].direction == "left")
        {
            Focus(leftCharacter);
            Unfocus(rightCharacter);
        }
        else if (gameManagerRef.currentScript.lines[index].direction == "right")
        {
            Focus(rightCharacter);
            Unfocus(leftCharacter);
        }
        else if (gameManagerRef.currentScript.lines[index].direction == "both")
        {
            Focus(leftCharacter);
            Focus(rightCharacter);
        }
        else
        {
            Unfocus(leftCharacter);
            Unfocus(rightCharacter);
        }
    }

    void Focus(Image focus)
    {
        focus.rectTransform.sizeDelta = new Vector2(focusScale, focusScale);
    }

    void Unfocus(Image unfocus)
    {
        unfocus.rectTransform.sizeDelta = new Vector2(unfocusScale, unfocusScale);
    }

    void PeopleCheck()
    {
        //Right Character Check
        if (rightID == 0)
        {
            rightCharacter.gameObject.SetActive(false);
        }
        else if (rightID == 1)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = detective;
        }
        else if (rightID == 2)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = nurse;
        }
        else if (rightID == 3)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = brood;
        }
        else if (rightID == 4)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = butcher;
        }
        else if (rightID == 5)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverend;
        }
        else if (rightID == 6)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = mayor;
        }
        else if (rightID == 7)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = employee;
        }
        else if (rightID == 8)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = sheriff;
        }
        else if (rightID == 9)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = deacon;
        }
        else if (rightID == 10)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = bootlegger;
        }
        else if (rightID == 11)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = mayorsWife;
        }
        else if (rightID == 12)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = monster;
        }

        //Left Character Check
        if (leftID == 0)
        {
            leftCharacter.gameObject.SetActive(false);
        }
        else if (leftID == 1)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = detective;
        }
        else if (leftID == 2)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = nurse;
        }
        else if (leftID == 3)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = brood;
        }
        else if (leftID == 4)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = butcher;
        }
        else if (leftID == 5)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = reverend;
        }
        else if (leftID == 6)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = mayor;
        }
        else if (leftID == 7)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = employee;
        }
        else if (leftID == 8)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = sheriff;
        }
        else if (leftID == 9)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = deacon;
        }
        else if (leftID == 10)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = bootlegger;
        }
        else if (leftID == 11)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = mayorsWife;
        }
        else if (leftID == 12)
        {
            leftCharacter.gameObject.SetActive(true);
            leftCharacter.sprite = monster;
        }
    }

    void Overlap()
    {
        if (gameManagerRef.currentScript.lines[index].direction != gameManagerRef.currentScript.lines[preIndex].direction)
        {
            CheckCharacter();
        }
    }

    void updatePeople()
    {
        leftID = gameManagerRef.currentScript.lines[index].leftCharacterID;
        rightID = gameManagerRef.currentScript.lines[index].rightCharacterID;
    }

    IEnumerator TypeLine()
    {
        //gradually typing lines
        foreach (char c in gameManagerRef.currentScript.lines[index].displayLine)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator waitAndEnd()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    void NextLine()
    {
        int lastLine = Mathf.Min(gameManagerRef.currentScript.finalLine, gameManagerRef.currentScript.lines.Count - 1);

        if (index < lastLine)
        {
            preIndex = index;
            index++;
            textComponent.text = string.Empty;
            updatePeople();
            PeopleCheck();
            StartCoroutine(TypeLine());
            Overlap();
        }
        else
        {
            scriptFinished = true;
            gameManagerRef.showButtons();
            if (gameManagerRef.allButtonsInactive())
            {
                StartCoroutine(waitAndEnd());
            }
        }
    }
}