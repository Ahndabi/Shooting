using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ����Ƽ�� Ư�� ���� �̸� �� Resources ��� ���� �ִ�.
 * ������ ���� �÷��̿� ����� ������ �ν��Ͻ��� ����� ���, ��ũ��Ʈ���� �µ��� �������� ������ �ε��� �� �ִ�.
 * �ν����� â���� �巡���ϸ鼭 ��Ʈ��Ʈ ���� ������Ʈ�� ä���ִ� ������ ������ �ִ�.
 *  1. ���� �÷��� ���߿� �巡�װ� �ȵǴ� ��(��Ÿ�� ��� ���� �Ұ�)
 *  2. �������� �ٲ���� �� ������ Ǯ���ٴ� ��
 * 
 * �� �� �������� �����ϱ� ���� Resources ��� Ư�������� �����, �� �ȿ� ���ӿ�����Ʈ���� �־��ش�.
 * �׸��� Resources.Load<TrailRenderer>("���ӿ�����Ʈ�̸�"); �� �۾����� ������ �־��ָ�
 * Resources ���� �ȿ��� ������ �����Ͽ� ���� �巡�׷� ���� �ʿ䰡 ��������. ���ӿ�����Ʈ�̸��� �˾Ƽ� �����Ѵ�.
 * 
 */

public class ResourceManager_Test : MonoBehaviour
{
	Dictionary<string, Object> resources = new Dictionary<string, Object>();
	// ��ųʸ��� ������ �����͵��� ���� �ְ�, �ʿ��� �� �� �����͸� �����ϴ� �����̴�.

	public T Load<T>(string path) where T : Object
	{
		string key = $"{typeof(T)}.{path}";

		// ���� �� �� �ε����� ���, �� �ε��� �� �̹� �ε��� ���� �����Ƿ�, �� �����͸� �����Ѵ�. (�Ź� �ε��� �ʿ䰡 ��������.)
		if (resources.ContainsKey(key))
			return resources[key] as T;

		T resouce = Resources.Load<T>(path);
		resources.Add(key, resouce);
		return resouce;
	}

	public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
	{
		if (pooling)
			return GameManager.Pool.Get(original, position, rotation);
		else
			return Object.Instantiate(original, position, rotation);
	}

	public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent, bool pooling = false) where T : Object
	{
		if (pooling)
			return GameManager.Pool.Get(original, position, rotation, parent);

		else
			return Object.Instantiate(original, position, rotation, parent);
	}

	public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, Transform parent, bool pooling = false) where T : Object
	{
		T original = Load<T>(path);    // �ε带 �����ϰ�,
		return Instantiate<T>(original, position, rotation, parent, pooling);   // �װ� �������ִ� �������
	}

	public T Instantiate<T>(string path, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
	{
		return Instantiate<T>(path, position, rotation, null, pooling);
	}

	public T Instantiate<T>(string path, Transform parent, bool pooling = false) where T : Object
	{
		return Instantiate<T>(path, Vector3.zero, Quaternion.identity, parent, pooling);
	}

	public T Instantiate<T>(string path, bool pooling = false) where T : Object
	{
		return Instantiate<T>(path, Vector3.zero, Quaternion.identity, null, pooling);
	}

	public void Destroy(GameObject go)
	{
		// �����ߴ� �ָ� ����� �������״�, �ϴ� ����� �� �� �غ���.
		if (GameManager.Pool.Release(go))
			return;

		// ���� ����� �����ߴٸ� ������ �Ұ����� ������Ʈ�����״�, Destroy�� �����Ѵ�.
		GameObject.Destroy(go);
	}
}
