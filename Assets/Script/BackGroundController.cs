using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private bool bInfectH;
    [SerializeField] private bool bInfectV;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;



    private void Start()
    {
        //cameraTransform = Camera.main.transform;
        GameObject gameObject = GameObject.Find("BackGroundCamera");
        cameraTransform = gameObject.GetComponent<Camera>().transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;

    }

    private void LateUpdate()
    {
        Vector3 deltMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltMovement.x * parallaxEffectMultiplier.x, deltMovement.y * parallaxEffectMultiplier.y, 0);
        lastCameraPosition = cameraTransform.position;
        if (bInfectH)
        {
            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
            {
                float offestPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offestPositionX, transform.position.y);
            }
        }

        if (bInfectV)
        {
            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
            {
                float offestPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offestPositionY);
            }
        }

    }
}
