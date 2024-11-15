using UnityEngine;

public class PathGoalPoint
{
    private GameObject _visualObject;

    private Vector3 _lastPosition;

    public PathGoalPoint(GameObject visualObject)
    {
        _visualObject = Object.Instantiate(visualObject);
    }

    public void MoveTo(Vector3 position)
    {
        _visualObject.transform.position = position;
    }
}
