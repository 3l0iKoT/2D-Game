using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditCombat : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distansRay;
    [SerializeField] private LayerMask layer;

    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (player.transform.position.x > transform.position.x)
        {
            sprite.flipX = true;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            sprite.flipX = false;
        }
        Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * distansRay);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, (player.transform.position - transform.position).normalized, distansRay);
        if (hitInfo.collider != null)
        {
            Debug.Log("Оппа");
        }
    }
}
