using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gun : MonoBehaviour
{
	[SerializeField] ParticleSystem hitEffect;		// 파티클. 총 쐈을 때의 시각효과
	[SerializeField] ParticleSystem muzzleEffect;   // 총구 이펙트
	[SerializeField] TrailRenderer bulletTrail;     // 총알이 날라가는 것 같은 효과 trail
	[SerializeField] float bulletSpeed;
	[SerializeField] float maxDistance; // 최대 사거리
	[SerializeField] int damage;		// 데미지

	public void Fire()
	{
		muzzleEffect.Play();

		RaycastHit hit;		// 레이캐스트로 총 쏘는 거 구현
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
		{
			// Destroy(hit.transform.gameObject);	// 레이캐스트는 바로 gameObject가 없음. 그래서 중간에 transform 부른다.
			// 맞춘 물체를 지운다.

			IHitable hitable = hit.transform.GetComponent<IHitable>();
			//ParticleSystem effect = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
			// 맞은 위치에 hitEffect 파티클을 나오게 함
			//ParticleSystem effect = GameManager.Resource.Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
			ParticleSystem effect = GameManager.Resource.Instantiate<ParticleSystem>("Prefabs/HitEffect", hit.point, Quaternion.LookRotation(hit.normal));
			// Instantiate를 GameManger.Pool.Get으로 대체한 거임!~!

			effect.transform.parent = hit.transform;
			// 총을 쏘고 타켓이 이동하면, effect는 맞춘 자리에 그대로 남아있음. 타켓과 이펙트는 같이 이동해야함/
			// 그래서 그 위치의 transform을 effect의 parent로 설정함.

			// Destroy(effect.gameObject, 0.1f);     // 파티클 효과를 0.1초 뒤에 없앰

			// TrailRenderer trail = Instantiate(bulletTrail, muzzleEffect.transform.position, Quaternion.identity);
			StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));
			StartCoroutine(ReleaseRoutine(effect.gameObject));
			//Destroy(trail.gameObject, 1f);   원래 이렇게 했다가 코루틴으로 바꿔줌
			// Quaternion.identity = 회전X
			// 총구위치인 muzzleEffect 위치에서 나가도록.

			hitable?.Hit(hit, damage);
			// 인터페이스를 쏜다! 로 구현. 그럼 인터페이스가 있는 물체가 맞을거임
			// IHitable을 포함하고 있는 컴포넌트의 오브젝트는 Hit 함
			// 인터페이스는 GetComponent에 넣을 수 있음.
			// 그 게임오브젝트의 IHitable 인터페이스를 추출함
			// 근데 IHitable이 없는 애일 수도 있지 (hitable?.Hit(hit, damage);) 그래서 이렇게 코드.
		}
		else    // 안 맞은 경우, 때리진 않고 날아가기만 하기
		{
			StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
			// 카메라가 보고 있는 최대 사거리까지 날릴 수 있도록.
		}
	}

	// 이펙트가 3초 뒤에 Destroy 되니까 코루틴으로 해줄거임
	IEnumerator ReleaseRoutine(GameObject effect)
	{
		yield return new WaitForSeconds(3f);	// 3초 기다렸다가 이펙트 사라지게 해줄거임
		GameManager.Resource.Destroy(effect);
	}

	// 시간에 따라서 이동시켜야 한다. 그래서 코루틴으로 이동시켜 줄 거임
	IEnumerator TrailRoutine(Vector3 startPoint, Vector3 endPoint)
	{
		TrailRenderer trail = GameManager.Resource.Instantiate(bulletTrail, startPoint, Quaternion.identity, true);
		// 풀링하고 싶으면 마지막에 true 써주고, 풀링하기 싫으면 false나 아무것도 안 쓰면 됨

		trail.Clear();
		
		float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;
		// startPoint부터 endPoint까지의 거리를 bulletSpeed로 나눔 -> 저기까지 가는데의 총 시간이 나옴

		float rate = 0;
		while (rate < 1)
		{
			trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);
			// 0에 가까우면 startPoint에 가까울 것, 1에 가까우면 endPoint에 가까울 것
			rate += Time.deltaTime / totalTime;

			// 시간에 따라서 그 위치로 bulletTrail가 쭉~ 나갈 수 있도록 했음

			yield return null;
		}
		// Destroy(trail.gameObject);	 // Destroy 대신에 릴리즈
		GameManager.Resource.Destroy(trail.gameObject);
	}
}
