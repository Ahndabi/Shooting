using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable_Test : MonoBehaviour
{
	[SerializeField] float releaseTime;     // ���� ������ �ݳ���

	ObjectPooling_Test pool;
	public ObjectPooling_Test Pool { get { return pool; } set { pool = value; } }

	private void OnEnable()     // Ȱ��ȭ ���� �� ����
	{
		// �������ڸ��� ���ʵڿ� �ݳ�
		StartCoroutine(ReleaseTimer());
	}

	IEnumerator ReleaseTimer()
	{
		yield return new WaitForSeconds(releaseTime);   // releaseTime��ŭ ��ٷ��ְ�
		pool.Release(this);
	}
}
