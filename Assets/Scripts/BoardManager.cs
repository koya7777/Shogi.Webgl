using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// マス管理
/// MasuManager manager = MasuManager.Instance;
/// manager.SetSquare(x, y);
/// </summary>
public class BoardManager : MonoBehaviour
{
	public const int BOARD_WIDTH = 9;
	public const int BOARD_HEIGHT = 9;

	/// <summary>
	/// インスタンス
	/// </summary>
	private static BoardManager _instance;

	Dictionary<string, Square> _board = new Dictionary<string, Square>();

	/// <summary>
	/// シングルトン
	/// </summary>
	public static BoardManager Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject gameObject = new GameObject("BoardManager");
				_instance = gameObject.AddComponent<BoardManager>();
			}
			return _instance;
		}
	}

	/// <summary>
	/// 将棋盤
	/// </summary>
	public Dictionary<string, Square> Board { get => _board; set => _board = value; }

    /// <summary>
    /// 将棋盤のクローン
    /// </summary>
    /// <returns></returns>
    public BoardManager Clone()
	{
		var  other = (BoardManager)MemberwiseClone();
		other._board = other.CopyBoard();
		return other;
	}

	/// <summary>
	/// 9x9マスの辞書型配列 boardのデータをコピーする
	/// </summary>
	/// <returns></returns>
	public Dictionary<string, Square> CopyBoard()
	{
		var board = new Dictionary<string, Square>();
		for (int x = 1; x <= 9; x++)
		{
			for (int y = 1; y <= 9; y++)
			{
				var square = new Square();
				square = CopySquare(_board[x + "-" + y]);
				board[x + "-" + y] = square;
			}
		}
		return board;
	}

	/// <summary>
	/// 駒の削除
	/// </summary>
	/// <param name="pieceType">削除する駒名</param>
	public void RemovePiece(PieceType pieceType)
	{
		var key = _board.Where(x => x.Value.PieceType == pieceType).FirstOrDefault().Key;
		if (key != null)
		{
			var square = new Square();
			square.Init(_board[key].Address.X, _board[key].Address.Y);
			_board[key] = square;

		}
	}

	/// <summary>
	/// 駒の移動
	/// </summary>
	/// <param name="from"></param>
	/// <param name="to"></param>
	public void MovePiece(Address from, Address to)
	{
		var src = GetSquare(from);
		var dest = GetSquare(to);
		dest.IsBlack = src.IsBlack;
		dest.IsWhite = src.IsWhite;
		dest.IsExist = src.IsExist;
		dest.PieceType = src.PieceType;
		dest.ObjName = src.ObjName;
		src.Init(from);
	}

	/// <summary>
	/// 持ち駒を打つ
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="pieceType"></param>
	/// <param name="reverse"></param>
	public void DropPiece(Address address, PieceType pieceType)
	{
		var square = GetSquare(address);
		square.PieceType = (PieceType)(pieceType - PieceType.CapturedPiece);
		square.IsExist = true;
		square.IsBlack = BoardUtility.IsBlackPiece(pieceType);
		square.IsWhite = BoardUtility.IsWhitePiece(pieceType);
	}

	/// <summary>
	/// 将棋盤の初期化
	/// </summary>
	public void Init()
	{
		for (int x = 1; x <= 9; x++)
		{
			for (int y = 1; y <= 9; y++)
			{
				var square = new Square();
				square.Init(x, y);
				_board[x + "-" + y] = square;
			}
		}
	}

	/// <summary>
	/// 引数で与えられたマスの情報をコピーして返す
	/// </summary>
	/// <param name="square">コピーするマス情報</param>
	/// <returns></returns>
	public Square CopySquare(Square square)
	{
		Square _square = new Square();
		_square.IsWhite = square.IsWhite;
		_square.IsBlack = square.IsBlack;
		_square.IsExist = square.IsExist;
		_square.PieceType = square.PieceType;
		_square.ObjName = square.ObjName;
		_square.Address = square.Address;
		return _square;
	}

	/// <summary>
	/// 1マスを空にする
	/// </summary>
	/// <param name="address">マスの座標</param>
	public void EmptySquare(Address address)
	{
		if (!address.IsValid())
		{
			return;
		}
		Square square = new Square();
		square.Init(address.X, address.Y);
		_board[address.X + "-" + address.Y] = square;
	}

	/// <summary>
	/// 平手の駒を盤面に配置する
	/// </summary>
	public void InitHirate()
	{
		Init();
		SetSquare(new Address(5, 9), PieceType.BKing);
		SetSquare(new Address(2, 8), PieceType.BRook);
		SetSquare(new Address(8, 8), PieceType.BBishop);
		SetSquare(new Address(6, 9), PieceType.BGold);
		SetSquare(new Address(4, 9), PieceType.BGold);
		SetSquare(new Address(7, 9), PieceType.BSilver);
		SetSquare(new Address(3, 9), PieceType.BSilver);
		SetSquare(new Address(8, 9), PieceType.BKnight);
		SetSquare(new Address(2, 9), PieceType.BKnight);
		SetSquare(new Address(9, 9), PieceType.BLance);
		SetSquare(new Address(1, 9), PieceType.BLance);
		for (int i = 1; i <= 9; i++)
		{
			SetSquare(new Address(i, 7), PieceType.BPawn);
		}
		SetSquare(new Address(5, 1), PieceType.WKing);
		SetSquare(new Address(8, 2), PieceType.WRook);
		SetSquare(new Address(2, 2), PieceType.WBishop);
		SetSquare(new Address(6, 1), PieceType.WGold);
		SetSquare(new Address(4, 1), PieceType.WGold);
		SetSquare(new Address(7, 1), PieceType.WSilver);
		SetSquare(new Address(3, 1), PieceType.WSilver);
		SetSquare(new Address(8, 1), PieceType.WKnight);
		SetSquare(new Address(2, 1), PieceType.WKnight);
		SetSquare(new Address(9, 1), PieceType.WLance);
		SetSquare(new Address(1, 1), PieceType.WLance);
		for (int i = 1; i <= 9; i++)
		{
			SetSquare(new Address(i, 3), PieceType.WPawn);
		}
	}

	/// <summary>
	/// マスに駒を配置する
	/// </summary>
	/// <param name="x">マスのｘ座標</param>
	/// <param name="y">マスのｙ座標</param>
	/// <param name="name">配置する駒名</param>
	public void SetSquare(Address address, PieceType pieceType)
	{
		if (!address.IsValid())
		{
			return;
		}
		var square = new Square();
		square.Address = address;
		square.PieceType = pieceType;
		square.IsExist = true;
		if (BoardUtility.IsWhitePiece(pieceType))
		{
			square.IsBlack = false;
			square.IsWhite = true;
		}
		else
		{
			square.IsBlack = true;
			square.IsWhite = false;
		}
		_board[address.X + "-" + address.Y] = square;
	}

	/// <summary>
	/// マスにオブジェクト名を設定する
	/// </summary>
	/// <param name="objName">駒のオブジェクト名</param>
	/// <param name="x">マスのｘ座標</param>
	/// <param name="y">マスのｙ座標</param>
	public void SetSquareObjName(string objName, Address address)
	{
		if (!address.IsValid())
		{
			return;
		}
		var square = GetSquare(address);
		square.ObjName = objName;
		_board[address.X + "-" + address.Y] = square;
	}

	/// <summary>
	/// マスの情報を取得する
	/// </summary>
	/// <param name="x">マスのｘ座標</param>
	/// <param name="y">マスのｙ座標</param>
	/// <returns></returns>
	public Square GetSquare(int x, int y)
	{
		var address = new Address(x, y);
		if (!address.IsValid())
		{
			var square = new Square();
			square.Init(0, 0);
			return square;
		}
		return _board[x + "-" + y];
	}

	/// <summary>
	/// マスの情報を取得する
	/// </summary>
	/// <param name="address">マスの座標</param>
	/// <returns></returns>
	public Square GetSquare(Address address)
	{
		if (!address.IsValid())
		{
			var square = new Square();
			square.Init(0, 0);
			return square;
		}
		return _board[address.X + "-" + address.Y];
	}

}