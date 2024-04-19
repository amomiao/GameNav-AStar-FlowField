using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Vector2、3的拓展方法 </summary>
public static class VectorExpand
{
    /// <summary> 参数1的x、y是否分别大于min,小于max的x、y </summary>
    /// <param name="v"> 检测点 </param>
    /// <param name="min"> 最小值 </param>
    /// <param name="max"> 最大值 </param>
    /// <param name="minEqual"> 是否允许等于最小值 </param>
    /// <param name="maxEqual"> 是否允许等于最大值 </param>
    /// <returns></returns>
    public static bool In(Vector2Int v, Vector2Int min, Vector2Int max, bool minEqual = true, bool maxEqual = true)
    {
        if (minEqual && maxEqual)
            return (v.x >= min.x && v.y >= min.y && v.x <= max.x && v.y <= max.y);
        else if (minEqual)
            return (v.x >= min.x && v.y >= min.y && v.x < max.x && v.y < max.y);
        else if (maxEqual)
            return (v.x > min.x && v.y > min.y && v.x < max.x && v.y < max.y);
        else
            return (v.x > min.x && v.y > min.y && v.x < max.x && v.y < max.y);
    }

    /// <summary> 不进行开方的情况下，仅求另一个Pos是否在范围内 </summary>
    public static bool InScope(this Vector3 self, Vector3 other, float disSquare)
    {
        return Mathf.Abs((self - other).OnlyMult()) < disSquare;
    }

    /// <summary> 仅乘而不开方:x方+y方+z方 </summary>
    public static float OnlyMult(this Vector2 v2)
    {
        return v2.x * v2.x + v2.y * v2.y;
    }

    /// <summary> 仅乘而不开方:x方+y方+z方 </summary>
    public static float OnlyMult(this Vector3 v3)
    {
        return v3.x * v3.x + v3.y * v3.y + v3.z * v3.z;
    }

    /// <summary> 距离公式不开方 </summary>
    public static float DisSquare(this Vector2 apos, Vector2 bpos)
    {
        return (apos.x - bpos.x) * (apos.x - bpos.x) + (apos.y - bpos.y) * (apos.y - bpos.y);
    }

    /// <summary> 距离公式不开方 </summary>
    public static float DisSquare(this Vector3 apos, Vector3 bpos)
    {
        return (apos.x - bpos.x) * (apos.x - bpos.x) + (apos.y - bpos.y) * (apos.y - bpos.y) + (apos.z - bpos.z) * (apos.z - bpos.z);
    }

    #region V3 To V2
    private static Vector2 v3ToV2;
    /// <summary> 返回一个三维的向量的xz赋值给二维向量的xy里 </summary>
    public static Vector2 V3ToV2(Vector3 v3)
    {
        v3ToV2.V3ToV2(v3);
        return v3ToV2;
    }
    /// <summary> 一个三维的向量的xz赋值给二维向量的xy里 </summary>
    public static void V3ToV2(this ref Vector2 v2, Vector3 v3)
    {
        v2.x = v3.x;
        v2.y = v3.z;
    }
    #endregion V3 To V2

    /// <summary> 大于180度的正角转为负角 </summary>
    public static void PositivAngleTranseNegative(this ref Vector2 v2)
    {
        if (v2.x > 180)
            v2.x -= 360;
        if (v2.y > 180)
            v2.y -= 360;
    }

    /// <summary> 大于180度的正角转为负角 </summary>
    public static Vector2 PositivAngleTranseNegative(Vector2 v2)
    {
        if (v2.x > 180)
            v2.x -= 360;
        if (v2.y > 180)
            v2.y -= 360;
        return v2;
    }

    /// <summary> 大于180度的正角转为负角 </summary>
    public static void PositivAngleTranseNegative(this ref Vector3 v3)
    {
        if (v3.x > 180)
            v3.x -= 360;
        if (v3.y > 180)
            v3.y -= 360;
        if (v3.z > 180)
            v3.z -= 360;
    }

    /// <summary> 大于180度的正角转为负角 </summary>
    public static Vector3 PositivAngleTranseNegative(Vector3 v3)
    {
        if (v3.x > 180)
            v3.x -= 360;
        if (v3.y > 180)
            v3.y -= 360;
        if (v3.z > 180)
            v3.z -= 360;
        return v3;
    }

    /// <summary> 小于0度的负角转为正角 </summary>
    public static void NegativeAngleTransPositive(this ref Vector2 v2)
    {
        if (v2.x < 0)
            v2.x += 360;
        if (v2.y < 0)
            v2.y += 360;
    }

    /// <summary> 小于0度的负角转为正角 </summary>
    public static Vector2 NegativeAngleTransPositive(Vector2 v2)
    {
        if (v2.x < 0)
            v2.x += 360;
        if (v2.y < 0)
            v2.y += 360;
        return v2;
    }

    /// <summary> 小于0度的负角转为正角 </summary>
    public static void NegativeAngleTransPositive(this ref Vector3 v3)
    {
        if (v3.x < 0)
            v3.x += 360;
        if (v3.y < 0)
            v3.y += 360;
        if (v3.z < 0)
            v3.z += 360;
    }

    /// <summary> 小于0度的负角转为正角 </summary>
    public static Vector3 NegativeAngleTransPositive(Vector3 v3)
    {
        if (v3.x < 0)
            v3.x += 360;
        if (v3.y < 0)
            v3.y += 360;
        if (v3.z < 0)
            v3.z += 360;
        return v3;
    }
}
