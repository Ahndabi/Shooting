using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMover : MonoBehaviour
{
	[SerializeField] float walkSpeed;	// �ȴ¼ӵ�
	[SerializeField] float runSpeed;	// �ȴ¼ӵ�
	[SerializeField] float moveSpeed;
	[SerializeField] float jumpSpeed;

	Animator anim;
	CharacterController controller;
	Vector3 moveDir;
	float ySpeed = 0;
	bool isWalking;
	//float moveSpeed;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
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

		if(moveDir.magnitude == 0)		// �� ������
		{
			// moveSpeed = 0;	�ٷ� ����
			moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.5f);
			// moveSpeed�� 0���� 0.5�ۼ�Ʈ�� �ش��ϴ� �κи�ŭ ����
			// ���� ���� ���߱⸦ ���ϸ� Mathf.Lerp (��������. ������~ �ϴ°�)
		}
		else if(isWalking)		// �����̴µ� ����
		{
			moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
		}
		else					// �����̴µ� ��
		{
			moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);

		}
		// magnitude : ũ�Ⱑ 0�̸� �ȿ�����

		// ���ñ��� ������
		controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);    // ���� �ٶ󺸴� �������� z �������� ��������
		controller.Move(transform.right* moveDir.x * moveSpeed * Time.deltaTime);       // �������� �� x ��������

		// Lerp : ó����(���簪)�� ����(��ǥ��) �߰��� ���ۼ�Ʈ�� ���� ����� �� ����. ���������� ���ϴ� �� ����
		// �ٵ� �ؿ��� ������ �ϴϱ� Lerp �� ����,, Mathf.Lerp

		anim.SetFloat("XSpeed", moveDir.x, 0.1f, Time.deltaTime);
		anim.SetFloat("YSpeed", moveDir.z, 0.1f, Time.deltaTime);
		anim.SetFloat("Speed", moveSpeed);
		// ���� ������ �ִϸ��̼��� ��� Ȯ�εǾ�� �ϴϱ� OnMove�� ���� �ʰ� Move()�� ����,,???
		// 0.1 ������ �־��ְ� ��ŸŸ�� �߰�. �ٷ� �ٲ��� �ʰ� ����� �ٲ�(����)

		// transform�� left�� ����. left ���� ������ right�� -1 ���ϸ� ��
	}

	void OnMove(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		moveDir = new Vector3(input.x, 0, input.y);
		// �����̴� �� ���ʿ�����(x), �յ�(z)�ϱ� Vector2�� x, y�� �޾ƿ���, ������ 3D�� y�൵ �����ϱ� moveDir�� Vector3�� ���������� �����Ѵ�.
		
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
		// 2D�� Physics2D.Paycast�� RaycastHit2D �� ��ȯ������
		// 3D�� Physics2D.Paycast�� ����� bool�� (������ true, �� ������ false) - (�׷��� out �Ķ��Ÿ�� hit�� ��ȯ��)
		RaycastHit hit;
		return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
		// hit�� ��ȯ�ϸ鼭 �ڱ� ���� *1 ��ġ���� 0.5�� �ѷ�������, �Ʒ� �������� 0.7�� ���̸�ŭ ���
		// SphereCast : ���� �������� ��. �������� �������� ��� ��ٴ��� ���������ؼ� �� ����Ȯ�ϴϱ� �������� ��� ����
	}

	void OnWalk(InputValue value)
	{
		isWalking = value.isPressed;	// ������ �� isWalking�� true�� �ǰ�, ���� �� false�� �� ����

	}
}
