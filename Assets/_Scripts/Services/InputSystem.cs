using UnityEngine;

public class InputSystem
{
    private const string GroundLayerName = "Ground";

    private const int LeftMouseButton = 0;
    private const int RightMouseButton = 1;

    public Vector3 ReadMousePosition()
        => Input.mousePosition;

    public bool LeftMousePressed()
        => Input.GetMouseButton(LeftMouseButton);

    public bool RightMousePressed()
        => Input.GetMouseButton(RightMouseButton);
}