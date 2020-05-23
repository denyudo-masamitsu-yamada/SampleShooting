using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// エネミーの制御クラス
/// </summary>
public class Enemy : MonoBehaviour
{
	// ---------------メンバ変数------------------------

	// 移動速度
	[SerializeField]
	float moveSpeed = 10.0f;

	// 3D空間の横幅
	[SerializeField]
	float width = 1.0f;

	// 弾を打つ間隔
	[SerializeField]
	float shotIntervalTime = 1.0f;

	// 弾のプレハブ
	[SerializeField]
	GameObject bulletPrefab = null;

	// HPバー
	[SerializeField]
	Image hpBar = null;

	// HP表示
	[SerializeField]
	Text hpText = null;

	// HP最大値
	[SerializeField]
	int maxHp = 100;

	// 死亡時に再生するエフェクト
	[SerializeField]
	GameObject effectDeadPrefab = null;

	// 画面サイズ
	Rect screenRect = new Rect();

	// 移動方向
	float moveDir = 1.0f;

	// 弾を打つタイミングを計る為に使う
	float shotTimingTime = 0.0f;

	// 現在のHP
	int currentHp = 0;

	// ------------------------------------------------

	/// <summary>
	/// エネミーが生成した瞬間に呼ばれます。
	/// </summary>
	private void Awake()
	{
		currentHp = maxHp;
	}

	/// <summary>
	/// エネミーが生成されて、1フレーム後に呼ばれます。
	/// </summary>
	private void Start()
	{

	}
	/// <summary>
	/// 初期化処理
	/// public：外部アクセスできる関数（メソッド）
	/// </summary>
	public void Init(Rect screenRect)
	{
		this.screenRect = screenRect;
	}

	/// <summary>
	/// 毎フレーム更新される関数（メソッド）
	/// </summary>
	private void Update()
	{
		// 死亡したら、動かないようにする
		if (currentHp <= 0)
		{
			return;
		}

		// 移動
		UpdateMove();

		// 弾発射
		UpdateShot();
	}

	/// <summary>
	/// 移動処理
	/// </summary>
	void UpdateMove()
	{
		// 一度、変数の格納する。
		// transform.position.x += 1.0f これはできない為！
		Vector3 position = transform.position;

		// 移動
		// Time.deltaTime：処理落ちがあっても、移動量が変わらないようにする為に使用している。
		position.x -= moveDir * moveSpeed * Time.deltaTime;

		// 画面外に行かせないようにしている。
		float halfWidth = width / 2.0f;
		float screenLeft = screenRect.x + halfWidth;
		float screenRight = screenRect.width - halfWidth;
		if (position.x < screenLeft)
		{
			moveDir *= -1.0f;
			position.x = screenLeft;
		}
		else if (position.x > screenRight)
		{
			moveDir *= -1.0f;
			position.x = screenRight;
		}

		// 反映
		transform.position = position;
	}

	/// <summary>
	/// 弾を打つ
	/// </summary>
	void UpdateShot()
	{
		shotTimingTime += Time.deltaTime;
		if (shotTimingTime >= shotIntervalTime)
		{
			shotTimingTime = 0.0f;

			// 弾を生成する。
			GameObject instance = Instantiate<GameObject>(bulletPrefab);

			// Bulletコンポーネントを取得
			Bullet bullet = instance.GetComponent<Bullet>();

			// 移動開始
			bullet.StartMove(transform.position, Vector3.down, screenRect);
		}
	}

	/// <summary>
	/// 何らかのColliderにヒットしたときに呼ばれる関数（メソッド）
	/// </summary>
	/// <param name="collision"></param>
	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 自分の弾だったら、無視する
		if (collision.gameObject.layer == gameObject.layer)
		{
			// これ以上、実行されない
			return;
		}

		// ヒットしたゲームオブジェクトが、Bulletなのかをチェックする
		Bullet bullet = collision.gameObject.GetComponent<Bullet>();
		if (bullet != null)
		{
			// HPを減らす
			Damage(bullet.GetDamageValue());
		}
	}

	/// <summary>
	/// ダメージを受けた！
	/// </summary>
	/// <param name="damageValue"></param>
	void Damage(int damageValue)
	{
		currentHp -= damageValue;

		// 死亡した！
		if (currentHp <= 0)
		{
			// マイナス値になったら、０にする
			currentHp = 0;

			// 死亡エフェクト再生
			GameObject instance = Instantiate(effectDeadPrefab);
			instance.transform.position = transform.position;

			// ゲームオブジェクトを非アクティブにして、非表示にする
			gameObject.SetActive(false);
		}

		// ゲージ反映
		float fill = (float)currentHp / (float)maxHp;
		hpBar.fillAmount = fill;

		// 表示
		hpText.text = currentHp.ToString() + " / " + maxHp.ToString();
	}
}
