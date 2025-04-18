using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//2024年11月19日
public class Dodge_Skill : Skill
{
    [Header("闪避")]//dodge
    [SerializeField] private UI_SKillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;


    [Header("躲避幻影")]//Mirage dodge
    [SerializeField] private UI_SKillTreeSlot unlockMirageDodge;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }
    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }
    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();//更新角色数值的UI
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if (unlockMirageDodge.unlocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0));
    }
}