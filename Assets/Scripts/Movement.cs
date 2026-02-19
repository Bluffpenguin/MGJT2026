using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public InputAction MoveAction, interaction;
    bool canMove = true;
    public float speed = 5f;
    private Rigidbody2D rb;

    private void Awake()
	  {
        EventManager.FreezePlayer.AddListener(freezeMovement);
		EventManager.UnfreezePlayer.AddListener(unfreezeMovement);
	  }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        interaction.Enable();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

   void FixedUpdate()
    {
        if (!canMove) return; // Anything below this will not run if canMove is false
        if (interaction.WasPressedThisFrame())
        {
            EventManager.PlayerTalk.Invoke();
        }
        Vector2 move = MoveAction.ReadValue<Vector2>();
        

        rb.linearVelocity = move * speed;


    }

    void freezeMovement()
    {
        canMove = false;
    }

	void unfreezeMovement()
	{
		canMove = true;
	}
}
