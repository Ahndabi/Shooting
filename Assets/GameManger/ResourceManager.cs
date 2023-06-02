using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**** �� ���߿� ����꿡�� ��������!!


public class ResourceManager : MonoBehaviour
{
	Dictionary<string, Object> resources = new Dictionary<string, Object>();

	// ������ �����͸� ���� �ְ�, �ʿ��� �� �� �����͸� �����ϴ� ����
	public T Load<T>(string path) where T : Object
	{
		string key = $"{typeof(T)}.{path}";

		// ������ ù��° �ְ� �̹����� �ε��ߴ�? �׷� �ٸ� �������� �̹����� �ε��� �� �̹� �ε��� ���� �����ϱ�
		// �� �ε��ƴ� ��������(�̹���)�� �����Ѵ�. (�Ź� �ε��� �ʿ�X)
		if(resources.ContainsKey(key))
			return resources[key] as T;     // (T)resorces�� �Ѵٸ�, T�� ����ȯ �Ұ��� ���α׷� ����

		T resource = Resources.Load<T>(path);
		resources.Add(key, resource);
		return resource;
	}

	public T Instantiate<T>(T original, Vector3 position, Quaternion rotation, bool pooling = false) where T : Object
	{
		if(pooling)
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
	{	// Ǯ���ߴ� �ָ� ����� �����ϰ���
		// �׷��� �ϴ� ����� �� �� �غ���
		if (GameManager.Pool.Release(go))
			return;

		// ����� ���� �����ߴٸ� Ǯ���� �Ұ����ߴ� �ֿ�����. ��������
		GameObject.Destroy(go);
	}
}
