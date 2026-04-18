using FSM.Playe;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using YuLongFSM;

public class Player : Creature
{
    public static Player Instance;
    public Transform shou;
    public FSMManager<FSMData> fSM;
    FSMData fSMData = new FSMData();
    private Vector3 bulletPos;
    private float hp = 100;
    bool isDie;

    private void Awake()
    {
        Instance = this;

        fSM = new FSMManager<FSMData>(fSMData);
        fSMData.creature = this;

        // ×¢²áÔ­ÓÐ×´Ì¬
        fSM.Register(FSMState.Idle, new Idle());
        fSM.Register(FSMState.Walk, new Walk());
        fSM.Register(FSMState.Run, new Run());

        // ×¢²áÐÂÔö×´Ì¬

        fSM.Register(FSMState.Jump, new Jump());


   

    }
    public void OnDestroy()
    {

    }
    private void AddHp(object obj)
    {
        float vaule = 0;
        float.TryParse(obj.ToString(), out vaule);
        roleData.hp += (int)vaule;
        if (roleData.hp>roleData.maxHp)
        {
            roleData.hp = roleData.maxHp;
        }
    }



    protected override void Start()
    {
        base.Start();

        fSM.Switch(FSMState.Idle);
    }

    float jieTime;
    void Update()
    {
        if (fSM != null)
        {
            fSM.OnUpdate();
        }

   

    }

    public override void Hurt(int attack)
    {
        base.Hurt(attack);
  
    }

    public void Gravity(float gravity = 5)
    {
        characterController.Move(Vector3.down * Time.deltaTime * gravity);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}