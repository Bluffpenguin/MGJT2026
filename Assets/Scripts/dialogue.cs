using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "dialogue", menuName = "Dialogue/dialogueObj")]
public class Dialogue : ScriptableObject
{
    public string speakerName;
    [TextArea] public string text;

	[Header("For non-linear dialogue:")]
	[SerializeField] public List<Choice> choices;

	[Header("For linear dialogue:")]
	public Dialogue next;

	[Header("Dialogue Event")]
	public string wordUnlock;
	public string transformation;
}

[System.Serializable]
public struct Choice
{
    public Dialogue next;
    public string choiceText;
	[Header("Optional")]
	public string prerequisite;

}
