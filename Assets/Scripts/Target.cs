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
		// rb 충돌체를 써서 날라갈 애들은 rb 컴포넌트가 있을테니, rigidbody가 있으면 하고 없으면 안한다
		rb?.AddForceAtPosition(-10 * hit.normal, hit.point, ForceMode.Impulse);
		// AddForceAtPosition : 여기서 힘을 준다.
	}
}
