using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuGO;
    [SerializeField] private Transform _hpBarGO;
    [SerializeField] private GameObject _healthIconPrefab;
    [SerializeField] private Transform _envGO;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _fruitPrefab;
    [SerializeField] private List<Sprite> _listOfFruitSprites;

    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _curHealth;
    [SerializeField] private GameObject[] _HealthGO;

    [SerializeField] private float _fruitSpawnTimeBaseValue = 3;
    [SerializeField] private int _amountOfFruits;
    [SerializeField] private int _amountOfEnemies = 3;

    private int _curGameScore;
    [SerializeField] private GameObject _curScoreGO, _recordScoreGO;

    private Player _player;

    [SerializeField] private List<Fruit> _fruits = new List<Fruit>();
    private List<Enemy> _enemies = new List<Enemy>();

    [ContextMenu("Game Init")]
    public void GameInit()
    {
        _mainMenuGO.SetActive(false);
        _curGameScore = 0;
        _curScoreGO.GetComponent<Text>().text = _curGameScore.ToString();
        HealthInit();
        PlayerInit();
        EnemiesInit();
        StartCoroutine("InitFruitSpawn");
    }

    private void HealthInit()
    {
        _curHealth = _maxHealth;
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
            Vector2 spawnPosition = Util.GenerateRandomCoordinatesInVieport();
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
                Vector2 spawnPosition = Util.GenerateRandomCoordinatesInVieport();
                Fruit fruit = Instantiate(_fruitPrefab, spawnPosition, Quaternion.identity, _envGO).GetComponent<Fruit>();
                fruit.SetupFruit(_listOfFruitSprites[Random.Range(0, _listOfFruitSprites.Count)]);
                _fruits.Add(fruit);
            }
            yield return new WaitForSeconds(_fruitSpawnTimeBaseValue * Random.Range(0.75f, 1.25f));
        }
    }

    private void EnemyHit()
    {
        Destroy(_HealthGO[_curHealth - 1].gameObject);
        _HealthGO[_curHealth - 1] = null;
        _curHealth--;
        if (_curHealth == 0)
        {
            EndGame();
        }
    }

    private void FruitHit(Fruit fruit)
    {
        _curGameScore++;
        _curScoreGO.GetComponent<Text>().text = _curGameScore.ToString();
        _fruits.Remove(fruit);
        Destroy(fruit.gameObject);
    }

    private void EndGame()
    {
        StopAllCoroutines();
        _mainMenuGO.SetActive(true);
        Destroy(_player.gameObject);
        foreach (Fruit fruit in _fruits)
        {
            if (fruit != null)
            {
                Destroy(fruit.gameObject);
            }  
        }
        _fruits.Clear();
        foreach (Enemy enemy in _enemies)
        {
            Destroy(enemy.gameObject);
        }
        _enemies.Clear();
        _curGameScore = 0;
    }
}
