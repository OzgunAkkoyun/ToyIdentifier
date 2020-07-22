using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TriangleShowForDragDrop : MonoBehaviour
{
    public GameObject[] dots = new GameObject[3];
    void Start()
    {

        //for (int i = 0; i < dots.Length; i++)
        //{
        //    dots[i] = GameObject.Find("Dots").transform.GetChild(i).gameObject;
        //}
        Vector2 p1, p2, p3;
        List<Vector2> touchList1 = new List<Vector2>();
        touchList1.Add(new Vector2(100, 300));
        touchList1.Add(new Vector2(500, 500));
        touchList1.Add(new Vector2(350, 200));
        for (int i = 0; i < touchList1.Count; i++)
        {
            dots[i].transform.position = touchList1[i];
            dots[i].transform.Find("Coord").GetComponent<TextMeshProUGUI>().text = touchList1[i].ToString();
        }
        //errorText.text += p1.ToString() + " -- " + p2.ToString() + " -- " + p3.ToString() + "\n";

        var sortedVectors = touchList1.OrderBy(v => v.x).ToArray<Vector2>();
        Debug.Log(sortedVectors[0]);
        var index = touchList1.FindIndex(e => e.x == sortedVectors[0].x);
        Debug.Log(index);
    }

   
}
