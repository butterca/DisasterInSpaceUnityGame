using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85;

    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


   // Used to set velocity
    public void Move (Vector3 _velocity)
    {
        velocity = _velocity;
    }


    // Used to set rotation
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    // Used to set camera rotation
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    //Apply Thruster
    public void ApplyThruster (Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    // Update is called once per frame
    void Update()
    {
        PerformMovement();
        PerformRotation();
    }

    // Moves player with the velocity
    void PerformMovement ()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
        }

        if(thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    // Roates player with the rotation and camera rotation
    void PerformRotation()
    {
        rb.MoveRotation(transform.rotation * Quaternion.Euler(rotation));
        if(cam != null)
        {
            //Set rotation and clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            // apply rotation to transform of camera
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        }
    }



}
