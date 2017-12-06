
using System;
using System.Reflection;

[module: DecoratorDemo]

public class DecoratorDemoAttribute : MethodDecoratorAttribute
{
    public override void Init(object instance, MethodBase method, object[] args)
    {
        throw new NotImplementedException();
    }

    public override void OnEntry()
    {
        throw new NotImplementedException();
    }

    public override void OnException(Exception exception)
    {
        throw new NotImplementedException();
    }

    public override void OnExit()
    {
        throw new NotImplementedException();
    }
}

/*
 class Test
 {
    [DecoratorDemo]
    public void GetData(){}
    
 }
     */
