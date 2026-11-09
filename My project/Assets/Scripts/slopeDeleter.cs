using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class slopeDeleter : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] SpriteShapeRenderer _shapeRenderer;
    [SerializeField] GameObject _player;
    [SerializeField] bool _isNotVisible;
    [SerializeField] bool _isLeftOfPlayer;
    [SerializeField] int _offSetLeftOfPlayer;

    private void Start()
    {
        _camera = Camera.main;
        _shapeRenderer = GetComponent<SpriteShapeRenderer>();
        _player = GameObject.FindWithTag("Player");
        _isNotVisible = false;
        _isLeftOfPlayer = false;
    }

    private void LateUpdate()
    {
        if (this.transform.position.x < _player.transform.position.x + _offSetLeftOfPlayer)
        {
            _isLeftOfPlayer = true;
        } else
        {
            _isLeftOfPlayer = false;
        }

        if (!_shapeRenderer.isVisible)
        {
            _isNotVisible = true;
        } else
        {
            _isNotVisible = false;
        }

        if (_isNotVisible && _isLeftOfPlayer)
        {
            //Debug.Log($" Delete {this.gameObject}");
            Destroy(this.gameObject);
        }
    }
}