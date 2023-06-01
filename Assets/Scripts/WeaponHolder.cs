using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
	[SerializeField] Gun gun;

	public void Fire()
	{
		gun.Fire();
	}
}
