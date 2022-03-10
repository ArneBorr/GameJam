using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerName : MonoBehaviour
{
    TextMeshPro _playerName = null;
    PhotonView _view = null;

    private void Start()
    {
        _playerName = GetComponentInChildren<TextMeshPro>();
        _view = GetComponent<PhotonView>();

        if (_view.IsMine)
        {
            _playerName.text = PhotonNetwork.NickName;
        }
        else 
        {
            _playerName.text = _view.Owner.NickName;
        }
    }
}
