using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] BackgroundSounds;
    public Sound[] SFXSounds;

    private AudioSource backgroundAudioSource;
    private AudioSource sfxAudioSource;

    private void Start()
    {
        backgroundAudioSource = gameObject.AddComponent<AudioSource>();
        backgroundAudioSource.loop = true;
        backgroundAudioSource.playOnAwake = false;

        sfxAudioSource = gameObject.AddComponent<AudioSource>();
    }
    public void PlayBackgroundSound(string name)
    {
        Sound sound = System.Array.Find(BackgroundSounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        backgroundAudioSource.clip = sound.clip;
        backgroundAudioSource.volume = sound.volume;
        backgroundAudioSource.pitch = sound.pitch;
        backgroundAudioSource.Play();
    }

    public void StopBackgroundSound()
    {
        backgroundAudioSource.Stop();
    }

    public void PlaySoundEffect(string name)
    {
        Sound sound = System.Array.Find(SFXSounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        sfxAudioSource.PlayOneShot(sound.clip, sound.volume);
    }

    public void SetBackgroundVolume(float volume)
    {
        backgroundAudioSource.volume = Mathf.Clamp01(volume);
        foreach (Sound sound in BackgroundSounds)
        {
            sound.volume = Mathf.Clamp01(volume);
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = Mathf.Clamp01(volume);
        foreach (Sound sound in SFXSounds)
        {
            sound.volume = Mathf.Clamp01(volume);
        }
    }

}
