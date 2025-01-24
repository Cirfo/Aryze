using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    Camera camera;
    public GameObject target;
    public float smoothTime;
    private Vector3 panVelocity = Vector3.zero;
    bool isZoom = false;
    bool isTurned = false;
    float cameraX;
    float turnTime = 0.75f;

    // target objects

    // zoom variables
    private float fov;
    private float targetFov;
    private float lerp;
    private float zoomDuration;
    private GameObject oldTarget;
    private float oldSmoothTime;

    // pan
    private bool enablePan;
    private bool isPanning;
    private bool stopPanningStarted;
    Coroutine stopPanningCoroutine;
    private static readonly float PanSpeed = 20f;
    private static readonly float ZoomSpeedTouch = 0.1f;
    private static readonly float ZoomSpeedMouse = 0.5f;

    private static readonly float[] BoundsX = new float[] { -2f, 2f }; // left righ
    private static readonly float[] BoundsZ = new float[] { -8f, 0f }; // up down

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    void Start()
    {
        camera = GetComponent<Camera>();
        fov = camera.fieldOfView;
        smoothTime = 0.3f;
        oldSmoothTime = smoothTime;
        lerp = 0;
        enablePan = true;
        isPanning = false;
        stopPanningStarted = false;
    }

    void Update()
    {
        if (target != null && !isPanning)
        {
            // following target
            FollowTarget();
        }
        if (isZoom)
        {
            // zooming
            lerp += Time.deltaTime / zoomDuration;
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFov, lerp);
            if (Mathf.Abs(camera.fieldOfView - targetFov) < 0.05f)
            {
                isZoom = false;
                lerp = 0;
            }
        }
        else if (enablePan)
        {
            if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                HandleTouch();
            }
            else
            {
                HandleMouse();
            }
        }
    }

    void FollowTarget()
    {
        cameraX = isTurned ? -4 : 4;
        Vector3 goalPos = new Vector3(target.transform.position.x - cameraX, transform.position.y, target.transform.position.z - 4);
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref panVelocity, smoothTime);
    }

    /// <summary>
    /// Zoom on a specific target
    /// </summary>
    /// <param name="target"></param>
    /// <param name="targetFov"></param>
    /// <param name="duration"></param>
    /// <param name="panSmoothTime"></param>
    public void ZoomOnTarget(GameObject target, int targetFov, float duration, float panSmoothTime)
    {
        oldTarget = this.target;
        oldSmoothTime = smoothTime;
        smoothTime = panSmoothTime;
        this.target = target;
        zoomDuration = duration;
        this.targetFov = targetFov;
        isZoom = true;
    }

    /// <summary>
    /// Reset the zoom applied by ZoomOnTarget()
    /// </summary>
    public void ResetZoom()
    {
        isZoom = true;
        zoomDuration = 1f;
        targetFov = fov;
        target = oldTarget;
        smoothTime = oldSmoothTime;
    }

    /// <summary>
    /// Turn camera in order to face the other side of the board
    /// Both automatically or with an input direction
    /// <see langword="false"/> is standard side <see langword="true"/> the other
    /// </summary>
    /// <param name="direction"></param>
    public bool Turn(bool? direction)
    {
        float x = transform.localRotation.eulerAngles.x;
        float z = transform.localRotation.eulerAngles.z;

        if (direction != null && direction.Value != isTurned)
        {
            if (direction.Value)
            {
                transform.DORotate(new Vector3(x, -45, z), turnTime);
                transform.DOLocalMoveX(4, turnTime);
                isTurned = true;
            }
            else
            {
                transform.DORotate(new Vector3(x, 45, z), turnTime);
                transform.DOLocalMoveX(-4, turnTime);
                isTurned = false;
            }
        }
        else if (direction == null)
        {
            if (!isTurned)
            {
                transform.DORotate(new Vector3(x, -45, z), turnTime);
                transform.DOLocalMoveX(4, turnTime);
                isTurned = true;
            }
            else
            {
                transform.DORotate(new Vector3(x, 45, z), turnTime);
                transform.DOLocalMoveX(-4, turnTime);
                isTurned = false;
            }
        }
        return isTurned;
    }

    //############################# PAN SECTION ############################


    /// <summary> Checks if the the current input is over canvas UI </summary>
    public bool IsPointerOverUIObject()
    {
        if (EventSystem.current == null) return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void EnablePanning()
    {
        enablePan = true;
    }

    public void DisablePanning()
    {
        enablePan = false;
        isPanning = false;
    }

    void HandleTouch()
    {
        // If the touch began, capture its position and its finger ID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !IsPointerOverUIObject())
            {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
            }
            // Otherwise, if the finger ID of the touch doesn't match, skip it.
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved && !IsPointerOverUIObject())
            {
                PanCamera(touch.position);
            }
        }
        else
        {
            if (stopPanningCoroutine == null)
                stopPanningCoroutine = StartCoroutine(StopPanning());
        }
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            lastPanPosition = Input.mousePosition;
        }
        // Otherwise, if the mouse is still down, pan the camera
        else if (Input.GetMouseButton(0) && !IsPointerOverUIObject())
        {
            PanCamera(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (stopPanningCoroutine == null)
                stopPanningCoroutine = StartCoroutine(StopPanning());
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        if (stopPanningCoroutine != null)
        {
            StopCoroutine(stopPanningCoroutine);
            stopPanningCoroutine = null;
        }
        isPanning = true;
        // Determine how much to move the camera
        Vector3 offset = camera.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    private IEnumerator StopPanning()
    {
        yield return new WaitForSeconds(2);
        isPanning = false;
    }
}
