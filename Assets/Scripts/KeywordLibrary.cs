using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeywordLibrary : MonoBehaviour 
{
    public static KeywordLibrary instance;

	static public UnityEvent<string> LearnWord = new(); // Unlock a word

	Dictionary<string, bool> Keywords = new();
	[SerializeField] string[] words;

	private void Awake()
	{
		instance = this;

		// Add words to dictionary
		foreach (var word in words)
		{
			Keywords.Add(word, false);
		}

		LearnWord.AddListener(Learn);
	}

	// Return whether or not the words has been found
	public bool Check(string keyword)
	{
		return Keywords[keyword];
	}

	//Set the provided word to True/False
	public void Learn(string keyword)
	{
		Keywords[keyword] = true;
	}

	// The all words to False
	public void Reset()
	{
		foreach (string word in Keywords.Keys)
		{
			Keywords[word] = false;
		}
	}
}
