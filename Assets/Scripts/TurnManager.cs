using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �΋ǊǗ�
/// </summary>
public class TurnManager
{
    private static TurnManager _instance;

    /// <summary>
    /// ��肪false
    /// ��肪true
    /// </summary>
    public NowPlayer _nowPlayer = NowPlayer.Black;
    /// <summary>
    /// �萔
    /// </summary>
    public int step = 0;
    /// <summary>
    /// �Q�[���I�[�o�[�ɂȂ����Ƃ�
    /// </summary>
    public bool IsGameOver = false;
    /// <summary>
    /// AI���~�����Ƃ�
    /// </summary>
    public bool IsStopAI = false;
    /// <summary>
    /// AI�}�l�[�W���[
    /// </summary>
    AIController _AIManager;
    
    SoundManager soundManager = SoundManager.Instance;
    BoardManager boardManager = BoardManager.Instance;

    /// <summary>
    /// �ΐ탂�[�h
    /// </summary>
    public MatchMode mode = MatchMode.HumanVsHuman;

    /// <summary>
    /// �Q�[���I�[�o�[�I�u�W�F�N�g
    /// </summary>
    GameObject _gameOverObj;
    /// <summary>
    /// �҂����{�^���I�u�W�F�N�g
    /// </summary>
    GameObject _undoButtonObj;
    /// <summary>
    /// �l�����I�u�W�F�N�g
    /// </summary>
    GameObject _thinkingGameObj = new GameObject();
    /// <summary>
    /// ���E���I�u�W�F�N�g
    /// </summary>
    GameObject _blackWhiteGameObj = new GameObject();
    /// <summary>
    /// ���X�v���C�g
    /// </summary>
    Sprite blackSprite;
    /// <summary>
    /// ���X�v���C�g
    /// </summary>
    Sprite whiteSprite;

    private TurnManager()
    {

    }

    /// <summary>
    /// �V���O���g��
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
    /// �΋ǂ̏�����
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
    /// �΋ǂ����Z�b�g
    /// </summary>
    public void Reset()
    {
        DisableGameOverObj();
        IsGameOver = false;
        step = 0;
        _nowPlayer = NowPlayer.Black;
    }

    /// <summary>
    /// �҂����{�^���̐ݒ�
    /// </summary>
    /// <param name="undoButtonObj"></param>
    public void SetUndoButtonObj(GameObject undoButtonObj)
    {
        this._undoButtonObj = undoButtonObj;
    }

    /// <summary>
    /// �Q�[���I�[�o�[�I�u�W�F�N�g�̐ݒ�
    /// </summary>
    /// <param name="gameObj"></param>
    public void SetGameOverObj(GameObject gameObj)
    {
        _gameOverObj = gameObj;
    }

    /// <summary>
    /// �T�E���h�}�l�W���[�̐ݒ�
    /// </summary>
    /// <param name="soundManager"></param>
    public void SetSoundManager(SoundManager soundManager)
    {
        this.soundManager = soundManager;
    }

    /// <summary>
    /// ����i�߂�
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
    /// AI�̎�Ԃ��ǂ���
    /// </summary>
    bool IsAITurn => mode == MatchMode.HumanVsAI && _nowPlayer == NowPlayer.White ||
                     mode == MatchMode.AIVsAI;

    /// <summary>
    /// �Q�[���I�o�[���ǂ����𔻒�
    /// </summary>
    bool JudgeGameOver => BoardUtility.IsGameOver(_nowPlayer);

    /// <summary>
    /// �ǖʂ��P�߂�
    /// </summary>
    public void Undo()
    {
        if (IsAITurn) //��AI�̏ꍇ�͎�Ԃ����̂Ƃ��͔�����
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
    /// �Q�[���I�[�o�[���̏���
    /// </summary>
    public void GameOver()
    {
        soundManager?.PlaySE(SEType.GameOver);
        EnableGameOverObj();
        IsGameOver = true;
    }

    /// <summary>
    /// AI�����s
    /// </summary>
    public void ExecAI()
    {
        _AIManager!.Exec();
        IsStopAI = false;
    }

    /// <summary>
    /// �T�[�o�[�Ƀ��N�G�X�g���ĉ����҂��̂Ƃ�true
    /// </summary>
    /// <returns></returns>
    public bool GetRequestFlag()
    {
        return _AIManager.GetRequestFlag();
    }

    /// <summary>
    /// �T�[�o�[����G���[���Ԃ��Ă����Ƃ�true
    /// </summary>
    /// <returns></returns>
    public bool GetErrorFlag()
    {
        return _AIManager.GetErrorFlag();
    }

    /// <summary>
    /// AI�����s��������擾
    /// </summary>
    /// <returns></returns>
    public string GetBestMove()
    {
        return _AIManager.GetBestMove();
    }

    /// <summary>
    /// AI�����s����������Z�b�g
    /// </summary>
    public void ResetBestMove()
    {
        _AIManager.ResetBestMove();
    }

    /// <summary>
    /// �Ō�ɑI��������̐F��ς���
    /// </summary>
    void ChangeColorLastPieceObj()
    {
        GameObject squareObj = GameObject.Find("/Canvas/Square");
        SquareManager suquareManager = squareObj.GetComponent<SquareManager>();
        suquareManager.ChangeColorPieceObj(suquareManager.choosePieceObjName);
    }

    /// <summary>
    /// ��̐F�����ɖ߂�
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
    /// �l�����I�u�W�F�N�g�̏�����
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
    /// �l�����I�u�W�F�N�g�̍X�V
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
    /// ���E���̏�����
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
    /// ���̋�I�u�W�F�N�g�̍X�V
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
    /// �Q�[���I�[�o�[�I�u�W�F�N�g�̗L����
    /// </summary>
    void EnableGameOverObj()
    {
        _gameOverObj.SetActive(true);
    }

     /// <summary>
     /// �Q�[���I�[�o�[�I�u�W�F�N�g�̖�����
     /// </summary>
    void DisableGameOverObj()
    {
        _gameOverObj.SetActive(false);
    }

    /// <summary>
    /// �҂����{�^���̗L����
    /// </summary>
    void EnableUndoButton()
    {
        if (!_undoButtonObj.activeSelf)
        {
            _undoButtonObj.SetActive(true);
        }
    }

    /// <summary>
    /// �҂����{�^���̖�����
    /// </summary>
    void DisableUndoButton()
    {
        if (_undoButtonObj.activeSelf)
        {
            _undoButtonObj.SetActive(false);
        }
    }

    /// <summary>
    /// �}�X�ɋ�𐶐�����
    /// </summary>
    void RefreshPiece()
    {
        GameObject.Find("/Canvas/Square").GetComponent<SquareManager>().RefreshPiece();
        GameObject.Find("/Canvas/CaptureBlack").GetComponent<CaptureManager>().RefreshPiece();
        GameObject.Find("/Canvas/CaptureWhite").GetComponent<CaptureManager>().RefreshPiece();
    }
}

/// <summary>
/// ���݂̎��
/// </summary>
public enum NowPlayer
{
    /// <summary>
    /// ���
    /// </summary>
    Black,
    /// <summary>
    /// ���
    /// </summary>
    White,
}

/// <summary>
/// �ΐ탂�[�h
/// </summary>
public enum MatchMode
{
    /// <summary>
    /// �l�ΐl 
    /// </summary>
    HumanVsHuman,
    /// <summary>
    /// �l��AI
    /// </summary>
    HumanVsAI,
    /// <summary>
    /// AI��AI
    /// </summary>
    AIVsAI,
}
