using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    private BaseMonster owner;
    private readonly List<IEffect> activeEffects = new List<IEffect>();

    
    public void Init(BaseMonster target)
    {
        owner = target;
        if (target != null && transform != target.transform)
        {
            transform.SetParent(target.transform);
            transform.localPosition = Vector3.zero;
        }
    }

    
    public void AddEffect(IEffect effect)
    {
        
        activeEffects.Add(effect);
        Debug.Log($"[EffectManager] Added {effect.GetType().Name} to {owner.name}");
    }

    void Update()
    {
       
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];
            Debug.Log($"[EffectManager] Updating {effect.GetType().Name} on {owner.name}");
            effect.Update(Time.deltaTime);

            if (effect.IsFinished)
            {
                Debug.Log($"[EffectManager] {effect.GetType().Name} finished on {owner.name}");
                activeEffects.RemoveAt(i);
            }
        }
    }
}
