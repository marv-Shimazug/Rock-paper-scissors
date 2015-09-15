using UnityEngine;
using System.Collections;

public class GameRule : MonoBehaviour {

	// 手.
	enum Hand
	{
		Gu = 0,
		Choki,
		Pa,
		Max,
	}

	// 勝敗.
	enum VictoryOrDefeat
	{
		Victory = 0,
		Defeat,
		Draw,
	}

	[SerializeField]GameObject Gu;
	[SerializeField]GameObject Choki;
	[SerializeField]GameObject Pa;

	// 初期化.
	void Start () 
	{
	
	}
	
	// 更新.
	void Update () 
	{
		// 自分の手を選択.
		if (Input.GetMouseButtonDown(0)) 
		{
			Vector3    aTapPoint   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D aCollider2d = Physics2D.OverlapPoint(aTapPoint);
			
			if (aCollider2d) 
			{
				GameObject obj = aCollider2d.transform.gameObject;
				Debug.Log("自分の手" +obj.name);
				Debug.Log("自分の手" + SelectHand(obj.name));
				// 相手の選んだ手と勝負
				Hand enemyHand = HandChast(RandomHand());
				Debug.Log("相手の手" + enemyHand);
				Debug.Log(CheckJanken(SelectHand(obj.name), enemyHand));
			}
		}
	}

	/// <summary>
	/// 手をランダムで選択.
	/// </summary>
	/// <returns>選んだ「手」を返す.</returns>
	int RandomHand()
	{
		int hand = Random.Range ((int)Hand.Gu, (int)Hand.Max);
		return hand;
	}


	/// <summary>
	/// 自分が選んだ手をHand型で返却.
	/// </summary>
	/// <returns>選んだ手のHand型.</returns>
	/// <param name="name">選んだ手の文字列型.</param>
	Hand SelectHand(string name)
	{
		Hand selectHand = Hand.Max;
		if (name == Gu.name) 
		{
			selectHand = Hand.Gu;
		} 
		else if (name == Choki.name) 
		{
			selectHand = Hand.Choki;
		} 
		else if (name == Pa.name) 
		{
			selectHand = Hand.Pa;
		}

		return selectHand;
	}


	// TODO
	// http://staku.designbits.jp/check-janken/
	/// <summary>
	/// じゃんけんの勝敗を決める.
	/// </summary>
	/// <returns>The janken.</returns>
	/// <param name="myHand">自分が選択した手.</param>
	/// <param name="enemyHand">相手が選択した手.</param>
	VictoryOrDefeat CheckJanken(Hand myHand, Hand enemyHand)
	{
		int c = ((int)myHand - (int)enemyHand + 3) % 3;
		if (0 == c) 
		{
			return VictoryOrDefeat.Draw;
		}
		else if (2 == c) 
		{
			return VictoryOrDefeat.Victory;
		} 
		else 
		{
			return VictoryOrDefeat.Defeat;
		}
	}

	
	/// <summary>
	/// int型をHand型にキャスト.
	/// </summary>
	/// <returns>Hand型の選択した手.</returns>
	/// <param name="hand">int型の選択した手.</param>
	Hand HandChast(int hand)
	{
		Hand retHand = Hand.Max;
		switch (hand) 
		{
		case 0:
			retHand = Hand.Gu;
			break;
		
		case 1:
			retHand = Hand.Choki;
			break;

		case 2:
			retHand = Hand.Pa;
			break;
		}
		return retHand;
	}

}
