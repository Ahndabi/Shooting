using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
	[SerializeField] float mouseSensitivity;    // ���콺 ����
	[SerializeField] Transform cameraRoot;

	Vector3 lookeDelta;
	float xRotation;
	float yRotation;

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked;	// Ŀ���� ������� ����
		// Ŀ���� ��� ��� ���ΰ�?
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	// ī�޶�� Update�� ����X - LateUpdate�� �����ؾ���

	private void LateUpdate()
	{
		Look();
	}

	void Look()
	{
		yRotation += lookeDelta.x * mouseSensitivity * Time.deltaTime;
		xRotation -= lookeDelta.y * mouseSensitivity * Time.deltaTime;
		xRotation = Mathf.Clamp(xRotation, -80f, 80f);
		// Clamp : �ִ밪�� �Ѿ�� �ִ밪, �ּڰ��� �Ѿ�� �ּڰ��� ��
		// �ִ� ���� 80, �Ʒ��� 80�� ������ �� �� ���� (����� 180������ �� ���� �� �Ǵϱ�!)
		// ���� ���� �� x ���忡���� - ȸ����
		// �׸��� x���� ��쿡�� ī�޶� ȸ���ؾ��� (�÷��̾�� ȸ��X �Ʒ��� ����ʹٰ� ����� �ȵ�)

		cameraRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);
		transform.localRotation = Quaternion.Euler(0, yRotation, 0);
	}

	void OnLook(InputValue value)
	{
		lookeDelta = value.Get<Vector2>();
	}
}
