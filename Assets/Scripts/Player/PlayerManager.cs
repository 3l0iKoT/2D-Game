using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float deathHeight;
    [SerializeField] private int playerHealth;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private TextMeshProUGUI playerHealthText;

    private float diedTime = 5;
    private bool isDied = false;
    
    private Animator anim;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (diedTime <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (isDied)
        {
            diedTime -= Time.deltaTime;
        }
        if (transform.position.y < deathHeight)
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;

        anim.SetTrigger("hurt");

        if (playerHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isDied)
        {
            Instantiate(deathEffect, transform.position, transform.rotation);
        }
        isDied = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Destroy(transform.Find("Square").gameObject);
    }
}
