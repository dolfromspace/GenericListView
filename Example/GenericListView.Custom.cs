using System;
using System.Collections.Generic;
using System.Linq;

namespace Example
{
    partial class GenericListView
    {
        static public Tuple<List<GridColumnProp>, List<MyListviewItem>> ConvertToListCollection(Object items)
        {
            List<Animals> listAnimals = items as List<Animals>;
            if (listAnimals != null)
                return ConvertAnimals(listAnimals);

            return null;
        }

        private static Tuple<List<GridColumnProp>, List<MyListviewItem>> ConvertAnimals(List<Animals> listAnimals)
        {
            List<GenericListView.GridColumnProp> columns = new List<GenericListView.GridColumnProp>();
            columns.Add(new GenericListView.GridColumnProp { sColumnName = "Animal Name" });
            columns.Add(new GenericListView.GridColumnProp { sColumnName = "Animal Type" });

            List<GenericListView.MyListviewItem> items = listAnimals.Select<Animals, GenericListView.MyListviewItem>(item =>
                {
                    GenericListView.MyListviewItem listviewitem = new GenericListView.MyListviewItem();
                    listviewitem.items.Add(item.sAnmialName);
                    listviewitem.items.Add(item.sAnimalType);
                    return listviewitem;
                }
            ).ToList();

            return new Tuple<List<GridColumnProp>, List<MyListviewItem>>(columns, items);
        }
    }
}
