using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : UnitController
{
    private void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out UnitController enemyController))
        {
            enemyController.data.Attack(data);
        }
    }
}
