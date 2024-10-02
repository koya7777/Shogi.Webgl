using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 対局管理
/// </summary>
public class TurnManager
{
    private static TurnManager _instance;

    /// <summary>
    /// 先手がfalse
    /// 後手がtrue
    /// </summary>
    public NowPlayer _nowPlayer = NowPlayer.Black;
    /// <summary>
    /// 手数
    /// </summary>
    public int step = 0;
    /// <summary>
    /// ゲームオーバーになったとき
    /// </summary>
    public bool IsGameOver = false;
    /// <summary>
    /// AIを停止したとき
    /// </summary>
    public bool IsStopAI = false;
    /// <summary>
    /// AIマネージャー
    /// </summary>
    AIController _AIManager;
    
    SoundManager soundManager = SoundManager.Instance;
    BoardManager boardManager = BoardManager.Instance;

    /// <summary>
    /// 対戦モード
    /// </summary>
    public MatchMode mode = MatchMode.HumanVsHuman;

    /// <summary>
    /// ゲームオーバーオブジェクト
    /// </summary>
    GameObject _gameOverObj;
    /// <summary>
    /// 待ったボタンオブジェクト
    /// </summary>
    GameObject _undoButtonObj;
    /// <summary>
    /// 考え中オブジェクト
    /// </summary>
    GameObject _thinkingGameObj = new GameObject();
    /// <summary>
    /// 先手・後手オブジェクト
    /// </summary>
    GameObject _blackWhiteGameObj = new GameObject();
    /// <summary>
    /// 先手スプライト
    /// </summary>
    Sprite blackSprite;
    /// <summary>
    /// 後手スプライト
    /// </summary>
    Sprite whiteSprite;

    private TurnManager()
    {

    }

    /// <summary>
    /// シングルトン
    /// </summary>
    public static TurnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TurnManager();
            }
            return _instance;
        }
    }

    /// <summary>
    /// 対局の初期化
    /// </summary>
    public void Init()
    {
        InitThinking();
        InitBlackWhite();
        Debug.Log("mode=" + mode);
        _AIManager = GameObject.Find("AIManager").GetComponent<AIController>();
        SfenManager.Instance.AddSfenHistory();
        DisableUndoButton();
    }

    /// <summary>
    /// 対局をリセット
    /// </summary>
    public void Reset()
    {
        DisableGameOverObj();
        IsGameOver = false;
        step = 0;
        _nowPlayer = NowPlayer.Black;
    }

    /// <summary>
    /// 待ったボタンの設定
    /// </summary>
    /// <param name="undoButtonObj"></param>
    public void SetUndoButtonObj(GameObject undoButtonObj)
    {
        this._undoButtonObj = undoButtonObj;
    }

    /// <summary>
    /// ゲームオーバーオブジェクトの設定
    /// </summary>
    /// <param name="gameObj"></param>
    public void SetGameOverObj(GameObject gameObj)
    {
        _gameOverObj = gameObj;
    }

    /// <summary>
    /// サウンドマネジャーの設定
    /// </summary>
    /// <param name="soundManager"></param>
    public void SetSoundManager(SoundManager soundManager)
    {
        this.soundManager = soundManager;
    }

    /// <summary>
    /// 一つ手を進める
    /// </summary>
    public IEnumerator TakeTurn()
    {
        soundManager?.PlaySE(SEType.KomaUti);
        step++;
        _nowPlayer = (_nowPlayer == NowPlayer.Black) ? NowPlayer.White : NowPlayer.Black;
        RefreshThinking(_nowPlayer);
        RefreshCaptureBase(_nowPlayer);
        ResetColorPieceObj();
        ChangeColorLastPieceObj();
        SfenManager.Instance.AddSfenHistory();

        if (JudgeGameOver)
        {
            GameOver();
        }

        if (IsAITurn)
        {
            ExecAI();
        }

        if (!(mode == MatchMode.AIVsAI))
        {
            EnableUndoButton();
        }        

        yield return null;
    }

    /// <summary>
    /// AIの手番かどうか
    /// </summary>
    bool IsAITurn => mode == MatchMode.HumanVsAI && _nowPlayer == NowPlayer.White ||
                     mode == MatchMode.AIVsAI;

    /// <summary>
    /// ゲームオバーかどうかを判定
    /// </summary>
    bool JudgeGameOver => BoardUtility.IsGameOver(_nowPlayer);

    /// <summary>
    /// 局面を１つ戻す
    /// </summary>
    public void Undo()
    {
        if (IsAITurn) //対AIの場合は手番が後手のときは抜ける
        {
            return;
        }
        if (mode == MatchMode.HumanVsAI)
        {
            SfenManager.Instance.RemoveSfenHistory();
            step--;
        }
        SfenManager.Instance.RemoveSfenHistory();
        step--;
        SquareManager squareManager = GameObject.Find("/Canvas/Square").GetComponent<SquareManager>();
        squareManager.Empty();
        squareManager.ResetMovables();
        squareManager.ResetFlag();
        SfenManager.Instance.SetSfen(SfenManager.Instance.GetSfenHistory(step));
        RefreshPiece();
        RefreshThinking(_nowPlayer);
        RefreshCaptureBase(_nowPlayer);
        ResetColorPieceObj();
        IsStopAI = true;

        if (IsGameOver)
        {
            DisableGameOverObj();
            IsGameOver = false;
        }
        if (step < 1)
        {
            DisableUndoButton();
        }

        if (IsAITurn)
        {
            ExecAI();
        }
    }

    /// <summary>
    /// ゲームオーバー時の処理
    /// </summary>
    public void GameOver()
    {
        soundManager?.PlaySE(SEType.GameOver);
        EnableGameOverObj();
        IsGameOver = true;
    }

    /// <summary>
    /// AIを実行
    /// </summary>
    public void ExecAI()
    {
        _AIManager!.Exec();
        IsStopAI = false;
    }

    /// <summary>
    /// サーバーにリクエストして応答待ちのときtrue
    /// </summary>
    /// <returns></returns>
    public bool GetRequestFlag()
    {
        return _AIManager.GetRequestFlag();
    }

    /// <summary>
    /// サーバーからエラーが返ってきたときtrue
    /// </summary>
    /// <returns></returns>
    public bool GetErrorFlag()
    {
        return _AIManager.GetErrorFlag();
    }

    /// <summary>
    /// AIが試行した手を取得
    /// </summary>
    /// <returns></returns>
    public string GetBestMove()
    {
        return _AIManager.GetBestMove();
    }

    /// <summary>
    /// AIが試行した手をリセット
    /// </summary>
    public void ResetBestMove()
    {
        _AIManager.ResetBestMove();
    }

    /// <summary>
    /// 最後に選択した駒の色を変える
    /// </summary>
    void ChangeColorLastPieceObj()
    {
        GameObject squareObj = GameObject.Find("/Canvas/Square");
        SquareManager suquareManager = squareObj.GetComponent<SquareManager>();
        suquareManager.ChangeColorPieceObj(suquareManager.choosePieceObjName);
    }

    /// <summary>
    /// 駒の色を元に戻す
    /// </summary>
    void ResetColorPieceObj()
    {
        GameObject squareObj = GameObject.Find("/Canvas/Square");
        SquareManager squareScript = squareObj.GetComponent<SquareManager>();
        var pieces = GameObject.Find("/Canvas").GetComponentsInChildren<PieceController>();
        foreach (var piece in pieces)
        {
            if (piece.ObjName.Equals(""))
                continue;
            squareScript.ResetColorPieceObj(piece.ObjName);
        }
    }

    /// <summary>
    /// 考え中オブジェクトの初期化
    /// </summary>
    void InitThinking()
    {
        _thinkingGameObj = new GameObject();
        Sprite sprite = Resources.Load<Sprite>("Thinking");
        SpriteRenderer spriteRenderer = _thinkingGameObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        _thinkingGameObj.transform.parent = MonoBehaviour.FindObjectOfType<Canvas>().transform;
        _thinkingGameObj.transform.localScale = new Vector3(18, 18, 0);
        _thinkingGameObj.transform.name = "Thinking";
        _thinkingGameObj.transform.position = new Vector3(2, 10, -3);
        BoardUtility.BlinkObject(_thinkingGameObj);
        RefreshThinking(_nowPlayer);
    }

    /// <summary>
    /// 考え中オブジェクトの更新
    /// </summary>
    /// <param name="reserve"></param>
    void RefreshThinking(NowPlayer nowPlayer)
    {
        float plusX = 2.0f;
        GameObject captureObj = GameObject.Find("Canvas/CaptureBlack");
        if (nowPlayer == NowPlayer.White)
        {
            captureObj = GameObject.Find("Canvas/CaptureWhite");
            plusX = -2.0f;
        }
        _thinkingGameObj.transform.position = new Vector3(captureObj.transform.position.x + plusX, captureObj.transform.position.y, -3);
    }

    /// <summary>
    /// 先手・後手の初期化
    /// </summary>
    void InitBlackWhite()
    {
        _blackWhiteGameObj = new GameObject();
        blackSprite = Resources.Load<Sprite>("black");
        whiteSprite = Resources.Load<Sprite>("white");
        SpriteRenderer spriteRender = _blackWhiteGameObj.AddComponent<SpriteRenderer>();
        _blackWhiteGameObj.transform.parent = MonoBehaviour.FindObjectOfType<Canvas>().transform;
        _blackWhiteGameObj.transform.localScale = new Vector3(40, 40, 0);
        _blackWhiteGameObj.transform.name = "BlackWhite";
        _blackWhiteGameObj.transform.position = new Vector3(0, 10, -3);
        RefreshCaptureBase(_nowPlayer);
    }

    /// <summary>
    /// 駒台の駒オブジェクトの更新
    /// </summary>
    /// <param name="reserve"></param>
    void RefreshCaptureBase(NowPlayer nowPlayer)
    {
        float plusX = 2.4f;
        GameObject motigomaObj = GameObject.Find("Canvas/CaptureBlack");
        _blackWhiteGameObj.GetComponent<SpriteRenderer>().sprite = blackSprite;
        if (nowPlayer == NowPlayer.White)
        {
            motigomaObj = GameObject.Find("Canvas/CaptureWhite");
            plusX = -2.4f;
            _blackWhiteGameObj.GetComponent<SpriteRenderer>().sprite = whiteSprite;
        }
        _blackWhiteGameObj.transform.position = new Vector3(motigomaObj.transform.position.x + plusX, motigomaObj.transform.position.y, -3);
    }

    /// <summary>
    /// ゲームオーバーオブジェクトの有効化
    /// </summary>
    void EnableGameOverObj()
    {
        _gameOverObj.SetActive(true);
    }

     /// <summary>
     /// ゲームオーバーオブジェクトの無効化
     /// </summary>
    void DisableGameOverObj()
    {
        _gameOverObj.SetActive(false);
    }

    /// <summary>
    /// 待ったボタンの有効化
    /// </summary>
    void EnableUndoButton()
    {
        if (!_undoButtonObj.activeSelf)
        {
            _undoButtonObj.SetActive(true);
        }
    }

    /// <summary>
    /// 待ったボタンの無効化
    /// </summary>
    void DisableUndoButton()
    {
        if (_undoButtonObj.activeSelf)
        {
            _undoButtonObj.SetActive(false);
        }
    }

    /// <summary>
    /// マスに駒を生成する
    /// </summary>
    void RefreshPiece()
    {
        GameObject.Find("/Canvas/Square").GetComponent<SquareManager>().RefreshPiece();
        GameObject.Find("/Canvas/CaptureBlack").GetComponent<CaptureManager>().RefreshPiece();
        GameObject.Find("/Canvas/CaptureWhite").GetComponent<CaptureManager>().RefreshPiece();
    }
}

/// <summary>
/// 現在の手番
/// </summary>
public enum NowPlayer
{
    /// <summary>
    /// 先手
    /// </summary>
    Black,
    /// <summary>
    /// 後手
    /// </summary>
    White,
}

/// <summary>
/// 対戦モード
/// </summary>
public enum MatchMode
{
    /// <summary>
    /// 人対人 
    /// </summary>
    HumanVsHuman,
    /// <summary>
    /// 人対AI
    /// </summary>
    HumanVsAI,
    /// <summary>
    /// AI対AI
    /// </summary>
    AIVsAI,
}
