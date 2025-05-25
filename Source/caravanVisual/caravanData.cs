using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace caravanVisual;

public class caravanData : IExposable
{
    public readonly List<Material> Materials = [];
    private Caravan localCaravan;

    // Data
    public List<Vector3> PrevPos = [];


    private int tickGap = 20;


    // Data Save
    public void ExposeData()
    {
        //Scribe_Collections.Look<Vector3>(ref prevPos, $"pastPos", LookMode.Value);
    }

    public void SetParent(Caravan caravan)
    {
        localCaravan = caravan;
    }

    public void TryAddPrevPos()
    {
        tickGap = Mathf.RoundToInt(localCaravan.pather.nextTileCostTotal / 110f);
        if (tickGap <= 0)
        {
            return;
        }

        if (Find.TickManager.TicksGame % tickGap != 0)
        {
            return; // 20 정도가 적당
        }

        if (!Find.TickManager.Paused && localCaravan.pather.MovingNow && localCaravan.pawns.Count > 1)
        {
            PrevPos.Insert(0, localCaravan.DrawPos);
        }
    }
}