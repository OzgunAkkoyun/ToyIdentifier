using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{

    private Vector2 p1;
    private Vector2 p2;
    private Vector2 p3;
    private float r1;
    private float r2;
    private float r3;
    private float d1;
    private float d2;
    private float d3;
    private List<float> distances = new List<float>();
    private List<float> ratios = new List<float>();
    //default constructor
    public Triangle(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
        float x1_x2 = (p1.x - p2.x);
        float x1_x3 = (p1.x - p3.x);
        float x2_x3 = (p2.x - p3.x);
        float y1_y2 = (p1.y - p2.y);
        float y1_y3 = (p1.y - p3.y);
        float y2_y3 = (p2.y - p3.y);
        d1 = Vector2.Distance(p1, p2);
        d2 = Vector2.Distance(p1, p3);
        d3 = Vector2.Distance(p2, p3);
        distances.Add(d1);
        distances.Add(d2);
        distances.Add(d3);
        distances.Sort();
        d1 = distances[0];
        d2 = distances[1];
        d3 = distances[2];
        r1 = d1 / d2;
        r2 = d1 / d3;
        r3 = d2 / d3;
        ratios.Add(r1);
        ratios.Add(r2);
        ratios.Add(r3);
        ratios.Sort();
    }
    //lazy constuctor
    public Triangle(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
    {
        p1 = new Vector2(p1x, p1y);
        p2 = new Vector2(p2x, p2y);
        p3 = new Vector2(p3x, p3y);
        float x1_x2 = (p1.x - p2.x);
        float x1_x3 = (p1.x - p3.x);
        float x2_x3 = (p2.x - p3.x);
        float y1_y2 = (p1.y - p2.y);
        float y1_y3 = (p1.y - p3.y);
        float y2_y3 = (p2.y - p3.y);
        d1 = Vector2.Distance(p1, p2);
        d2 = Vector2.Distance(p1, p3);
        d3 = Vector2.Distance(p2, p3);
        distances.Add(d1);
        distances.Add(d2);
        distances.Add(d3);
        distances.Sort();
        d1 = distances[0];
        d2 = distances[1];
        d3 = distances[2];
        r1 = d1 / d2;
        r2 = d1 / d3;
        r3 = d2 / d3;
        ratios.Add(r1);
        ratios.Add(r2);
        ratios.Add(r3);
        ratios.Sort();

    }

    void Start()
    {

    }

    /// <summary>
    /// Compares 2 triangles and returns if they are the same
    /// </summary>
    /// <param name="t">A triangle</param>
    /// <returns></returns>
    public bool isSame(Triangle t)
    {
        int samePointCount = 0;
        if (this.getP1() == t.getP1() || this.getP1() == t.getP2() || this.getP1() == t.getP3())
        {
            samePointCount++;
        }
        if (this.getP2() == t.getP1() || this.getP2() == t.getP2() || this.getP2() == t.getP3())
        {
            samePointCount++;
        }
        if (this.getP3() == t.getP1() || this.getP3() == t.getP2() || this.getP3() == t.getP3())
        {
            samePointCount++;
        }
        if (samePointCount == 3)
        {
            return true;
        }
        return false;
    }    /// <summary>
         /// Compares 2 triangles and returns if they are the same
         /// </summary>
         /// <param name="t">A triangle</param>
         /// <returns></returns>
    public bool hasSameRatio(Triangle t)
    {
        if (this.getR1() == t.getR1() || this.getR2() == t.getR2() || this.getR3() == t.getR3())
        {
            return true;
        }
        if (this.getR1() == t.getR1() || this.getR2() == t.getR3() || this.getR3() == t.getR2())
        {
            return true;
        }
        if (this.getR1() == t.getR2() || this.getR2() == t.getR1() || this.getR3() == t.getR3())
        {
            return true;
        }
        if (this.getR1() == t.getR2() || this.getR2() == t.getR3() || this.getR3() == t.getR1())
        {
            return true;
        }
        if (this.getR1() == t.getR3() || this.getR2() == t.getR1() || this.getR3() == t.getR2())
        {
            return true;
        }
        if (this.getR1() == t.getR3() || this.getR2() == t.getR2() || this.getR3() == t.getR1())
        {
            return true;
        }
        return false;
    }
    public bool hasSimilarRatio(Triangle t, float errorMargin)
    {
        //Debug.Log("Comparing: " + this.RatioString() + "\nWith: " + t.RatioString());
        //Debug.Log("this.getR1() >= " + this.getR1().ToString() + "errorMargin - t.getR1()" + (errorMargin - t.getR1()).ToString() + "errorMargin + t.getR1()" + (errorMargin + t.getR1()).ToString()  );
        //Debug.Log("this.getR2() >= " + this.getR2().ToString() + "errorMargin - t.getR2()" + (errorMargin - t.getR2()).ToString() + "errorMargin + t.getR2()" + (errorMargin + t.getR2()).ToString()  );
       // Debug.Log("this.getR3() >= " + this.getR3().ToString() + "errorMargin - t.getR3()" + (errorMargin - t.getR3()).ToString() + "errorMargin + t.getR3()" + (errorMargin + t.getR3()).ToString());
        //errorMargin = 0.006 
        if ((this.getR1() >=  t.getR1() - errorMargin && this.getR1() <= errorMargin + t.getR1()) &&
            (this.getR2() >=  t.getR2() - errorMargin && this.getR2() <= errorMargin + t.getR2()) &&
            (this.getR3() >=  t.getR3() - errorMargin && this.getR3() <= errorMargin + t.getR3()))
        {
            return true;
        }
        if ((this.getR1() >= t.getR1() - errorMargin && this.getR1() <= errorMargin + t.getR1()) &&
            (this.getR2() >= t.getR3() - errorMargin && this.getR2() <= errorMargin + t.getR3()) &&
            (this.getR3() >= t.getR2() - errorMargin && this.getR3() <= errorMargin + t.getR2()))
        {
            return true;
        }
        if ((this.getR1() >= t.getR2() - errorMargin && this.getR1() <= errorMargin + t.getR2()) &&
            (this.getR2() >= t.getR1() - errorMargin && this.getR2() <= errorMargin + t.getR1()) &&
            (this.getR3() >= t.getR3() - errorMargin && this.getR3() <= errorMargin + t.getR3()))
        {
            return true;
        }
        if ((this.getR1() >= t.getR2() - errorMargin && this.getR1() <= errorMargin + t.getR2()) &&
            (this.getR2() >= t.getR3() - errorMargin && this.getR2() <= errorMargin + t.getR3()) &&
            (this.getR3() >= t.getR1() - errorMargin && this.getR3() <= errorMargin + t.getR1()))
        {
            return true;
        }
        if ((this.getR1() >= t.getR3() - errorMargin && this.getR1() <= errorMargin + t.getR3()) &&
            (this.getR2() >= t.getR1() - errorMargin && this.getR2() <= errorMargin + t.getR1()) &&
            (this.getR3() >= t.getR2() - errorMargin && this.getR3() <= errorMargin + t.getR2()))
        {
            return true;
        }
        if ((this.getR1() >= t.getR3() - errorMargin && this.getR1() <= errorMargin + t.getR3()) &&
            (this.getR2() >= t.getR2() - errorMargin && this.getR2() <= errorMargin + t.getR2()) &&
            (this.getR3() >=t.getR1() - errorMargin && this.getR3() <= errorMargin + t.getR1()))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// returns total distance between 2 triangles(dist. btw. closest point to p1 + dist. btw. closest point to p2 + dist. btw. closest point to p3 )
    /// </summary>
    /// <param name="t"></param>
    /// <param name="rectangleEdgeSize"></param>
    /// <returns></returns>
    public float distance(Triangle t)
    {
        //find closest point to this.p1
        Vector2 closestPoint;
        float d1 = 0, d2 = 0, d3 = 0;
        closestPoint = getClosestPoint(this.p1, t);
        d1 = Vector2.Distance(this.p1, closestPoint);

        closestPoint = getClosestPoint(this.p2, t);
        d2 = Vector2.Distance(this.p2, closestPoint);

        closestPoint = getClosestPoint(this.p3, t);
        d3 = Vector2.Distance(this.p3, closestPoint);

        return d1 + d2 + d3;
    }
    public string RatioString() {
        return "("+r1 +",  "+ r2 + ",  "+ r3 +")";
    }
    /// <summary>
    /// Finds closest corner of triangle t to point p
    /// </summary>
    /// <param name="p"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public Vector2 getClosestPoint(Vector2 p, Triangle t)
    {
        Vector2 p2;
        float smallestDistance = Vector2.Distance(p, t.p1);
        p2 = t.p1;
        if (smallestDistance > Vector2.Distance(p, t.p2))
        {
            smallestDistance = Vector2.Distance(p, t.p2);
            p2 = t.p2;
        }
        if (smallestDistance > Vector2.Distance(p, t.p3))
        {
            smallestDistance = Vector2.Distance(p, t.p3);
            p2 = t.p3;
        }

        return p2;
    }
    //getters
    public Vector2 getP1()
    {
        return p1;
    }
    public Vector2 getP2()
    {
        return p2;
    }
    public Vector2 getP3()
    {
        return p3;
    }
    public float getR1()
    {
        return r1;
    }
    public float getR2()
    {
        return r2;
    }
    public float getR3()
    {
        return r3;
    }

    public void Print()
    {
        Debug.Log("P1: <" + p1.x + ", " + p1.y + "> P2: <" + p2.x + ", " + p2.y + "> P3: <" + p3.x + ", " + p3.y + ">\n");
        Debug.Log("D1: <" + d1 + "> D2: <" + d2 + "> D3: <" + d3 + ">\n");
        Debug.Log("O1: <" + r1 + "> O2: <" + r2+ "> O3: <" + r3 + ">\n");
        Debug.Log("D1: <" + distances[0] + "> D2: <" + distances[1] + "> D3: <" + distances[2] + ">\n");
        Debug.Log("O1: <" + ratios[0] + "> O2: <" + ratios[1] + "> O3: <" + ratios[2] + ">\n");
    }
    public void PrintWolfram()
    {
        Debug.Log("P1: (" + p1.x + ", " + p1.y + ") P2: (" + p2.x + ", " + p2.y + ") P3: (" + p3.x + ", " + p3.y + ")\n");
        Debug.Log("D1: <" + d1 + "> D2: <" + d2 + "> D3: <" + d3 + ">\n");
        Debug.Log("O1: <" + r1 + "> O2: <" + r2 + "> O3: <" + r3 + ">\n");
        Debug.Log("D1: <" + distances[0] + "> D2: <" + distances[1] + "> D3: <" + distances[2] + ">\n");
        Debug.Log("O1: <" + ratios[0] + "> O2: <" + ratios[1] + "> O3: <" + ratios[2] + ">\n");
    }
    public string ToString(int triangleIndex)
    {
        return "T#" + triangleIndex + "-" + p1.x + "," + p1.y + "-" + p2.x + "," + p2.y + "-" + p3.x + "," + p3.y;
    }
    /// <summary>
    /// Sorts points by distance to origin
    /// </summary>
    public void SortPoints()
    {
        //distance to origin for p1 etc.
        Vector2 origin = new Vector2(0, 0);
        float d2O1 = Vector2.Distance(origin, p1);
        float d2O2 = Vector2.Distance(origin, p2);
        float d2O3 = Vector2.Distance(origin, p3);
        float min = Mathf.Min(d2O1, d2O2, d2O3);
        float min2 = -1.0f;
        //if p1 is closest to origin
        if (min == d2O1)
        {
            //p1 - ? - ?
            min2 = Mathf.Min(d2O2, d2O3);
            if (min2 == d2O3)
            {

                Vector2 tmp = new Vector2(p2.x, p2.y);
                p2 = new Vector2(p3.x, p3.y);
                p3 = tmp;
            }

        }
        else if (min == d2O2)
        {

            Vector2 tmp = new Vector2(p1.x, p1.y);
            p1 = new Vector2(p2.x, p2.y);
            p2 = tmp;

            min2 = Mathf.Min(d2O1, d2O3);
            if (min2 == d2O3)
            {
                Vector2 tmp2 = new Vector2(p2.x, p2.y);
                p2 = new Vector2(p3.x, p3.y);
                p3 = tmp2;
            }
        }
        else
        {
            Vector2 tmp = new Vector2(p1.x, p1.y);
            p1 = new Vector2(p3.x, p3.y);
            p3 = tmp;

            min2 = Mathf.Min(d2O1, d2O2);
            if (min2 == d2O1)
            {
                Vector2 tmp2 = new Vector2(p2.x, p2.y);
                p2 = new Vector2(p3.x, p3.y);
                p3 = tmp2;
            }
        }
    }
}
