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
    public string[] lines;
    public int[] focus;
    public int people;
    public int focusScale;
    public int unfocusScale;
    public float textSpeed;
   

    private int index;
    private int preIndex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
        PeopleCheck();
        CheckCharacter();
    }

    void CheckCharacter()
    {
        if (focus[index] == 0)
        {
            Focus(leftCharacter);
            Unfocus(rightCharacter);
        }
        else
        {
            Focus(rightCharacter);
            Unfocus(leftCharacter);
        }
    }

    void Focus(Image focus)
    {
        focus.color = new Color(200, 0, 0);
        focus.rectTransform.sizeDelta = new Vector2(focusScale, focusScale);
    }

    void Unfocus(Image unfocus)
    {
        unfocus.color = new Color(0, 0, 200);
        unfocus.rectTransform.sizeDelta = new Vector2(unfocusScale, unfocusScale);
    }

    void PeopleCheck()
    {
        if (people == 1)
        {
            rightCharacter.enabled = false;
        }
    }

    void Overlap()
    {
        if (focus[index] != focus[preIndex])
        {
            CheckCharacter();
        }
    }
    
       IEnumerator TypeLine()
    {
        //gradually typing lines
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            preIndex = index;
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            Overlap();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}