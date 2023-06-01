using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling_Test : MonoBehaviour
{
	[SerializeField] Poolable_Test poolablePrefab;

	[SerializeField] int poolSize;      // �̸� ����� ���� ������
	[SerializeField] int maxSize;       // �ݳ�, �� ���� ���, �ִ� �� ������ ������ �� ������?

	Stack<Poolable_Test> objectPool = new Stack<Poolable_Test>();
	private void Awake()        // �ܺλ�Ȳ�� ������ ���� �� ������ Start, �� ���� �� ������ Awake
	{
		CreatePool();
	}

	void CreatePool()
	{
		for (int i = 0; i < poolSize; i++)
		{
			Poolable_Test poolable = Instantiate(poolablePrefab);
			// ����Ʈ�� �������� �����ϰ� ������, ����Ƽ�� �����ڸ��� Ȱ��ȭ�� ��. �׷��� �̰� ��Ȱ��ȭ������
			poolable.gameObject.SetActive(false);       // �����ڸ��� ��Ȱ��ȭ ���ѳ���
			poolable.transform.SetParent(transform);    // ������ƮǮ�� ������ �־��� (parent�� �� �ڽ�����)
			poolable.Pool = this;                       // �ݳ��� ��ġ. �ݳ��� ��ġ�� this ��
			objectPool.Push(poolable);
		}
	}

	public Poolable_Test Get()
	{
		if (objectPool.Count > 0)   // ���� �� �ִ� ���
		{
			Poolable_Test poolable = objectPool.Pop();
			poolable.gameObject.SetActive(true);    // ������ ���� �Ŵϱ� Ȱ��ȭ
			poolable.transform.parent = null;       // ������ �� �� �ֵ���
													// ����ӿ�����Ʈ �����ڽ����� poolable�� �δ� ������� (�������ؼ�) ��µ�, �� ���� �ڽĿ��� ���� �����
			return poolable;
		}
		else   // ���� ���� �� ������? ���� ���¡
		{
			Poolable_Test poolable = Instantiate(poolablePrefab);
			poolable.Pool = this;   // �ݳ��� ��ġ. �ݳ��� ��ġ�� this ��
			return poolable;
		}
	}

	public void Release(Poolable_Test poolable)
	{
		if (objectPool.Count < maxSize)     // �ݳ��� �� �ִ� �ִ뷮�� ���� �ʾҴٸ�, �ݳ�~
		{
			poolable.gameObject.SetActive(false);
			poolable.transform.SetParent(transform);    // �ش� ������Ʈ Ǯ���ٰ� ����
			objectPool.Push(poolable);
		}
		else    // �ִ뷮�� �Ѿ��ٸ� ����
		{
			Destroy(poolable.gameObject);
		}
	}

}
