using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
	[SerializeField]
	Player player = null;

	[SerializeField]
	Enemy enemy = null;

	// 画面の左・右・上・下の境界の位置
	public Rect screenRect = new Rect();
	
	/// <summary>
	/// GameManagerのゲームオブジェクトが作られた瞬間に呼ばれる関数（メソッド）
	/// </summary>
    private void Awake()
    {
		// あらかじめ、画面上下左右の縁がワールド空間上でどこに位置するか調べておく
		var mainCamera = Camera.main;
		var positionZ = this.transform.position.z;
		var topRight = mainCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, positionZ));
		var bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, positionZ));
		screenRect.x = bottomLeft.x;
		screenRect.width = topRight.x;
		screenRect.y = topRight.y;
		screenRect.height = bottomLeft.y;

		player.Init(screenRect);
		enemy.Init(screenRect);
	}


}
