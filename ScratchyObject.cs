using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScratchyObject : MonoBehaviour
{
    private int TimerLimit = 20;
    private ScrathyTimer[] Timers;

    public ScratchyObject()
    {
        Timers = new ScrathyTimer[TimerLimit];
    }

    // Use this for initialization
    public virtual void Start()
    {
        this.OnStart();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        this.OnUpdate();
    }

    // FixedUpdate is called regularly
    public void FixedUpdate()
    {
        // Update any timers
        for (int i = 0; i < Timers.Length; i++ )
        {
            ScrathyTimer t = Timers[i];
            if (t != null && t.Active)
            {
                t.Update(Time.fixedDeltaTime);
            }
        }

        this.OnFixedUpdate();
    }

    // Never put anything in these virutal functions. They are for the derived class only
    public virtual void OnStart()
    {
    }
    public virtual void OnUpdate()
    {
    }
    public virtual void OnFixedUpdate()
    {
    }

    public ScrathyTimer Wait(float delay, TimerCallback callback)
    {
        ScrathyTimer timer = GetTimerFromPool();
        timer.Init(delay, false, callback);
        return timer;
    }

    public ScrathyTimer Forever(float every, TimerCallback callback)
    {
        ScrathyTimer timer = GetTimerFromPool();
        timer.Init(every, true, callback);
        return timer;
    }

    public ScrathyTimer Repeat(int count, float every, TimerCallback callback)
    {
        ScrathyTimer timer = GetTimerFromPool();
        timer.Init(every, true, callback);
        timer.CountLimit = count;
        return timer;
    }

    private ScrathyTimer GetTimerFromPool()
    {
        ScrathyTimer timer = null;
        int timerIndex = -1;
        for (int i = 0; i < Timers.Length; i++ )
        {
            timer = Timers[i];
            if (timer == null || !timer.Active)
            {
                timerIndex = i;
                break;
            }
        }
        if (timerIndex == -1)
        {
            throw new System.Exception("Object exceeded TimerLimit");
        }
        if (timer == null)
        {
            timer = new ScrathyTimer();
            this.Timers[timerIndex] = timer;
        }
        return timer;
    }

    /// <summary>
    /// Exit the game
    /// </summary>
    public void StopAll()
    {
        Debug.Log("stop all");
        Application.Quit();
    }

    public void DrawRectangle(Bounds bt, Color c)
    {
        Debug.DrawLine(bt.min, new Vector3(bt.min.x, bt.max.y, bt.min.z), c);
        Debug.DrawLine(bt.max, new Vector3(bt.min.x, bt.max.y, bt.min.z), c);
        Debug.DrawLine(bt.min, new Vector3(bt.max.x, bt.min.y, bt.min.z), c);
        Debug.DrawLine(bt.max, new Vector3(bt.max.x, bt.min.y, bt.min.z), c);
    }

    public void DrawDot(Vector3 pos, float size, Color c)
    {
        Debug.DrawLine(new Vector3(pos.x - size, pos.y, pos.z), new Vector3(pos.x + size, pos.y, pos.z), c);
        Debug.DrawLine(new Vector3(pos.x, pos.y - size, pos.z), new Vector3(pos.x, pos.y + size, pos.z), c);
    }

    public List<ScratchySprite> GetSprites<T>() where T : ScratchySprite
    {
        Type t = typeof(T);
        if (ScratchySprite.Instances.ContainsKey(t))
        {
            return ScratchySprite.Instances[t];
        }
        return new List<ScratchySprite>();
    }

    public void PointTowardsMouse(float angleAdjust = 0)
    {
        Vector3 objectInScreenSpace = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= objectInScreenSpace.x;
        mousePos.y -= objectInScreenSpace.y;
        mousePos.z = objectInScreenSpace.z;
        var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + angleAdjust);
    }
}
