using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;         // 유니티 기본용 오브젝트 풀을 지원해준다! 이거를 사용한다.

/*
 * 오브젝트 풀링을 쓸 때 조심해야 할 상활들이 있다.
 * 
 *  1. 오브젝트 풀링을 쓸 때, 초기화를 해야한다. 초기화를 하지 않으면 죽은 오브젝트를 반납한 상태에서 다시 대여했을 때 
 *   오브젝트가 죽은 상태에서 꺼내진다. 따라서 Get 하고 나서 초기화 과정을 꼭 거쳐야 한다.
 *   
 *  2. 오브젝트가 있는지 없는지를 체크할 때 null 표현을 쓰는 것을 주의해야 한다. 
 *   대여&반납 형식이기 때문에(비활성화 표시가 null 체크가 되어서) null이거나, 비활성화 되어 있는 경우 둘 다 따져야한다.
 *   이것을 확장메서드로 표현해주면 좋다. -> IsValid 
 *   
 *  3. 풀링한 오브젝트를 실수로 Destroy로 지울 수 있다. 반대로, Instantiate를 했는데 Destroy가 아닌 반납을 할 수 있다.
 *  
 *   이러한 실수를 제거하기 위해 ResourceManager를 통해서만 생성&삭제를 진행하도록 한다.
 *   ResourceManager를 싱글톤으로 만들고, ResourceManager를 통해서 Instantiate와 Destroy를 진행한다.
 */


public class PoolManager_Test : MonoBehaviour
{
	Dictionary<string, ObjectPool<GameObject>> poolDic;

	private void Awake()
	{
		poolDic = new Dictionary<string, ObjectPool<GameObject>>();
	}

	// 일반화를 적용한다. 다만 반환할 수 있는 것은 오브젝트만 이어야 하기 때문에 where 제약조건으로 Object를 해준다.
	public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
	{
		if (original is GameObject)
		{
			GameObject prefab = original as GameObject;     // original을 as를 통해 prefab으로 형변환 해준다.
			if (!poolDic.ContainsKey(prefab.name))			// 포함하지 않고 있으면 만들어주어야 하니까 '!'
				CreatePool(prefab.name, prefab);

			ObjectPool<GameObject> pool = poolDic[prefab.name];
			GameObject go = pool.Get();
			go.transform.position = position;
			go.transform.rotation = rotation;
			return go as T;
		}

		else if (original is Component)
		{
			Component component = original as Component;    // original을 Component로 형변환 해준다.
			string key = component.gameObject.name;		   // 자주쓸 것 같아서 key 라는 키워드로 만든다,

			if (!poolDic.ContainsKey(key))
				CreatePool(key, component.gameObject);

			GameObject go = poolDic[key].Get();
			go.transform.position = position;
			go.transform.rotation = rotation;
			return go.GetComponent<T>();    // 게임오브젝트에 붙어있는 GetComponent로 반환한다.
		}
		else		// 근데 만약 풀링이 안 되는 애가 왔다면 null 반환한다.
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
			createFunc: () =>                    // 만들어질 때
			{
				// Instantiate 로 만들 때 프리팹이 씬에 생성이 되는데, 프리팹은 씬에 여러개가 생기면 이름 (1) 이런식으로 콜론이 붙는다.
				// 이름이 완전히 같아야 반납이 가능하니까 go.name = key; 작업으로 이름을 key값과 같게 만들어준다.
				GameObject go = Instantiate(prefab);
				go.name = key;
				return go;
			},
			actionOnGet: (GameObject go) =>         // Get 할 때
			{
				go.SetActive(true);                 // 가져갈 땐 활성화
				go.transform.SetParent(null);
			},
			actionOnRelease: (GameObject go) =>     // 반납할 때
			{
				go.SetActive(false);
				go.transform.SetParent(transform);
			},
			actionOnDestroy: (GameObject go) =>     // 삭제할 때 
			{
				Destroy(go);
			});
		poolDic.Add(key, pool);     // 딕셔너리의 Add를 통해서 만들어준다.
	}
}
