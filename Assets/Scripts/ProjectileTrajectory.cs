using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrajectory : MonoBehaviour
{
    [SerializeField] private Transform _shootPosition;

    [SerializeField] private LayerMask _collideLayer;

    [SerializeField] private CannonController _cannonController;

    private LineRenderer _lineRenderer;

    private int _maxPoints = 50;

    private float _pointsSpace = 0.1f;
    private float _shootForce;

    private void Start()
    {
        Initialize();
        SetupDelegates();
    }

    private void OnDestroy()
    {
        RemoveDelegates();
    }

    private void Update()
    {
        DrawLine();
    }

    private void SetupDelegates()
    {
        CannonController.OnStopUsingCannon += DeleteLine;
    }

    private void RemoveDelegates()
    {
        CannonController.OnStopUsingCannon -= DeleteLine;
    }

    private void DrawLine()
    {
        _lineRenderer.positionCount = _maxPoints;
        List<Vector3> points = new List<Vector3>();

        Vector3 startPos = _shootPosition.position;
        Vector3 startVel = _shootPosition.forward * _shootForce;

        for (float i = 0; i < _maxPoints; i += _pointsSpace)
        {
            Vector3 newPoint = startPos + i * startVel;
            newPoint.y = startPos.y + startVel.y * i + Physics.gravity.y / 2 * i * i;
            points.Add(newPoint);

            if (Physics.OverlapSphere(newPoint, 1, _collideLayer).Length > 0)
            {
                _lineRenderer.positionCount = points.Count;
            }
        }
        _lineRenderer.SetPositions(points.ToArray());
    }

    private void DeleteLine()
    {
        _lineRenderer.positionCount = 0;
        enabled = false;
    }

    private void Initialize()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _shootForce = _cannonController.GetShotForce();
    }

}
