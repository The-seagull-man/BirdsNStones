using UnityEngine;

public class partical : MonoBehaviour
{
    private GameObject _player;
    private ParticleSystem _particleSystem;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Play();
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate()
    {
        if (Vector3.Distance(this.gameObject.transform.position, _player.gameObject.transform.position) > 40)
        {
            Destroy(this.gameObject);
        }
    }
}
