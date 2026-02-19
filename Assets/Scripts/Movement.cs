using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public InputAction MoveAction, interaction;
    bool canMove = true;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return; // Anything below this will not run if canMove is false
        if (interaction.WasPressedThisFrame())
        {
            EventManager.PlayerTalk.Invoke();
        }
        Vector2 move = MoveAction.ReadValue<Vector2>();
        //Debug.Log(move);
        Vector2 position = (Vector2)transform.position + move * 0.04f;
        transform.position = position;

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
