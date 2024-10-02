public class CheckBestMove
{
    public MoveState CheckBestMoveState(string bestmove)
    {
		if (bestmove == null)
		{
			return MoveState.Timeout;
		}
		if (bestmove.Equals("win"))
		{
			return MoveState.Win;
		}
		else if (bestmove.Equals(""))  //サーバーエラーの場合
		{
			return MoveState.ServerError;
		}
		else if (IsDrop(bestmove))    //打ち駒の場合
		{
			return MoveState.Drop;
		}
		else //駒移動の場合
		{
			return MoveState.Move;
		}
	}

	/// <summary>
	/// 駒打ちのとき
	/// </summary>
	/// <param name="bestMove"></param>
	/// <returns></returns>
	bool IsDrop(string bestMove)
	{
		return bestMove.Substring(1, 1).Equals("*");
	}
}

public enum MoveState
{
    ServerError,
	Timeout,
    Win,
    Drop,
    Move,
}
