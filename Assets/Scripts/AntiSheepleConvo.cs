using UnityEngine;

public class AntiSheepleConvo : MonoBehaviour
{
    [SerializeField] Dialogue diag1, diag2, diag3;

	bool talkingToPlayer = false, playerInRange = false;
	GameObject player;
	enum Stage
	{
		Intro, Untransformed, Transformed
	}
	Stage currentStage = Stage.Intro;
	[SerializeField] GameObject prompt;
	bool waitingForDialogueToEnd = false;

	private void Awake()
	{
		EventManager.CloseDialogueBox.AddListener(OnConversationEnd);
		EventManager.PlayerTalk.AddListener(OnConversationStart);
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerInRange = true;
			prompt.SetActive(true);
			player = other.gameObject;
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
		if (!waitingForDialogueToEnd) EventManager.UnfreezePlayer.Invoke();
		if (waitingForDialogueToEnd)
		{
			if (KeywordLibrary.instance.Check("realStay"))
			{
				EventManager.RealStay.Invoke();
			}
			else if (KeywordLibrary.instance.Check("realGo"))
			{
				EventManager.RealGo.Invoke();
			}
			else if (KeywordLibrary.instance.Check("fakeStay"))
			{
				EventManager.FakeStay.Invoke();
			}
			else if (KeywordLibrary.instance.Check("fakeGo"))
			{
				EventManager.FakeGo.Invoke();
			}
		}
	}

	void OnConversationStart()
	{
		if (!playerInRange || DialogueBox.instance.IsOpen) return;

		talkingToPlayer = true;
		EventManager.FreezePlayer.Invoke();

		if (player.GetComponent<Movement>().currentForm != Movement.Form.Man) currentStage = Stage.Transformed;

		switch (currentStage)
		{
			case Stage.Intro:
				EventManager.OpenDialogueBox.Invoke(diag1);
				EventManager.FinalePostIntro.Invoke();
				currentStage = Stage.Untransformed;
				
				break;
			case Stage.Untransformed:
				EventManager.OpenDialogueBox.Invoke(diag2);
				break;
			case Stage.Transformed:
				EventManager.OpenDialogueBox.Invoke(diag3);
				waitingForDialogueToEnd = true;
				break;
		}
		
		
		


	}
}
