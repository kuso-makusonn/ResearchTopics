using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("効果音")]
    public AudioClip attack;
    public AudioClip damage;

    public AudioClip bgm;
    public AudioClip dangers;

    AudioSource audioSource;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        BGMPlay(bgm);
    }
    public void Danger(){
        BGMPlay(dangers);
    }
    public void NotDanger(){
        BGMPlay(bgm);
    }
    void BGMPlay(AudioClip newBGM){
        audioSource.Stop();
        audioSource.clip = newBGM;
        audioSource.Play();
    }

    public void AttackSound(){
        audioSource.PlayOneShot(attack);
    }
    public void DamageSound(){
        audioSource.PlayOneShot(damage);
    }
    
}
