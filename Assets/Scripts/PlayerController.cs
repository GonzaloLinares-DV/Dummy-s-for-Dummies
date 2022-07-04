using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    PlayerCharacter _player;

    public PlayerController(PlayerCharacter player)
    {
        _player = player;
    }

    public void OnUpdate()         
    {
        Movement();
        CameraLook();
    }

    public void Movement()
    {
        _player.moveX = Input.GetAxisRaw("Horizontal");
        _player.moveZ = Input.GetAxisRaw("Vertical");
    }

    public void CameraLook()
    {
        _player.lookX = Input.GetAxisRaw("Mouse X");
        _player.lookY = Input.GetAxisRaw("Mouse Y");
    }

    public bool Jump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool Hookshot()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
