using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerController : MonoBehaviour
{
    public enum CharacterState
    {
        idle = 0,
        jump,
        highjump,
        fly,
        fall,
        start,
        end,
    }
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] m_aniReference = new AnimationReferenceAsset[6];

    [SerializeField] public float m_fMoveSpeeds;
    [SerializeField] public float m_fJumpSpeeds;
    public bool m_bTouchCloud;

    private Rigidbody2D m_rigidbody2D;
    private BoxCollider2D m_MyFeet;
    private CircleCollider2D m_pRoll;
    private CharacterState m_currentState;
    private bool m_IsGround;
    private string m_sCurrentAnimation;
    private float m_fFallThenJumpTime = 1f;
    private float m_fMovments;

    private bool canJump;
    private bool run;
    private bool jump;
    private bool fall;
    private bool fly;
    private bool idle;
    private bool end;
    private bool start;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_MyFeet = GetComponent<BoxCollider2D>();
        m_pRoll = GetComponent<CircleCollider2D>();
        m_currentState = CharacterState.start;
        SetCharacterState(m_currentState);
        idle = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_currentState != CharacterState.start)
        //{
            Move();
            Filp();
        //}

        Jump();
        CheckGround();        //检查地板碰撞
        ChangeVar();          //改变变量
        SetAnimationByVar();  //通过变量播放动画
    }

    //检测是否是地面
    void CheckGround()
    {
        m_IsGround = m_MyFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) ||
                      m_MyFeet.IsTouchingLayers(LayerMask.GetMask("OneWayPlatform")) ||
                       m_bTouchCloud;
        m_bTouchCloud = false;
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

    }

    //设置动画状态
    void SetCharacterState(CharacterState iState)
    {
        switch (iState)
        {
            case CharacterState.idle:
                SetAnimation(m_aniReference[0], true, 1);
                break;
            case CharacterState.start:
                //SetAnimation(m_aniReference[1], false, 1);
                //先滚后落地
                EnterGame();
                break;
            case CharacterState.jump:
                SetAnimation(m_aniReference[1], false, 1);
                break;
            case CharacterState.highjump:
                //if (m_currentState != CharacterState.start)
                //{
                //    m_pRoll.enabled = true;
                //}
                SetAnimation(m_aniReference[2], true, 1);
                break;
            case CharacterState.fly:
                SetAnimation(m_aniReference[3], true, 1);
                break;
            case CharacterState.fall:
                //m_pRoll.enabled = false;
                SetAnimation(m_aniReference[4], false, 1);
                break;
            case CharacterState.end:
                SetAnimation(m_aniReference[5], false, 1);
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

    //进入游戏
    void EnterGame()
    {
        //从左中上转到中间
        //m_MyFeet.enabled = false;

        m_rigidbody2D.velocity = new Vector2(0, m_rigidbody2D.velocity.y);

        //SetAnimation(m_aniReference[2], true, 1);
    }

    //移动变量
    void Move()
    {
        m_fMovments = Input.GetAxis("Horizontal");
        Vector3 Pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Pos.z);
        //Debug.Log(mousePos);
        m_rigidbody2D.velocity = new Vector2((Input.mousePosition.x - 320) * m_fMoveSpeeds / 200, m_rigidbody2D.velocity.y);
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
        if (m_fMovments != 0)
        {
            run = true;
        }
        else
        {
            run = false;
        }
    }

    //跳跃
    void Jump()
    {
        if (m_IsGround && canJump)
        {
            jump = true;
            m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, m_fJumpSpeeds);
        }
    }

    void ChangeVar()
    {
        idle = false;

        if (jump)
        {
            if (m_rigidbody2D.velocity.y < 0.0f)
            {
                jump = false;
                fly = true;
            }
        }
        else if (m_IsGround)
        {
            m_MyFeet.enabled = true;
            m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, 0);
            transform.localPosition += new Vector3(0, 1, 0);
            fall = true;
            fly = false;

            //开启延时，延时后进行跳跃
            StartCoroutine(FallThenJump());
        }

        if (transform.localPosition.y < -13 && m_MyFeet.enabled == false)
        {
            m_MyFeet.enabled = true;
            m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, 0);
            transform.localPosition = new Vector3(transform.localPosition.x, -12, 0);
            SetCharacterState(CharacterState.fall);
        }
    }

    IEnumerator FallThenJump()
    {
        yield return new WaitForSeconds(m_fFallThenJumpTime);
        //开始进行跳跃
        jump = true;
        fall = false;
        canJump = true;
    }

    void SetAnimationByVar()
    {
        switch (m_currentState)
        {
            case CharacterState.start:
                if (idle && !start)
                {
                    SetCharacterState(CharacterState.idle);
                }
                break;
            case CharacterState.idle:
                if (jump)
                {
                    SetCharacterState(CharacterState.jump);
                }
                if (end)
                {
                    SetCharacterState(CharacterState.end);
                }
                break;
            case CharacterState.jump:
                if (!jump && fly)
                {
                    SetCharacterState(CharacterState.fly);
                }
                break;
            case CharacterState.fly:
                if (fly && fall)
                {
                    SetCharacterState(CharacterState.fall);
                }
                break;
            case CharacterState.fall:
                if (idle && !fall)
                {
                    SetCharacterState(CharacterState.idle);
                }
                else if (jump && !fall)
                {
                    SetCharacterState(CharacterState.jump);
                }
                break;
            case CharacterState.end:
                if (idle && !end)
                {
                    SetCharacterState(CharacterState.end);
                }
                break;
        }
    }
}
