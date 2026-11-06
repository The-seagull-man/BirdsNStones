using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody2D _rb;
    [SerializeField] private InputActionReference _moving;
    [SerializeField] private float _speedX;
    [SerializeField] private float _speedY;
    LayerMask _layerMask;
    [SerializeField] private bool _onGround;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _layerMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        if (_moving.action.IsPressed())
        {
            if (_onGround)
            {
                _rb.linearVelocityX = _speedX;

            }
            else
            {
                _rb.rotation = -33;
            }
        }
       /* RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10,_layerMask);
        Debug.DrawRay(transform.position,Vector2.down, Color.red,10);

        transform.rotation = Quaternion.LookRotation(Vector2.up);
        if (hit)
        {
            Debug.Log(hit.normal);
            transform.rotation = Quaternion.FromToRotation(hit.normal, transform.TransformDirection(Vector2.up));
        }
       */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            _onGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            _onGround = false;
        }
    }
}
