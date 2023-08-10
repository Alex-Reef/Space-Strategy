using System;
using Cinemachine;
using GalaxySystem.Behavior;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent, ]
public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom, maxZoom;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Camera camera;
    [SerializeField] private int viewRadius;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private GalaxyBehavior _galaxyBehavior;
    
    private float _startedDistance = 0;
    private float _currentZoom;

    private void Awake()
    {
        _galaxyBehavior.OnStarSelected += (id) =>
        {
            if (id != -1)
                MoveTo(_galaxyBehavior.SystemsBehaviors[id].transform);
            else
                MoveToDefault();
        };
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if(Input.touchCount == 1)
                Movement();
            if(Input.touchCount >= 2)
                Zoom();
        }   
    }

    private void Movement()
    {
        Touch touch = Input.GetTouch(0);
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;
        if (touch.phase == TouchPhase.Moved)
        {
            Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, 0f);
            Vector3 worldTouchPosition = camera.ScreenToWorldPoint(touchPosition);
            worldTouchPosition.x = Mathf.Clamp(worldTouchPosition.x, -viewRadius, viewRadius);
            worldTouchPosition.z = Mathf.Clamp(worldTouchPosition.z, -viewRadius, viewRadius);
            worldTouchPosition.y = 0f; 
                
            aimTarget.position = Vector3.Lerp(aimTarget.position, worldTouchPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void Zoom()
    {
        var touches = Input.touches;
        if (EventSystem.current.IsPointerOverGameObject(touches[0].fingerId) ||
            EventSystem.current.IsPointerOverGameObject(touches[1].fingerId))
            return;

        if (touches[0].phase == TouchPhase.Began || touches[1].phase == TouchPhase.Began)
        {
            _startedDistance = Vector2.Distance(touches[0].position, touches[1].position);
        }
        
        if (touches[0].phase == TouchPhase.Moved && touches[1].phase == TouchPhase.Moved)
        {
            var distance = Vector2.Distance(touches[0].position, touches[1].position);
            var current = virtualCamera.m_Lens.OrthographicSize;
            if (distance > _startedDistance)
            {
                if (current > maxZoom)
                    current -= zoomSpeed * Time.deltaTime;
            }
            else
            {
                if(current < minZoom)
                    current += zoomSpeed * Time.deltaTime;
            }

            _startedDistance = distance;
            virtualCamera.m_Lens.OrthographicSize = current;
        }
    }

    private void MoveTo(Transform starTransform)
    {
        _currentZoom = virtualCamera.m_Lens.OrthographicSize;

        virtualCamera.Follow = starTransform;
        virtualCamera.LookAt = starTransform;

        virtualCamera.m_Lens.OrthographicSize = 5;
    }

    private void MoveToDefault()
    {
        virtualCamera.Follow = aimTarget;
        virtualCamera.LookAt = aimTarget;
        virtualCamera.m_Lens.OrthographicSize = _currentZoom;
    }
}
