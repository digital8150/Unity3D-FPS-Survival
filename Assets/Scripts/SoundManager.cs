using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //���� �̸�
    public AudioClip clip; //��
}

public class SoundManager : MonoBehaviour
{

    static public SoundManager instance;

    //�̱��� ����
    #region singleton
    private void Awake() //��ü ���� �� ���� ����
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBGM;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }


    public void PlaySE(string _name)
    {
        foreach (var sound in effectSounds)
        {
            if (sound.name == _name)
            {
                for (int i = 0; i < audioSourceEffects.Length; i++)
                {
                    if (!audioSourceEffects[i].isPlaying)
                    {
                        playSoundName[i] = sound.name;
                        audioSourceEffects[i].clip = sound.clip;
                        audioSourceEffects[i].Play();
                        return;
                    }
                }
                Debug.LogWarning("��� ���� AudioSource�� ������Դϴ�");
                return;
            }
        }
        Debug.LogWarning($"{_name} ���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
    }

    public void StopAllSE()
    {
        foreach (AudioSource audioSource in audioSourceEffects)
        {
            audioSource.Stop();
        }
    }

    public void StopSE(string _name)
    {
        for(int i = 0; i <audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log($"��� ���� {_name} ���尡 �����ϴ�.");
    }


}
