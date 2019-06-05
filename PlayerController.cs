using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    //Create Variables
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float lookSens = 3f;

    [SerializeField]
    private float thrusterForce = 1000;

    [Header("Spring settings:")]
    [SerializeField]
    private JointDriveMode jointMode = JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;


    private PlayerMotor motor;
    private ConfigurableJoint joint;

    //Start Method
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();

        SetJointSettings(jointSpring);
    }


    //Update Method
    void Update()
    {
        //Calc Movement Velocity
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _xMov;
        Vector3 _moveVertical = transform.forward * _zMov;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;

        //Apply Movement
        motor.Move(_velocity);

        //Calc Rotation (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSens;

        //Apply Camera Rotation
        motor.Rotate(_rotation);

        //Calc Camera Rotation (looking up/down)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSens;

        //Apply CAmera Rotation
        motor.RotateCamera(_cameraRotationX);

        //Calculate Thruster Force
        Vector3 _thrusterForce = Vector3.zero;

        if (Input.GetButton("Jump"))
        {
            _thrusterForce = Vector3.up * thrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(jointSpring);
        }

        //Apply Thruster Force
        motor.ApplyThruster(_thrusterForce);

    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            mode = jointMode,
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }

}
