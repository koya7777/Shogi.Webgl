using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sfen�t�H�[�}�b�g�̎擾�Ɛݒ�
/// </summary>
public class SfenManager
{
	/// <summary>
	/// �C���X�^���X
	/// </summary>
	private static SfenManager _instance;
	/// <summary>
	/// �{�[�h�}�l�[�W���[
	/// </summary>
	private BoardManager _boardManager;
	/// <summary>
	/// ������}�l�[�W���[
	/// </summary>
	private CapturedPieceManager _capturedPieceManager;
	/// <summary>
	/// �^�[���}�l�[�W���[
	/// </summary>
	private TurnManager _turnManager;

	/// <summary>
	/// Sfen����
	/// </summary>
	List<string> _sfenHistory;

	/// <summary>
	/// �V���O���g��
	/// </summary>
	public static SfenManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SfenManager();
			}
			return _instance;
		}
	}

	private SfenManager()
    {
		_boardManager = BoardManager.Instance;
		_capturedPieceManager = CapturedPieceManager.Instance;
		_turnManager = TurnManager.Instance;
		_sfenHistory = new List<string>();
	}

	/// <summary>
	/// SfenHistory�ɒǉ�
	/// </summary>
	public void AddSfenHistory()
    {
		_sfenHistory.Add(GetSfen());
    }

	/// <summary>
	/// SfenHistory�̍Ō�̃f�[�^���폜
	/// </summary>
	public void RemoveSfenHistory()
    {
		if (_sfenHistory.Count == 0) return;
		_sfenHistory.RemoveAt(_sfenHistory.Count - 1);
	}

	/// <summary>
	/// SfenHistory��Ԃ�
	/// </summary>
	/// <param name="step"></param>
	/// <returns></returns>
	public string GetSfenHistory(int step)
    {
		if (_sfenHistory.Count == 0) return null;
		return _sfenHistory[step];
	}

	/// <summary>
	/// �����̏�Ԃ�Sfen�t�H�[�}�b�g�Ŏ擾
	/// </summary>
	/// <returns></returns>
	public string GetSfen()
	{
		var sfenOnBoardPiece = GetSfenOnBoardPiece();
		var sfenCapturedPiece = GetSfenCapturedPiece();
		var turn = _turnManager._nowPlayer == NowPlayer.Black ? " b " : " w ";
		var step = _turnManager.step.ToString();
		return "sfen " + sfenOnBoardPiece + turn + sfenCapturedPiece + " " + step;
	}

	/// <summary>
	/// �Տ�̊����̏�Ԃ�Sfen�t�H�[�}�b�g�Ŏ擾
	/// </summary>
	/// <returns>
	/// ��Flnsgkgsnl/1r5b1/ppppppppp/9/9/9/PPPPPPPPP/1B5R1/LNSGKGSNL
	/// </returns>
	public string GetSfenOnBoardPiece()
	{
		var line = "";
		var emptyCnt = 0;
		for (var y = 1; y <= 9; y++)
		{
			for (var x = 9; x >= 1; x--)
			{
				var square = _boardManager.GetSquare(x, y);
				if (square.IsExist && emptyCnt > 0)
				{
					line += emptyCnt;
					emptyCnt = 0;
				}
				if (!square.IsExist)
				{
					emptyCnt++;
				}
				else
				{
					line += GetSfenOne(square.PieceType);
				}
			}
			if (emptyCnt > 0)
			{
				line += emptyCnt;
				emptyCnt = 0;
			}
			if (y < 9)
			{
				line += "/";
			}
		}
		return line;
	}

	/// <summary>
	/// ����̏�Ԃ�Sfen�t�H�[�}�b�g�Ŏ擾
	/// </summary>
	/// <returns>�i��jPp2</returns>
	public string GetSfenCapturedPiece()
	{
		var capturedPieces = _capturedPieceManager.CapturedPieces;
		string line = "";
		foreach (var piece in capturedPieces)
		{
			if (capturedPieces[piece.Key] > 0)
			{
				line += GetSfenOne(piece.Key);
				if (capturedPieces[piece.Key] > 1)
				{
					line += capturedPieces[piece.Key];
				}
			}
		}
		return line.Equals("") ? "-" : line;
	}

	/// <summary>
	/// sfen�̏�Ԃ������ɐݒ�
	/// </summary>
	/// <param name="sfen"></param>
	public void SetSfen(string sfen)
	{
		var sfens = sfen.Split(" ");
		var sfenOnBoardPiece = sfens[1];
		var turn = sfens[2];
		var sfenCapturedPiece = sfens[3];

		_turnManager._nowPlayer = turn.Equals("b") ? NowPlayer.Black : NowPlayer.White;

		SetSfenOnBoardPiece(sfenOnBoardPiece);
		SetSfenCapturedPiece(sfenCapturedPiece);
	}

	/// <summary>
	/// sfen�t�H�[�}�b�g�̏�Ԃ�Ֆʏ�ɐݒ�
	/// </summary>
	/// <param name="sfen"></param>
	public void SetSfenOnBoardPiece(string sfen)
	{
		_boardManager.Init();
		var rows = sfen.Split("/");
		int y = 1;
		foreach (var row in rows)
		{
			int x = 9;
			for (int i = 0; i < row.Length; i++)
			{
				string sfenOne = row.Substring(i, 1);
				PieceType pieceType = GetPieceTypeBySfenOne(sfenOne);
				if (i < row.Length - 1 && row.Substring(i + 1, 1).Equals("+"))
				{
					sfenOne = row.Substring(i, 2);
					i++;
					pieceType = GetPieceTypeBySfenOne(sfenOne);
					_boardManager.SetSquare(new Address(x, y), pieceType);
				}
				else if (pieceType.Equals(PieceType.Empty))
				{
					x -= int.Parse(sfenOne) - 1;
				}
				else
				{
					_boardManager.SetSquare(new Address(x, y), pieceType);
				}
				x--;
			}
			y++;
		}
	}

	/// <summary>
	/// sfen�t�H�[�}�b�g�̎�����̏�Ԃ�ݒ�
	/// </summary>
	/// <param name="sfen"></param>
	public void SetSfenCapturedPiece(string sfen)
	{
		_capturedPieceManager.Reset();
		for (int i = 0; i < sfen.Length;)
		{
			var pieceType = GetPieceTypeBySfenOne(sfen.Substring(i, 1));
			if (i < sfen.Length - 1)
			{
				var pieceType2 = GetPieceTypeBySfenOne(sfen.Substring(i + 1, 1));
				if (pieceType2.Equals(""))
				{
					var num = int.Parse(sfen.Substring(i + 1, 1));
					for (int j = 0; j < num; j++)
					{
						_capturedPieceManager.Plus(pieceType);
					}
					i++;
				}
				else
				{
					_capturedPieceManager.Plus(pieceType);
					i++;
				}
			}
			else
			{
				_capturedPieceManager.Plus(pieceType);
				i++;
			}
		}
	}

	/// <summary>
	/// ���sfen�t�H�[�}�b�g�ɕϊ�
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	public string GetSfenOne(PieceType pieceType)
	{
        switch (pieceType)
        {
			case PieceType.BPawn:
				return "P";
			case PieceType.BLance:
				return "L";
			case PieceType.BKnight:
				return "N";
			case PieceType.BSilver:
				return "S";
			case PieceType.BGold:
				return "G";
			case PieceType.BKing:
				return "K";
			case PieceType.BRook:
				return "R";
			case PieceType.BBishop:
				return "B";
			case PieceType.BPromPawn:
				return "P+";
			case PieceType.BPromLance:
				return "L+";
			case PieceType.BPromKnight:
				return "N+";
			case PieceType.BPromSilver:
				return "S+";
			case PieceType.BPromRook:
				return "R+";
			case PieceType.BPromBishop:
				return "B+";
			case PieceType.WPawn:
				return "p";
			case PieceType.WLance:
				return "l";
			case PieceType.WKnight:
				return "n";
			case PieceType.WSilver:
				return "s";
			case PieceType.WGold:
				return "g";
			case PieceType.WKing:
				return "k";
			case PieceType.WRook:
				return "r";
			case PieceType.WBishop:
				return "b";
			case PieceType.WPromPawn:
				return "p+";
			case PieceType.WPromLance:
				return "l+";
			case PieceType.WPromKnight:
				return "n+";
			case PieceType.WPromSilver:
				return "s+";
			case PieceType.WPromRook:
				return "r+";
			case PieceType.WPromBishop:
				return "b+";
			default:
                return null;
        }
    }

	/// <summary>
	/// sfen�t�H�[�}�b�g�̋�������擾
	/// </summary>
	/// <param name="sfenOne"></param>
	/// <returns></returns>
	public PieceType GetPieceTypeBySfenOne(string sfenOne)
	{
		switch (sfenOne)
		{
			case "P":
				return PieceType.BPawn;
			case "L":
				return PieceType.BLance;
			case "N":
				return PieceType.BKnight;
			case "S":
				return PieceType.BSilver;
			case "G":
				return PieceType.BGold;
			case "K":
				return PieceType.BKing;
			case "R":
				return PieceType.BRook;
			case "B":
				return PieceType.BBishop;
			case "P+":
				return PieceType.BPromPawn;
			case "L+":
				return PieceType.BPromLance;
			case "N+":
				return PieceType.BPromKnight;
			case "S+":
				return PieceType.BPromSilver;
			case "R+":
				return PieceType.BPromRook;
			case "B+":
				return PieceType.BPromBishop;
			case "p":
				return PieceType.WPawn;
			case "l":
				return PieceType.WLance;
			case "n":
				return PieceType.WKnight;
			case "s":
				return PieceType.WSilver;
			case "g":
				return PieceType.WGold;
			case "k":
				return PieceType.WKing;
			case "r":
				return PieceType.WRook;
			case "b":
				return PieceType.WBishop;
			case "p+":
				return PieceType.WPromPawn;
			case "l+":
				return PieceType.WPromLance;
			case "n+":
				return PieceType.WPromKnight;
			case "s+":
				return PieceType.WPromSilver;
			case "r+":
				return PieceType.WPromRook;
			case "b+":
				return PieceType.WPromBishop;
			default:
				return PieceType.Empty;
		}
	}
}
