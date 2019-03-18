using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclesManager : MonoBehaviour
{
    
    public CPMLogic CPMData;
    public List<Circle> circleList;
    public Circle circlePrefab;




    public void drawCircles()
    {
        foreach (var circle in circleList)
        {
            Destroy(circle.gameObject);
        }
        circleList.Clear();

        int currentDeepness = 0;
        int curDeepAmmount = 0;
        int maxDeepAmmount = 0;

        while(true){
            curDeepAmmount = 0;
            foreach (var cpmEvent in CPMData.eventsGlobal)
            {
                if(cpmEvent.deepness == currentDeepness) curDeepAmmount++;
            }
            maxDeepAmmount = curDeepAmmount;
            if(curDeepAmmount == 0) break;

            for(int i = 0; i < CPMData.eventsGlobal.Count; i++)
            {
                if(CPMData.eventsGlobal[i].deepness == currentDeepness)
                {
                    float yAxisDiff = 0;
                    if(curDeepAmmount > 1) yAxisDiff = 3.0f * --curDeepAmmount;
                    yAxisDiff -= (3.0f * maxDeepAmmount)/2;
                    
                    Vector3 position = new Vector3(this.transform.position.x + currentDeepness * 2.5f + i * Random.Range(0.5f, 1f), this.transform.position.y + yAxisDiff, this.transform.position.z);

                    Circle newCircle = (Circle)Instantiate(circlePrefab, position, Quaternion.identity);

                    newCircle.Left.text = CPMData.eventsGlobal[i].earliest.ToString();
                    newCircle.Right.text = CPMData.eventsGlobal[i].latest.ToString();
                    newCircle.Top.text = CPMData.eventsGlobal[i].eventID.ToString();
                    newCircle.Bottom.text = CPMData.eventsGlobal[i].spareTime.ToString();

                    newCircle.name = "Circle " + CPMData.eventsGlobal[i].eventID.ToString();
                    newCircle.transform.parent = this.transform;

                    circleList.Add(newCircle); 
                }
                
            }
            currentDeepness++;
        }
        
    }

    public void ColorCriticalPath(List<CPMLogic.CPMEvent> criticalEventList)
    {
        foreach (var circle in circleList)
        {
            foreach (var critical in criticalEventList)
            {
                print(critical.eventID.ToString() + " " + circle.Top.text);
                if(critical.eventID.ToString() == circle.Top.text) circle.circleSprite.color = Color.red;
            }
        }
    }

}
