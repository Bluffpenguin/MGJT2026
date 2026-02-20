using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{
	bool playerInRange = false;
	GameObject player;
	[SerializeField] GameObject prompt;
	[SerializeField] AudioClip failSFX, breakSFX;
	[SerializeField] Transform[] rockSprites;

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
		AudioManager.instance.UI_One_Shot(breakSFX, 1);
		foreach(var rock in rockSprites)
		{
			rock.localScale -= new Vector3(0.2f, 0.2f, 0.2f);
		}
		if (rockSprites[0].localScale.x <= 0.1)
		{
			transform.parent.gameObject.SetActive(false);
		}
	}
}
