using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
	[SerializeField] Rig aimRig;		// ����Ƽ����.���ϸ��̼�.���뿡 ����
	[SerializeField] float reloadTime;
	[SerializeField] WeaponHolder weaponHolder;		// �ν����Ϳ��� �巡�� ���ٰ���
	Animator anim;
	bool isReloading;

	private void Awake()
	{
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
		aimRig.weight = 1f;		// �������� ������ 1f���ؼ�, �ٽ� �����ϰԲ�
	}

	public void Fire()
	{
		weaponHolder.Fire();
		anim.SetTrigger("Fire");
	}


	void OnFire(InputValue value)
	{
		if (isReloading)	// ������ ���� ���� ��� �� �Ǵϱ�
			return;

		Fire();
	}
}
