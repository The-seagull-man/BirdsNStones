using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 _parallaxEffectMultiplier;
    [SerializeField] private bool _infiniteHorizontal;
    [SerializeField] private bool _infiniteVertical;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private float _textureUnitySizeX;
    private float _textureUnitySizeY;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        _textureUnitySizeX = texture.width / sprite.pixelsPerUnit;
        _textureUnitySizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * _parallaxEffectMultiplier.x, deltaMovement.y * _parallaxEffectMultiplier.y);
        _lastCameraPosition = _cameraTransform.position;

        if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitySizeX && _infiniteHorizontal)
        {
            float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitySizeX;
            transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
        }

        if (Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureUnitySizeY && _infiniteVertical)
        {
            float offsetPositionY = (_cameraTransform.position.y - transform.position.y) % _textureUnitySizeY;
            transform.position = new Vector3(transform.position.x, transform.position.y + offsetPositionY);
        }
    }
}