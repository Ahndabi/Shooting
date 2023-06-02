using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 유니티의 특수 폴더 이름 중 Resources 라는 것이 있다.
 * 씬에서 게임 플레이에 사용할 에셋의 인스턴스를 만드는 대신, 스크립트에서 온디멘드 형식으로 에셋을 로드할 수 있다.
 * 인스펙터 창에서 드래그하면서 스트립트 안의 컴포넌트를 채워넣는 형식의 문제가 있다.
 *  1. 게임 플레이 도중에 드래그가 안되는 것(런타임 당시 참조 불가)
 *  2. 변수명이 바뀌었을 때 참조가 풀린다는 것
 * 
 * 위 두 문제들을 방지하기 위해 Resources 라는 특수폴더를 만들고, 이 안에 게임오브젝트들을 넣어준다.
 * 그리고 Resources.Load<TrailRenderer>("게임오브젝트이름"); 이 작업으로 변수에 넣어주면
 * Resources 폴더 안에서 참조가 가능하여 굳이 드래그로 넣을 필요가 없어진다. 게임오브젝트이름을 알아서 참조한다.
 * 
 */

public class ResourceManager_Test : MonoBehaviour
{
	Dictionary<string, Object> resources = new Dictionary<string, Object>();
	// 딕셔너리로 고유의 데이터들을 갖고 있고, 필요할 때 그 데이터를 참조하는 형식이다.

	public T Load<T>(string path) where T : Object
	{
		string key = $"{typeof(T)}.{path}";

		// 만약 한 번 로딩했을 경우, 또 로딩할 때 이미 로딩한 적이 있으므로, 그 데이터를 참조한다. (매번 로딩할 필요가 없어진다.)
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
	{
		// 폴링했던 애면 릴리즈도 가능할테니, 일단 릴리즈를 한 번 해본다.
		if (GameManager.Pool.Release(go))
			return;

		// 만약 릴리즈가 실패했다면 폴링이 불가능한 오브젝트였을테니, Destroy로 삭제한다.
		GameObject.Destroy(go);
	}
}
