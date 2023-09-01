using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private readonly float swipeSpeed = 0.3f;

    private Ray ray;
    private bool isBusy, isSwiping;
    private HashSet<Vector2> rotatingPlatforms = new HashSet<Vector2>();
    private RaycastHit hit;
    private Vector3 firstPos, secondPos;
    private float howLongMousePressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            howLongMousePressed++;
            //StartCoroutine(moveCamera());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            howLongMousePressed = 0;
                        
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 50))
            {
                Transform part = hit.collider.transform;

                if (hit.collider.gameObject.layer.Equals(7) && !rotatingPlatforms.Contains(new Vector2(part.position.x, part.position.z)))
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

                    StartCoroutine(rotatePart(part, sign));
                }

                
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            firstPos = Input.mousePosition;
            
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 50))
            {
                /*
                Transform part = hit.collider.transform;

                if (hit.collider.gameObject.layer.Equals(7) && !rotatingPlatforms.Contains(new Vector2(part.position.x, part.position.z) ))
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

                    StartCoroutine(rotatePart(part, sign));
                }*/

                //StartCoroutine(moveCamera());
            }
        }
                
    }

    private IEnumerator moveCamera()
    {
        yield return new WaitForSeconds(Time.deltaTime);

        secondPos = Input.mousePosition;
        Vector3 swipeVector = secondPos - firstPos;
        float delta = swipeVector.magnitude;

        if (delta <= 1)
        {
            //yield break;
        }

        //print("delta - " + delta);
        
        Vector3 moving = mainCamera.transform.position - new Vector3(swipeVector.x, 0, swipeVector.y).normalized * 150 * Time.deltaTime;
        mainCamera.transform.DOMove(new Vector3(moving.x,  mainCamera.transform.position.y, moving.z), 0.5f);                
    }

    private IEnumerator rotatePart(Transform _transform, int sign)
    {
        
        //yield return new WaitForSeconds(Time.deltaTime);

        float delta = (Input.mousePosition - firstPos).magnitude;

        //print("!!!!! - long:" + howLongMousePressed + ", delta: " + delta);

        if (delta > 2/* || howLongMousePressed > 3*/)
        {
            yield break;
        }
        

        Vector3 pos = _transform.position;
        rotatingPlatforms.Add(new Vector2(pos.x, pos.z));
        
        _transform.DOPunchPosition(new Vector3(0, 1, 0), 0.05f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.025f);
        _transform.DOPunchPosition(new Vector3(0, 0.5f, 0), 0.05f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.025f);

        
        _transform.DORotate(new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y + 90 * sign, _transform.eulerAngles.z), swipeSpeed);
        yield return new WaitForSeconds(swipeSpeed);
        _transform.position = pos;

        rotatingPlatforms.Remove(new Vector2(pos.x, pos.z));
    }
}
