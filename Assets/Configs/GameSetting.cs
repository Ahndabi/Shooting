using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
	// 게임시작되면 무조건 추가되는 함수임 (바로 밑줄)
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

	private static void Init()
	{
		InitGameManager();
	}

	private static void InitGameManager()
	{
		if (GameManager.Instance == null)
		{
			GameObject gameManager = new GameObject();
			gameManager.name = "GameManager";
			gameManager.AddComponent<GameManager>();
		}
	}
}
