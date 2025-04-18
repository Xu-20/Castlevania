using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//2024年11月19日添加解锁技能逻辑
public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SKillTreeSlot dashUnlockButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UI_SKillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOmDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SKillTreeSlot cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked { get; private set; }


    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);

    }
    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }


    private void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
            cloneOmDashUnlocked = true;

    }

    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash()
    {
        if (cloneOmDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }

}