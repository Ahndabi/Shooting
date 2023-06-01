using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	[SerializeField] Poolable poolablePrefab;

	[SerializeField] int poolSize;      // 미리 만들어 놓을 사이즈
	[SerializeField] int maxSize;       // 반납, 더 만들 경우, 최대 몇 개까지 보관할 수 있을지?

	Stack<Poolable> objectPool = new Stack<Poolable>();
	private void Awake()		// 외부상황에 영향을 받을 것 같으면 Start, 안 받을 것 같으면 Awake
	{
		CreatePool();
	}

	void CreatePool()
	{
		for (int i = 0; i < poolSize; i++)
		{
			Poolable poolable = Instantiate(poolablePrefab);
			// 이펙트를 만들어놓고 보관하고 싶은데, 유니티는 만들자마자 활성화가 됨. 그래서 이걸 비활성화시켜줌
			poolable.gameObject.SetActive(false);		// 만들자마자 비활성화 시켜놓음
			poolable.transform.SetParent(transform);    // 오브젝트풀러 밑으로 넣어줌 (parent를 나 자신으로)
			poolable.Pool = this;						// 반납할 위치. 반납할 위치는 this 다
			objectPool.Push(poolable);
		}
	}

	public Poolable Get()
	{
		if(objectPool.Count > 0)	// 남은 게 있는 경우
		{
			Poolable poolable = objectPool.Pop();
			poolable.gameObject.SetActive(true);    // 쓰려고 꺼낸 거니까 활성화
			poolable.transform.parent = null;       // 꺼내서 쓸 수 있도록
													// 빈게임오브젝트 하위자식으로 poolable을 두는 방식으로 (폴더링해서) 썼는데, 쓸 때는 자식에서 빼서 써야함
			return poolable;
		}
		else   // 만약 남은 게 없으면? 만들어서 써야징
		{
			Poolable poolable = Instantiate(poolablePrefab);
			poolable.Pool = this;  	// 반납할 위치. 반납할 위치는 this 다
			return poolable;
		}
	}

	public void Release(Poolable poolable)
	{
		if (objectPool.Count < maxSize)		// 반납할 수 있는 최대량을 넘지 않았다면, 반납~
		{
			poolable.gameObject.SetActive(false);
			poolable.transform.SetParent(transform);    // 해당 오브젝트 풀에다가 ㄱㄱ
			objectPool.Push(poolable);
		}
		else    // 최대량을 넘었다면 삭제
		{
			Destroy(poolable.gameObject);
		}
	}
}
