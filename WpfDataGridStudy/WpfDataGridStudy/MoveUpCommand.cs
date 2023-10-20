using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace WpfDataGridStudy;

public class MoveUpCommand<T> : ICommand
{
    private readonly ObservableCollection<T> _items;
    private readonly ObservableCollection<T> _selectedItems;

    public MoveUpCommand(ObservableCollection<T> items, ObservableCollection<T> selectedItems)
    {
        _items = items;
        _selectedItems = selectedItems;

        _selectedItems.CollectionChanged += SelectedItems_CollectionChanged;
    }

    private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CanExecute(object parameter)
    {
        return _selectedItems.Count > 0 && _items.IndexOf(_selectedItems[0]) > 0;
    }

    public event EventHandler? CanExecuteChanged;

    public void Execute(object parameter)
    {
        foreach (var selectedItem in _selectedItems.ToArray())
        {
            var currentIndex = _items.IndexOf(selectedItem);
            _items.Move(currentIndex, currentIndex - 1);
        }
    }
}