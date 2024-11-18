using UnityEngine;

public interface IMover
{
    Vector3 GoalPosition { get; }

    void ProcessMove(Vector3 direction);
}