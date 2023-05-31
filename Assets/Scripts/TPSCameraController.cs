using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
	[SerializeField] Transform cameraRoot;
	[SerializeField] float mouseSensitivity;
	[SerializeField] float lookDistance;
	[SerializeField] Transform aimTarget;		// 따라다닐 애

	Vector2 lookDelta;
	float xRotation;
	float yRotation;

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
		// 비활성화되면 마우스 풀림
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
		// 플레이어가 카메라를 따라 바라보게함. 카메라를 따라 플레이어가 카메라의 방향을 바라보게함
		Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
		aimTarget.position = lookPoint;		// 바라보기 위한 위치를 쏘고자 하는 위치로 설정

		// lookDistance만큼 떨어진 곳을 바라보게끔
		lookPoint.y = transform.position.y;     // 발 밑
												// 바라보는 것까진 ㄱㅊ은데 위아래 관련해서 플레이어가 눕거나 앞으로 고꾸라지는 경우가 있으니
												// 바라보고 있는 위치의 y가 transform 위치의 y를 바라보게끔
		transform.LookAt(lookPoint);
	}

	void OnLook(InputValue value)
	{
		lookDelta = value.Get<Vector2>();
	}

	void Look()
	{
		yRotation += lookDelta.x * mouseSensitivity * Time.deltaTime;
		xRotation -= lookDelta.y * mouseSensitivity * Time.deltaTime;
		xRotation = Mathf.Clamp(xRotation, -80f, 80f);
		// Clamp : 최대값을 넘어가면 최대값, 최솟값을 넘어가면 최솟값을 줌
		// 최대 위로 80, 아래로 80도 까지만 볼 수 있음 (뒤통수 180도까지 막 보면 안 되니까!)
		// 위를 보는 게 x 입장에서는 - 회전임
		// 그리고 x같은 경우에는 카메라만 회전해야함 (플레이어는 회전X 아래를 보고싶다고 누우면 안됨)

		cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
	}
}