using UnityEngine;

public class TransformMover : IMover
{
    private Transform _transform;

    private float _speed;

    public TransformMover(Transform transform, float speed)
    {
        _transform = transform;
        _speed = speed;
    }

    public Vector3 GoalPosition { get;private set; }

    public void ProcessMove(Vector3 direction)
    {
        if (direction.sqrMagnitude > Mathf.Epsilon * Mathf.Epsilon)
        {
            _transform.position += _speed * Time.deltaTime * direction.normalized;
            GoalPosition = _transform.position;
        }
    }
}
