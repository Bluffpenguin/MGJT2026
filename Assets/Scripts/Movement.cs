using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public InputAction MoveAction;
    public float speed = 5f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

   void FixedUpdate()
    {
        Vector2 move = MoveAction.ReadValue<Vector2>();

        rb.linearVelocity = move * speed;
        
        //Debug.Log(move);
        //Vector2 position = (Vector2)transform.position + move * 0.04f;
        //transform.position = position;

    }
}
