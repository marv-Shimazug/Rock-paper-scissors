using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	[Header("自分の手")]
	[SerializeField]GameObject Gu;
	[SerializeField]GameObject Choki;
	[SerializeField]GameObject Pa;

	[Header("相手の手")]
	[SerializeField]GameObject EnemyGu;
	[SerializeField]GameObject EnemyChoki;
	[SerializeField]GameObject EnemyPa;

	[Header("ゲーム開始ボタン")] 
	[SerializeField]GameObject GameStartButton;
	[Header("もう1度プレイするボタン")]
	[SerializeField]GameObject RetryButton;

	[Header("ゲーム進行用のテキスト表示場所")]
	[SerializeField]GameObject CenterText;

	// ゲーム進行用テキスト.
	private string[] GameText = new string[] {"じゃんけん", "ぽん", "勝ち", "敗け", "あいこ"};

	[Header("勝利回数の表示")]
	[SerializeField]GameObject VictoryNumPlace;
	// 自分の勝利回数.
	private int VictoryNum;

	// 初期化.
	void Start () 
	{
		// 勝利回数.
		VictoryNum = 0;

		// ゲームテキストの設定.
		CenterText.SetActive (false);

		// リトライボタン.
		RetryButton.SetActive (false);

		// 勝利回数.
		VictoryNumPlace.SetActive (false);
		VictoryNumIndicator(VictoryNum);

		// 手を非表示.
		Gu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		Choki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		Pa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		EnemyGu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		EnemyChoki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		EnemyPa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}

	// ゲーム開始.
	public void OnGameStart()
	{
		// ボタンを非表示.
		GameStartButton.SetActive (false);
		// リトライボタン.
		RetryButton.SetActive (false);

		// じゃんけん開始の文言を表示.
		CenterText.GetComponent<Text>().text = GameText[0];
		CenterText.SetActive (true);

		// 勝利回数.
		VictoryNumPlace.SetActive (true);

		// 手を表示.
		Gu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		Choki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		Pa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		EnemyGu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		EnemyChoki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		EnemyPa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	}
	
	// 更新.
	void Update () 
	{
		// マウスクリック.
		if (Input.GetMouseButtonDown(0)) 
		{
			Vector3    aTapPoint   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D aCollider2d = Physics2D.OverlapPoint(aTapPoint);

			// 手を選択.
			if (aCollider2d) 
			{
				// じゃんけん開始の文言を非表示.
				CenterText.GetComponent<Text>().text = GameText[1];

				GameObject obj = aCollider2d.transform.gameObject;
				Debug.Log("自分の手" +obj.name);
				Debug.Log("自分の手" + SelectHand(obj.name));
				SelectHandIndicator(SelectHand(obj.name), true);

				// 相手の選んだ手と勝負
				Hand enemyHand = HandChast(RandomHand());
				SelectHandIndicator(enemyHand, false);
				Debug.Log("相手の手" + enemyHand);
//				Debug.Log(CheckJanken(SelectHand(obj.name), enemyHand));
				// 勝敗の表示.
				VictoryOrDefeatIndicator(CheckJanken(SelectHand(obj.name), enemyHand));
				// 勝利回数の表示.
				VictoryNumIndicator(VictoryNum);
				RetryButton.SetActive(true);
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
			// 勝利回数の更新.
			VictoryNum ++;
			return VictoryOrDefeat.Victory;
		} 
		else 
		{
			return VictoryOrDefeat.Defeat;
		}
	}

	// 勝ち負けの表示.
	void VictoryOrDefeatIndicator(VictoryOrDefeat myResult)
	{
		switch (myResult)
		{
		case VictoryOrDefeat.Victory:
			CenterText.GetComponent<Text>().text = GameText[2];
			break;

		case VictoryOrDefeat.Defeat:
			CenterText.GetComponent<Text>().text = GameText[3];
			break;

		case VictoryOrDefeat.Draw:
			CenterText.GetComponent<Text>().text = GameText[4];
			break;

		}
	}

	// 勝ち数の表示.
	void VictoryNumIndicator(int victoryNum)
	{
		VictoryNumPlace.GetComponent<Text> ().text = "勝利回数：" + victoryNum;
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


	/// <summary>
	/// 選んだ手のみ表示.
	/// </summary>
	/// <param name="hand">選んだ手.</param>
	/// <param name="friendOrFoe">自分の手 <c>true</c> 相手の手<c>false</c>.</param>
	void SelectHandIndicator(Hand hand, bool friendOrFoe)
	{
		// 自分の手.
		if(true == friendOrFoe)
		{
			switch (hand) 
			{
			case Hand.Gu:
				Choki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				Pa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				break;

			case Hand.Choki:
				Gu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				Pa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				break;

			case Hand.Pa:
				Gu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				Choki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				break;
			}
		}
		// 相手の手.
		else
		{
			switch (hand) 
			{
			case Hand.Gu:
				EnemyChoki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				EnemyPa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				break;
				
			case Hand.Choki:
				EnemyGu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				EnemyPa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				break;
				
			case Hand.Pa:
				EnemyGu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				EnemyChoki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
				break;
			}
		}
	}

}
