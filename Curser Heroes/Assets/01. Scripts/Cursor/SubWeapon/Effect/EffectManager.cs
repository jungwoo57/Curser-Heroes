using UnityEngine;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    private BaseMonster owner;
    private readonly List<IEffect> activeEffects = new();

    public void Init(BaseMonster target)
    {
        owner = target;
        if (target != null)
        {
            transform.SetParent(target.transform);
            transform.localPosition = Vector3.zero;
        }
    }

    public void AddEffect(IEffect effect)
    {
        effect.Apply(owner);
        activeEffects.Add(effect);
    }

    void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Update(Time.deltaTime);
            if (activeEffects[i].IsFinished)
                activeEffects.RemoveAt(i);
        }
    }

}
