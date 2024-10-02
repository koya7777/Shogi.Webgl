using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 駒の情報管理クラス
/// </summary>
public class PieceInfo
{
	/// <summary>
	/// アドレス
	/// </summary>
	public Address Address;
	/// <summary>
	/// 駒の種類
	/// </summary>
	public PieceType PieceType;
	/// <summary>
	/// 先手
	/// </summary>
	public bool IsBlack = false;
	/// <summary>
	/// 後手
	/// </summary>
	public bool IsWhite = false;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	/// <param name="address"></param>
	/// <param name="pieceType"></param>
	public PieceInfo(Address address, PieceType pieceType)
    {
		SetPiece(address, pieceType);
	}

	/// <summary>
	/// 駒に情報を設定
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="objName"></param>
	public void SetPiece(Address address, PieceType pieceType)
	{
		Address = address;
		PieceType = pieceType;
		if (BoardUtility.IsWhitePiece(PieceType))
		{
			IsBlack = false;
			IsWhite = true;
		}
		else
		{
			IsBlack = true;
			IsWhite = false;
		}
	}
}
