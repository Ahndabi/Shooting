using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;		// 유니티 기본용 오브젝트 풀을 지원해줌! 이거 사용함


// 대부분 깃에서 베낌!!


public class PoolManager : MonoBehaviour
{
	Dictionary<string, ObjectPool<GameObject>> poolDic;
	// 딕셔너리 ex. 총탄이펙트 라는 키 값으로 총탄이펙트의 게임오브젝트 풀을 찾을 수 있음
	private void Awake()
	{
		poolDic = new Dictionary<string, ObjectPool<GameObject>>();
	}

	// 일반화를 적용. 만약 TrailRenderer를 넣었다? 그럼 TrailRenderer를 반환해주고,
	// 게임오브젝트 넣었으면 게임 오브젝트 반환해주도록... 근데 오브젝트 풀링될 수 있는 애들만 넣어야 하니까
	// (막 int 이런 자료형을 넣으면 안 되니까) where T로 Object로만 제약조건을 넣음(게임오브젝트, 컴포넌트만 가능하도록)
	public T Get<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
	{
		// is 는 ==랑 같지 않음! 이건 공부 ㄱㄱ
		// original이 GameObject일 때와, Component일 때를 나누어서 해줌!
		if (original is GameObject)
		{
			GameObject prefab = original as GameObject;		// original을 as를 통해 prefab으로 형변환 시켜줌

			if (!poolDic.ContainsKey(prefab.name))      // 안 포함하고 있으면 만들어야하니까 '!'
				CreatePool(prefab.name, prefab);

			ObjectPool<GameObject> pool = poolDic[prefab.name];
			GameObject go = pool.Get();
			go.transform.position = position;
			go.transform.rotation = rotation;
			return go as T;
		}

		else if(original is Component)
		{
			Component component = original as Component;	// original을 Component로 형변환
			string key = component.gameObject.name;		// 자주쓸 것 같아서 key 라는 키워드로 만들어준겨
			if (!poolDic.ContainsKey(key))
				CreatePool(key, component.gameObject);

			GameObject go = poolDic[key].Get();
			go.transform.position = position;
			go.transform.rotation = rotation;
			return go.GetComponent<T>();	// 게임오브젝트에 붙어있는 GetComponent로 반환
		}
		else	// 근데 만약 풀링이 안 되는 애가 왔다? null 반환!
		{
			return null;
		}
	}


	// 프리팹의 이름과 키값이 똑같아야 함. 그래야 안 헷갈리게 쓸 수 있음
	/*
	public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (!poolDic.ContainsKey(prefab.name))      // 안 포함하고 있으면 만들어야하니까 '!'
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
	// 반납이 실패했는지 성공했는지, bool
	public bool Release(GameObject go)	// 어떤 이름의 게임 오브젝트 반납
	{
		if(!poolDic.ContainsKey(go.name))
		{
			return false;		// 반납 실패!
		}

		ObjectPool<GameObject> pool = poolDic[go.name];
		pool.Release(go);	
		return true;			// 반납 성공!
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
			createFunc: () =>					 // 만들어질 때 호출될 애임
			{
				// Instantiate 로 만들 때 프리팹이 씬에 생성이 되겠지?
				// 근데 프리팹은 씬에 여러개가 생기면 이름 (1) 이런식으로 콜론이 붙음..
				// 이름이 완전히 같아야 반납이 가능하니까 go.name = key; 작업으로
				// 이름을 key값과 같게 만들어준거임
				GameObject go = Instantiate(prefab);
				go.name = key;
				return go;
			},
			actionOnGet: (GameObject go) =>     // Get 할 때 호출될 애임
			{
				go.SetActive(true);                 // 가져갈 땐 활성화
				go.transform.SetParent(null);
			},
			actionOnRelease: (GameObject go) =>     // 반납할 때 호출될 거임
			{
				go.SetActive(false);
				go.transform.SetParent(transform);
			},
			actionOnDestroy: (GameObject go) =>     // 삭제할 때 작업
			{
				Destroy(go);
			});
		poolDic.Add(key, pool);		// 딕셔너리의 Add를 통해서 만들어줌
	}
}
