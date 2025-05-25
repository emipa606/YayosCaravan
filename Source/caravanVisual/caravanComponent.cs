using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace caravanVisual;

public class caravanComponent : WorldComponent
{
    public enum ZoomMode
    {
        none,
        bigLeader,
        vanilla
    }

    private static float time;

    private caravanData caravanData;

    public caravanComponent(World world) : base(world)
    {
        dataUtility.Reset();
    }

    public override void WorldComponentTick()
    {
        update();
        if (WorldRendererUtility.WorldRenderedNow)
        {
            return;
        }

        foreach (var c in dataUtility.CaravanDatas.Keys)
        {
            caravanData = dataUtility.GetData(c);
            caravanData.TryAddPrevPos();
        }
    }


    public static Rot4 GetRot(Caravan caravan)
    {
        var tweenedVelocity = caravan.tweener.LastTickTweenedVelocity;
        Rot4 rot;
        if (Mathf.Abs(tweenedVelocity.x) > Mathf.Abs(tweenedVelocity.y))
        {
            rot = tweenedVelocity.x >= 0 ? Rot4.East : Rot4.West;
        }
        else
        {
            rot = tweenedVelocity.y > 0 ? Rot4.North : Rot4.South;
        }

        return rot;
    }

    public static Rot4 GetRot(Vector3 velocity)
    {
        Rot4 rot;
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            rot = velocity.x >= 0 ? Rot4.East : Rot4.West;
        }
        else
        {
            rot = velocity.y > 0 ? Rot4.North : Rot4.South;
        }

        return rot;
    }

    private static void update()
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
            }
            else
            {
                var wiggle = Mathf.Sin((time + uniqueTick) * 2f);
                angle += Vector3.up * wiggle * 0.05f;
                pos += wiggle * new Vector3(0.015f, 0f, 0f);
            }

            angle.Normalize();
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