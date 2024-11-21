using UnityEngine;
using UnityEngine.AI;

public class NavMeshMover : IMover
{
    private NavMeshAgent _navAgent;

    public NavMeshMover(NavMeshAgent navAgent)
    {
        _navAgent = navAgent;
    }

    public Vector3 GoalPosition { get; private set; }

    public void ProcessMove(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();

        if (position.magnitude > _navAgent.stoppingDistance)
        {
            _navAgent.isStopped = false;

            if (IsPathExists(path, position))
            {
                _navAgent.SetDestination(position);

                GoalPosition = _navAgent.destination;
            }
        }
        else
        {
            _navAgent.isStopped = true;
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