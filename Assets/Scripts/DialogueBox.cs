using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox instance;

    [SerializeField] internal Button[] choiceButtons;
    [SerializeField] internal TextMeshProUGUI textBox, speakerText;
    [SerializeField] internal GameObject dialoguePanel;

    
    [SerializeField, Range(0.01f, 0.4f)] internal float TEXT_SPEED = 1f;
    [HideInInspector] internal float textSpeed;

    private bool ConfirmationInput()
    {
        return (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame);

	}

    bool finishedDialogue = true;
    bool finishedConversation = true;
    bool opened = false;
    bool fastText = false;
    public bool IsOpen
    {
        get { return opened; }
    }

    Dialogue currentDialogue;

    [Header("Dialogue Bob")]
    [SerializeField] float bobVariance = 4;
    float defBoxY;
    float[] defChoicesY = new float[5];
    [SerializeField] float bobSpeed = 8;
    [SerializeField] float choiceOffsetY = 50;
	[SerializeField] float choiceOffsetX = 100;
	bool goingUp = false;



	[Header("Debug")]
    [SerializeField] Dialogue debugMessage;

    private void Awake()
	{
		instance = this;
		
        EventManager.OpenDialogueBox.AddListener(OpenMenu);
        EventManager.CloseDialogueBox.AddListener(CloseMenu);
        textSpeed = TEXT_SPEED;
        for (int i = 0; i <choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
            defChoicesY[i] = choiceButtons[i].transform.position.y;

		}
        dialoguePanel.gameObject.SetActive(false);
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        textBox.text = string.Empty;
        defBoxY = dialoguePanel.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor && Keyboard.current.pKey.wasPressedThisFrame)
        {
            OpenMenu(debugMessage);
        } 

        if (!opened) return;

        // Player Input
        if (finishedDialogue && finishedConversation && ConfirmationInput())
        {
            finishedDialogue = false;
            EventManager.CloseDialogueBox.Invoke();
        }
        else if (finishedDialogue && ConfirmationInput() && currentDialogue.choices.Count == 0)
        {
            currentDialogue = currentDialogue.next;
            PopulateText();
        }
        else if (ConfirmationInput())
        {
            // Speed Up Text
            if (!fastText)
            {
                textSpeed = TEXT_SPEED / 3;
                fastText = true;
            }
            else
            {
                textSpeed = 0;
            }
        }

        // Dialogue Box Bobing
        if (goingUp)
        {
            dialoguePanel.transform.position += new Vector3(0, bobSpeed * Time.deltaTime, 0);
        }
        else
        {
            dialoguePanel.transform.position -= new Vector3(0, bobSpeed * Time.deltaTime, 0);
        }

        if (dialoguePanel.transform.position.y < defBoxY - bobVariance)
        {
            goingUp = true;
        }
        else if (dialoguePanel.transform.position.y > defBoxY + bobVariance)
        {
            goingUp = false;
        }
	}

    void OpenMenu(Dialogue dialogue)
    {
        opened = true;
        finishedConversation = false;
		dialoguePanel.SetActive(true);
        currentDialogue = dialogue;
        EventManager.FreezePlayer.Invoke();    
		PopulateText();
    }

    void CloseMenu()
    {
        StopAllCoroutines();

        // Check for transformations
        if (currentDialogue.transformation != "")
        {
            EventManager.Transformation.Invoke(currentDialogue.transformation);
        }
        else EventManager.UnfreezePlayer.Invoke();

            opened = false;
        dialoguePanel.SetActive(false);
		foreach (var choice in choiceButtons)
		{
			choice.onClick.RemoveAllListeners();
            choice.gameObject.SetActive(false);
		}
	}

    void PopulateText()
    {
		speakerText.text = currentDialogue.speakerName;
		textSpeed = TEXT_SPEED;
		if (currentDialogue.choices.Count != 0)
		{
			PopulateChoices(currentDialogue);
		}
        else
        {
            PopulatePlain(currentDialogue);
        }

        CheckDialogueEvents();
	}

    void PopulateChoices(Dialogue dialogue)
    {
        StartDialogue(dialogue.text);

        for (int i = 0; i < dialogue.choices.Count && i < choiceButtons.Length; i++)
        {
            choiceButtons[i].transform.position += new Vector3(UnityEngine.Random.Range(-choiceOffsetX, choiceOffsetX), UnityEngine.Random.Range(-choiceOffsetY, choiceOffsetY));
            // Check if the option has a prerequisite
            if (dialogue.choices[i].prerequisite != "")
            {
                // Already learned word
                if (KeywordLibrary.instance.Check(dialogue.choices[i].prerequisite))
                {
					choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialogue.choices[i].choiceText;
					choiceButtons[i].interactable = true;
				}
                else
                {
					choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "???";
					choiceButtons[i].interactable = false;
				}
                    
			}
            else
            {
				choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialogue.choices[i].choiceText;
                choiceButtons[i].interactable = true;
			}
                
            choiceButtons[i].gameObject.SetActive(true);
            if (i == 0) choiceButtons[0].onClick.AddListener(ProcessDialogueChoice1);
			if (i == 1) choiceButtons[1].onClick.AddListener(ProcessDialogueChoice2);
			if (i == 2) choiceButtons[2].onClick.AddListener(ProcessDialogueChoice3);
			if (i == 3) choiceButtons[3].onClick.AddListener(ProcessDialogueChoice4);
			if (i == 4) choiceButtons[4].onClick.AddListener(ProcessDialogueChoice5);
		}
    }

    void PopulatePlain(Dialogue dialogue)
    {
        StartDialogue(dialogue.text);
        if (dialogue.next == null)
        {
            finishedConversation = true;
            //dialogue.endEvent.Invoke();
        }
        
    }

    void ProcessDialogueChoice1() { ProcessChoice(0); }
	void ProcessDialogueChoice2() { ProcessChoice(1); }
	void ProcessDialogueChoice3() { ProcessChoice(2); }
	void ProcessDialogueChoice4() { ProcessChoice(3); }
	void ProcessDialogueChoice5() { ProcessChoice(4); }

	void ProcessChoice(int index)
    {
    
        // Reset buttons
        foreach (var choice in choiceButtons)
        {
            choice.onClick.RemoveAllListeners();
            choice.gameObject.SetActive(false);
        }

        if (currentDialogue.choices[index].next != null)
        {
			currentDialogue = currentDialogue.choices[index].next;
			PopulateText();
		}
        else
        {
			EventManager.CloseDialogueBox.Invoke();
		}
        
    }

	void StartDialogue(string dialogue)
	{
		if (AudioManager.instance) AudioManager.instance.TW_Checker();
        fastText = false;
		StopAllCoroutines();
		textBox.text = string.Empty;
		StartCoroutine(TypeLine(dialogue));
	}

    void CheckDialogueEvents()
    {
        // Check for word gain
        if (currentDialogue.wordUnlock != "")
        {
            KeywordLibrary.LearnWord.Invoke(currentDialogue.wordUnlock);
        }
    }

	IEnumerator TypeLine(string dialogue)
	{
		finishedDialogue = false;
        bool printingHtmlfunc = false;
		foreach (char c in dialogue.ToCharArray())
		{

            textBox.text += c;

            //Check for HTML functions
			if (c == '<')
			{
                printingHtmlfunc = true;
			}

            if (c == '>')
            {
                printingHtmlfunc = false;
            }

            // Don't space html functions
			if (!printingHtmlfunc) yield return new WaitForSeconds(textSpeed);
		}
		if (AudioManager.instance) AudioManager.instance.TW_Stop();
		yield return new WaitForSeconds(0.5f);
		finishedDialogue = true;
        
	}

}
