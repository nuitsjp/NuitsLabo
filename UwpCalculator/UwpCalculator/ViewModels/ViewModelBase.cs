using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Reactive.Bindings;

namespace UwpCalculator.ViewModels
{
    public abstract class ViewModelBase
    {
        protected void SetProperty<T>(Expression<Func<IReactiveProperty<T>>> propertyExpression)
        {
            var getProperty = propertyExpression.Compile();
            var property = getProperty();
            var body = propertyExpression.Body as MemberExpression;
            property.Skip(1).Subscribe(
                x => Analytics.TrackEvent(
                    $"{body.Member.DeclaringType.Name}#{body.Member.Name}",
                    new Dictionary<string, string>{{"value", x?.ToString()}}));
        }

        protected void SetCommand(Expression<Func<ReactiveCommand>> commandExpression)
        {
            var getProperty = commandExpression.Compile();
            var property = getProperty();
            var body = commandExpression.Body as MemberExpression;
            property.Subscribe(
                x => Analytics.TrackEvent(
                    $"{body.Member.DeclaringType.Name}#{body.Member.Name}",
                    new Dictionary<string, string> { { "value", x?.ToString() } }));
        }

    }
}
