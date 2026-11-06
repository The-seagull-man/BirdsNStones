using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject _player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_player.transform.position.x+2,transform.position.y,-10);
    }
}
