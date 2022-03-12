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
        int playerCount = PhotonNetwork.PlayerList.Length - 1;

        while (_textScores.Count < playerCount)
        {
            GameObject ui = Instantiate(_uiPrefab);
            _listingElement.transform.SetParent(ui.transform);
            _textScores.Add(ui.GetComponentInChildren<TextMeshProUGUI>());
        }

        while (_textScores.Count > playerCount)
        {
            int i = _textScores.Count - 1;
            GameObject obj = _textScores[i].gameObject;
            _textScores.RemoveAt(i);
            Destroy(obj.transform.parent.gameObject);
        }
    }

    private void UpdateScores() 
    {
        //int i = 0;
        //foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        //{
        //    if (player != PhotonNetwork.LocalPlayer)
        //    {
        //        //int score = player.TagObject;
        //        if(PhotonNetwork.LocalPlayer.TagObject == null) 
        //        {
        //            _textScores[i].text = "000";
        //        }
        //        else 
        //        {
        //            _textScores[i].text = (player.TagObject as GameObject).GetComponent<Player>().TotalScore.ToString();
        //        }
        //
        //        i++;
        //    }
        //}
    }
}