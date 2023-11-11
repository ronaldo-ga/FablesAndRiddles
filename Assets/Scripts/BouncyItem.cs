using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class BouncyItem : MonoBehaviour
{
    [SerializeField] private ThirdPersonController _player;

    public float forceMagnitude = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        _player.Bouncy();
    }
}
