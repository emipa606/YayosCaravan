using System.Collections.Generic;
using RimWorld.Planet;

namespace caravanVisual;

public static class dataUtility
{
    public static Dictionary<Caravan, caravanData> CaravanDatas = new();

    public static void Reset()
    {
        CaravanDatas = new Dictionary<Caravan, caravanData>();
    }

    public static caravanData GetData(Caravan key)
    {
        if (!CaravanDatas.ContainsKey(key))
        {
            CaravanDatas[key] = new caravanData();
        }

        CaravanDatas[key].SetParent(key);
        return CaravanDatas[key];
    }
}