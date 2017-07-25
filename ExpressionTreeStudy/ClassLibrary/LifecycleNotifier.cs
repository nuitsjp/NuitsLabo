
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ClassLibrary
{
    public static class LifecycleNotifier
    {

        public static void OnLoading(this Page page, object parameter)
        {
            var constant = Expression.Constant(parameter);

            
            //page.OnLoadingInner(parameter);
            var interfaceType = GetImplementedInterface(page, typeof(IPageLoadingAware<>));
            var methodInfo = interfaceType.GetRuntimeMethods().FirstOrDefault(x => x.Name == "OnLoading");
            //var genericMethodInfo = methodInfo.MakeGenericMethod(interfaceType);
            Action<Page> lambda = (p) => methodInfo.Invoke(p, new []{parameter});


            // インスタンスへのnewを行う部分まで生成する、つまり以下の様なラムダを作る
            // (x, y) => new HogeController().HugaAction(x, y)
            var genericType = interfaceType.GenericTypeArguments[0];
            var pageParam = Expression.Parameter(genericType, "parameter");
            var target = Expression.Parameter(interfaceType);
            var call = Expression.Call(target, methodInfo, pageParam);
            var expressionAction = Expression.Lambda(call, pageParam);
            var action = expressionAction.Compile();
            action.DynamicInvoke(parameter);
            //var lambda = Expression.Lambda<Func<int, int, string>>(
            //        Expression.Call( // .HugaAction(x, y)
            //            Expression.New(type), // new HogeController()
            //            method,
            //            x, y),
            //        x, y) // (x, y) => 
            //    .Compile();
            //var onLoadMethodInfo = interfaceType.GetRuntimeMethods().Single(x => x.Name == "OnLoading");
            //methodInfo.Invoke(page, new[] {page, (p, param) => OnLoadingInner(p, param) });
        }
        

        public static void Invoke(Page page, object parameter)
        {
            var interfaceType = GetImplementedInterface(page, typeof(IPageLoadingAware<>));
            var methodInfo = interfaceType.GetRuntimeMethods().FirstOrDefault(x => x.Name == "OnLoading");
            //var genericMethodInfo = methodInfo.MakeGenericMethod(interfaceType);
            methodInfo.Invoke(page, new []{parameter});
        }

        public static void OnLoadingInner(this Page page, object parameter)
        {
            var type = typeof(IPageLoadingAware<>);
            foreach (var implementedInterface in page.GetType().GetTypeInfo().ImplementedInterfaces)
            {
                if (type.Name == implementedInterface.Name)
                {
                    var methodInfo = implementedInterface.GetRuntimeMethods().FirstOrDefault(x => x.Name == "OnLoading");
                    methodInfo.Invoke(page, new []{parameter});
                }
            }
        }

        private static Type GetImplementedInterface(object target, Type interfacType)
        {
            foreach (var implementedInterface in target.GetType().GetTypeInfo().ImplementedInterfaces)
            {
                if (interfacType.Name == implementedInterface.Name)
                    return implementedInterface;
            }
            return null;
        }

        private static void Notify<T>(this Page page, Action<T> action) where T : class
        {
        }
    }
}
