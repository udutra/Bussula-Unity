using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    private readonly float halfSize = 500;

    public RectTransform north;
    public RectTransform east;
    public RectTransform west;
    public RectTransform south;

    //public RectTransform marker;
    //public Transform markerObj;
    
    public RawImage texture;
    public List<Marker> markers;
    public bool withTexture;
    public GameObject markersGO;
    private void Start()
    {
        if (!withTexture)
        {
            texture.gameObject.SetActive(false);
        }
        else
        {
            if (texture.texture != null)
            {
                markersGO.gameObject.SetActive(false);
            }
            else
            {
                texture.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (withTexture)
        {
            texture.uvRect = new Rect(Player.obj.LocalYRotation / 360f, 0, 1f, 1f);
        }
        else
        {
            float northAngle = -Vector2.SignedAngle(Player.obj.ForwardXZ, Vector2.up);
            float eastAngle = -Vector2.SignedAngle(Player.obj.ForwardXZ, Vector2.right);
            float westAngle = -Vector2.SignedAngle(Player.obj.ForwardXZ, Vector2.left);
            float southAngle = -Vector2.SignedAngle(Player.obj.ForwardXZ, Vector2.down);

            north.anchoredPosition = new Vector2(AngleToCompassPos(northAngle), north.anchoredPosition.y);
            east.anchoredPosition = new Vector2(AngleToCompassPos(eastAngle), north.anchoredPosition.y);
            west.anchoredPosition = new Vector2(AngleToCompassPos(westAngle), north.anchoredPosition.y);
            south.anchoredPosition = new Vector2(AngleToCompassPos(southAngle), north.anchoredPosition.y);

            foreach (Marker obj in markers)
            {
                float angle = CalculateAngle(Player.obj.transform.position, obj.objecRefence.transform.position);
                obj.rectTransform.anchoredPosition = new Vector2(AngleToCompassPos(angle), obj.YPosition);
                TransparentEffect(obj.image, obj.XPosition);
                ScaleEffect(obj.rectTransform, Player.obj.transform.position, obj.objecRefence.transform.position, 20f);
            }
        }
    }

    private float AngleToCompassPos(float angle)
    {
        return (halfSize / 180f) * angle;
    }

    private float CalculateAngle(Vector3 playerPosition, Vector3 objectPosition)
    {
        Vector2 playerXZ = new Vector2(playerPosition.x, playerPosition.z);
        Vector2 objectXZ = new Vector2(objectPosition.x, objectPosition.z);
        return Vector2.SignedAngle(objectXZ - playerXZ, Player.obj.ForwardXZ);
    }

    private void TransparentEffect(Image image, float x)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1.1f - (Mathf.Abs(x) / halfSize));
    }

    private void ScaleEffect(RectTransform rectT, Vector3 player, Vector3 objectRef, float maxDistChange)
    {
        float dist = Vector3.Distance(player, objectRef) / maxDistChange;
        dist = Mathf.Clamp(dist, 0f, 1f);
        rectT.localScale = Vector2.one * Mathf.Lerp(1.2f, 0.5f, dist);
    }
}