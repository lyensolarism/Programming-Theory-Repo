using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            var horizontalInput = Input.GetAxis("Horizontal");

            transform.Rotate(Vector3.down, horizontalInput * rotationSpeed * Time.deltaTime);
        }
    }
}
