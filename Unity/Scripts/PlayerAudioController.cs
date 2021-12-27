using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    public AudioClip gunClip;
    public AudioClip getHurtClip;
    public AudioClip gameOverClip;
    public AudioClip fireClip;
    public AudioClip shieldClip;
    public AudioClip bulletHitClip;


    public AudioSource audioSource;

    public void PlayGunShot()
    {
        audioSource.clip = gunClip;
        audioSource.Play();
    }
    public void PlayGetHurt()
    {
        audioSource.clip = getHurtClip;
        audioSource.Play();
    }

    public void PlayGameOver()
    {
        audioSource.clip = gameOverClip;
        audioSource.Play();
    }

    public void PlayFire()
    {
        audioSource.clip = fireClip;
        audioSource.Play();
    }
    public void PlayShield()
    {
        audioSource.clip = shieldClip;
        audioSource.Play();
    }
    public void PlayBulletHit()
    {
        audioSource.clip = bulletHitClip;
        audioSource.Play();
    }
}
