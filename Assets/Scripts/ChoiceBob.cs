using UnityEngine;

public class ChoiceBob : MonoBehaviour
{
	[SerializeField] float maxBobOffset = 6, minBobOffset = 2;
	float bobOffset = 0;
	[SerializeField] float minSpeed = 2;
	[SerializeField] float maxSpeed = 8;
	float defaultY, speed;
	bool goingUp = false;
	private void OnEnable()
	{
		defaultY = transform.position.y;
		speed = Random.Range(minSpeed, maxSpeed);
		bobOffset = Random.Range(minBobOffset, maxBobOffset);
		if (Random.Range((int)0, (int)2) == 1)
		{
			goingUp = true;
		}
		else goingUp = false;
	}

	// Update is called once per frame
	void Update()
    {
		// Dialogue Box Bobing
		if (goingUp)
		{
			transform.position += new Vector3(0, speed * Time.deltaTime, 0);
		}
		else
		{
			transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
		}

		if (transform.position.y < defaultY - bobOffset)
		{
			goingUp = true;
		}
		else if (transform.position.y > defaultY + bobOffset)
		{
			goingUp = false;
		}
	}
}
