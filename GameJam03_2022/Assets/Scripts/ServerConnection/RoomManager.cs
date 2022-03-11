using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _createInput;
    [SerializeField] private InputField _joinInput;
    [SerializeField] private InputField _nameInput;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ChangeName()
    {
        PhotonNetwork.NickName = _nameInput.text;
    }

    public void CreateRoom()
    {
        _audioSource.Play();
        PhotonNetwork.CreateRoom(_createInput.text);
    }

    public void JoinRoom()
    {
        _audioSource.Play();
        PhotonNetwork.JoinRoom(_joinInput.text);
    }

    public override void OnJoinedRoom() 
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
