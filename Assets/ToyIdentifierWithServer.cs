using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dist
{
    public List<float> dist_a = new List<float>();
    public List<float> dist_b = new List<float>();
    public List<float> dist_c = new List<float>();
}

public class ToyIdentifierWithServer : MonoBehaviour
{
    private List<Vector2> touchList = new List<Vector2>();
    private List<Touch> touchListforRotate = new List<Touch>();
    public ToolListNew toolListNew;

    public List<AllToys> toys = new List<AllToys>();
    public TextMeshProUGUI text;
    public TextMeshProUGUI dpiToyName;
    public TextMeshProUGUI touchtext;
    public TextMeshProUGUI ratiotext;
    public TextMeshProUGUI distanceDpi;
    public TextMeshProUGUI rotatetext;
    public TextMeshProUGUI ratetext;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI rotateObjectsCount;

    public Image toyImage;
    public Sprite[] images;
    public int whichToy = 0;

    public GameObject[] dots = new GameObject[3];

    private AudioSource audioSource;

    public TMP_InputField inputField;
    public TMP_InputField inputFieldR;
    public TMP_InputField inputFieldRotation;

    private float errorMargin => float.Parse(inputField.text);
    private float r => float.Parse(inputFieldR.text);
    private float rotationSensivity => float.Parse(inputFieldRotation.text);

    public TextMeshProUGUI errorText;
    public GameObject toyPanel;
    public GameObject lockPanel;

    public List<GameObject> rotateObjects = new List<GameObject>();
    public float maxAngle;
    public float rotationAngle;
    public float singleObjectAngle = 30;

    float[] score;
    public bool canUseToy { get; private set; }
    public bool canUseRotate = false;

    Dist dist = new Dist();

    //Connection variables

    private string url = "http://www.merakliabidin.com/toyidentifier/getRatios.php";

    void Start()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] = GameObject.Find("Dots").transform.GetChild(i).gameObject;
        }

        GetRotateObjects();

        audioSource = GetComponent<AudioSource>();

        //var currentToyRatios = GetRatiosWithLine(tempDist);

        //var a = toolListNew.tools[11];

        //var currentToyRatios = new List<float>() { a.ratios[0], a.ratios[1], a.ratios[2] };
        //Debug.Log(currentToyRatios[0] + " -- " + currentToyRatios[1] + " -- " + currentToyRatios[2]);
        //ratiotext.text = "Ratios: " + currentToyRatios[0] + " -- " + currentToyRatios[1] + " -- " + currentToyRatios[2];

        //if (isSmilarRatios(currentToyRatios))
        //{
        //    Debug.Log("---------------------" + toolListNew.tools[whichToy].toolName);
        //    UseTool(toolListNew.tools[whichToy].toolName);
        //    SetImage(toolListNew.tools[whichToy].toolName);
        //}
        //isToysSame();
        GetToys();

    }

    public void dene()
    {
        touchList.Add(new Vector2(911.3f, 71.9f));
        touchList.Add(new Vector2(1000.2f, 233.7f));
        touchList.Add(new Vector2(1012.2f, 167.8f));
        for (int i = 0; i < 3; i++)
        {
            dots[i].transform.position = new Vector3(touchList[i].x, touchList[i].y, 0);
            dots[i].transform.Find("Coord").GetComponent<TextMeshProUGUI>().text = touchList[i].ToString();
        }
        FindTriangleLefttoRight();
    }

    public void GetRotateObjects()
    {
        rotateObjects.Clear();
        for (int i = 0; i < GameObject.Find("RotateObjects").transform.childCount; i++)
        {
            rotateObjects.Add(GameObject.Find("RotateObjects").transform.GetChild(i).gameObject);
        }

        maxAngle = singleObjectAngle * rotateObjects.Count;
        rotateObjectsCount.text = rotateObjects.Count.ToString();
    }

    public void GetToys()
    {
        StartCoroutine("GetConnection");
    }

    IEnumerator GetConnection()
    {
        WWW www = new WWW(url);
        yield return www;

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            var toysDist = JSON.Parse(www.text);
            toys.Clear();
            errorText.text = "";
            for (int i = 0; i < toysDist.Count; i++)
            {
                List<float> ratios = new List<float>();
                List<float> distances = new List<float>();

                distances.Add(toysDist[i]["dist_a"]);
                distances.Add(toysDist[i]["dist_b"]);
                distances.Add(toysDist[i]["dist_c"]);

                ratios = GetRatiosWithLine(distances);

                toys.Add(new AllToys(ratios, distances, toysDist[i]["toy_index"], (i + 1).ToString()));
                errorText.text += (i + 1) + ". toy-->" + distances[0] + "  " + distances[1] + "  " + distances[2] +
                                  "\n";
            }

            foreach (var toy in toys)
            {
                if (dist.dist_a.IndexOf(toy.distances[0]) < 0)
                {
                    dist.dist_a.Add(toy.distances[0]);
                }
                if (dist.dist_b.IndexOf(toy.distances[1]) < 0)
                {
                    dist.dist_b.Add(toy.distances[1]);
                }
                if (dist.dist_c.IndexOf(toy.distances[2]) < 0)
                {
                    dist.dist_c.Add(toy.distances[2]);
                }
            }

            errorText.text += "Dist values:\n" + "For edge A: " + dist.dist_a.ListPrint() + "\n" + "For edge B: " +
                              dist.dist_b.ListPrint() + "\n" + "For edge C: " + dist.dist_c.ListPrint() + "\n";

        }

    }

    void Update()
    {
        HandleUserInput();
    }

    public void ClearButton()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].transform.position = new Vector2(-100, -100);
        }

        text.text = "Clear";
        rotatetext.text = "";
        toyImage.sprite = images[10];
        //errorText.text = "";
    }

    private void HandleUserInput()
    {
        int fingerCount = 0;

        int ct = Input.touches.Length;
        touchtext.text = "TouchCount: " + ct.ToString();

        if (ct == 3)
        {

        }
        else if (ct == 0)
        {
            canUseToy = true;
            canUseRotate = false;
            //CancelInvoke("ToyRotate");
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    ct = 3;
        //    touchList.Add(new Vector2(100, 100));
        //    touchList.Add(new Vector2(200, 300));
        //    touchList.Add(new Vector2(400, 200));
        //}

        if (ct == 3 && canUseToy)
        {
            canUseToy = false;

            touchList.Clear();
            fingerCount = 0;
            
            foreach (Touch touch in Input.touches)
            {
                TouchPhase phase = touch.phase;

                if (phase != TouchPhase.Ended)
                {
                    fingerCount++;
                    touchList.Add(new Vector2(touch.position.x, touch.position.y));
                }
            }

            audioSource.Play();

            for (int i = 0; i < ct; i++)
            {
                dots[i].transform.position = new Vector3(touchList[i].x, touchList[i].y, 0);
                dots[i].transform.Find("Coord").GetComponent<TextMeshProUGUI>().text = touchList[i].ToString();
            }

            FindTriangleLefttoRight();
        }

        if (canUseRotate)
        {
            ToyRotate();
        }
    }

    void FindTriangleLefttoRight()
    {
        List<Vector2> sortedTouchList = new List<Vector2>();

        var touchList = dots.Select(item => item.gameObject.transform.position.Vector3ToVector2()).ToList();

        var sortedVectorsX = touchList.OrderBy(v => v.x).ToArray<Vector2>();
        var firstX = sortedVectorsX[0];
        var lastX = sortedVectorsX[2];

        var P = touchList.Where(v => (v != firstX && v != lastX)).ToList().First();

        var k = lastX - firstX;
        var l = P - firstX;

        var kontrol = k.x * l.y - k.y * l.x;

        if (kontrol > 0)
        {
            sortedTouchList.Add(firstX);
            sortedTouchList.Add(P);
            sortedTouchList.Add(lastX);
        }
        else if (kontrol < 0)
        {
            sortedTouchList.Add(firstX);
            sortedTouchList.Add(lastX);
            sortedTouchList.Add(P);
        }
        else
        {
            Debug.Log("üzerinde");
        }

        List<float> dist = new List<float>();

        dist.Add(Vector2.Distance(sortedTouchList[0], sortedTouchList[1]));
        dist.Add(Vector2.Distance(sortedTouchList[1], sortedTouchList[2]));
        dist.Add(Vector2.Distance(sortedTouchList[0], sortedTouchList[2]));
        //var dotA = Array.Find(dots, v => v.transform.position.Vector3ToVector2() == sortedTouchList[0]);
        ////dotA.transform.Find("Points").GetComponent<TextMeshProUGUI>().text = "A";

        //var dotB = Array.Find(dots, v => v.transform.position.Vector3ToVector2() == sortedTouchList[1]);
        ////dotB.transform.Find("Points").GetComponent<TextMeshProUGUI>().text = "B";

        //var dotC = Array.Find(dots, v => v.transform.position.Vector3ToVector2() == sortedTouchList[2]);
        ////dotC.transform.Find("Points").GetComponent<TextMeshProUGUI>().text = "C";

        //var tempDist = dist;

        //var lowestDist = tempDist.OrderBy(v => v).First();
        //var lowestDistIndex = dist.FindIndex(e => e == lowestDist);

        //while (lowestDistIndex != 0)
        //{
        //    var a = dist[dist.Count - 1];
        //    dist.Remove(a);
        //    dist.Insert(0, a);
        //    lowestDistIndex = dist.FindIndex(e => e == lowestDist);
        //}

        //var currentToyRatios = GetRatiosWithLine(dist);

        //ratiotext.text = "Ratios: " + currentToyRatios[0] + " -- " + currentToyRatios[1] + " -- " + currentToyRatios[2];

        //FindAngle(dotA.transform, dotB.transform, dotC.transform);
        FindAngle(sortedTouchList);
        FindAngleWithDpi(sortedTouchList);

        //if (isSmilarRatios(currentToyRatios))
        //{
        //    UseTool(toys[whichToy].toolName + ". oyuncak");
        //    SetImage(toys[whichToy].toolName);
        //}
        //else
        //{
        //    UseTool("Default");
        //    SetImage("11");

        //    
        //    //InvokeRepeating("ToyRotate",0f,0.1f);
        //}

    }

    void ToyRotate()
    {
        touchListforRotate = Input.touches.ToList();
        var prevPos1 =
            touchListforRotate[0].position -
            touchListforRotate[0].deltaPosition; // Generate previous frame's finger positions
        var prevPos2 = touchListforRotate[1].position - touchListforRotate[1].deltaPosition;

        var prevDir = prevPos2 - prevPos1;
        var currDir = touchListforRotate[1].position - touchListforRotate[0].position;

        var angle = Vector2.Angle(prevDir, currDir);

        var a = Vector3.Cross(prevDir.ToVector3(), currDir.ToVector3());

        rotationAngle += a.z > 0 ? -angle * rotationSensivity : angle * rotationSensivity;

        rotationAngle = rotationAngle > maxAngle ? maxAngle : rotationAngle;
        rotationAngle = rotationAngle < 0 ? 0 : rotationAngle;

        rotatetext.text = "Rotate: " + rotationAngle.ToString("F2");

        RotationToySelect(rotationAngle);
    }

    private void RotationToySelect(float angle)
    {
        int whichCondition = (int) Mathf.Floor(angle / singleObjectAngle);

        rotateObjects[whichCondition].GetComponent<Image>().color = Color.blue;

        foreach (var item in rotateObjects.Where((v, index) => index != whichCondition).ToList())
        {
            item.GetComponent<Image>().color = Color.white;
        }

    }

    public void RotationToyAddorRemove(string sign)
    {
        if (sign == "+")
        {
            var a = Instantiate(rotateObjects[0], rotateObjects[0].transform);
            a.transform.parent = GameObject.Find("RotateObjects").transform;
        }
        else
        {
            DestroyImmediate(rotateObjects[0]);
        }

        GetRotateObjects();

    }

    private void FindAngle(List<Vector2> sortedList)
    {
        var posA = sortedList[0];
        var posB = sortedList[1];
        var posC = sortedList[2];

        Debug.Log(sortedList.ListPrint());
        var distAB = Vector2.Distance(posA, posB);
        var distBC = Vector2.Distance(posB, posC);
        var distAC = Vector2.Distance(posA, posC);

        //                  c^2       +       b^2       -       a^2        /2   *   c    *    b
        float cosA = (distAB * distAB + distAC * distAC - distBC * distBC) / (2 * distAB * distAC);
        float cosB = (distAB * distAB + distBC * distBC - distAC * distAC) / (2 * distAB * distBC);
        float cosC = (distAC * distAC + distBC * distBC - distAB * distAB) / (2 * distAC * distBC);

        float angleA = Mathf.Acos(cosA);
        float angleB = Mathf.Acos(cosB);
        float angleC = Mathf.Acos(cosC);

        angleText.text = "a= " + angleA*Mathf.Rad2Deg + "° b= " + angleB * Mathf.Rad2Deg + "° c= " + angleC * Mathf.Rad2Deg + "°";

        var katsayi = 2 * r * r; 

        var distABcm = Mathf.Sqrt(katsayi - katsayi * Mathf.Cos(2 * angleC));

        var distBCcm = Mathf.Sqrt(katsayi - katsayi * Mathf.Cos(2 * angleA));

        var distACcm = Mathf.Sqrt(katsayi - katsayi * Mathf.Cos(2 * angleB));

        ratetext.text = Screen.dpi + "--"+ Screen.dpi/2.54f + " :: " + distAB / distABcm +"--" + distBC / distBCcm + "--" + distAC / distACcm;
        List<float> tempDistcm = new List<float>();

        tempDistcm.Add(distABcm);
        tempDistcm.Add(distBCcm);
        tempDistcm.Add(distACcm);

        var lowestDist = tempDistcm.OrderBy(v => v).First();
        var lowestDistIndex = tempDistcm.FindIndex(e => e == lowestDist);

        while (lowestDistIndex != 0)
        {
            var lastElement = tempDistcm[tempDistcm.Count - 1];
            tempDistcm.Remove(lastElement);
            tempDistcm.Insert(0, lastElement);
            lowestDistIndex = tempDistcm.FindIndex(e => e == lowestDist);
        }

        ratiotext.text = "Dist:" + tempDistcm[0].ToString("F3") + " -- " + tempDistcm[1].ToString("F3") + " -- " +
                         tempDistcm[2].ToString("F3") + "\n";

        if (isSmilarDistanceOneByOne(tempDistcm))
        {
            canUseRotate = true;
            rotationAngle = 0;
            UseTool(toys[whichToy].toolIndex + ". oyuncak");
            SetImage(toys[whichToy].toolName);

            ratiotext.text +="Find:" + toys[whichToy].distances.ListPrint();
        }
        else
        {
            UseTool("Default");
            SetImage("13");
        }

    }

    private void FindAngleWithDpi(List<Vector2> sortedList)
    {
        var posA = sortedList[0];
        var posB = sortedList[1];
        var posC = sortedList[2];
        Debug.Log(sortedList.ListPrint());
        var distAB = Vector2.Distance(posA, posB);
        var distBC = Vector2.Distance(posB, posC);
        var distAC = Vector2.Distance(posA, posC);

        var dpiDistances = new List<float>();
        var screenDpiForcm = Screen.dpi / 2.54f;

        dpiDistances.Add(distAB / screenDpiForcm);
        dpiDistances.Add(distBC / screenDpiForcm);
        dpiDistances.Add(distAC / screenDpiForcm);

        var lowestDist = dpiDistances.OrderBy(v => v).First();
        var lowestDistIndex = dpiDistances.FindIndex(e => e == lowestDist);

        while (lowestDistIndex != 0)
        {
            var lastElement = dpiDistances[dpiDistances.Count - 1];
            dpiDistances.Remove(lastElement);
            dpiDistances.Insert(0, lastElement);
            lowestDistIndex = dpiDistances.FindIndex(e => e == lowestDist);
        }

        distanceDpi.text = "DpiD:" + dpiDistances[0].ToString("F3") + " -- " + dpiDistances[1].ToString("F3") + " -- " +
                         dpiDistances[2].ToString("F3") + "\n";

        if (isSmilarDistanceOneByOne(dpiDistances))
        {
            canUseRotate = true;
            rotationAngle = 0;
            dpiToyName.text = toys[whichToy].toolName + ". oyuncak";
            distanceDpi.text += "Find:" + toys[whichToy].distances.ListPrint();
        }
        else
        {
            dpiToyName.text = "Default";
        }

    }
    //This function is checking all edges and find the current toy's nearest toy value and change it to that.
    public bool isSmilarDistanceOneByOne(List<float> currentToy)
    {
        var witchEdge = new List<float>();
        var nearestList = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            if (i == 0) //i == 0 is edge A
            {
                witchEdge = dist.dist_a;
            }
            else if (i == 1) //i == 1 is edge B
            {
                witchEdge = dist.dist_b;
            }
            else //i == 2 is edge C
            {
                witchEdge = dist.dist_c;
            }

            var nearest = witchEdge.OrderBy(x => Math.Abs(x - currentToy[i])).First();
            nearestList.Add(nearest);
        }

        whichToy = 0;

        score = new float[toys.Count];

        foreach (var toy in toys)
        {
            float scoreValue = 0f;
            var toolDistances = toy.distances;

            var ratioCurrectCount = 0;

            for (int i = 0; i < 3; i++)
            {
                scoreValue += Mathf.Abs(nearestList[i] - toolDistances[i]);
            }
            
            score[whichToy] = scoreValue;
            whichToy++;
        }

        var lowestScore = score.OrderBy(v => v).First();

        whichToy = Array.FindIndex(score, e => e == lowestScore);
     
        if (score[whichToy] == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        //return isSmilarDistance(nearestList);
    }

    public bool isSmilarDistance(List<float> currentToy)
    {
        whichToy = 0;

        score = new float[toys.Count];

        foreach (var toy in toys)
        {
            //tool.ratios.Sort();
            float scoreValue = 0f;
            var toolDistances = toy.distances;

            var ratioCurrectCount = 0;

            for (int i = 0; i < 3; i++)
            {
                //var min = toolRatios[i] - errorMargin;
                //var max = toolRatios[i] + errorMargin;

                //if (currentToy[i] >= min && currentToy[i] < max)
                //{
                //    ratioCurrectCount++;
                scoreValue += Mathf.Abs(currentToy[i] - toolDistances[i]);
                //}
            }

            score[whichToy] = scoreValue;
            whichToy++;

        }

        var lowestScore = score.OrderBy(v => v).First();

        whichToy = Array.FindIndex(score, e => e == lowestScore);

        if (score[whichToy] == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool isSmilarRatios(List<float> currentToy)
    {
        whichToy = 0;

        score = new float[toys.Count];

        foreach (var toy in toys)
        {
            //tool.ratios.Sort();
            float a = 0f;
            var toolRatios = toy.ratios;

            var ratioCurrectCount = 0;

            for (int i = 0; i < 3; i++)
            {
                var min = toolRatios[i] - errorMargin;
                var max = toolRatios[i] + errorMargin;

                if (currentToy[i] >= min && currentToy[i] < max)
                {
                    ratioCurrectCount++;
                    a += Mathf.Abs(currentToy[i] - toolRatios[i]);
                }
            }

            score[whichToy] = a;
            whichToy++;

        }

        var lowestScore = score.OrderBy(v => v).First();

        whichToy = Array.FindIndex(score, e => e == lowestScore);

        if (score[whichToy] == 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public List<float> GetRatiosWithLine(List<float> dist)
    {
        float d1, d2, d3, r1, r2, r3 = 0;
        List<float> ratios = new List<float>();

        d1 = dist[0];
        d2 = dist[1];
        d3 = dist[2];
        r1 = d1 / d2;
        r2 = d2 / d3;
        r3 = d1 / d3;
        r1 = r1 >= 1 ? 1 / r1 : r1;
        r2 = r2 >= 1 ? 1 / r2 : r2;
        r3 = r3 >= 1 ? 1 / r3 : r3;

        ratios.Add(r1);
        ratios.Add(r2);
        ratios.Add(r3);
        //ratios.Sort();

        return ratios;
    }

    private void UseTool(string toolName)
    {
        text.text = toolName;
    }

    void SetImage(string name)
    {
        toyImage.sprite = images[int.Parse(name) - 1];
    }

    public void OpenShowToysPanel()
    {
        toyPanel.SetActive(!toyPanel.activeSelf);
    }

    public void OpenShowLockPanel()
    {
        lockPanel.SetActive(!lockPanel.activeSelf);
    }
}
