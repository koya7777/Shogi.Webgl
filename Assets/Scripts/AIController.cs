using Assets.Scripts.Interface;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AIController : MonoBehaviour, IAIController
{
    const string uri = "https://17xn1ovxga.execute-api.ap-northeast-1.amazonaws.com/production/gikou?byoyomi=1&position=";
    bool _webRequestFlag = false;
    WebRequest _webRequest;
    string _bestMove;
    List<string> _bestPv = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        _webRequest = new GameObject().AddComponent<WebRequest>();
        _webRequest.Timeout = 1;
        _webRequest.RequestSuccessEvent.AddListener(()=>
        {
            if (!_webRequest.GetBody().Equals(""))
            {
                var jsonObject = JObject.Parse(_webRequest.GetBody());
                //var depth = jsonObject["bestpv"]["depth"].ToString();
                //var score = jsonObject["bestpv"]["score_cp"].ToString();
                _bestMove = jsonObject["bestmove"].ToString();
                var pvArray = (JArray)jsonObject["bestpv"]["pv"];
                _bestPv.Clear();
                foreach (var item in pvArray)
                {
                    _bestPv.Add(item.ToString());
                }
                _webRequestFlag = false;
                Debug.Log("BestMove = " + _bestMove);
            }
            else if (_webRequest.ErrorFlag)
            {
            }
        });

        _webRequest.RequestFailureEvent.AddListener(() =>
        {
            _webRequestFlag = false;
        });
    }

    private void Update()
    {
        
    }

    public void Exec()
    {
        string sfen = SfenManager.Instance.GetSfen();
        Debug.Log("sfen = " + sfen);
        string url = uri + UnityWebRequest.EscapeURL(sfen);
        _webRequest.Reset();
        _webRequest.Exec(url);
        _webRequestFlag = true;
    }

    public string GetBestMove()
    {
        return _bestMove;
    }

    public bool GetRequestFlag()
    {
        return _webRequestFlag;
    }
    
    public bool GetErrorFlag()
    {
        return _webRequest.ErrorFlag;
    }

    public void ResetBestMove()
    {
        _bestMove = "";
    }
}
