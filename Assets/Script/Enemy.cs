using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int Blood;
    public int Damage;
    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        if (Blood <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void ComputeDamage(int damage)
    {
        Blood -= damage;
    }
}
