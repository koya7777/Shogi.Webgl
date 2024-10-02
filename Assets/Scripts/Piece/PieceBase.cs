using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 駒基底クラス
/// </summary>
public abstract class PieceBase
{
    protected PieceType _pieceType;
    abstract public List<Address> GetOnBoardMoves(BoardManager manager, PieceInfo pieceInfo, bool isCheck = false);
    abstract public List<Address> GetDropMoves(PieceType pieceType);

    protected static Dictionary<Direction, Address> MoveDirection = new Dictionary<Direction, Address>()
    {
        { Direction.Up, new Address(0,-1) },
        { Direction.Down, new Address(0,1) },
        { Direction.Left, new Address(1,0) },
        { Direction.Right, new Address(-1,0) },
        { Direction.UpLeft, new Address(1,-1) },
        { Direction.UpRight, new Address(-1,-1) },
        { Direction.DownLeft, new Address(1,1) },
        { Direction.DownRight, new Address(-1,1) },
        { Direction.KnightLeft, new Address(1,-2) },
        { Direction.KnightRight, new Address(-1,-2) },
    };

    public BestHandInfo GetBestHand(BoardManager boardManager, CapturedPieceManager captureManager, Address moveFrom)
    {
        BestHandInfo bestHandInfo = new BestHandInfo();
        bestHandInfo.MoveInfo.SetMoveFrom(boardManager.GetSquare(moveFrom.X, moveFrom.Y));

        var piece = PieceUtility.ConvertToPieceInfoFrom(boardManager.GetSquare(moveFrom.X, moveFrom.Y));
        var movableRangesOnBoard = GetOnBoardMoves(boardManager, piece, true);

        int score = 0;
        foreach (var moveTo in movableRangesOnBoard)
        {
            if (!moveTo.IsValid())
            {
                Debug.LogError("想定外の場所でエラーが起きました。Address -> " + moveTo);
                continue;
            }
            // TODO:駒を動かす
            if (piece.IsBlack)
            {
                score = boardManager.Board.Where(s => s.Value.IsBlack).Select(s => s.Value.PieceType).Sum(pieceType => PieceConst.GetPieceValue(pieceType));
                score += captureManager.CapturedPieces.Where(s => s.Key < PieceType.WhitePiece && s.Value > 0).Sum(s => PieceConst.GetPieceValue(s.Key) * s.Value);
            }
            else
            {
                score = boardManager.Board.Where(s => s.Value.IsWhite).Select(s => s.Value.PieceType).Sum(pieceType => PieceConst.GetPieceValue(pieceType));
                score += captureManager.CapturedPieces.Where(s => s.Key > PieceType.WhitePiece && s.Value > 0).Sum(s => PieceConst.GetPieceValue(s.Key) * s.Value);
            }
            // TODO: 空のマスにも移動できるように >= の = も付けているが、それでいいのか再検討
            if (score >= bestHandInfo.Score)
            {
                bestHandInfo.Score = score;
                bestHandInfo.MoveInfo.SetMoveTo(moveTo);
            }
        }

        return bestHandInfo;
    }

}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
    KnightLeft,
    KnightRight,
}