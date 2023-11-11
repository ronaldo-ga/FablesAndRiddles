using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using Cinemachine;

public class Car : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _enterPosition;
    [SerializeField] private Transform _insidePosition;
    [SerializeField] private Animator _playerAnim;
    [SerializeField] private CinemachineVirtualCamera _carCam;
    [SerializeField] private float _enterDistance;

    private bool _insideCar = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float dist = Vector3.Distance(_player.transform.position, _enterPosition.position);

            if (dist > _enterDistance)
            {
                return;
            }

            if (_insideCar)
            {
                ExitCar();
            }
            else
            {
                EnterCar();
            }
        }
    }

    private async void EnterCar()
    {
        _player.transform.DOMove(_enterPosition.position, 0.3f);
        _player.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.3f, RotateMode.FastBeyond360);

        await Task.Delay(300);

        _carCam.Priority = 2;
        _playerAnim.SetTrigger("EnterCar");
        await Task.Delay(1400);
        _door.transform.DORotate(new Vector3(0, 150, 0), 1f);

        await Task.Delay(2300);

        _door.transform.DORotate(new Vector3(0, 90, 0), 1f);

        _insideCar = true;
    }

    private async void ExitCar()
    {
        _playerAnim.SetTrigger("ExitCar");
        _carCam.Priority = 0;
        _player.transform.DOLocalRotate(new Vector3(0, 270, 0), 0);
        await Task.Delay(700);
        _door.transform.DORotate(new Vector3(0, 150, 0), 1f);

        await Task.Delay(2700);

        _door.transform.DORotate(new Vector3(0, 90, 0), 1f);
        _carCam.Priority = 0;
        _insideCar = true;
    }
}
