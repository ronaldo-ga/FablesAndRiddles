using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputListener : MonoBehaviour
{
    public delegate void PauseHandler();
    public PauseHandler OnPause;

    public delegate void InteractHandler();
    public InteractHandler OnInteract;

    public delegate void ShootHandler();
    public ShootHandler OnShoot;

    [SerializeField] private Vector2 _movement;
    [SerializeField] private Vector2 _look;

    [SerializeField] private bool _jump;
    [SerializeField] private bool _run;
    [SerializeField] private bool _walk;

    private PlayerInputActions _input;


    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        Movement();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnDestroy()
    {
        _input.Player.Jump.performed -= ctx => Jump(ctx);
        _input.Player.Run.performed -= ctx => Run(ctx);
        _input.Player.Run.canceled -= ctx => Run(ctx);
        _input.Player.Pause.performed -= _ => Pause();
        _input.Player.Interact.performed -= _ => Interact();
        _input.Weapons.Shoot.performed -= _ => Shoot();
    }

    private void Initialize()
    {
        _input = new PlayerInputActions();
        _input.Player.Jump.performed += ctx => Jump(ctx);
        _input.Player.Run.performed += ctx => Run(ctx);
        _input.Player.Run.canceled += ctx => Run(ctx);
        _input.Player.Pause.performed += _ => Pause();
        _input.Player.Interact.performed += _ => Interact();
        _input.Weapons.Shoot.performed += _ => Shoot();
    }

    private void Movement()
    {
        _movement = _input.Player.Movement.ReadValue<Vector2>();
        if (_movement.y != 0)
        {
            _walk = true;
        }
        else
        {
            _walk = false;
        }
    }


    private void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _jump = true;
        }
    }

    private void Run(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _run = true;
        }
        else if (ctx.canceled)
        {
            _walk = true;
            _run = false;
        }
    }

    private void Pause()
    {
        OnPause?.Invoke();
    }

    private void Interact()
    {
        OnInteract?.Invoke();
    }

    private void Shoot()
    {
        OnShoot?.Invoke();
    }
}