using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//##		디자인 패턴 FlyWeight		##
//========================================
/*
 *  ex) 슬라임 5마리가 있는데 최대 체력이라는 게 슬라임 마다 5개씩 있는 것 보다, 최대 체력은 하나만 있고
 *		슬라임 5마리가 그 하나를 참조하는 형식으로 하는 게 더 낫겠지?
 * 
 * 
    경량화 패턴 :
    한개의 고유 데이터를 다른 객체들에서 공유하게 만들어 메모리 사용량을 줄임
	고유 데이터를 참조하여 사용하기에 불러오는 시간을 줄임

    구현 :
    1. 여러 객체가 참조할 고유 데이터를 생성하여 보관
    2. 객체는 각자의 데이터를 가지지 않고 고유 데이터를 참조하여 사용
    3. 고유 데이터를 보관하는 객체를 싱글톤으로 구성할 경우 더욱 쉽게 사용 가능

    장점 :
    1. 여러 객체가 각자 데이터를 가지지 않고, 고유 데이터를 참조하기에 메모리를 절약할 수 있음
    2. 여러 객체가 각자 데이터를 생성하지 않고, 고유 데이터를 참조하기에 객체 생성의 오버헤드를 줄일 수 있음

    단점 :
    1. 특정 객체만 다른 데이터를 가지게 처리하는 것이 힘듬

*/


namespace DesingPattern
{
	public class FlyWeight
	{
		public class MonsterDataContainer
		{
			private static MonsterDataContainer instance;
			public static MonsterDataContainer Instance { get { return instance; } }

			Dictionary<string, MonsterData> dictionary;

			public MonsterDataContainer()
			{
				instance = this;
				dictionary = new Dictionary<string, MonsterData>();

				// 각 몬스터마다 고유의 데이터 값을 입력해줌
				dictionary.Add("오크", new MonsterData("오크", 100, 20, 10, "무자비한 몬스터입니다."));
				dictionary.Add("드래곤", new MonsterData("드래곤", 500, 120, 30, "브레스를 피해야 합니다."));
				dictionary.Add("슬라임", new MonsterData("슬라임", 3, 10, 5, "분열하는 산성 몬스터입니다."));
			}

			public MonsterData GetData(string name)
			{
				return dictionary[name];
			}
		}


		public class MonsterData
		{
			public string Name { get; private set; }
			public int MaxHP { get; private set; }
			public int Damage { get; private set; }
			public int Shield { get; private set; }
			public string Desription { get; private set; }

			public MonsterData(string name, int maxHP, int damage, int shield, string desription)
			{
				this.Name = name;
				this.MaxHP = maxHP;
				this.Damage = damage;
				this.Shield = shield;
				this.Desription = desription;
			}
		}


		public class Monster		// 몬스터는 몬스터 데이터를 가지고 있음. 
									// 몬스터데이터 하나하나를 가지고 있는 게 아니라 참조하는 거임
		{
			protected MonsterData data;
		}

		public class Orc : Monster	// 여기서 오크를 하나 만들어주면 위의 정보들이 참조됨.
									// 위의 공공 정보를 갖다쓰는 거임
		{
			public Orc()
			{
				this.data = MonsterDataContainer.Instance.GetData("오크");
			}
		}

		public class Dragon : Monster
		{
			public Dragon()
			{
				this.data = MonsterDataContainer.Instance.GetData("드래곤");
			}
		}

		public class Slime : Monster
		{
			public Slime()
			{
				this.data = MonsterDataContainer.Instance.GetData("슬라임");
			}
		}
	}
}