using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody2D _rb;
    [SerializeField] private InputActionReference _moving;
    [SerializeField] private float _speedX;
    [SerializeField] private float _speedY;
    private LayerMask _layerMask;
    [SerializeField] private int _groundCount;
    private float _timeOnGround;
    private float _lastVelocity;
    [SerializeField] private float _gravityScalePressed;
    [SerializeField] private float _gravityScaleReleased;
    private quaternion _currentRotation;
    private ParticleSystem _Particles;
    [SerializeField] private GameObject _spriteObj;
    [SerializeField] private float _elapsedTime;
    private LineRenderer _lr;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _layerMask = LayerMask.GetMask("Ground");
        _Particles = GetComponentInChildren<ParticleSystem>();
        _lr = GetComponent<LineRenderer>();
        _Particles.Pause();
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1, _layerMask);
        if (hit && _groundCount > 0)
        {
            _currentRotation = Quaternion.FromToRotation(Vector2.up, hit.normal);
            transform.rotation = _currentRotation;

            if (_rb.linearVelocity.magnitude > 0.01f)
            {
                _Particles.Play();
            }
            else
            {
                _Particles.Clear();
            }
        }
        else
        {
            _Particles.Clear();
        }

        if (_moving.action.IsPressed())
        {
            _rb.gravityScale = _gravityScalePressed;

            if (_groundCount > 0)
            {
                _timeOnGround += Time.fixedDeltaTime;
                if (_rb.linearVelocity.magnitude < 0.01f)
                {
                    _rb.linearVelocityX = 5;

                }
                else
                {
                    _lastVelocity =_rb.linearVelocity.magnitude;
                   _rb.linearVelocity = transform.right*_speedX + transform.right * _lastVelocity/3*_timeOnGround;
                    Debug.Log(_rb.linearVelocity.magnitude);
                }

            }
            else
             {
                _rb.linearVelocityY -= _speedY;
                _timeOnGround =0;
            }
        }
        else
        {
            _rb.gravityScale = _gravityScaleReleased;
        }


        _elapsedTime += Time.deltaTime;

        if (_rb.linearVelocityX > 0)
        {
            if (_elapsedTime >= 0.1f / (_rb.linearVelocity.magnitude + 0.001f))
            {
                _elapsedTime = 0;
                RotateSpriteRight();
            }
        }
        else if (_rb.linearVelocityX < 0)
        {
            if (_elapsedTime >= 0.1f / (_rb.linearVelocity.magnitude + 0.001f))
            {
                _elapsedTime = 0;
                RotateSpriteLeft();
            }
        }

    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            _groundCount++;
        }

        if(collision.gameObject.layer == 6 && _rb.linearVelocityY > 0.01f)
        {
            Vector2[] trajectory = Plot(_rb, (Vector2)transform.position, _rb.linearVelocity, 500);

            _lr.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }
            _lr.SetPositions(positions);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            _groundCount--;

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

    void RotateSpriteRight()
    {
        CancelInvoke("RotateSpriteLeft");
        _spriteObj.transform.Rotate(0, 0, -5);
    }
    void RotateSpriteLeft()
    {
        CancelInvoke("RotateSpriteRight");
        _spriteObj.transform.Rotate(0, 0, 5);
    }
}
