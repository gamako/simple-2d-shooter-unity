using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

    private AudioSource audioSource;
    
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip bomnbClip;
    [SerializeField] private AudioClip itemShotClip;
    [SerializeField] private AudioClip powerUp1Clip;
    [SerializeField] private AudioClip powerUp2Clip;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update () {
    }

    public void playBomb() {
        audioSource.PlayOneShot(bomnbClip);
    }
    public void playShot() {
        audioSource.PlayOneShot(shotClip);
    }
    public void playItemShot() {
        audioSource.PlayOneShot(itemShotClip);
    }
    public void playPowerUp1() {
        audioSource.PlayOneShot(powerUp1Clip);
    }
    public void playPowerUp2() {
        audioSource.PlayOneShot(powerUp2Clip);
    }
}
