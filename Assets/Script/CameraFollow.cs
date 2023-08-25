using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform m_traTarget;
    public float m_fSmooth;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (m_traTarget != null)
        {
            if (transform.position != m_traTarget.position)
            {
                Vector3 v3TargetPos = m_traTarget.position + new Vector3(0, 3, 0);
                Vector3 v3FinalPos = Vector3.Lerp(transform.position, v3TargetPos, m_fSmooth);
                transform.position = new Vector3(transform.position.x, v3FinalPos.y, transform.position.z); 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


}
