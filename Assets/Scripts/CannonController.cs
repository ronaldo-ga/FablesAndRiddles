using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using DG.Tweening;
using StarterAssets;
using System.Threading.Tasks;

public class CannonController : MonoBehaviour
{
    public delegate void StartUsingCannonHandler();
    public static StartUsingCannonHandler OnStartUsingCannon;

    public delegate void StopUsingCannonHandler();
    public static StopUsingCannonHandler OnStopUsingCannon;

    public delegate void CannonShotHandler();
    public static CannonShotHandler OnCannonShot;

    public delegate void TriggerWithCannonHandler(bool isTrigger);
    public static TriggerWithCannonHandler OnTriggerWithCannon;

    [SerializeField] private CannonBalancer _cannonBalancer;

    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private GameObject _cannonObject;
    [SerializeField] private GameObject _ammoObject;
    [SerializeField] private GameObject _playerObject;

    [SerializeField] private Transform _shootPosition;
    [SerializeField] private Vector3 _playerShootPosition;

    [SerializeField] private ProjectileTrajectory _projectileTrajectory;
    //[SerializeField] private CameraController _camController;

    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private InputListener _input;
    [SerializeField] private CinemachineDollyCart _dollyCart;
    [SerializeField] private ThirdPersonController _playerController;
    [SerializeField] private Animator _playerAnimator;

    //private ObjectPooler _objectPooler;

    private bool _isUsingCannon = false;
    private bool _isTriggerWithPlayer = false;
    private bool _canShoot = true;

    private float _ammo;
    private float _startRotationY;

    private void Start()
    {
        SetupDelegates();
        Initialize();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void Initialize()
    {
        _camera = _camera.GetComponent<CinemachineVirtualCamera>();
        _startRotationY = _cannonObject.transform.eulerAngles.y;
    }

    private void SetupDelegates()
    {
        _input.OnInteract += UseCannon;
        _input.OnShoot += Shoot;
        //_camController.OnCameraRotate += Movement;
    }

    private void RemoveDelegates()
    {
        _input.OnInteract -= UseCannon;
        _input.OnShoot -= Shoot;
        //_camController.OnCameraRotate -= Movement;
    }

    private void UseCannon()
    {
        if (_isTriggerWithPlayer)
        {
            _dollyCart.m_Position = 0;
            nearFinish = false;
            OnStartUsingCannon?.Invoke();
            StartCoroutine(CannonUseDelay());
            _playerObject.transform.SetParent(_shootPosition.gameObject.transform);
            _playerController.enabled = false;
            ShootAnimation();
        }
    }

    private async void ShootAnimation()
    {
        _playerObject.transform.DOLocalMove(_playerShootPosition, 0);
        _playerObject.transform.DOLocalRotate(new Vector3(70, 0, 0), 0);
        _camera.Priority = 15;
        await Task.Delay(800);
        _playerAnimator.SetTrigger("Fly");
        _shootPosition.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine);
        DOVirtual.Float(0, 1, 3, PathPosition);
        await Task.Delay(2200);
        //_playerObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
    }

    bool nearFinish = false;

    private void PathPosition(float pos)
    {
        _dollyCart.m_Position = pos;

        if (pos > 0.9f && !nearFinish)
        {
            _playerAnimator.SetTrigger("Land");
            nearFinish = true;
            _playerObject.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
            //_shootPosition.transform.DORotate(new Vector3(360, 0, 0), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine);
        }

        if (pos == 1)
        {
            _playerAnimator.SetTrigger("Land");
            _playerObject.transform.SetParent(null);
            _playerController.enabled = true;
            _camera.Priority = 0;
        }
    }

    private IEnumerator CannonUseDelay()
    {
        yield return new WaitForSeconds(_cannonBalancer.startDelay);
        _projectileTrajectory.enabled = true;
        _isUsingCannon = true;
    }

    private void Movement(float yRot, float xRot)
    {
        if (_isUsingCannon)
        {
            _cannonObject.transform.eulerAngles = new Vector3(xRot, yRot + _startRotationY, 0);
        }
    }
    //TODO MOVENDO NO PAUSE
    private void Shoot()
    {
        if (_isUsingCannon && _canShoot && _ammo > 0)
        {
            _canShoot = false;

            _shootParticle.Play();

            OnCannonShot?.Invoke();
            StartCoroutine(ShootDelay());

            GameObject obj = Instantiate(_ammoObject);

            obj.transform.eulerAngles = _cannonObject.transform.eulerAngles;
            obj.transform.position = _shootPosition.position;
            obj.GetComponent<CannonBall>().SetShootAttributes(_cannonBalancer.blastRadius, _cannonBalancer.damage);

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.velocity = _shootPosition.transform.forward * _cannonBalancer.shootForce;
        }
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(_cannonBalancer.shootDelay);
        _canShoot = true;
    }

    public float GetShotForce()
    {
        return _cannonBalancer.shootForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        _isTriggerWithPlayer = true;
        OnTriggerWithCannon?.Invoke(true);
    }

    private void OnTriggerExit(Collider other)
    {
        _isTriggerWithPlayer = false;
        OnTriggerWithCannon?.Invoke(false);
    }

}
