using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dweiss;


public class CanManager : MonoBehaviour
{
    [SerializeField] private GameObject _canPrefab = null;
    [SerializeField] private Vector2 _spawnFrequency = Vector2.one;
    [SerializeField] private Transform _leftTopSpawnCorner = null;
    [SerializeField] private Transform _rightBotSpawnCorner = null;
    [SerializeField] private float _shootStrength = 50f;
    [SerializeField] GameObject _wer;

    private float _timer = 0;
    private float _nextSpawnTime = 0;

    void Awake()
    {
        _nextSpawnTime = Random.Range(_spawnFrequency.x, _spawnFrequency.y);
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _nextSpawnTime)
        {
            Spawn();
            _nextSpawnTime = Random.Range(_spawnFrequency.x, _spawnFrequency.y);
            _timer = 0;
        }
    }

    void Spawn()
    {
        //Spawn Position
        int rand1 = Random.Range(0, 2);
        int rand2 = Random.Range(0, 2);
        Vector3 spawnPos;
        spawnPos.x = rand1 == 0 ? rand2 == 0 ?_leftTopSpawnCorner.position.x : _rightBotSpawnCorner.position.x : 
            Random.Range(_leftTopSpawnCorner.position.x, _rightBotSpawnCorner.position.x);
        spawnPos.z = rand1 == 0 ? Random.Range(_leftTopSpawnCorner.position.z, _rightBotSpawnCorner.position.z) : 
            rand2 == 0 ? _leftTopSpawnCorner.position.z : _rightBotSpawnCorner.position.z;
        spawnPos.y = _leftTopSpawnCorner.position.y;

        //Spawn
        GameObject can = Instantiate(_canPrefab, spawnPos, Quaternion.identity);

        //Direction
        Vector3 dirToMiddle = new Vector3(-spawnPos.x, spawnPos.y, -spawnPos.z);
        Vector3 direction = Quaternion.AngleAxis(Random.Range(-45.0f, 45.0f), Vector3.forward) * dirToMiddle; ;

        //Rotation
        can.transform.LookAt(direction);
        can.transform.rotation *= Quaternion.Euler(0, 90, 0);

        //Shoot
        can.GetComponent<Rigidbody>().AddForce(direction.normalized * _shootStrength);
        Vector3[] _spawnPositions = can.GetComponent<Rigidbody>().CalculateMovement(100, 0.1f, Vector3.zero, direction.normalized * _shootStrength);
        Vector3 _landingPos = Vector3.zero;
        for (int i=0; i < _spawnPositions.Length; i++)
        {
            if (_spawnPositions[i].y <= 0.1f)
            {
                _landingPos = _spawnPositions[i];
                break;
            }
        }

        Instantiate(_wer, _landingPos, Quaternion.identity);
    }
}
