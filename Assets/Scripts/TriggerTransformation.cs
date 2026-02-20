using UnityEngine;

public class TriggerTransformation : MonoBehaviour
{
  
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.RevertForm.Invoke();
        }
    }
}
