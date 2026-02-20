using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreens : MonoBehaviour
{
	[SerializeField] GameObject canvas1, canvas2, canvas3, canvas4;
	GameObject selectedCanvas;
	[SerializeField] Image fade;

	private void Awake()
	{
        EventManager.RealStay.AddListener(Ending1);
        EventManager.RealGo.AddListener(Ending2);
        EventManager.FakeStay.AddListener(Ending3);
        EventManager.FakeGo.AddListener(Ending4);
	}
	
	void Ending1()
	{
		StartCoroutine(FadeToBlack()); ;
		selectedCanvas = canvas1;
		
	}

	void Ending2()
	{
		StartCoroutine(FadeToBlack());
		selectedCanvas = canvas2;
	}

	void Ending3()
	{
		StartCoroutine(FadeToBlack());
		selectedCanvas = canvas3;
	}

	void Ending4()
	{
		StartCoroutine(FadeToBlack());
		selectedCanvas = canvas4;
	}

	IEnumerator FadeToBlack()
	{
		Color c = fade.color;
		c.a = 0;
		fade.color = c;
		fade.enabled = true;
		EventManager.FreezePlayer.Invoke();
		float elapsedTime = 0;
		while (elapsedTime < 5)
		{
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / 5;
			c.a = 1 * t;
			fade.color = c;
			yield return null;
		}
		c.a = 1;
		fade.color = c;
		fade.enabled = false;
		selectedCanvas.SetActive(true);
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene("MainMenu");
	}
}
