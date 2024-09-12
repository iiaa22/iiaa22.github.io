using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public void OnSkill0Learnd(Unit unit, Skill skill)
    {
        Debug.Log($"{unit.Config.name}习得了{skill.Config.name}！");
    }
}
