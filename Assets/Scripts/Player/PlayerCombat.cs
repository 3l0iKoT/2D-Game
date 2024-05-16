using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackColldown = 0.15f;
    [SerializeField] private float attackDecelerationMult = 2;
    [SerializeField] private float attackDamage = 15;
    [SerializeField] private float impactForce;
    private Animator anim;
    private Rigidbody2D rb;
    private float attackRange = 1.2f;
    private float moveInput;
    private float attackPointX;
    private bool isAttack = true;
    private float lastAttackTime;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        attackPointX = attackPoint.localPosition.x;
    }

    private void Update()
    {
        lastAttackTime -= Time.deltaTime;
        moveInput = Input.GetAxis("Horizontal");

        if (moveInput > 0)
        {
            attackPoint.localPosition = new Vector3(attackPointX, attackPoint.localPosition.y);
        }
        else if (moveInput < 0)
        {
            attackPoint.localPosition = new Vector3(-attackPointX, attackPoint.localPosition.y);
        }

        if (lastAttackTime < 0 && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("attack");
            isAttack = false;
            lastAttackTime = 0.3f + attackColldown;
        }

        if (!isAttack)
        {
            Attack();
        }

        if (lastAttackTime > attackColldown)
        {
            rb.AddForce(new Vector2(-rb.velocity.x * attackDecelerationMult, 0), ForceMode2D.Impulse);
        }

    }

    private void Attack()
    {
        if (lastAttackTime <= 0.15f + attackColldown)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(4.28f, 1.64f), 0, enemyLayers);
            Rigidbody2D enemyRB;

            foreach(Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyStats>().TakeDamage(attackDamage);
                enemyRB = enemy.GetComponent<Rigidbody2D>();
                enemyRB.AddForce(Vector2.right * Mathf.Sign(enemy.transform.position.x - transform.position.x) * impactForce, ForceMode2D.Impulse);
            }
            isAttack = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(4.28f, 1.64f));
    }
}
