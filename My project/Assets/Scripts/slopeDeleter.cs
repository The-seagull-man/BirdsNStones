using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class slopeDeleter : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] SpriteShapeRenderer _shapeRenderer;
    [SerializeField] GameObject _player;
    [SerializeField] int _offSetLeftOfPlayer;

    private void Start()
    {
        _camera = Camera.main;
        _shapeRenderer = GetComponent<SpriteShapeRenderer>();
        _player = GameObject.FindWithTag("Player");
    }

    private void LateUpdate()
    {
        if (this.transform.position.x < _player.transform.position.x + _offSetLeftOfPlayer && !_shapeRenderer.isVisible)
        {
            //Debug.Log($" Delete {this.gameObject}");
            Destroy(this.gameObject);
        }
    }
}