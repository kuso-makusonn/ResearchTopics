using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("効果音")]
    public AudioClip attack;
    public AudioClip damage;

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
    }

    public void AttackSound(){
        audioSource.PlayOneShot(attack);
    }
    public void DamageSound(){
        audioSource.PlayOneShot(damage);
    }
    
}
