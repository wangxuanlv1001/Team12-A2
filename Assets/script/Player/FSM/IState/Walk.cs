using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YuLongFSM;
using static HeroAnimations;

namespace FSM.Playe
{
    public class Walk : FSMIState<FSMData>
    {
        CharacterController characterController;
        HeroAnimations animations;

        Vector3 direction=Vector3.zero;
        public override void OnEnter()
        {
            characterController = fSMData.creature.characterController;
            animations= fSMData.creature.animations; 
            animations.PlayWalk();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
          //  fSMData.player.Gravity();
            Move();
            if (animations.state!= HState.Walk)
            {
                animations.PlayWalk();
            }
        }
        public  void Move()
        {
            direction.x = Input.GetAxisRaw("Horizontal");
            direction.z = Input.GetAxisRaw("Vertical");
            Vector3 forward=Vector3.zero;
                forward = Camera.main.transform.TransformDirection(direction).normalized;
                // Vector3 forward = Camera.main.transform.forward;
                forward.y = 0;
            if (direction != Vector3.zero)
            {
                fSMData.creature.TurnTo(forward);
                forward.y = -1;
                characterController.Move( forward * Time.deltaTime * PlayerData.speed);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    fSMManager.Switch(FSMState.Run);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                fSMManager.Switch(FSMState.Jump);
            }
            else
            {
                fSMManager.Switch(FSMState.Idle);

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fSMManager.Switch(FSMState.Jump);
            }

        }
    }
}
