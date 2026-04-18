using System;
using UnityEngine;
using YuLongFSM;

namespace FSM.Playe
{
    public class Jump : FSMIState<FSMData>
    {
        HeroAnimations animations;
        CharacterController characterController;
        float jumpForce = 10f;      // 跳跃力度
        float moveSpeed = PlayerData.speed;      // 跳跃时的移动速度
        Vector3 jumpVelocity;
        Vector3 moveDir;           // 移动方向
        Vector3 direction = Vector3.zero;
        public override void OnEnter()
        {
            animations = fSMData.creature.animations;
            characterController = fSMData.creature.characterController;
            animations.PlayJump();

            // 跳跃瞬间给一个向上的力
            jumpVelocity = Vector3.up * jumpForce;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = PlayerData.speed*2;
            }
            else
            {
                moveSpeed = PlayerData.speed;
            }

            // =============== 【相机视角】跳跃时移动逻辑 ===============
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");


            direction.x = Input.GetAxisRaw("Horizontal");
            direction.z = Input.GetAxisRaw("Vertical");
            Vector3 forward = Vector3.zero;
            forward = Camera.main.transform.TransformDirection(direction).normalized;
            // Vector3 forward = Camera.main.transform.forward;
            forward.y = 0;
            if (direction != Vector3.zero)
            {
                fSMData.creature.TurnTo(forward);
            }
                
            // 获取主相机
            Camera mainCam = Camera.main;
            Transform camTransform = mainCam.transform;

            // 计算相机方向的水平移动（标准第三人称写法）
            Vector3 camForward = camTransform.forward;
            Vector3 camRight = camTransform.right;
            camForward.y = 0; // 消除垂直方向，只保留水平
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // 最终移动方向 = 相机前 + 相机右
            moveDir = (camForward * v + camRight * h).normalized;

            // =============== 物理逻辑 ===============
            // 重力
            jumpVelocity.y -= 13f * Time.deltaTime;

            // 最终运动 = 跳跃方向 + 水平移动方向
            Vector3 finalVelocity = jumpVelocity;
            if (moveDir.magnitude > 0.1f)
            {
                finalVelocity += moveDir * moveSpeed;
            }

            // 执行移动
            characterController.Move(finalVelocity * Time.deltaTime);

            // =============== 落地检测 ===============
            if (characterController.isGrounded && jumpVelocity.y <= 0)
            {
                animations.SetGround(true);
                fSMManager.Switch(FSMState.Idle);
            }
        }

        public override void OnExit()
        {
            jumpVelocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
    }
}