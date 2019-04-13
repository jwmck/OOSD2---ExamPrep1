using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace S00190761_JamesMcKeon_CA1
{
    public class Ward
    {
        public string Name { get; set; }

        public int Capacity { get; set; }

        public ObservableCollection<Patient> Patients { get; set; }

        #region constructors

        public Ward()
        {
            Patients = new ObservableCollection<Patient>();
        }

        public Ward(string name, int capacity):this()
        {
            Name = name;
            Capacity = capacity;
        }


        //public Ward(string name, int capacity):this()
        //{
        //    Name = name;
        //    Capacity = capacity;
        //}

        //public Ward(string name):this()
        //{
        //    Name = name;
        //    Capacity = 8;
        //}
        #endregion constructors

        #region methods

        public override string ToString()
        {
            //return string.Format("{0,-10} {1, 20} Patients", Name, Capacity);
            return string.Format("{0}\t\t {1} Patients Max", Name, Capacity);
        }

        #endregion methods

    }
}
