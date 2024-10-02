using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 王クラス
/// </summary>
public class King : PieceBase {

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="pieceType"></param>
	public King(PieceType pieceType)
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

		var moveRanges = new List<Address>()
		{
			MoveDirection[Direction.Up] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.UpLeft] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.UpRight] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.Left] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.Right] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.Down] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.DownLeft] * new Address(1, reversenum) + piece.Address,
			MoveDirection[Direction.DownRight] * new Address(1, reversenum) + piece.Address,
		};
		var moves = moveRanges.Where(moveTo => moveTo.IsValid()
									&& !PieceUtility.IsSelfPiece(manager.GetSquare(moveTo), piece)
									&& !PieceUtility.IsCheckMate(piece.Address, moveTo, reverse, isCheck)
									).ToList();

		var saveBoard = manager.CopyBoard();
		var enemyEffectiveMoves = PieceUtility.GetEnemyEffectWithoutKing(manager, !reverse);
		manager.Board = saveBoard;

		var ret = moves.Where(moveTo => moveTo.IsValid()
							&& !enemyEffectiveMoves.Contains(moveTo)
							&& !PieceUtility.IsCheckMate(piece.Address, moveTo, reverse, isCheck)
							).ToList();
		return ret;
	}

	/// <summary>
	/// 移動可能なマス取得
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	public override List<Address> GetDropMoves(PieceType pieceType)
	{
		return null;
	}

}
