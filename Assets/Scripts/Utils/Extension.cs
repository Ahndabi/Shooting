using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extension		// 확장메서드
{
	// 있는지 없는지 확인하기 위함
	public static bool IsValid(this GameObject go)			// GameObject의 확장 메서드
	{
		return go != null && go.activeInHierarchy;
		// 게임 오브젝트가 널이 아니면서 활성화되어 있는지 확인
	}

	public static bool IsValid(this Component component)	// Component의 확장 메서드
	{
		return component != null && component.gameObject.activeInHierarchy;
	}
}
