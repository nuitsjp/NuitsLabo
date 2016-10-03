using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Reactive.Bindings.Extensions;

namespace PropertyBinderStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new Model();
            var viewModel = new ViewModel(model);
            model.Message = "Hello, Binding.";
            Console.WriteLine("Please press any key.");
            Console.ReadKey();
        }
        public class ViewModel : BindableBase
        {
            private IModel _model;
            private string _message;
            public string Message
            {
                get { return _message; }
                set
                {
                    SetProperty(ref _message, value);
                    Console.WriteLine(Message);
                }
            }
            public ViewModel(IModel model)
            {
                _model = model;
                //var observableModel = _model.PropertyChangedAsObservable();
                //observableModel.Where(e => e.PropertyName == "Message").Subscribe(x => Message = model.Message);
                var binder = new PropertyBinder<IModel>(_model);
                binder.Subscribe(() => binder.Source.Message, _ => Message = binder.Source.Message);
            }
        }

        public class PropertyBinder<T> : IObservable<ProgressChangedEventArgs> where T : INotifyPropertyChanged
        {
            private readonly IObservable<PropertyChangedEventArgs> _observable;
            public T Source { get; }
            public PropertyBinder(T source)
            {
                Source = source;
                _observable = Source.PropertyChangedAsObservable();
            }

            public void Subscribe<TProperty>(Expression<Func<TProperty>> propertyExpression, Action<PropertyChangedEventArgs> onNext)
            {
                var name = PropertySupport.ExtractPropertyName(propertyExpression);
                _observable.Where(e => e.PropertyName == name).Subscribe(onNext);
            }

            public IDisposable Subscribe(IObserver<ProgressChangedEventArgs> observer)
            {
                return _observable.Subscribe(observer);
            }
        }
        public interface IModel : INotifyPropertyChanged
        {
            string Message { get; set; }
        }

        public class Model : BindableBase, IModel
        {
            private string _message;

            public string Message
            {
                get { return _message; }
                set { SetProperty(ref _message, value); }
            }
        }
    }
}
