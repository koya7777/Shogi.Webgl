using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ‘Ò‚Á‚½ƒ{ƒ^ƒ“
/// </summary>
public class UndoButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(()=>
        {
            TurnManager.Instance.Undo();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
