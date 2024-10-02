using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1マスの詳細クラス
/// </summary>
public class Square{
	public Address Address;
	/// <summary>
	/// 駒の種類
	/// </summary>
	public PieceType PieceType;
	/// <summary>
	/// 駒のオブジェクト名
	/// </summary>
	public string ObjName;
	/// <summary>
	/// 駒が存在している
	/// </summary>
	public bool IsExist = false;
	/// <summary>
	/// 先手
	/// </summary>
	public bool IsBlack = false;
	/// <summary>
	/// 後手
	/// </summary>
	public bool IsWhite = false;

	/// <summary>
	///	1マスを空にする
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void Init(int x, int y) {
		Address = new Address(x, y);
		PieceType = PieceType.Empty;
		ObjName = "";
		IsExist = false;
		IsBlack = false;
		IsWhite = false;
	}

	/// <summary>
	/// 1マスを空にする
	/// </summary>
	/// <param name="_address"></param>
	public void Init(Address _address)
	{
		Address = _address;
		PieceType = PieceType.Empty;
		ObjName = "";
		IsExist = false;
		IsBlack = false;
		IsWhite = false;
	}
}
