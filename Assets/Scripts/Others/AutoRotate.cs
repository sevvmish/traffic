using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public float Speed = 10;
    public bool IsX;
    public bool IsY;
    public bool IsZ;

    
    private void Update()
    {
        if (IsX || IsY || IsZ)
        {
            if (IsX)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + Speed * Time.deltaTime, transform.localEulerAngles.y, transform.localEulerAngles.z);
            }
            else if (IsY)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + Speed * Time.deltaTime, transform.localEulerAngles.z);
            }
            else if (IsZ)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + Speed * Time.deltaTime);
            }
        }
    }


}
