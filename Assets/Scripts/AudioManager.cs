using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource click;
    [SerializeField] private AudioSource megaClickIn;
    [SerializeField] private AudioSource megaClickOut;
    [SerializeField] private AudioSource buy;
    [SerializeField] private AudioSource riding;
    [SerializeField] private AudioSource coin;
    [SerializeField] private AudioSource drift;
    [SerializeField] private AudioSource shot;
    public bool IsRidingPlaying() => riding != null && riding.isPlaying;
    public void SetPitch(float pitch) => riding.pitch = pitch;


    public void PlayClick()
    {
        click.Play();
    }

    public void PlayMegaClickIn()
    {
        megaClickIn.Play();
    }

    public void PlayMegaClickOut()
    {
        megaClickOut.Play();
    }

    public void PlayBuying()
    {
        buy.Play();
    }

    public void PlayCoin()
    {
        coin.Play();
    }

    public void StopRiding()
    {
        riding.Stop();
    }
    public void PlayRiding()
    {
        riding.Play();
    }

    public void PlayDrift()
    {
        drift.Play();
    }

    public void StopDrift()
    {
        drift.Stop();
    }

    public void PlayShot()
    {
        shot.Play();
    }
}
