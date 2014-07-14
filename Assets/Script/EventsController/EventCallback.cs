using UnityEngine;
using System;
using System.Collections.Generic;

public class EventCallback : MonoBehaviour 
{
    private Queue<QueueInfo> _eventQueue = new Queue<QueueInfo>();

    private Dictionary<string, EventTiming> _registeredEventTiming
        = new Dictionary<string, EventTiming>();

    private List<string> _registeredDisposables 
        = new List<string>();

    public void registerEvent(string requestingObj,
        float delayDuration, bool isLoop, 
        Action<UnityEngine.Object> callbackAction)
    {
        if(!_registeredEventTiming.ContainsKey(requestingObj))
        {
            EventTiming et = new EventTiming(Time.time,delayDuration,isLoop,callbackAction);
            QueueInfo qi = new QueueInfo(requestingObj, et);
            _eventQueue.Enqueue(qi);
            //_registeredEventTiming.Add(requestingObj, et);
        }
        else
        {            
            EventTiming et = new EventTiming(Time.time, delayDuration, isLoop, callbackAction);

            QueueInfo qi = new QueueInfo(requestingObj, et);
            _eventQueue.Enqueue(qi);
        }
    }

    public void cancelEventCallback(string obj)
    {
        disposeEvent(obj);
    }

    public void disposeEvent(string obj)
    {
        if(!_registeredDisposables.Contains(obj))
        {
            _registeredDisposables.Add(obj);
        }
    }

    private void checkDisposable()
    {
        //Dispose the object from the list
        foreach (string obj in _registeredDisposables)
        {
            if (_registeredEventTiming.ContainsKey(obj))
            {
                _registeredEventTiming.Remove(obj);
            }                
        }

        //Clear the disposal list
        _registeredDisposables.Clear();
    }

    private void Update()
    {
        //Run the check for disposable event before it gets process.
        checkDisposable();

        foreach(KeyValuePair<string,EventTiming> kvp in _registeredEventTiming)
        {
            //Fire off the event when the time is up
            if((Time.time - kvp.Value.startTime) > kvp.Value.duration)
            {
                if (kvp.Value.callbackAction != null)
                    kvp.Value.callbackAction(null);

                //Register this entry for disposal
                if (!kvp.Value.isLoop)
                {
                    if (!_registeredDisposables.Contains(kvp.Key))
                        _registeredDisposables.Add(kvp.Key);
                }
                else
                {
                    kvp.Value.startTime = Time.time;
                }              
            }
        }

        checkDisposable();

        if(_eventQueue.Count > 0)
        {
            QueueInfo qi = _eventQueue.Peek();
            if(!_registeredEventTiming.ContainsKey(qi.requestingObj))
            {
                qi = _eventQueue.Dequeue();
                _registeredEventTiming.Add(qi.requestingObj, qi.eventTiming);
            }            
        }        
    }
}

public class EventTiming
{
    public float startTime;
    public float duration;
    public bool isLoop;
    public Action<UnityEngine.Object> callbackAction;

    public EventTiming(float p_startTime,float p_duration,
        bool p_isLoop, Action<UnityEngine.Object> p_callbackAction)
    {
        startTime = p_startTime;
        duration = p_duration;
        isLoop = p_isLoop;
        callbackAction = p_callbackAction;
    }
}

public class QueueInfo
{
    public EventTiming eventTiming;
    public string requestingObj;

    public QueueInfo(string obj,EventTiming et)
    {
        eventTiming = et;
        requestingObj = obj;
    }
}
