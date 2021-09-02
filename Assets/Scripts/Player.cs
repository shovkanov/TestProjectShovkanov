using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    private float _scale;
    
    private Animator _animator;
    private Collider2D _collider;

    public event Action onEnemyHit;
    public event Action<Fruit> onFruitHit;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Move();
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
            
    }

    public void Setup()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _scale = transform.localScale.x;
    }

    private void Move()
    {
        _animator.SetBool("isRunning", true);
        Vector3 cursorPos = Input.mousePosition;
        Vector3 viewport = Camera.main.ScreenToViewportPoint(cursorPos);

        if (viewport.x < 0 || viewport.y < 0 || viewport.x > 1 || viewport.y > 1) return;

        Vector2 destination = Camera.main.ScreenToWorldPoint(cursorPos);
        float distance = Vector3.Distance(transform.position, destination);

        transform.position = Vector2.MoveTowards(transform.position, destination, _speed * Time.deltaTime);
        transform.localScale = destination.x > transform.position.x ? new Vector3(_scale, transform.localScale.y, transform.localScale.z) : new Vector3(-_scale, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            onEnemyHit?.Invoke();
        }

        if (collision.gameObject.tag == "Fruit")
        {
            onFruitHit?.Invoke(collision.gameObject.GetComponent<Fruit>());
        }
    }
}
