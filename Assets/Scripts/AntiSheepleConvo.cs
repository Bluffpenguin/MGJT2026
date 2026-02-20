using System.Collections;
using Unity.VisualScripting;
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
	[SerializeField] GameObject prompt, shineVFX;
	[SerializeField] Transform shineVFXTransform;
	[SerializeField] AudioClip transformSFX;
	float finalScale = 0.3f;
	float transformTime = 0.75f;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Sprite blobSprite;
	bool waitingForDialogueToEnd = false;

	private void Awake()
	{
		EventManager.CloseDialogueBox.AddListener(OnConversationEnd);
		EventManager.PlayerTalk.AddListener(OnConversationStart);
		EventManager.BecomeBlob.AddListener(BecomeBlob);

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

	void BecomeBlob()
	{
		AudioManager.instance.UI_One_Shot(transformSFX, 1);
		StartCoroutine(BeBlob());
	}

	IEnumerator BeBlob()
	{
		shineVFXTransform.localScale = Vector3.zero;
		shineVFX.SetActive(true);
		Vector3 startScale = shineVFXTransform.localScale;
		Vector3 endScale = Vector3.one * finalScale;
		float elaspedTime = 0;

		while (elaspedTime < transformTime)
		{
			float t = elaspedTime / transformTime;

			shineVFXTransform.localScale = Vector3.Lerp(startScale, endScale, t);
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		shineVFXTransform.localScale = endScale;
		yield return new WaitForSeconds(0.5f);
		Debug.Log("First Half");
		spriteRenderer.sprite = blobSprite;

		elaspedTime = 0;

		while (elaspedTime < transformTime)
		{
			float t = elaspedTime / transformTime;

			shineVFXTransform.localScale = Vector3.Lerp(endScale, startScale, t);
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		shineVFXTransform.localScale = Vector3.zero;
		shineVFX.SetActive(false);
		
	}
}
