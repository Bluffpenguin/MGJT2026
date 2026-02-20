using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
	bool playerInRange = false;
	GameObject player;
	[SerializeField] GameObject prompt;
	[SerializeField] AudioClip failSFX, digSFX;
	[SerializeField] Transform digVFXTransform;
	[SerializeField] Transform siblingHole;
	[SerializeField] float transitionTime = 1;


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
			if (player.GetComponent<Movement>().currentForm == Movement.Form.Shovel)
			{
				Dig();
			}
			else
			{
				AudioManager.instance.UI_One_Shot(failSFX, 1);
			}
		}
	}

	void Dig()
	{
		// Temp
		StartCoroutine(DigTransitionFromSelf());
	}

	IEnumerator DigTransitionFromSelf()
	{
		EventManager.FreezePlayer.Invoke();
		digVFXTransform.position = transform.position;
		digVFXTransform.localScale = Vector3.zero;
		Vector3 startScale = digVFXTransform.localScale;
		Vector3 endScale = Vector3.one * 0.65f;
		digVFXTransform.gameObject.SetActive(true);
		
		float elaspedTime = 0;

		while (elaspedTime < transitionTime)
		{
			float t = elaspedTime / transitionTime;

			digVFXTransform.localScale = Vector3.Lerp(startScale, endScale, t);
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		digVFXTransform.localScale = endScale;

		EventManager.HidePlayer.Invoke();

		elaspedTime = 0;

		while (elaspedTime < transitionTime)
		{
			float t = elaspedTime / transitionTime;

			digVFXTransform.localScale = Vector3.Lerp(endScale, startScale, t);
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		digVFXTransform.localScale = Vector3.zero;
		Debug.Log("finished transform");
		digVFXTransform.gameObject.SetActive(false);
		StartCoroutine(DigTransitionToSibling());
	}

	IEnumerator DigTransitionToSibling()
	{
		
		digVFXTransform.position = siblingHole.position;
		digVFXTransform.localScale = Vector3.zero;
		Vector3 startScale = digVFXTransform.localScale;
		Vector3 endScale = Vector3.one * 0.65f;
		digVFXTransform.gameObject.SetActive(true);
		player.transform.position = siblingHole.position;

		float elaspedTime = 0;

		while (elaspedTime < transitionTime)
		{
			float t = elaspedTime / transitionTime;

			digVFXTransform.localScale = Vector3.Lerp(startScale, endScale, t);
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		digVFXTransform.localScale = endScale;

		EventManager.ShowPlayer.Invoke();
		

		elaspedTime = 0;

		while (elaspedTime < transitionTime)
		{
			float t = elaspedTime / transitionTime;

			digVFXTransform.localScale = Vector3.Lerp(endScale, startScale, t);
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		digVFXTransform.localScale = Vector3.zero;
		Debug.Log("finished transform");
		digVFXTransform.gameObject.SetActive(false);
		EventManager.UnfreezePlayer.Invoke();
	}

	IEnumerator DiggingSound()
	{
		float elapsedTime = 0;
		float sfxSpacing = 0;
		while (elapsedTime < transitionTime*8)
		{
			elapsedTime += Time.deltaTime;
			sfxSpacing += Time.fixedDeltaTime;
			if (sfxSpacing >= transitionTime)
			{
				sfxSpacing = 0;
				AudioManager.instance.UI_One_Shot(digSFX, 1);
			}
			yield return null;
		}
	}
}
