using UnityEngine;
using UnityEngine.Events;

static public class EventManager 
{
    // Dialogue Menu
    static public UnityEvent<Dialogue> OpenDialogueBox = new(); // Takes a dialogue scriptable object
    static public UnityEvent CloseDialogueBox = new();

    // Dialogue Choices
    static public UnityEvent<string> transformation = new();
}
