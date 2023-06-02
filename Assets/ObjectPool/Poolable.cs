using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
	public class Poolable : MonoBehaviour
	{
		[SerializeField] float releaseTime;     // ���� ������ �ݳ���

		ObjectPool pool;
		public ObjectPool Pool { get { return pool; } set { pool = value; } }

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
}