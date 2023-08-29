using Cinemachine;
using GalaxySystem.Behavior;
using GalaxySystem.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Zenject;

namespace CameraSystem
{
    [DisallowMultipleComponent]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private NumbersRange zoom;
        [SerializeField] private Camera camera;
        [SerializeField] private int moveRadius;
         
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [Inject] private GalaxyBehavior _galaxyBehavior;

        private Transform _touchPoint;
        private float _startedDistance = 0;
        private float _currentZoom;

        private void Awake()
        {
            _currentZoom = virtualCamera.m_Lens.OrthographicSize;
            _touchPoint = new GameObject("TouchPoint").transform;

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
                if (Input.touchCount == 1)
                    Movement();
                if (Input.touchCount >= 2)
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
                worldTouchPosition.x = Mathf.Clamp(worldTouchPosition.x, -moveRadius, moveRadius);
                worldTouchPosition.z = Mathf.Clamp(worldTouchPosition.z, -moveRadius, moveRadius);
                worldTouchPosition.y = 0f;

                _touchPoint.position =
                    Vector3.Lerp(_touchPoint.position, worldTouchPosition, moveSpeed * Time.deltaTime);
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
                    if (current > zoom.Max)
                        current -= zoomSpeed * Time.deltaTime;
                }
                else
                {
                    if (current < zoom.Min)
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
            virtualCamera.Follow = _touchPoint;
            virtualCamera.LookAt = _touchPoint;
            virtualCamera.m_Lens.OrthographicSize = _currentZoom;
        }
    }
}
