using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
	[SerializeField] float moveSpeed;
	[SerializeField] float jumpSpeed;

	CharacterController controller;
	Vector3 moveDir;
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

	void Move()
	{
		// �ӷ��� ����� �ʹٸ� ��ŸŸ�� ���ϱ�

		// ����������� ������
		//controller.Move(moveDir * moveSpeed * Time.deltaTime);	

		// ���ñ��� ������
		controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
			// ���� �ٶ󺸴� �������� z �������� ��������
		controller.Move(transform.right* moveDir.x * moveSpeed * Time.deltaTime);
			// �������� �� x ��������

		// transform�� left�� ����. left ���� ������ right�� -1 ���ϸ� ��
	}

	void OnMove(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		moveDir = new Vector3(input.x, 0, input.y);
	}

	void Jump()
	{
		ySpeed += Physics.gravity.y * Time.deltaTime;
		// �߷��� y�������� ����ؼ� �ӷ��� �޾���(�߷��� �ӷ��� ��� ����: ��ӿ). �ӷ��� �����̴ϱ� ��ŸŸ��

		if (GroundCheck() && ySpeed < 0)    // GroundCheck�� true�̰� �Ʒ��� �������� �ִ� ���� �ƴҶ�
		// controllrt.isGrounded ��������! �ֳ��ϸ�, �� �ȸ�����,, �������� ����
		{
			ySpeed = -1;
			// �Ʒ��� �������ٰ� ���� �������� ���ٴ��� ��� y�ӷ� -1����
		}
		controller.Move(Vector3.up * ySpeed * Time.deltaTime);
	}

	void OnJump(InputValue value)
	{
		if(GroundCheck())
			ySpeed = jumpSpeed;
	}

	bool GroundCheck()
	{
		// �ٴ����� ������ ���� ���߿��� ���� ���ϵ��� (1�� ������ �����ϵ���)

		// 2D�� 3D�� ��¦�ٸ�
		// 2D�� Physics2D.Paycast�� 
		// 3D�� Physics2D.Paycast�� ����� bool�� (������ true, �� ������ false)
		RaycastHit hit;
		return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
		// hit�� ��ȯ�ϸ鼭 �ڱ� ���� *1 ��ġ���� 0.5�� �ѷ�������, �Ʒ� �������� 0.7�� ���̸�ŭ ���
	}
}
