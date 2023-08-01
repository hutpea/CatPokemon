using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    public void Init()
    {
        sfxAudioSource = transform.Find("SFX_Audio").GetComponent<AudioSource>();
        musicAudioSource = transform.Find("Music_Audio").GetComponent<AudioSource>();
        cat1AudioSource = transform.Find("Cat1_Audio").GetComponent<AudioSource>();
        cat2AudioSource = transform.Find("Cat2_Audio").GetComponent<AudioSource>();

        AudioMixerGroup[] musics = gameAudioMixer.FindMatchingGroups("Music");
        musicMixer = musics[0];
        AudioMixerGroup[] sfxs = gameAudioMixer.FindMatchingGroups("SFX");
        sfxMixer = sfxs[0];
        Debug.Log(musicMixer + " " + sfxMixer);
    }

    [SerializeField] private AudioClip buttonNormalSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip connectSound;
    [SerializeField] private AudioClip cannotConnectSound;
    [SerializeField] private AudioClip[] catMewSound;
    [SerializeField] private AudioClip BGMusic1;

    [SerializeField] private AudioMixer gameAudioMixer;
    private AudioMixerGroup musicMixer;
    private AudioMixerGroup sfxMixer;

    private AudioSource sfxAudioSource;
    private AudioSource musicAudioSource;
    private AudioSource cat1AudioSource;
    private AudioSource cat2AudioSource;

    public void PlaySound(AUDIO_CLIP_TYPE soundType, int index = 1)
    {
        switch (soundType)
        {
            case AUDIO_CLIP_TYPE.ButtonNormal:
                {
                    sfxAudioSource.clip = buttonNormalSound;
                    sfxAudioSource.Play();
                    break;
                }
            case AUDIO_CLIP_TYPE.Win:
                {
                    sfxAudioSource.clip = winSound;
                    sfxAudioSource.Play();
                    break;
                }
            case AUDIO_CLIP_TYPE.Lose:
                {
                    Debug.Log("LOSE SOUND");
                    sfxAudioSource.clip = loseSound;
                    sfxAudioSource.Play();
                    break;
                }
            case AUDIO_CLIP_TYPE.Connect:
                {
                    sfxAudioSource.clip = connectSound;
                    sfxAudioSource.Play();
                    break;
                }
            case AUDIO_CLIP_TYPE.CannotConnect:
                {
                    sfxAudioSource.clip = cannotConnectSound;
                    sfxAudioSource.Play();
                    break;
                }
            case AUDIO_CLIP_TYPE.CatMewSound:
                {
                    if(index == 1)
                    {
                        cat1AudioSource.clip = catMewSound[UnityEngine.Random.Range(0, catMewSound.Length)];
                        cat1AudioSource.Play();
                    }
                    else
                    {
                        cat2AudioSource.clip = catMewSound[UnityEngine.Random.Range(0, catMewSound.Length)];
                        cat2AudioSource.Play();
                    }
                    
                    break;
                }
            case AUDIO_CLIP_TYPE.BGMusic1:
                {
                    musicAudioSource.clip = BGMusic1;
                    musicAudioSource.Play();
                    Debug.Log("music source play bg music");
                    break;
                }
        }

    }

    public void ToggleMusic()
    {
        if (GameController.Instance.useProfile.OnMusic)
        {
            musicMixer.audioMixer.SetFloat("VolumeMusic", -80f);
            GameController.Instance.useProfile.OnMusic = false;
        }
        else
        {
            musicMixer.audioMixer.SetFloat("VolumeMusic", -14f);
            GameController.Instance.useProfile.OnMusic = true;
        }
    }
    public void ToggleSound()
    {
        if (GameController.Instance.useProfile.OnSound)
        {
            sfxMixer.audioMixer.SetFloat("VolumeSFX", -80f);
            GameController.Instance.useProfile.OnSound = false;
        }
        else
        {
            sfxMixer.audioMixer.SetFloat("VolumeSFX", 0);
            GameController.Instance.useProfile.OnSound = true;
        }
    }

    public void ToggleVibration()
    {
        if (UseProfile.OnVibration)
        {
            UseProfile.OnVibration = false;
        }
        else
        {
            UseProfile.OnVibration = true;
        }
    }
}

