using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    public class Animals
    {
        private string _sAnimalName;
        public string sAnmialName
        {
            get
            {
                return _sAnimalName;
            }
            set
            {
                _sAnimalName = value;
            }
        }

        private string _sAnimalType;
        public string sAnimalType
        {
            get
            {
                return _sAnimalType;
            }
            set
            {
                _sAnimalType = value;
            }
        }

    }
}
