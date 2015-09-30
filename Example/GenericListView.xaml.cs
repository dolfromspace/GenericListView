using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using System.Collections;

namespace Example
{
    /// <summary>
    /// Interaction logic for GenericListView.xaml
    /// </summary>
    public partial class GenericListView : UserControl
    {
        private int _iRowHeight;
        public int iRowHeight
        {
            get { return _iRowHeight; }
            set { _iRowHeight = value; }
        }

        public class ColumnWidthConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                double columnPercent = System.Convert.ToDouble(parameter);
                double width = (double)value;
                return width * columnPercent;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public class ListSorter
        {
            GridViewColumnHeader _lastHeaderClicked = null;
            ListSortDirection _lastDirection = ListSortDirection.Ascending;
            Action<string, ListSortDirection, object> sortCallBack;

            public ListSorter(Action<string, ListSortDirection, object> callback)
            {
                sortCallBack = callback;
            }

            private void Sort(string sortBy, ListSortDirection direction, object view)
            {
                sortCallBack(sortBy, direction, view);
            }

            public void ReceiveSortCommand(RoutedEventArgs e, object view)
            {
                GridViewColumnHeader headerClicked =
                      e.OriginalSource as GridViewColumnHeader;
                ListSortDirection direction;

                if (headerClicked != null)
                {
                    if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                    {
                        if (headerClicked != _lastHeaderClicked)
                        {
                            direction = ListSortDirection.Ascending;
                        }
                        else
                        {
                            if (_lastDirection == ListSortDirection.Ascending)
                            {
                                direction = ListSortDirection.Descending;
                            }
                            else
                            {
                                direction = ListSortDirection.Ascending;
                            }
                        }

                        string header = headerClicked.Column.Header as string;
                        Sort(header, direction, view);
                        
                        // Remove arrow from previously sorted header 
                        if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                        {
                            _lastHeaderClicked.Column.HeaderTemplate = null;
                        }

                        _lastHeaderClicked = headerClicked;
                        _lastDirection = direction;
                    }
                }
            }
        }

        public static DependencyProperty CollectionSourceProperty = DependencyProperty.Register("CollectionSource", typeof(Object), typeof(GenericListView), new UIPropertyMetadata(CollectionChangedHandler));

        public static DependencyProperty RowHeightProperty = DependencyProperty.Register("RowHeight", typeof(long), typeof(GenericListView), new UIPropertyMetadata(RowHeightChangedHandler));

        public static void RowHeightChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            GenericListView control = sender as GenericListView;
            var listview = control.listv;
            var s = new Style(typeof(ListViewItem));

            var HeightSetter = new Setter { Property = HeightProperty, Value = Convert.ToDouble( e.NewValue ) };

            s.Setters.Add(HeightSetter);

            listview.ItemContainerStyle = s;  
        }

        public static void CollectionChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Get instance of current control from sender
            // and property value from e.NewValue
            Tuple<List<GridColumnProp>, List<MyListviewItem>> listData = ConvertToListCollection(e.NewValue);

            if (listData == null)
                return;

            // Set public property on TaregtCatalogControl, e.g.
            ((GenericListView)sender).columns = listData.Item1;
            ((GenericListView)sender).aItems = listData.Item2;
        }
        
        public Object CollectionSource
        {
            get { return GetValue(CollectionSourceProperty) as IEnumerable; }
            set { SetValue(CollectionSourceProperty, value); }
        }

        public long RowHeight
        {
            get { return (long)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        public class MyListviewItem
        {
            private List<string> _items = new List<string>();
            public List<string> items
            {
                get
                {
                    return _items;
                }
                set
                {
                    _items = value;
                }
            }

            public MyListviewItem()
            {
            }
        }

        public class GridColumnProp
        {
            private string _sColumnName;
            public string sColumnName
            {
                get
                {
                    return _sColumnName;
                }
                set
                {
                    _sColumnName = value;
                }
            }

            public GridColumnProp()
            {
            }
        }

        private List<MyListviewItem> _aItems;
        public List<MyListviewItem> aItems
        {
            get
            {
                return _aItems;
            }
            set
            {
                _aItems = value;
                listv.ItemsSource = _aItems;
            }
        }

        private List<GridColumnProp> _columns;
        public List<GridColumnProp> columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
                GridView viewLayout = new GridView();
                double dbRatio = (double)(1.0 / _columns.Count);
                for (int i = 0; i < _columns.Count; i++)
                {
                    var newTextBlock = new FrameworkElementFactory(typeof(TextBlock));
                    newTextBlock.SetBinding(TextBlock.TextProperty, new Binding("items[" + i + "]"));
                    newTextBlock.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    newTextBlock.SetValue(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis);
                    DataTemplate newDataTemplate = new DataTemplate() { VisualTree = newTextBlock };
                    GridViewColumn gridviewcolumn = new GridViewColumn();
                    gridviewcolumn.Header = _columns[i].sColumnName;

                    Binding myWidthBinding = new Binding();
                    myWidthBinding.ElementName = "listv";
                    myWidthBinding.Path = new PropertyPath("ActualWidth");
                    myWidthBinding.Converter = new ColumnWidthConverter();
                    myWidthBinding.ConverterParameter = dbRatio;
                    BindingOperations.SetBinding(gridviewcolumn, GridViewColumn.WidthProperty, myWidthBinding);
                    gridviewcolumn.CellTemplate = newDataTemplate;

                    viewLayout.Columns.Add(gridviewcolumn);
                }

                listv.View = viewLayout;
            }
        }

        public ListSorter listSort = new ListSorter(Sort);

        public GenericListView()
        {
            InitializeComponent();
            listv.ItemsSource = aItems;
            listv.DataContext = this;
            // DataContext = this;
        }

        // Sorting function 
        public static void Sort(string sortBy, ListSortDirection direction, object view)
        {
            Func<MyListviewItem, string> selector;

            GenericListView genericListview = view as GenericListView;

            if (genericListview != null)
            {
                for (int i = 0; i < genericListview.columns.Count; i++)
                {
                    if (sortBy.Contains(genericListview.columns[i].sColumnName) == true)
                    {
                        selector = new Func<MyListviewItem, string>(o => o.items[i]);
                        if (direction == ListSortDirection.Ascending)
                            genericListview.aItems = new List<MyListviewItem>(genericListview.aItems.OrderBy(selector));
                        else
                            genericListview.aItems = new List<MyListviewItem>(genericListview.aItems.OrderByDescending(selector));
                        break;
                    }
                }
            }
        }

        // Event handler for clicking a column header.
        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            listSort.ReceiveSortCommand(e, this);
        }

    }
}
