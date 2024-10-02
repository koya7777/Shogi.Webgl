using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 駒台の管理
/// </summary>
public class CaptureManager : MonoBehaviour
{

	[SerializeField] float per1x = 0.57f;
	[SerializeField] float basex = -1.9f;

	/// <summary> 先手の駒 </summary>
	PieceType[] blackPieceNames = {
			PieceType.BKing,
			PieceType.BRook,
			PieceType.BBishop,
			PieceType.BGold,
			PieceType.BSilver,
			PieceType.BKnight,
			PieceType.BLance,
			PieceType.BPawn
		};
	/// <summary> 後手の駒 </summary>
	PieceType[] whitePieceNames = {
			PieceType.WKing,
			PieceType.WRook,
			PieceType.WBishop,
			PieceType.WGold,
			PieceType.WSilver,
			PieceType.WKnight,
			PieceType.WLance,
			PieceType.WPawn
		};
	// Use this for initialization
	void Start()
	{
		RefreshPiece();
	}

	/// <summary>
	/// 先手駒初期化
	/// </summary>
	void InitBlackPiece()
	{
		foreach (PieceType pieceType in blackPieceNames)
		{
			GameObject obj = GameObject.Find(PieceType.CapturedPiece.ToString() + pieceType.ToString());
			Destroy(obj);
			GameObject objNum = GameObject.Find(PieceType.CapturedPiece.ToString() + pieceType.ToString() + PieceConst.num);
			Destroy(objNum);
			GameObject objNum2 = GameObject.Find(PieceType.CapturedPiece.ToString() + pieceType.ToString() + PieceConst.num2);
			Destroy(objNum2);
		}
	}

	/// <summary>
	/// 後手駒初期化
	/// </summary>
	void InitWhitePiece()
	{
		foreach (PieceType pieceType in whitePieceNames)
		{
			GameObject obj = GameObject.Find(PieceType.CapturedPiece.ToString() + pieceType.ToString());
			Destroy(obj);
			GameObject objNum = GameObject.Find(PieceType.CapturedPiece.ToString() + pieceType.ToString() + PieceConst.num);
			Destroy(objNum);
			GameObject objNum2 = GameObject.Find(PieceType.CapturedPiece.ToString() + pieceType.ToString() + PieceConst.num2);
			Destroy(objNum2);
		}
	}

	/// <summary>
	/// 駒再描画
	/// </summary>
	public void RefreshPiece()
	{
		if (transform.name == "CaptureBlack")
		{
			RefreshBlackPiece();
		}
		else if (transform.name == "CaptureWhite")
		{
			RefreshWhitePiece();
		}
	}

	/// <summary>
	/// 先手駒再描画
	/// </summary>
	void RefreshBlackPiece()
	{
		InitBlackPiece();
		CapturedPieceManager manager = CapturedPieceManager.Instance;
		int i = 0;
		foreach (PieceType piecType in blackPieceNames)
		{
			if (piecType.Equals(PieceType.BKing) && manager.CapturedPieces[PieceType.BKing] >= 1)
			{
				createPieceObj(PieceType.BKing, i++, manager.CapturedPieces[PieceType.BKing]);
			}
			else if (piecType.Equals(PieceType.BRook) && manager.CapturedPieces[PieceType.BRook] >= 1)
			{
				createPieceObj(PieceType.BRook, i++, manager.CapturedPieces[PieceType.BRook]);
			}
			else if (piecType.Equals(PieceType.BBishop) && manager.CapturedPieces[PieceType.BBishop] >= 1)
			{
				createPieceObj(PieceType.BBishop, i++, manager.CapturedPieces[PieceType.BBishop]);
			}
			else if (piecType.Equals(PieceType.BGold) && manager.CapturedPieces[PieceType.BGold] >= 1)
			{
				createPieceObj(PieceType.BGold, i++, manager.CapturedPieces[PieceType.BGold]);
			}
			else if (piecType.Equals(PieceType.BSilver) && manager.CapturedPieces[PieceType.BSilver] >= 1)
			{
				createPieceObj(PieceType.BSilver, i++, manager.CapturedPieces[PieceType.BSilver]);
			}
			else if (piecType.Equals(PieceType.BKnight) && manager.CapturedPieces[PieceType.BKnight] >= 1)
			{
				createPieceObj(PieceType.BKnight, i++, manager.CapturedPieces[PieceType.BKnight]);
			}
			else if (piecType.Equals(PieceType.BLance) && manager.CapturedPieces[PieceType.BLance] >= 1)
			{
				createPieceObj(PieceType.BLance, i++, manager.CapturedPieces[PieceType.BLance]);
			}
			else if (piecType.Equals(PieceType.BPawn) && manager.CapturedPieces[PieceType.BPawn] >= 1)
			{
				createPieceObj(PieceType.BPawn, i++, manager.CapturedPieces[PieceType.BPawn]);
			}
		}
	}

	/// <summary>
	/// 後手駒再描画
	/// </summary>
	void RefreshWhitePiece()
	{
		InitWhitePiece();
		CapturedPieceManager manager = CapturedPieceManager.Instance;
		int i = 0;
		foreach (PieceType pieceName in whitePieceNames)
		{
			if (pieceName.Equals(PieceType.WKing) && manager.CapturedPieces[PieceType.WKing] >= 1)
			{
				createPieceObj(PieceType.WKing, i++, manager.CapturedPieces[PieceType.WKing]);
			}
			else if (pieceName.Equals(PieceType.WRook) && manager.CapturedPieces[PieceType.WRook] >= 1)
			{
				createPieceObj(PieceType.WRook, i++, manager.CapturedPieces[PieceType.WRook]);
			}
			else if (pieceName.Equals(PieceType.WBishop) && manager.CapturedPieces[PieceType.WBishop] >= 1)
			{
				createPieceObj(PieceType.WBishop, i++, manager.CapturedPieces[PieceType.WBishop]);
			}
			else if (pieceName.Equals(PieceType.WGold) && manager.CapturedPieces[PieceType.WGold] >= 1)
			{
				createPieceObj(PieceType.WGold, i++, manager.CapturedPieces[PieceType.WGold]);
			}
			else if (pieceName.Equals(PieceType.WSilver) && manager.CapturedPieces[PieceType.WSilver] >= 1)
			{
				createPieceObj(PieceType.WSilver, i++, manager.CapturedPieces[PieceType.WSilver]);
			}
			else if (pieceName.Equals(PieceType.WKnight) && manager.CapturedPieces[PieceType.WKnight] >= 1)
			{
				createPieceObj(PieceType.WKnight, i++, manager.CapturedPieces[PieceType.WKnight]);
			}
			else if (pieceName.Equals(PieceType.WLance) && manager.CapturedPieces[PieceType.WLance] >= 1)
			{
				createPieceObj(PieceType.WLance, i++, manager.CapturedPieces[PieceType.WLance]);
			}
			else if (pieceName.Equals(PieceType.WPawn) && manager.CapturedPieces[PieceType.WPawn] >= 1)
			{
				createPieceObj(PieceType.WPawn, i++, manager.CapturedPieces[PieceType.WPawn]);
			}
		}
	}

	/// <summary>
	/// 駒オブジェクトの生成
	/// </summary>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="cnt"></param>
	void createPieceObj(PieceType pieceType, int x, int cnt)
	{
		// 駒表示
		string pieceName = pieceType.ToString();
		Sprite[] sprites = Resources.LoadAll<Sprite>("pieces");
		Sprite sp = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(pieceName));
		GameObject gameObj = new GameObject();
		SpriteRenderer spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sp;
		gameObj.transform.parent = FindObjectOfType<Canvas>().transform;
		string objName = PieceType.CapturedPiece.ToString() + pieceName;
		gameObj.transform.name = objName;
		gameObj.transform.localScale = new Vector3(100, 100, 0);
		gameObj.transform.position = new Vector3(transform.position.x + basex + per1x * x, transform.position.y, 2);
		gameObj.AddComponent<BoxCollider2D>();
		PieceController piece = gameObj.AddComponent<PieceController>();
		piece.SetPiece(new Address(0, 0), objName);
		// 枚数
		createPieceNum2Obj(pieceName, x, cnt); // 2桁目
		createPieceNumObj(pieceName, x, cnt); // 1桁目
	}

	/// <summary>
	/// 駒枚数1桁目を生成
	/// </summary>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="cnt"></param>
	void createPieceNumObj(string name, int x, int cnt)
	{
		cnt = (int)(cnt % 10);
		Sprite[] numSprites = Resources.LoadAll<Sprite>("Num");
		Sprite numSp = System.Array.Find<Sprite>(numSprites, (sprite) => sprite.name.Equals("Num_" + cnt));
		GameObject numGameObj = new GameObject();
		SpriteRenderer numSpriteRenderer = numGameObj.AddComponent<SpriteRenderer>();
		numSpriteRenderer.sprite = numSp;
		numGameObj.transform.parent = FindObjectOfType<Canvas>().transform;
		numGameObj.transform.name = PieceType.CapturedPiece.ToString() + name + PieceConst.num;
		numGameObj.transform.localScale = new Vector3(30, 20, 0);
		numGameObj.transform.position = new Vector3(transform.position.x + basex + per1x * x + 0.28f, transform.position.y + 0.2f, 2);
	}

	/// <summary>
	/// 駒枚数2桁目を生成
	/// </summary>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="cnt"></param>
	void createPieceNum2Obj(string name, int x, int cnt)
	{
		int keta2Cnt = (int)(cnt / 10);
		if (keta2Cnt > 0)
		{
			Sprite[] numSprites = Resources.LoadAll<Sprite>("Num");
			Sprite numSp = System.Array.Find<Sprite>(numSprites, (sprite) => sprite.name.Equals("Num_" + keta2Cnt));
			GameObject numGameObj = new GameObject();
			SpriteRenderer numSpriteRenderer = numGameObj.AddComponent<SpriteRenderer>();
			numSpriteRenderer.sprite = numSp;
			numGameObj.transform.parent = FindObjectOfType<Canvas>().transform;
			numGameObj.transform.name = PieceType.CapturedPiece.ToString() + name + PieceConst.num2;
			numGameObj.transform.localScale = new Vector3(30, 20, 0);
			numGameObj.transform.position = new Vector3(transform.position.x + basex + per1x * x + 0.28f - 0.1f, transform.position.y + 0.2f, 2);
		}
	}

	/// <summary>
	/// 駒を浮かせる
	/// </summary>
	/// <param name="objName"></param>
	public void FloatPieceObj(string objName)
	{
		GameObject pieceObj = GameObject.Find(objName);
		BoardUtility.FloatObject(pieceObj);
	}

	/// <summary>
	/// 駒を浮かせた駒を元に戻す
	/// </summary>
	/// <param name="objName"></param>
	public void ResetFloatPieceObj(string objName)
	{
		GameObject pieceObj = GameObject.Find(objName);
		BoardUtility.ResetFloatObject(pieceObj);
	}

	/// <summary>
	/// 駒を点滅させる
	/// </summary>
	/// <param name="objName"></param>
	public void BlinkPieceObj(string objName)
	{
		GameObject pieceObj = GameObject.Find(objName);
		BoardUtility.BlinkObject(pieceObj);
	}

	/// <summary>
	/// 点滅させた駒を元に戻す
	/// </summary>
	/// <param name="objName"></param>
	public void ResetBlinkPieceObj(string objName)
	{
		GameObject pieceObj = GameObject.Find(objName);
		BoardUtility.ResetBlinkObject(pieceObj);
	}
}
