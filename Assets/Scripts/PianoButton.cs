using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PianoButton : MonoBehaviour
{
    [SerializeField] private GameObject _button;
    [SerializeField] private AudioClip _audio;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTER");
        _button.transform.DOLocalMoveY(0, 0.2f);
        _audioSource.clip = _audio;
        _audioSource.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        _button.transform.DOLocalMoveY(0.05f, 0.2f);
    }
}
