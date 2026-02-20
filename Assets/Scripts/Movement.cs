using System.Collections;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditorInternal.ReorderableList;

public class Movement : MonoBehaviour
{
    public InputAction MoveAction, interaction;
    bool canMove = true;
    public float speed = 5f;
    private Rigidbody2D rb;
    private Animator animator;

    // Transformations
    public enum Form
    {
        Man, Shovel, Brakes, Fly, Apathy, Frog, Airplane, Hammer, Malamute, Wheelbarrow, Battleaxe, Sarmale, Potatoe
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
    [SerializeField] float hamRotSpeedX = 2;
    [SerializeField] float hamRotSpeedZ=2;

    [Header("Battleaxe Movement")]
    [SerializeField] Sprite altAxeSprite;
    [SerializeField] float spriteInterval = 0.5f;
    float elapsedTime = 0;

    [Header("Fly+Apathy Hover")]
    [SerializeField] float hoverSpeed = 1;
    [SerializeField] float bobOffset = 5;
    bool goingUp = false;
	Vector3 defSpritePosition;


	private void Awake()
	  {
        // Listeners
        EventManager.FreezePlayer.AddListener(freezeMovement);
		EventManager.UnfreezePlayer.AddListener(unfreezeMovement);
        EventManager.Transformation.AddListener(Transform);
        EventManager.ShowPlayer.AddListener(ShowPlayer);
        EventManager.HidePlayer.AddListener(HidePlayer);
        EventManager.RevertForm.AddListener(RevertForm);

        // Components
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	  }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        interaction.Enable();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        defSpritePosition = spriteRenderer.transform.localPosition;
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
        rb.linearVelocity = Vector2.zero;
	}

	void unfreezeMovement()
	{
		canMove = true;
	}

	#region Transformation

    void RevertForm()
    {
        if (currentForm != Form.Man)
        {
            Transform("man");
        }
    }

	void Transform(string newForm)
    {
        transforming = true;
        rb.linearVelocity.Set(0, 0);
        animator.enabled = false;
        
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
            case "wheelbarrow":
                currentForm = Form.Wheelbarrow;
                break;
            case "battleaxe":
                currentForm = Form.Battleaxe;
                break;
            case "malamute":
                currentForm = Form.Malamute;
                break;
            case "sarmale":
                currentForm = Form.Sarmale;
                break;
            case "potatoe":
                currentForm = Form.Potatoe;
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
		spriteRenderer.transform.localPosition = defSpritePosition;
        
		switch (currentForm)
		{
			case Form.Man:
                spriteRenderer.sprite = transformSprites[0];
                animator.enabled = true;
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
            case Form.Malamute:
                spriteRenderer.sprite = transformSprites[8];
                break;
            case Form.Wheelbarrow:
                spriteRenderer.sprite = transformSprites[9];
                break;
            case Form.Battleaxe:
                spriteRenderer.sprite = transformSprites[10];
                elapsedTime = 0;
                break;
            case Form.Sarmale:
                spriteRenderer.sprite = transformSprites[11];
                break;
            case Form.Potatoe:
                spriteRenderer.sprite = transformSprites[12];
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
				animator.SetBool("IsWalking", true);
				animator.SetFloat("InputX", move.x);
				animator.SetFloat("InputY", move.y);
				break;
			case Form.Shovel:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
				}
				break;
			case Form.Brakes:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
				}
				break;
			case Form.Fly:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
				}

				// Fly Bobbing
				if (goingUp)
				{
					spriteRenderer.transform.localPosition += new Vector3(0, hoverSpeed * Time.fixedDeltaTime, 0);
				}
				else
				{
					spriteRenderer.transform.localPosition -= new Vector3(0, hoverSpeed * Time.fixedDeltaTime, 0);
				}

				if (spriteRenderer.transform.localPosition.y < defSpritePosition.y - bobOffset)
				{
					goingUp = true;
				}
				else if (spriteRenderer.transform.localPosition.y > defSpritePosition.y + bobOffset)
				{
					goingUp = false;
				}

				break;
			case Form.Apathy:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
				}

				// Apathy Bobbing
				if (goingUp)
				{
					spriteRenderer.transform.localPosition += new Vector3(0, hoverSpeed * Time.fixedDeltaTime, 0);
				}
				else
				{
					spriteRenderer.transform.localPosition -= new Vector3(0, hoverSpeed * Time.fixedDeltaTime, 0);
				}

				if (spriteRenderer.transform.localPosition.y < defSpritePosition.y - bobOffset)
				{
					goingUp = true;
				}
				else if (spriteRenderer.transform.localPosition.y > defSpritePosition.y + bobOffset)
				{
					goingUp = false;
				}

				break;
			case Form.Frog:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
				}
				break;
			case Form.Airplane:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
				}
				break;
			case Form.Hammer:
                /*
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
                */
                if (move.x > 0.1)
                {
                    spriteRenderer.flipX = false;
                }
                else if (move.x < -0.1)
                {
                    spriteRenderer.flipX = true;
                }
                    break;
			case Form.Malamute:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
				}
				break;
			case Form.Wheelbarrow:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
				}
				break;
			case Form.Battleaxe:
                if (!transforming)
                {
                    elapsedTime += Time.fixedDeltaTime;
                    if (elapsedTime > spriteInterval && spriteRenderer.sprite == transformSprites[10])
                    {
                        spriteRenderer.sprite = altAxeSprite;
                        elapsedTime = 0;
                    }
                    else if (elapsedTime > spriteInterval)
                    {
                        spriteRenderer.sprite = transformSprites[10];
                        elapsedTime = 0;
                    }
                }
				
				break;
			case Form.Sarmale:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
				}
				break;
            case Form.Potatoe:
				if (move.x > 0.1)
				{
					spriteRenderer.flipX = false;
				}
				else if (move.x < -0.1)
				{
					spriteRenderer.flipX = true;
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
