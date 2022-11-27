using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace caravanVisual;

public class caravanComponent : WorldComponent
{
    public enum en_zoomMode
    {
        none,
        bigLeader,
        vanilla
    }

    public static float time;

    private caravanData cd;

    public caravanComponent(World world) : base(world)
    {
        dataUtility.reset();
    }

    public override void WorldComponentTick()
    {
        Update();
        if (WorldRendererUtility.WorldRenderedNow)
        {
            return;
        }

        foreach (var c in dataUtility.dic_caravan.Keys)
        {
            cd = dataUtility.GetData(c);
            cd.tryAddPrevPos();
        }
    }


    public static Rot4 getRot(Caravan caravan)
    {
        var vel = caravan.tweener.LastTickTweenedVelocity;
        Rot4 r;
        if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
        {
            r = vel.x >= 0 ? Rot4.East : Rot4.West;
        }
        else
        {
            r = vel.y > 0 ? Rot4.North : Rot4.South;
        }

        return r;
    }

    public static Rot4 getRot(Vector3 vel)
    {
        Rot4 r;
        if (Mathf.Abs(vel.x) > Mathf.Abs(vel.y))
        {
            r = vel.x >= 0 ? Rot4.East : Rot4.West;
        }
        else
        {
            r = vel.y > 0 ? Rot4.North : Rot4.South;
        }

        return r;
    }

    public void Update()
    {
        if (Current.ProgramState != ProgramState.Playing)
        {
            return;
        }

        if (!Find.TickManager.Paused)
        {
            time += Time.deltaTime;
        }

        if (time > 1000f)
        {
            time = 0f;
        }
    }


    public static void DrawQuadTangentialToPlanet(int uniqueTick, Vector3 pos, float size, float altOffset,
        Material material, float longitude, MaterialPropertyBlock propertyBlock = null)
    {
        if (material == null)
        {
            return;
        }

        var vector = pos.normalized;
        var angle = Vector3.left;
        switch (longitude)
        {
            case > 45f:
                angle = new Vector3(0, 0, -1);
                break;
            case < -45f:
                angle = new Vector3(0, 0, 1);
                break;
        }

        if (longitude is > 135f or < -135f)
        {
            angle = Vector3.right;
        }

        if (caravanVisualMod.instance.Settings.SwingAnimation)
        {
            if (uniqueTick >= 0)
            {
                var wiggle = Mathf.Sin((time + uniqueTick) * 5f);
                angle += Vector3.up * wiggle * 0.1f;
                pos += wiggle * new Vector3(0.03f, 0f, 0f);
                angle.Normalize();
            }
            else
            {
                var wiggle = Mathf.Sin((time + uniqueTick) * 2f);
                angle += Vector3.up * wiggle * 0.05f;
                pos += wiggle * new Vector3(0.015f, 0f, 0f);
                angle.Normalize();
            }
        }


        var q = Quaternion.LookRotation(Vector3.Cross(vector, angle), vector);

        var s = new Vector3(size, 1f, size);
        var matrix = default(Matrix4x4);
        matrix.SetTRS(pos + (vector * altOffset), q, s);

        var layer = WorldCameraManager.WorldLayer;
        if (propertyBlock != null)
        {
            Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, propertyBlock);
        }
        else
        {
            Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer);
        }
    }
}