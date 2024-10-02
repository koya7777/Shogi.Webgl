using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    /// <summary>
    /// ���N�G�X�g�������̃C�x���g
    /// </summary>
    public UnityEvent RequestSuccessEvent = new UnityEvent();

    /// <summary>
    /// ���N�G�X�g���s���̃C�x���g
    /// </summary>
    public UnityEvent RequestFailureEvent = new UnityEvent();

    string _text = "";
    public bool ErrorFlag = false;

    public int Timeout { get; internal set; }

    public void Exec(string url)
    {
        _text = "";
        ErrorFlag = false;
        StartCoroutine(Get(url));
    }

    IEnumerator Get(string url)
    {
        UnityWebRequest request = new UnityWebRequest();
        request.downloadHandler = new DownloadHandlerBuffer();
        request.url = url;
        request.timeout = Timeout;
        request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
        request.method = UnityWebRequest.kHttpVerbGET;
        yield return request.SendWebRequest();
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("���N�G�X�g��");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("���N�G�X�g����");
                _text = request.downloadHandler.text;
                Debug.Log(_text);
                RequestSuccessEvent?.Invoke();
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log
                (
                    @"�T�[�o�Ƃ̒ʐM�Ɏ��s�B
���N�G�X�g���ڑ��ł��Ȃ������A
�Z�L�����e�B�ŕی삳�ꂽ�`���l�����m���ł��Ȃ������ȂǁB"
                );
                ErrorFlag = true;
                RequestFailureEvent?.Invoke();
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log
                (
                    @"�T�[�o���G���[������Ԃ����B
�T�[�o�Ƃ̒ʐM�ɂ͐����������A
�ڑ��v���g�R���Œ�`����Ă���G���[���󂯎�����B"
                );
                ErrorFlag = true;
                RequestFailureEvent?.Invoke();
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log
                (
                    @"�f�[�^�̏������ɃG���[�������B
���N�G�X�g�̓T�[�o�Ƃ̒ʐM�ɐ����������A
��M�����f�[�^�̏������ɃG���[�������B
�f�[�^���j�����Ă��邩�A�������`���ł͂Ȃ��ȂǁB"
                );
                ErrorFlag = true;
                RequestFailureEvent?.Invoke();
                break;

            default: throw new ArgumentOutOfRangeException();
        }
    }

    public string GetBody()
    {
        return _text;
    }

    public void Reset()
    {
        _text = "";
        ErrorFlag = false;
    }
}
