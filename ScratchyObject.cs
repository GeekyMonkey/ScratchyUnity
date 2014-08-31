using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScratchyObject : MonoBehaviour
{
    private int TimerLimit = 20;
    private ScratchyTimer[] Timers;

    public ScratchyObject()
    {
        Timers = new ScratchyTimer[TimerLimit];
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
            ScratchyTimer t = Timers[i];
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

    public ScratchyTimer Wait(float delay, TimerCallback callback)
    {
        ScratchyTimer timer = GetTimerFromPool();
        timer.Init(delay, false, callback);
        return timer;
    }

    public ScratchyTimer Forever(float every, TimerCallback callback)
    {
        ScratchyTimer timer = GetTimerFromPool();
        timer.Init(every, true, callback);
        return timer;
    }

    public ScratchyTimer Repeat(int count, float every, TimerCallback callback)
    {
        ScratchyTimer timer = GetTimerFromPool();
        timer.Init(every, true, callback);
        timer.CountLimit = count;
        return timer;
    }

    private ScratchyTimer GetTimerFromPool()
    {
        ScratchyTimer timer = null;
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
            timer = new ScratchyTimer();
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

    /// <summary>
    /// Clone another object at the location of this ScratchySprite
    /// </summary>
    /// <param name="prefab">The prefab to instantiate</param>
    /// <returns>The new cloned object</returns>
    public ScratchySprite Clone(GameObject prefab)
    {
        return Clone(prefab, this.transform.position, this.transform.rotation);
    }

    public static ScratchySprite Clone(GameObject prefab, Vector3 position)
    {
        return Clone(prefab, position, Quaternion.identity);
    }

    public static ScratchySprite Clone(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var gameObject = (GameObject)Instantiate(prefab, position, Quaternion.identity);
        ScratchySprite sprite;
        try
        {
            sprite = gameObject.GetComponent<ScratchySprite>();
        }
        catch
        {
            sprite = null;
        }

        if (sprite == null)
        {
            throw new Exception("Clone only works for ScratchySprite objects");
        }

        return sprite;
    }

}
