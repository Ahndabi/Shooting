using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Extension		// Ȯ��޼���
{
	// �ִ��� ������ Ȯ���ϱ� ����
	public static bool IsValid(this GameObject go)			// GameObject�� Ȯ�� �޼���
	{
		return go != null && go.activeInHierarchy;
		// ���� ������Ʈ�� ���� �ƴϸ鼭 Ȱ��ȭ�Ǿ� �ִ��� Ȯ��
	}

	public static bool IsValid(this Component component)	// Component�� Ȯ�� �޼���
	{
		return component != null && component.gameObject.activeInHierarchy;
	}
}
