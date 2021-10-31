using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateHandler_Base: MonoBehaviour
{
    //[SerializeField] protected StateHandlerProperties stateProperties;
    [SerializeField] protected float totalHealth;
    [SerializeField] protected float hitDamage;

    [Space(10)]
    [Header("== VFX ==")]
    [SerializeField] protected bool hideHealthBarOnAwake = false;
    [SerializeField] protected GameObject healthBarBaseObject;
    [SerializeField] protected Transform healthBarFill;
    [SerializeField] protected MeshRenderer mainObjectRenderer;
    [SerializeField] protected Color damageColor = Color.red;
    [SerializeField] protected Color healingColor = Color.green;
    private Color initialColor;


    [Space(10)]
    [Header("== SO Events ==")]
    [SerializeField] protected SimpleEventSO dieEventSO;

    protected float maxHealth;
    
    // Definitions:

    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void HandleDamage(float damage);
    protected abstract void HandleHealing(float health);

    //Implementations:

    public virtual void Awake()
    {
        maxHealth = totalHealth;
        //maxHealth = stateProperties.totalHealth;
        initialColor = mainObjectRenderer.material.color;

        healthBarBaseObject.SetActive(!hideHealthBarOnAwake);

    }

    public virtual void OnDisable()
    {
        if (mainObjectRenderer == null)
        {
            Debug.LogError("[StateHandler_Base] Main renderer not assigned");
            return;
        }
        mainObjectRenderer.material.color = initialColor;
        healthBarFill.localScale = Vector3.one;
    }

    public virtual void OnDestroy()
    {
        if(mainObjectRenderer == null)
        {
            Debug.LogError("[StateHandler_Base] Main renderer not assigned");
            return;
        }
        mainObjectRenderer.material.color = initialColor;
    }

    public float GetHitDamage() => hitDamage;/*stateProperties.hitDamage;*/

    public void ShowHealthBar() => healthBarBaseObject.SetActive(true);

    protected void UpdateHealthBar()
    {
        float healthPctg = totalHealth / maxHealth;
        healthPctg = Mathf.Clamp(healthPctg, 0, 1);
        Vector3 currentScale = healthBarFill.localScale;
        healthBarFill.localScale = new Vector3(healthPctg, currentScale.y, currentScale.z);
    }

    protected IEnumerator EditMaterial(Color c)
    {
        mainObjectRenderer.material.color = c;
        yield return new WaitForSeconds(0.1f);

        if (mainObjectRenderer == null)
        {
            Debug.LogError("[StateHandler_Base] Main renderer not assigned");
            yield return null;
        }
        mainObjectRenderer.material.color = initialColor;
    }
}
