using UnityEngine;
using MilkShake;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private ShakePreset _passiveShake;
    private Shaker _shaker;

    void Start()
    {
        _shaker = GetComponent<Shaker>();
        _shaker.Shake(_passiveShake);
    }

    private void Update()
    {
    }
}
