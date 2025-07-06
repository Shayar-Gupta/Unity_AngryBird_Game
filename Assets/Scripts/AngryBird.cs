using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBird : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _circleCollider;
    private bool _hasBeenLaunched, _shouldFaceVelocityDirection;

    [SerializeField] private AudioClip _hitClip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _rigidbody.isKinematic = true;
        _circleCollider.enabled = false;
    }

    private void FixedUpdate(){
        if(_hasBeenLaunched && _shouldFaceVelocityDirection) transform.right = _rigidbody.velocity;
    }

    public void LaunchBird(Vector2 direction, float force)
    {
        _rigidbody.isKinematic = false;
        _circleCollider.enabled = true;

        //apply the force
        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);

        _hasBeenLaunched = true;
        _shouldFaceVelocityDirection = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _shouldFaceVelocityDirection = false;
        SoundManager.instance.PlayClip(_hitClip, _audioSource);
        Destroy(this);
    }
}
