using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//##		디자인 패턴 ObjectPool		##
//========================================
	/*
	 *  ****************** 최적화 관련 아이라서 짱 중요 ******************
	 * 
		오브젝트 풀링 패턴 :
		프로그램 내에서 빈번하게 재활용되는 인스턴스들을 생성&삭제하지 않고
		미리 생성해놓은 인스턴스들을 보관한 객체집합(풀)에서
		인스턴스를 대여&반납하는 방식으로 사용하는 기법

		구현 :
		1. 인스턴스들을 보관할 객체집합(풀)을 생성
		2. 프로그램의 시작시 객체집합(풀)에 인스턴스들을 생성하여 보관
		3. 인스턴스가 필요로 하는 상황에서 객체집합(풀)의 인스턴스를 대여하여 사용
		4. 인스턴스가 필요로 하지 않는 상황에서 객체집합(풀)에 인스턴스를 반납하여 보관    

		장점 :
		1. 빈번하게 사용하는 인스턴스의 생성에 소요되는 오버헤드를 줄임
		2. 빈번하게 사용하는 인스턴스의 삭제에 가비지 콜렉터 부담을 줄임

		단점 :  * 단점도 중요! *
		1. 미리 생성해놓은 인스턴스에 의해 사용하지 않는 경우에도 메모리를 차지하고 있음
		2. 메모리가 넉넉하지 않은 기기에서 너무 많은 오브젝트 풀링을 사용하는 경우,
		   힙영역의 여유공간이 줄어들어 오히려 프로그램에 부담이 되는 경우가 있음
	*/

	// 무지성으로 막 쓰지 말고, 알고 쓰래


namespace DesingPattern
{

	public class ObjectPooler
	{
		private Stack<PooledObject> objectPool = new Stack<PooledObject>();
		// 스택이 큐보다 연산하기 조금 더 단간 (큐는 프론트 앤드 화살표 때문에 연산을 더 함)
		private int poolSize = 100;		// pc가 아닌 모바일이라면 이걸 더 작게 해야 함
										// 100개를 미리 만들어

		public void CreatePool()
		{
			for (int i = 0; i < poolSize; i++)
			{
				objectPool.Push(new PooledObject());	// 100개를 미리 만들어
			}
		}

		public PooledObject GetPool()		// 대여
		{
			if (objectPool.Count > 0)		// Pool에 남아있다면
				return objectPool.Pop();	// 대여
			else							// Pool에 더이상 남아있는 게 없으면
				return new PooledObject();	// 만들면 되지~
		}

		public void ReturnPool(PooledObject pooled)		// 반납
		{
			objectPool.Push(pooled);	
		}
	}

	public class PooledObject
	{
		// 생성&삭제가 빈번한 클래스
	}

}
