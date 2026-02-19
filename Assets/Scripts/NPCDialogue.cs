using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string npcName;
    public string[] dialogueLines;
    public float typingSpeed = 0.5f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
}
