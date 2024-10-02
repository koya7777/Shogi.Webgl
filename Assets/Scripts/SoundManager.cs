using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// BGM,SEの管理
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> BGMList;
    [SerializeField] List<AudioClip> SEList;

    AudioSource audioSource;

    private static SoundManager instance;

    public static SoundManager Instance { get => instance; private set => instance = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }

    void Update()
    {

    }

    /// <summary>
    /// BGM再生 
    /// </summary>
    /// <param name="bgmType"></param>
    public void PlayBGM(BGMType bgmType)
    {
        audioSource.clip = BGMList[(int)bgmType];
        audioSource.Play();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="seType"></param>
    public void PlaySE(SEType seType)
    {
        audioSource.PlayOneShot(SEList[(int)seType]);
    }
}

public enum BGMType
{
    /// <summary>
    /// タイトルのBGM
    /// </summary>
    Title,
}
public enum SEType
{
    /// <summary>
    /// 駒を打つ時の効果音
    /// </summary>
    KomaUti,
    /// <summary>
    /// ゲームオーバーのときの効果音
    /// </summary>
    GameOver,
}
