using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Cinemachine;

public class CameraObject : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _playerAnim;
    [SerializeField] private InputListener _input;
    [SerializeField] private Transform _playerRotation;
    [SerializeField] private GameObject _animationPlayer;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _endPos;
    [SerializeField] private CinemachineVirtualCamera _pushCamera;

    private bool _isTriggerWithPlayer = false;

    private void Start()
    {
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void SetupDelegates()
    {
        _input.OnInteract += PushCamera;
    }

    private void RemoveDelegates()
    {
        _input.OnInteract -= PushCamera;
    }

    private async void PushCamera()
    {
        if (_isTriggerWithPlayer)
        {
            _pushCamera.Priority = 10;
            //_playerRotation.transform.DOLocalMove(_startPos, 0.1f);
            _playerRotation.transform.DOLocalRotate(new Vector3(0, -167.65f, 0), 0.1f);

            await Task.Delay(100);
            _animationPlayer.SetActive(true);
            _playerRotation.gameObject.SetActive(false);
            _playerRotation.transform.localPosition = _endPos;
            _playerAnim.SetTrigger("PushStart");
            _anim.SetTrigger("Play");
            await Task.Delay(1000);
            _playerAnim.SetTrigger("PushEnd");
            await Task.Delay(3000);
            _animationPlayer.SetActive(false);
            _playerRotation.gameObject.SetActive(true);
            _playerRotation.transform.localPosition = _endPos;
            await Task.Delay(1000);

            _playerRotation.transform.localPosition = _endPos;
            _pushCamera.Priority = 0;

            _isTriggerWithPlayer = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _isTriggerWithPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isTriggerWithPlayer = false;
    }

}
