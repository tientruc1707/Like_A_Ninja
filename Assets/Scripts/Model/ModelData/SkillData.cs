using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public string Description;
    public int Power;
    public List<StringConstant.SIGN> HandSigns;
}
