/// Original Code written and designed by Aeden C Graves.
///
///
/// CHANGE LOG:
///
/// DATE || msg: "" || Author Signature: SNG || version VERSION
///
/// 06/13/20 || msg: "Added Retro camera movement mode." || Author: Garrett Richards || version 20.6.8cp >> 20.6.13cu
/// 06/08/20 || msg: "Added ControllerPause function." || Author: Aedan Graves || version 20.6.2ff >> 20.6.8cp
/// 06/02/20 || msg: "Major fixes on slope detection. Fixed scaling issues. Fixed image grab errors." || Author: Aedan Graves || version 20.5.15b >> 20.6.2ff
/// 05/15/20 || msg: "Reinstated FOV kick controls under advanced movement settings." || Author: Aedan Graves || version 20.5.9b >> 20.5.15f
/// 05/14/20 || msg: "Fixed some slope detection bugs" || Author: Aedan Graves || version 20.5.9a >> 20.5.9b
/// 05/09/20 || msg: "Fixed slope/step detection registering triggers" || Author: Aedan Graves || version 20.4.22b >> 20.5.9a
/// 04/22/20 || msg: "Snap Rotation added to RotateCamera function. Added "Enable Audio/SFX" toggle and fixed static steps not working without headbobing enabled. || Aedan Graves || version 20.4.11qf >> 20.4.22b
/// 04/11/20 || msg: "Fixed controller slowly slipping down inclines." || Author: Aedan Graves || version 20.4.4qf >> 20.4.11qf
/// 04/04/20 || msg: "Fixed wall detection issues. Mouse Input Optimizations." || Author: Aedan Graves || version 20.3.26 >> 20.4.4qf
/// 26/03/20 || msg: "Fixed Stamina meter not draining. Fixed inconsistent jumping in Unity 2019.3+. Fixed Terrain not registering as ground. Added new Ground detection system using the capsule collider. Added RotateCamera Function" || Author Signature: Aedan Graves || version 20.3.8qf >> 20.3.26
/// 08/03/20 || msg: "Fixed draw stamina meter toggle throwing an error and disabling movement" || Author Signature: Aedan Graves || version 20.2.28cfu >> 20.3.8qf
/// 28/02/20 || msg: "ADDITIONS: 1) Added Mouse Inversion Options 2) Added Stair Detection (Experimental) UPDATES: 1) Removed InfiniGun compatibility features. 2) Converted Jump/Land movements to be intensity based. 3) Audio arrays now utilize Drag N' Drop. 4) Dynamic foot steps now are able to use more then one physic material as well as use normal materials (Experimental). FIXES 1) Fixed some variables not "Sticking" 2) Fixed phantom "_useFootStepSounds" flip-floping. 3) Fixed Jumping no longer working when scale is changed. 4) Fixed Slope Detection. 5) Fixed Crouch to properly move camera." || Author Signature: Aedan Graves || version: 19.9.21f >> 20.2.28cfu
/// 17/02/20 || msg: "Fixed input checking being done in FixedUpdate, causing inconsistent jumping and crouching." || Author Signature: Samuel Förström || version: 19.9.20f >> 19.9.21f 
/// 10/17/19 || msg: "Fixed inconsistent jumping/ground detection. Fixed headbobing axis amplification. Added toggle crouching." || Author Signature: Aedan Graves || version: 19.9.20f >> 19.10.17f 
/// 09/20/19 || msg: "Added support Email to the bottom of the inspector. Fixed issues with sticking to the walls. Removed the need for external assigning of a min and max friction material" || Author Signature: Aedan Graves || version: 19.9.13 >> 19.9.20f
/// 09/13/19 || msg: "New Editor script, Fixed Stamina, Fixed Crouching, Put 'FOV Kick' Under reconstruction, made dynamic foot steps easier to understand." || Author Signature: Aedan Graves || version: 19.7.28cu >> 19.9.13cu
/// 07/28/19 || msg: "Added function to effect mouse sensitivity based on the cameras FOV." || Author Signature: Aedan Graves || version: 19.6.7cu >> 19.7.28cu
/// 06/07/19 || msg: "Added ability to toggle the ability to jump from the editor." || Author Signature: Adam Worrell || 19.5.12feu >> version 19.6.7cu
/// 05/12/19 || msg: "Fixed non dynamic footsteping. Remade crouching system to be more efficient and added an input over ride. || Author Signature: Aedan Graves || version 19.3.22 cl >> 19.5.12feu
/// 03/22/19 || msg: "Cleaned up code" || Author Signature: Aedan Graves || version 19.3.19cu >> 19.3.22cl
/// 03/19/19 || msg: "Added a rudimentary slope detection system." || Author Signature: Aedan Graves || version 19.3.18a >> 19.3.19cu
/// 03/18/19 || msg: "Fixed Stamina" || Author Signature: Aedan Graves || version 19.3.11p >> 19.3.18a
/// 03/02/19 || msg: "Improved camera System" || Author Signature: Aedan Graves || version 19.3.2 >> 19.3.11p
/// 03/02/19 || msg: "Lowered maximum walk, sprint, and jump values" || Author Signature: Aedan Graves || version: 19.2.21 >> 19.3.2
/// 02/21/19 || msg: "Removed dynamic speed curve. Modified headbob logic || Author Signature: Aedan Graves || version: 19.2.15 >> 19.2.21
/// 02/15/19 || msg: "Added Camera shake. Made it possable to disable camera movement when jumping and landing." || Author Signature: Aedan Graves || version: 19.2.12 >> 19.2.15
/// 02/12/19 || msg: "Seperated Dynamic Footsteps from the Headbob calculations." || Author Signature: Aedan Graves || version: 1.6b >> 19.2.12
/// 02/08/19 || msg: "Added some more tooltips." || Author Signature: Aedan C Graves || version 1.6a >> 1.6b
/// 02/04/19 || msg: "Changed crouch function to use an In Editor defined input axis" || Author Signature: Aedan Graves || version 1.6 >> 1.6a
/// 12/13/18 || msg: "Added 'Custom' entry for Dynamic footstep system" || Author Signature: Aedan Graves || version 1.5b >> 1.6
/// 12/11/18 || msg: "Added Volume control to Footstep and Jump/land SFX." || Author Signature: Aedan Graves || version 1.5a >> 1.5b
/// 02/18/18 || msg: "Updated mouse rotation to allow pre-play rotation." || Author Signature: Aedan Graves || version 1.5 >> 1.5a
/// 01/31/18 || msg: "Changed Dynamic footstep system to use physics materials." || Author Signature: Aedan Graves || version 1.4c >> 1.5
/// 12/19/17 || msg: "Added headbob passthrough variables" || Author Signature: Aedan Graves || version 1.4b >> 1.4c
/// 12/02/17 || msg: "Made camera movement toggleable" || Author Signature: Aedan Graves || version 1.4a >> 1.4b
/// 10/16/17 || msg: "Made all sounds optional." || Author Signature: Aedan Graves || version 1.4 >> 1.4a
/// 10/09/17 || msg: "Added Optional FOV Kick" || Author Signature: Aedan Graves || version 1.3b >> 1.4
/// 10/08/17 || msg: "Improved Dynamic Footsteps." || Author Signature: Aedan Graves || version 1.3a >> 1.3b
/// 10/07/17 || msg: "BetaTesting Class" || Author Signature: Aedan Graves || version 1.3 >> 1.3a
/// 10/07/17 || msg: "Added Optional Dynamic Footsteps. Added optional Dynamic Speed Curve." || Author Signature: Aedan C Graves || version 1.2 >> 1.3
/// 10/03/17 || msg: "Added optional Crouch." || Author Signature: Aedan Graves || version v1.1 >> v1.2
/// 09/26/17 || msg: "Fixed Headbobbing in mid air. Added a option for head bobbing, Added optional Stamina. Added Auto Crosshair Feature." || Author Signature: Aedan Graves|| version v1.0 >> v1.1
/// 09/21/17 || msg: "Finished SMB FPS Logic." || Author Signature: Aedan Graves || version v0.0 >> v1.0
///
/// 
/// 
/// Made changes that you think should come "Out of the box"? E-mail the modified Script with A new entry on the top of the Change log to: modifiedassets@aedangraves.info

using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Net;
#endif

[RequireComponent(typeof(CapsuleCollider)),RequireComponent(typeof(Rigidbody)),AddComponentMenu("First Person AIO")]

public class FirstPersonAIO : MonoBehaviour {


    #region Variables

    #region Input Settings
    public bool controllerPauseState = false;
    #endregion

    #region Look Settings
    public bool enableCameraMovement = true;
    public enum InvertMouseInput{None,X,Y,Both}
    public InvertMouseInput mouseInputInversion = InvertMouseInput.None;
    public enum CameraInputMethod{Traditional, TraditionalWithConstraints, Retro}
    public CameraInputMethod cameraInputMethod =CameraInputMethod.Traditional;

    public float verticalRotationRange = 170;
    public float mouseSensitivity = 10;
    public  float   fOVToMouseSensitivity = 1;
    public float cameraSmoothing = 5f;
    public bool lockAndHideCursor = false;
    public Camera playerCamera;
    public bool enableCameraShake=false;
    internal Vector3 cameraStartingPosition;
    float baseCamFOV;
    

    public bool autoCrosshair = false;
    public bool drawStaminaMeter = true;
    float smoothRef;
    Image StaminaMeter;
    Image StaminaMeterBG;
    public Sprite Crosshair;
    public Vector3 targetAngles;
    private Vector3 followAngles;
    private Vector3 followVelocity;
    private Vector3 originalRotation;
    #endregion

    #region Movement Settings

    public bool playerCanMove = true;
    public bool walkByDefault = true;
    public float walkSpeed = 4f;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 8f;
    public float jumpPower = 5f;
    public bool canJump = true;
    public bool canHoldJump;
    bool jumpInput;
    bool didJump;
    public bool useStamina = true;
    public float staminaDepletionSpeed = 5f;
    public float staminaLevel = 50;
    public float speed;
    public float staminaInternal;
    internal float walkSpeedInternal;
    internal float sprintSpeedInternal;
    internal float jumpPowerInternal;

    [System.Serializable]
    public class CrouchModifiers {
        public bool useCrouch = true;
        public bool toggleCrouch = false;
        public KeyCode crouchKey = KeyCode.LeftControl;
        public float crouchWalkSpeedMultiplier = 0.5f;
        public float crouchJumpPowerMultiplier = 0f;
        public bool crouchOverride;
        internal float colliderHeight;
        
    }
    public CrouchModifiers _crouchModifiers = new CrouchModifiers();

    [System.Serializable]
    public class AdvancedSettings {
        public float gravityMultiplier = 1.0f;
        public PhysicMaterial zeroFrictionMaterial;
        public PhysicMaterial highFrictionMaterial;
        public float maxSlopeAngle = 55;
        internal bool isTouchingWalkable;
        internal bool isTouchingUpright;
        internal bool isTouchingFlat;
        public float maxWallShear = 89;
        public float maxStepHeight = 0.2f;
        internal bool stairMiniHop = false;
        public RaycastHit surfaceAngleCheck;
        public Vector3 curntGroundNormal;
        public Vector2 moveDirRef;
        public float lastKnownSlopeAngle;
        public float FOVKickAmount = 2.5f;
        public float changeTime = 0.75f;
        public float fovRef;
        
    }
    public AdvancedSettings advanced = new AdvancedSettings();
    private CapsuleCollider capsule;
    public bool IsGrounded { get; private set; }
    Vector2 inputXY;
    public bool isCrouching;
    float yVelocity;
    float checkedSlope;
    bool isSprinting = false;

    public Rigidbody fps_Rigidbody;

    #endregion

    #region Headbobbing Settings
    public bool useHeadbob = true;
    public Transform head = null;
    public bool snapHeadjointToCapsul = true;
    public float headbobFrequency = 1.5f;
    public float headbobSwayAngle = 5f;
    public float headbobHeight = 3f;
    public float headbobSideMovement =5f;  
    public float jumpLandIntensity =3f;
    private Vector3 originalLocalPosition;
    private float nextStepTime = 0.5f;
    private float headbobCycle = 0.0f;
    private float headbobFade = 0.0f;
    private float springPosition = 0.0f;
    private float springVelocity = 0.0f;
    private float springElastic = 1.1f;
    private float springDampen = 0.8f;
    private float springVelocityThreshold = 0.05f;
    private float springPositionThreshold = 0.05f;
    Vector3 previousPosition;
    Vector3 previousVelocity = Vector3.zero;
    Vector3 miscRefVel;
    bool previousGrounded;
    AudioSource audioSource;

    #endregion

    #region Audio Settings

    public bool enableAudioSFX = true;
    public float Volume = 5f;
    public AudioClip jumpSound = null;
    public AudioClip landSound = null;
    public List<AudioClip> footStepSounds = null;
    public enum FSMode{Static, Dynamic}
    public FSMode fsmode;
 
    [System.Serializable]
    public class DynamicFootStep{
        public enum matMode{physicMaterial,Material};
        public matMode materialMode;
        public List<PhysicMaterial> woodPhysMat;
        public List<PhysicMaterial> metalAndGlassPhysMat;
        public List<PhysicMaterial> grassPhysMat;
        public List<PhysicMaterial> dirtAndGravelPhysMat;
        public List<PhysicMaterial> rockAndConcretePhysMat;
        public List<PhysicMaterial> mudPhysMat;
        public List<PhysicMaterial> customPhysMat;

        public List<Material> woodMat;
        public List<Material> metalAndGlassMat;
        public List<Material> grassMat;
        public List<Material> dirtAndGravelMat;
        public List<Material> rockAndConcreteMat;
        public List<Material> mudMat;
        public List<Material> customMat;
        public List<AudioClip> currentClipSet;

        public List<AudioClip> woodClipSet;
        public List<AudioClip> metalAndGlassClipSet;
        public List<AudioClip> grassClipSet;
        public List<AudioClip> dirtAndGravelClipSet;
        public List<AudioClip> rockAndConcreteClipSet;
        public List<AudioClip> mudClipSet;
        public List<AudioClip> customClipSet;
    }
    public DynamicFootStep dynamicFootstep = new DynamicFootStep();

    #endregion

    #endregion

    private void Awake(){
        #region Look Settings - Awake
        originalRotation = transform.localRotation.eulerAngles;

        #endregion 

        #region Movement Settings - Awake
        walkSpeedInternal = walkSpeed;
        sprintSpeedInternal = sprintSpeed;
        jumpPowerInternal = jumpPower;
        capsule = GetComponent<CapsuleCollider>();
        IsGrounded = true;
        isCrouching = false;
        fps_Rigidbody = GetComponent<Rigidbody>();
        fps_Rigidbody.interpolation = RigidbodyInterpolation.Extrapolate;
        fps_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _crouchModifiers.colliderHeight = capsule.height;
        #endregion

        #region Headbobbing Settings - Awake

        #endregion

    }

    private void Start(){
        #region Look Settings - Start

        if(autoCrosshair || drawStaminaMeter){
            Canvas canvas = new GameObject("AutoCrosshair").AddComponent<Canvas>();
            canvas.gameObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.pixelPerfect = true;
            canvas.transform.SetParent(playerCamera.transform);
            canvas.transform.position = Vector3.zero;

            if(autoCrosshair){
                Image crossHair = new GameObject("Crosshair").AddComponent<Image>();
                crossHair.sprite = Crosshair;
                crossHair.rectTransform.sizeDelta = new Vector2(25,25);
                crossHair.transform.SetParent(canvas.transform);
                crossHair.transform.position = Vector3.zero;
            }

            if(drawStaminaMeter){
                StaminaMeterBG = new GameObject("StaminaMeter").AddComponent<Image>();
                StaminaMeter = new GameObject("Meter").AddComponent<Image>();
                StaminaMeter.transform.SetParent(StaminaMeterBG.transform);
                StaminaMeterBG.transform.SetParent(canvas.transform);
                StaminaMeterBG.transform.position = Vector3.zero;
                StaminaMeterBG.rectTransform.anchorMax = new Vector2(0.5f,0);
                StaminaMeterBG.rectTransform.anchorMin = new Vector2(0.5f,0);
                StaminaMeterBG.rectTransform.anchoredPosition = new Vector2(0,15);
                StaminaMeterBG.rectTransform.sizeDelta = new Vector2(250,6);
                StaminaMeterBG.color = new Color(0,0,0,0);
                StaminaMeter.rectTransform.sizeDelta = new Vector2(250,6);
                StaminaMeter.color = new Color(0,0,0,0);
            }
        }
        cameraStartingPosition = playerCamera.transform.localPosition;
        if(lockAndHideCursor) { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        baseCamFOV = playerCamera.fieldOfView;
        #endregion

        #region Movement Settings - Start  
        capsule.radius = capsule.height/4;
        staminaInternal = staminaLevel;
        advanced.zeroFrictionMaterial = new PhysicMaterial("Zero_Friction");
        advanced.zeroFrictionMaterial.dynamicFriction =0;
        advanced.zeroFrictionMaterial.staticFriction =0;
        advanced.zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        advanced.zeroFrictionMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
        advanced.highFrictionMaterial = new PhysicMaterial("Max_Friction");
        advanced.highFrictionMaterial.dynamicFriction =1;
        advanced.highFrictionMaterial.staticFriction =1;
        advanced.highFrictionMaterial.frictionCombine = PhysicMaterialCombine.Maximum;
        advanced.highFrictionMaterial.bounceCombine = PhysicMaterialCombine.Average;
        #endregion

        #region Headbobbing Settings - Start
        
        originalLocalPosition = snapHeadjointToCapsul ? new Vector3(head.localPosition.x, (capsule.height/2)*head.localScale.y ,head.localPosition.z) : head.localPosition;
        if(GetComponent<AudioSource>() == null) { gameObject.AddComponent<AudioSource>(); }

        previousPosition = fps_Rigidbody.position;
        audioSource = GetComponent<AudioSource>();
        #endregion
    }

    private void Update(){

        #region Look Settings - Update

            if(enableCameraMovement && !controllerPauseState){
            float mouseYInput = 0;
            float mouseXInput = 0;
            float camFOV = playerCamera.fieldOfView;
            if (cameraInputMethod == CameraInputMethod.Traditional || cameraInputMethod == CameraInputMethod.TraditionalWithConstraints){
                    mouseYInput = mouseInputInversion == InvertMouseInput.None || mouseInputInversion == InvertMouseInput.X ? Input.GetAxis("Mouse Y") : -Input.GetAxis("Mouse Y");
                    mouseXInput = mouseInputInversion == InvertMouseInput.None || mouseInputInversion == InvertMouseInput.Y ? Input.GetAxis("Mouse X") : -Input.GetAxis("Mouse X");
            }
            else{
                mouseXInput= Input.GetAxis("Horizontal") * (mouseInputInversion == InvertMouseInput.None || mouseInputInversion == InvertMouseInput.Y ? 1 : -1);
            }            if(targetAngles.y > 180) { targetAngles.y -= 360; followAngles.y -= 360; } else if(targetAngles.y < -180) { targetAngles.y += 360; followAngles.y += 360; }
            if(targetAngles.x > 180) { targetAngles.x -= 360; followAngles.x -= 360; } else if(targetAngles.x < -180) { targetAngles.x += 360; followAngles.x += 360; }
            targetAngles.y += mouseXInput * (mouseSensitivity - ((baseCamFOV-camFOV)*fOVToMouseSensitivity)/6f);
            if (cameraInputMethod == CameraInputMethod.Traditional){ targetAngles.x += mouseYInput * (mouseSensitivity - ((baseCamFOV - camFOV) * fOVToMouseSensitivity) / 6f);}
            else {targetAngles.x = 0f;}
            targetAngles.x = Mathf.Clamp(targetAngles.x, -0.5f * verticalRotationRange, 0.5f * verticalRotationRange);
            followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, (cameraSmoothing)/100);
            
            playerCamera.transform.localRotation = Quaternion.Euler(-followAngles.x + originalRotation.x,0,0);
            transform.localRotation =  Quaternion.Euler(0, followAngles.y+originalRotation.y, 0);
        }
    
        #endregion

        #region Input Settings - Update
        if(canHoldJump ? (canJump && Input.GetButton("Jump")) : (Input.GetButtonDown("Jump") && canJump) ){
            jumpInput = true;
        }else if(Input.GetButtonUp("Jump")){jumpInput = false;}
        
        
        if(_crouchModifiers.useCrouch){
            if(!_crouchModifiers.toggleCrouch){ isCrouching = _crouchModifiers.crouchOverride || Input.GetKey(_crouchModifiers.crouchKey);}
            else if(Input.GetKeyDown(_crouchModifiers.crouchKey)){isCrouching = !isCrouching || _crouchModifiers.crouchOverride;}
            }

        if(Input.GetButtonDown("Cancel")){ControllerPause();}
        #endregion

        #region Movement Settings - Update
        
        #endregion

        #region Headbobbing Settings - Update

        #endregion

    }

    private void FixedUpdate(){

        #region Look Settings - FixedUpdate

        #endregion

        #region Movement Settings - FixedUpdate
        
        if(useStamina){
            isSprinting = Input.GetKey(sprintKey) && !isCrouching && staminaInternal > 0 && (Mathf.Abs(fps_Rigidbody.velocity.x) > 0.01f || Mathf.Abs(fps_Rigidbody.velocity.z) > 0.01f);
            if(isSprinting){
                staminaInternal -= (staminaDepletionSpeed*2)*Time.deltaTime;
                if(drawStaminaMeter){
                    StaminaMeterBG.color = Vector4.MoveTowards(StaminaMeterBG.color, new Vector4(0,0,0,0.5f),0.15f);
                    StaminaMeter.color = Vector4.MoveTowards(StaminaMeter.color, new Vector4(1,1,1,1),0.15f);
                }
            }else if((!Input.GetKey(sprintKey)||Mathf.Abs(fps_Rigidbody.velocity.x)< 0.01f || Mathf.Abs(fps_Rigidbody.velocity.z)< 0.01f || isCrouching)&&staminaInternal<staminaLevel){
                staminaInternal += staminaDepletionSpeed*Time.deltaTime;
            }
                if(drawStaminaMeter){
                   if(staminaInternal==staminaLevel){ StaminaMeterBG.color = Vector4.MoveTowards(StaminaMeterBG.color, new Vector4(0,0,0,0),0.15f);
                    StaminaMeter.color = Vector4.MoveTowards(StaminaMeter.color, new Vector4(1,1,1,0),0.15f);}
                    float x = Mathf.Clamp(Mathf.SmoothDamp(StaminaMeter.transform.localScale.x,(staminaInternal/staminaLevel)*StaminaMeterBG.transform.localScale.x,ref smoothRef,(1)*Time.deltaTime,1),0.001f, StaminaMeterBG.transform.localScale.x);
                    StaminaMeter.transform.localScale = new Vector3(x,1,1); 
                }
                staminaInternal = Mathf.Clamp(staminaInternal,0,staminaLevel);
        } else{isSprinting = Input.GetKey(sprintKey);}

        Vector3 MoveDirection = Vector3.zero;
        speed = walkByDefault ? isCrouching ? walkSpeedInternal : (isSprinting ? sprintSpeedInternal : walkSpeedInternal) : (isSprinting ? walkSpeedInternal : sprintSpeedInternal);
  

        if(advanced.maxSlopeAngle>0){
            if(advanced.isTouchingUpright && advanced.isTouchingWalkable){

                MoveDirection = (transform.forward * inputXY.y * speed + transform.right * inputXY.x * walkSpeedInternal); 
                if(!didJump){fps_Rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;}              
                }
                else if(advanced.isTouchingUpright && !advanced.isTouchingWalkable){
                    fps_Rigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                }
                
                else{
                    
                fps_Rigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                MoveDirection = ((transform.forward * inputXY.y * speed + transform.right * inputXY.x * walkSpeedInternal) * (fps_Rigidbody.velocity.y>0.01f ? SlopeCheck() : 0.8f));
                }
        }
        else{
        MoveDirection = (transform.forward * inputXY.y * speed + transform.right * inputXY.x * walkSpeedInternal);
        }

        
            #region step logic
                RaycastHit WT;
                if(advanced.maxStepHeight > 0 && Physics.Raycast(transform.position - new Vector3(0,((capsule.height/2)*transform.localScale.y)-0.01f,0),MoveDirection,out WT,capsule.radius+0.15f,Physics.AllLayers,QueryTriggerInteraction.Ignore) && Vector3.Angle(WT.normal, Vector3.up)>88){
                    RaycastHit ST;
                    if(!Physics.Raycast(transform.position - new Vector3(0,((capsule.height/2)*transform.localScale.y)-(advanced.maxStepHeight),0),MoveDirection,out ST,capsule.radius+0.25f,Physics.AllLayers,QueryTriggerInteraction.Ignore)){
                        advanced.stairMiniHop = true;
                        transform.position += new Vector3(0,advanced.maxStepHeight*1.2f,0);
                    }
                }
                Debug.DrawRay(transform.position, MoveDirection,Color.red,0,false);
            #endregion
            
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        inputXY = new Vector2(horizontalInput, verticalInput);
        if(inputXY.magnitude > 1) { inputXY.Normalize(); }

            #region Jump
            yVelocity = fps_Rigidbody.velocity.y;
            
            if(IsGrounded && jumpInput && jumpPowerInternal > 0 && !didJump){
                if(advanced.maxSlopeAngle>0){
                    if(advanced.isTouchingFlat || advanced.isTouchingWalkable){
                            didJump=true;
                            jumpInput=false;
                            yVelocity += fps_Rigidbody.velocity.y<0.01f? jumpPowerInternal : jumpPowerInternal/3;
                            advanced.isTouchingWalkable = false;
                            advanced.isTouchingFlat = false;
                            advanced.isTouchingUpright = false;
                            fps_Rigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
                    }
                    
                }else{
                    didJump=true;
                    jumpInput=false;
                    yVelocity += jumpPowerInternal;
                }
        
            }

            if(advanced.maxSlopeAngle>0){
            
            
            if(!didJump && advanced.lastKnownSlopeAngle>5 && advanced.isTouchingWalkable){
            yVelocity *= SlopeCheck()/4;
            }
            if(advanced.isTouchingUpright && !advanced.isTouchingWalkable && !didJump){
                yVelocity +=  Physics.gravity.y;
            }
        }

            #endregion

        if(playerCanMove && !controllerPauseState){
          fps_Rigidbody.velocity = MoveDirection+(Vector3.up * yVelocity);

        } else{fps_Rigidbody.velocity = Vector3.zero;}

        if(inputXY.magnitude > 0 || !IsGrounded) {
            capsule.sharedMaterial = advanced.zeroFrictionMaterial;
        } else { capsule.sharedMaterial = advanced.highFrictionMaterial; }
  
        fps_Rigidbody.AddForce(Physics.gravity * (advanced.gravityMultiplier - 1));
        
        
        if(advanced.FOVKickAmount>0){
            if(isSprinting && !isCrouching && playerCamera.fieldOfView != (baseCamFOV+(advanced.FOVKickAmount*2)-0.01f)){
                if(Mathf.Abs(fps_Rigidbody.velocity.x)> 0.5f || Mathf.Abs(fps_Rigidbody.velocity.z)> 0.5f){
                    playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView,baseCamFOV+(advanced.FOVKickAmount*2),ref advanced.fovRef,advanced.changeTime);
                    }
                
            }
            else if(playerCamera.fieldOfView != baseCamFOV){ playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView,baseCamFOV,ref advanced.fovRef,advanced.changeTime*0.5f);}
            
        }

        if(_crouchModifiers.useCrouch) {
            
            if(isCrouching) {
                    capsule.height = Mathf.MoveTowards(capsule.height, _crouchModifiers.colliderHeight/1.5f, 5*Time.deltaTime);
                        walkSpeedInternal = walkSpeed*_crouchModifiers.crouchWalkSpeedMultiplier;
                        jumpPowerInternal = jumpPower* _crouchModifiers.crouchJumpPowerMultiplier;

                } else {
                capsule.height = Mathf.MoveTowards(capsule.height, _crouchModifiers.colliderHeight, 5*Time.deltaTime);    
                walkSpeedInternal = walkSpeed;
                sprintSpeedInternal = sprintSpeed;
                jumpPowerInternal = jumpPower;
            }
        }




        #endregion

        #region Headbobbing Settings - FixedUpdate
        float yPos = 0;
        float xPos = 0;
        float zTilt = 0;
        float xTilt = 0;
        float bobSwayFactor = 0;
        float bobFactor = 0;
        float strideLangthen = 0;
        float flatVel = 0;
        //calculate headbob freq
        if(useHeadbob == true || enableAudioSFX){
            Vector3 vel = (fps_Rigidbody.position - previousPosition) / Time.deltaTime;
            Vector3 velChange = vel - previousVelocity;
            previousPosition = fps_Rigidbody.position;
            previousVelocity = vel;
            springVelocity -= velChange.y;
            springVelocity -= springPosition * springElastic;
            springVelocity *= springDampen;
            springPosition += springVelocity * Time.deltaTime;
            springPosition = Mathf.Clamp(springPosition, -0.3f, 0.3f);

            if(Mathf.Abs(springVelocity) < springVelocityThreshold && Mathf.Abs(springPosition) < springPositionThreshold) { springPosition = 0; springVelocity = 0; }
            flatVel = new Vector3(vel.x, 0.0f, vel.z).magnitude;
            strideLangthen = 1 + (flatVel * ((headbobFrequency*2)/10));
            headbobCycle += (flatVel / strideLangthen) * (Time.deltaTime / headbobFrequency);
            bobFactor = Mathf.Sin(headbobCycle * Mathf.PI * 2);
            bobSwayFactor = Mathf.Sin(Mathf.PI * (2 * headbobCycle + 0.5f));
            bobFactor = 1 - (bobFactor * 0.5f + 1);
            bobFactor *= bobFactor;

            yPos = 0;
            xPos = 0;
            zTilt = 0;
            if(jumpLandIntensity>0 && !advanced.stairMiniHop){xTilt = -springPosition * (jumpLandIntensity*5.5f);}
            else if(!advanced.stairMiniHop){xTilt = -springPosition;}

            if(IsGrounded){
                if(new Vector3(vel.x, 0.0f, vel.z).magnitude < 0.1f) { headbobFade = Mathf.MoveTowards(headbobFade, 0.0f,0.5f); } else { headbobFade = Mathf.MoveTowards(headbobFade, 1.0f, Time.deltaTime); }
                float speedHeightFactor = 1 + (flatVel * 0.3f);
                xPos = -(headbobSideMovement/10) * headbobFade *bobSwayFactor;
                yPos = springPosition * (jumpLandIntensity/10) + bobFactor * (headbobHeight/10) * headbobFade * speedHeightFactor;
                zTilt = bobSwayFactor * (headbobSwayAngle/10) * headbobFade;
            }
        }
        //apply headbob position
            if(useHeadbob == true){
                if(fps_Rigidbody.velocity.magnitude >0.1f){
                    head.localPosition = Vector3.MoveTowards(head.localPosition, snapHeadjointToCapsul ? (new Vector3(originalLocalPosition.x,(capsule.height/2)*head.localScale.y,originalLocalPosition.z)  + new Vector3(xPos, yPos, 0)) : originalLocalPosition + new Vector3(xPos, yPos, 0),0.5f);
                }else{
                    head.localPosition = Vector3.SmoothDamp(head.localPosition, snapHeadjointToCapsul ? (new Vector3(originalLocalPosition.x,(capsule.height/2)*head.localScale.y,originalLocalPosition.z)  + new Vector3(xPos, yPos, 0)) : originalLocalPosition + new Vector3(xPos, yPos, 0),ref miscRefVel, 0.15f);
                }
                head.localRotation = Quaternion.Euler(xTilt, 0, zTilt);
                
           
        }
        #endregion
        
        #region Dynamic Footsteps
        if(enableAudioSFX){    
            if(fsmode == FSMode.Dynamic)
            {   
                RaycastHit hit = new RaycastHit();

                if(Physics.Raycast(transform.position, Vector3.down, out hit)){
                     
                    if(dynamicFootstep.materialMode == DynamicFootStep.matMode.physicMaterial){
                        dynamicFootstep.currentClipSet = (dynamicFootstep.woodPhysMat.Any() && dynamicFootstep.woodPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.woodClipSet.Any()) ? // If standing on Wood
                        dynamicFootstep.woodClipSet : ((dynamicFootstep.grassPhysMat.Any() && dynamicFootstep.grassPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.grassClipSet.Any()) ? // If standing on Grass
                        dynamicFootstep.grassClipSet : ((dynamicFootstep.metalAndGlassPhysMat.Any() && dynamicFootstep.metalAndGlassPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.metalAndGlassClipSet.Any()) ? // If standing on Metal/Glass
                        dynamicFootstep.metalAndGlassClipSet : ((dynamicFootstep.rockAndConcretePhysMat.Any() && dynamicFootstep.rockAndConcretePhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.rockAndConcreteClipSet.Any()) ? // If standing on Rock/Concrete
                        dynamicFootstep.rockAndConcreteClipSet : ((dynamicFootstep.dirtAndGravelPhysMat.Any() && dynamicFootstep.dirtAndGravelPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.dirtAndGravelClipSet.Any()) ? // If standing on Dirt/Gravle
                        dynamicFootstep.dirtAndGravelClipSet : ((dynamicFootstep.mudPhysMat.Any() && dynamicFootstep.mudPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.mudClipSet.Any())? // If standing on Mud
                        dynamicFootstep.mudClipSet : ((dynamicFootstep.customPhysMat.Any() && dynamicFootstep.customPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.customClipSet.Any())? // If standing on the custom material 
                        dynamicFootstep.customClipSet : footStepSounds)))))); // If material is unknown, fall back
                    }else if (hit.collider.GetComponent<MeshRenderer>()){
                        dynamicFootstep.currentClipSet = (dynamicFootstep.woodMat.Any() && dynamicFootstep.woodMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.woodClipSet.Any()) ? // If standing on Wood
                        dynamicFootstep.woodClipSet : ((dynamicFootstep.grassMat.Any() && dynamicFootstep.grassMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.grassClipSet.Any()) ? // If standing on Grass
                        dynamicFootstep.grassClipSet : ((dynamicFootstep.metalAndGlassMat.Any() && dynamicFootstep.metalAndGlassMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.metalAndGlassClipSet.Any()) ? // If standing on Metal/Glass
                        dynamicFootstep.metalAndGlassClipSet : ((dynamicFootstep.rockAndConcreteMat.Any() && dynamicFootstep.rockAndConcreteMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.rockAndConcreteClipSet.Any()) ? // If standing on Rock/Concrete
                        dynamicFootstep.rockAndConcreteClipSet : ((dynamicFootstep.dirtAndGravelMat.Any() && dynamicFootstep.dirtAndGravelMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.dirtAndGravelClipSet.Any()) ? // If standing on Dirt/Gravle
                        dynamicFootstep.dirtAndGravelClipSet : ((dynamicFootstep.mudMat.Any() && dynamicFootstep.mudMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.mudClipSet.Any())? // If standing on Mud
                        dynamicFootstep.mudClipSet : ((dynamicFootstep.customMat.Any() && dynamicFootstep.customMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.customClipSet.Any())? // If standing on the custom material 
                        dynamicFootstep.customClipSet : footStepSounds.Any() ? footStepSounds : null)))))); // If material is unknown, fall back
                    }

                    if(IsGrounded)
                    {
                        if(!previousGrounded)
                        {
                            if(dynamicFootstep.currentClipSet.Any()) { audioSource.PlayOneShot(dynamicFootstep.currentClipSet[Random.Range(0, dynamicFootstep.currentClipSet.Count)],Volume/10); }
                            nextStepTime = headbobCycle + 0.5f;
                        } else
                        {
                            if(headbobCycle > nextStepTime)
                            {
                                nextStepTime = headbobCycle + 0.5f;
                                if(dynamicFootstep.currentClipSet.Any()){ audioSource.PlayOneShot(dynamicFootstep.currentClipSet[Random.Range(0, dynamicFootstep.currentClipSet.Count)],Volume/10); }
                            }
                        }
                        previousGrounded = true;
                    } else
                    {
                        if(previousGrounded)
                        {
                            if(dynamicFootstep.currentClipSet.Any()){ audioSource.PlayOneShot(dynamicFootstep.currentClipSet[Random.Range(0, dynamicFootstep.currentClipSet.Count)],Volume/10); }
                        }
                        previousGrounded = false;
                    }

                } else {
                    dynamicFootstep.currentClipSet = footStepSounds;
                    if(IsGrounded)
                    {
                        if(!previousGrounded)
                        {
                            if(landSound){ audioSource.PlayOneShot(landSound,Volume/10); }
                            nextStepTime = headbobCycle + 0.5f;
                        } else
                        {
                            if(headbobCycle > nextStepTime)
                            {
                                nextStepTime = headbobCycle + 0.5f;
                                int n = Random.Range(0, footStepSounds.Count);
                                if(footStepSounds.Any()){ audioSource.PlayOneShot(footStepSounds[n],Volume/10); }
                                footStepSounds[n] = footStepSounds[0];
                            }
                        }
                        previousGrounded = true;
                    } else
                    {
                        if(previousGrounded)
                        {
                            if(jumpSound){ audioSource.PlayOneShot(jumpSound,Volume/10); }
                        }
                        previousGrounded = false;
                    }
                }
                
            } else
            {
                if(IsGrounded)
                {
                    if(!previousGrounded)
                    {
                        if(landSound) { audioSource.PlayOneShot(landSound,Volume/10); }
                        nextStepTime = headbobCycle + 0.5f;
                    } else
                    {
                        if(headbobCycle > nextStepTime)
                        {
                            nextStepTime = headbobCycle + 0.5f;
                            int n = Random.Range(0, footStepSounds.Count);
                            if(footStepSounds.Any() && footStepSounds[n] != null){ audioSource.PlayOneShot(footStepSounds[n],Volume/10);}
                            
                        }
                    }
                    previousGrounded = true;
                } else
                {
                    if(previousGrounded)
                    {
                        if(jumpSound) { audioSource.PlayOneShot(jumpSound,Volume/10); }
                    }
                    previousGrounded = false;
                }
            }

        }
        #endregion

        #region  Reset Checks

        IsGrounded = false;
        
        if(advanced.maxSlopeAngle>0){
            if(advanced.isTouchingFlat || advanced.isTouchingWalkable || advanced.isTouchingUpright){didJump = false;}
            advanced.isTouchingWalkable = false;
            advanced.isTouchingUpright = false;
            advanced.isTouchingFlat = false;
        }
        #endregion
    }

 

    public IEnumerator CameraShake(float Duration, float Magnitude){
        float elapsed =0;
        while(elapsed<Duration && enableCameraShake){
            playerCamera.transform.localPosition =Vector3.MoveTowards(playerCamera.transform.localPosition, new Vector3(cameraStartingPosition.x+ Random.Range(-1,1)*Magnitude,cameraStartingPosition.y+Random.Range(-1,1)*Magnitude,cameraStartingPosition.z), Magnitude*2);
            yield return new WaitForSecondsRealtime(0.001f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        playerCamera.transform.localPosition = cameraStartingPosition;
    }

    public void RotateCamera(Vector2 Rotation, bool Snap){
        enableCameraMovement = !enableCameraMovement;
        if(Snap){followAngles = Rotation;targetAngles = Rotation;}else{targetAngles = Rotation;}
        enableCameraMovement = !enableCameraMovement;
    }

    public void ControllerPause(){
        controllerPauseState = !controllerPauseState;
        if(lockAndHideCursor){
            Cursor.lockState = controllerPauseState? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = controllerPauseState;
        }
    }



    float SlopeCheck(){
        
            advanced.lastKnownSlopeAngle = Mathf.MoveTowards(advanced.lastKnownSlopeAngle, Vector3.Angle(advanced.curntGroundNormal, Vector3.up),5f);
            
            return new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(advanced.maxSlopeAngle+15, 0f),new Keyframe(advanced.maxWallShear, 0.0f),new Keyframe(advanced.maxWallShear+0.1f, 1.0f),new Keyframe(90, 1.0f)){preWrapMode = WrapMode.Clamp, postWrapMode = WrapMode.ClampForever}.Evaluate(advanced.lastKnownSlopeAngle);
          
    }



    private void OnCollisionEnter(Collision CollisionData){
        for(int i = 0; i<CollisionData.contactCount; i++){
                float a = Vector3.Angle(CollisionData.GetContact(i).normal, Vector3.up);
                if(CollisionData.GetContact(i).point.y <  transform.position.y - ((capsule.height/2) - capsule.radius*0.95f)){
                    
                    if(!IsGrounded){
                        IsGrounded = true;
                        advanced.stairMiniHop = false;
                        if(didJump && a <= 70){didJump = false;}
                    }

                    if(advanced.maxSlopeAngle>0){
                        if(a<5.1f){advanced.isTouchingFlat = true; advanced.isTouchingWalkable = true;}
                        else if(a<advanced.maxSlopeAngle+0.1f){advanced.isTouchingWalkable = true; /* IsGrounded = true; */}
                        else if(a<90){advanced.isTouchingUpright = true;}
                        
                        advanced.curntGroundNormal = CollisionData.GetContact(i).normal;
                    }
                    
            }
        }
    }
    private void OnCollisionStay(Collision CollisionData) {
        
            for(int i = 0; i<CollisionData.contactCount; i++){
                float a = Vector3.Angle(CollisionData.GetContact(i).normal, Vector3.up);
                if(CollisionData.GetContact(i).point.y <  transform.position.y - ((capsule.height/2) - capsule.radius*0.95f)){
                        
                    if(!IsGrounded){
                        IsGrounded = true;
                        advanced.stairMiniHop = false;
                    }

                    if(advanced.maxSlopeAngle>0){
                        if(a<5.1f){advanced.isTouchingFlat = true; advanced.isTouchingWalkable = true;}
                        else if(a<advanced.maxSlopeAngle+0.1f){advanced.isTouchingWalkable = true; /* IsGrounded = true; */}
                        else if(a<90){advanced.isTouchingUpright = true;}
                        
                        advanced.curntGroundNormal = CollisionData.GetContact(i).normal;
                    }
                    
            }
        }
    }
    private void OnCollisionExit(Collision CollisionData) {
        IsGrounded = false;
        if(advanced.maxSlopeAngle>0){advanced.curntGroundNormal = Vector3.up; advanced.lastKnownSlopeAngle = 0; advanced.isTouchingWalkable = false; advanced.isTouchingUpright = false;}

    }


}

#if UNITY_EDITOR
    [CustomEditor(typeof(FirstPersonAIO)),InitializeOnLoadAttribute]
    public class FPAIO_Editor : Editor{
        
        FirstPersonAIO t;
        SerializedObject SerT;
        static bool showCrouchMods = false;
        static bool showAdvanced = false;
        static bool showStaticFS = false;
        
        /* static bool viewFuncSnipets=false;
        static bool showCameraShakeSnip=false;
        static bool showRotateCamSnip = false;
        static bool showControllerPauesSnip = false; */

        SerializedProperty staticFS;

        static bool showWoodFS = false;
        SerializedProperty woodFS;
        SerializedProperty woodMat;
        SerializedProperty woodPhysMat;

        static bool showMetalFS = false;
        SerializedProperty metalFS;
        SerializedProperty metalAndGlassMat;
        SerializedProperty metalAndGlassPhysMat;

        static bool showGrassFS = false;
        SerializedProperty grassFS;
        SerializedProperty grassMat;
        SerializedProperty grassPhysMat;

        static bool showDirtFS = false;
        SerializedProperty dirtFS;
        SerializedProperty dirtAndGravelMat;
        SerializedProperty dirtAndGravelPhysMat;

        static bool showConcreteFS = false;
        SerializedProperty concreteFS;
        SerializedProperty rockAndConcreteMat;
        SerializedProperty rockAndConcretePhysMat;

        static bool showMudFS = false;
        SerializedProperty mudFS;
        SerializedProperty mudMat;
        SerializedProperty mudPhysMat;

        static bool showCustomFS = false;
        SerializedProperty customFS;
        SerializedProperty customMat;
        SerializedProperty customPhysMat;

        static Texture2D adTex1;
        bool loadedAds = false;

        string versionNum = "20.6.13cu";
        void OnEnable(){
            
            t = (FirstPersonAIO)target;
            loadedAds = false;
            SerT = new SerializedObject(t);
            staticFS = SerT.FindProperty("footStepSounds");
            
            woodFS = SerT.FindProperty("dynamicFootstep.woodClipSet");
            woodMat = SerT.FindProperty("dynamicFootstep.woodMat");
            woodPhysMat = SerT.FindProperty("dynamicFootstep.woodPhysMat");

            metalFS = SerT.FindProperty("dynamicFootstep.metalAndGlassClipSet");
            metalAndGlassMat = SerT.FindProperty("dynamicFootstep.metalAndGlassMat");
            metalAndGlassPhysMat = SerT.FindProperty("dynamicFootstep.metalAndGlassPhysMat");

            grassFS = SerT.FindProperty("dynamicFootstep.grassClipSet");
            grassMat = SerT.FindProperty("dynamicFootstep.grassMat");
            grassPhysMat = SerT.FindProperty("dynamicFootstep.grassPhysMat");

            dirtFS = SerT.FindProperty("dynamicFootstep.dirtAndGravelClipSet");
            dirtAndGravelMat = SerT.FindProperty("dynamicFootstep.dirtAndGravelMat");
            dirtAndGravelPhysMat = SerT.FindProperty("dynamicFootstep.dirtAndGravelPhysMat");
            
            concreteFS = SerT.FindProperty("dynamicFootstep.rockAndConcreteClipSet");
            rockAndConcreteMat = SerT.FindProperty("dynamicFootstep.rockAndConcreteMat");
            rockAndConcretePhysMat = SerT.FindProperty("dynamicFootstep.rockAndConcretePhysMat");

            mudFS = SerT.FindProperty("dynamicFootstep.mudClipSet");
            mudMat = SerT.FindProperty("dynamicFootstep.mudMat");
            mudPhysMat = SerT.FindProperty("dynamicFootstep.mudPhysMat");

            customFS = SerT.FindProperty("dynamicFootstep.customClipSet");
            customMat = SerT.FindProperty("dynamicFootstep.customMat");
            customPhysMat = SerT.FindProperty("dynamicFootstep.customPhysMat");

        }   
        public override void OnInspectorGUI(){
            if(t.transform.localScale!=Vector3.one){
                t.transform.localScale = Vector3.one;
                Debug.LogWarning("Scale needs to be (1,1,1)! \n Please scale controller via Capsule collider height/raduis.");
            }
            SerT.Update();
            EditorGUILayout.Space();

            GUILayout.Label("First Person AIO",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16});
            GUILayout.Label("version: "+ versionNum,new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter});
            EditorGUILayout.Space();

            if(t.controllerPauseState){GUILayout.Label("<b><color=#B40404>Controller Paused</color></b>",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, richText = true, fontSize = 16});}

        #region Camera Setup
            EditorGUILayout.LabelField("",GUI.skin.horizontalSlider);
            GUILayout.Label("Camera Setup",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter,fontStyle = FontStyle.Bold, fontSize = 13},GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            t.enableCameraMovement = EditorGUILayout.ToggleLeft(new GUIContent("Enable Camera Movement","Determines whether the player can move camera or not."),t.enableCameraMovement);
            EditorGUILayout.Space();
            GUI.enabled = t.enableCameraMovement;
            t.cameraInputMethod = (FirstPersonAIO.CameraInputMethod)EditorGUILayout.EnumPopup(new GUIContent("Input Method", "Determines the method used to rotate camera. \n\nTraditional uses the mouse on all axes. \nTraditional with constraints uses the mouse on the Y axis only. \nRetro uses Keybinds (left and right movement keys) to rotate the camera along the Y axis."),t.cameraInputMethod);
            if(t.cameraInputMethod == FirstPersonAIO.CameraInputMethod.Traditional){t.verticalRotationRange = EditorGUILayout.Slider(new GUIContent("Vertical Rotation Range","Determines how much range does the camera have to move vertically."),t.verticalRotationRange,90,180);}
            if(t.cameraInputMethod == FirstPersonAIO.CameraInputMethod.Traditional || t.cameraInputMethod == FirstPersonAIO.CameraInputMethod.TraditionalWithConstraints){
            t.mouseInputInversion = (FirstPersonAIO.InvertMouseInput)EditorGUILayout.EnumPopup(new GUIContent("Mouse Input Inversion","Determines if mouse input should be inverted, and along which axes"),t.mouseInputInversion);
            t.mouseSensitivity = EditorGUILayout.Slider(new GUIContent("Mouse Sensitivity","Determines how sensitive the mouse is."),t.mouseSensitivity, 1,15);
            t.fOVToMouseSensitivity = EditorGUILayout.Slider(new GUIContent("FOV to Mouse Sensitivity","Determines how much the camera's Field Of View will effect the mouse sensitivity. \n\n0 = no effect, 1 = full effect on sensitivity."),t.fOVToMouseSensitivity,0,1);
            }else{
                t.mouseSensitivity = EditorGUILayout.Slider(new GUIContent("Rotation Speed","Determines how fast the camera spins when turning the camera."),t.mouseSensitivity, 1,15);
            }            t.cameraSmoothing = EditorGUILayout.Slider(new GUIContent("Camera Smoothing","Determines how smooth the camera movement is."),t.cameraSmoothing,1,25);
            t.playerCamera = (Camera)EditorGUILayout.ObjectField(new GUIContent("Player Camera", "Camera attached to this controller"),t.playerCamera,typeof(Camera),true);
            if(!t.playerCamera){EditorGUILayout.HelpBox("A Camera is required for operation.",MessageType.Error);}
            t.enableCameraShake = EditorGUILayout.ToggleLeft(new GUIContent("Enable Camera Shake?", "Call this Coroutine externally with duration ranging from 0.01 to 1, and a magnitude of 0.01 to 0.5."), t.enableCameraShake);
            t.lockAndHideCursor = EditorGUILayout.ToggleLeft(new GUIContent("Lock and Hide Cursor","For debuging or if You don't plan on having a pause menu or quit button."),t.lockAndHideCursor);
            t.autoCrosshair = EditorGUILayout.ToggleLeft(new GUIContent("Auto Crosshair","Determines if a basic crosshair will be generated."),t.autoCrosshair);
            if(t.autoCrosshair){EditorGUI.indentLevel++; EditorGUILayout.BeginHorizontal(); EditorGUILayout.PrefixLabel(new GUIContent("Crosshair","Sprite to use as a crosshair."));t.Crosshair = (Sprite)EditorGUILayout.ObjectField(t.Crosshair,typeof(Sprite),false); EditorGUILayout.EndHorizontal(); EditorGUI.indentLevel--;}
            GUI.enabled = true;
            EditorGUILayout.Space();
        #endregion
        
        #region Movement Setup
            EditorGUILayout.LabelField("",GUI.skin.horizontalSlider);
            GUILayout.Label("Movement Setup",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter,fontStyle = FontStyle.Bold, fontSize = 13},GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            t.playerCanMove = EditorGUILayout.ToggleLeft(new GUIContent("Enable Player Movement","Determines if the player is allowed to move."),t.playerCanMove);
            GUI.enabled = t.playerCanMove;
            t.walkByDefault = EditorGUILayout.ToggleLeft(new GUIContent("Walk By Default","Determines if the default mode of movement is 'Walk' or 'Srpint'."),t.walkByDefault);
            t.walkSpeed = EditorGUILayout.Slider(new GUIContent("Walk Speed","Determines how fast the player walks."),t.walkSpeed,0.1f,10);
            t.sprintKey = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Sprint Key","Determines what key needs to be pressed to enter a sprint"),t.sprintKey);
            t.sprintSpeed = EditorGUILayout.Slider(new GUIContent("Sprint Speed","Determines how fast the player sprints."),t.sprintSpeed,0.1f,20);
            t.canJump = EditorGUILayout.ToggleLeft(new GUIContent("Can Player Jump?","Determines if the player is allowed to jump."),t.canJump);
            GUI.enabled = t.playerCanMove && t.canJump; EditorGUI.indentLevel++;
            t.jumpPower = EditorGUILayout.Slider(new GUIContent("Jump Power","Determines how high the player can jump."),t.jumpPower,0.1f,15);
            t.canHoldJump = EditorGUILayout.ToggleLeft(new GUIContent("Hold Jump","Determines if the jump button needs to be pressed down to jump, or if the player can hold the jump button to automaticly jump every time the it hits the ground."),t.canHoldJump);
            EditorGUI.indentLevel --;GUI.enabled = t.playerCanMove;
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            showCrouchMods = EditorGUILayout.BeginFoldoutHeaderGroup(showCrouchMods,new GUIContent("Crouch Modifiers","Stat modifiers that will apply when player is crouching."));
            if(showCrouchMods){
                t._crouchModifiers.useCrouch = EditorGUILayout.ToggleLeft(new GUIContent("Enable Coruch","Determines if the player is allowed to crouch."),t._crouchModifiers.useCrouch);
                GUI.enabled = t.playerCanMove && t._crouchModifiers.useCrouch;
                t._crouchModifiers.crouchKey = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Crouch Key","Determines what key needs to be pressed to crouch"),t._crouchModifiers.crouchKey);
                t._crouchModifiers.toggleCrouch = EditorGUILayout.ToggleLeft(new GUIContent("Toggle Crouch?","Determines if the crouching behaviour is on a toggle or momentary basis."),t._crouchModifiers.toggleCrouch);
                t._crouchModifiers.crouchWalkSpeedMultiplier = EditorGUILayout.Slider(new GUIContent("Crouch Movement Speed Multiplier","Determines how fast the player can move while crouching."),t._crouchModifiers.crouchWalkSpeedMultiplier,0.01f,1.5f);
                t._crouchModifiers.crouchJumpPowerMultiplier = EditorGUILayout.Slider(new GUIContent("Crouching Jump Power Mult.","Determines how much the player's jumping power is increased or reduced while crouching."),t._crouchModifiers.crouchJumpPowerMultiplier,0,1.5f);
                t._crouchModifiers.crouchOverride = EditorGUILayout.ToggleLeft(new GUIContent("Force Crouch Override","A Toggle that will override the crouch key to force player to crouch."),t._crouchModifiers.crouchOverride);
            }
            GUI.enabled = t.playerCanMove;
            EditorGUILayout.EndFoldoutHeaderGroup();      
            EditorGUILayout.Space();
            GUI.enabled =t.playerCanMove;
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            showAdvanced = EditorGUILayout.BeginFoldoutHeaderGroup(showAdvanced,new GUIContent("Advanced Movement","Advanced movenet settings"));
            if(showAdvanced){
                t.useStamina = EditorGUILayout.ToggleLeft(new GUIContent("Enable Stamina","Determines if spriting will be limited by stamina."),t.useStamina);
                GUI.enabled = t.playerCanMove && t.useStamina; EditorGUI.indentLevel++;
                t.staminaLevel = EditorGUILayout.Slider(new GUIContent("Stamina Level","Determines how much stamina the player has. if left 0, stamina will not be used."),t.staminaLevel,0,100);
                t.staminaDepletionSpeed = EditorGUILayout.Slider(new GUIContent("Stamina Depletion Speed","Determines how quickly the player's stamina depletes."),t.staminaDepletionSpeed,0.1f,9);
                t.drawStaminaMeter = EditorGUILayout.ToggleLeft(new GUIContent("Draw Stamina Meter","Determines if a basic stamina meter will be generated."),t.drawStaminaMeter);
                GUI.enabled = t.playerCanMove; EditorGUI.indentLevel --;
                EditorGUILayout.Space();
                t.advanced.FOVKickAmount = EditorGUILayout.Slider(new GUIContent("FOV Kick Amount","Determines how much the camera's FOV will kick upon entering a sprint."),t.advanced.FOVKickAmount,0,5);
                if(t.advanced.FOVKickAmount > 0){t.advanced.changeTime = EditorGUILayout.Slider(new GUIContent("FOV Change Time","Determines the speed of the FOV kick"),t.advanced.changeTime,0.1f,2);}
                EditorGUILayout.Space();
                t.advanced.gravityMultiplier = EditorGUILayout.Slider(new GUIContent("Gravity Multiplier","Determines how much the physics engine's gravitational force is multiplied."),t.advanced.gravityMultiplier,0.1f,5);
                EditorGUILayout.Space();
                t.advanced.maxSlopeAngle = EditorGUILayout.Slider(new GUIContent("Max Slope Angle","Determines the maximum angle the player can walk up. If left 0, the slope detection/limiting system will not be used."),t.advanced.maxSlopeAngle,0,55);
                if(t.advanced.maxSlopeAngle>0){EditorGUILayout.HelpBox("For slops with angles greater than 55° should have a 90° (upright) collider positioned at the bottom of the slope. With out this, the player will have trouble jumping while touching both ground and the slope.",MessageType.Info);}
                EditorGUILayout.Space();
                t.advanced.maxStepHeight = EditorGUILayout.Slider(new GUIContent("Max Step Height","EXPERIMENTAL! Determines if a small ledge is a stair by comparing it to this value. Values over 0.5 produces odd results."),t.advanced.maxStepHeight,0,1);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            GUI.enabled = true;
            EditorGUILayout.Space();
        #endregion

        #region Headbobbing Setup
            EditorGUILayout.LabelField("",GUI.skin.horizontalSlider);
            GUILayout.Label("Headbobbing Setup",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter,fontStyle = FontStyle.Bold, fontSize = 13},GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            t.useHeadbob = EditorGUILayout.ToggleLeft(new GUIContent("Enable Headbobbing","Determines if headbobbing will be used."),t.useHeadbob);
            GUI.enabled = t.useHeadbob;
            t.head = (Transform)EditorGUILayout.ObjectField(new GUIContent("Head Transform","A transform representing the head. The camera should be a child to this transform."),t.head,typeof(Transform),true);
            if(!t.head){EditorGUILayout.HelpBox("A Head Transform is required for headbobbing.",MessageType.Error);}
            GUI.enabled = t.useHeadbob && t.head;
            t.snapHeadjointToCapsul = EditorGUILayout.ToggleLeft(new GUIContent("Snap Head to collider","Recommended. Determines if the head joint will snap to the top on the capsul Collider, It provides better crouch results."),t.snapHeadjointToCapsul);
            t.headbobFrequency = EditorGUILayout.Slider(new GUIContent("Headbob Frequency (Hz)","Determines how fast the headbobbing cycle is."),t.headbobFrequency,0.1f,10);
            t.headbobSwayAngle = EditorGUILayout.Slider(new GUIContent("Tilt Angle","Determines the angle the head will tilt."),t.headbobSwayAngle,0,10);
            t.headbobHeight = EditorGUILayout.Slider(new GUIContent("Headbob Hight","Determines the highest point the head will reach in the headbob cycle."),t.headbobHeight,0,10);
            t.headbobSideMovement = EditorGUILayout.Slider(new GUIContent("Headbob Horizontal Movement","Determines how much vertical movement will occur in the headbob cycle."),t.headbobSideMovement,0,10);
            t.jumpLandIntensity = EditorGUILayout.Slider(new GUIContent("Jump/Land Jerk Intensity","Determines the Jerk intensity when jumping and landing if any."),t.jumpLandIntensity,0,15);
            GUI.enabled = true;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("",GUI.skin.horizontalSlider);

        #endregion

        #region Audio/SFX Setup
            GUILayout.Label("Audio/SFX Setup",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter,fontStyle = FontStyle.Bold, fontSize = 13},GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            t.enableAudioSFX = EditorGUILayout.ToggleLeft(new GUIContent("Enable Audio/SFX", "Enable Audio/SFX systems?"),t.enableAudioSFX);
            GUI.enabled = t.enableAudioSFX;
            t.Volume = EditorGUILayout.Slider(new GUIContent("Volume","Volume to play audio at."),t.Volume,0,10);
            EditorGUILayout.Space();
            t.fsmode = (FirstPersonAIO.FSMode)EditorGUILayout.EnumPopup(new GUIContent("Footstep Mode","Determines the method used to trigger footsetps."),t. fsmode);
            EditorGUILayout.Space();

            #region FS Static
            if(t.fsmode == FirstPersonAIO.FSMode.Static){
                showStaticFS = EditorGUILayout.BeginFoldoutHeaderGroup(showStaticFS,new GUIContent("Footstep Clips","Audio clips available as footstep sounds."));
                if(showStaticFS){
                    GUILayout.BeginVertical("box");
                    for(int i=0; i<staticFS.arraySize; i++){
                    SerializedProperty LS_ref = staticFS.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
                    LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip "+(i+1)+":",LS_ref.objectReferenceValue,typeof(AudioClip),false);
                    if(GUILayout.Button(new GUIContent("X", "Remove this clip"),GUILayout.MaxWidth(20))){ this.t.footStepSounds.RemoveAt(i);}
                    EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))){ this.t.footStepSounds.Add(null);}
                    if(GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))){ this.t.footStepSounds.Clear();}
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(t.footStepSounds,GUILayoutUtility.GetLastRect());
                } 
                EditorGUILayout.EndFoldoutHeaderGroup();
                t.jumpSound = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Jump Clip","An audio clip that will play when jumping."),t.jumpSound,typeof(AudioClip),false);
                t.landSound = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Land Clip","An audio clip that will play when landing."),t.landSound,typeof(AudioClip),false);

            }
            #endregion
            
            else{
                t.dynamicFootstep.materialMode = (FirstPersonAIO.DynamicFootStep.matMode)EditorGUILayout.EnumPopup(new GUIContent("Material Type", "Determines the type of material will trigger footstep audio."),t.dynamicFootstep.materialMode);
                EditorGUILayout.Space();
                #region Wood Section
                showWoodFS = EditorGUILayout.BeginFoldoutHeaderGroup(showWoodFS,new GUIContent("Wood Clips","Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Wood Physic Material'"));
                if(showWoodFS){
                    GUILayout.BeginVertical("box");
                    if(t.dynamicFootstep.materialMode == FirstPersonAIO.DynamicFootStep.matMode.physicMaterial){
                        if(! t.dynamicFootstep.woodPhysMat.Any()){EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Wood Physic Materials",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<woodPhysMat.arraySize; i++){ 
                        SerializedProperty LS_ref = woodPhysMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(PhysicMaterial),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Physic Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.woodPhysMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                          
                      
                        if(GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))){ t.dynamicFootstep.woodPhysMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.woodPhysMat.Any();}

                    else{
                        if(!t.dynamicFootstep.woodMat.Any()){EditorGUILayout.HelpBox("At least one Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Wood Materials", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<woodMat.arraySize; i++){ 
                        SerializedProperty LS_ref = woodMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(Material),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.woodMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                        if(GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))){ t.dynamicFootstep.woodMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.woodMat.Any();
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Wood Audio Clips", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                    for(int i=0; i<woodFS.arraySize; i++){ 
                    SerializedProperty LS_ref = woodFS.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
                    LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip "+(i+1)+":",LS_ref.objectReferenceValue,typeof(AudioClip),false);
                    if(GUILayout.Button(new GUIContent("X", "Remove this clip"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.woodClipSet.RemoveAt(i);}
                    EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))){ t.dynamicFootstep.woodClipSet.Add(null);}
                    if(GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))){ t.dynamicFootstep.woodClipSet.Clear();}
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(t.dynamicFootstep.woodClipSet,GUILayoutUtility.GetLastRect());
                } 
                GUI.enabled = t.enableAudioSFX;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();
                #endregion 
                #region Metal Section
                showMetalFS = EditorGUILayout.BeginFoldoutHeaderGroup(showMetalFS,new GUIContent("Metal & Glass Clips","Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Metal & Glass Physic Material'"));
                if(showMetalFS){
                    GUILayout.BeginVertical("box");
                    
                    if(t.dynamicFootstep.materialMode == FirstPersonAIO.DynamicFootStep.matMode.physicMaterial){
                        if(! t.dynamicFootstep.metalAndGlassPhysMat.Any()){EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Metal & Glass Physic Materials",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<metalAndGlassPhysMat.arraySize; i++){ 
                        SerializedProperty LS_ref = metalAndGlassPhysMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(PhysicMaterial),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Physic Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.metalAndGlassPhysMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                          
                      
                        if(GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))){ t.dynamicFootstep.metalAndGlassPhysMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.metalAndGlassPhysMat.Any();}

                    else{
                        if(!t.dynamicFootstep.metalAndGlassMat.Any()){EditorGUILayout.HelpBox("At least one Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Metal & Glass Materials", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<metalAndGlassMat.arraySize; i++){ 
                        SerializedProperty LS_ref = metalAndGlassMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(Material),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.metalAndGlassMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                        if(GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))){ t.dynamicFootstep.metalAndGlassMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.metalAndGlassMat.Any();
                    }

                    EditorGUILayout.LabelField("Metal & Glass Audio Clips", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                    for(int i=0; i<metalFS.arraySize; i++){ 
                    SerializedProperty LS_ref = metalFS.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
                    LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip "+(i+1)+":",LS_ref.objectReferenceValue,typeof(AudioClip),false);
                    if(GUILayout.Button(new GUIContent("X", "Remove this clip"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.metalAndGlassClipSet.RemoveAt(i);}
                    EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))){ t.dynamicFootstep.metalAndGlassClipSet.Add(null);}
                    if(GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))){ t.dynamicFootstep.metalAndGlassClipSet.Clear();}
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(t.dynamicFootstep.metalAndGlassClipSet,GUILayoutUtility.GetLastRect());
                } 
                GUI.enabled = t.enableAudioSFX;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();
                #endregion
                #region Grass Section
                showGrassFS = EditorGUILayout.BeginFoldoutHeaderGroup(showGrassFS,new GUIContent("Grass Clips","Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Grass Physic Material'"));
                if(showGrassFS){
                    GUILayout.BeginVertical("box");

                    if(t.dynamicFootstep.materialMode == FirstPersonAIO.DynamicFootStep.matMode.physicMaterial){
                        if(! t.dynamicFootstep.grassPhysMat.Any()){EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Grass Physic Materials",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<grassPhysMat.arraySize; i++){ 
                        SerializedProperty LS_ref = grassPhysMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(PhysicMaterial),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Physic Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.grassPhysMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                          
                      
                        if(GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))){ t.dynamicFootstep.grassPhysMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.grassPhysMat.Any();}

                    else{
                        if(!t.dynamicFootstep.grassMat.Any()){EditorGUILayout.HelpBox("At least one Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Grass Materials", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<grassMat.arraySize; i++){ 
                        SerializedProperty LS_ref = grassMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(Material),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.grassMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                        if(GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))){ t.dynamicFootstep.grassMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.grassMat.Any();
                    }
                    
                    EditorGUILayout.LabelField("Grass Audio Clips", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                    for(int i=0; i<grassFS.arraySize; i++){ 
                    SerializedProperty LS_ref = grassFS.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
                    LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip "+(i+1)+":",LS_ref.objectReferenceValue,typeof(AudioClip),false);
                    if(GUILayout.Button(new GUIContent("X", "Remove this clip"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.grassClipSet.RemoveAt(i);}
                    EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))){ t.dynamicFootstep.grassClipSet.Add(null);}
                    if(GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))){ t.dynamicFootstep.grassClipSet.Clear();}
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(t.dynamicFootstep.grassClipSet,GUILayoutUtility.GetLastRect());
                } 
                GUI.enabled = t.enableAudioSFX;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();
                #endregion
                #region Dirt Section
                showDirtFS = EditorGUILayout.BeginFoldoutHeaderGroup(showDirtFS,new GUIContent("Dirt & Gravel Clips","Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Dirt & Gravel Physic Material'"));
                if(showDirtFS){
                    GUILayout.BeginVertical("box");

                    if(t.dynamicFootstep.materialMode == FirstPersonAIO.DynamicFootStep.matMode.physicMaterial){
                        if(! t.dynamicFootstep.dirtAndGravelPhysMat.Any()){EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Dirt & Gravel Physic Materials",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<dirtAndGravelPhysMat.arraySize; i++){ 
                        SerializedProperty LS_ref = dirtAndGravelPhysMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(PhysicMaterial),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Physic Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.dirtAndGravelPhysMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                          
                      
                        if(GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))){ t.dynamicFootstep.dirtAndGravelPhysMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.dirtAndGravelPhysMat.Any();}

                    else{
                        if(!t.dynamicFootstep.dirtAndGravelMat.Any()){EditorGUILayout.HelpBox("At least one Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Dirt & Gravel Materials", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<dirtAndGravelMat.arraySize; i++){ 
                        SerializedProperty LS_ref = dirtAndGravelMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(Material),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.dirtAndGravelMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                        if(GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))){ t.dynamicFootstep.dirtAndGravelMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.dirtAndGravelMat.Any();
                    }

                    EditorGUILayout.LabelField("Dirt & Gravel Audio Clips", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                    for(int i=0; i<dirtFS.arraySize; i++){ 
                    SerializedProperty LS_ref = dirtFS.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
                    LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip "+(i+1)+":",LS_ref.objectReferenceValue,typeof(AudioClip),false);
                    if(GUILayout.Button(new GUIContent("X", "Remove this clip"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.dirtAndGravelClipSet.RemoveAt(i);}
                    EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))){ t.dynamicFootstep.dirtAndGravelClipSet.Add(null);}
                    if(GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))){ t.dynamicFootstep.dirtAndGravelClipSet.Clear();}
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(t.dynamicFootstep.dirtAndGravelClipSet,GUILayoutUtility.GetLastRect());
                } 
                GUI.enabled = t.enableAudioSFX;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();
                #endregion
                #region Rock Section
                showConcreteFS = EditorGUILayout.BeginFoldoutHeaderGroup(showConcreteFS,new GUIContent("Rock & Concrete Clips","Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Rock & Concrete Physic Material'"));
                if(showConcreteFS){
                    GUILayout.BeginVertical("box");

                    if(t.dynamicFootstep.materialMode == FirstPersonAIO.DynamicFootStep.matMode.physicMaterial){
                        if(! t.dynamicFootstep.rockAndConcretePhysMat.Any()){EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Rock & Concrete Physic Materials",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<rockAndConcretePhysMat.arraySize; i++){ 
                        SerializedProperty LS_ref = rockAndConcretePhysMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(PhysicMaterial),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Physic Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.rockAndConcretePhysMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                          
                      
                        if(GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))){ t.dynamicFootstep.rockAndConcretePhysMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.rockAndConcretePhysMat.Any();}

                    else{
                        if(!t.dynamicFootstep.rockAndConcreteMat.Any()){EditorGUILayout.HelpBox("At least one Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Rock & Concrete Materials", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<rockAndConcreteMat.arraySize; i++){ 
                        SerializedProperty LS_ref = rockAndConcreteMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(Material),false); 
                        if(GUILayout.Button(new GUIContent("X", "Remove this Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.rockAndConcreteMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                        if(GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))){ t.dynamicFootstep.rockAndConcreteMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.rockAndConcreteMat.Any();
                    }

                    EditorGUILayout.LabelField("Rock & Concrete Audio Clips", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                    for(int i=0; i<concreteFS.arraySize; i++){ 
                    SerializedProperty LS_ref = concreteFS.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
                    LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip "+(i+1)+":",LS_ref.objectReferenceValue,typeof(AudioClip),false);
                    if(GUILayout.Button(new GUIContent("X", "Remove this clip"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.rockAndConcreteClipSet.RemoveAt(i);}
                    EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))){ t.dynamicFootstep.rockAndConcreteClipSet.Add(null);}
                    if(GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))){ t.dynamicFootstep.rockAndConcreteClipSet.Clear();}
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(t.dynamicFootstep.rockAndConcreteClipSet,GUILayoutUtility.GetLastRect());
                } 
                GUI.enabled = t.enableAudioSFX;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();
                #endregion
                #region Mud Section
                showMudFS = EditorGUILayout.BeginFoldoutHeaderGroup(showMudFS,new GUIContent("Mud Clips","Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Mud Physic Material'"));
                if(showMudFS){
                    GUILayout.BeginVertical("box");
                    
                    if(t.dynamicFootstep.materialMode == FirstPersonAIO.DynamicFootStep.matMode.physicMaterial){
                        if(! t.dynamicFootstep.mudPhysMat.Any()){EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Mud Physic Materials",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<mudPhysMat.arraySize; i++){ 
                        SerializedProperty LS_ref = mudPhysMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(PhysicMaterial),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Physic Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.mudPhysMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                          
                      
                        if(GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))){ t.dynamicFootstep.mudPhysMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.mudPhysMat.Any();}

                    else{
                        if(!t.dynamicFootstep.mudMat.Any()){EditorGUILayout.HelpBox("At least one Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Mud Materials", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<mudMat.arraySize; i++){ 
                        SerializedProperty LS_ref = mudMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(Material),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Material"),GUILayout.MaxWidth(20))){t.dynamicFootstep.mudMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                        if(GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))){t.dynamicFootstep.mudMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.mudMat.Any();
                    }

                    EditorGUILayout.LabelField("Mud Audio Clips", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                    for(int i=0; i<mudFS.arraySize; i++){ 
                    SerializedProperty LS_ref = mudFS.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
                    LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip "+(i+1)+":",LS_ref.objectReferenceValue,typeof(AudioClip),false);
                    if(GUILayout.Button(new GUIContent("X", "Remove this clip"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.mudClipSet.RemoveAt(i);}
                    EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))){ t.dynamicFootstep.mudClipSet.Add(null);}
                    if(GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))){ t.dynamicFootstep.mudClipSet.Clear();}
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(t.dynamicFootstep.mudClipSet,GUILayoutUtility.GetLastRect());
                } 
                GUI.enabled = t.enableAudioSFX;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();
                #endregion
                #region Custom Section
                showCustomFS = EditorGUILayout.BeginFoldoutHeaderGroup(showCustomFS,new GUIContent("Custom Material Clips","Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Custom Physic Material'"));
                if(showCustomFS){
                    GUILayout.BeginVertical("box");

                    if(t.dynamicFootstep.materialMode == FirstPersonAIO.DynamicFootStep.matMode.physicMaterial){
                        if(! t.dynamicFootstep.customPhysMat.Any()){EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Custom Physic Materials",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<customPhysMat.arraySize; i++){ 
                        SerializedProperty LS_ref = customPhysMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(PhysicMaterial),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Physic Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.customPhysMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                          
                      
                        if(GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))){ t.dynamicFootstep.customPhysMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.customPhysMat.Any();}

                    else{
                        if(!t.dynamicFootstep.customMat.Any()){EditorGUILayout.HelpBox("At least one Material must be assigned first.",MessageType.Warning);}
                        EditorGUILayout.LabelField("Custom Materials", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                        for(int i=0; i<customMat.arraySize; i++){ 
                        SerializedProperty LS_ref = customMat.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("",LS_ref.objectReferenceValue,typeof(Material),false);
                        if(GUILayout.Button(new GUIContent("X", "Remove this Material"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.customMat.RemoveAt(i);}
                        EditorGUILayout.EndHorizontal();
                        }
                        if(GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))){ t.dynamicFootstep.customMat.Add(null);}
                        GUI.enabled = t.dynamicFootstep.customMat.Any();
                    }

                    EditorGUILayout.LabelField("Custom Audio Clips", new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold});
                    for(int i=0; i<customFS.arraySize; i++){ 
                    SerializedProperty LS_ref = customFS.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
                    LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip "+(i+1)+":",LS_ref.objectReferenceValue,typeof(AudioClip),false);
                    if(GUILayout.Button(new GUIContent("X", "Remove this clip"),GUILayout.MaxWidth(20))){ t.dynamicFootstep.customClipSet.RemoveAt(i);}
                    EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))){ t.dynamicFootstep.customClipSet.Add(null);}
                    if(GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))){ t.dynamicFootstep.customClipSet.Clear();}
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(t.dynamicFootstep.customClipSet,GUILayoutUtility.GetLastRect());
                }
                GUI.enabled = t.enableAudioSFX;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();
                #endregion
                #region Fallback Section
                showStaticFS = EditorGUILayout.BeginFoldoutHeaderGroup(showStaticFS,new GUIContent("Fallback Footstep Clips","Audio clips available as footsteps in case a collider with an unrecognized/null Physic Material is walked on."));
                if(showStaticFS){
                    GUILayout.BeginVertical("box");
                    for(int i=0; i<staticFS.arraySize; i++){
                    SerializedProperty LS_ref = staticFS.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal("box");
                    LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip "+(i+1)+":",LS_ref.objectReferenceValue,typeof(AudioClip),false);
                    if(GUILayout.Button(new GUIContent("X", "Remove this clip"),GUILayout.MaxWidth(20))){ this.t.footStepSounds.RemoveAt(i);}
                    EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    
                    if(GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))){ this.t.footStepSounds.Add(null);}
                    if(GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))){ this.t.footStepSounds.Clear();}
                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(t.footStepSounds,GUILayoutUtility.GetLastRect());
                } 
                EditorGUILayout.EndFoldoutHeaderGroup();
                #endregion
            }
        #endregion

        /*   
        #region FunctionSnipets
            GUILayout.Label("Audio/SFX Setup",new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter,fontStyle = FontStyle.Bold, fontSize = 13},GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.Space();

        #endregion 
        
        */



            GUI.enabled = true;
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label(new GUIContent("Other Assets:","Some of my other Assets I develop"),new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter},GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            if(!loadedAds){
                if(GUILayout.Button("View Assets")){loadedAds = true; DownloadImage("http://www.aedangraves.info/portfolio-site/MiscSite_content/Images/adTex1.png");}
            }else{if(GUILayout.Button(adTex1, new GUIStyle(){stretchWidth = true,fixedHeight = 64, alignment = TextAnchor.MiddleCenter})){Application.OpenURL("https://assetstore.unity.com/packages/templates/systems/infinigun-universal-fps-firearm-153247");}}

            
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            if(GUI.changed){
                EditorUtility.SetDirty(t);
                Undo.RecordObject(t,"FPAIO Change");
                SerT.ApplyModifiedProperties();
                }
        }
        private void DropAreaGUI(List<AudioClip> clipList, Rect dropArea){
            var evt = Event.current;

            switch (evt.type){
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if(!dropArea.Contains(evt.mousePosition)){break;}
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if(evt.type == EventType.DragPerform){
                        DragAndDrop.AcceptDrag();
                        foreach(var draggedObject in DragAndDrop.objectReferences){
                            var drago = draggedObject as AudioClip;
                            if(!drago){continue;}
                            clipList.Add(drago);
                        }
                    }
                Event.current.Use();
                EditorUtility.SetDirty(t);
                SerT.ApplyModifiedProperties();
                break;
            }
        }
        public static void DownloadImage(string url)
         {
             using (WebClient client = new WebClient())
             {
                 byte[] data = client.DownloadData(url);
                 Texture2D tex = new Texture2D(2, 2);
                 tex.LoadImage(data);
 
                 adTex1 = tex;
             }
         }
    }
#endif