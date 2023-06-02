using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public const string DefaultName = "Manager";

	private static GameManager instance;
	private static PoolManager pool;
	private static ResourceManager resource;

	public static GameManager Instance { get { return instance; } }
	public static PoolManager Pool { get { return pool; } }
	public static ResourceManager Resource { get { return resource; } }

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
			return;
		}

		instance = this;
		DontDestroyOnLoad(this);
		InitManagers();
	}

	private void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	private void InitManagers()
	{
		GameObject poolObj = new GameObject();
		poolObj.name = "PoolManager";
		poolObj.transform.parent = transform;
		pool = poolObj.AddComponent<PoolManager>();

		GameObject resourceObj = new GameObject();
		resourceObj.name = "ResourceManager";
		resourceObj.transform.parent = transform;
		resource = resourceObj.AddComponent<ResourceManager>();
	}
}
