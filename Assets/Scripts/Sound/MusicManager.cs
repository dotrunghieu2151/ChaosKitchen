using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private AudioSource _audioSource;
    private float _volume = 0.3f;

    private const string PLAYER_PREF_MUSIC_VOLUME = "SoundMusicVolume";

    private void Awake()
    {
        Instance = this;
        _volume = PlayerPrefs.GetFloat(PLAYER_PREF_MUSIC_VOLUME, 0.3f);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _volume;
    }
    public void ChangeVolume()
    {
        _volume += 0.1f;
        if (_volume > 1.1f)
        {
            _volume = 0f;
        }
        _audioSource.volume = _volume;
        PlayerPrefs.SetFloat(PLAYER_PREF_MUSIC_VOLUME, _volume);
    }

    public float GetVolume()
    {
        return _volume;
    }
}
