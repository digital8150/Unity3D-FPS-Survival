using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //곡의 이름
    public AudioClip clip; //곡
}

public class SoundManager : MonoBehaviour
{

    static public SoundManager instance;

    //싱글톤 패턴
    #region singleton
    private void Awake() //객체 생성 시 최초 실행
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
                Debug.LogWarning("모든 가용 AudioSource가 사용중입니다");
                return;
            }
        }
        Debug.LogWarning($"{_name} 사운드가 SoundManager에 등록되지 않았습니다.");
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
        Debug.Log($"재생 중인 {_name} 사운드가 없습니다.");
    }


}
