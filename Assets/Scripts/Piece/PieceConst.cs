using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceConst : MonoBehaviour
{
	/// <summary>持ち駒数</summary>
	public const string num = "Num";
	/// <summary>持ち駒数</summary>
	public const string num2 = "Num2";
	/// <summary>成駒BG</summary>
	public const string promBg = "PromBg";

	/// <summary>
	/// 駒の種類と駒名を紐づけた辞書
	/// </summary>
	public static Dictionary<PieceType, string> PieceNameByPieceTypeDic = new Dictionary<PieceType, string>()
	{
		{ PieceType.BKing, "Ou" },
		{ PieceType.BRook, "Hi"},
		{ PieceType.BBishop, "Ka"},
		{ PieceType.BGold, "Ki"},
		{ PieceType.BSilver, "Gi"},
		{ PieceType.BKnight, "Ke"},
		{ PieceType.BLance, "Ky"},
		{ PieceType.BPawn, "Fu"},
		{ PieceType.BPromRook, "Ry"},
		{ PieceType.BPromBishop, "Um"},
		{ PieceType.BPromSilver, "Ng"},
		{ PieceType.BPromKnight, "Nk"},
		{ PieceType.BPromLance, "Ny"},
		{ PieceType.BPromPawn, "To"},
		{ PieceType.WKing, "Oum"},
		{ PieceType.WRook, "Him"},
		{ PieceType.WBishop, "Kam"},
		{ PieceType.WGold, "Kim"},
		{ PieceType.WSilver, "Gim"},
		{ PieceType.WKnight, "Kem"},
		{ PieceType.WLance, "Kym"},
		{ PieceType.WPawn, "Fum"},
		{ PieceType.WPromRook, "Rym"},
		{ PieceType.WPromBishop, "Umm"},
		{ PieceType.WPromSilver, "Ngm"},
		{ PieceType.WPromKnight, "Nkm"},
		{ PieceType.WPromLance, "Nym"},
		{ PieceType.WPromPawn, "Tom"},
	};

	/// <summary>
	/// Sfenフォーマットと駒の種類を紐づけた辞書
	/// </summary>
	public static Dictionary<string, PieceType> PieceTypeBySfenOneDic = new Dictionary<string, PieceType>()
	{
		{ "P", PieceType.BPawn},
		{ "L", PieceType.BLance},
		{ "N", PieceType.BKnight},
		{ "S", PieceType.BSilver},
		{ "G", PieceType.BGold},
		{ "K", PieceType.BKing},
		{ "R", PieceType.BRook},
		{ "B", PieceType.BBishop},
		{ "P+", PieceType.BPromPawn},
		{ "L+", PieceType.BPromLance},
		{ "N+", PieceType.BPromKnight},
		{ "S+", PieceType.BPromSilver},
		{ "R+", PieceType.BPromRook},
		{ "B+", PieceType.BPromBishop},
		{ "p", PieceType.WPawn},
		{ "l", PieceType.WLance},
		{ "n", PieceType.WKnight},
		{ "s", PieceType.WSilver},
		{ "g", PieceType.WGold},
		{ "k", PieceType.WKing},
		{ "r", PieceType.WRook},
		{ "b", PieceType.WBishop},
		{ "p+", PieceType.WPromPawn},
		{ "l+", PieceType.WPromLance},
		{ "n+", PieceType.WPromKnight},
		{ "s+", PieceType.WPromSilver},
		{ "r+", PieceType.WPromRook},
		{ "b+", PieceType.WPromBishop},
		{ "1", PieceType.Empty },
		{ "2", PieceType.Empty },
		{ "3", PieceType.Empty },
		{ "4", PieceType.Empty },
		{ "5", PieceType.Empty },
		{ "6", PieceType.Empty },
		{ "7", PieceType.Empty },
		{ "8", PieceType.Empty },
		{ "9", PieceType.Empty },
	};

	/// <summary>
	/// 通常駒の種類と成駒の種類の紐づける辞書
	/// </summary>
	public static Dictionary<PieceType, PieceType> PromPieceTypeByPieceTypeDic = new Dictionary<PieceType, PieceType>()
	{
		{ PieceType.BPawn, PieceType.BPromPawn},
		{ PieceType.BLance, PieceType.BPromLance},
		{ PieceType.BKnight, PieceType.BPromKnight},
		{ PieceType.BSilver, PieceType.BPromSilver},
		{ PieceType.BRook, PieceType.BPromRook},
		{ PieceType.BBishop, PieceType.BPromBishop},
		{ PieceType.WPawn, PieceType.WPromPawn},
		{ PieceType.WLance, PieceType.WPromLance},
		{ PieceType.WKnight, PieceType.WPromKnight},
		{ PieceType.WSilver, PieceType.WPromSilver},
		{ PieceType.WRook, PieceType.WPromRook},
		{ PieceType.WBishop, PieceType.WPromBishop},
	};

	/// <summary>
	/// 評価関数用の駒の価値
	/// </summary>
	public class PieceValue
	{
		/// <summary>なし</summary>
		public static int Empty = 0;
		/// <summary>王将</summary>
		public static int King = 10000;
		/// <summary>飛車</summary>
		public static int Rook = 2000;
		/// <summary>角行</summary>
		public static int Bishop = 1800;
		/// <summary>金将</summary>
		public static int Gold = 1200;
		/// <summary>銀将</summary>
		public static int Silver = 1000;
		/// <summary>桂馬</summary>
		public static int Knight = 700;
		/// <summary>香車</summary>
		public static int Lance = 600;
		/// <summary>歩兵</summary>
		public static int Pawn = 100;
		/// <summary>竜王(成り飛車)</summary>
		public static int PromotedRook = 2200;
		/// <summary>竜馬(成り角行)</summary>
		public static int PromotedBishop = 2000;
		/// <summary>成銀</summary>
		public static int PromotedSilver = 1190;
		/// <summary>成桂</summary>
		public static int PromotedKnight = 1180;
		/// <summary>成香</summary>
		public static int PromotedLance = 1170;
		/// <summary>と金</summary>
		public static int PromotedPawn = 1160;
	}

	/// <summary>
	/// 駒の価値の取得
	/// </summary>
	/// <param name="pieceType"></param>
	/// <returns></returns>
	public static int GetPieceValue(PieceType pieceType)
	{
		int pieceValue = 0;
		switch (pieceType)
		{
			case PieceType.BKing:
			case PieceType.WKing:
			case PieceType.CapturedPieceBKing:
			case PieceType.CapturedPieceWKing:
				pieceValue += PieceValue.King;
				break;

			case PieceType.BRook:
			case PieceType.WRook:
			case PieceType.CapturedPieceBRook:
			case PieceType.CapturedPieceWRook:
				pieceValue += PieceValue.Rook;
				break;

			case PieceType.BBishop:
			case PieceType.WBishop:
			case PieceType.CapturedPieceBBishop:
			case PieceType.CapturedPieceWBishop:
				pieceValue += PieceValue.Bishop;
				break;

			case PieceType.BGold:
			case PieceType.WGold:
			case PieceType.CapturedPieceBGold:
			case PieceType.CapturedPieceWGold:
				pieceValue += PieceValue.Gold;
				break;

			case PieceType.BSilver:
			case PieceType.WSilver:
			case PieceType.CapturedPieceBSilver:
			case PieceType.CapturedPieceWSilver:
				pieceValue += PieceValue.Silver;
				break;

			case PieceType.BKnight:
			case PieceType.WKnight:
			case PieceType.CapturedPieceBKnight:
			case PieceType.CapturedPieceWKnight:
				pieceValue += PieceValue.Knight;
				break;

			case PieceType.BLance:
			case PieceType.WLance:
			case PieceType.CapturedPieceBLance:
			case PieceType.CapturedPieceWLance:
				pieceValue += PieceValue.Lance;
				break;

			case PieceType.BPawn:
			case PieceType.WPawn:
			case PieceType.CapturedPieceBPawn:
			case PieceType.CapturedPieceWPawn:
				pieceValue += PieceValue.Pawn;
				break;

			case PieceType.BPromRook:
			case PieceType.WPromRook:
				pieceValue += PieceValue.PromotedRook;
				break;

			case PieceType.BPromBishop:
			case PieceType.WPromBishop:
				pieceValue += PieceValue.PromotedBishop;
				break;

			case PieceType.BPromSilver:
			case PieceType.WPromSilver:
				pieceValue += PieceValue.PromotedSilver;
				break;

			case PieceType.BPromKnight:
			case PieceType.WPromKnight:
				pieceValue += PieceValue.PromotedKnight;
				break;

			case PieceType.BPromLance:
			case PieceType.WPromLance:
				pieceValue += PieceValue.PromotedLance;
				break;

			case PieceType.BPromPawn:
			case PieceType.WPromPawn:
				pieceValue += PieceValue.PromotedPawn;
				break;
		}
		return pieceValue;
	}

}

/// <summary>
/// 駒のタイプ
/// </summary>
public enum PieceType
{
	/// <summary>盤面の外</summary>
	OutOfBoard = -2,
	/// <summary>なし</summary>
	Empty = -1,

	/// <summary>先手</summary>
	BlackPiece = 0,

	/// <summary>先手 王</summary>
	BKing,
	/// <summary>先手 飛車</summary>
	BRook,
	/// <summary>先手 角</summary>
	BBishop,
	/// <summary>先手 金</summary>
	BGold,
	/// <summary>先手 銀</summary>
	BSilver,
	/// <summary>先手 桂馬</summary>
	BKnight,
	/// <summary>先手 香車</summary>
	BLance,
	/// <summary>先手 歩</summary>
	BPawn,
	
	Promoted = 16,

	/// <summary>先手 竜</summary>
	BPromRook = Promoted + BRook,
	/// <summary>先手 馬</summary>
	BPromBishop = Promoted + BBishop,
	/// <summary>先手 成銀</summary>
	BPromSilver = Promoted + BSilver,
	/// <summary>先手 成桂</summary>
	BPromKnight = Promoted + BKnight,
	/// <summary>先手 成香</summary>
	BPromLance = Promoted + BLance,
	/// <summary>先手 ト金</summary>
	BPromPawn = Promoted + BPawn,

	/// <summary>後手</summary>
	WhitePiece = 32,

	/// <summary>後手 王</summary>
	WKing = WhitePiece + BKing,
	/// <summary>後手 飛車</summary>
	WRook = WhitePiece + BRook,
	/// <summary>後手 角</summary>
	WBishop = WhitePiece + BBishop,
	/// <summary>後手 金</summary>
	WGold = WhitePiece + BGold,
	/// <summary>後手 銀</summary>
	WSilver = WhitePiece + BSilver,
	/// <summary>後手 桂馬</summary>
	WKnight = WhitePiece + BKnight,
	/// <summary>後手 香車</summary>
	WLance = WhitePiece + BLance,
	/// <summary>後手 歩</summary>
	WPawn = WhitePiece + BPawn,
	/// <summary>後手 竜</summary>
	WPromRook = WhitePiece + BPromRook,
	/// <summary>後手 馬</summary>
	WPromBishop = WhitePiece + BPromBishop,
	/// <summary>後手 成銀</summary>
	WPromSilver = WhitePiece + BPromSilver,
	/// <summary>後手 成桂</summary>
	WPromKnight = WhitePiece + BPromKnight,
	/// <summary>後手 成香</summary>
	WPromLance = WhitePiece + BPromLance,
	/// <summary>後手 ト金</summary>
	WPromPawn = WhitePiece + BPromPawn,

	/// <summary>持ち駒</summary>
	CapturedPiece = 64,

	/// <summary>先手 玉</summary>
	CapturedPieceBKing = CapturedPiece + BKing,
	/// <summary>先手 飛車</summary>
	CapturedPieceBRook = CapturedPiece + BRook,
	/// <summary>先手 角</summary>
	CapturedPieceBBishop = CapturedPiece + BBishop,
	/// <summary>先手 金</summary>
	CapturedPieceBGold = CapturedPiece + BGold,
	/// <summary>先手 銀</summary>
	CapturedPieceBSilver = CapturedPiece + BSilver,
	/// <summary>先手 桂馬</summary>
	CapturedPieceBKnight = CapturedPiece + BKnight,
	/// <summary>先手 香車</summary>
	CapturedPieceBLance = CapturedPiece + BLance,
	/// <summary>先手 歩</summary>
	CapturedPieceBPawn = CapturedPiece + BPawn,

	/// <summary>後手 王</summary>
	CapturedPieceWKing = CapturedPiece + WKing,
	/// <summary>後手 飛車</summary>
	CapturedPieceWRook = CapturedPiece + WRook,
	/// <summary>後手 角</summary>
	CapturedPieceWBishop = CapturedPiece + WBishop,
	/// <summary>後手 金</summary>
	CapturedPieceWGold = CapturedPiece + WGold,
	/// <summary>後手 銀</summary>
	CapturedPieceWSilver = CapturedPiece + WSilver,
	/// <summary>後手 桂馬</summary>
	CapturedPieceWKnight = CapturedPiece + WKnight,
	/// <summary>後手 香車</summary>
	CapturedPieceWLance = CapturedPiece + WLance,
	/// <summary>後手 歩</summary>
	CapturedPieceWPawn = CapturedPiece + WPawn,
	/// <summary>終わり</summary>
	End,
}
