using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AttackEffect : MonoBehaviour
{
    public float moveSpeed;
    [HideInInspector]
    public UnitController attacker;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        foreach (UnitController target in FindObjectsOfType<UnitController>())
        {
            if (attacker.data.Config.faction != target.data.Config.faction)
            {
                Vector2 direction = (target.rb.position - attacker.rb.position).normalized;
                rb.velocity = direction * moveSpeed;
                break;
            }
        }
        attacker.data.OnDead += (attacker, damageInfo) => Destroy(gameObject);
        StartCoroutine(PrepareDestroy());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out UnitController target) && attacker.data.Config.faction != target.data.Config.faction)
        {
            attacker.data.Attack(target.data);
            Destroy(gameObject);
        }
    }

    private IEnumerator PrepareDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
