# GenericListView

Very simple general-purpose list view. 

## How to use
Take three files:
* GenericListView.Custom.cs
* GenericListView.xaml
* GenericListView.xaml.cs

to Visual Studio WPF project.

On a window or user control xaml, place the control by :
* Setting namespace (e.g. xmlns:i="clr-namespace:Example" )
* Setting Properties (e.g <i:GenericListView CollectionSource="{Binding Path=listAnimals}" RowHeight="50"></i:GenericListView>)
    This requires some object to pass through as data source for CollectionSource.

For more details on the properties, look below.

## Properties

It has two dependency properties exposed: CollectionSource and RowHeight.

### CollectionSource
It contains data to be displayed on the list view.

GenercListView is fed with this object in the binding.
Then, ConvertToListCollection method processes this object to display contents of the object.
It is up to the user to convert the object and to decide what to display.
The example already contains an example conversion to display Animals class data.

### RowHeight 
It is the height of each list view row.

