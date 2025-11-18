using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour, iShootControlReviever, IHealthUpdateReceiver
{
    [Header("Input Management")]
    Vector3 inputVector;
    Vector3 rotateVector;

    bool flag_doJump = false;
    bool flag_shoot = false;

    [Header("Movement")]
    public float MoveSpeed = 5;
    public float JumpImpulse = 8;
    public float gravity = -20;
    public bool below = false;

    float ySpeed = 0f;

    CharacterController controller;

    [Header("Rotations")]
    public Transform viewCamera;
    public float RotateMultiplierX = 10f;
    public float RotateMultiplierY = 10f;
    public float MaxYRotateLimit = 90f;
    public float MinYRotateLimit = -90f;

    float rotateY = 0f;

    [Header("Shooting")]
    ShootingControl shootControl;

    [Header("Game over")]
    public GameObject HUDNode;
    public GameObject deathMenuNode;

    [Header("OpenPauseMenu Listener")]
    PauseListener pauseListener;

    // Leave out for now---public GameObject ammoGUI;
    //[SerializeField] 
    //private int currentAmmo = 0; //Store how much ammo we currently hold

    public void InjectShootControl(ShootingControl shootControl)
    {
        this.shootControl = shootControl;
    }

    void IHealthUpdateReceiver.GetHurt(int currentHealth, int maxHealth)
    {
        
    }

    void IHealthUpdateReceiver.GetHealed(int currentHealth, int maxHealth)
    {
        
    }

    void IHealthUpdateReceiver.GetKilled()
    {
        if(HUDNode != null)
        {
            HUDNode.SetActive(false);
        }
        if (deathMenuNode != null)
        {
            deathMenuNode.SetActive(true);
        }

        Destroy(this);
    }

    private void OnDestroy() //Gets called when this object gets deleted
    {
        pauseListener.Destroy();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameObject here is the same object as our player object
        controller = gameObject.GetComponent<CharacterController>();
        pauseListener = new PauseListener();
    }



    // Update is called once per frame
    void Update()
    {
        if (pauseListener.paused)
        {
            return;
        }
        GetInput();
        Move();
        Rotate();
        if (CanShoot() == true)
        {
            shootControl.shoot();
        }
    }

    void GetInput()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");

        if (inputVector.magnitude > 1)
        {
            inputVector.Normalize();
        }

        rotateVector.x = Input.GetAxis("Mouse X");
        rotateVector.y = Input.GetAxis("Mouse Y");

        if (rotateVector.magnitude > 1)
        {
            rotateVector.Normalize();
        }

        flag_doJump = Input.GetButtonDown("Jump");
        flag_shoot = Input.GetButtonDown("Fire1");

    }

    void Move()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) ||  Input.GetKeyDown(KeyCode.RightShift))
        {
            MoveSpeed = 10;
        }
        else
        {
            MoveSpeed = 5;
        }

            //Foward, Back, Left, Right movement
            Vector3 moveVector = transform.TransformDirection(inputVector);
        moveVector *= MoveSpeed * Time.deltaTime;

        //Jump movement
        if (canJump())
        {
            ySpeed = JumpImpulse;
        }
        else
        {
            ySpeed += gravity * Time.deltaTime;
        }

        ySpeed = Mathf.Clamp(ySpeed, gravity, JumpImpulse);
        moveVector.y = ySpeed * Time.deltaTime;
        
        //Apply movement to character
        controller.Move(moveVector);

        //Stopping extra gravity when on the ground
        if (OnGround())
        {
            ySpeed = 0f;
            below = true;
        }
        else
        {
            below = false;
        }

    }

    void Rotate()
    {
        transform.Rotate(transform.up, rotateVector.x * RotateMultiplierX, Space.Self);

        rotateY += rotateVector.y * RotateMultiplierY; //Rotation calculation
        rotateY = Mathf.Clamp(rotateY, MinYRotateLimit, MaxYRotateLimit); //Limits rotations

        viewCamera.localEulerAngles = new Vector3(-rotateY, viewCamera.localEulerAngles.y, viewCamera.localEulerAngles.z);
    }

    bool CanShoot()
    {
        return flag_shoot;
    }
    
    bool canJump()
    {
        return flag_doJump && OnGround();
    }

    bool OnGround()
    {
        return (controller.collisionFlags & CollisionFlags.Below) != 0;
    }

}
