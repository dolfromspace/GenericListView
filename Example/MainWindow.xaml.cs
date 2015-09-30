using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Animals> _listAnimals = new List<Animals>();
        public List<Animals> listAnimals
        {
            get { return _listAnimals; }
            set
            {
                _listAnimals = value;
            }
        }

        public MainWindow()
        {
            listAnimals.Add(new Animals { sAnmialName = "Dog", sAnimalType = "4 legs" });
            listAnimals.Add(new Animals { sAnmialName = "Cat", sAnimalType = "4 legs" });
            listAnimals.Add(new Animals { sAnmialName = "Snake", sAnimalType = "0 legs" });
            listAnimals.Add(new Animals { sAnmialName = "Bird", sAnimalType = "2 legs" });

            InitializeComponent();
            DataContext = this;
        }
    }
}
