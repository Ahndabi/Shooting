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
		// 속력을 만들고 싶다면 델타타임 곱하기

		// 월드기준으로 움직임
		//controller.Move(moveDir * moveSpeed * Time.deltaTime);	

		// 로컬기준 움직임
		controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);
			// 내가 바라보는 방향으로 z 방향으로 움직여줌
		controller.Move(transform.right* moveDir.x * moveSpeed * Time.deltaTime);
			// 오른쪽일 땐 x 방향으로

		// transform엔 left가 없음. left 쓰고 싶으면 right에 -1 곱하면 됨
	}

	void OnMove(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		moveDir = new Vector3(input.x, 0, input.y);
	}

	void Jump()
	{
		ySpeed += Physics.gravity.y * Time.deltaTime;
		// 중력의 y방향으로 계속해서 속력을 받아줌(중력이 속력을 계속 더함: 등가속운동). 속력의 개념이니까 델타타임

		if (GroundCheck() && ySpeed < 0)    // GroundCheck가 true이고 아래로 떨어지고 있는 중이 아닐때
		// controllrt.isGrounded 쓰지말래! 왜냐하면, 잘 안먹힌대,, 정교하지 않음
		{
			ySpeed = -1;
			// 아래로 떨어지다가 땅에 떨어져서 땅바닥인 경우 y속력 -1으로
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
		// 바닥으로 레이저 쏴서 공중에서 점프 못하도록 (1단 점프만 가능하도록)

		// 2D랑 3D랑 살짝다름
		// 2D는 Physics2D.Paycast는 
		// 3D는 Physics2D.Paycast의 결과가 bool임 (맞으면 true, 안 맞으면 false)
		RaycastHit hit;
		return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
		// hit을 반환하면서 자기 몸의 *1 위치에서 0.5의 둘레정도로, 아래 방향으로 0.7의 길이만큼 쏜다
	}
}
