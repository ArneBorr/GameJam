using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    private float x = 4.6f;
    private float z = 4.6f;
    private float y = 0.3f;

    private void Start()
    {
        Vector3 position = new Vector3(Random.Range(-x, x), y, Random.Range(-z, z));
        PhotonNetwork.Instantiate(_playerPrefab.name, position, Quaternion.identity);
    }


}