using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 持ち駒の管理
/// MotigomaManager manager = MotigomaManager.Instance;
/// </summary>
public class CapturedPieceManager : MonoBehaviour
{
	private static CapturedPieceManager _instance;

	private static Dictionary<PieceType, int> _capturedPieces = new Dictionary<PieceType, int>()
	{
		{PieceType.BPawn, 0},
		{PieceType.BLance, 0},
		{PieceType.BKnight, 0},
		{PieceType.BSilver, 0},
		{PieceType.BGold, 0},
		{PieceType.BBishop, 0},
		{PieceType.BRook, 0},
		{PieceType.BKing, 0},
		{PieceType.WPawn, 0},
		{PieceType.WLance, 0},
		{PieceType.WKnight, 0},
		{PieceType.WSilver, 0},
		{PieceType.WGold, 0},
		{PieceType.WBishop, 0},
		{PieceType.WRook, 0},
		{PieceType.WKing, 0},
	};

	/// <summary>
	/// シングルトン
	/// </summary>
	public static CapturedPieceManager Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("CapturedPieceManager");
				_instance = gameObject.AddComponent<CapturedPieceManager>();
			}
			return _instance;
		}
	}

	/// <summary>
	/// 持ち駒
	/// </summary>
    public Dictionary<PieceType, int> CapturedPieces { get => _capturedPieces; set => _capturedPieces = value; }

	/// <summary>
	/// 持ち駒のデータをコピーする
	/// </summary>
	/// <returns></returns>
	public Dictionary<PieceType, int> CopyCapturedPieces()
	{
		var capturedPiece = new Dictionary<PieceType, int>(_capturedPieces);
		return capturedPiece;
	}

	/// <summary>
	/// 持ち駒をクリアする
	/// </summary>
	public void Reset()
    {
		_capturedPieces[PieceType.BPawn] = 0;
		_capturedPieces[PieceType.BLance] = 0;
		_capturedPieces[PieceType.BKnight] = 0;
		_capturedPieces[PieceType.BSilver] = 0;
		_capturedPieces[PieceType.BGold] = 0;
		_capturedPieces[PieceType.BBishop] = 0;
		_capturedPieces[PieceType.BRook] = 0;
		_capturedPieces[PieceType.BKing] = 0;
		_capturedPieces[PieceType.WPawn] = 0;
		_capturedPieces[PieceType.WLance] = 0;
		_capturedPieces[PieceType.WKnight] = 0;
		_capturedPieces[PieceType.WSilver] = 0;
		_capturedPieces[PieceType.WGold] = 0;
		_capturedPieces[PieceType.WBishop] = 0;
		_capturedPieces[PieceType.WRook] = 0;
		_capturedPieces[PieceType.WKing] = 0;
	}

    /// <summary>
    /// 持ち駒をプラスする
    /// </summary>
    /// <param name="pieceType"></param>
    public void Plus(PieceType pieceType)
	{
		if (pieceType == PieceType.Empty) 
			return;
		_capturedPieces[pieceType]++;
	}

	/// <summary>
	/// 持ち駒をマイナスする
	/// </summary>
	/// <param name="pieceType"></param>
	public void Minus(PieceType pieceType)
	{
		if (pieceType == PieceType.Empty)
			return;
		var captureType = pieceType - PieceType.CapturedPiece;
		_capturedPieces[(PieceType)captureType]--;
	}
}
