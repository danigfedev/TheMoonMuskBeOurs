using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateHandler : StateHandler_Base
{
    [Space(15)]
    [Header("=== Component Specific Fields ===")]
    [SerializeField] Transform playerShip;
    [SerializeField] Transform shieldPrefab;
    [SerializeField] Transform shieldSpawnPos;
    
    [Space(15)]
    [Header("=== Cloud Interaction Fields ===")]
    [SerializeField] Material visibilityFadeMaterial;
    [SerializeField] Color visibilityInitialColor;
    [SerializeField] [Range(0, 1)] float maxFadeAlphaValue;
    [SerializeField] float fadeDuration = 0.25f;
    private float currentTime = 0;
    private Coroutine fadeCoroutine = null;

    private Transform shieldInstance;
    
    public override void Awake()
    {
        InitializeShield();
        base.Awake();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        visibilityFadeMaterial.color = visibilityInitialColor;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        string _tag = other.tag;
        if(_tag == TagList.PU_shieldTag)
        {
            int shieldDuration = other.gameObject.GetComponent<PowerUpHandler>().GetPowerUpAmount();
            EnableShield(shieldDuration);
        }
        else if(_tag== TagList.PU_healthTag)
        {
            int healthAmount = other.gameObject.GetComponent<PowerUpHandler>().GetPowerUpAmount();
            HandleHealing(healthAmount);
        }
        else if(_tag.Contains(TagList.enemyTag) /*== TagList.enemyTag*/)
        {
            // Collision with enemy.
            Debug.Log("Parent: " + other.transform.parent.name);
            Debug.Log("Tag: " + _tag);
            Debug.Log(other.name);
            float damage = other.gameObject.GetComponentInParent<StateHandler_Base>().GetHitDamage();
            HandleDamage(damage);
        }
        else if (_tag.Contains(TagList.bulletPrefix))
        {
            if (_tag != TagList.bulletPlayerTag)
            {
                float damage = other.GetComponentInParent<Bullet>().GetBulletDamage();
                HandleDamage(damage);
            }
        }
        else if( _tag.Contains(TagList.obstaclePrefix) && _tag.Contains(TagList.cloudPrefix))
        {
            //Fade Out visibility
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Fade(maxFadeAlphaValue));
        }

    }

    private void OnTriggerExit(Collider other)
    {
        string _tag = other.tag;
        if (_tag.Contains(TagList.obstaclePrefix) && _tag.Contains(TagList.cloudPrefix))
        {
            //Fade In visibility
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(Fade(0));
        }
    }


    /// <summary>
    /// Difficutl visibility
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fade(float endAlpha)
    {
        Color c = visibilityFadeMaterial.color;
        currentTime = 0;
        float initialAlpha = c.a;
        //float initialAlpha = (endAlpha > 0) ? 0 : maxFadeAlphaValue;
        //float alpha = initialAlpha;
        while (currentTime <= 1)
        {
            float alpha = Mathf.Lerp(initialAlpha, endAlpha, currentTime);
            visibilityFadeMaterial.color = new Color(c.r, c.g, c.b, alpha);

            currentTime += Time.deltaTime / fadeDuration;
            yield return null;
        }
        visibilityFadeMaterial.color = new Color(c.r, c.g, c.b, endAlpha);

        fadeCoroutine = null;
    }


    private void InitializeShield()
    {
        shieldInstance = Instantiate(shieldPrefab);
        shieldInstance.gameObject.SetActive(false);
        shieldInstance.position = shieldSpawnPos.position;
        shieldInstance.GetComponent<Shield>().PlayerShip = playerShip;
    }

    private void EnableShield(int shieldDuration)
    {
        shieldInstance.gameObject.SetActive(true);
        shieldInstance.GetComponent<Shield>().EnableShield(shieldDuration);
    }


    protected override void HandleDamage(float damage)
    {
        //stateProperties.totalHealth -= damage;
        totalHealth -= damage;

        UpdateHealthBar();
        StartCoroutine(EditMaterial(damageColor));

        //Debug.Log("Damage taken: " + totalHealth);
        //Edit material
        if (/*stateProperties.totalHealth*/totalHealth <= 0)
        {
            //GameOver!
            StartCoroutine(Die());
        }
    }

    protected override void HandleHealing(float health)
    {
        totalHealth += health;
        //stateProperties.totalHealth += health;

        UpdateHealthBar();
        StartCoroutine(EditMaterial(healingColor));

        if (totalHealth > maxHealth) totalHealth = maxHealth;
        //if (stateProperties.totalHealth > maxHealth) stateProperties.totalHealth = maxHealth;

        Debug.Log("Healing: " + totalHealth);
        //Debug.Log("Healing: " + stateProperties.totalHealth);
        
        //Edit material?
    }

    private IEnumerator Die()
    {
        Debug.LogError("YOU DIED");
        if (dieEventSO != null)
            dieEventSO.RaiseEvent(); //==> GameManager? and UI
        yield return new WaitForEndOfFrame();

        gameObject.SetActive(false);
        Time.timeScale = 0;
    }
}
