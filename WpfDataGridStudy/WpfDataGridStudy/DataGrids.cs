using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfDataGridStudy;

/// <summary>
/// DataGridに対する添付プロパティクラス。
/// </summary>
public static class DataGrids
{
    /// <summary>
    /// SelectedItemsを
    /// </summary>
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(DataGrids),
            new PropertyMetadata(null, OnSelectedItemsChanged));

    public static void SetSelectedItems(DependencyObject element, IList value)
    {
        element.SetValue(SelectedItemsProperty, value);
    }

    public static IList GetSelectedItems(DependencyObject element)
    {
        return (IList)element.GetValue(SelectedItemsProperty);
    }

    private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGrid dataGrid)
        {
            dataGrid.SelectionChanged += (s, args) =>
            {
                var items = dataGrid.Items;

                var selectedItemsList = GetSelectedItems(dataGrid);
                selectedItemsList?.Clear();

                var selectedItems = dataGrid.SelectedItems
                    .Cast<object>()
                    .Select(x => (Index: items.IndexOf(x), Item: x))
                    .OrderBy(x => x.Index)
                    .Select(x => x.Item);

                foreach (var item in selectedItems)
                {
                    selectedItemsList?.Add(item);
                }
            };
        }
    }
}