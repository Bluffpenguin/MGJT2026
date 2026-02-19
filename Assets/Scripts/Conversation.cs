using UnityEngine;

public class Conversation : MonoBehaviour
{
    public Dialogue dialogue;
    bool talkingToPlayer = false, playerInRange = false;
	[SerializeField] GameObject prompt;

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
        EventManager.OpenDialogueBox.Invoke(dialogue);
        EventManager.FreezePlayer.Invoke();


    }

}
