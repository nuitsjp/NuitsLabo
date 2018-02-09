using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Microsoft.AppCenter.Analytics;
using Reactive.Bindings;

namespace UwpCalculator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public IList<int> IntValues { get; } = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

        public ReactiveProperty<int?> Left { get; } = new ReactiveProperty<int?>();
        public ReactiveProperty<int?> Right { get; } = new ReactiveProperty<int?>();
        public ReactiveCommand AddCommand { get; }
        public ReactiveProperty<int?> Result { get; } = new ReactiveProperty<int?>();

        public MainWindowViewModel()
        {
            AddCommand =
                Left.CombineLatest(Right, (left, right) => left != null && right != null)
                .ToReactiveCommand();
            AddCommand.Subscribe(_ => Result.Value = Left.Value.Value + Right.Value.Value);

            SetProperty(() => Left);
            SetProperty(() => Right);
            SetCommand(() => AddCommand);
        }
    }
}
