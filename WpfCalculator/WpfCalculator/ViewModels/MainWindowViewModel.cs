using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace WpfCalculator.ViewModels
{
    public class MainWindowViewModel
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
        }
    }
}
