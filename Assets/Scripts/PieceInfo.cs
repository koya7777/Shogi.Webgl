using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��̏��Ǘ��N���X
/// </summary>
public class PieceInfo
{
	/// <summary>
	/// �A�h���X
	/// </summary>
	public Address Address;
	/// <summary>
	/// ��̎��
	/// </summary>
	public PieceType PieceType;
	/// <summary>
	/// ���
	/// </summary>
	public bool IsBlack = false;
	/// <summary>
	/// ���
	/// </summary>
	public bool IsWhite = false;

	/// <summary>
	/// �R���X�g���N�^
	/// </summary>
	/// <param name="address"></param>
	/// <param name="pieceType"></param>
	public PieceInfo(Address address, PieceType pieceType)
    {
		SetPiece(address, pieceType);
	}

	/// <summary>
	/// ��ɏ���ݒ�
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
