using UnityEngine;
using System.Collections;

public delegate void TimerCallback();

public class ScrathyTimer : ScratchyPromise
{
    public bool Repeat = false;
    public float Elapsed = 0;
    public float Duration = 0;
    public int Count = 0;
    public int CountLimit = -1;

    private TimerCallback callback;

    public ScrathyTimer()
    {
        this.Active = false;
    }

    public ScrathyTimer(float duration, bool repeat, TimerCallback callback)
    {
        this.Init(duration, repeat, callback);
    }

    public void Init(float duration, bool repeat, TimerCallback callback)
    {
        this.Active = true;
        this.Duration = duration;
        this.Repeat = repeat;
        this.callback = callback;
        this.Elapsed = 0;
    }

    public void Update(float deltaTime)
    {
        if (Active == true && Duration > 0)
        {
            Elapsed += deltaTime;
            if (Elapsed >= Duration)
            {
                bool skip = false;
                if (this.whileTest != null)
                {
                    this.Active = this.whileTest();
                }
                if (this.whenTest != null)
                {
                    if (this.whenTest() == false)
                    {
                        skip = true;
                    }
                }

                if (this.Active && !skip)
                {
                    callback();
                                        
                    if (this.CountLimit > -1)
                    {
                        this.Count++;
                        if (this.Count >= CountLimit)
                        {
                            this.Repeat = false;
                            this.Active = false;
                        }
                    }
                }

                if (Repeat)
                {
                    while (this.Elapsed > this.Duration)
                    {
                        this.Elapsed -= this.Duration;
                    }
                }
                else
                {
                    this.Active = false;
                    if (this.thenFunction != null)
                    {
                        this.thenFunction();
                    }
                }
            }
        }
    }
}
