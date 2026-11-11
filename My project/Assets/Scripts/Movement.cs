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
    [SerializeField] private float _gravityScalePressed;
    [SerializeField] private float _gravityScaleReleased;
    private quaternion _currentRotation;
    private ParticleSystem _Particles;
    private Animator _animator;
    private LineRenderer _lr;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _layerMask = LayerMask.GetMask("Ground");
        _Particles = GetComponentInChildren<ParticleSystem>();
        _animator = GetComponentInChildren<Animator>();
        _lr = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1, _layerMask);
        if (hit && _groundCount > 0)
        {
            _currentRotation = Quaternion.FromToRotation(Vector2.up, hit.normal);
            transform.rotation = _currentRotation;

            if (_rb.linearVelocity.magnitude < 0.01f)
            {
                _Particles.Play();
            }
        }
        else
        {
            _Particles.Pause();
        }

        if (_moving.action.IsPressed())
        {
            _rb.gravityScale = _gravityScalePressed;

            if (_groundCount > 0)
            {

                if (_rb.linearVelocity.magnitude < 0.01f)
                {
                    _rb.linearVelocityX = _speedX;

                }
                else
                {
                    _rb.linearVelocity = transform.right * _speedX;
                }

            }
            else
            {
                _rb.linearVelocityY -= _speedY;

            }
        }
        else
        {
            _rb.gravityScale = _gravityScaleReleased;
        }
        if (_rb.linearVelocityX > 0)
        {
            _animator.SetBool("MovingRight",true);
            _animator.speed = _rb.linearVelocity.magnitude / 10;   
        }
        else
        {
            _animator.SetBool("MovingRight", false);
            _animator.speed = _rb.linearVelocity.magnitude / 10;
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
}
