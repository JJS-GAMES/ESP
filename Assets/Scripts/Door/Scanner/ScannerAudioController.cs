using UnityEngine;

public class ScannerAudioController : MonoBehaviour
{
    private AudioSource _voiceSource;
    private AudioSource _loopSource;

    public void Init(float volume)
    {
        _voiceSource = gameObject.AddComponent<AudioSource>();
        _loopSource = gameObject.AddComponent<AudioSource>();

        _voiceSource.loop = false;
        _voiceSource.playOnAwake = false;
        _voiceSource.volume = volume;
        _voiceSource.spatialBlend = 1f;
        _voiceSource.dopplerLevel = 0f;
        _voiceSource.spread = 0f;

        _loopSource.loop = true;
        _loopSource.playOnAwake = false;
        _loopSource.volume = volume;
        _loopSource.spatialBlend = 1f;
        _loopSource.dopplerLevel = 0f;
        _loopSource.spread = 0f;
    }

    public void PlaySoundOneShot(AudioClip clip)
    {
        if (clip != null)
            _voiceSource?.PlayOneShot(clip);
    }

    public void PlayLoopSound(AudioClip clip)
    {
        if (_loopSource == null) return;

        if (clip != null)
        {
            if (_loopSource.isPlaying) return;
            _loopSource.clip = clip;
            _loopSource.Play();
        }
    }

    public void StopLoopSound()
    {
        if (_loopSource == null) return;

        _loopSource.Stop();
        _loopSource.clip = null;
    }
}
