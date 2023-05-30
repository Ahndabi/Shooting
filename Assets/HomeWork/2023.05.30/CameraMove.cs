using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
	[SerializeField] Transform cameraRoot;
	[SerializeField] float mouseSencitivity;
	[SerializeField] float lookDistance;

	Vector2 lookDelta;
	float xRotation;
	float yRotation;

	private void OnEnable()			// 활성화 되면
	{	
		Cursor.lockState = CursorLockMode.Locked;	// 커서를 잠근다.
	}

	private void OnDisable()		// 비활성화 되면
	{
		Cursor.lockState = CursorLockMode.None;		// 커서를 푼다.
	}

	private void Update()
	{
		Rotate();
	}

	private void LateUpdate()
	{
		Look();	
	}

	void Rotate()
	{
		// 플레이어가 카메라를 바라보게 해야한다. 카메라를 따라 플레이어가 카메라의 방향을 바라보게 한다.
		Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;

		// lookDistance만큼 떨어진 곳을 바라보게끔 한다.
		lookPoint.y = transform.position.y;     // 발 밑
												// 바라보고 있는 위치의 y가 transform 위치의 y를 바라보게끔 한다.
		transform.LookAt(lookPoint);
	}

	void OnLook(InputValue value)
	{
		lookDelta = value.Get<Vector2>();
	}

	void Look()
	{
		yRotation += lookDelta.x * mouseSencitivity * Time.deltaTime;
		xRotation -= lookDelta.y * mouseSencitivity * Time.deltaTime;
		xRotation = Mathf.Clamp(xRotation, -80f, 80f);
		// Clamp : 최대값을 넘어가면 최대값, 최솟값을 넘어가면 최솟값을 준다.
		// 최대 위로 80, 아래로 80도 까지만 볼 수 있다.

		cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
	}
}
