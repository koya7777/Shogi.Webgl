using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 駒オブジェクト管理クラス
/// </summary>
public class PieceController : MonoBehaviour {

	/// <summary>
	/// 駒の情報
	/// </summary>
	public PieceInfo pieceInfo;
	/// <summary>
	/// 駒のオブジェクト名
	/// </summary>
	public string ObjName;
	/// <summary>
	/// 選択中
	/// </summary>
	public bool IsChoose = false;
	
	/// <summary>
	/// 駒に情報を設定
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="objName"></param>
	public void SetPiece(Address address, string objName) {
		string[] names = objName.Split (new char[]{ '_' });
		string pieceName = names[0];		
		var pieceType = BoardUtility.GetPieceType(pieceName);

		pieceInfo = new PieceInfo(address, pieceType);

		ObjName = objName;
		if (BoardUtility.IsWhitePiece(pieceType)) {
			pieceInfo.IsBlack = false;
			pieceInfo.IsWhite = true;
		} else {
			pieceInfo.IsBlack = true;
			pieceInfo.IsWhite = false;
		}
	}
}
