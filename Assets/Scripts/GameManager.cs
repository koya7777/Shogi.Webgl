using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	/// <summary>
	/// ゲームステート
	/// </summary>
	enum GameState
    {
		/// <summary>
		/// 何もしない
		/// </summary>
		None,
		/// <summary>
		/// 駒選択
		/// </summary>
		Choose,
		/// <summary>
		/// 駒移動
		/// </summary>
		Move,
		/// <summary>
		/// 駒成り
		/// </summary>
		Promote,
    }

	/// <summary>
	/// 現在の状態
	/// </summary>
	GameState currentState;
	/// <summary>
	/// ゲームオーバーオブジェクト
	/// </summary>
	[SerializeField] GameObject gameOverObj;
	/// <summary>
	/// 待ったボタンオブジェクト
	/// </summary>
	[SerializeField] GameObject undoButtonObj;
	/// <summary>
	///	対局マネージャー
	/// </summary>
	TurnManager turnManager;
	/// <summary>
	/// 局面マネージャー
	/// </summary>
	BoardManager boardManager;
	/// <summary>
	/// マススクリプト
	/// </summary>
	SquareManager squareManager;
	/// <summary>
	/// 駒の移動先
	/// </summary>
	Address moveTo = new Address();
	/// <summary>
	/// 駒の移動元
	/// </summary>
	Address moveFrom = new Address();

	// Use this for initialization
	void Start()
	{
		currentState = GameState.Choose;
		turnManager = TurnManager.Instance;
		turnManager.SetGameOverObj(gameOverObj);
		turnManager.SetUndoButtonObj(undoButtonObj);
		turnManager._nowPlayer = NowPlayer.Black;
		turnManager.Reset();
		turnManager.Init();

		boardManager = BoardManager.Instance;
		squareManager = GameObject.Find("Canvas/Square").GetComponent<SquareManager>();
		SelectMode(turnManager.mode);
	}

	/// <summary>
	/// ゲームモードの選択
	/// </summary>
	/// <param name="mode"></param>
	void SelectMode(MatchMode mode)
    {
		switch (mode)
		{
			case MatchMode.HumanVsHuman:
				StartCoroutine(HumanVsHumanMain());
				break;
			case MatchMode.HumanVsAI:
				StartCoroutine(HumanVsAIMain());
				break;
			case MatchMode.AIVsAI:
				StartCoroutine(AIVsAIMain());
				break;
			default:
				break;
		}
	}

	// Update is called once per frame
	void Update()
	{
		
	}

    /// <summary>
    /// 人対人
    /// </summary>
    /// <returns></returns>
    IEnumerator HumanVsHumanMain()
    {
		while (true)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Collider2D collider = null;
				yield return new WaitUntil(() =>
				{
					collider = GetCollider();
					return collider;
				});

				switch (currentState)
				{
					case GameState.Choose:
						StartCoroutine(ChoosePiece(collider));
						break;
					case GameState.Move:
						StartCoroutine(MovePiece(collider));
						break;
					case GameState.Promote:
						StartCoroutine(PromotePiece(collider));
						break;
					default:
						break;
				}
				currentState = ToNextState();
			}
			yield return null;
		}
	}

	/// <summary>
	/// 人対AI
	/// </summary>
	/// <returns></returns>
	IEnumerator HumanVsAIMain()
	{
		while (true)
		{
			if (IsBlack)
			{
				if (Input.GetMouseButtonDown(0))
				{
					Collider2D collider = GetCollider();
					switch (currentState)
					{
						case GameState.Choose:
							StartCoroutine(ChoosePiece(collider));
							break;
						case GameState.Move:
							StartCoroutine(MovePiece(collider));
							break;
						case GameState.Promote:
							StartCoroutine(PromotePiece(collider));
							break;
						default:
							break;
					}
					currentState = ToNextState();
				}
			}
			else if(IsWhite)
			{
				switch (currentState)
				{
					case GameState.Choose:
						StartCoroutine(ChoosePiece());
						break;
					case GameState.Move:
						StartCoroutine(MovePiece());
						break;
					case GameState.Promote:
						StartCoroutine(PromotePiece());
						break;
					default:
						break;
				}
				currentState = ToNextState();
			}
			yield return null;
		}
	}

	/// <summary>
	/// AI対AI
	/// </summary>
	/// <returns></returns>
	IEnumerator AIVsAIMain()
	{
		turnManager.ExecAI();

		while (true)
		{
			switch (currentState)
			{
				case GameState.Choose:
					StartCoroutine(ChoosePiece());
					break;
				case GameState.Move:
					StartCoroutine(MovePiece());
					break;
				case GameState.Promote:
					StartCoroutine(PromotePiece());
					break;
				default:
					break;
			}
			currentState = ToNextState();
			yield return null;
		}
	}

	/// <summary>
	/// 次の状態に遷移する
	/// </summary>
	/// <returns></returns>
	GameState ToNextState()
	{
		// 成り駒選択
		if (squareManager.IsPromotedBg)
		{
			return GameState.Promote;
		}
		// 駒移動
		else if (squareManager.chooseFlag)
		{
			return GameState.Move;
		}
		// 駒選択
		else
		{
			return GameState.Choose;
		}
	}

	/// <summary>
	/// 駒のコライダーを取得
	/// </summary>
	/// <returns></returns>
	Collider2D GetCollider()
	{
		Vector2 tap = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Collider2D collider = Physics2D.OverlapPoint(tap);
		return collider;
	}

	/// <summary>
	/// 駒選択
	/// </summary>
	/// <param name="masuScript"></param>
	/// <param name="name"></param>
	IEnumerator ChoosePiece(Collider2D collider = null)
	{
		string objName = string.Empty;
		if (collider)
		{
			objName = collider.transform.gameObject.name;
		}
		else
		{
			if (!SelectPieceAI(out objName))
			{
				yield break;
			}
		}

		if (IsEnemyPiece(objName))
		{
			yield break;
		}

		squareManager.ChoosePiece(objName);

		yield return null;
	}

	/// <summary>
	/// サーバーからの応答待ちのとき
	/// </summary>
	/// <returns></returns>
	bool IsWaitingAIResponse => turnManager.GetRequestFlag();

	/// <summary>
	/// 選択可能な駒があればtrueを返す
	/// </summary>
	/// <param name="objName">選択可能な駒オブジェクト名</param>
	/// <returns></returns>
	bool SelectPieceAI(out string objName)
    {
		objName = string.Empty;

		if (IsWaitingAIResponse)
		{
			return false;
		}
		squareManager.DeleteChangeColorSquareObj();

		do
		{
            if (turnManager.IsGameOver)
            {
				return false;
            }
            if (turnManager.IsStopAI)
            {
				return false;
            }
            if (!DoBestMove(out objName))
            {
				return false;
            }
		}
		while (!BoardUtility.IsMovable(objName));	//選択した駒が移動可能になるまで続ける

		return true;
	}

	/// <summary>
	/// 持ち駒を取得
	/// </summary>
	/// <param name="bestMove"></param>
	/// <returns></returns>
	string GetCapturedPieceAsSfenOne(string bestMove)
    {
		return IsBlack ? bestMove.Substring(0, 1) : bestMove.Substring(0, 1).ToLower();
	}

	/// <summary>
	/// AIが返したBestMoveを実行する
	/// </summary>
	/// <param name="objName"></param>
	/// <returns>エラー時はfalseを返す</returns>
	bool DoBestMove(out string objName)
    {
		objName = string.Empty;
		moveFrom = new Address();
		moveTo = new Address();

		string bestmove = turnManager.GetBestMove();
		CheckBestMove checkBestMove = new CheckBestMove();

        switch (checkBestMove.CheckBestMoveState(bestmove))
        {
            case MoveState.Timeout:
            case MoveState.ServerError:
				RandomPiece(out objName);
				break;
			case MoveState.Win:
				turnManager.GameOver();
				break;
			case MoveState.Drop:
				DropPiece(bestmove, out objName);
				break;
            case MoveState.Move:
				MovePiece(bestmove, out objName);
				break;
            default:
                break;
        }

		return true;
	}

	void RandomPiece(out string objName)
    {
		objName = SelectRandomPieceObj(turnManager._nowPlayer);
	}

	/// <summary>
	/// 駒打ち
	/// </summary>
	/// <param name="bestmove"></param>
	/// <param name="objName">駒の名前</param>
	void DropPiece(string bestmove, out string objName)
    {
		moveTo = new Address(int.Parse(bestmove.Substring(2, 1)),
				BoardUtility.ConvertAlphabetToNumber(bestmove.Substring(3, 1)));
		squareManager.ChangeColorSquare(moveTo);
		string sfenOne = GetCapturedPieceAsSfenOne(bestmove);
		var pieceType = SfenManager.Instance.GetPieceTypeBySfenOne(sfenOne);
		var capturedPieceName = PieceType.CapturedPiece.ToString() + pieceType.ToString();
		var capturedPieceType = BoardUtility.GetPieceType(capturedPieceName);
		objName = BoardUtility.GetObjName(capturedPieceType);
		if (BoardUtility.IsEnemyPiece(pieceType, turnManager._nowPlayer))
		{
			objName = SelectRandomPieceObj(turnManager._nowPlayer);
		}
		turnManager.ResetBestMove();
	}

	/// <summary>
	/// 駒移動
	/// </summary>
	/// <param name="bestmove"></param>
	/// <param name="objName">駒の名前</param>
	void MovePiece(string bestmove, out string objName)
    {
		moveFrom = new Address(int.Parse(bestmove.Substring(0, 1)),
				BoardUtility.ConvertAlphabetToNumber(bestmove.Substring(1, 1)));
		moveTo = new Address(int.Parse(bestmove.Substring(2, 1)),
			BoardUtility.ConvertAlphabetToNumber(bestmove.Substring(3, 1)));
		squareManager.ChangeColorSquare(moveTo, moveFrom);
		Square square = boardManager.GetSquare(moveFrom);
		var pieceType = square.PieceType;
		objName = square.ObjName;
		if (BoardUtility.IsEnemyPiece(pieceType, turnManager._nowPlayer))
		{
			objName = SelectRandomPieceObj(turnManager._nowPlayer);
		}
		turnManager.ResetBestMove();
	}

	/// <summary>
	/// 選択可能な駒をランダムに選ぶ
	/// </summary>
	/// <returns></returns>
	string SelectRandomPieceObj(NowPlayer nowPlayer)
    {
		var pieces = GetSelectablePieceObjects(nowPlayer);
		return pieces[UnityEngine.Random.Range(0, pieces.Count)].ObjName;
	}

	/// <summary>
	/// 敵駒のとき（先手の場合:後手の駒、後手の場合:先手の駒）
	/// </summary>
	/// <param name="objName"></param>
	/// <returns></returns>
	bool IsEnemyPiece(string objName)
	{
		var piece = GameObject.Find("Canvas").GetComponentsInChildren<PieceController>().Where(x => x.ObjName == objName).FirstOrDefault().pieceInfo;
		return BoardUtility.IsEnemyPiece(piece.PieceType, turnManager._nowPlayer);
	}

	/// <summary>
	/// 後手のとき
	/// </summary>
	/// <returns></returns>
	bool IsWhite => turnManager._nowPlayer == NowPlayer.White;

	/// <summary>
	/// 先手のとき
	/// </summary>
	/// <returns></returns>
	bool IsBlack => turnManager._nowPlayer == NowPlayer.Black;

	/// <summary>
	/// 駒移動
	/// </summary>
	/// <param name="collider"></param>
	IEnumerator MovePiece(Collider2D collider = null)
    {
		string objName = string.Empty;

		if (collider)
		{
			objName = collider.transform.gameObject.name;
		}
        else
        {
            if (!MovePieceAI(out objName))
            {
				yield break;
            }
		}

		if (objName.IndexOf("PieceMovable") > -1)
		{
			var pieceMovable = GameObject.Find(objName).GetComponent<PieceMovable>();
			squareManager.MovePiece(squareManager.choosePieceObjName, pieceMovable.Address);
			squareManager.DeleteChooseMoves();
			ResetBlinkPiece();

			if (!squareManager.IsPromotedBg)
			{
				StartCoroutine(turnManager.TakeTurn());
			}
			yield break;
		}

		ResetFloatPiece();
		ResetBlinkPiece();

		squareManager.DeleteChooseMoves();

		yield return null;
	}

	/// <summary>
	/// AIが返した移動先に駒が移動可能な場合はtrueを返す
	/// </summary>
	/// <param name="objName">移動する駒のオブジェクト名を返す</param>
	/// <returns></returns>
	bool MovePieceAI(out string objName)
    {
		var movables = GetMovablePieceObjects();
		if (!IsPieceMovable(movables, out objName))
        {
			objName = movables[Random.Range(0, movables.Count)].name;
		}
		return true;
	}

	/// <summary>
	/// 移動先vecToに駒が移動可能であればtrueを返し、引数にオブジェクト名を返す
	/// </summary>
	/// <param name="movables">移動可能範囲</param>
	/// <param name="objName"></param>
	/// <returns></returns>
	bool IsPieceMovable(List<PieceMovable> movables, out string objName)
    {
		objName = string.Empty;
		foreach (var movable in movables)
		{
			if (movable.Address == moveTo)
			{
				objName = movable.name;
				return true;
			}
		}
		return false;
    }

	/// <summary>
	/// 駒の浮き駒戻し
	/// </summary>
	void ResetFloatPiece()
    {
		var pieceName = BoardUtility.GetPieceName(squareManager.choosePieceObjName);
		var pieceType = BoardUtility.GetPieceType(pieceName);
		// 持ち駒でない時の浮き駒戻し
		if (squareManager.choosePieceObjName.IndexOf(PieceType.CapturedPiece.ToString()) == -1)
		{
			squareManager.ResetFloatPieceObj(squareManager.choosePieceObjName);
		}
		else
		{
			// 持ち駒の時の浮き駒戻し
			if (BoardUtility.IsBlackPiece(pieceType))
			{
				GameObject capturePieceObj = GameObject.Find("Canvas/CaptureBlack");
				CaptureManager motigomaScript = capturePieceObj.GetComponent<CaptureManager>();
				motigomaScript.ResetFloatPieceObj(squareManager.choosePieceObjName);
			}
			else
			{
				GameObject motigomaObj = GameObject.Find("Canvas/CaptureWhite");
				CaptureManager capturePiece = motigomaObj.GetComponent<CaptureManager>();
				capturePiece.ResetFloatPieceObj(squareManager.choosePieceObjName);
			}
		}
	}

	/// <summary>
	/// 駒の点滅を元に戻す
	/// </summary>
	void ResetBlinkPiece()
	{
		var pieceName = BoardUtility.GetPieceName(squareManager.choosePieceObjName);
		var pieceType = BoardUtility.GetPieceType(pieceName);
		// 持ち駒でない時の浮き駒戻し
		if (pieceType < PieceType.CapturedPiece)
		{
			squareManager.ResetBlinkKomaObj(squareManager.choosePieceObjName);
		}
		else
		{
			// 持ち駒の時の浮き駒戻し
			if (BoardUtility.IsBlackPiece(pieceType))
			{
				GameObject captureObj = GameObject.Find("Canvas/CaptureBlack");
				CaptureManager capture = captureObj.GetComponent<CaptureManager>();
				capture.ResetBlinkPieceObj(squareManager.choosePieceObjName);
			}
			else
			{
				GameObject captureObj = GameObject.Find("Canvas/CaptureWhite");
				CaptureManager capture = captureObj.GetComponent<CaptureManager>();
				capture.ResetBlinkPieceObj(squareManager.choosePieceObjName);
			}
		}
	}

	/// <summary>
	/// 駒を成る
	/// </summary>
	/// <param name="collider"></param>
	/// <returns></returns>
	IEnumerator PromotePiece(Collider2D collider = null)
    {
		if (collider)
		{
			string objName = collider.transform.gameObject.name;
			if (objName.IndexOf(PieceConst.promBg) > -1)
			{
				// 駒を成る選択をした時
				if (objName.Equals(PieceConst.promBg + "1"))
				{
					squareManager.PromotePiece(squareManager.choosePieceObjName
						, squareManager.promotedBgName
						, new Address(squareManager.promotedBgX, squareManager.promotedBgY)
					);
				}
				squareManager.DestroyPromotedBg();
				squareManager.IsPromotedBg = false;
				StartCoroutine(turnManager.TakeTurn());
			}
        }
        else
        {
			// 駒を成る選択をした時
			if (Random.Range(0,1) == 0)
			{
				squareManager.PromotePiece(squareManager.choosePieceObjName
					, squareManager.promotedBgName
					, new Address(squareManager.promotedBgX, squareManager.promotedBgY)
				);
			}
			squareManager.DestroyPromotedBg();
			squareManager.IsPromotedBg = false;
			StartCoroutine(turnManager.TakeTurn());
		}

		yield return null;
	}

	/// <summary>
	/// 選択可能な駒のリストを取得
	/// </summary>
	/// <returns></returns>
	List<PieceController> GetSelectablePieceObjects(NowPlayer nowPlayer)
    {
		return BoardUtility.GetSelectablePieceObjects(nowPlayer);
    }

	/// <summary>
	/// 移動可能な駒のリストを取得
	/// </summary>
	/// <returns></returns>
	List<PieceMovable> GetMovablePieceObjects()
    {
		return BoardUtility.GetMovablePieceObjects();
	}
}
