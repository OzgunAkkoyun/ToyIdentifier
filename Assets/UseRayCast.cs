using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UseRayCast : MonoBehaviour
{
    private CreateLine line;
    public Transform[] dots = new Transform[3];
    public TextMeshProUGUI debugText;
    private GameObject tempDragObject;
    public GameObject[] distanceTexts = new GameObject[3];

    void Start()
    {
        line = FindObjectOfType<CreateLine>();
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] = GameObject.Find("Dots").transform.GetChild(i);
        }

        for (int i = 0; i < distanceTexts.Length; i++)
        {
            distanceTexts[i] = GameObject.Find("DistanceTexts").transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos2D = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 dir = Vector2.zero;

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, dir);
            if (hit != null && hit.collider != null)
            {
                tempDragObject = hit.transform.gameObject;
                //hit.transform.position = mousePos2D;
                hit.transform.Find("Coord").GetComponent<TextMeshProUGUI>().text = mousePos2D.ToString();
                var dotsPos = dots.Select(item => item.gameObject.transform.position).ToArray();
                line.DrawLine(dotsPos);
            }

            if (tempDragObject != null) tempDragObject.transform.position = mousePos2D;
        }

        if (Input.GetMouseButtonUp(0))
        {
            tempDragObject = null;
            FindTriangleLefttoRight();
            
        }

    }

    private void FindAngle(Transform a ,Transform b,Transform c)
    {
        var dotsPos = dots.Select(item => item.gameObject.transform.position).ToArray();
        var posA = a.position.Vector3ToVector2();
        var posB = b.position.Vector3ToVector2();
        var posC = c.position.Vector3ToVector2();

        var distAB = Vector2.Distance(posA, posB);
        var distBC = Vector2.Distance(posB, posC);
        var distAC = Vector2.Distance(posA, posC);

        //var angleA = Mathf.Sqrt(distAB*distAB + distAC*distAC-)

        //                  c^2       +       b^2     -         a^2        /2   *   c    *    b
        float cosA = (distAB * distAB + distAC * distAC - distBC * distBC) / (2 * distAB * distAC);
        float cosB = (distAB * distAB + distBC * distBC - distAC * distAC) / (2 * distAB * distBC);
        float cosC = (distAC * distAC + distBC * distBC - distAB * distAB) / (2 * distAC * distBC);

        float angleA = Mathf.Acos(cosA);
        float angleB = Mathf.Acos(cosB);
        float angleC = Mathf.Acos(cosC);

        Debug.Log(angleA + angleB + angleC);

        var distABcm = Mathf.Sqrt(8 - 8 * Mathf.Cos(2 * angleC));

        distanceTexts[0].transform.position = (posA + posB) / 2f;
        //distanceTexts[0].GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs( (posA - posB).x)/2, 50);
        distanceTexts[0].GetComponent<TextMeshProUGUI>().text = distABcm.ToString("F3");

        var distBCcm = Mathf.Sqrt(8 - 8 * Mathf.Cos(2 * angleA));

        distanceTexts[1].transform.position = (posB + posC) / 2f;
        //distanceTexts[1].GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs((posC - posB).x)/2, 50);
        distanceTexts[1].GetComponent<TextMeshProUGUI>().text = distBCcm.ToString("F3");

        var distACcm = Mathf.Sqrt(8 - 8 * Mathf.Cos(2 * angleB));

        distanceTexts[2].transform.position = (posA + posC) / 2f;
        //distanceTexts[2].GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs((posC - posA).x)/2, 50);
        distanceTexts[2].GetComponent<TextMeshProUGUI>().text = distACcm.ToString("F3");

        //var angleAB = Mathf.Atan2(posB.y - posA.y, posB.x - posA.x) * Mathf.Rad2Deg;
        //var distABcm = Mathf.Sqrt(8 - 8 * Mathf.Cos(2* angleAB));

        //Vector3 dirAB = (posA.ToVector3() - posB.ToVector3()).normalized;


        //distanceTexts[0].transform.position = (posA + posB) / 2f;
        //distanceTexts[0].GetComponent<TextMeshProUGUI>().text = distAB.ToString();
        ////distanceTexts[0].transform.Rotate(new Vector3(distanceTexts[0].transform.rotation.x, distanceTexts[0].transform.rotation.y, angleAB));
        //var targetRotation = Quaternion.LookRotation(dirAB);

        //distanceTexts[0].transform.rotation = targetRotation;

        //Debug.Log("angleAB: " + angleAB);
        //Debug.Log("distAB: " + distAB);
    }

    void FindTriangleLefttoRight()
    {
        var touchList = dots.Select(item => item.gameObject.transform.position.Vector3ToVector2()).ToList();

        var sortedVectorsX = touchList.OrderBy(v => v.x).ToArray<Vector2>();
        var firstX = sortedVectorsX[0];
        var lastX = sortedVectorsX[2];

        var P = touchList.Where(v => (v != firstX && v != lastX)).ToList().First();

        var k = lastX - firstX;
        var l = P - firstX;

        var kontrol = k.x*l.y - k.y*l.x;

        List<Vector2> sortedTouchList = new List<Vector2>();
        if (kontrol > 0)
        {
            Debug.Log("sol");
            sortedTouchList.Add(firstX);
            sortedTouchList.Add(P);
            sortedTouchList.Add(lastX);
        }
        else if (kontrol < 0)
        {
            Debug.Log("sağ");

            sortedTouchList.Add(firstX);
            sortedTouchList.Add(lastX);
            sortedTouchList.Add(P);
        }
        else
        {
            Debug.Log("üzerinde");
        }


        //touchList.Remove(firstX);

        //var sortedVectorsY = touchList.OrderByDescending(v => v.y).ToArray<Vector2>();
        //var firstY = sortedVectorsY[0];
        //touchList.Remove(firstY);

        //List<Vector2> sortedTouchList = new List<Vector2>();

        //sortedTouchList.Add(firstX);
        //sortedTouchList.Add(firstY);
        //sortedTouchList.Add(touchList[0]);



        List<float> dist = new List<float>();

        dist.Add(Vector2.Distance(sortedTouchList[0],sortedTouchList[1]));
        dist.Add(Vector2.Distance(sortedTouchList[1],sortedTouchList[2]));
        dist.Add(Vector2.Distance(sortedTouchList[0],sortedTouchList[2]));

        var dotA = Array.Find(dots,v => v.position.Vector3ToVector2() == sortedTouchList[0]);
        dotA.Find("Points").GetComponent<TextMeshProUGUI>().text = "A";

        var dotB = Array.Find(dots, v => v.position.Vector3ToVector2() == sortedTouchList[1]);
        dotB.Find("Points").GetComponent<TextMeshProUGUI>().text = "B";

        var dotC = Array.Find(dots, v => v.position.Vector3ToVector2() == sortedTouchList[2]);
        dotC.Find("Points").GetComponent<TextMeshProUGUI>().text = "C";

        var tempDist = dist;

        var lowestDist = tempDist.OrderBy(v=>v).First();
        var lowestDistIndex = dist.FindIndex(e=>e==lowestDist);

        debugText.text = dist.ListPrint();

        while (lowestDistIndex!=0)
        {
            var a = dist[dist.Count - 1];
            dist.Remove(a);
            dist.Insert(0, a);
            lowestDistIndex = dist.FindIndex(e => e == lowestDist);
        }

        debugText.text += dist.ListPrint();

        FindAngle(dotA, dotB, dotC);
        //FindRotatePosition(dotA, dotB, dotC);
    }

    private void FindRotatePosition(Transform dotA, Transform dotB, Transform dotC)
    {
        var posA = dotA.position.Vector3ToVector2();
        var posB = dotB.position.Vector3ToVector2();
        var posC = dotC.position.Vector3ToVector2();


        var angleAB = Mathf.Atan2(posB.y - posA.y, posB.x - posA.x) * Mathf.Rad2Deg;

        Debug.Log(angleAB);
    }
}