using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 locationOffset;
    public Vector3 rotationOffset;
    public float _zoomOffSet;
    public float _zoomMultiplier;
    public float _minY;
    public float _maxY;

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + /*target.rotation*/ locationOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothedPosition.z = -10;
        transform.position = smoothedPosition;

        Camera.main.orthographicSize = Mathf.Clamp(transform.position.y * _zoomMultiplier + _zoomOffSet, 7.65f, 7.65f * 3f);

        //Quaternion desiredrotation = target.rotation * Quaternion.Euler(rotationOffset);
        //Quaternion smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, smoothSpeed);
        //transform.rotation = smoothedrotation;

        if (transform.position.y > _maxY)
        {
            transform.position = new Vector3 (transform.position.x, _maxY, -10);
        }
        
        if (transform.position.y < _minY)
        {
            transform.position = new Vector3 (transform.position.x, _minY, -10);
        }
    }
}
