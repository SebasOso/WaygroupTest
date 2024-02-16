using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;
public interface IModifierProvider
{
    IEnumerable<float> GetAdditiveModifier(Stat stat);
    IEnumerable<float> GetPercentageModifier(Stat stat);
}
