using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage;
    public float AttackTime;
    public float WaitAttackTime;
    private GameObject Player;
    private PolygonCollider2D m_coll2D;


    // Start is called before the first frame update
    void Start()
    {
        Player = this.transform.parent.gameObject;
        m_coll2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Attack();
    }

    //void Attack()
    //{
    //    if (Player.GetComponent<PlayerController>().GiveAttack)
    //    {
    //        Player.GetComponent<PlayerController>().GiveAttack = false;
    //        StartCoroutine(StarttAttack());
    //    }
    //}

    //IEnumerator StarttAttack()
    //{
    //    yield return new WaitForSeconds(WaitAttackTime);
    //    m_coll2D.enabled = true;
    //    StartCoroutine(DisableHitBox());
    //}

    //IEnumerator DisableHitBox()
    //{
    //    yield return new WaitForSeconds(AttackTime);
    //    m_coll2D.enabled = false;
    //}

    //void OnTriggerEnter2D(Collider2D  other)
    //{
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        other.GetComponent<Enemy>().ComputeDamage(damage);
    //    }
    //}
}
