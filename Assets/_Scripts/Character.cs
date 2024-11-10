using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    private NavMeshAgent _navAgent;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {

    }
}
