using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角クラス
/// </summary>
public class Bishop : PieceBase {

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="pieceType"></param>
	public Bishop(PieceType pieceType)
	{
		_pieceType = pieceType;
	}

	/// <summary>
	/// 移動可能なマス取得
	/// </summary>
	/// <param name="manager"></param>
	/// <param name="piece"></param>
	/// <param name="isCheck"></param>
	/// <returns></returns>
	public override List<Address> GetOnBoardMoves(BoardManager manager, PieceInfo piece, bool isCheck = false)
	{
		var reverse = BoardUtility.IsWhitePiece(_pieceType);
		var moves = new List<Address>();
		var defineRanges = new List<Address>()
		{
			MoveDirection[Direction.UpLeft],
			MoveDirection[Direction.UpRight],
			MoveDirection[Direction.DownLeft],
			MoveDirection[Direction.DownRight],
		};
		var movableRanges = PieceUtility.CalcForeverMoveRange(manager, piece, defineRanges, reverse, isCheck);
		return movableRanges;
	}

	/// <summary>
	/// 移動可能なマス取得
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	public override List<Address> GetDropMoves (PieceType pieceType) 
	{
		var reverse = BoardUtility.IsWhitePiece(_pieceType);
		var moves = new List<Address> ();
		var manager = BoardManager.Instance;
		moves = PieceUtility.CalcDropablePieceRange(manager, pieceType, reverse);
		return moves;
	}
}
