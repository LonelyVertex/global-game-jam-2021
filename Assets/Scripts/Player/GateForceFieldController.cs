using UnityEngine;
using Utils;
using Zenject;

public class GateForceFieldController : MonoBehaviour
{
    [SerializeField]
    GameObject gate;
    [SerializeField]
    GameObject door;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Tags.PLAYER) || door.activeSelf)
        {
            return;
        }
        
        gate.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(Tags.PLAYER))
        {
            return;
        }
        
        gate.SetActive(true);
    }
}
