using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	public Transform target;
	public float distance = 10.0f;
	public float height = 5.0f;
	public float rotateValue = 5.0f;

	private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
		_transform = GetComponent<Transform>();  
    }

    // Update is called once per frame
    void LateUpdate()
    {
		float currentAngle = Mathf.LerpAngle(_transform.eulerAngles.y, 0, rotateValue * Time.deltaTime);
		Quaternion rotateAngle = Quaternion.Euler(0, currentAngle, 0);
		_transform.position = target.position - (rotateAngle * Vector3.forward * distance) + (Vector3.up * height);
		_transform.LookAt(target);
    }
}
