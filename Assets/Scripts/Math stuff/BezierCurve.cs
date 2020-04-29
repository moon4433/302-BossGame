using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class BezierCurve : MonoBehaviour
{
    /// <summary>
    /// The resolution to calculate each individual curve (not
    /// the whole spline!). A spline with n points will have a
    /// total resoluntion of RESOLUTION * (n - 2)
    /// </summary>
    public const int RESOLUTION = 10;

    public Vector3[] points = new Vector3[0];

    ////////// Cached values: ////////

    public Vector3[] worldPoints { get; private set; }
    float[] curveLengths = new float[0];
    float splineLength = 0;

    ///////////////////////////////

    public void AddPoint()
    {
        // create an array that is +1 bigger:
        Vector3[] pts = new Vector3[points.Length + 1];

        //copy data over from array:
        for (int i = 0; i < points.Length; i++)
        {
            pts[i] = points[i];
        }

        if(pts.Length == 1)
        {
            pts[0] = Vector3.zero;
        }
        else if(pts.Length == 2)
        {
            pts[1] = pts[0] + Vector3.right;
        }
        else if (points.Length >= 3)
        {
            Vector3 posLast = pts[pts.Length - 2];
            Vector3 pos2ndToLast = pts[pts.Length - 3];
            Vector3 dir = posLast - pos2ndToLast;
            pts[pts.Length - 1] = posLast + dir;
        }
        points = pts;
        
    }

    public void RemovePoint()
    {
        // create an array that is -1 smaller:
        Vector3[] pts = new Vector3[points.Length - 1];

        //copy data over from array:
        for (int i = 0; i < points.Length - 1; i++)
        {
            
            pts[i] = points[i];
            
        }
        if (pts.Length == 1)
        {
            pts[0] = Vector3.zero;
        }
        else if (pts.Length == 2)
        {
            pts[1] = pts[0] + Vector3.right;
        }
        
        points = pts;
    }


    // Update is called once per frame
    void Update()
    {
        CacheSplineData();
    }
    private void OnValidate()
    {
        CacheSplineData();
    }
    public void CacheSplineData()
    {
        CalcWorldPositions();
        LengthofSpline();
    }

    public Vector3 FindPositionAt(float p)
    {
        if (worldPoints == null) return Vector3.zero;
        if (worldPoints.Length == 0) return Vector3.zero;
        if (worldPoints.Length == 1) return worldPoints[0];
        if (worldPoints.Length == 2) return AnimMath.Lerp(worldPoints[0], worldPoints[1], p);

        Vector3 result = Vector3.zero;
        float leftValue = 0;
        for(int i = 0; i < curveLengths.Length; i++)
        {
            float rightValue = leftValue + curveLengths[i];
            float rightPercent = rightValue / splineLength;

            if(rightPercent >= p)
            {
                float leftPercent = leftValue / splineLength;
                float curvePercent = (p - leftPercent) / (rightPercent - leftPercent);

                Vector3 a = worldPoints[i];
                Vector3 b = worldPoints[i + 1];
                Vector3 c = worldPoints[i + 2];

                if (i > 0) a = AnimMath.Lerp(a, b, .5f);
                if (i < curveLengths.Length - 1) c = AnimMath.Lerp(b, c, .5f);

                result = AnimMath.QuadraticBezier(a, b, c, curvePercent);
                break;
            }
            leftValue = rightValue;
        }

        /// find the value

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        for(int i = 1; i < worldPoints.Length; i++)
        {

            Vector3 p1 = worldPoints[i - 1];
            Vector3 p2 = worldPoints[i];

            Gizmos.DrawLine(p1, p2);
        }
        Gizmos.color = Color.white;
        DrawSpline();
        
    }
    void DrawSpline()
    {
        int numOfCurves = worldPoints.Length - 2;

        for (int i = 1; i <= numOfCurves; i++)
        {
            Vector3 a = worldPoints[i - 1];
            Vector3 b = worldPoints[i];
            Vector3 c = worldPoints[i + 1];

            if (i > 1) a = AnimMath.Lerp(a, b, .5f);
            if (i < numOfCurves) c = AnimMath.Lerp(b, c, .5f);

            DrawCurve(a, b, c);
        }
    }

    void DrawCurve(Vector3 a, Vector3 b, Vector3 c)
    {

        Vector3 pos1 = new Vector3();
        for (int i  = 0; i <= RESOLUTION; i++)
        {
            float p = i / (float)RESOLUTION;
            Vector3 pos2 = AnimMath.QuadraticBezier(a, b, c, p);
            if(i > 0)Gizmos.DrawLine(pos1, pos2);
            pos1 = pos2;
        }
    }

    void CalcWorldPositions()
    {
        worldPoints = new Vector3[points.Length];
        for(int i = 0; i < points.Length; i++)
        {
            worldPoints[i] = transform.TransformPoint(points[i]);
        }
    }
    void LengthofSpline()
    {
        if(worldPoints.Length <= 1)
        {
            curveLengths = new float[0];
            splineLength = 0;
            return;
        }
        else if(worldPoints.Length == 2)
        {
            curveLengths = new float[0];
            splineLength = (worldPoints[0] - worldPoints[1]).magnitude;
            return;
        }

        int numOfCurves = worldPoints.Length - 2;
        curveLengths = new float[numOfCurves];
        splineLength = 0;

        for (int i = 1; i <= numOfCurves; i++)
        {
            Vector3 a = worldPoints[i - 1];
            Vector3 b = worldPoints[i];
            Vector3 c = worldPoints[i + 1];

            if (i > 1) a = AnimMath.Lerp(a, b, .5f);
            if (i < numOfCurves) c = AnimMath.Lerp(b, c, .5f);

            float length = LengthofCurve(a, b, c);
            curveLengths[i - 1] = length;
            splineLength += length;
        }
    }

    /// <summary>
    /// Calculate the length of the bezier curve from a to c
    /// </summary>
    /// <param name="a">Anchor point</param>
    /// <param name="b">Handle</param>
    /// <param name="c">Anchor Point</param>
    /// <returns>The length of the curve</returns>
    float LengthofCurve(Vector3 a, Vector3 b, Vector3 c)
    {
        float result = 0;

        Vector3 pos1 = new Vector3();
        for (int i = 0; i <= RESOLUTION; i++)
        {
            float p = i / (float)RESOLUTION;
            Vector3 pos2 = AnimMath.QuadraticBezier(a, b, c, p);
            if (i > 0) result += (pos2 - pos1).magnitude;
            pos1 = pos2;
        }

        return result;
    }
}


