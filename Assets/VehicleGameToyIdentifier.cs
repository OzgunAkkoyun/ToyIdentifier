using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleGameToyIdentifier : MonoBehaviour
{
    private List<Vector2> touchList = new List<Vector2>();
    public ToolList toolList;
    public ToolListNew toolListNew;
    public TextMeshProUGUI text;
    public TextMeshProUGUI touchtext;
    public TextMeshProUGUI ratiotext;
    public Image toyImage;
    public Sprite[] images;
    public int whichToy = 0;
    public GameObject[] dots = new GameObject[10];
    private AudioSource audioSource;
    public TMP_InputField inputField;
    private float errorMargin => float.Parse(inputField.text);
    public TextMeshProUGUI errorText;
    public int tempct = 0;

    float[] score;
    public bool canUseToy { get; private set; }

    void Start()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] = GameObject.Find("Dots").transform.GetChild(i).gameObject;
        }

        audioSource = GetComponent<AudioSource>();
        //errorMarginText.text = errorMargin.ToString();
        //errorMargin.text = "0.03";
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

    }

    void Update()
    {
        HandleUserInput();
    }

    public void ClearButton()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].transform.position = new Vector2(-100,-100);
        }

        //errorText.text = "";
    }

    private void HandleUserInput()
    {
        int fingerCount = 0;

        float scwidth = Screen.width;
        float scheight = Screen.height;
        int ct = Input.touches.Length;
        touchtext.text = "TouchCount: "+ ct.ToString();

        //RecognizeMultipleTouch(ct,fingerCount);

        //RecognizeTriangleTouch(ct, fingerCount);
        RecognizeTriangleWithLine(ct, fingerCount);
    }

    private void RecognizeTriangleWithLine(int ct, int fingerCount)
    {
        if (ct == 3)
        {
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
        }
        else if (ct == 0)
        {
            canUseToy = true;
        }

        if (ct == 3 && canUseToy)
        {
            errorText.text = "";
            canUseToy = false;
            audioSource.Play();

            for (int i = 0; i < ct; i++)
            {
                dots[i].transform.position = new Vector3(touchList[i].x, touchList[i].y, 0);
                dots[i].transform.Find("Coord").GetComponent<TextMeshProUGUI>().text = touchList[i].ToString();
            }

            FindTriangleLefttoRight(touchList);
            
        }
    }

    void FindTriangleLefttoRight(List<Vector2> _touchList)
    {
        touchList = _touchList;

        var sortedVectorsX = touchList.OrderBy(v => v.x).ToArray<Vector2>();
        var firstX = sortedVectorsX[0];
        touchList.Remove(firstX);

        var sortedVectorsY = touchList.OrderByDescending(v => v.y).ToArray<Vector2>();
        var firstY = sortedVectorsY[0];
        touchList.Remove(firstY);

        List<Vector2> sortedTouchList = new List<Vector2>();

        sortedTouchList.Add(firstX);
        sortedTouchList.Add(firstY);
        sortedTouchList.Add(touchList[0]);

        List<float> dist = new List<float>();

        dist.Add(Vector2.Distance(sortedTouchList[0], sortedTouchList[1]));
        dist.Add(Vector2.Distance(sortedTouchList[1], sortedTouchList[2]));
        dist.Add(Vector2.Distance(sortedTouchList[0], sortedTouchList[2]));

        var tempDist = dist;

        var lowestDist = tempDist.OrderBy(v => v).First();
        var lowestDistIndex = dist.FindIndex(e => e == lowestDist);

        while (lowestDistIndex != 0)
        {
            var a = dist[dist.Count - 1];
            dist.Remove(a);
            dist.Insert(0, a);
            lowestDistIndex = dist.FindIndex(e => e == lowestDist);
        }

        var currentToyRatios = GetRatiosWithLine(dist);

        ratiotext.text = "Ratios: " + currentToyRatios[0] + " -- " + currentToyRatios[1] + " -- " + currentToyRatios[2] + "";

        if (isSmilarRatios(currentToyRatios))
        {
            UseTool(toolListNew.tools[whichToy].toolName + ". oyuncak");
            SetImage(toolListNew.tools[whichToy].toolName);
        }
        else
        {
            UseTool("Default");
            SetImage("13");
        }

    }

    public bool isSmilarRatios(List<float> currentToy)
    {
        whichToy = 0;

        score = new float[toolListNew.tools.Count];

        foreach (var tool in toolListNew.tools)
        {
            //tool.ratios.Sort();
            float a = 0f;
            var toolRatios = tool.ratios;

            var ratioCurrectCount = 0;

            for (int i = 0; i < 3; i++)
            {
                var min = toolRatios[i] - errorMargin;
                var max = toolRatios[i] + errorMargin;

                //errorMarginText.text +=whichToy + ": " + currentToy[i] + " -- " + min + " -- " + max+"\n";
                if (currentToy[i] >= min && currentToy[i] < max)
                {
                    ratioCurrectCount++;
                    a += Mathf.Abs(currentToy[i] - toolRatios[i]);
                }
            }

            if (ratioCurrectCount == 3)
            {
                score[whichToy] = a;
                //return true;
                //break;
            }

            whichToy++;

        }

        var lowestScore = score.Where(v => v != 0f).ToArray().OrderBy(v => v).First();
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

    void RecognizeTriangleTouch(int ct, int fingerCount)
    {
        if (ct == 3)
        {
            touchList.Clear();
            fingerCount = 0;

            foreach (Touch touch in Input.touches)
            {
                TouchPhase phase = touch.phase;
                // orientationText.text = "phase: " + phase;
                if (phase != TouchPhase.Ended)
                {
                    fingerCount++;
                    touchList.Add(new Vector2(touch.position.x, touch.position.y));
                }
            }
        }
        else if (ct == 0)
        {
            canUseToy = true;
        }

        if (ct == 3 && canUseToy)
        {
            canUseToy = false;

            Vector2 p1, p2, p3;

            p1 = new Vector2(touchList[0].x, touchList[0].y);
            p2 = new Vector2(touchList[1].x, touchList[1].y);
            p3 = new Vector2(touchList[2].x, touchList[2].y);

            audioSource.Play();

            for (int i = 0; i < ct; i++)
            {
                dots[i].transform.position = new Vector3(touchList[i].x, touchList[i].y, 0);
            }

            var currentToyRatios = GetRatios(p1, p2, p3);

            ratiotext.text = "Ratios: " + currentToyRatios[0] + " -- " + currentToyRatios[1] + " -- " + currentToyRatios[2] + "";

            if (isSmilarRatios(currentToyRatios))
            {
                UseTool(toolListNew.tools[whichToy].toolName + ". oyuncak");
                SetImage(toolListNew.tools[whichToy].toolName);
            }
            else
            {
                UseTool("Default");
                SetImage("13");
            }

        }
    }

    void RecognizeMultipleTouch(int ct,int fingerCount)
    {
        if (ct > 0 && tempct != ct)
        {
            touchList.Clear();
           
            //errorText.text += "if\n";
            foreach (Touch touch in Input.touches)
            {
                TouchPhase phase = touch.phase;
                // orientationText.text = "phase: " + phase;
                if (phase != TouchPhase.Ended)
                {
                    fingerCount++;
                    touchList.Add(new Vector2(touch.position.x, touch.position.y));
                }
            }
        }
        else if (ct == 0)
        {
            canUseToy = true;
            tempct = 0;
        }

        if (ct > 0 && ct != tempct)
        {
            canUseToy = false;
            tempct = ct;

            audioSource.Play();
            for (int i = 0; i < ct; i++)
            {
                dots[i].transform.position = new Vector3(touchList[i].x, touchList[i].y, 0);
                //errorText.text += "firstfor: " + i + "\n";

            }

            for (int i = ct; i < dots.Length; i++)
            {
                //errorText.text += "secondfor: " + i + "\n";

                dots[i].transform.position = new Vector3(-100, -100, 0);
            }
        }
    }

    void isToysSame()
    {
        var tools = toolListNew.tools;
        for (int i = 0; i < tools.Count; i++)
        {
            var currentToy = tools[i];
            var currentToyRatios = currentToy.ratios;

            for (int j = 0; j < tools.Count; j++)
            {
                var toolRatios = tools[j].ratios;
                var ratioCurrectCount = 0;

                if (i != j)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        var min = toolRatios[k] - errorMargin;
                        var max = toolRatios[k] + errorMargin;

                        //errorMarginText.text +=whichToy + ": " + currentToy[i] + " -- " + min + " -- " + max+"\n";
                        if (currentToyRatios[k] >= min && currentToyRatios[k] < max)
                        {
                            ratioCurrectCount++;
                        }
                    }

                    if (ratioCurrectCount == 3)
                    {
                        Debug.Log(i + ". oyuncak " + errorMargin + "errorMargin ile" + j + ". oyuncakla benzer.");
                    }
                }
            }
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
        r2 = d1 / d3;
        r3 = d2 / d3;
        r1 = r1 >= 1 ? 1 / r1 : r1;
        r2 = r2 >= 1 ? 1 / r2 : r2;
        r3 = r3 >= 1 ? 1 / r3 : r3;

        ratios.Add(r1);
        ratios.Add(r2);
        ratios.Add(r3);
        //ratios.Sort();

        return ratios;
    }

    public List<float> GetRatios(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float d1, d2, d3, r1, r2, r3 = 0;
        List<float> ratios = new List<float>();

        d1 = Vector2.Distance(p1, p2);
        d2 = Vector2.Distance(p1, p3);
        d3 = Vector2.Distance(p2, p3);
        r1 = d1 / d2;
        r2 = d1 / d3;
        r3 = d2 / d3;
        r1 = r1 >= 1 ? 1 / r1 : r1;
        r2 = r2 >= 1 ? 1 / r2 : r2;
        r3 = r3 >= 1 ? 1 / r3 : r3;

        ratios.Add(r1);
        ratios.Add(r2);
        ratios.Add(r3);
        ratios.Sort();

        return ratios;
    }

    private void UseTool(string toolName)
    {
        text.text = toolName;
        //switch (toolName)
        //{
        //    case "MaviKare":
        //        Debug.Log(toolName);
        //        break;
        //    case "Balon":
        //        Debug.Log(toolName);
        //        break;
        //    case "Bisiklet":
        //        Debug.Log(toolName);
        //        break;
           
        //}
    }

    void SetImage(string name)
    {
       
        toyImage.sprite = images[int.Parse(name) - 1];
    }
}
