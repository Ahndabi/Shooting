using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable_Test : MonoBehaviour
{
	[SerializeField] float releaseTime;     // 몇초 지나면 반납됨

	ObjectPooling_Test pool;
	public ObjectPooling_Test Pool { get { return pool; } set { pool = value; } }

	private void OnEnable()     // 활성화 됐을 때 진행
	{
		// 시작하자마자 몇초뒤에 반납
		StartCoroutine(ReleaseTimer());
	}

	IEnumerator ReleaseTimer()
	{
		yield return new WaitForSeconds(releaseTime);   // releaseTime만큼 기다려주고
		pool.Release(this);
	}
}
