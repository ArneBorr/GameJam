using UnityEngine;

public class VacuumDestroy : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().IsBeingVacuumed)
                other.GetComponent<Player>().Vacuum();
        }
        else if (other.CompareTag("Can"))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Huisstofmijt"))
        {
            Destroy(other.gameObject);
            GameObject.FindObjectOfType<HuisstofmijtManager>().SpawnMijt();
        }
        else if (other.CompareTag("Dust"))
        {
            Destroy(other.gameObject);
        }
    }
}
