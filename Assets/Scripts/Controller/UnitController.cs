using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UnitController : MonoBehaviour
{
    public int unitID;
    public AttackEffect attackEffect;
    [HideInInspector]
    public Rigidbody2D rb;
    [System.NonSerialized]
    public Unit data;
    protected Vector2 movement = Vector2.zero;

    private void Start()
    {
        data = unitID == 0 ? UserDataManager.GetUserData<Player>() : new Unit(unitID);
        rb = GetComponent<Rigidbody2D>();
        if (unitID != 0)
        {
            data.OnDead += (attacker, damageInfo) => Destroy(gameObject);
            StartCoroutine(AutoAttack());
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + data.SPD * Time.fixedDeltaTime * movement);
    }

    private IEnumerator AutoAttack()
    {
        while (true)
        {
            AttackEffect effect = Instantiate(attackEffect);
            effect.transform.position = transform.position;
            effect.attacker = this;
            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }
    }
}
