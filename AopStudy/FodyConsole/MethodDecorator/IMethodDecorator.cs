using System;
using System.Reflection;

public interface IMethodDecorator
{
    void Init(object instance, MethodBase method, object[] args);
    void OnEntry();
    void OnExit();
    void OnException(Exception exception);
}

// Any attribute which provides OnEntry/OnExit/OnException with proper args
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Assembly | AttributeTargets.Module)]
public abstract class MethodDecoratorAttribute : Attribute, IMethodDecorator
{
    public abstract void Init(object instance, MethodBase method, object[] args);
    public abstract void OnEntry();
    public abstract void OnException(Exception exception);
    public abstract void OnExit();
}