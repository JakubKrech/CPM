using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{

    public CPMLogic CPM;
    public CirclesManager circlesManager;
    public Material materialRed, materialWhite;
    public List<LineRenderer> lineList;

    public void drawLines(List<CPMLogic.CPMAction> CriticalPathActions)
    {
        foreach (var line in lineList)
        {
            Destroy(line.gameObject);
        }
        lineList.Clear();

        foreach (var action in CPM.actionsGlobal)
        {
            Material chosenMaterial = materialWhite;

            foreach (var item in CriticalPathActions)
            {
                if(item.actionID == action.actionID) chosenMaterial = materialRed;
            }
            
            DrawLine(circlesManager.circleList[action.from - 1].transform.position, circlesManager.circleList[action.to-1].transform.position, Color.black, chosenMaterial);
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, Material material)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = material;
        //lr.SetColors(color, color);
        lr.SetWidth(0.7f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.transform.parent = this.transform;
        //GameObject.Destroy(myLine, duration);

        lineList.Add(lr);
    }

    // public void ColorCriticalPath(List<CPMLogic.CPMAction> criticalActionList)
    // {
    //     foreach (var circle in lineList)
    //     {
    //         foreach (var critical in criticalActionList)
    //         {
    //             print(critical.eventID.ToString() + " " + circle.Top.text);
    //             if(critical.eventID.ToString() == circle.Top.text) circle.circleSprite.color = Color.red;
    //         }
    //     }
    // }
}
