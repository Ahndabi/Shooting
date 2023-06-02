using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**** 얘 나중에 깃허브에서 베껴오셈!!


public class ResourceManager : MonoBehaviour
{
	Dictionary<string, Object> resources = new Dictionary<string, Object>();

	// 고유의 데이터를 갖고 있고, 필요할 때 그 데이터를 참조하는 형식
	public T Load<T>(string path) where T : Object
	{
		string key = $"{typeof(T)}.{path}";

		// 슬라임 첫번째 애가 이미지를 로딩했다? 그럼 다른 슬라임이 이미지를 로딩할 때 이미 로딩한 적이 있으니까
		// 그 로딩됐던 공데이터(이미지)를 참조한다. (매번 로딩할 필요X)
		if(resources.ContainsKey(key))
			return resources[key] as T;     // (T)resorces로 한다면, T로 형변환 불가시 프로그램 터짐

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
		T original = Load<T>(path);    // 로드를 먼저하고,
		return Instantiate<T>(original, position, rotation, parent, pooling);   // 그걸 생성해주는 방식으로
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
	{	// 풀링했던 애면 릴리즈도 가능하겠지
		// 그래서 일단 릴리즈를 한 번 해보자
		if (GameManager.Pool.Release(go))
			return;

		// 릴리즈가 만약 실패했다면 풀링이 불가능했던 애였겠지. 삭제하자
		GameObject.Destroy(go);
	}
}
