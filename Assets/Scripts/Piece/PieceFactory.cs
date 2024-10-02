using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 駒インスタンス生成クラス
/// </summary>
public class PieceFactory
{
	/// <summary>
	/// インスタンス
	/// </summary>
	private static PieceFactory _instance;

	/// <summary>
	/// シングルトン
	/// </summary>
	public static PieceFactory Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PieceFactory();
			}
			return _instance;
		}
	}

    /// <summary>
    /// 駒の作成
    /// </summary>
    /// <param name="pieceType"></param>
    /// <returns></returns>
    public PieceBase Create(PieceType pieceType)
    {
        switch (pieceType)
        {
            case PieceType.BKing:
            case PieceType.WKing:
            case PieceType.CapturedPieceBKing:
            case PieceType.CapturedPieceWKing:
                return new King(pieceType);
            case PieceType.BRook:
            case PieceType.WRook:
            case PieceType.CapturedPieceBRook:
            case PieceType.CapturedPieceWRook:
                return new Rook(pieceType);
            case PieceType.BBishop:
            case PieceType.WBishop:
            case PieceType.CapturedPieceBBishop:
            case PieceType.CapturedPieceWBishop:
                return new Bishop(pieceType);
            case PieceType.BGold:
            case PieceType.WGold:
            case PieceType.CapturedPieceBGold:
            case PieceType.CapturedPieceWGold:
                return new Gold(pieceType);
            case PieceType.BSilver:
            case PieceType.WSilver:
            case PieceType.CapturedPieceBSilver:
            case PieceType.CapturedPieceWSilver:
                return new Silver(pieceType);
            case PieceType.BKnight:
            case PieceType.WKnight:
            case PieceType.CapturedPieceBKnight:
            case PieceType.CapturedPieceWKnight:
                return new Knight(pieceType);
            case PieceType.BLance:
            case PieceType.WLance:
            case PieceType.CapturedPieceBLance:
            case PieceType.CapturedPieceWLance:
                return new Lance(pieceType);
            case PieceType.BPawn:
            case PieceType.WPawn:
            case PieceType.CapturedPieceBPawn:
            case PieceType.CapturedPieceWPawn:
                return new Pawn(pieceType);
            case PieceType.BPromRook:
            case PieceType.WPromRook:
                return new PromRook(pieceType);
            case PieceType.BPromBishop:
            case PieceType.WPromBishop:
                return new PromBishop(pieceType);
            case PieceType.BPromSilver:
            case PieceType.WPromSilver:
                return new PromSilver(pieceType);
            case PieceType.BPromKnight:
            case PieceType.WPromKnight:
                return new PromKnight(pieceType);
            case PieceType.BPromLance:
            case PieceType.WPromLance:
                return new PromLance(pieceType);
            case PieceType.BPromPawn:
            case PieceType.WPromPawn:
                return new PromPawn(pieceType);
            default:
                return null;
        }
    }

}
