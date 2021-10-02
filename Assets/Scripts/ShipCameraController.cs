using System;
using UnityEngine;
using UnityEngine.Assertions;

public class ShipCameraController : MonoBehaviour
{
    public Camera mainCamera;
    public float movementSmoothness = 0.9f;
    public float sizeSmoothness = 0.9f;

    public Transform pointOfInterest;
    public float pointOfInterestRadius = 1.0f;
    
    private Vector3 _lastCameraPosition;
    private float _lastSize = 1.0f;
    private float _initialSize = 1.0f;

    private void Awake()
    {
        Assert.IsNotNull(mainCamera, "mainCamera != null");
        
        _lastCameraPosition = transform.position;
        _lastSize = _initialSize = mainCamera.orthographicSize;
        mainCamera.transform.position = _lastCameraPosition;
    }

    private void FixedUpdate()
    {
        var currentPosition = transform.position;
        var targetPosition = currentPosition;
        var targetSize = _initialSize;
        
        if (pointOfInterest != null)
        {
            var pointOfInterestPosition = pointOfInterest.position;
            targetPosition = (targetPosition + pointOfInterestPosition) / 2;


            var cornerPosition = pointOfInterestPosition;
            cornerPosition.x += pointOfInterestRadius * (cornerPosition.x < targetPosition.x ? -1 : 1);
            cornerPosition.y += pointOfInterestRadius * (cornerPosition.y < targetPosition.y ? -1 : 1);

            var screenAspect = (float)Screen.width / Screen.height;
            var diffX = Math.Abs(cornerPosition.x - currentPosition.x);
            var diffY = Math.Abs(cornerPosition.y - currentPosition.y);

            targetSize = Math.Max(_initialSize, Math.Max(diffX / screenAspect, diffY));
        }
        
        var newPosition = Vector3.Lerp(_lastCameraPosition, targetPosition, movementSmoothness);
        mainCamera.transform.position = newPosition;
        _lastCameraPosition = newPosition;

        var newSize = _lastSize + (targetSize - _lastSize) * sizeSmoothness;
        mainCamera.orthographicSize = newSize;
        _lastSize = newSize;
    }
}
