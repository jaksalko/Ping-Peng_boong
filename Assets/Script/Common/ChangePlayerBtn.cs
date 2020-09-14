using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangePlayerBtn : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	private Vector2 lastMousePosition;
	public float duration = 2;
	private float timer = 0;
	public bool isHolding = false;
	private bool timerStart = false;
	public GameObject gameController;

	private void Start()
	{
		float positionX = PlayerPrefs.GetFloat("ChangePlayerBtnX");
		float positionY = PlayerPrefs.GetFloat("ChangePlayerBtnY");
		GetComponent<RectTransform>().position = new Vector2(positionX, positionY);
	}

	private void Update()
	{
		// touch 테스트 필요
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			lastMousePosition = touch.position;

			switch (touch.phase)
			{
				case TouchPhase.Began:
					timer = 0;
					isHolding = false;
					break;

				case TouchPhase.Stationary:
					timer += Time.deltaTime;
					if(timer > duration)
					{
						isHolding = true;
						StartCoroutine(LockComponent(0));
					}
					break;

				case TouchPhase.Moved:
					if(isHolding)
					{
						Vector2 currentMousePosition = touch.position;
						Vector2 diff = currentMousePosition - lastMousePosition;
						RectTransform rect = GetComponent<RectTransform>();

						Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
						Vector3 oldPos = rect.position;
						rect.position = newPosition;
						if (!IsRectTransformInsideSreen(rect))
						{
							rect.position = oldPos;
						}
						lastMousePosition = currentMousePosition;
					}
					break;

				case TouchPhase.Ended:
					PlayerPrefs.SetFloat("ChangePlayerBtnX", lastMousePosition.x);
					PlayerPrefs.SetFloat("ChangePlayerBtnY", lastMousePosition.y);
					Debug.Log("End Drag");

					timer = 0;
					timerStart = false;
					isHolding = false;
					StartCoroutine(LockComponent(1));
					break;
			}
		}

		if (timerStart)
		{
			timer += Time.deltaTime;
		}

		if (timer > duration)
		{
			isHolding = true;
			StartCoroutine(LockComponent(0));
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("Click");
		lastMousePosition = eventData.position;
		timerStart = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Debug.Log("Click End");
		timer = 0;
		timerStart = false;
		isHolding = false;
		StartCoroutine(LockComponent(1));
	}

	public void OnDrag(PointerEventData eventData)
	{
		if(isHolding)
		{
			Vector2 currentMousePosition = eventData.position;
			Vector2 diff = currentMousePosition - lastMousePosition;
			RectTransform rect = GetComponent<RectTransform>();

			Vector3 newPosition = rect.position + new Vector3(diff.x, diff.y, transform.position.z);
			Vector3 oldPos = rect.position;
			rect.position = newPosition;
			if (!IsRectTransformInsideSreen(rect))
			{
				rect.position = oldPos;
			}
			lastMousePosition = currentMousePosition;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		RectTransform rect = GetComponent<RectTransform>();

		PlayerPrefs.SetFloat("ChangePlayerBtnX", rect.position.x);
		PlayerPrefs.SetFloat("ChangePlayerBtnY", rect.position.y);
		Debug.Log("End Drag");

		timer = 0;
		timerStart = false;
		isHolding = false;
		StartCoroutine(LockComponent(1));
	}

	private IEnumerator LockComponent(int i)
	{
		yield return new WaitForSeconds(1f);
		switch(i)
		{
			case 0:
				gameObject.GetComponent<Button>().enabled = false;
				break;
			case 1:
				gameObject.GetComponent<Button>().enabled = true;
				break;
			default:
				break;
		}
	}

	private bool IsRectTransformInsideSreen(RectTransform rectTransform)
	{
		bool isInside = false;
		Vector3[] corners = new Vector3[4];
		rectTransform.GetWorldCorners(corners);
		int visibleCorners = 0;
		Rect rect = new Rect(0, 0, Screen.width, Screen.height);
		foreach (Vector3 corner in corners)
		{
			if (rect.Contains(corner))
			{
				visibleCorners++;
			}
		}
		if (visibleCorners == 4)
		{
			isInside = true;
		}
		return isInside;
	}
}
