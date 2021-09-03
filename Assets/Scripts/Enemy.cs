using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum EnemyAIStance { Idle, Run, Attack}
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private EnemyAIStance _aiStance;
    private float _scale;
    [SerializeField] private Vector2 _destination;

    private Animator _animator;
    private Collider2D _collider;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        //_aiStance = EnemyAIStance.Idle;
        _destination = Util.GenerateRandomCoordinatesInVieport();
        _scale = transform.localScale.x;
        StartCoroutine("Idle");
        StartCoroutine("ChangeDestination");
    }

    void Update()
    {
        if (_aiStance == EnemyAIStance.Run)
        {
            Run();
        }
    }

    private void Run()
    {
        //_aiStance = EnemyAIStance.Run;
        //_animator.SetBool("isRunning", true);

        transform.position = Vector2.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
        transform.localScale = _destination.x > transform.position.x ? new Vector3(-_scale, transform.localScale.y, transform.localScale.z) : new Vector3(_scale, transform.localScale.y, transform.localScale.z);
    }

    private IEnumerator Idle()
    {
        _aiStance = EnemyAIStance.Idle;
        _animator.SetBool("isIdle", true);
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        _aiStance = EnemyAIStance.Run;
        _animator.SetBool("isRunning", true);
    }

    private IEnumerator ChangeDestination()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1.6f, 3.5f));
            StartCoroutine("Idle");
            _destination = Util.GenerateRandomCoordinatesInVieport();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopCoroutine("Idle");
            _aiStance = EnemyAIStance.Attack;
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isAttacking", true);
            StartCoroutine("Idle");
        }
    }
}
