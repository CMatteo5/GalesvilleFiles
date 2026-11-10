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
    public SpriteRenderer rightSprite;
    public SpriteRenderer leftSprite;
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


    GameManager gameManagerRef { get { return GameManager.Instance; } }


    public GameObject dialogueStuff;

    int startFrame = -1;



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
    public Sprite reverseDetective;
    public Sprite reverseNurse;
    public Sprite reverseBrood;
    public Sprite reverseButcher;
    public Sprite reverseReverend;
    public Sprite reverseMayor;
    public Sprite reverseEmployee;
    public Sprite reverseSheriff;
    public Sprite reverseDeacon;
    public Sprite reverseBootlegger;
    public Sprite reverseMayorsWife;
    public Sprite reverseMonster;


    public int index;
    public int preIndex;

    public AudioSource audioSoruce;
    string soundPath;

    void Awake()
    {
        rightSprite = rightCharacter.GetComponent<SpriteRenderer>();
        leftSprite = leftCharacter.GetComponent<SpriteRenderer>();
        scriptFinished = true;
        if (dialogueStuff != null)
        {
            dialogueStuff.SetActive(false);
        }
        else
        {
            Debug.LogError("Dialogue: assign DialogueStuff to the Dialogue Stuff slot.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.frameCount != startFrame)
        {
            if (!scriptFinished && gameManagerRef != null && gameManagerRef.currentScript != null)
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
        if (dialogueStuff != null)
        {
            dialogueStuff.SetActive(true);
        }
        startFrame = Time.frameCount;
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
        playLineSound();
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
            rightCharacter.sprite = reverseDetective;
        }
        else if (rightID == 2)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseNurse;
        }
        else if (rightID == 3)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseBrood;
        }
        else if (rightID == 4)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseButcher;
        }
        else if (rightID == 5)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseReverend;
        }
        else if (rightID == 6)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseReverend;
        }
        else if (rightID == 7)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseEmployee;
        }
        else if (rightID == 8)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseSheriff;
        }
        else if (rightID == 9)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseDeacon;
        }
        else if (rightID == 10)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseBootlegger;
        }
        else if (rightID == 11)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseMayorsWife;
        }
        else if (rightID == 12)
        {
            rightCharacter.gameObject.SetActive(true);
            rightCharacter.sprite = reverseMonster;
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
        scriptFinished = true;

        if (dialogueStuff != null)
        {
            dialogueStuff.SetActive(false);
        }
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
            playLineSound();
            Overlap();
        }
        else
        {
            gameManagerRef.showButtons();
            if (gameManagerRef.allButtonsInactive())
            {
                StartCoroutine(waitAndEnd());
            }
        }
    }

    void playLineSound()
    {
        if (gameManagerRef.currentScript.lines[index].sfx != "")
        {
            soundPath = gameManagerRef.currentScript.lines[index].sfx;
            AudioClip temp = Resources.Load<AudioClip>(soundPath);
            audioSoruce.PlayOneShot(temp);
            }
    }
}