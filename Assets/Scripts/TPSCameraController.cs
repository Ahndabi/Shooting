using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TPSCameraController : MonoBehaviour
{
	[SerializeField] Transform cameraRoot;
	[SerializeField] float mouseSensitivity;
	[SerializeField] float lookDistance;
	[SerializeField] Transform aimTarget;		// ����ٴ� ��

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
		// ��Ȱ��ȭ�Ǹ� ���콺 Ǯ��
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
		// �÷��̾ ī�޶� ���� �ٶ󺸰���. ī�޶� ���� �÷��̾ ī�޶��� ������ �ٶ󺸰���
		Vector3 lookPoint = Camera.main.transform.position + Camera.main.transform.forward * lookDistance;
		aimTarget.position = lookPoint;		// �ٶ󺸱� ���� ��ġ�� ����� �ϴ� ��ġ�� ����

		// lookDistance��ŭ ������ ���� �ٶ󺸰Բ�
		lookPoint.y = transform.position.y;     // �� ��
												// �ٶ󺸴� �ͱ��� �������� ���Ʒ� �����ؼ� �÷��̾ ���ų� ������ ��ٶ����� ��찡 ������
												// �ٶ󺸰� �ִ� ��ġ�� y�� transform ��ġ�� y�� �ٶ󺸰Բ�
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
		// Clamp : �ִ밪�� �Ѿ�� �ִ밪, �ּڰ��� �Ѿ�� �ּڰ��� ��
		// �ִ� ���� 80, �Ʒ��� 80�� ������ �� �� ���� (����� 180������ �� ���� �� �Ǵϱ�!)
		// ���� ���� �� x ���忡���� - ȸ����
		// �׸��� x���� ��쿡�� ī�޶� ȸ���ؾ��� (�÷��̾�� ȸ��X �Ʒ��� ����ʹٰ� ����� �ȵ�)

		cameraRoot.rotation = Quaternion.Euler(xRotation, yRotation, 0);
	}
}