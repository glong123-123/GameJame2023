using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryCloud : MonoBehaviour
{
    // Start is called before the first frame update

    private BoxCollider2D boxCollider2D;
    public GameObject player;
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        check();
    }


    private void check()
    {
        if (boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            boxCollider2D.enabled = false;
            player.GetComponent<PlayerController>().m_bTouchCloud = true;
            GetComponent<Renderer>().enabled = false;
            StartCoroutine(DestroyItSelf());
        }
    }
    IEnumerator DestroyItSelf()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
