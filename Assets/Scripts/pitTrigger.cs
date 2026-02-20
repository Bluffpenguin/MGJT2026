using System.Collections;
using UnityEngine;
using static System.TimeZoneInfo;

public class pitTrigger : MonoBehaviour
{
	GameObject player;
	[SerializeField] float transitionTime = 3;
	[SerializeField] float fallRotSpeed = 4;
	[SerializeField] Transform respawnPoint;
	[SerializeField] GameObject pitBlocker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			player = other.gameObject;
			Movement.Form playerForm = player.GetComponent<Movement>().currentForm;

			//Cannot cross the pit safely
			if (playerForm != Movement.Form.Airplane && playerForm != Movement.Form.Fly)
			{
				//Fall into the pit
				StartCoroutine(FallIntoPit());
			}

		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			//player = null;
		}
	}

	IEnumerator FallIntoPit()
	{
		pitBlocker.SetActive(true);
		EventManager.FreezePlayer.Invoke();

		Vector3 startScale = player.transform.localScale;
		Vector3 endScale = Vector3.zero;

		float elaspedTime = 0;

		while (elaspedTime < transitionTime)
		{
			float t = elaspedTime / transitionTime;

			player.transform.localScale = Vector3.Lerp(startScale, endScale, t);
			player.transform.Rotate(Vector3.left, fallRotSpeed, Space.Self);
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		player.transform.localScale = endScale;
		yield return new WaitForSeconds(1);

		player.transform.position = respawnPoint.position;
		player.transform.rotation = Quaternion.identity;
		player.transform.localScale = startScale;

		pitBlocker.SetActive(false);

		EventManager.UnfreezePlayer.Invoke();
	}
	
		
}
