using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate bool TestFunction();
public delegate void ThenFunction();

public class ScratchyPromise
{
    protected ThenFunction thenFunction;
    protected TestFunction whileTest;
    protected TestFunction whenTest;
    public bool Active = true;

    public ScratchyPromise()
    {
    }

    public ScratchyPromise While(TestFunction test)
    {
        this.whileTest = test;
        return this;
    }

    public ScratchyPromise When(TestFunction test)
    {
        this.whileTest = test;
        return this;
    }

    public void Then(ThenFunction then)
    {
        this.thenFunction = then;
    }
}
