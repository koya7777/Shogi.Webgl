using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 駒管理
/// PieceManager manager = PieceManager.Instance;
/// </summary>
public class PieceManager : MonoBehaviour {
	private static PieceManager _instance;
	// 駒作成ID()
	public int komaAttachId = 0;
	private PieceManager () {
	}
	public static PieceManager Instance {
		get {
			if (_instance == null) {
				GameObject gameObject = new GameObject("PieceManager");
				_instance = gameObject.AddComponent<PieceManager>();
			}
			return _instance;
		}
	}
	// 駒作成するたびにIDをインクリメンタルする
	public int IssuePieceId(){
		komaAttachId++;
		return komaAttachId;
	}
	void Start () {
	}
	void Update () {
	}
}