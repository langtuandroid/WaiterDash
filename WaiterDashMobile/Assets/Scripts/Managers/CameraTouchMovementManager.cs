using Cinemachine;
using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CameraTouchMovementManager : MonoBehaviour
    {
        public static CameraTouchMovementManager Instance;
        public Camera Camera;
        public bool Rotate;
        protected Plane Plane;

        private Vector3 previousMousePosition;
        public bool isDragging = false;

        private void OnEnable()
        {
            RestaurantManager.OnRestaurantClosed += OnRestaurantClosed;
            RestaurantManager.OnRestaurantOpened += OnRestaurantOpened;
        }

        private void OnDisable()
        {
            RestaurantManager.OnRestaurantClosed -= OnRestaurantClosed;
            RestaurantManager.OnRestaurantOpened -= OnRestaurantOpened;
        }

        private void Awake()
        {
            Instance = this;
            if (Camera == null)
                Camera = Camera.main;
        }

        private void Start()
        {

            Camera.gameObject.GetComponent<CinemachineBrain>().enabled = false;
        }

        private void OnRestaurantOpened(object sender, EventArgs e)
        {
            Camera.gameObject.GetComponent<CinemachineBrain>().enabled = true;
        }

        private void OnRestaurantClosed(object sender, EventArgs e)
        {
            Camera.gameObject.GetComponent<CinemachineBrain>().enabled = false;
        }

        private void Update()
        {
            // Update Plane
            if (Input.touchCount >= 1 || Input.GetMouseButton(0))
                Plane.SetNormalAndPosition(transform.up, transform.position);

            var Delta1 = Vector3.zero;
            var Delta2 = Vector3.zero;

            // Handle touch input
            if (Input.touchCount >= 1)
            {
                Delta1 = PlanePositionDelta(Input.GetTouch(0));
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Camera.transform.Translate(Delta1, Space.World);
                    isDragging = true;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    isDragging = false;
                }
            }

            // Handle mouse input
            if (Input.GetMouseButtonDown(0))
            {
                previousMousePosition = Input.mousePosition;
                isDragging = false;
            }
            else if (Input.GetMouseButton(0))
            {
                Delta1 = PlanePositionDeltaMouse();
                Camera.transform.Translate(Delta1, Space.World);

                if ((Input.mousePosition - previousMousePosition).magnitude > 5f) // Consider a threshold to determine dragging
                {
                    isDragging = true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // Handle pinch with touch
            if (Input.touchCount >= 2)
            {
                var pos1 = PlanePosition(Input.GetTouch(0).position);
                var pos2 = PlanePosition(Input.GetTouch(1).position);
                var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
                var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

                // Calculate zoom
                var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);

                // Edge case
                if (zoom == 0 || zoom > 10)
                    return;

                // Move cam along the mid ray
                Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);

                if (Rotate && pos2b != pos2)
                    Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
            }

            // Handle zoom with mouse scroll wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                var zoom = 1.0f + scroll;
                var midPoint = PlanePosition(Input.mousePosition);
                Camera.transform.position = Vector3.LerpUnclamped(midPoint, Camera.transform.position, 1 / zoom);
            }

            // Update previous mouse position
            previousMousePosition = Input.mousePosition;
        }

        protected Vector3 PlanePositionDelta(Touch touch)
        {
            // Not moved
            if (touch.phase != TouchPhase.Moved)
                return Vector3.zero;

            // Delta
            var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
            var rayNow = Camera.ScreenPointToRay(touch.position);
            if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
                return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

            // Not on plane
            return Vector3.zero;
        }

        protected Vector3 PlanePositionDeltaMouse()
        {
            // Not moved
            if (!Input.GetMouseButton(0))
                return Vector3.zero;

            // Delta
            var delta = new Vector2(Input.mousePosition.x - previousMousePosition.x, Input.mousePosition.y - previousMousePosition.y);
            var rayBefore = Camera.ScreenPointToRay(previousMousePosition);
            var rayNow = Camera.ScreenPointToRay(Input.mousePosition);
            if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
                return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

            // Not on plane
            return Vector3.zero;
        }

        protected Vector3 PlanePosition(Vector2 screenPos)
        {
            // Position
            var rayNow = Camera.ScreenPointToRay(screenPos);
            if (Plane.Raycast(rayNow, out var enterNow))
                return rayNow.GetPoint(enterNow);

            return Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.up);
        }
    }
}
