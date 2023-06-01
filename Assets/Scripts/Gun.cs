using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gun : MonoBehaviour
{
	[SerializeField] ParticleSystem hitEffect;		// ��ƼŬ. �� ���� ���� �ð�ȿ��
	[SerializeField] ParticleSystem muzzleEffect;   // �ѱ� ����Ʈ
	[SerializeField] TrailRenderer bulletTrail;     // �Ѿ��� ���󰡴� �� ���� ȿ�� trail
	[SerializeField] float bulletSpeed;
	[SerializeField] float maxDistance; // �ִ� ��Ÿ�
	[SerializeField] int damage;		// ������

	public void Fire()
	{
		muzzleEffect.Play();

		RaycastHit hit;		// ����ĳ��Ʈ�� �� ��� �� ����
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
		{
			// Destroy(hit.transform.gameObject);	// ����ĳ��Ʈ�� �ٷ� gameObject�� ����. �׷��� �߰��� transform �θ���.
			// ���� ��ü�� �����.

			IHitable hitable = hit.transform.GetComponent<IHitable>();
			ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
			// ���� ��ġ�� hitEffect ��ƼŬ�� ������ ��

			effect.transform.parent = hit.transform;
			// ���� ��� Ÿ���� �̵��ϸ�, effect�� ���� �ڸ��� �״�� ��������. Ÿ�ϰ� ����Ʈ�� ���� �̵��ؾ���/
			// �׷��� �� ��ġ�� transform�� effect�� parent�� ������.
			Destroy(effect.gameObject, 0.1f);     // ��ƼŬ ȿ���� 0.1�� �ڿ� ����

			TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
			StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));
			Destroy(trail.gameObject, 1f);
			// Quaternion.identity = ȸ��X
			// �ѱ���ġ�� muzzleEffect ��ġ���� ��������.

			hitable?.Hit(hit, damage);
			// �������̽��� ���! �� ����. �׷� �������̽��� �ִ� ��ü�� ��������
			// IHitable�� �����ϰ� �ִ� ������Ʈ�� ������Ʈ�� Hit ��
			// �������̽��� GetComponent�� ���� �� ����.
			// �� ���ӿ�����Ʈ�� IHitable �������̽��� ������
			// �ٵ� IHitable�� ���� ���� ���� ���� (hitable?.Hit(hit, damage);) �׷��� �̷��� �ڵ�.
		}
		else    // �� ���� ���, ������ �ʰ� ���ư��⸸ �ϱ�
		{
			StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
			// ī�޶� ���� �ִ� �ִ� ��Ÿ����� ���� �� �ֵ���.
		}
	}

	// �ð��� ���� �̵����Ѿ� �Ѵ�. �׷��� �ڷ�ƾ���� �̵����� �� ����
	IEnumerator TrailRoutine(Vector3 startPoint, Vector3 endPoint)
	{
		TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
		float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;
		// startPoint���� endPoint������ �Ÿ��� bulletSpeed�� ���� -> ������� ���µ��� �� �ð��� ����

		float rate = 0;
		while (rate < 1)
		{
			trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);
			// 0�� ������ startPoint�� ����� ��, 1�� ������ endPoint�� ����� ��
			rate += Time.deltaTime / totalTime;

			// �ð��� ���� �� ��ġ�� bulletTrail�� ��~ ���� �� �ֵ��� ����

			yield return null;
		}
		Destroy(trail.gameObject);
	}
}
