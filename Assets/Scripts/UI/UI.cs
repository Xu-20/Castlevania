using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("End screens")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;

    [SerializeField] private GameObject restartButton;
    [Space]


    [SerializeField] private GameObject charcaterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillToolTip;

    private void Awake()
    {
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);
    }
    void Start()
    {
        SwitchTo(inGameUI);

        itemTooltip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchWithKeyTo(charcaterUI);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SwitchWithKeyTo(craftUI);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchWithKeyTo(optionsUI);
        }
    }
    public void SwitchTo(GameObject _menu)//该方法用于切换到指定的UI界面
    {
        for (int i = 0; i < transform.childCount; i++)//遍历当前UI对象的所有子物体
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;//检查UI界面是否有FadeScreens

            if (fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);//遍历并隐藏所有子元素,确保了在显示新的UI界面时，所有其他的UI界面都会被隐藏

        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            checkForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }
    private void checkForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;

            SwitchTo(inGameUI);
        }
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(1f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();
}
