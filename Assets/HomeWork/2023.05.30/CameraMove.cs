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

	private void OnEnable()			// Ȱ��ȭ �Ǹ�
	{	
		Cursor.lockState = CursorLockMode.Locked;	// Ŀ���� ��ٴ�.
	}

	private void OnDisable()		// ��Ȱ��ȭ �Ǹ�
	{
		Cursor.lockState = CursorLockMode.None;		// Ŀ���� Ǭ��.
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
		// �÷��̾ ī�޶� �ٶ󺸰� �ؾ��Ѵ�. ī�޶� ���� �÷��̾ ī�޶��� ������ �ٶ󺸰� �Ѵ�.
		Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;

		// lookDistance��ŭ ������ ���� �ٶ󺸰Բ� �Ѵ�.
		lookPoint.y = transform.position.y;     // �� ��
												// �ٶ󺸰� �ִ� ��ġ�� y�� transform ��ġ�� y�� �ٶ󺸰Բ� �Ѵ�.
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
		// Clamp : �ִ밪�� �Ѿ�� �ִ밪, �ּڰ��� �Ѿ�� �ּڰ��� �ش�.
		// �ִ� ���� 80, �Ʒ��� 80�� ������ �� �� �ִ�.

		cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
	}
}
