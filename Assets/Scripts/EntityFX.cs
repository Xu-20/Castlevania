using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    [Header("弹出文本")]
    [SerializeField] private GameObject popUpTextPrefab;


    [Header("震动特效")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHeightDamage;

    [Header("残影特效")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;

    [Header("闪光特效")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("异常状态颜色")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("异常状态粒子")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("攻击特效")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;

    [Space]
    [SerializeField] private ParticleSystem dustFx;

    [Header("弹出文本颜色")]
    [SerializeField] private Color physicalDamageColor = Color.white;
    [SerializeField] private Color fireDamageColor = new Color(1f, 0.5f, 0f); // 橙红色
    [SerializeField] private Color iceDamageColor = new Color(0.5f, 0.8f, 1f); // 淡蓝色
    [SerializeField] private Color lightningDamageColor = new Color(1f, 1f, 0f); // 黄色
    [SerializeField] private Color criticalDamageColor = new Color(1f, 0f, 0f); // 红色
    [SerializeField] private Color healColor = new Color(0f, 1f, 0f); // 绿色

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        screenShake = GetComponent<CinemachineImpulseSource>();
        originalMat = sr.material;
    }
    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }


    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }
    /// 创建伤害弹出文本
    public void CreatePopUpText(string _text, DamageType _damageType = DamageType.Physical, bool _isCritical = false)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(3, 5);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);


        GameObject newText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

        TextMeshPro textMesh = newText.GetComponent<TextMeshPro>();
        textMesh.text = _text;

        // 根据伤害类型设置颜色
        if (_isCritical)
        {
            textMesh.color = criticalDamageColor;
            textMesh.fontSize += 2; // 暴击伤害文字稍大
        }
        else
        {
            switch (_damageType)
            {
                case DamageType.Physical:
                    textMesh.color = physicalDamageColor;
                    break;
                case DamageType.Fire:
                    textMesh.color = fireDamageColor;
                    break;
                case DamageType.Ice:
                    textMesh.color = iceDamageColor;
                    break;
                case DamageType.Lightning:
                    textMesh.color = lightningDamageColor;
                    break;
                case DamageType.Heal:
                    textMesh.color = healColor;
                    break;
            }
        }
    }


    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
        }
    }

    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;
    }
    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();

    }
    public void IgniteFxFor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("igniteColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ChilFxFor(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating("ChillColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void ShockFxFor(float _seconds)
    {
        shockFX.Play();
        InvokeRepeating("shockColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _seconds);
    }
    private void igniteColorFx()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }
    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        else
        {
            sr.color = chillColor[1];
        }
    }
    private void shockColorFx()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }

    //灰尘的粒子效果
    public void CreateFitFx(Transform _target, bool _critical)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);

        Vector3 hitFxRotion = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFx;

        if (_critical)
        {
            hitPrefab = criticalHitFx;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFxRotion = new Vector3(0, yRotation, zRotation);

        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity, _target);
        newHitFx.transform.Rotate(hitFxRotion);
        Destroy(newHitFx, 0.5f);
    }

    public void PlayDustFx()
    {
        if (dustFx != null)
        {
            dustFx.Play();
        }
    }
}
