using UnityEngine;

public class SheepleFinaleConvo : MonoBehaviour
{
	public Dialogue diag1, diag2;
	Dialogue currentDiag;
	bool talkingToPlayer = false, playerInRange = false;
	[SerializeField] GameObject prompt;

	private void Awake()
	{
		EventManager.CloseDialogueBox.AddListener(OnConversationEnd);
		EventManager.PlayerTalk.AddListener(OnConversationStart);
		EventManager.FinalePostIntro.AddListener(SwitchDialogue);

		currentDiag = diag1;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerInRange = true;
			prompt.SetActive(true);
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerInRange = false;
			prompt.SetActive(false);
		}
	}

	void OnConversationEnd()
	{
		if (!talkingToPlayer) return;

		talkingToPlayer = false;
		EventManager.UnfreezePlayer.Invoke();
	}

	void OnConversationStart()
	{
		if (!playerInRange || DialogueBox.instance.IsOpen) return;

		talkingToPlayer = true;
		EventManager.OpenDialogueBox.Invoke(currentDiag);
		EventManager.FreezePlayer.Invoke();


	}

	void SwitchDialogue()
	{
		currentDiag = diag2;
	}
}
