using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FreezeEnemies_Effect", menuName = "Data/Item effect/Freeze _Enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.1f)
        {
            return;
        }
        if (!Inventory.instance.canUseArmor())
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2f);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
