using UnityEngine;

public class Conversation : MonoBehaviour
{
    public Dialogue dbox;

    private bool triggerActive = false;

    // Reference to the prefab. Drag a prefab into this field in the Inspector.
    public GameObject myPrefab;
    void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        EventManager.OpenDialogueBox.Invoke(dbox);
        if (other.CompareTag("Player"))
        {
            triggerActive = true;
       
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            triggerActive = false;
        }
    }

    private void Update()
    {
        if (triggerActive)
        {
            Instantiate(myPrefab);
        }
    }
}
