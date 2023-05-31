using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
	Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void OnReload(InputValue value)
	{
		anim.SetTrigger("Reload");
	}

	void OnFire(InputValue value)
	{
		anim.SetTrigger("Fire");
	}
}
