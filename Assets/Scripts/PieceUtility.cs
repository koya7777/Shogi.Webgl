using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PieceUtility
{
    /// <summary>
    /// ��̘A���ړ��͈͌v�Z�i��ԁA�p�s�A���ԁj
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="piece"></param>
    /// <param name="defineRanges"></param>
    /// <param name="reverse"></param>
    /// <param name="isCheck"></param>
    /// <returns></returns>
	public static List<Address> CalcForeverMoveRange(BoardManager manager, PieceInfo piece, List<Address> defineRanges, bool reverse = false, bool isCheck = false)
	{
        int reversenum = GetReverseNum(reverse);

        var ranges = new List<Address>();
		var tmpAddress = piece.Address;
		int defineRangeIndex = 0;
		while (true)
		{
			tmpAddress += defineRanges[defineRangeIndex] * new Address(1, reversenum);
			bool canAddAddress = tmpAddress.IsValid() && !IsSelfPiece(manager.GetSquare(tmpAddress), piece);
			if (canAddAddress)
			{
                if (!IsCheckMate(piece.Address, tmpAddress, reverse, isCheck))
                {
                    ranges.Add(tmpAddress);
                }
			}

			if (!canAddAddress || IsEnemyPiece(manager.GetSquare(tmpAddress), piece))
			{
				defineRangeIndex++;
				if (defineRangeIndex == defineRanges.Count)
				{
					break;
				}
                tmpAddress = piece.Address;
                continue;
			}
		}
		return ranges;
	}

    /// <summary>
    /// ������̑łĂ�͈͌v�Z
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="pieceType"></param>
    /// <param name="reverse"></param>
    /// <param name="isDoublePawnCheck">2���`�F�b�N�����邩�ǂ���</param>
    /// <returns></returns>
    public static List<Address> CalcDropablePieceRange(BoardManager manager, PieceType pieceType, bool reverse, bool isDoublePawnCheck = false)
    {
        (int ymin, int ymax) = GetDropPieceYRange(pieceType);
        var boad = manager.Board.Where(s => IsWithinRange(s.Value.Address.Y, ymin, ymax) 
                                        && !PieceUtility.IsExistPiece(s.Value) 
                                        && !PieceUtility.IsDoublePawn(s.Value.Address.X, reverse, isDoublePawnCheck)
                                        ).ToList();
        var moves = boad.Where(s => !IsCheckMate(pieceType, s.Value.Address, reverse, true)).Select(s => s.Value.Address).ToList();
        return moves;
    }

    /// <summary>
    /// ��ł��̏c�����͈̔͂��擾
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    static (int ymin, int ymax) GetDropPieceYRange(PieceType pieceType)
    {
        int ymin = 1;
        int ymax = 9;
        switch (pieceType)
        {
            case PieceType.CapturedPieceBPawn:
            case PieceType.CapturedPieceBLance:
                ymin = 2;
                break;
            case PieceType.CapturedPieceBKnight:
                ymin = 3;
                break;
            case PieceType.CapturedPieceWPawn:
            case PieceType.CapturedPieceWLance:
                ymax = 8;
                break;
            case PieceType.CapturedPieceWKnight:
                ymax = 7;
                break;
            default:
                break;
        }
        return (ymin, ymax);
    }

    /// <summary>
    /// �l���͈͓��̂Ƃ�
    /// </summary>
    /// <param name="y">�l</param>
    /// <param name="ymin">�ŏ��l</param>
    /// <param name="ymax">�ő�l</param>
    /// <returns></returns>
    static bool IsWithinRange(int y, int ymin, int ymax)
    {
        return y >= ymin && y <= ymax;
    }

    /// <summary>
    /// �����̋���݂��Ă���Ƃ�
    /// </summary>
    /// <param name="square"></param>
    /// <param name="piece"></param>
    /// <returns></returns>
    public static bool IsSelfPiece(Square square, PieceInfo piece)
	{
		return square.IsBlack == piece.IsBlack && square.IsWhite == piece.IsWhite;
	}

	/// <summary>
	/// �G�̋���݂��Ă���Ƃ�
	/// </summary>
	/// <param name="square"></param>
	/// <param name="piece"></param>
	/// <returns></returns>
	public static bool IsEnemyPiece(Square square, PieceInfo piece)
	{
		return square.IsWhite == piece.IsBlack && square.IsBlack == piece.IsWhite;
	}

    /// <summary>
    /// ����݂��Ă���Ƃ�
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    public static bool IsExistPiece(Square square)
    {
        return square.IsExist;
    }

    /// <summary>
    /// reverse��false�Ȃ�-1�Atrue�Ȃ�1��Ԃ�
    /// </summary>
    /// <param name="reverse"></param>
    /// <returns></returns>
    public static int GetReverseNum(bool reverse)
    {
        return reverse ? -1 : 1;
    }

    /// <summary>
    /// 2���̂Ƃ�true��Ԃ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="reverse"></param>
    /// <param name="isDoublePawnCheck">2���`�F�b�N�����邩�ǂ���</param>
    /// <returns></returns>
	public static bool IsDoublePawn(int x, bool reverse, bool isDoublePawnCheck)
    {
        if (!isDoublePawnCheck)
        {
            return false;
        }
        int ymin = 1;
        int ymax = 9;
        if (reverse)
        {
            ymax = 8;
        }
        else
        {
            ymin = 2;
        }

        BoardManager manager = BoardManager.Instance;

        for (int y = ymin; y <= ymax; y++)
        {
            Square square = manager.GetSquare(x, y);
            if (!reverse && square.PieceType.Equals(PieceType.BPawn))
            {
                return true;
            }
            else if (reverse && square.PieceType.Equals(PieceType.WPawn))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// ����̂Ƃ�true��Ԃ�
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="move"></param>
    /// <param name="reverse"></param>
    /// <param name="checkFlag"></param>
    /// <returns></returns>
    public static bool IsCheckMate(Address from, Address to, bool reverse = false, bool checkFlag = false)
    {
        if (!checkFlag)
        {
            return false;
        }
        var manager = BoardManager.Instance;
        var saveBoard = manager.CopyBoard();
        manager.MovePiece(from, to);
        var kingPosition = BoardUtility.GetKingPosition(manager, reverse);
        var enemyEffectiveMoves = GetEnemyEffectWithoutKing(manager, !reverse);
        manager.Board = saveBoard;
        
        return enemyEffectiveMoves.Any(address => address == kingPosition);
    }

    /// <summary>
    /// ����̂Ƃ�true��Ԃ�
    /// </summary>
    /// <param name="name"></param>
    /// <param name="move"></param>
    /// <param name="reverse"></param>
    /// <param name="checkFlag"></param>
    /// <returns></returns>
    public static bool IsCheckMate(PieceType pieceType, Address move, bool reverse = false, bool checkFlag = false)
    {
        if (!checkFlag)
        {
            return false;
        }
        var manager = BoardManager.Instance;
        var saveBoard = manager.CopyBoard();
        manager.DropPiece(move, pieceType);
        var kingPosition = BoardUtility.GetKingPosition(manager, reverse);
        var enemyEffectiveMoves = GetEnemyEffectWithoutKing(manager, !reverse);
        manager.Board = saveBoard;
        var ret = enemyEffectiveMoves.Any(address => address == kingPosition);
        return ret;
    }

    /// <summary>
	/// Square�^��PieceInfot�^�ɕϊ�
	/// </summary>
	/// <param name="square"></param>
	/// <returns></returns>
	public static PieceInfo ConvertToPieceInfoFrom(Square square)
    {
        return new PieceInfo(square.Address, square.PieceType);
    }

    /// <summary>
	/// ���w�̉����������Ƃ��̑S�Ă̓G��̌����Ă���}�X���擾(��΍��W)
	/// </summary>
	/// <param name="reverse"></param>
	/// <returns></returns>
	public static List<Address> GetEnemyEffectWithoutKing(BoardManager manager, bool reverse)
    {
        var moves = new List<Address>();
        // �G���̋�̌�����Ԃ�
        if (!reverse)
        {
            manager.RemovePiece(PieceType.WKing);
            var squares = manager.Board.Where(s => s.Value.IsBlack).Select(s => s.Value).ToList();
            foreach (var square in squares)
            {
                var pieceInfo = ConvertToPieceInfoFrom(square);
                var move = PieceFactory.Instance.Create(square.PieceType).GetOnBoardMoves(manager, pieceInfo);
                moves.AddRange(move);
            }
            var ret = moves.Distinct().ToList();
            return ret;
        }
        // ����̌�����Ԃ�
        else
        {
            manager.RemovePiece(PieceType.BKing);
            var squares = manager.Board.Where(s => s.Value.IsWhite).Select(s => s.Value).ToList();
            foreach (var square in squares)
            {
                var pieceInfo = ConvertToPieceInfoFrom(square);
                var move = PieceFactory.Instance.Create(square.PieceType).GetOnBoardMoves(manager, pieceInfo);
                moves.AddRange(move);
            }
            var ret = moves.Distinct().ToList();
            return ret;
        }
    }
}

public class AddressComparer : IEqualityComparer<Address>
{
    public bool Equals(Address a, Address b)
    {
        if (Object.ReferenceEquals(a, b))
            return true;
        if (Object.ReferenceEquals(a, null) || Object.ReferenceEquals(b, null))
            return false;
        return (a.X == b.X && a.Y == b.Y);
    }
    public int GetHashCode(Address address)
    {
        if (Object.ReferenceEquals(address, null)) 
            return 0;
        int hashAddressX = address.X.GetHashCode();
        int hashAddressY = address.Y.GetHashCode();
        return hashAddressX ^ hashAddressY;
    }
}
