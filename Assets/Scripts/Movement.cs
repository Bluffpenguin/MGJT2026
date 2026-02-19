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
    public enum Form
    {
        Man, Shovel, Brakes, Fly, Apathy, Frog, Airplane, Hammer
    }
    [SerializeField] public Form currentForm = Form.Man;
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

    [Header("Hammer Movement")]
    [SerializeField] float hamRotSpeedX=2, hamRotSpeedZ=2;

    private void Awake()
	  {
        // Listeners
        EventManager.FreezePlayer.AddListener(freezeMovement);
		EventManager.UnfreezePlayer.AddListener(unfreezeMovement);
        EventManager.Transformation.AddListener(Transform);
        EventManager.ShowPlayer.AddListener(ShowPlayer);
        EventManager.HidePlayer.AddListener(HidePlayer);

        // Components
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
        //Debug
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            Transform("shovel");
        }
		if (Keyboard.current.iKey.wasPressedThisFrame)
		{
			Transform("hammer");
		}
		if (Keyboard.current.oKey.wasPressedThisFrame)
		{
			Transform("frog");
		}
		if (Keyboard.current.yKey.wasPressedThisFrame)
		{
			Transform("man");
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
        FormMovemenUpdate(move);


    }

    void freezeMovement()
    {
        canMove = false;
		rb.linearVelocity.Set(0, 0);
	}

	void unfreezeMovement()
	{
		canMove = true;
	}

	#region Transformation

	void Transform(string newForm)
    {
        transforming = true;
        rb.linearVelocity.Set(0, 0);
        
        switch(newForm)
        {
            case "man":
                currentForm = Form.Man;
                break;
            case "shovel":
                Debug.Log("Transform into shovel");
                currentForm = Form.Shovel;
                break;
            case "fly":
                currentForm = Form.Fly;
                break;
            case "airplane":
                currentForm = Form.Airplane;
                break;
            case "brakes":
                currentForm = Form.Brakes;
                break;
            case "apathy":
                currentForm = Form.Apathy;
                break;
            case "frog":
                currentForm = Form.Frog;
                break;
            case "hammer":
                currentForm = Form.Hammer;
                break;
            default: 
                break;
        }
        shineVFXTransform = Instantiate(shineVFXPrefab, spriteRenderer.transform.position, Quaternion.identity).transform;
        StartCoroutine("TransformTrasition");
    }

    void ApplyTransform()
    {
		spriteRenderer.transform.localRotation = Quaternion.identity;
		switch (currentForm)
		{
			case Form.Man:
                spriteRenderer.sprite = transformSprites[0];
				break;
			case Form.Shovel:
                spriteRenderer.sprite = transformSprites[1];
				break;
			case Form.Brakes:
                spriteRenderer.sprite = transformSprites[2];
				break;
			case Form.Fly:
				spriteRenderer.sprite = transformSprites[3];
				break;
			case Form.Apathy:
				spriteRenderer.sprite = transformSprites[4];
				break;
			case Form.Frog:
				spriteRenderer.sprite = transformSprites[5];
				break;
			case Form.Airplane:
				spriteRenderer.sprite = transformSprites[6];
				break;
            case Form.Hammer:
				spriteRenderer.sprite = transformSprites[7];
				break;
            default:
                break;
		}
	}

    void FormMovemenUpdate(Vector2 move)
    {
		switch (currentForm)
		{
			case Form.Man:
				
				break;
			case Form.Shovel:
				
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
			case Form.Hammer:
				if (move.x > 0.1)
                {
                    spriteRenderer.transform.Rotate(Vector3.forward, -hamRotSpeedZ, Space.Self);
                }
                else if (move.x < -0.1)
                {
					spriteRenderer.transform.Rotate(Vector3.forward, hamRotSpeedZ, Space.Self);
				}

                if (move.y > 0.1)
                {
					spriteRenderer.transform.Rotate(Vector3.left, hamRotSpeedX, Space.Self);
				}
				else if (move.y < -0.1)
				{
					spriteRenderer.transform.Rotate(Vector3.left, -hamRotSpeedX, Space.Self);
				}
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

    void ShowPlayer()
    {
        spriteRenderer.enabled = true;
    }
    
    void HidePlayer()
    {
        spriteRenderer.enabled = false;
    }
	#endregion
}
