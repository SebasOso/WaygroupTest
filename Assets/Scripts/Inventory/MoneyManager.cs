using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

public class MoneyManager : MonoBehaviour, IJsonSaveable
{
    public int moneyAmount = 0;

    public JToken CaptureAsJToken()
    {
        return JToken.FromObject(moneyAmount);
    }

    public void RestoreFromJToken(JToken state)
    {
        moneyAmount = state.ToObject<int>();
    }
    public void AddMoney(int moneyToAdd)
    {
        moneyAmount += moneyToAdd;
    }
    public void SubtractMoney(int moneyToSubtract)
    {
        if(moneyAmount <= 0){return;}
        moneyAmount = Mathf.Max(moneyAmount - moneyToSubtract, 0);
    }
}
