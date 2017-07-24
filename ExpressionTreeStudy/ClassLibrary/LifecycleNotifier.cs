
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
            //page.OnLoadingInner(parameter);
            var interfaceType = GetImplementedInterface(page, typeof(IPageLoadingAware<>));
            var methodInfo = typeof(LifecycleNotifier).GetRuntimeMethods().Single(x => x.Name == "Notify");
            var genericMethodInfo = methodInfo.MakeGenericMethod(interfaceType);


            // インスタンスへのnewを行う部分まで生成する、つまり以下の様なラムダを作る
            // (x, y) => new HogeController().HugaAction(x, y)
            var obj = Expression.Parameter(interfaceType, "obj");
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
