using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 馬クラス
/// </summary>
public class PromBishop : Bishop {

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="pieceType"></param>
	public PromBishop(PieceType pieceType) : base(pieceType)
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
		var reversenum = PieceUtility.GetReverseNum(reverse);
		var moves = base.GetOnBoardMoves(manager, piece, isCheck);
		var moveRanges = new List<Address>()
		{
			MoveDirection[Direction.Up] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.Left] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.Right] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.Down] * new Address(1, reversenum) + piece.Address,
		};
		moves.AddRange(moveRanges.Where(moveTo => moveTo.IsValid() 
						&& !PieceUtility.IsSelfPiece(manager.GetSquare(moveTo), piece)
						&& !PieceUtility.IsCheckMate(piece.Address, moveTo, reverse, isCheck)
						).ToList());
		return moves;
	}
}
