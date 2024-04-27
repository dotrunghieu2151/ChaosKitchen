using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefSO _audioClipRefsSO;

    private const string PLAYER_PREF_SOUND_EFFECT_VOLUME = "SoundEffectVolume";

    private float _volume = 1f;

    private void Awake()
    {
        _volume = PlayerPrefs.GetFloat(PLAYER_PREF_SOUND_EFFECT_VOLUME, 1f);
    }
    private void Start()
    {
        Instance = this;
        DeliveryManager.Instance.OnRecipeSuccess += (sender, args) =>
        {
            PlaySound(_audioClipRefsSO.deliverySuccess, args.deliveryCounter.transform.position);
        };

        DeliveryManager.Instance.OnRecipeFailed += (sender, args) =>
        {
            PlaySound(_audioClipRefsSO.deliveryFailed, args.deliveryCounter.transform.position);
        };

        CuttingCounter.OnAnyCut += (sender, args) =>
        {
            PlaySound(_audioClipRefsSO.chop, (sender as CuttingCounter).transform.position);
        };

        PlayerMovement.OnPlayerPickup += (sender, args) =>
        {
            PlaySound(_audioClipRefsSO.objectPickup, (sender as PlayerMovement).transform.position);
        };

        BaseCounter.OnAnyObjectPlacedHere += (sender, args) =>
        {
            PlaySound(_audioClipRefsSO.objectDrop, (sender as BaseCounter).transform.position);
        };

        TrashCounter.OnAnyObjectTrashed += (sender, args) =>
        {
            PlaySound(_audioClipRefsSO.trash, (sender as TrashCounter).transform.position);
        };
    }

    public void PlayFootstepSound(Vector3 position, float volume = 1f)
    {
        PlaySound(_audioClipRefsSO.footstep, position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultipler = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultipler * _volume);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volumeMultipler = 1f)
    {
        PlaySound(audioClips[Random.Range(0, audioClips.Length - 1)], position, volumeMultipler * _volume);
    }

    public void ChangeVolume()
    {
        _volume += 0.1f;
        Debug.Log("Sound effect before: " + _volume);
        if (_volume > 1.1f)
        {
            _volume = 0f;
        }

        Debug.Log("Sound effect after: " + _volume);

        PlayerPrefs.SetFloat(PLAYER_PREF_SOUND_EFFECT_VOLUME, _volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return _volume;
    }
}
