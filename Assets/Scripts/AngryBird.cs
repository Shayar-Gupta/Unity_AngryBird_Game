using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBird : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private CircleCollider2D _circleCollider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        _rigidbody.isKinematic = true;
        _circleCollider.enabled = false;
    }

    public void LaunchBird(Vector2 direction, float force)
    {
        _rigidbody.isKinematic = false;
        _circleCollider.enabled = true;

        //apply the force
        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
