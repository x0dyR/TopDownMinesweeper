using UnityEngine;

public class PathGoalPoint
{
    private PathGoalObject _visualObject;

    private Vector3 _lastPosition;

    public PathGoalPoint(PathGoalObject visualObject)
    {
        _visualObject = Object.Instantiate(visualObject, Vector3.down, Quaternion.identity, null);
    }

    public void MoveTo(Vector3 position)
    {
        if (_lastPosition == position)
            return;

        _lastPosition = position;
        _visualObject.transform.position = position;
    }
}