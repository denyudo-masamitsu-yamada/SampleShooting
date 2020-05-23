using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	// ---------------メンバ変数------------------------

	// 移動速度
	[SerializeField]
	float moveSpeed = 20.0f;

	// ダメージ値
	[SerializeField]
	int damageValue = 5;

	Vector3 moveDir = Vector3.zero;
	Rect screenRect = new Rect();

	// ------------------------------------------------

	/// <summary>
	/// 移動開始
	/// </summary>
	/// <param name="moveDir"></param>
	public void StartMove(Vector3 createPosition, Vector3 moveDir, Rect screenRect)
	{
		this.moveDir = moveDir;
		this.screenRect = screenRect;

		transform.position = createPosition;
	}

	private void Update()
	{
		transform.position += moveDir * moveSpeed * Time.deltaTime;

		// 画面外に行ったので、削除する
		if (screenRect.y < transform.position.y)
		{
			Destroy(gameObject);
		}
		else if (screenRect.height > transform.position.y)
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// ダメージ値を取得
	/// </summary>
	/// <returns></returns>
	public int GetDamageValue()
	{
		return damageValue;
	}
}
