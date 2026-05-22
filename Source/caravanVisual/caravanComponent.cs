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
        if (WorldRendererUtility.WorldRendered)
        {
            return;
        }

        foreach (var c in dataUtility.CaravanDatas.Keys)
        {
            caravanData = dataUtility.GetData(c);
            caravanData.TryAddPrevPos(1);
        }
    }


    public static Rot4 GetRot(Caravan caravan)
    {
        return GetRot(caravan.DrawPos, caravan.tweener.LastTickTweenedVelocity);
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

    public static Rot4 GetRot(Vector3 pos, Vector3 velocity)
    {
        if (velocity.sqrMagnitude <= 1E-06f)
        {
            return Rot4.South;
        }

        WorldRendererUtility.GetTangentsToPlanet(pos, out var northTangent, out var eastTangent);
        var east = Vector3.Dot(velocity, eastTangent);
        var north = Vector3.Dot(velocity, northTangent);

        if (Mathf.Abs(east) > Mathf.Abs(north))
        {
            return east >= 0f ? Rot4.East : Rot4.West;
        }

        return north > 0f ? Rot4.North : Rot4.South;
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
        var angle = Vector3.Cross(Vector3.up, vector);
        if (angle.sqrMagnitude <= 1E-06f)
        {
            WorldRendererUtility.GetTangentsToPlanet(pos, out _, out angle);
            angle = -angle;
        }

        angle.Normalize();

        if (caravanVisualMod.instance.Settings.SwingAnimation)
        {
            var swingAxis = Vector3.Cross(angle, vector).normalized;
            if (uniqueTick >= 0)
            {
                var wiggle = Mathf.Sin((time + uniqueTick) * 5f);
                angle = (angle + swingAxis * wiggle * 0.1f).normalized;
                pos += wiggle * angle * 0.03f;
            }
            else
            {
                var wiggle = Mathf.Sin((time + uniqueTick) * 2f);
                angle = (angle + swingAxis * wiggle * 0.05f).normalized;
                pos += wiggle * angle * 0.015f;
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