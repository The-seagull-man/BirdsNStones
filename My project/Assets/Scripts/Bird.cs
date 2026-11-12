using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private GameObject Partical;
    private Animator _animator;
    private bool _hit;
    private float _fallSpeed;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
        _fallSpeed = 0;
        _hit = false;
        if (this.gameObject.GetComponentInChildren<SpriteRenderer>().isVisible)
        {
            Destroy(this.gameObject);
        }
        if (this.gameObject.transform.position.y < 8)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _animator.SetBool("Hit", true);
            Instantiate(Partical, this.gameObject.transform);
            _hit = true;
            _fallSpeed = 0.05f;
            ScoreManager.instance._Score++;
            Destroy(this.gameObject.GetComponent<CircleCollider2D>());
        }
    }
    
    private void FixedUpdate()
    {
        if (_hit)
        {
            this.transform.position += new Vector3(0, _fallSpeed, 0);
            _fallSpeed -= 0.025f;
        }

        if (!this.gameObject.GetComponentInChildren<SpriteRenderer>().isVisible && Vector3.Distance(this.gameObject.transform.position, _player.gameObject.transform.position) > 40)
        {
            Destroy(this.gameObject);
        }
    }
    
}