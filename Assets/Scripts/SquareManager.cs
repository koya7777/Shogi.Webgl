using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SquareManager : MonoBehaviour
{
	[SerializeField] float per1x = 0.42f;
	[SerializeField] float per1y = 0.43f;
	[SerializeField] float basex = 2.05f;
	[SerializeField] float basey = 3.22f;
	/// <summary>
	/// 駒の移動表示フラグ
	/// </summary>
	public bool chooseFlag = false;
	/// <summary>
	/// 成り駒BGが表示されているかフラグ
	/// </summary>
	public bool IsPromotedBg = false;
	/// <summary>
	/// 選択駒オブジェクト名
	/// </summary>
	public string choosePieceObjName;
	/// <summary>
	/// 選択駒の移動可能座標リスト
	/// </summary>
	public List<GameObject> chooseMoves = new List<GameObject>();
	/// <summary>
	/// 成駒BGを開いた時の選択駒のx座標
	/// </summary>
	public int promotedBgX = 0;
	/// <summary>
	/// 成駒BGを開いた時の選択駒のy座標
	/// </summary>
	public int promotedBgY = 0;
	/// <summary>
	/// 成駒BGを開いた時の駒名
	/// </summary>
	public string promotedBgName = "";

	// Use this for initialization
	private void Start()
	{
		var manager = BoardManager.Instance;
		manager.Init();
		manager.InitHirate();
		StartCoroutine(UpdateBoard());
	}

	private IEnumerator UpdateBoard()
	{
		yield return new WaitForSeconds(0.1f);
		RefreshPiece();
	}

	/// <summary>
	/// 選択フラグと成駒フラグをリセットする
	/// </summary>
	public void ResetFlag()
	{
		chooseFlag = false;
		IsPromotedBg = false;
	}

	/// <summary>
	/// 将棋盤のマスを空にする
	/// </summary>
	public void Empty()
	{
		var manager = BoardManager.Instance;
		for (int y = 1; y <= 9; y++)
		{
			for (int x = 1; x <= 9; x++)
			{
				Square square = manager.GetSquare(x, y);
				if (square.IsExist)
				{
					DestroyPieceObj(square.ObjName);
				}
				square.Init(x, y);
			}
		}
	}

	/// <summary>
	/// 将棋盤のマスに駒を生成する
	/// </summary>
	public void RefreshPiece()
	{
		var manager = BoardManager.Instance;
		for (int y = 1; y <= 9; y++)
		{
			for (int x = 1; x <= 9; x++)
			{
				Square square = manager.GetSquare(new Address(x, y));
				if (square.IsExist)
				{
					string objName = CreatePieceObj(square.PieceType.ToString(), new Address(x, y));
					manager.SetSquareObjName(objName, new Address(x, y));
				}
			}
		}
	}

	/// <summary>
	/// 駒オブジェクトを作成
	/// </summary>
	/// <param name="pieceName">駒名</param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns>駒オブジェクト名</returns>
	public string CreatePieceObj(string pieceName, Address address)
	{

		var gameObj = new GameObject();
		gameObj.AddComponent<SpriteRenderer>().sprite = GetPieceSprite(pieceName);
		gameObj.transform.parent = FindObjectOfType<Canvas>().transform;
		var objName = pieceName + "_" + PieceManager.Instance.IssuePieceId();
		gameObj.transform.name = objName;
		gameObj.transform.localScale = new Vector3(100, 100, 0);
		gameObj.transform.position = new Vector3(basex - per1x * address.X, basey - per1y * address.Y, 2);
		gameObj.AddComponent<BoxCollider2D>();
		gameObj.AddComponent<PieceController>().SetPiece(address, objName);
		return objName;
	}

	/// <summary>
	/// 駒名から駒画像を取得
	/// </summary>
	/// <param name="pieceName"></param>
	/// <returns></returns>
	Sprite GetPieceSprite(string pieceName)
	{
		var sprites = Resources.LoadAll<Sprite>("pieces");
		var sprite = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(pieceName));
		return sprite;
	}

	/// <summary>
	/// 駒削除
	/// </summary>
	/// <param name="objName"></param>
	public void DestroyPieceObj(string objName)
	{
		var pieceObj = transform.Find("../" + objName).gameObject;
		Destroy(pieceObj);
	}

	/// <summary>
	/// 駒を浮かせる
	/// </summary>
	/// <param name="objName"></param>
	public void FloatPieceObj(string objName)
	{
		var pieceObj = GameObject.Find(objName);
		BoardUtility.FloatObject(pieceObj);
	}

	/// <summary>
	/// 浮駒を元に戻す
	/// </summary>
	/// <param name="objName"></param>
	public void ResetFloatPieceObj(string objName)
	{
		var pieceObj = GameObject.Find(objName);
		BoardUtility.ResetFloatObject(pieceObj);
	}

	/// <summary>
	/// 駒を点滅させる
	/// </summary>
	/// <param name="objName"></param>
	public void BlinkPieceObj(string objName)
	{
		var pieceObj = GameObject.Find(objName);
		BoardUtility.BlinkObject(pieceObj);
	}

	/// <summary>
	/// 駒の点滅を元に戻す
	/// </summary>
	/// <param name="objName"></param>
	public void ResetBlinkKomaObj(string objName)
	{
		var pieceObj = GameObject.Find(objName);
		BoardUtility.ResetBlinkObject(pieceObj);
	}

	/// <summary>
	/// 駒の色を変える
	/// </summary>
	/// <param name="objName"></param>
	public void ChangeColorPieceObj(string objName)
	{
		var pieceObj = GameObject.Find(objName);
		BoardUtility.ChangeColorObject(pieceObj);
	}

	/// <summary>
	/// 駒の色を元に戻す
	/// </summary>
	/// <param name="objName"></param>
	public void ResetColorPieceObj(string objName)
	{
		var pieceObj = GameObject.Find(objName);
		BoardUtility.ResetChangeColorObject(pieceObj);
	}

	/// <summary>
	/// 駒の移動可能範囲の表示
	/// </summary>
	/// <param name="moves"></param>
	/// <param name="name"></param>
	public void RefreshMovables(List<Address> moves, string name)
	{
		var choosePieceMoves = new List<GameObject>();
		if (!chooseFlag)
		{
			int movableID = 0;
			foreach (var move in moves)
			{
				if (!move.IsValid())
				{
					continue;
				}
				var gameObj = new GameObject();
				gameObj.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PieceMovable");
				gameObj.transform.parent = FindObjectOfType<Canvas>().transform;
				gameObj.transform.localScale = new Vector3(140, 140, 0);
				gameObj.transform.name = "PieceMovable" + movableID++;
				gameObj.transform.position = new Vector3(basex - per1x * (move.X), basey - per1y * (move.Y), 0);
				var box = gameObj.AddComponent<BoxCollider2D>() as BoxCollider2D;
				var PieceMovable = gameObj.AddComponent<PieceMovable>();
				PieceMovable.Address = move;
				choosePieceMoves.Add(gameObj);
			}
			chooseFlag = true;
			chooseMoves = choosePieceMoves;
			choosePieceObjName = name;
		}
	}

	/// <summary>
	/// 駒の移動可能オブジェクトを削除する
	/// </summary>
	public void ResetMovables()
	{
		var movables = GameObject.Find("Canvas").GetComponentsInChildren<PieceMovable>().ToList();
		foreach (var movable in movables)
		{
			Destroy(movable.gameObject);
		}
	}

	/// <summary>
	/// マスの色を変える
	/// </summary>
	/// <param name="vecTo"></param>
	public void ChangeColorSquare(Address vecTo)
	{
		var sp = Resources.Load<Sprite>("PieceMovable");

		var gameObj = new GameObject();
		var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sp;
		gameObj.transform.parent = FindObjectOfType<Canvas>().transform;
		gameObj.transform.localScale = new Vector3(140, 140, 0);
		gameObj.transform.name = "VecTo";
		gameObj.transform.position = new Vector3(basex - per1x * vecTo.X, basey - per1y * vecTo.Y, 0);
		var masuChangeColor = gameObj.AddComponent<SquareChangeColor>();
	}

	/// <summary>
	/// マスの色を変える
	/// </summary>
	/// <param name="vecTo"></param>
	/// <param name="vecFrom"></param>
	public void ChangeColorSquare(Address vecTo, Address vecFrom)
	{
		var sp = Resources.Load<Sprite>("PieceMovable");

		var gameObjTo = new GameObject();
		var spriteRendererTo = gameObjTo.AddComponent<SpriteRenderer>();
		spriteRendererTo.sprite = sp;
		gameObjTo.transform.parent = FindObjectOfType<Canvas>().transform;
		gameObjTo.transform.localScale = new Vector3(140, 140, 0);
		gameObjTo.transform.name = "VecTo";
		gameObjTo.transform.position = new Vector3(basex - per1x * vecTo.X, basey - per1y * vecTo.Y, 0);
		var masuChangeColorTo = gameObjTo.AddComponent<SquareChangeColor>();

		var gameObjFrom = new GameObject();
		var spriteRendererFrom = gameObjFrom.AddComponent<SpriteRenderer>();
		spriteRendererFrom.sprite = sp;
		gameObjFrom.transform.parent = FindObjectOfType<Canvas>().transform;
		gameObjFrom.transform.localScale = new Vector3(140, 140, 0);
		gameObjFrom.transform.name = "VecFrom";
		gameObjFrom.transform.position = new Vector3(basex - per1x * vecFrom.X, basey - per1y * vecFrom.Y, 0);
		var masuChangeColorFrom = gameObjFrom.AddComponent<SquareChangeColor>();
	}

	/// <summary>
	/// マスの色オブジェクトを削除する
	/// </summary>
	public void DeleteChangeColorSquareObj()
	{
		SquareChangeColor[] squares = GameObject.Find("/Canvas").GetComponentsInChildren<SquareChangeColor>();
		foreach (var square in squares)
		{
			Destroy(square.gameObject);
		}
	}

	/// <summary>
	/// 移動可能範囲のマス表示を元に戻す
	/// </summary>
	public void DeleteChooseMoves()
	{
		if (chooseFlag)
		{
			int i = 0;
			if (chooseMoves != null && chooseMoves.Count > 0)
			{
				foreach (GameObject obj in chooseMoves)
				{
					Destroy(obj);
					i++;
				}
			}
			chooseFlag = false;
		}
	}

	/// <summary>
	/// 駒を成るかどうかの確認をする
	/// </summary>
	/// <param name="objName"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	void CheckPromote(string objName, Address address)
	{
		var gameObj = GameObject.Find(objName);
		var piece = gameObj.GetComponent<PieceController>().pieceInfo;
		if (BoardUtility.CanPromote(piece, address.Y))
		{
			if (BoardUtility.IsNotYetPromote(piece.PieceType))
			{
				if (BoardUtility.IsRequirePromote(piece.PieceType, address))
				{
					PromotePiece(choosePieceObjName, BoardUtility.GetPromPieceName(piece.PieceType), address);
				}
				else
				{
					var pieceName = BoardUtility.GetPieceName(objName);
					var pieceType = BoardUtility.GetPieceType(pieceName);
					CreatePromotedBg(BoardUtility.GetPromPieceName(pieceType), address);
					CreateNormalBg(pieceName, address);
					IsPromotedBg = true;
				}
			}
		}
	}

	/// <summary>
	/// 成駒BGの成り駒側BG作成
	/// </summary>
	/// <param name="objName"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	void CreatePromotedBg(string objName, Address address)
	{
		var sp = Resources.Load<Sprite>("PromotedBg");
		var gameObj = new GameObject();
		var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sp;
		spriteRenderer.color = new Color(0.8f, 0.8f, 0.5f, 1f);
		gameObj.transform.parent = FindObjectOfType<Canvas>().transform;
		gameObj.transform.name = PieceConst.promBg + "1";
		gameObj.transform.localScale = new Vector3(150, 200, 0);
		float xPlus = 0;
		if (address.X == 9)
		{
			xPlus = 0.20f;
		}
		else if (address.X == 1)
		{
			xPlus = -0.15f;
		}
		gameObj.transform.position = new Vector3(basex - per1x * address.X - 0.30f + xPlus, basey - per1y * address.Y, -2);
		gameObj.AddComponent<BoxCollider2D>();
		gameObj.AddComponent<PromotedBgScript>();
		promotedBgX = address.X;
		promotedBgY = address.Y;
		promotedBgName = objName;
		CreatePieceObjOnPromotedBG(objName, address);
	}

	/// <summary>
	/// 成駒BGの通常駒側BG作成
	/// </summary>
	/// <param name="objName"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	void CreateNormalBg(string objName, Address address)
	{
		var sp = Resources.Load<Sprite>("PromotedBg");
		var gameObj = new GameObject();
		var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sp;
		spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
		gameObj.transform.parent = FindObjectOfType<Canvas>().transform;
		gameObj.transform.name = PieceConst.promBg + "2";
		gameObj.transform.localScale = new Vector3(150, 200, 0);
		float xPlus = 0;
		if (address.X == 9)
		{
			xPlus = 0.20f;
		}
		else if (address.X == 1)
		{
			xPlus = -0.15f;
		}
		gameObj.transform.position = new Vector3(basex - per1x * address.X + 0.30f + xPlus, basey - per1y * address.Y, -2);
		gameObj.AddComponent<BoxCollider2D>();
		gameObj.AddComponent<PromotedBgScript>();
		CreatePieceObjOnNormalBG(objName, address);
	}

	/// <summary>
	/// 成駒BGの成駒側内部駒作成
	/// </summary>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void CreatePieceObjOnPromotedBG(string name, Address address)
	{
		var sprites = Resources.LoadAll<Sprite>("pieces");
		var sp = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(name));
		var gameObj = new GameObject();
		var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sp;
		gameObj.transform.parent = FindObjectOfType<Canvas>().transform;
		gameObj.transform.name = PieceConst.promBg + "piece1";
		gameObj.transform.localScale = new Vector3(100, 100, 0);
		float xPlus = 0;
		if (address.X == 9)
		{
			xPlus = 0.20f;
		}
		else if (address.X == 1)
		{
			xPlus = -0.15f;
		}
		gameObj.transform.position = new Vector3(basex - per1x * address.X - 0.30f + xPlus, basey - per1y * address.Y, -2);
		spriteRenderer.sortingOrder = 1;
	}

	/// <summary>
	/// 成駒BGの通常側内部駒作成
	/// </summary>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void CreatePieceObjOnNormalBG(string name, Address address)
	{
		var sprites = Resources.LoadAll<Sprite>("pieces");
		var sp = System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(name));
		var gameObj = new GameObject();
		var spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = sp;
		gameObj.transform.parent = FindObjectOfType<Canvas>().transform;
		gameObj.transform.name = PieceConst.promBg + "piece2";
		gameObj.transform.localScale = new Vector3(100, 100, 0);
		float xPlus = 0;
		if (address.X == 9)
		{
			xPlus = 0.20f;
		}
		else if (address.X == 1)
		{
			xPlus = -0.15f;
		}
		gameObj.transform.position = new Vector3(basex - per1x * address.X + 0.30f + xPlus, basey - per1y * address.Y, -2);
		spriteRenderer.sortingOrder = 1;
	}

	/// <summary>
	/// 成駒BGを破棄する
	/// </summary>
	public void DestroyPromotedBg()
	{
		var obj = transform.Find("../" + PieceConst.promBg + "1").gameObject;
		var obj2 = transform.Find("../" + PieceConst.promBg + "2").gameObject;
		var objPiece = transform.Find("../" + PieceConst.promBg + "piece1").gameObject;
		var objPiece2 = transform.Find("../" + PieceConst.promBg + "piece2").gameObject;
		Destroy(obj);
		Destroy(obj2);
		Destroy(objPiece);
		Destroy(objPiece2);
	}

	/// <summary>
	/// 駒を選択する
	/// </summary>
	/// <param name="objName"></param>
	public void ChoosePiece(string objName)
	{
        if (IsCapturedPiece(objName))
        {
			ChooseCapturedPiece(objName);
		}
        else
        {
			ChooseOnBoardPiece(objName);
		}
	}

	/// <summary>
	/// 盤上の駒を選択
	/// </summary>
	/// <param name="objName"></param>
	/// <returns></returns>
	void ChooseOnBoardPiece(string objName)
	{
		FloatPieceObj(objName);

		var pieceType = BoardUtility.GetPieceType(objName.Split(new char[] { '_' })[0]);
		var pieceInfo = GameObject.Find(objName).GetComponent<PieceController>().pieceInfo;
		var manager = BoardManager.Instance;
		var moves = PieceFactory.Instance.Create(pieceType).GetOnBoardMoves(manager, pieceInfo, true);
		RefreshMovables(moves, objName);
	}

	/// <summary>
	/// 持ち駒選択処理
	/// </summary>
	/// <param name="objName"></param>
	/// <returns></returns>
	void ChooseCapturedPiece(string objName)
	{
		var pieceType = BoardUtility.GetPieceType(BoardUtility.GetPieceName(objName));
		// 選択した持ち駒を浮かせて点滅させる
		var str = BoardUtility.IsBlackPiece(pieceType) ? "../CaptureBlack" : "../CaptureWhite";
		var capture = transform.Find(str).gameObject.GetComponent<CaptureManager>();
		capture.FloatPieceObj(objName);
		capture.BlinkPieceObj(objName);

		var moves = PieceFactory.Instance.Create(pieceType).GetDropMoves(pieceType);
		RefreshMovables(moves, objName);
	}

	/// <summary>
	/// 駒を移動する
	/// </summary>
	/// <param name="pieceObjName">オブジェクト名</param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void MovePiece(string pieceObjName, Address address)
	{
		if (IsCapturedPiece(pieceObjName))
		{
			DropPiece(pieceObjName, address);
		}
		else
		{
			MoveOnBoradPiece(pieceObjName, address);
		}
	}

	/// <summary>
	/// 持ち駒のとき
	/// </summary>
	/// <param name="objName"></param>
	/// <returns></returns>
	bool IsCapturedPiece(string objName)
	{
		var pieceType = BoardUtility.GetPieceType(BoardUtility.GetPieceName(objName));
		return pieceType > PieceType.CapturedPiece;
	}

	/// <summary>
	/// 駒を打つ
	/// </summary>
	/// <param name="pieceObjName"></param>
	/// <param name="address"></param>
	void DropPiece(string pieceObjName, Address address)
    {
		var pieceType = BoardUtility.GetPieceType(BoardUtility.GetPieceName(pieceObjName));
		pieceObjName = pieceObjName.Replace(PieceType.CapturedPiece.ToString(), "");
		var pieceName = BoardUtility.GetPieceName(pieceObjName);
		CapturedPieceManager.Instance.Minus(pieceType);
		if (BoardUtility.IsBlackPiece(pieceType))
		{
			GameObject obj = transform.Find("../CaptureBlack").gameObject;
			CaptureManager capture = obj.GetComponent<CaptureManager>();
			capture.RefreshPiece();
		}
		else
		{
			GameObject obj = transform.Find("../CaptureWhite").gameObject;
			CaptureManager capture = obj.GetComponent<CaptureManager>();
			capture.RefreshPiece();
		}
		pieceObjName = CreatePieceObj(pieceObjName, address);
		var boad = BoardManager.Instance;
		boad.SetSquare(address, BoardUtility.GetPieceType(pieceName));
		boad.SetSquareObjName(pieceObjName, address);
		var square = GameObject.Find("/Canvas/Square").GetComponent<SquareManager>();
		square.choosePieceObjName = pieceObjName;
		MovePieceObj(pieceObjName, address);
	}

	/// <summary>
	/// 盤上の駒を動かす
	/// </summary>
	/// <param name="pieceObjName"></param>
	/// <param name="address"></param>
	void MoveOnBoradPiece(string pieceObjName, Address address)
    {
		var piece = GameObject.Find(pieceObjName).GetComponent<PieceController>().pieceInfo;
		CapturePiece(pieceObjName, address);
		CheckPromote(pieceObjName, address);
		BoardManager manager = BoardManager.Instance;
		manager.EmptySquare(piece.Address);
		piece.Address = new Address(address);
		var pieceName = BoardUtility.GetPieceName(pieceObjName);
		manager.SetSquare(address, BoardUtility.GetPieceType(pieceName));
		manager.SetSquareObjName(pieceObjName, address);
		MovePieceObj(pieceObjName, address);
	}

	/// <summary>
	/// 駒オブジェクトの移動
	/// </summary>
	/// <param name="pieceObjName"></param>
	/// <param name="address"></param>
	void MovePieceObj(string pieceObjName, Address address)
    {
		var gameObj = GameObject.Find(pieceObjName);
		var piece = gameObj.GetComponent<PieceController>().pieceInfo;
		gameObj.transform.position = new Vector3(basex - per1x * address.X, basey - per1y * address.Y, 2);
	}

	/// <summary>
	/// 駒を取る	
	/// </summary>
	/// <param name="objName"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	void CapturePiece(string objName, Address address)
	{
		var gameObj = GameObject.Find(objName);
		var piece = gameObj.GetComponent<PieceController>().pieceInfo;
		var square = BoardManager.Instance.GetSquare(address);
		var capturedPiece = CapturedPieceManager.Instance;
		if (piece.IsBlack && square.IsWhite)
		{
            switch (square.PieceType)
            {
				case PieceType.WKing:
					capturedPiece.Plus(PieceType.BKing);
					break;
				case PieceType.WRook:
				case PieceType.WPromRook:
					capturedPiece.Plus(PieceType.BRook);
					break;
				case PieceType.WBishop:
				case PieceType.WPromBishop:
					capturedPiece.Plus(PieceType.BBishop);
					break;
				case PieceType.WGold:
					capturedPiece.Plus(PieceType.BGold);
					break;
				case PieceType.WSilver:
				case PieceType.WPromSilver:
					capturedPiece.Plus(PieceType.BSilver);
					break;
				case PieceType.WKnight:
				case PieceType.WPromKnight:
					capturedPiece.Plus(PieceType.BKnight);
					break;
				case PieceType.WLance:
				case PieceType.WPromLance:
					capturedPiece.Plus(PieceType.BLance);
					break;
				case PieceType.WPawn:
				case PieceType.WPromPawn:
					capturedPiece.Plus(PieceType.BPawn);
					break;
				default:
					break;
			}
			CaptureManager capture = transform.Find("../CaptureBlack").gameObject.GetComponent<CaptureManager>();
			capture.RefreshPiece();
			DestroyPieceObj(square.ObjName);
		}
		else if (piece.IsWhite && square.IsBlack)
		{
			switch (square.PieceType)
			{
				case PieceType.BKing:
					capturedPiece.Plus(PieceType.WKing);
					break;
				case PieceType.BRook:
				case PieceType.BPromRook:
					capturedPiece.Plus(PieceType.WRook);
					break;
				case PieceType.BBishop:
				case PieceType.BPromBishop:
					capturedPiece.Plus(PieceType.WBishop);
					break;
				case PieceType.BGold:
					capturedPiece.Plus(PieceType.WGold);
					break;
				case PieceType.BSilver:
				case PieceType.BPromSilver:
					capturedPiece.Plus(PieceType.WSilver);
					break;
				case PieceType.BKnight:
				case PieceType.BPromKnight:
					capturedPiece.Plus(PieceType.WKnight);
					break;
				case PieceType.BLance:
				case PieceType.BPromLance:
					capturedPiece.Plus(PieceType.WLance);
					break;
				case PieceType.BPawn:
				case PieceType.BPromPawn:
					capturedPiece.Plus(PieceType.WPawn);
					break;
				default:
					break;
			}
			CaptureManager capture = transform.Find("../CaptureWhite").gameObject.GetComponent<CaptureManager>();
			capture.RefreshPiece();
			DestroyPieceObj(square.ObjName);
		}
	}

	/// <summary>
	///	駒を成る
	/// </summary>
	/// <param name="objName"></param>
	/// <param name="promotedName"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void PromotePiece(string objName, string promotedName, Address address)
	{
		string promotedObjName = CreatePieceObj(promotedName, address);
		SquareManager square = GameObject.Find("/Canvas/Square").GetComponent<SquareManager>();
		square.choosePieceObjName = promotedObjName;
		BoardManager manager = BoardManager.Instance;
		var promotedPieceType = BoardUtility.GetPieceType(promotedName);
		manager.SetSquare(address, promotedPieceType);
		manager.SetSquareObjName(promotedObjName, address);
		DestroyPieceObj(objName);
	}
}
