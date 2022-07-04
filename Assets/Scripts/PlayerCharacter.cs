using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    #region Variables
    PlayerController _control;

    private float NORMAL_FOV = 60f;
    private float HOOKSHOT_FOV = 100f;

    [SerializeField] private Transform debugHitPointTransform;
    [SerializeField] private Transform hookshotTransform;


    [Header("Base stats")]
    public float health = 100f;
    public float moveSpeed = 20f;
    public float hookRange = 20f;
    public LayerMask hookMask;
    public AudioSource walkClip;

    [Header("Wall Running")]
    [SerializeField] private Transform orientation;
    [SerializeField] private float wallDistance = .5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit; 
    public LayerMask wallRunningMask;
    public float wallJumpForce = 100f;
    public float wallRunningSpeed = 60f;
    public bool wallLeft = false;
    public bool wallRight = false;
    
    [Header("Mouse")]
    public float mouseSensitivityValue;
    public float mouseSensitivity;

    [Header("Horizontal/Vertical movement")]
    public float moveX;
    public float moveZ;

    [Header("Camera movement")]
    public float lookX;
    public float lookY;


    public float coyoteTime = 3f;
    public float coyoteTimer;

    private CharacterController characterController;
    private float cameraVerticalAngle;
    private float characterVelocityY;
    private Vector3 characterVelocityMomentum;
    private Camera playerCamera;
    private CameraFov cameraFov;
    private ParticleSystem speedLinesParticleSystem;
    private ParticleSystem impulseLinesParticleSystem;
    private State state;
    private Vector3 hookshotPosition;
    private float hookshotSize;

    #endregion
    
    #region Unity callbacks
    void Start()
    {
        coyoteTimer = coyoteTime;
        mouseSensitivity = mouseSensitivityValue;
        _control = new PlayerController(this);

        characterController = GetComponent<CharacterController>();
        playerCamera = transform.Find("Camera").GetComponent<Camera>();
        cameraFov = playerCamera.GetComponent<CameraFov>();
        speedLinesParticleSystem = transform.Find("Camera").Find("SpeedLinesParticleSystem").GetComponent<ParticleSystem>();
        impulseLinesParticleSystem = transform.Find("Camera").Find("ImpulseLinesParticleSystem").GetComponent<ParticleSystem>();
        Cursor.lockState = CursorLockMode.Locked;
        state = State.Normal;
        hookshotTransform.gameObject.SetActive(false);
    }

    void Update() {
        _control.OnUpdate();
        States();

        if (PauseMenu.GameIsPaused && Time.timeScale == 0) mouseSensitivity = 0;
        else mouseSensitivity = mouseSensitivityValue;
        //wall running 
        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun();
                Debug.Log("wall running: left");
            }
            else if (wallRight)
            {
                StartWallRun();
                Debug.Log("wall running: right");
            }
            else StopWallRun();
        }
        else StopWallRun();

    }
    #endregion

    #region Control de Camara 

    private void CharacterLook() {
        // Rota el transform con la speed del input al rededor de su eje Y local
        transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);

        // Input vertical para el angulo vertical de la camara
        cameraVerticalAngle -= lookY * mouseSensitivity;

        // Clampeo los limites min/max del angulo vertical de la camara
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

        // Aplico el angulo vertical como rotacion local del transform de la camara
        // para mirar hacia arriba y abajo
        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
    }
    #endregion

    #region Desplazamientos
    private void CharacterMovement() {        
        Vector3 characterVelocity = transform.right * moveX * moveSpeed + transform.forward * moveZ * moveSpeed;
        float jumpSpeed = 30f;

        //COYOTE TIME (MAS TIEMPO PARA SALTAR) PARA MAGNO

        coyoteTimer = Mathf.Clamp(coyoteTimer, 0f, coyoteTime);
        if(coyoteTimer >= 0f) {
            if (characterController.isGrounded)
            {
                characterVelocityY = 0f;
                coyoteTimer = coyoteTime;
                // Jump
                if (_control.Jump() && coyoteTimer >= 0f) {
                    coyoteTimer = 0f;
                    characterVelocityY = jumpSpeed;

                }
            }
            else if(characterController.isGrounded == false)
                coyoteTimer -= Time.deltaTime;
            if(_control.Jump() && coyoteTimer >= 0f) { 
                characterVelocityY = jumpSpeed;
                coyoteTimer = 0f;
                AudioManager.instance.Play("Player Jump");

            }
        }

        //Aplico gravedad
        float gravityDownForce = -60f;
        characterVelocityY += gravityDownForce * Time.deltaTime;

        // Aplico velocidad en Y para mover el vector
        characterVelocity.y = characterVelocityY;

        // Aplico momentum
        characterVelocity += characterVelocityMomentum;

        // Muevo el Character Controller
        characterController.Move(characterVelocity * Time.deltaTime);

        if(characterController.isGrounded == true && characterController.velocity.magnitude > 2f && walkClip.isPlaying == false)
        {
            walkClip.pitch = Random.Range(0.9f, 1.4f);
            walkClip.Play();
        }else if(characterController.isGrounded == false || characterController.isGrounded == true && characterController.velocity.magnitude == 0f && walkClip.isPlaying == true) walkClip.Stop();
        
         
         
        // Reducir constantemente el momentum
        if (characterVelocityMomentum.magnitude > 0f)
        {
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;
            if (characterVelocityMomentum.magnitude < .0f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }

        //Clampeo la velocidad para no moverme mas rapido en diagonales
        characterVelocity = Vector3.ClampMagnitude(characterVelocity, moveSpeed);
    }

    public void HookshotStart() {
        if (_control.Hookshot())
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit, hookRange,hookMask))
            {
                // El rayo conecta con algo
                debugHitPointTransform.position = raycastHit.point;
                hookshotPosition = raycastHit.point;
                hookshotSize = 0f;
                hookshotTransform.gameObject.SetActive(true);
                hookshotTransform.localScale = Vector3.zero;
                state = State.HookshotThrown;

                AudioManager.instance.Play("Player Hook");


            }
        }
    }

    private void HookshotThrow() {
        hookshotTransform.LookAt(hookshotPosition);

        float hookshotThrowSpeed = 500f;
        hookshotSize += hookshotThrowSpeed * Time.deltaTime;
        hookshotTransform.localScale = new Vector3(1, 1, hookshotSize);

        if (hookshotSize >= Vector3.Distance(transform.position, hookshotPosition))
        {
            state = State.HookshotFly;
            cameraFov.SetCameraFov(HOOKSHOT_FOV);
            speedLinesParticleSystem.Play();
        }
    }

    private void HookshotMovement() {
        hookshotTransform.LookAt(hookshotPosition);

        Vector3 hookshotDir = (hookshotPosition - transform.position).normalized;

        float hookshotSpeedMin = 10f;
        float hookshotSpeedMax = 40f;
        float hookshotSpeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPosition), hookshotSpeedMin, hookshotSpeedMax);
        float hookshotSpeedMultiplier = 5f;

        // Muevo el Character Controller hacia a pos del hook
        characterController.Move(hookshotDir * hookshotSpeed * hookshotSpeedMultiplier * Time.deltaTime);

        float reachedHookshotPositionDistance = 1f;
        if (Vector3.Distance(transform.position, hookshotPosition) < reachedHookshotPositionDistance)       
            // Llego a la posicion del hook y me suelto
            StopHookshot();
        
        if (_control.Hookshot())
            // Cancelar Hookshot mientras estoy volando volviendo 
            // a apretar Hookshot
            StopHookshot();
        
        if (_control.Jump())
        {
            // Cancelar Hookshot con salto
            float momentumExtraSpeed = 7f;
            characterVelocityMomentum = hookshotDir * hookshotSpeed * momentumExtraSpeed;
            float jumpSpeed = 40f;
            characterVelocityMomentum += Vector3.up * jumpSpeed;
            StopHookshot();

            impulseLinesParticleSystem.Play();

            AudioManager.instance.Play("Player Impulse");

        }
    }

    private void ResetGravityEffect()
    {
        //Fix - reseteo gravedad para que no se sume constantemente 
        //mientras no toque el suelo
        characterVelocityY = 0f;
    }

    private void StopHookshot() {
        state = State.Normal;
        ResetGravityEffect();
        hookshotTransform.gameObject.SetActive(false);
        cameraFov.SetCameraFov(NORMAL_FOV);
        speedLinesParticleSystem.Stop();
    }

    #endregion
    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, wallRunningMask);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, wallRunningMask);
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    void StartWallRun()
    {
        moveSpeed = wallRunningSpeed;
        ResetGravityEffect();
        //characterVelocityY -= Time.deltaTime;

        if (_control.Jump())
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + transform.forward + leftWallHit.normal;
                //ResetGravityEffect();
                
                characterVelocityMomentum += wallRunJumpDirection * wallJumpForce;
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + transform.forward + rightWallHit.normal;
                //ResetGravityEffect();
                
                characterVelocityMomentum += wallRunJumpDirection * wallJumpForce;
            }
           
        }
    }

    void StopWallRun()
    {
        moveSpeed = 20f;
    }

    #region Daño y Cura
    public void Damage(float damage) {
        health -= damage;
        AudioManager.instance.Play("Hit");
    
    }

    public void Heal(float heal)
    {
        health += heal;

        if (health >= 100f)
            health = 100f;
    }
    #endregion

    #region Estados del player
    private enum State
    {
        Normal,
        HookshotThrown,
        HookshotFly,
    }

    private void States()
    {
        switch (state)
        {
            default:
            case State.Normal:
                CharacterLook();
                CharacterMovement();
                HookshotStart();
                break;
            case State.HookshotThrown:
                HookshotThrow();
                CharacterLook();
                CharacterMovement();
                break;
            case State.HookshotFly:
                CharacterLook();
                HookshotMovement();
                break;
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        var button = other.GetComponent<Button>();

        if (button != null)
            button.TouchButton();
    }
}
