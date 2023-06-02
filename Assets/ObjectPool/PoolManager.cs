using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;		// ����Ƽ �⺻�� ������Ʈ Ǯ�� ��������! �̰� �����


// ��κ� �꿡�� ����!!


public class PoolManager : MonoBehaviour
{
	Dictionary<string, ObjectPool<GameObject>> poolDic;
	// ��ųʸ� ex. ��ź����Ʈ ��� Ű ������ ��ź����Ʈ�� ���ӿ�����Ʈ Ǯ�� ã�� �� ����
	private void Awake()
	{
		poolDic = new Dictionary<string, ObjectPool<GameObject>>();
	}

	// �Ϲ�ȭ�� ����. ���� TrailRenderer�� �־���? �׷� TrailRenderer�� ��ȯ���ְ�,
	// ���ӿ�����Ʈ �־����� ���� ������Ʈ ��ȯ���ֵ���... �ٵ� ������Ʈ Ǯ���� �� �ִ� �ֵ鸸 �־�� �ϴϱ�
	// (�� int �̷� �ڷ����� ������ �� �Ǵϱ�) where T�� Object�θ� ���������� ����(���ӿ�����Ʈ, ������Ʈ�� �����ϵ���)
	public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
	{
		// is �� ==�� ���� ����! �̰� ���� ����
		// original�� GameObject�� ����, Component�� ���� ����� ����!
		if (original is GameObject)
		{
			GameObject prefab = original as GameObject;		// original�� as�� ���� prefab���� ����ȯ ������

			if (!poolDic.ContainsKey(prefab.name))      // �� �����ϰ� ������ �������ϴϱ� '!'
				CreatePool(prefab.name, prefab);

			ObjectPool<GameObject> pool = poolDic[prefab.name];
			GameObject go = pool.Get();
			go.transform.position = position;
			go.transform.rotation = rotation;
			return go as T;
		}

		else if(original is Component)
		{
			Component component = original as Component;	// original�� Component�� ����ȯ
			string key = component.gameObject.name;		// ���־� �� ���Ƽ� key ��� Ű����� ������ذ�
			if (!poolDic.ContainsKey(key))
				CreatePool(key, component.gameObject);

			GameObject go = poolDic[key].Get();
			go.transform.position = position;
			go.transform.rotation = rotation;
			return go.GetComponent<T>();	// ���ӿ�����Ʈ�� �پ��ִ� GetComponent�� ��ȯ
		}
		else	// �ٵ� ���� Ǯ���� �� �Ǵ� �ְ� �Դ�? null ��ȯ!
		{
			return null;
		}
	}


	// �������� �̸��� Ű���� �Ȱ��ƾ� ��. �׷��� �� �򰥸��� �� �� ����
	/*
	public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (!poolDic.ContainsKey(prefab.name))      // �� �����ϰ� ������ �������ϴϱ� '!'
			CreatePool(prefab.name, prefab);

		ObjectPool<GameObject> pool = poolDic[prefab.name];
		GameObject go = pool.Get();
		go.transform.position = position;
		go.transform.rotation = rotation;
		return go;
	}*/


	public T Get<T>(T original, Vector3 position, Quaternion rotation) where T : Object
	{
		return Get<T>(original, position, rotation, null);
	}

	public T Get<T>(T original, Transform parent) where T : Object
	{
		return Get<T>(original, Vector3.zero, Quaternion.identity, parent);
	}

	public T Get<T>(T original) where T : Object
	{
		return Get(original, Vector3.zero, Quaternion.identity);
	}


	/*
	// �ݳ��� �����ߴ��� �����ߴ���, bool
	public bool Release(GameObject go)	// � �̸��� ���� ������Ʈ �ݳ�
	{
		if(!poolDic.ContainsKey(go.name))
		{
			return false;		// �ݳ� ����!
		}

		ObjectPool<GameObject> pool = poolDic[go.name];
		pool.Release(go);	
		return true;			// �ݳ� ����!
	}*/

	
	public bool Release<T>(T instance) where T : Object
	{
		if (instance is GameObject)
		{
			GameObject go = instance as GameObject;
			string key = go.name;

			if (!poolDic.ContainsKey(key))
				return false;

			poolDic[key].Release(go);
			return true;
		}
		else if (instance is Component)
		{
			Component component = instance as Component;
			string key = component.gameObject.name;

			if (!poolDic.ContainsKey(key))
				return false;

			poolDic[key].Release(component.gameObject);
			return true;
		}
		else
		{
			return false;
		}
	}


	public bool IsContain<T>(T original) where T : Object
	{
		if (original is GameObject)
		{
			GameObject prefab = original as GameObject;
			string key = prefab.name;

			if (poolDic.ContainsKey(key))
				return true;
			else
				return false;

		}
		else if (original is Component)
		{
			Component component = original as Component;
			string key = component.gameObject.name;

			if (poolDic.ContainsKey(key))
				return true;
			else
				return false;
		}
		else
		{
			return false;
		}
	}

	void CreatePool(string key, GameObject prefab)
	{
		ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
			createFunc: () =>					 // ������� �� ȣ��� ����
			{
				// Instantiate �� ���� �� �������� ���� ������ �ǰ���?
				// �ٵ� �������� ���� �������� ����� �̸� (1) �̷������� �ݷ��� ����..
				// �̸��� ������ ���ƾ� �ݳ��� �����ϴϱ� go.name = key; �۾�����
				// �̸��� key���� ���� ������ذ���
				GameObject go = Instantiate(prefab);
				go.name = key;
				return go;
			},
			actionOnGet: (GameObject go) =>     // Get �� �� ȣ��� ����
			{
				go.SetActive(true);                 // ������ �� Ȱ��ȭ
				go.transform.SetParent(null);
			},
			actionOnRelease: (GameObject go) =>     // �ݳ��� �� ȣ��� ����
			{
				go.SetActive(false);
				go.transform.SetParent(transform);
			},
			actionOnDestroy: (GameObject go) =>     // ������ �� �۾�
			{
				Destroy(go);
			});
		poolDic.Add(key, pool);		// ��ųʸ��� Add�� ���ؼ� �������
	}
}
