using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliticianTextScript : MonoBehaviour {

    public Politician politician;
    public LocalPolitics localPoliticsManager;
    float rectHeight;
    float rectWidth;
    float rectX;
    float rectY;

    private void Start()
    {
        rectHeight = transform.GetComponent<RectTransform>().rect.height;
        rectWidth = transform.GetComponent<RectTransform>().rect.width;

        rectX = transform.GetComponent<RectTransform>().rect.position.x;
        rectY = transform.GetComponent<RectTransform>().rect.position.y;
    }
    private void Update()
    {
        Transform polInspectorPanel = GameObject.FindGameObjectWithTag("PolInspectorPanel").transform;

        bool mouseIsOverDialog = new Rect(polInspectorPanel.position - new Vector3(polInspectorPanel.GetComponent<RectTransform>().rect.width, polInspectorPanel.GetComponent<RectTransform>().rect.height / 2) + new Vector3(100, 0, 0), new Vector3(polInspectorPanel.GetComponent<RectTransform>().rect.width, polInspectorPanel.GetComponent<RectTransform>().rect.height)).Contains(Input.mousePosition);
        bool mouseIsOverPoliticianText = new Rect(new Vector2(transform.position.x - rectWidth / 4, transform.position.y - rectHeight / 4), new Vector3(rectWidth / 2, rectHeight / 2)).Contains(Input.mousePosition);

        if (mouseIsOverPoliticianText && !mouseIsOverDialog)
        {
            SetMenuToPolitician();
            polInspectorPanel.SetPositionAndRotation(new Vector3(transform.position.x + 100, transform.position.y), Quaternion.identity);
        }
        else
        {
            
            if (Input.GetMouseButtonDown(0) && !mouseIsOverDialog)
            {
                polInspectorPanel.SetPositionAndRotation(new Vector3(5000, 5000), Quaternion.identity);
            }
        }
    }
    public void SetMenuToPolitician()
    {
        GameObject.FindGameObjectWithTag("_Manager").GetComponent<LocalPolitics>().SelectPolitician(politician);
    }
}