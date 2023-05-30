using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
	[SerializeField] float moveSpeed;
	[SerializeField] float jumpSpeed;

	CharacterController controller;
	Vector3 dir;
	float ySpeed = 0;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	private void Update()
	{
		Move();
		Jump();
	}

	void OnMove(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();		// input�� Vector2�� �����´�.
		dir = new Vector3(input.x, 0, input.y);		// dir�� x�� y�������� �ݿ���Ų��.
	}

	void Move()
	{
		// �÷��̾�� ���ñ������� �������� �Ѵ�.
		controller.Move(transform.forward * dir.z * moveSpeed * Time.deltaTime);
			// ���� �ٶ󺸴� �������� z �������� �������ش�.
		controller.Move(transform.right * dir.x * moveSpeed * Time.deltaTime);
			// �������� �� x�������� �������ش�.
	}

	void OnJump(InputValue value)
	{
		if (GroundCheck())			// �ٴڿ� ����� ��
			ySpeed = jumpSpeed;		// ySpeed�� jumpSpeed�� �ȴ�. (������ �ȴ�)
	}

	void Jump()
	{
		ySpeed += Physics.gravity.y + Time.deltaTime;
		// �߷��� y�������� ����ؼ� �ӷ��� �޾��ش�. (�߷��� �ӷ��� ��� ���Ѵ� : ��ӿ).
		
		if(GroundCheck() && ySpeed < 0)		// GroundCheck�� true�̰� �Ʒ��� �������� �ִ� ���� �ƴ� ��,
			ySpeed = -1;    // �Ʒ��� �������ٰ� ���� �������� ���ٴ��� ��� y�ӷ��� -1���� �Ѵ�.

		controller.Move(Vector3.up * ySpeed * Time.deltaTime);
		// ���� �������� ySpeed��ŭ �������ش�.
	}

	bool GroundCheck()
	{
		// �ٴ����� �������� ���� ���߿��� �������� ���ϵ��� �Ѵ�. (1�� ������ �����ϵ��� �Ѵ�.)
		RaycastHit hit;
		return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
		// hit�� ��ȯ�ϸ鼭 �ڱ� ���� *1 ��ġ���� 0.5�� �ѷ�������, �Ʒ� �������� 0.7�� ���̸�ŭ ���.
	}
}
