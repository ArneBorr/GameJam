using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    [SerializeField] List<Sprite> _sprites = new List<Sprite>();

    private void Start()
    {
        if(_sprites.Count == 0) 
        {
            Debug.LogError("Dust::Start() -> No sprites given.");
            return;
        }

        // Change the sprite of the dust particle to a random one
        GetComponentInChildren<SpriteRenderer>().sprite = _sprites[Random.Range(0, _sprites.Count)];        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            Player p = other.gameObject.GetComponent<Player>();
            p.DustPickedUp(1);
            Destroy(this.gameObject);
        }
        if (other.tag == "Huisstofmijt")
        {
            other.GetComponent<Huisstofmijt>().ReachedDust();
            AudioSource audios = other.GetComponent<AudioSource>();
            audios.Play();
            Destroy(this.gameObject, audios.time);
        }
    }
}
