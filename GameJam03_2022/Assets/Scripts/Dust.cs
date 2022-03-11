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
        Debug.Log("huh");
        if (other.tag == "Player")
        {
            Debug.Log("Oke");
            Player p = other.gameObject.GetComponent<Player>();
            p.DustPickedUp(6);
            Destroy(this.gameObject);
        }
    }
}
