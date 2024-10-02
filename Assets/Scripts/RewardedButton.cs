using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardedButton : MonoBehaviour
{
    public Text ResultText;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
        });

    }

    // Update is called once per frame
    void Update()
    {

    }
}
