using UnityEngine;
using UnityEngine.AI;

public class NavMeshMover : IMover
{
    private NavMeshAgent _navAgent;

    public NavMeshMover(NavMeshAgent navAgent)
    {
        _navAgent = navAgent;
    }

    public void ProcessMove(Vector3 direction)
    {
        NavMeshPath path = new NavMeshPath();

        if (direction.sqrMagnitude > _navAgent.stoppingDistance * _navAgent.stoppingDistance)
        {
            if (IsPathExists(path, direction))
                _navAgent.SetDestination(direction);
        }
    }

    private bool IsPathExists(NavMeshPath path, Vector3 targetPosition)
    {
        path.ClearCorners();

        if (_navAgent.CalculatePath(targetPosition, path) && path.status != NavMeshPathStatus.PathInvalid)
            return true;

        return false;
    }
}
