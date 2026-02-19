using System.Collections;
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

    // Transformations
    enum Form
    {
        Man, Shovel, Brakes, Fly, Apathy, Frog, Airplane
    }
    [SerializeField] Form currentForm = Form.Man;
    [SerializeField] bool transforming = false;
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] transformSprites; // Keep in the same order as the Form enum

    // Transformation Transition
    [Header("Transform Transition")]
    [SerializeField] GameObject shineVFXPrefab;
    [SerializeField] Transform shineVFXTransform;
	[SerializeField] float startingScale = 0;
    [SerializeField] float finalScale = 0.4f;
    [SerializeField] float transformTime = 1;

    private void Awake()
	  {
        // Listeners
        EventManager.FreezePlayer.AddListener(freezeMovement);
		EventManager.UnfreezePlayer.AddListener(unfreezeMovement);
        EventManager.Transformation.AddListener(Transform);

        // Components
        spriteRenderer = GetComponent<SpriteRenderer>();
	  }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        interaction.Enable();
        rb = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	private void Update()
	{
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            Transform("Shovel");
        }
        if (transforming) canMove = false;
		if (!canMove) return;

		if (interaction.WasPressedThisFrame())
		{
			EventManager.PlayerTalk.Invoke();
		}
	}
	void FixedUpdate()
    {
        if (!canMove) return; // Anything below this will not run if canMove is false
        
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

    void Transform(string newForm)
    {
        transforming = true;
        switch(newForm)
        {
            case "Man":
                currentForm = Form.Man;
                break;
            case "Shovel":
                Debug.Log("Transform into shovel");
                currentForm = Form.Shovel;
                break;
            case "Fly":
                currentForm = Form.Fly;
                break;
            case "Airplane":
                currentForm = Form.Airplane;
                break;
            case "Brakes":
                currentForm = Form.Brakes;
                break;
            case "Apathy":
                currentForm = Form.Apathy;
                break;
            default: 
                break;
        }
        shineVFXTransform = Instantiate(shineVFXPrefab, transform.position, Quaternion.identity).transform;
        StartCoroutine("TransformTrasition");
    }

    void ApplyTransform()
    {
		switch (currentForm)
		{
			case Form.Man:
                spriteRenderer.sprite = transformSprites[0];
				break;
			case Form.Shovel:
                spriteRenderer.sprite = transformSprites[1];
				break;
			case Form.Brakes:
				break;
			case Form.Fly:
				break;
			case Form.Apathy:
				break;
			case Form.Frog:
				break;
			case Form.Airplane:
				break;
            default:
                break;
		}
	}

	IEnumerator TransformTrasition()
    {
        shineVFXTransform.localScale = Vector3.zero;
        Vector3 startScale = shineVFXTransform.localScale;
        Vector3 endScale = Vector3.one * finalScale;
        float elaspedTime = 0;
        
        while (elaspedTime < transformTime)
        {
            float t = elaspedTime / transformTime;

            shineVFXTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            elaspedTime += Time.deltaTime;
            yield return null;
        }
        shineVFXTransform.localScale = endScale;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("First Half");
        ApplyTransform();

		elaspedTime = 0;

		while (elaspedTime < transformTime)
		{
			float t = elaspedTime / transformTime;

			shineVFXTransform.localScale = Vector3.Lerp(endScale, startScale, t);
			elaspedTime += Time.deltaTime;
			yield return null;
		}
		shineVFXTransform.localScale = Vector3.zero;
        Debug.Log("finished transform");
        transforming = false;
        canMove = true;
        Destroy(shineVFXTransform.gameObject);
	}
}
