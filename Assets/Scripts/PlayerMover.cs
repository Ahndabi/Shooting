using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMover : MonoBehaviour
{
	[SerializeField] float walkSpeed;	// 걷는속도
	[SerializeField] float runSpeed;	// 걷는속도
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
		// 속력을 만들고 싶다면 델타타임 곱하기

		// 월드기준으로 움직임
		//controller.Move(moveDir * moveSpeed * Time.deltaTime);	

		if(moveDir.magnitude == 0)		// 안 움직임
		{
			// moveSpeed = 0;	바로 멈춤
			moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.5f);
			// moveSpeed를 0에서 0.5퍼센트에 해당하는 부분만큼 영향
			// 만약 점점 멈추기를 원하면 Mathf.Lerp (선형보간. 점점점~ 하는거)
		}
		else if(isWalking)		// 움직이는데 걸음
		{
			moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.5f);
		}
		else					// 움직이는데 뜀
		{
			moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.5f);

		}
		// magnitude : 크기가 0이면 안움직임

		// 로컬기준 움직임
		controller.Move(transform.forward * moveDir.z * moveSpeed * Time.deltaTime);    // 내가 바라보는 방향으로 z 방향으로 움직여줌
		controller.Move(transform.right* moveDir.x * moveSpeed * Time.deltaTime);       // 오른쪽일 땐 x 방향으로

		// Lerp : 처음값(현재값)과 끝값(목표값) 중간에 몇퍼센트의 값을 얻어줄 수 있음. 순차적으로 변하는 것 구현
		// 근데 밑에서 댐핑을 하니까 Lerp 안 쓴대,, Mathf.Lerp

		anim.SetFloat("XSpeed", moveDir.x, 0.1f, Time.deltaTime);
		anim.SetFloat("YSpeed", moveDir.z, 0.1f, Time.deltaTime);
		anim.SetFloat("Speed", moveSpeed);
		// 누를 때마다 애니메이션이 계속 확인되어야 하니까 OnMove에 넣지 않고 Move()에 넣음,,???
		// 0.1 댐핑을 넣어주고 델타타임 추가. 바로 바뀌지 않고 슬며시 바뀜(댐핑)

		// transform엔 left가 없음. left 쓰고 싶으면 right에 -1 곱하면 됨
	}

	void OnMove(InputValue value)
	{
		Vector2 input = value.Get<Vector2>();
		moveDir = new Vector3(input.x, 0, input.y);
		// 움직이는 건 왼쪽오른쪽(x), 앞뒤(z)니까 Vector2로 x, y를 받아오고, 실제로 3D는 y축도 있으니까 moveDir를 Vector3의 움직임으로 저장한다.
		
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
		// 2D는 Physics2D.Paycast는 RaycastHit2D 를 반환하지만
		// 3D는 Physics2D.Paycast의 결과가 bool임 (맞으면 true, 안 맞으면 false) - (그래서 out 파라메타로 hit을 반환함)
		RaycastHit hit;
		return Physics.SphereCast(transform.position + Vector3.up * 1, 0.5f, Vector3.down, out hit, 0.6f);
		// hit을 반환하면서 자기 몸의 *1 위치에서 0.5의 둘레정도로, 아래 방향으로 0.7의 길이만큼 쏜다
		// SphereCast : 원형 레이저를 쏨. 일직선의 레이저를 쏘면 길바닥은 울퉁불퉁해서 좀 부정확하니까 원형으로 쏘는 거임
	}

	void OnWalk(InputValue value)
	{
		isWalking = value.isPressed;	// 눌렀을 땐 isWalking이 true가 되고, 뗐을 땐 false가 될 거임

	}
}
