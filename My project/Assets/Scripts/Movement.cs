using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    Rigidbody2D _rb;
    [SerializeField] private InputActionReference _moveing;

    private void OnEnable()
    {
        _moveing.action.started += MoveStone;
    }
    
    private void OnDisable()
    {
        _moveing.action.started -= MoveStone;
    }
    private void MoveStone(InputAction.CallbackContext obj)
    {
        Debug.Log("Moving");
    }
    void Start()
    {
    }

    void Update()
    {
       
    }
}
