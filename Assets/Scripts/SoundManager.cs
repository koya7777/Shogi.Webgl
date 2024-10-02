using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// BGM,SE�̊Ǘ�
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
    /// BGM�Đ� 
    /// </summary>
    /// <param name="bgmType"></param>
    public void PlayBGM(BGMType bgmType)
    {
        audioSource.clip = BGMList[(int)bgmType];
        audioSource.Play();
    }

    /// <summary>
    /// SE�Đ�
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
    /// �^�C�g����BGM
    /// </summary>
    Title,
}
public enum SEType
{
    /// <summary>
    /// ���ł��̌��ʉ�
    /// </summary>
    KomaUti,
    /// <summary>
    /// �Q�[���I�[�o�[�̂Ƃ��̌��ʉ�
    /// </summary>
    GameOver,
}
