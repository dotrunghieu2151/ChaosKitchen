using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefSO _audioClipRefsSO;
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
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClips[Random.Range(0, audioClips.Length - 1)], position, volume);
    }
}
