using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{
	bool playerInRange = false;
	GameObject player;
	[SerializeField] GameObject prompt;
	[SerializeField] AudioClip failSFX;

	private void Awake()
	{
		EventManager.PlayerTalk.AddListener(OnInteract);
	}
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playerInRange = true;
			player = other.gameObject;
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

	void OnInteract()
	{
		if (playerInRange && !DialogueBox.instance.IsOpen)
		{
			if (player.GetComponent<Movement>().currentForm == Movement.Form.Hammer)
			{
				BreakRocks();
			}
			else
			{
				AudioManager.instance.UI_One_Shot(failSFX, 1);
			}
		}
	}

	void BreakRocks()
	{
		// Temp
		transform.parent.gameObject.SetActive(false);
	}
}
