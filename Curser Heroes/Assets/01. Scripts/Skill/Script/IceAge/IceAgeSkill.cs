using System.Collections.Generic;
using UnityEngine;
using static SkillManager;

public class IceAgeSkill : MonoBehaviour
{
    public GameObject iceFieldPrefab;

    private float spawnInterval = 2f;
    private float timer = 0f;
    private int maxFields = 10;
    private List<GameObject> spawnedFields = new();

    private SkillManager.SkillInstance skillInstance;

    private AudioSource audioSource;

    public void Init(SkillManager.SkillInstance instance)
    {
        skillInstance = instance;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && spawnedFields.Count < maxFields)
        {
            timer = 0f;
            SpawnField();
        }

        spawnedFields.RemoveAll(f => f == null);
    }

    private void SpawnField()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0f;

        if (audioSource != null && skillInstance.skill.audioClip != null)
        {
            audioSource.PlayOneShot(skillInstance.skill.audioClip);
        }

        GameObject field = Instantiate(iceFieldPrefab, cursorPos, Quaternion.identity);


        int damage = skillInstance.skill.levelDataList[skillInstance.level- 1].damage;

        field.GetComponent<IceAgeField>().Setup(damage, skillInstance.skill.audioClip);
        spawnedFields.Add(field);
    }
}