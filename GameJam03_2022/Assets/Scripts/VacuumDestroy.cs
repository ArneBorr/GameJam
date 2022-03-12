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
    }
}
