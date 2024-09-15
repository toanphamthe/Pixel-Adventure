using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMoveable
{
    bool IsWallJumping { get; }
    bool IsWallSliding { get; }
    bool IsGrounded { get; }
    bool IsDoubleJump { get; set; }
    void Move();
    void StopMove();
    void Jump();
    void WallJump();
    void WallSlide();
    void Flip();
}
