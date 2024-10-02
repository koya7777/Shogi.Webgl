using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleStartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            TurnManager.Instance.SetSoundManager(soundManager);
            if (transform.name.IndexOf("HumanVsHuman") > -1)
            {
                TurnManager.Instance.mode = MatchMode.HumanVsHuman;
            }
            else if (transform.name.IndexOf("HumanVsCom") > -1)
            {
                TurnManager.Instance.mode = MatchMode.HumanVsAI;
            }
            else if (transform.name.IndexOf("ComVsCom") > -1)
            {
                TurnManager.Instance.mode = MatchMode.AIVsAI;
            }
            SceneManager.LoadScene("MainScene");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
