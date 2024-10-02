using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    /// <summary>
    /// リクエスト成功時のイベント
    /// </summary>
    public UnityEvent RequestSuccessEvent = new UnityEvent();

    /// <summary>
    /// リクエスト失敗時のイベント
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
                Debug.Log("リクエスト中");
                break;

            case UnityWebRequest.Result.Success:
                Debug.Log("リクエスト成功");
                _text = request.downloadHandler.text;
                Debug.Log(_text);
                RequestSuccessEvent?.Invoke();
                break;

            case UnityWebRequest.Result.ConnectionError:
                Debug.Log
                (
                    @"サーバとの通信に失敗。
リクエストが接続できなかった、
セキュリティで保護されたチャネルを確立できなかったなど。"
                );
                ErrorFlag = true;
                RequestFailureEvent?.Invoke();
                break;

            case UnityWebRequest.Result.ProtocolError:
                Debug.Log
                (
                    @"サーバがエラー応答を返した。
サーバとの通信には成功したが、
接続プロトコルで定義されているエラーを受け取った。"
                );
                ErrorFlag = true;
                RequestFailureEvent?.Invoke();
                break;

            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log
                (
                    @"データの処理中にエラーが発生。
リクエストはサーバとの通信に成功したが、
受信したデータの処理中にエラーが発生。
データが破損しているか、正しい形式ではないなど。"
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
