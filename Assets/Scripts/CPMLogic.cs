using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPMLogic : MonoBehaviour
{
    public InputFieldGenerator InputFieldGenerator;
    public Text CriticalActionPathText, CriticalEventPathText, CriticalTimeText;
    public CirclesManager CirclesManager;
    public LineManager LineManager;
    public List<CPMAction> actionsGlobal;
    public List<CPMEvent> eventsGlobal;
    public Toggle predefinedConfigToggle;

    public class CPMAction
    {
        public char actionID;
        public int duration;
        public int from, to;

        public CPMAction(char id, int d, int f, int t) 
        {
            actionID = id;
            duration = d;
            from = f;
            to = t;
        }

        public void Print() {
            Debug.Log(actionID + " " + duration + " " + from + " " + to);
        }
    };

    public class CPMEvent
    {
        public int eventID;

        public List<CPMAction> In = new List<CPMAction>();
        public List<CPMAction> Out = new List<CPMAction>();

        public int earliest = 0;
        public int latest = int.MaxValue;
        public int spareTime = 0;
        public int deepness = int.MaxValue;

        public CPMEvent(int id, int e, int l, int s) 
        {
            eventID = id;
            earliest = e;
            latest = l;
            spareTime = s;
        }
        public CPMEvent(int id)
        {
            eventID = id;
        }
        
        public void Print() {
            Debug.Log(eventID + " " + earliest + " " + latest + " " + spareTime);
        }
    };
    
    public void CalculateCPM()
    {
        List<CPMAction> Actions;
        
        if(predefinedConfigToggle.isOn)
        {
            Actions = new List<CPMAction> {
                new CPMAction('A', 3, 1, 2),
                new CPMAction('B', 4, 2, 3),
                new CPMAction('C', 6, 2, 4),
                new CPMAction('D', 7, 3, 5),
                new CPMAction('E', 1, 5, 7),
                new CPMAction('F', 2, 4, 7),
                new CPMAction('G', 3, 4, 6),
                new CPMAction('H', 4, 6, 7),
                new CPMAction('I', 1, 7, 8),
                new CPMAction('J', 2, 8, 9)
            };
        }
        else Actions = ReadActionsInfoFromInputFields();

        foreach (var item in Actions)
        {
            item.Print();
        }

        List<CPMEvent> Events = createEvents(Actions);
        
        assignActionsToEvents(Actions, ref Events);

        calculateEarliest(ref Events);
        Events[Events.Count - 1].latest = Events[Events.Count - 1].earliest;
        calculateLatest(ref Events);

        foreach (var item in Events)
        {
            item.Print();
        }

        List<CPMEvent> CriticalPathEvents = new List<CPMEvent>{Events[Events.Count-1]};
        List<CPMAction> CriticalPathActions = new List<CPMAction>();
        chooseCPM(Events, Events[Events.Count - 1], ref CriticalPathEvents, ref CriticalPathActions);

        string cpe = "";
        foreach (CPMEvent ev in CriticalPathEvents)
        {
            cpe += ev.eventID + " ";
        }
        CriticalActionPathText.text = cpe;
        
        string cpa = "";
        foreach (CPMAction ac in CriticalPathActions)
        {
            cpa += ac.actionID + " ";
        }
        CriticalEventPathText.text = cpa;

        CriticalTimeText.text = calculateCPMTime(CriticalPathActions).ToString();

        actionsGlobal = Actions;
        eventsGlobal = Events;

        CPMEvent firstEvent = Events[0];
        firstEvent.deepness = 0;
        calculateDeepness(ref firstEvent);

        foreach (CPMEvent ev in Events)
        {
            print(ev.deepness);
        }

        CirclesManager.drawCircles();
        LineManager.drawLines(CriticalPathActions);

        CirclesManager.ColorCriticalPath(CriticalPathEvents);
    }

    List<CPMAction> ReadActionsInfoFromInputFields()
    {
        List<CPMAction> actions = new List<CPMAction>();

        int inputAmmount = int.Parse(InputFieldGenerator.ammountOfInputsText.text);
        for(int i = 0; i < inputAmmount; i++)
        {
            string input = InputFieldGenerator.inputFieldList[i].inputText.text;
            string[] subStrings = input.Split(',');

            actions.Add(new CPMAction(char.Parse(subStrings[0]), int.Parse(subStrings[1]), int.Parse(subStrings[2]), int.Parse(subStrings[3])));
        }

        return actions;
    }

    // public void LoadPredefinedConfig()
    // {
    //     List<CPMAction> Actions = new List<CPMAction> {
    //         new CPMAction('A', 3, 1, 2),
    //         new CPMAction('B', 4, 2, 3),
    //         new CPMAction('C', 6, 2, 4),
    //         new CPMAction('D', 7, 3, 5),
    //         new CPMAction('E', 1, 5, 7),
    //         new CPMAction('F', 2, 4, 7),
    //         new CPMAction('G', 3, 4, 6),
    //         new CPMAction('H', 4, 6, 7),
    //         new CPMAction('I', 1, 7, 8),
    //         new CPMAction('J', 2, 8, 9)
    //     };
    // }

    List<CPMEvent> createEvents(List<CPMAction> ActionList)
    {
        List<CPMEvent> Events = new List<CPMEvent> { new CPMEvent(1, 0, 0, 0) };

        int lastEventID = 1;

        foreach (CPMAction ac in ActionList)
        {
            if (ac.to > lastEventID) lastEventID = ac.to;
        }

        for (int i = 2; i <= lastEventID; i++) {
            Events.Add(new CPMEvent(i));
        }

        return Events;
    }

    void assignActionsToEvents(List<CPMAction> ActionList, ref List<CPMEvent> EventList)
    {
        List<CPMEvent> Events = new List<CPMEvent> { new CPMEvent(1, 0, 0, 0) };

        foreach (CPMAction ac in ActionList)
        {
            EventList[ac.from - 1].Out.Add(ac);
            EventList[ac.to - 1].In.Add(ac);
        }
    }

    void calculateEarliest(ref List<CPMEvent> EventList)
    {
        foreach (CPMEvent ev in EventList)
        {
            foreach (CPMAction ac in ev.Out)
            {
                if(ev.earliest + ac.duration > EventList[ac.to - 1].earliest) EventList[ac.to - 1].earliest = ev.earliest + ac.duration;
            }
        }
    }

    void calculateLatest(ref List<CPMEvent> EventList)
    {
        for (int i = EventList.Count - 1; i >= 0; i--)
        {
            CPMEvent ev = EventList[i];

            foreach (CPMAction ac in ev.In)
            {
                if (ev.latest - ac.duration < EventList[ac.from - 1].latest) EventList[ac.from - 1].latest = ev.latest - ac.duration;
            }
        }
    }

    void chooseCPM(List<CPMEvent> EventList, CPMEvent current, ref List<CPMEvent> CriticalPathEvents, ref List<CPMAction> CriticalPathActions)
    {
        if (current.In.Count != 0)
        {
            CPMEvent next;
            foreach (CPMAction action in current.In)
            {
                if (current.latest - action.duration == EventList[action.from - 1].earliest)
                {
                    next = EventList[action.from - 1];
                    
                    CriticalPathEvents.Insert(0, EventList[action.from - 1]);
                    CriticalPathActions.Insert(0, action);

                    chooseCPM(EventList, next, ref CriticalPathEvents, ref CriticalPathActions);
                    break;
                }
            }
        }
    }

    int calculateCPMTime(List<CPMAction> CriticalPathActions)
    {
        int totalTime = 0;

        foreach (var ac in CriticalPathActions) 
            totalTime += ac.duration;

        return totalTime;
    }

    void calculateDeepness(ref CPMEvent ev)
    {
        if(ev.Out.Count != 0){
            foreach (var action in ev.Out)
            {
                if(ev.deepness + 1 < eventsGlobal[action.to - 1].deepness) eventsGlobal[action.to - 1].deepness = ev.deepness + 1;

                CPMEvent nextEvent = eventsGlobal[action.to - 1];
                calculateDeepness(ref nextEvent);
            }
        }
    }
}
