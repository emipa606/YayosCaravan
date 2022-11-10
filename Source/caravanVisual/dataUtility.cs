using System.Collections.Generic;
using RimWorld.Planet;

namespace caravanVisual;

public static class dataUtility
{
    public static Dictionary<Caravan, caravanData> dic_caravan = new Dictionary<Caravan, caravanData>();

    public static void reset()
    {
        dic_caravan = new Dictionary<Caravan, caravanData>();
    }

    public static caravanData GetData(Caravan key)
    {
        if (!dic_caravan.ContainsKey(key))
        {
            dic_caravan[key] = new caravanData();
        }

        dic_caravan[key].setParent(key);
        return dic_caravan[key];
    }

    public static void Remove(Caravan key)
    {
        dic_caravan.Remove(key);
    }
}