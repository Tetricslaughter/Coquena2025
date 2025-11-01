using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float rotationSpeed = 3f;

    private float currentX;
    private float currentY;
    public float minY = -20f;
    public float maxY = 60f;

    void LateUpdate()
    {
        if (!target) return;

        currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        currentY = Mathf.Clamp(currentY, minY, maxY);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 dir = new Vector3(0, 0, -distance);
        Vector3 position = target.position + rotation * dir + Vector3.up * height;

        transform.position = position;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
