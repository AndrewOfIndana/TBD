using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameOptions : MonoBehaviour
{
    public AudioMixer musicMixer;
    public AudioMixer effectMixer;
    public AudioMixer voiceMixer;
    public VolumeProfile postProcess;

    private ColorAdjustments colorAdjustments;

    private float cameraZoom = 6;
    private float musicVolume = 0;
    private float effectVolume = 0;
    private float voiceVolume = 0;
    private float brightness = 0;

    private void Start()
    {
        if(postProcess.TryGet<ColorAdjustments>(out var colAdj))
        {
            colorAdjustments = colAdj;
            colorAdjustments.postExposure.value = brightness;
            colorAdjustments.colorFilter.value = new Color(1, 1, 1);
        }
    }

    public void SetCameraZoom(float zoom)
    {
        cameraZoom = zoom;
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicMixer.SetFloat("musicVolume", musicVolume);
    }
    public void SetEffectVolume(float volume)
    {
        effectVolume = volume;
        effectMixer.SetFloat("effectsVolume", effectVolume);
    }
    public void SetVoiceVolume(float volume)
    {
        voiceVolume = volume;
        voiceMixer.SetFloat("voiceVolume", voiceVolume);
    }
    public void SetBrightness(float br)
    {
        brightness = br;
        colorAdjustments.postExposure.value = brightness;
    }
    public void SetColorFilter(Color fil)
    {
        colorAdjustments.colorFilter.value = fil;
    }

    /*---      SET/GET FUNCTIONS     ---*/
    public float GetCameraZoom()
    {
        return cameraZoom * -1;
    }
    public float GetCameraZoomY()
    {
        return cameraZoom * 0.4f;
    }
    public float GetCameraZoomAttackRange()
    {
        return cameraZoom * 2;
    }
    public float GetVoiceClipVolume()
    {
        return (float)(voiceVolume + 80)/100;;
    }
    public float GetOptionsValues(int index)
    {
        if(index == 0)
        {
            return cameraZoom;
        }
        else if(index == 1)
        {
            return musicVolume;
        }
            else if(index == 2)
        {
            return effectVolume;
        }
        else if(index == 3)
        {
            return voiceVolume;
        }
        else if(index == 4)
        {
            return brightness;
        }
        return 0;
    }
}
