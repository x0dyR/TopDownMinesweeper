using UnityEngine;

public class MovePositionMediator : MonoBehaviour
{
    private Character _character;
    private PathGoalPoint _pathGoalVisualizer;

    public void Initialize(Character character, PathGoalPoint pathGoalPoint)
    {
        _character = character;
        _pathGoalVisualizer = pathGoalPoint;

        _character.Running += OnRunning;
        _character.Died += OnDied;
    }

    private void OnDied()
    {
        _pathGoalVisualizer.PathGolaObject.gameObject.SetActive(false);
    }

    private void OnRunning(Vector3 position)
    {
        _pathGoalVisualizer.MoveTo(position);
    }

    private void OnDestroy()
    {
        _character.Running -= OnRunning;
    }
}
