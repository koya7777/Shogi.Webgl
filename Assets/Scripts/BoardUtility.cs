using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardUtility
{
	/// <summary>
	/// 通常駒名から成駒名を取得する
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	public static string GetPromPieceName(PieceType pieceType)
	{
		return PieceConst.PromPieceTypeByPieceTypeDic[pieceType].ToString();
	}

	/// <summary>
	/// まだ成ってない
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	public static bool IsNotYetPromote(PieceType pieceType)
	{
		return IsNormalPiece(pieceType);
	}

	/// <summary>
	/// 通常駒かどうか
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	static bool IsNormalPiece(PieceType pieceType)
	{
		if (pieceType.Equals(PieceType.BRook)
			|| pieceType.Equals(PieceType.BBishop)
			|| pieceType.Equals(PieceType.BSilver)
			|| pieceType.Equals(PieceType.BKnight)
			|| pieceType.Equals(PieceType.BLance)
			|| pieceType.Equals(PieceType.BPawn)
			|| pieceType.Equals(PieceType.WRook)
			|| pieceType.Equals(PieceType.WBishop)
			|| pieceType.Equals(PieceType.WSilver)
			|| pieceType.Equals(PieceType.WKnight)
			|| pieceType.Equals(PieceType.WLance)
			|| pieceType.Equals(PieceType.WPawn))
		{
			return true;
		}
		return false;
	}

	/// <summary>
	///	成れるかどうか
	/// </summary>
	/// <param name="piece"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static bool CanPromote(PieceInfo piece, int y)
	{
		if (piece.IsBlack)
		{
			if (y <= 3 || piece.Address.Y <= 3 && piece.Address.Y != 0)
			{
				return true;
			}
		}
		else if (piece.IsWhite)
		{
			if (y >= 7 || piece.Address.Y >= 7)
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 成り必須のとき
	/// </summary>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static bool IsRequirePromote(PieceType pieceType, Address address)
	{
		if (address.Y == 1 && (pieceType.Equals(PieceType.BPawn) || pieceType.Equals(PieceType.BLance)))
		{
			return true;
		}
		else if (address.Y <= 2 && (pieceType.Equals(PieceType.BKnight)))
		{
			return true;
		}
		else if (address.Y == 9 && (pieceType.Equals(PieceType.WPawn) || pieceType.Equals(PieceType.WLance)))
		{
			return true;
		}
		else if (address.Y >= 8 && (pieceType.Equals(PieceType.WKnight)))
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// 駒名を取得
	/// </summary>
	/// <param name="pieceType">駒の種類</param>
	/// <returns></returns>
	public static string GetPieceName(PieceType pieceType)
	{
		return PieceConst.PieceNameByPieceTypeDic[pieceType];
	}


	/// <summary>
	/// 先手駒であればtrue
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	public static bool IsBlackPiece(PieceType pieceType)
	{
		PieceType capturedPieceType = (PieceType)(pieceType - PieceType.CapturedPiece);

		if (pieceType > PieceType.BlackPiece && pieceType < PieceType.WhitePiece || //盤上の駒の場合
			capturedPieceType > PieceType.BlackPiece && capturedPieceType < PieceType.WhitePiece)   //持駒の場合
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// 後手駒であればtrue
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	public static bool IsWhitePiece(PieceType pieceType)
	{
		PieceType capturedPieceType = (PieceType)(pieceType - PieceType.CapturedPiece);

		if (pieceType > PieceType.WhitePiece && pieceType < PieceType.CapturedPiece ||   //盤上の駒の場合
			capturedPieceType > PieceType.WhitePiece && capturedPieceType < PieceType.CapturedPiece) //持駒の場合
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// 敵駒（手番が先手のとき後手の駒、後手のとき先手の駒）であればtrue
	/// </summary>
	/// <param name="pieceType"></param>
	/// <param name="nowPlayer"></param>
	/// <returns></returns>
	public static bool IsEnemyPiece(PieceType pieceType, NowPlayer nowPlayer)
	{
		return IsBlackPiece(pieceType) && nowPlayer == NowPlayer.White || IsWhitePiece(pieceType) && nowPlayer == NowPlayer.Black;
	}

	/// <summary>
	/// 自駒（手番が先手のとき先手の駒、後手のとき後手の駒）であればtrue
	/// </summary>
	/// <param name="pieceType"></param>
	/// <param name="nowPlayer"></param>
	/// <returns></returns>
	public static bool IsSelfPiece(PieceType pieceType, NowPlayer nowPlayer)
	{
		return IsBlackPiece(pieceType) && nowPlayer == NowPlayer.Black || IsWhitePiece(pieceType) && nowPlayer == NowPlayer.White;
	}

	/// <summary>
	/// オブジェクト名から駒名を取得
	/// </summary>
	/// <param name="objName">オブジェクト名</param>
	/// <returns></returns>
	public static string GetPieceName(string objName)
	{
		string[] names = objName.Split(new char[] { '_' });
		return names[0];
	}

	/// <summary>
	/// 駒名から駒タイプを取得
	/// </summary>
	/// <param name="pieceName"></param>
	/// <returns></returns>
	public static PieceType GetPieceType(string pieceName)
	{
		return (PieceType)Enum.Parse(typeof(PieceType), pieceName);
	}

	/// <summary>
	/// Square型をPieceController型に変換
	/// </summary>
	/// <param name="square"></param>
	/// <returns></returns>
	public static PieceController ConvertToPieceControllerFrom(Square square)
	{
		return GameObject.Find("Canvas").GetComponentsInChildren<PieceController>().Where(x => x.ObjName == square.ObjName).FirstOrDefault();
	}

	/// <summary>
	/// 駒タイプから駒のオブジェクト名を取得
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	public static String GetObjName(PieceType pieceType)
	{
		return GameObject.Find("Canvas").GetComponentsInChildren<PieceController>().Where(x => x.pieceInfo.PieceType.Equals(pieceType)).FirstOrDefault().ObjName;
	}

	/// <summary>
	/// a～iを1～9の数字に変換する
	/// </summary>
	/// <param name="alphabet"></param>
	/// <returns></returns>
	public static int ConvertAlphabetToNumber(string alphabet)
	{
		switch (alphabet.ToLower())
		{
			case "a":
				return 1;
			case "b":
				return 2;
			case "c":
				return 3;
			case "d":
				return 4;
			case "e":
				return 5;
			case "f":
				return 6;
			case "g":
				return 7;
			case "h":
				return 8;
			case "i":
				return 9;
			default:
				break;
		}
		return -1;
	}

	/// <summary>
	/// 玉の位置を返す
	/// </summary>
	/// <param name="manager"></param>
	/// <param name="reverse"></param>
	/// <returns></returns>
	public static Address GetKingPosition(BoardManager manager, bool reverse)
	{
		PieceType kingType = reverse ? PieceType.WKing : PieceType.BKing;
		var ret = manager.Board.Where(s => s.Value.PieceType == kingType).Select(s => s.Value.Address).FirstOrDefault();
		return ret;
	}

	/// <summary>
	/// 詰み判定
	/// </summary>
	/// <param name="nowPlayer"></param>
	/// <returns></returns>
	public static bool IsGameOver(NowPlayer nowPlayer)
	{
		var pieces = GetSelectablePieceObjects(nowPlayer);

		if (pieces.Count == 0)
			return true;

		return !pieces.Any(piece => GetMovableRanges(piece.pieceInfo).Count > 0);
	}

	/// <summary>
	/// 選択駒が移動可能のとき
	/// </summary>
	/// <param name="koma"></param>
	/// <returns></returns>
	public static bool IsMovable(string objName)
	{
        try
        {
			var piece = GameObject.Find("Canvas").GetComponentsInChildren<PieceController>().Where(x => x.ObjName == objName).FirstOrDefault();
			if (GetMovableRanges(piece.pieceInfo).Count > 0)
				return true;
			return false;
		}
        catch (Exception e)
        {
			Debug.Log(e);
            throw;
        }
	}

	/// <summary>
	/// 駒の移動可能なリストを取得
	/// </summary>
	/// <param name="pieceInfot"></param>
	/// <returns></returns>
	public static List<Address> GetMovableRanges(PieceInfo pieceInfot)
	{
        try
        {
			var moves = new List<Address>();
			var pieceType = pieceInfot.PieceType;
			var manager = BoardManager.Instance;

			if (pieceType > PieceType.CapturedPiece)
			{
				moves = PieceFactory.Instance.Create(pieceType).GetDropMoves(pieceType);
			}
			else
			{
				moves = PieceFactory.Instance.Create(pieceType).GetOnBoardMoves(manager, pieceInfot, true);
			}
			return moves;
		}
        catch (Exception e)
        {
			Debug.Log(e);
			throw;
        }
	}

	/// <summary>
	/// 選択可能な駒オブジェクトのリストを取得
	/// </summary>
	/// <param name="nowPlayer"></param>
	/// <returns></returns>
	public static List<PieceController> GetSelectablePieceObjects(NowPlayer nowPlayer)
	{
		var pieces = GameObject.Find("/Canvas").GetComponentsInChildren<PieceController>();
		var result = pieces.Where(piece => IsSelfPiece(piece.pieceInfo.PieceType, nowPlayer)).ToList();
		return result;
	}

	/// <summary>
	/// 移動可能な駒のリストを取得
	/// </summary>
	/// <returns></returns>
	public static List<PieceMovable> GetMovablePieceObjects()
	{
		var pieces = GameObject.Find("/Canvas").GetComponentsInChildren<PieceMovable>().ToList();
		return pieces;
	}

	/// <summary>
	/// オブジェクトを点滅させる
	/// </summary>
	/// <param name="gameObj"></param>
	public static void BlinkObject(GameObject gameObj)
	{
		iTween.ColorTo(gameObj, iTween.Hash("r", 0.7f, "g", 0.7f, "b", 0.7f, "time", 0.5f, "loopType", "pingpong"));
	}

	/// <summary>
	/// オブジェクトの点滅を元に戻す
	/// </summary>
	/// <param name="gameObj"></param>
	public static void ResetBlinkObject(GameObject gameObj)
	{
		iTween.ColorTo(gameObj, iTween.Hash("r", 1f, "g", 1f, "b", 1f, "time", 0.1f));
	}

	/// <summary>
	/// オブジェクトを浮かせる
	/// </summary>
	/// <param name="gameObj"></param>
	public static void FloatObject(GameObject gameObj)
	{
		gameObj.transform.position = new Vector3(gameObj.transform.position.x - 0.05f, gameObj.transform.position.y + 0.05f, 2);
	}

	/// <summary>
	/// オブジェクトの浮きを元に戻す
	/// </summary>
	public static void ResetFloatObject(GameObject gameObj)
	{
		gameObj.transform.position = new Vector3(gameObj.transform.position.x + 0.05f, gameObj.transform.position.y - 0.05f, 2);
	}

	/// <summary>
	/// オブジェクトの色を変える
	/// </summary>
	/// <param name="gameObj"></param>
	public static void ChangeColorObject(GameObject gameObj)
	{
		gameObj.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
	}

	/// <summary>
	/// オブジェクトの色を元に戻す
	/// </summary>
	/// <param name="gameObj"></param>
	public static void ResetChangeColorObject(GameObject gameObj)
	{
		gameObj.GetComponent<SpriteRenderer>().color = Color.white;
	}

}
