using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] private Transform _hpBarGO;
    [SerializeField] private GameObject _healthIconPrefab;
    [SerializeField] private Transform _envGO;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _fruitPrefab;
    [SerializeField] private List<Sprite> _listOfFruitSprites;

    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _curHealth;
    private GameObject[] _HealthGO;

    [SerializeField] private float _fruitSpawnTimeBaseValue = 3;
    [SerializeField] private int _amountOfFruits;
    [SerializeField] private int _amountOfEnemies = 3;

    private int _curGameScore;

    private Player _player;

    [SerializeField] private List<Fruit> _fruits = new List<Fruit>();
    private List<Enemy> _enemies = new List<Enemy>();

    [ContextMenu("Game Init")]
    public void GameInit()
    {
        HealthInit();
        PlayerInit();
        EnemiesInit();
        StartCoroutine("InitFruitSpawn");
    }

    private void HealthInit()
    {
        _HealthGO = new GameObject[_maxHealth];
        for (int i = 0; i < _maxHealth; i++)
        {
            _HealthGO[i] = Instantiate(_healthIconPrefab, _hpBarGO);
        }
    }

    private void PlayerInit()
    {
        _player = Instantiate(_playerPrefab, _envGO).GetComponent<Player>();
        _player.Setup();
        _player.onEnemyHit += EnemyHit;
        _player.onFruitHit += FruitHit;
    }

    private void EnemiesInit()
    {
        for (int i = 0; i < _amountOfEnemies; i++)
        {
            float spawnY = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            Vector2 spawnPosition = new Vector2(spawnX, spawnY);
            Enemy enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity, _envGO).GetComponent<Enemy>();

            _enemies.Add(enemy);
        }
    }

    private IEnumerator InitFruitSpawn()
    {
        while (true)
        {
            if (_fruits.Count < _amountOfFruits)
            {
                float spawnY = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
                float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

                Vector2 spawnPosition = new Vector2(spawnX, spawnY);
                Fruit fruit = Instantiate(_fruitPrefab, spawnPosition, Quaternion.identity, _envGO).GetComponent<Fruit>();
                fruit.SetupFruit(_listOfFruitSprites[Random.Range(0, _listOfFruitSprites.Count)]);
                _fruits.Add(fruit);
            }
            yield return new WaitForSeconds(_fruitSpawnTimeBaseValue * Random.Range(0.75f, 1.25f));
        }
    }

    private void EnemyHit()
    {
        Destroy(_HealthGO[_curHealth].gameObject);
        _HealthGO[_curHealth] = null;
        _curHealth--;
        if (_curHealth == 0)
        {

        }
    }

    private void FruitHit(Fruit fruit)
    {
        _curGameScore++;
        _fruits.Remove(fruit);
        Destroy(fruit.gameObject);
    }
}
