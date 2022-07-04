using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFov : MonoBehaviour {

    private Camera playerCamera;
    private float targetFov;
    private float fov;
    public GameObject canHook;
    public GameObject wallLeftIndicator;
    public GameObject wallRightIndicator;
    PlayerCharacter player;

    private void Awake() 
    {
        playerCamera = GetComponent<Camera>();
        targetFov = playerCamera.fieldOfView;
        fov = targetFov;

        player = FindObjectOfType<PlayerCharacter>();
    }

    void Update() 
    {
        float fovSpeed = 4f;
        fov = Mathf.Lerp(fov, targetFov, Time.deltaTime * fovSpeed);
        playerCamera.fieldOfView = fov;

        HookIndicator();
        WallRunIndicator();
    }

    public void SetCameraFov(float targetFov) 
    {
        this.targetFov = targetFov;
    }

    public void HookIndicator()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit, player.hookRange, player.hookMask))
            canHook.SetActive(true);
        else
            canHook.SetActive(false);
    }

    public void WallRunIndicator()
    {
        if (player.wallLeft) wallLeftIndicator.SetActive(true);
        else wallLeftIndicator.SetActive(false);

        if (player.wallRight) wallRightIndicator.SetActive(true);
        else wallRightIndicator.SetActive(false);
    }
}
