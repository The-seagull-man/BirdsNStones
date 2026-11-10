using System;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TrejectoryPrediction : MonoBehaviour
{
    [SerializeField] private float _power = 5f;
    Rigidbody2D _rigidbody2D;
    LineRenderer _lineRenderer;
    [SerializeField] InputAction _inputAction;
    Vector2 _dragStartPosition;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _dragStartPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 _dragEndPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 _velocity = (_dragEndPosition - _dragStartPosition) * _power;

            Vector2[] trajectory = Plot(_rigidbody2D, (Vector2)transform.position, _velocity, 500);

            _lineRenderer.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }
            _lineRenderer.SetPositions(positions);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Vector2 _dragEndPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 _velocity = (_dragEndPosition - _dragStartPosition) * _power;

            _rigidbody2D.linearVelocity = _velocity;
        }
    }

    public Vector2[] Plot(Rigidbody2D rigidbody, Vector2 position, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAcceleration = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.linearDamping;
        Vector2 moveStep = velocity * timestep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAcceleration;
            moveStep *= drag;
            position += moveStep;
            results[i] = position;
        }

        return results;
    }
}
