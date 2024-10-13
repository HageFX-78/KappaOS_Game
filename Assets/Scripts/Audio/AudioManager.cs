using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("UI Reference (Optional)")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfSlider;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerGroup masterGroup;
    [SerializeField] AudioMixerGroup bgmGroup;
    [SerializeField] AudioMixerGroup sfxGroup;

    private const string MasterVolume = "MasterVolume";
    private const string BGMVolume = "BGMVolume";
    private const string SFXVolume = "SFXVolume";

    private const string masterMix = "Master_Mixer";
    private const string bgmMix = "BGM_Mixer";
    private const string sfxMix = "SFX_Mixer";

    [SerializeField] int sfxPoolSize = 20;
    [Header("Audio Lists")]
    [NonReorderable]
    public List<Sound> BGM = new List<Sound>();
    [NonReorderable]
    public List<AudioGroupWrapper> SFX = new List<AudioGroupWrapper>();

    private AudioSource bgmSource;
    private Queue<AudioSource> sfxQueue = new Queue<AudioSource>();
    private Dictionary<string, Sound> audioDictionary = new Dictionary<string, Sound>();

    public static AudioManager amInstance;
    void Awake()
    {
        if (amInstance == null)
        {
            amInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        


        //Create a dictionary for easy access
        foreach (Sound s in BGM)
        {
            audioDictionary.Add(s.name, s);
        }
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.outputAudioMixerGroup = bgmGroup;

        // SFX Pool
        foreach (AudioGroupWrapper group in SFX)
        {
            foreach (Sound s in group.groupAudio)
            {
                audioDictionary.Add(s.name, s);
            }
        }
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxGroup;
            sfxQueue.Enqueue(sfxSource);
        }
        ResetAllVolume();
    }
    void Start()
    {
        UpdateSliders();
    }


    public void ResetAllVolume()
    {
        PlayerPrefs.SetFloat(MasterVolume, 1.0f);
        PlayerPrefs.SetFloat(BGMVolume, 1.0f);
        PlayerPrefs.SetFloat(SFXVolume, 1.0f);

        UpdateSliders(1.0f);
    }
    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat(MasterVolume, volume);
        audioMixer.SetFloat(masterMix, volume == 0? -80f:Mathf.Log10(volume) * 20);
    }
    public void SetBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat(BGMVolume, volume);
        audioMixer.SetFloat(bgmMix, volume == 0? -80f:Mathf.Log10(volume) * 20);
    }
    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(SFXVolume, volume);
        audioMixer.SetFloat(sfxMix, volume == 0? -80f:Mathf.Log10(volume) * 20);
    }

    public void UpdateSliders(float val = -1f)
    {
        //Defaults to -1 if val is <=0, meaning use player prefs value
        if(masterSlider!=null)
        {
            masterSlider.value = val<=0?PlayerPrefs.GetFloat(MasterVolume):val;
        }
        if(bgmSlider!=null)
        {
            bgmSlider.value = val<=0?PlayerPrefs.GetFloat(BGMVolume):val;
        }
        if(sfSlider!=null)
        {
            sfSlider.value = val<=0?PlayerPrefs.GetFloat(SFXVolume):val;
        }
    }
    public void ReassignSliderReferences(Slider mSlider, Slider bSlider, Slider sSlider)
    {
        masterSlider = mSlider;
        bgmSlider = bSlider;
        sfSlider = sSlider;
    }
    public void DebugVolume()
    {
        Debug.Log("Master Volume: " + PlayerPrefs.GetFloat(MasterVolume));
        Debug.Log("BGM Volume: " + PlayerPrefs.GetFloat(BGMVolume));
        Debug.Log("SFX Volume: " + PlayerPrefs.GetFloat(SFXVolume));
    }



#region PlayStop
    public void PlayBGM(string name)
    {
        bgmSource.clip = audioDictionary[name].clip;
        bgmSource.volume = audioDictionary[name].volume;
        bgmSource.pitch = audioDictionary[name].pitch;
        bgmSource.Play();
        
    }
    public void PlaySF(string name)
    {
        AudioSource sfxSource = sfxQueue.Dequeue();
        sfxSource.enabled = true;
        sfxSource.clip = audioDictionary[name].clip;
        sfxSource.volume = audioDictionary[name].volume;
        sfxSource.pitch = audioDictionary[name].pitch;
        sfxSource.Play();
        sfxQueue.Enqueue(sfxSource);
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }
#endregion PlayStop

    public void StopAllSF()
    {
        foreach (AudioSource sfxSource in sfxQueue)
        {
            sfxSource.Stop();
        }
    }
}
