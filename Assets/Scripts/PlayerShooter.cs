using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
	
	public TrailRenderer bulletTrail;

	[SerializeField] Rig aimRig;		// 유니티엔진.에니메이션.리깅에 있음
	[SerializeField] float reloadTime;
	[SerializeField] WeaponHolder weaponHolder;		// 인스펙터에서 드래그 해줄거임
	Animator anim;
	bool isReloading;

	private void Awake()
	{
		bulletTrail =  Resources.Load<TrailRenderer>("Prefabs/BulletTrail");
		// 플레이 안 하고 있을 땐 할당이 안 되어 있지만, 플레이 시작하면 바로 할당되는 것을 볼 수 있음!
		anim = GetComponent<Animator>();
	}

	void OnReload(InputValue value)
	{
		if (isReloading)
			return;

		StartCoroutine(ReloadRoutine());
	}

	IEnumerator ReloadRoutine()
	{
		anim.SetTrigger("Reload");
		isReloading = true;
		aimRig.weight = 0f;
		yield return new WaitForSeconds(reloadTime);
		isReloading = false;
		aimRig.weight = 1f;		// 재장전이 끝나면 1f로해서, 다시 조준하게끔
	}

	public void Fire()
	{
		weaponHolder.Fire();
		anim.SetTrigger("Fire");
	}


	void OnFire(InputValue value)
	{
		if (isReloading)	// 재장전 중일 때는 쏘면 안 되니까
			return;

		Fire();
	}
}
