using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Movement : MonoBehaviour
{
    private bool _madeLine = false;
    public GameObject _Bird;
    public GameObject leaderboard;
    public Material _shader;
    Rigidbody2D _rb;
    [SerializeField] private InputActionReference _moving;
    [SerializeField] private float _speedX;
    [SerializeField] private float _speedY;
    private LayerMask _layerMask;
    [SerializeField] private int _groundCount;
    private float _timeOnGroundPressed;
    [SerializeField] private float _timeInAirPressed;
    private float _lastVelocity;
    [SerializeField] private float _gravityScalePressed;
    [SerializeField] private float _gravityScaleReleased;
    private quaternion _currentRotation;
    private ParticleSystem _Particles;
    [SerializeField] private GameObject _spriteObj;
    private float _elapsedTime;
    private LineRenderer _lr;
     private bool _playerStart;
    [SerializeField] private GameObject _deathText;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _layerMask = LayerMask.GetMask("Ground");
        _Particles = GetComponentInChildren<ParticleSystem>();
        _lr = GetComponent<LineRenderer>();
        _Particles.Pause();
        leaderboard.SetActive(true);
        _playerStart = false;
    }

    void FixedUpdate()
    {
        _shader.SetVector("_Visibility", new Vector4(Mathf.Clamp((_rb.linearVelocity.magnitude / 100f), 0f, 1f), 0, 0, 0));

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
            _playerStart = true;

            leaderboard.SetActive(false);

            _rb.gravityScale = _gravityScalePressed;

            if (_groundCount > 0)
            {
                _timeOnGroundPressed += Time.fixedDeltaTime;
                if (_rb.linearVelocity.magnitude < 0.01f)
                {
                    _rb.linearVelocityX = 5;

                }
                else
                {
                    _lastVelocity =_rb.linearVelocity.magnitude;
                   _rb.linearVelocity = (transform.right * _speedX + transform.right * _lastVelocity / 3 * (_timeOnGroundPressed + 0.3f) * (_timeInAirPressed + 0.3f));
                    //Debug.Log(_rb.linearVelocity.magnitude);

                    if (_rb.linearVelocity.magnitude >= 100f)
                    {
                        _rb.linearVelocity *= 0.99f;
                    }
                }

            }
            else
            {
                _rb.linearVelocityY -= _speedY;
                _timeInAirPressed += Time.fixedDeltaTime;
                _timeOnGroundPressed = 0;
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

            _madeLine = false;
        }

        if(collision.gameObject.layer == 6 && _rb.linearVelocityY > 1f )
        {
            if (_madeLine)
                return;
            _rb.linearVelocityY += _timeInAirPressed + _timeOnGroundPressed;

            Vector2[] trajectory = Plot(_rb, (Vector2)transform.position, _rb.linearVelocity, 2000);

            _lr.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }
            _lr.SetPositions(positions);

            _madeLine = true;

            for (int i = 0; i < _lr.positionCount; i += 100)
            {
                Instantiate(_Bird, _lr.GetPosition(i), quaternion.identity);
            }

            _timeInAirPressed = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            _groundCount--;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            Debug.Log(Vector2.Angle(_rb.linearVelocity, collision.contacts[0].normal));
            if (Vector2.Angle(_rb.linearVelocity, collision.contacts[0].normal) < 60 && _playerStart == true)
            {
                _moving.action.Disable();
                _deathText.SetActive(true);
            }
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
