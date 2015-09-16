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


	enum DebugMode
	{
		Normal = 0,	// 通常のランダムじゃんけん
		Victory,	// 確実に自分の勝ち.
		Defeat,	// 確実に自分の負け.
		Draw,		// 確実にあいこ.
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

	// デバッグモード用テキスト.
	private string[] DebugModeText = new string[]{"通常", "確実に勝ち", "確実に敗け", "確実にあいこ"};
	[Header("デバッグモードボタンの表示テキスト")]
	[SerializeField] GameObject DebugModeButtonText;

	[Header("自分の勝利結果表示")]
	[SerializeField]GameObject VictoryNumPlace;
	// 自分の勝利回数.
	private int VictoryNum;
	// 自分の連続勝利回数.
	public int WinningStreak;

	// 現在のゲームモード(デバッグ用).
	private DebugMode NowDebugMode;


	void Awake () 
	{
		Screen.SetResolution (800, 600, false);
	}

	// 初期化.
	void Start () 
	{
		// デバッグモード.
		NowDebugMode = DebugMode.Normal;

		// 勝利回数.
		VictoryNum = 0;
		// 連勝数.
		WinningStreak = 0;

		// ゲームテキストの設定.
		CenterText.SetActive (false);

		// リトライボタン.
		RetryButton.SetActive (false);

		// 勝利回数.
		VictoryNumPlace.SetActive (false);
		VictoryNumIndicator(VictoryNum, WinningStreak);

		// 手を非表示.
		Gu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		Choki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		Pa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		EnemyGu.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		EnemyChoki.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
		EnemyPa.GetComponent<SpriteRenderer> ().material.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}

	public void OnDebugModeChange()
	{
		NowDebugMode ++;
		if(DebugMode.Draw < NowDebugMode)
		{
			NowDebugMode = DebugMode.Normal;
		}
		DebugModeButtonText.GetComponent<Text> ().text = NowDebugModeText ();
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
				Hand enemyHand = ChoiseEnemyHand(SelectHand(obj.name));
				SelectHandIndicator(enemyHand, false);
				Debug.Log("相手の手" + enemyHand);
//				Debug.Log(CheckJanken(SelectHand(obj.name), enemyHand));
				VictoryOrDefeat myResult = VictoryOrDefeat.Draw;
				myResult = CheckJanken(SelectHand(obj.name), enemyHand);
				// 勝敗の表示.
				VictoryOrDefeatIndicator(myResult);
				// 勝利回数の更新.
				UpdateWiningNum(myResult);
				// 勝利回数の表示.
				VictoryNumIndicator(VictoryNum, WinningStreak);
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

	// 敵の手の選び方.
	Hand ChoiseEnemyHand(Hand myHand)
	{
		Hand enemyHand = Hand.Max;
		switch (NowDebugMode) 
		{
		case DebugMode.Normal:
			enemyHand = HandChast(RandomHand());
			break;
			
		case DebugMode.Victory:
			switch(myHand)
			{
			case Hand.Gu:
				enemyHand = Hand.Choki;
				break;
			case Hand.Choki:
				enemyHand = Hand.Pa;
				break;
			case Hand.Pa:
				enemyHand = Hand.Gu;
				break;
			}
			break;
			
		case DebugMode.Defeat:
			switch(myHand)
			{
			case Hand.Gu:
				enemyHand = Hand.Pa;
				break;
			case Hand.Choki:
				enemyHand = Hand.Gu;
				break;
			case Hand.Pa:
				enemyHand = Hand.Choki;
				break;
			}
			break;
			
		case DebugMode.Draw:
			enemyHand = myHand;
			break;
			
		}
		return enemyHand;
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
	void VictoryNumIndicator(int victoryNum, int winningStreak)
	{
		VictoryNumPlace.GetComponent<Text> ().text = "勝利回数：" + victoryNum + "\n"
			+ "連勝数：" + winningStreak + "\n";

	}


	// 累計勝利回数、連勝数の更新.
	void UpdateWiningNum(VictoryOrDefeat myResult)
	{
		switch (myResult)
		{
		case VictoryOrDefeat.Victory:
			VictoryNum++;
			WinningStreak++;
			break;
			
		case VictoryOrDefeat.Defeat:
			WinningStreak = 0;
			break;
			
		case VictoryOrDefeat.Draw:
			WinningStreak = 0;
			break;
			
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

	// 現在のデバッグモードのテキスト.
	private string NowDebugModeText()
	{
		string nowMessage = "";
		switch (NowDebugMode) 
		{
		case DebugMode.Normal:
			nowMessage = DebugModeText[0];
			break;

		case DebugMode.Victory:
			nowMessage = DebugModeText[1];
			break;
		
		case DebugMode.Defeat:
			nowMessage = DebugModeText[2];
			break;
		
		case DebugMode.Draw:
			nowMessage = DebugModeText[3];
			break;
		}
		return nowMessage;
	}

}
