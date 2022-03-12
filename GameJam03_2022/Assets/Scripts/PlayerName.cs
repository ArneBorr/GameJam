using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerName : MonoBehaviour
{
    [SerializeField] private GameObject _uiPrefab;

    GameObject _listingElement;
    List<TextMeshProUGUI> _textScores = new List<TextMeshProUGUI>();
    TextMeshPro _playerName = null;
    PhotonView _view = null;

    private void Start()
    {
        _playerName = GetComponentInChildren<TextMeshPro>();
        _view = GetComponent<PhotonView>();
        _listingElement = GameObject.Find("ScoreListing");

        if (_view.IsMine)
        {
            _playerName.text = PhotonNetwork.NickName;
        }
        else
        {
            _playerName.text = _view.Owner.NickName;
        }
    }

    private void Update()
    {
        if (_view.IsMine) 
        {
            UpdateScoreTexts();
            UpdateScores();
        }
    }

    private void UpdateScoreTexts()
    {
        int playerCount = PhotonNetwork.CountOfPlayers - 1;

        while (_textScores.Count < playerCount)
        {
            GameObject ui = Instantiate(_uiPrefab);
            ui.transform.parent = _listingElement.transform;
            _textScores.Add(ui.GetComponent<TextMeshProUGUI>());
        }

        while (_textScores.Count > playerCount)
        {
            GameObject obj = _textScores[_textScores.Count - 1].gameObject;
            _textScores.RemoveAt(_textScores.Count - 1);
            Destroy(obj);
        }
    }

    private void UpdateScores() 
    {
        int i = 0;
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if (player != PhotonNetwork.LocalPlayer)
            {
                _textScores[i].text = 1.ToString();

                i++;
            }
        }
    }
}