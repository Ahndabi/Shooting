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
		Vector2 input = value.Get<Vector2>();		// input에 Vector2를 가져온다.
		dir = new Vector3(input.x, 0, input.y);		// dir에 x와 y움직임을 반영시킨다.
	}

	void Move()
	{
		// 플레이어는 로컬기준으로 움직여야 한다.
		controller.Move(transform.forward * dir.z * moveSpeed * Time.deltaTime);
			// 내가 바라보는 방향으로 z 방향으로 움직여준다.
		controller.Move(transform.right * dir.x * moveSpeed * Time.deltaTime);
			// 오른쪽일 땐 x방향으로 움직여준다.
	}

	void OnJump(InputValue value)
	{
		if (GroundCheck())			// 바닥에 닿았을 때
			ySpeed = jumpSpeed;		// ySpeed는 jumpSpeed가 된다. (점프가 된다)
	}

	void Jump()
	{
		ySpeed += Physics.gravity.y + Time.deltaTime;
		// 중력의 y방향으로 계속해서 속력을 받아준다. (중력이 속력을 계속 더한다 : 등가속운동).
		
		if(GroundCheck() && ySpeed < 0)		// GroundCheck가 true이고 아래로 떨어지고 있는 중이 아닐 때,
			ySpeed = -1;    // 아래로 떨어지다가 땅에 떨어져서 땅바닥인 경우 y속력을 -1으로 한다.

		controller.Move(Vector3.up * ySpeed * Time.deltaTime);
		// 위의 방향으로 ySpeed만큼 움직여준다.
	}

	bool GroundCheck()
	{
		// 바닥으로 레이저를 쏴서 공중에서 점프하지 못하도록 한다. (1단 점프만 가능하도록 한다.)
		RaycastHit hit;
		return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
		// hit을 반환하면서 자기 몸의 *1 위치에서 0.5의 둘레정도로, 아래 방향으로 0.7의 길이만큼 쏜다.
	}
}
