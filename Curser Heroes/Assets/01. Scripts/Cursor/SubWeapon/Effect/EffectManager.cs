using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private List<IEffect> activeEffects = new List<IEffect>();
    private Monster monster;

    void Awake()
    {
        monster = GetComponent<Monster>();
    }

    public void AddEffect(IEffect effect)
    {
        effect.Apply(monster);
        activeEffects.Add(effect);
    }

    void Update()
    {
        float delta = Time.deltaTime;
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].Tick(delta);
            if (activeEffects[i].IsFinished)
                activeEffects.RemoveAt(i);
        }
    }
}
