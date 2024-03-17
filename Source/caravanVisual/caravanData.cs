using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace caravanVisual;

public class caravanData : IExposable
{
    public readonly List<Material> m = [];
    private Caravan c;

    // Data
    public List<Vector3> prevPos = [];


    private int tickGap = 20;


    // Data Save
    public void ExposeData()
    {
        //Scribe_Collections.Look<Vector3>(ref prevPos, $"pastPos", LookMode.Value);
    }

    public void setParent(Caravan _c)
    {
        c = _c;
    }

    public void tryAddPrevPos()
    {
        tickGap = Mathf.RoundToInt(c.pather.nextTileCostTotal / 110f);
        if (tickGap <= 0)
        {
            return;
        }

        if (Find.TickManager.TicksGame % tickGap != 0)
        {
            return; // 20 정도가 적당
        }

        if (!Find.TickManager.Paused && c.pather.MovingNow && c.pawns.Count > 1)
        {
            prevPos.Insert(0, c.DrawPos);
        }
    }
}