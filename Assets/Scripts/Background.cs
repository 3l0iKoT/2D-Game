using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float speed = 1f;
    private Transform[] layers;

    private float oldCamPos;

    private float offset = 19.584f;

    private void Awake()
    {
        if (!cam)
        {
            cam = FindObjectOfType<Camera>().transform;
        }
        layers = new Transform[transform.childCount];
        for( int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }
        oldCamPos = cam.position.x;
    }

    private void LateUpdate()
    {   
        float delta = cam.position.x - oldCamPos;
        oldCamPos = cam.position.x;
        transform.position = new Vector3(transform.position.x, cam.position.y, transform.position.z);
        if (layers[1].position.x - cam.position.x > offset / 2)
        {
            MoveBackground(-offset);
        }
        if (cam.position.x - layers[1].position.x > offset / 2)
        {
            MoveBackground(offset);
        }
        if (delta != 0) ParallaxBackground(delta);
    }

    private void MoveBackground(float offsetBG)
    {
        transform.position = transform.position + new Vector3(offsetBG, 0, 0);
    }

    private void ParallaxBackground(float delta)
    {
        transform.position = transform.position + new Vector3(delta * speed, 0, 0);
    }
}
