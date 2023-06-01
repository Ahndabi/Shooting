using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IHitable
{
	Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void Hit(RaycastHit hit, int damage)
	{
		// rb �浹ü�� �Ἥ ���� �ֵ��� rb ������Ʈ�� �����״�, rigidbody�� ������ �ϰ� ������ ���Ѵ�
		rb?.AddForceAtPosition(-10 * hit.normal, hit.point, ForceMode.Impulse);
		// AddForceAtPosition : ���⼭ ���� �ش�.
	}
}
