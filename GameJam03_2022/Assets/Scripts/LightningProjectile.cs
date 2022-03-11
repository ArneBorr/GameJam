using UnityEngine;

public class LightningProjectile : MonoBehaviour
{
    [SerializeField] private float _movementspeed = 0.5f;
    [SerializeField] private float _lifeTime = 5f;

    private GameObject _shooter = null;
    private AudioSource _audioSource = null;

    public GameObject Shooter { set { _shooter = value; } }

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(this.transform.forward * _movementspeed);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
        Destroy(gameObject, _lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject == _shooter) return;

            Player p = other.gameObject.GetComponent<Player>();
            p.TakeDustOff(1);
            Destroy(this.gameObject);
        }
        else if (other.GetComponent<Huisstofmijt>()) 
        {
            GameObject.FindObjectOfType<HuisstofmijtManager>().SpawnMijt();
            Destroy(other.gameObject);
        }
    }
}
