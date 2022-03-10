using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Animator _animator;
    private PhotonView _view;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_view.IsMine)
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            transform.position += input.normalized * _speed * Time.deltaTime;

            _animator.SetBool("isRunning", input != Vector3.zero);
        }
    }
}
