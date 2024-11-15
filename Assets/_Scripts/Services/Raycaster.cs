using UnityEngine;

public class Raycaster
{
    private RaycastHit[] _hitedObjects = new RaycastHit[16];

    Vector3 _lastPosition;

    public Vector3 RaycastToGround(Vector3 screenPoint, LayerMask raycastableLayer)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);

        int hitCount = Physics.RaycastNonAlloc(ray, _hitedObjects, Mathf.Infinity, raycastableLayer);

        if (hitCount > 0)
            _lastPosition = _hitedObjects[hitCount - 1].point;

        return _lastPosition;
    }
}