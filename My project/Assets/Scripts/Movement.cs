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
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _layerMask = LayerMask.GetMask("Ground");
        _Particles = GetComponentInChildren<ParticleSystem>();
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1, _layerMask);
        if (hit && _groundCount > 0)
        {
            _currentRotation = Quaternion.FromToRotation(Vector2.up, hit.normal);
            transform.rotation = _currentRotation;
            _Particles.Play();
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
         
        //Debug.DrawRay(transform.position, Vector2.down,Color.darkOliveGreen, 1);

        

    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            _groundCount++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            _groundCount--;

        }
    }
}
