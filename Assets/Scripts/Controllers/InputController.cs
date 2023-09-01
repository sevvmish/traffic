using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Camera mainCamera;
    private Transform cameraTransform;
    private Transform car;
    private Transform location;
    private Joystick joystick;
    private RegionController regionController;

    private readonly float speed = 30;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 firstPos;

    public void SetData(Camera camera, Transform cameraT, Joystick j, Transform _car, Transform loc, RegionController r)
    {
        mainCamera = camera;
        cameraTransform = cameraT;
        joystick = j;
        location = loc;
        car = _car;
        regionController = r;
    }

    
    void Update()
    {
        if (joystick.Direction.magnitude > 0 )
        {
            float x = joystick.Direction.x;
            float z = joystick.Direction.y;

            if ((cameraTransform.position.x > regionController.xBorder.y && x<0) || (cameraTransform.position.x < regionController.xBorder.x && x > 0))
            {
                x = 0;
            }

            if ((cameraTransform.position.z > regionController.zBorder.y && z<0) || (cameraTransform.position.z < regionController.zBorder.x && z>0))
            {
                z = 0;
            }

            cameraTransform.position -= new Vector3(x, 0, z) * Time.deltaTime * speed;

        }

        if (Input.GetKey(KeyCode.J))
        {
            mainCamera.transform.DOPunchPosition(new Vector3(0, 1, 0), 0.2f).SetEase(Ease.OutSine);
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (cameraTransform.position.z < regionController.zBorder.y)
                cameraTransform.position += new Vector3(0, 0, 1) * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (cameraTransform.position.z > regionController.zBorder.x)
                cameraTransform.position += new Vector3(0, 0, -1) * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (cameraTransform.position.x > regionController.xBorder.x)
            cameraTransform.position += new Vector3(-1, 0, 0) * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (cameraTransform.position.x < regionController.xBorder.y)
                cameraTransform.position += new Vector3(1, 0, 0) * Time.deltaTime * speed;
        }

        if (Input.GetMouseButtonUp(0))
        {            
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 50))
            {
                Transform part = hit.collider.transform;

                if (hit.collider.gameObject.layer.Equals(7) && hit.collider.TryGetComponent(out Region region) && !region.IsBusyRotate)
                {
                    int sign = 0;
                    if (hit.point.x >= part.position.x)
                    {
                        sign = 1;
                    }
                    else
                    {
                        sign = -1;
                    }

                    float delta = (Input.mousePosition - firstPos).magnitude;
                    
                    if (delta <= 50)
                    {
                        region.RotateRegion(sign);
                    }                    
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            firstPos = Input.mousePosition;
        }
    }
}
