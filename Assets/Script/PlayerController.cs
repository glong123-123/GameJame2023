using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerController : MonoBehaviour
{
    public enum CharacterState
    {
        idle = 0,
        run,
        jump,
        fall,
        attack,
        dead,
    }
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] m_aniReference = new AnimationReferenceAsset[5];

    public float m_fMoveSpeed;
    public float m_fJumpSpeed;
    public float m_fMovment;

    private Rigidbody2D m_rigidbody2D;
    private BoxCollider2D m_MyFeet;
    private CharacterState m_currentState;
    private CharacterState m_preState;
    private bool m_IsGround;
    private bool isOneWayPlatform;
    public string m_sCurrentAnimation;

    //public bool GiveAttack;

    private bool run;
    private bool jump;
    private bool fall;
    private bool idle;
    //private bool attack;



    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_MyFeet = GetComponent<BoxCollider2D>();
        m_currentState = CharacterState.idle;
        SetCharacterState(m_currentState);
        idle = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Filp();
        Jump();
        //Attack();
        CheckGround();        //检查地板碰撞
        ChangeVar();          //改变变量
        SetAnimationByVar();  //通过变量播放动画
        //OneGamePlatformCheck();
    }

    //检测是否是地面
    void CheckGround()
    {
        m_IsGround = m_MyFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))||
                      m_MyFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform"));
    }

    //开始播放动画
    void SetAnimation(AnimationReferenceAsset animation, bool loop, float fTimeScale)
    {
        if (animation.name.Equals(m_sCurrentAnimation))
        {
            return;
        }
        Spine.TrackEntry animationEntry = skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale = fTimeScale;
        animationEntry.Complete += AnimationEntry_Complete;
        m_sCurrentAnimation = animation.name;
    }

    private void AnimationEntry_Complete(Spine.TrackEntry trackEntry)
    {
        //if (m_currentState == CharacterState.attack)
        //{
        //    attack = false;
        //    if (jump)
        //    {
        //        attack = false;
        //        SetCharacterState(CharacterState.jump);
        //    }
        //    else if (fall)
        //    {
        //        //SetCharacterState(CharacterState.fall);
        //        m_currentState = CharacterState.fall;
        //    }
        //    else if (run)
        //    {
        //        SetCharacterState(CharacterState.run);
        //    }
        //    else if (idle)
        //    {
        //        SetCharacterState(CharacterState.idle);
        //    }
        //}
    }

    //设置动画状态
    void SetCharacterState(CharacterState iState)
    {
        switch (iState)
        {
            case CharacterState.idle:
                SetAnimation(m_aniReference[0], true, 1);
                break;
            case CharacterState.run:
                SetAnimation(m_aniReference[1], true, 1.5f);
                break;
            case CharacterState.jump:
                SetAnimation(m_aniReference[2], false, 1);
                break;
            //case CharacterState.attack:
            //    SetAnimation(m_aniReference[3], false, 3);
            //    break;
            case CharacterState.dead:
                SetAnimation(m_aniReference[4], false, 1);
                break;
        }

        m_currentState = iState;
    }

    //翻转
    void Filp()
    {
        bool bHasXaAxisSpeed = Mathf.Abs(m_rigidbody2D.velocity.x) > Mathf.Epsilon; ;
        if (bHasXaAxisSpeed)
        {
            if (m_rigidbody2D.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (m_rigidbody2D.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    //移动变量
    void Move()
    {
        m_fMovment = Input.GetAxis("Horizontal");
        Vector3 Pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Pos.z);
        //Debug.Log(mousePos);
        m_rigidbody2D.velocity = new Vector2((Input.mousePosition.x - 320) * m_fMoveSpeed / 200, m_rigidbody2D.velocity.y);
        if (transform.position.x < -6)
        {
            m_rigidbody2D.velocity = new Vector2(0, m_rigidbody2D.velocity.y);
            transform.position = new Vector2(-6, transform.position.y);
        }
        else if (transform.position.x > 7)
        {
            m_rigidbody2D.velocity = new Vector2(0, m_rigidbody2D.velocity.y);
            transform.position = new Vector2(7, transform.position.y);
        }
        if (m_fMovment != 0)
        {
            run = true;
        }
        else
        {
            run = false;
        }
    }

    //单面石块
    //void OneGamePlatformCheck()
    //{
    //    float moveY = Input.GetAxis("Vertical");
    //    if (isOneWayPlatform && moveY < -0.1)
    //    {
    //        gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
    //    }
    //}

    //跳跃
    void Jump()
    {
        if (m_IsGround && Input.GetButtonDown("Jump"))
        {
            jump = true;
            m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, m_fJumpSpeed);
        }
    }

    //void Attack()
    //{
    //    if (Input.GetButtonDown("Attack"))
    //    {
    //        attack = true;
    //        GiveAttack = true;
    //    }
    //}

    void ChangeVar()
    {
        idle = false;

        if (jump)
        {
            if (m_rigidbody2D.velocity.y < 0.0f)
            {
                jump = false;
                fall = true;
            }
        }
        else if (m_IsGround)
        {
            idle = true;
            fall = false;
        }
        //if (jump)
        //{
        //    if (m_rigidbody2D.velocity.y < 0.0f)
        //    {
        //        jump = false;
        //        fall = true;
        //    }
        //}
        //else if (m_IsGround)
        //{
        //    idle = true;
        //    fall = false;
        //}

    }
    void SetAnimationByVar()
    {
        switch (m_currentState)
        {
            case CharacterState.idle:
                //if (attack)
                //{
                //    SetCharacterState(CharacterState.attack);
                //}
                if (jump)
                {
                    SetCharacterState(CharacterState.jump);
                }
                else if (run)
                {
                    SetCharacterState(CharacterState.run);
                }
                break;
            case CharacterState.run:
                //if (attack)
                //{
                //    SetCharacterState(CharacterState.attack);
                //}
                if (!run)
                {
                    SetCharacterState(CharacterState.idle);
                }
                else if (jump)
                {
                    SetCharacterState(CharacterState.jump);
                }
                break;
            case CharacterState.jump:
                //if (attack)
                //{
                //    SetCharacterState(CharacterState.attack);
                //}
                if (!jump && fall)
                {
                    //SetCharacterState(CharacterState.fall);
                    m_currentState = CharacterState.fall;
                }
                break;
            case CharacterState.fall:
                //if (attack)
                //{
                //    SetCharacterState(CharacterState.attack);
                //}
                if (idle && !fall)
                {
                    SetCharacterState(CharacterState.idle);
                }
                break;
        }
    }
}
