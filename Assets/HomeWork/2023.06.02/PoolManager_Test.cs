using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;         // ����Ƽ �⺻�� ������Ʈ Ǯ�� �������ش�! �̰Ÿ� ����Ѵ�.

/*
 * ������Ʈ Ǯ���� �� �� �����ؾ� �� ��Ȱ���� �ִ�.
 * 
 *  1. ������Ʈ Ǯ���� �� ��, �ʱ�ȭ�� �ؾ��Ѵ�. �ʱ�ȭ�� ���� ������ ���� ������Ʈ�� �ݳ��� ���¿��� �ٽ� �뿩���� �� 
 *   ������Ʈ�� ���� ���¿��� ��������. ���� Get �ϰ� ���� �ʱ�ȭ ������ �� ���ľ� �Ѵ�.
 *   
 *  2. ������Ʈ�� �ִ��� �������� üũ�� �� null ǥ���� ���� ���� �����ؾ� �Ѵ�. 
 *   �뿩&�ݳ� �����̱� ������(��Ȱ��ȭ ǥ�ð� null üũ�� �Ǿ) null�̰ų�, ��Ȱ��ȭ �Ǿ� �ִ� ��� �� �� �������Ѵ�.
 *   �̰��� Ȯ��޼���� ǥ�����ָ� ����. -> IsValid 
 *   
 *  3. Ǯ���� ������Ʈ�� �Ǽ��� Destroy�� ���� �� �ִ�. �ݴ��, Instantiate�� �ߴµ� Destroy�� �ƴ� �ݳ��� �� �� �ִ�.
 *  
 *   �̷��� �Ǽ��� �����ϱ� ���� ResourceManager�� ���ؼ��� ����&������ �����ϵ��� �Ѵ�.
 *   ResourceManager�� �̱������� �����, ResourceManager�� ���ؼ� Instantiate�� Destroy�� �����Ѵ�.
 */


public class PoolManager_Test : MonoBehaviour
{
	Dictionary<string, ObjectPool<GameObject>> poolDic;

	private void Awake()
	{
		poolDic = new Dictionary<string, ObjectPool<GameObject>>();
	}

	// �Ϲ�ȭ�� �����Ѵ�. �ٸ� ��ȯ�� �� �ִ� ���� ������Ʈ�� �̾�� �ϱ� ������ where ������������ Object�� ���ش�.
	public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
	{
		if (original is GameObject)
		{
			GameObject prefab = original as GameObject;     // original�� as�� ���� prefab���� ����ȯ ���ش�.
			if (!poolDic.ContainsKey(prefab.name))			// �������� �ʰ� ������ ������־�� �ϴϱ� '!'
				CreatePool(prefab.name, prefab);

			ObjectPool<GameObject> pool = poolDic[prefab.name];
			GameObject go = pool.Get();
			go.transform.position = position;
			go.transform.rotation = rotation;
			return go as T;
		}

		else if (original is Component)
		{
			Component component = original as Component;    // original�� Component�� ����ȯ ���ش�.
			string key = component.gameObject.name;		   // ���־� �� ���Ƽ� key ��� Ű����� �����,

			if (!poolDic.ContainsKey(key))
				CreatePool(key, component.gameObject);

			GameObject go = poolDic[key].Get();
			go.transform.position = position;
			go.transform.rotation = rotation;
			return go.GetComponent<T>();    // ���ӿ�����Ʈ�� �پ��ִ� GetComponent�� ��ȯ�Ѵ�.
		}
		else		// �ٵ� ���� Ǯ���� �� �Ǵ� �ְ� �Դٸ� null ��ȯ�Ѵ�.
		{
			return null;
		}
	}

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
			createFunc: () =>                    // ������� ��
			{
				// Instantiate �� ���� �� �������� ���� ������ �Ǵµ�, �������� ���� �������� ����� �̸� (1) �̷������� �ݷ��� �ٴ´�.
				// �̸��� ������ ���ƾ� �ݳ��� �����ϴϱ� go.name = key; �۾����� �̸��� key���� ���� ������ش�.
				GameObject go = Instantiate(prefab);
				go.name = key;
				return go;
			},
			actionOnGet: (GameObject go) =>         // Get �� ��
			{
				go.SetActive(true);                 // ������ �� Ȱ��ȭ
				go.transform.SetParent(null);
			},
			actionOnRelease: (GameObject go) =>     // �ݳ��� ��
			{
				go.SetActive(false);
				go.transform.SetParent(transform);
			},
			actionOnDestroy: (GameObject go) =>     // ������ �� 
			{
				Destroy(go);
			});
		poolDic.Add(key, pool);     // ��ųʸ��� Add�� ���ؼ� ������ش�.
	}
}
