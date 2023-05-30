using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
	[SerializeField] float mouseSensitivity;    // 마우스 감도
	[SerializeField] Transform cameraRoot;

	Vector3 lookeDelta;
	float xRotation;
	float yRotation;

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked;	// 커서를 정가운데에 묶음
		// 커서를 어떻게 잠글 것인가?
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	// 카메라는 Update에 구현X - LateUpdate에 구현해야함

	private void LateUpdate()
	{
		Look();
	}

	void Look()
	{
		yRotation += lookeDelta.x * mouseSensitivity * Time.deltaTime;
		xRotation -= lookeDelta.y * mouseSensitivity * Time.deltaTime;
		xRotation = Mathf.Clamp(xRotation, -80f, 80f);
		// Clamp : 최대값을 넘어가면 최대값, 최솟값을 넘어가면 최솟값을 줌
		// 최대 위로 80, 아래로 80도 까지만 볼 수 있음 (뒤통수 180도까지 막 보면 안 되니까!)
		// 위를 보는 게 x 입장에서는 - 회전임
		// 그리고 x같은 경우에는 카메라만 회전해야함 (플레이어는 회전X 아래를 보고싶다고 누우면 안됨)

		cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);
		transform.localRotation = Quaternion.Euler(0, yRotation, 0);
	}

	void OnLook(InputValue value)
	{
		lookeDelta = value.Get<Vector2>();
	}
}
