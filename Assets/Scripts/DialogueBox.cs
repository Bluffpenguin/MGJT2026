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
    [SerializeField] Dialogue currentDialogue;

    [Header("Debug")]
    [SerializeField] Dialogue debugMessage;

    private void Awake()
	{
		instance = this;
        EventManager.OpenDialogueBox.AddListener(OpenMenu);
        EventManager.CloseDialogueBox.AddListener(CloseMenu);
        textSpeed = TEXT_SPEED;
		foreach (var choice in choiceButtons)
		{
			choice.gameObject.SetActive(false);
		}
        dialoguePanel.gameObject.SetActive(false);
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        textBox.text = string.Empty;

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
            if (textSpeed != TEXT_SPEED / 2)
            {
                textSpeed = TEXT_SPEED / 2;
            }
        }
	}

    void OpenMenu(Dialogue dialogue)
    {
        opened = true;
        finishedConversation = false;
		dialoguePanel.SetActive(true);
        currentDialogue = dialogue;
            
		PopulateText();
    }

    void CloseMenu()
    {
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
	}

    void PopulateChoices(Dialogue dialogue)
    {
        StartDialogue(dialogue.text);

        for (int i = 0; i < dialogue.choices.Count && i < choiceButtons.Length; i++)
        {
            // Check if the option has a prerequisite
            if (dialogue.choices[i].prerequisite != "")
            {
				choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "???";
				choiceButtons[i].interactable = false;
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
		}
    }

    void PopulatePlain(Dialogue dialogue)
    {
        StartDialogue(dialogue.text);
        if (dialogue.next == null)
        {
            finishedConversation = true;
            dialogue.endEvent.Invoke();
        }
        
    }

    void ProcessDialogueChoice1() { ProcessChoice(0); }
	void ProcessDialogueChoice2() { ProcessChoice(1); }
	void ProcessDialogueChoice3() { ProcessChoice(2); }
	void ProcessDialogueChoice4() { ProcessChoice(3); }

	void ProcessChoice(int index)
    {
    
        // Reset buttons
        foreach (var choice in choiceButtons)
        {
            choice.onClick.RemoveAllListeners();
            choice.gameObject.SetActive(false);
        }

        currentDialogue = currentDialogue.choices[index].next;
        PopulateText();
    }

	void StartDialogue(string dialogue)
	{
		if (AudioManager.instance) AudioManager.instance.TW_Checker();
		StopAllCoroutines();
		textBox.text = string.Empty;
		StartCoroutine(TypeLine(dialogue));
	}

	IEnumerator TypeLine(string dialogue)
	{
		finishedDialogue = false;
		foreach (char c in dialogue.ToCharArray())
		{
			textBox.text += c;
			yield return new WaitForSeconds(textSpeed);
		}
		if (AudioManager.instance) AudioManager.instance.TW_Stop();
		yield return new WaitForSeconds(0.5f);
		finishedDialogue = true;
        
	}

}
