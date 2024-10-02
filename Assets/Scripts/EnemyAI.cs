using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI
{
    [System.Flags]
    public enum Difficulty
    {
        Easy = 1,
        Normal = 3,
        Hard = 5,
    }

    private int _deepLevel = (int)Difficulty.Easy;
    private BoardManager _boardManager = BoardManager.Instance;
    private CapturedPieceManager _captureManager = CapturedPieceManager.Instance;

    /// <summary>
    /// 探索の深さ
    /// </summary>
    public int DeepLevel { get => _deepLevel; set => _deepLevel = value; }

    /// <summary>
    /// AIの思考
    /// </summary>
    /// <param name="boardManager"></param>
    /// <returns></returns>
    public PieceMoveInfo ThinkEnemyAIHand(BoardManager boardManager)
    {
        BestHandInfo bestHand = AlphaBetaMax(depth: _deepLevel, alpha: 0, beta: 999999);
        return bestHand.MoveInfo;
    }

    /// <summary>
    /// AIにとって最善の手を返す
    /// </summary>
    /// <param name="boardManager"></param>
    /// <returns></returns>
    private BestHandInfo ThinkBestAIHand(BoardManager boardManager, CapturedPieceManager captureManager)
    {
        var bestHandInfoList = new List<BestHandInfo>();
        bestHandInfoList.Add(new BestHandInfo { Score = -9999 });

        for (int x = 1; x <= BoardManager.BOARD_WIDTH; x++)
        {
            for (int y = 1; y <= BoardManager.BOARD_WIDTH; y++)
            {
                Square square = boardManager.GetSquare(x, y);
                PieceBase enemyPiece = GetSquareEnemyPiece(square);
                if (enemyPiece == null) continue;

                var bestHand = enemyPiece.GetBestHand(boardManager, captureManager, square.Address);
                if (!bestHand.MoveInfo.MoveTo.IsValid()) continue;

                if (bestHand.Score > bestHandInfoList[0].Score)
                {
                    bestHandInfoList.Clear();
                    bestHandInfoList.Add(bestHand);
                }
                else if (bestHand.Score == bestHandInfoList[0].Score)
                {
                    bestHandInfoList.Add(bestHand);
                }
            }
        }

        var best = bestHandInfoList[Random.Range(0, bestHandInfoList.Count)];
        return best;
    }

    /// <summary>
    /// Playerにとって最善の手を返す
    /// </summary>
    /// <param name="boardManager"></param>
    /// <returns></returns>
    private BestHandInfo ThinkBestPlayerHand(BoardManager boardManager, CapturedPieceManager captureManager)
    {
        var bestHandInfoList = new List<BestHandInfo>();
        bestHandInfoList.Add(new BestHandInfo { Score = -9999 });

        for (int x = 1; x <= BoardManager.BOARD_WIDTH; x++)
        {
            for (int y = 1; y <= BoardManager.BOARD_WIDTH; y++)
            {
                Square square = boardManager.GetSquare(x, y);
                PieceBase playerPiece = GetSquarePlayerPiece(square);
                if (playerPiece == null) continue;

                var bestHand = playerPiece.GetBestHand(boardManager, captureManager, square.Address);
                if (!bestHand.MoveInfo.MoveTo.IsValid()) continue;

                if (bestHand.Score > bestHandInfoList[0].Score)
                {
                    bestHandInfoList.Clear();
                    bestHandInfoList.Add(bestHand);
                }
                else if (bestHand.Score == bestHandInfoList[0].Score)
                {
                    bestHandInfoList.Add(bestHand);
                }
            }
        }

        var best = bestHandInfoList[Random.Range(0, bestHandInfoList.Count)];
        best.Score *= -1; // AIにとってはマイナスなので、-1をかける
        return best;
    }

    /// <summary>
    /// 自分(AI)の手を調べる
    /// </summary>
    private BestHandInfo AlphaBetaMax(int depth, int alpha, int beta)
    {
        if (depth == 0)
        {
            return ThinkBestAIHand(_boardManager, _captureManager); // 現在の局面の評価
        }

        // AIの可能な手を全て取得
        var allHandList = GetAllHandList(NowPlayer.White);

        const int INIT_SCORE = -9999;
        var bestHand = new BestHandInfo { Score = INIT_SCORE };
        foreach (var hand in allHandList)
        {
            int score = 0;
            var saveBoard = _boardManager.CopyBoard();
            var saveCapture = _captureManager.CopyCapturedPieces();

            // AIの手を打つ
            _boardManager.SetSquare(hand.MoveFrom, PieceType.Empty);
            var square = _boardManager.GetSquare(hand.MoveTo);
            if (square.IsBlack)
            {
                _captureManager.Plus(square.PieceType);
            }
            _boardManager.SetSquare(hand.MoveTo, hand.PieceType);

            // 次の相手の手
            var best = AlphaBetaMin(depth - 1, alpha, beta);
            score = best.Score;

            // AIの手を戻す
            _boardManager.Board = saveBoard;
            _captureManager.CapturedPieces = saveCapture;

            if (score >= beta)
            {
                // beta値を上回ったら探索中止
                return bestHand;
            }

            if (score >= bestHand.Score)
            {
                // スコアが同じ場合はランダム抽選
                if (score != INIT_SCORE &&
                    score == bestHand.Score &&
                    Random.Range(0, 2) == 0)
                {
                    continue;
                }

                // より良い手が見つかった
                bestHand.Score = score;
                alpha = Mathf.Max(alpha, bestHand.Score);
                bestHand.MoveInfo.SetMoveFrom(_boardManager.GetSquare(hand.MoveFrom));
                bestHand.MoveInfo.SetMoveTo(hand.MoveTo);
            }
        }

        return bestHand;
    }

    /// <summary>
    /// 相手の手を調べる
    /// </summary>
    private BestHandInfo AlphaBetaMin(int depth, int alpha, int beta)
    {
        if (depth == 0)
        {
            return ThinkBestPlayerHand(_boardManager, _captureManager); // 現在の局面の評価
        }

        // プレイヤーの可能な手を全て取得
        List<PieceMoveInfo> allHandList = GetAllHandList(NowPlayer.Black);

        const int INIT_SCORE = 9999;
        var bestHand = new BestHandInfo { Score = INIT_SCORE };
        foreach (var hand in allHandList)
        {
            int score = 0;
            var saveBoard = _boardManager.CopyBoard();
            var saveCapture = _captureManager.CopyCapturedPieces();
            // プレイヤーの手を打つ
            _boardManager.GetSquare(hand.MoveFrom.X, hand.MoveFrom.Y).PieceType = PieceType.Empty;
            var square = _boardManager.GetSquare(hand.MoveTo);
            if (square.IsWhite)
            {
                _captureManager.Plus(square.PieceType);
            }
            _boardManager.GetSquare(hand.MoveTo.X, hand.MoveTo.Y).PieceType = hand.PieceType;

            // 次のAIの手
            score = AlphaBetaMax(depth - 1, alpha, beta).Score;

            // プレイヤーの手を戻す
            _boardManager.Board = saveBoard;
            _captureManager.CapturedPieces = saveCapture;

            if (score <= alpha)
            {
                // alpha値を下回ったら探索中止
                return bestHand;
            }

            if (score <= bestHand.Score)
            {
                // スコアが同じ場合はランダム抽選
                if (score != INIT_SCORE &&
                    score == bestHand.Score &&
                    Random.Range(0, 2) == 0)
                {
                    continue;
                }

                bestHand.Score = score;
                beta = Mathf.Min(beta, bestHand.Score);
                bestHand.MoveInfo.SetMoveFrom(_boardManager.GetSquare(hand.MoveFrom.X, hand.MoveFrom.Y));
                bestHand.MoveInfo.SetMoveTo(hand.MoveTo);
            }
        }

        return bestHand;
    }

    /// <summary>
    /// 可能な手を全て取得
    /// </summary>
    /// <param name="nowPlayer"></param>
    /// <returns></returns>
    private List<PieceMoveInfo> GetAllHandList(NowPlayer nowPlayer)
    {
        var handList = new List<PieceMoveInfo>();

        for (int x = 1; x < BoardManager.BOARD_WIDTH - 1; x++)
        {
            for (int y = 1; y < BoardManager.BOARD_HEIGHT - 1; y++)
            {
                Square square = _boardManager.GetSquare(x, y);
                PieceBase piece = (nowPlayer == NowPlayer.Black) ? GetSquarePlayerPiece(square) : GetSquareEnemyPiece(square);
                if (piece == null) continue;

                var moveRanges = piece.GetOnBoardMoves(_boardManager, PieceUtility.ConvertToPieceInfoFrom(square));
                for (int i = 0; i < moveRanges.Count; i++)
                {
                    handList.Add(new PieceMoveInfo(square.Address, moveRanges[i], square.PieceType));
                }
            }
        }
        return handList;
    }

    private PieceBase GetSquareEnemyPiece(Square square)
    {
        if (!square.IsBlack)
        {
            return null;
        }

        return PieceFactory.Instance.Create(square.PieceType);
    }

    private PieceBase GetSquarePlayerPiece(Square square)
    {
        if (!square.IsWhite)
        {
            return null;
        }

        return PieceFactory.Instance.Create(square.PieceType);
    }
}

public class BestHandInfo
{
    public int Score = 0;
    public PieceMoveInfo MoveInfo = new PieceMoveInfo();
}
