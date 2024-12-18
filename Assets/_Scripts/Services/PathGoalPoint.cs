using UnityEngine;

public class PathGoalPoint : MonoBehaviour
{
    private PathGoalObject _visualObject;

    private Vector3 _lastPosition;

    public PathGoalObject PathGolaObject => _visualObject;

    public void Initialize(PathGoalObject visualObject)
    {
        _visualObject = Instantiate(visualObject, Vector3.down, Quaternion.identity, null);
    }

    public void MoveTo(Vector3 position)
    {
        if (_lastPosition == position)
            return;

        _lastPosition = position;
        _visualObject.transform.position = _lastPosition;
    }
}