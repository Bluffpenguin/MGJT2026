using UnityEngine;

public class UIWidthLinker : MonoBehaviour
{
    [SerializeField] RectTransform objToCopy;
    RectTransform self;

	Vector2 size;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Awake()
	{
		self = GetComponent<RectTransform>();
	}
	// Update is called once per frame
	void Update()
    {
		size = self.sizeDelta;
		size.x = objToCopy.sizeDelta.x;
		self.sizeDelta = size;
    }
}
