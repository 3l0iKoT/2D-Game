using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearParticle : MonoBehaviour
{
    [SerializeField] private float maxParticleTime = 0;
    private float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
            Destroy(gameObject);
    }
}
