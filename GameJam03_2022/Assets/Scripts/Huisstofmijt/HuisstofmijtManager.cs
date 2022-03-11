using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuisstofmijtManager : MonoBehaviour
{
    [SerializeField] private GameObject _huisstofmijtPrefab;
    [SerializeField] private Transform _leftSpawnCorner = null;
    [SerializeField] private Transform _rightSpawnCorner = null;
    [SerializeField] private int _amount = 3;

    void Awake()
    {
        for (int i=0; i < _amount; i++)
        {
            Vector3 dustLocation = new Vector3(Random.Range(_leftSpawnCorner.position.x, _rightSpawnCorner.position.x), 0f, Random.Range(_leftSpawnCorner.position.z, _rightSpawnCorner.position.z));
            PhotonNetwork.Instantiate(_huisstofmijtPrefab.name, dustLocation, Quaternion.identity);
        }
    }
    
}
