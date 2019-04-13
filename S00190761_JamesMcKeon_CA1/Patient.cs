using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S00190761_JamesMcKeon_CA1
{
    public enum BloodType { A, B, AB, O }

    public class Patient
    {
        #region properties

        public string PName { get; set; }

        public DateTime DOB { get; set; }

        public BloodType Blood { get; set; }

        public string BloodImage
        {
            get
            {
                return string.Format("/images/{0}.png", Blood);
            }
        }
        #endregion properties

        #region constructors

        public Patient(string pName, DateTime dob, BloodType blood)
        {
            PName = pName;
            DOB = dob;
            Blood = blood;
        }

        public Patient()
        {

        }

        #endregion constructors

        #region methods
        public override string ToString()
        {
            return string.Format("{0}, \t({1} Years), \tType: {2}", PName, CalculateAge(DOB), Blood);
        }

        public int CalculateAge(DateTime DOB)
        {
            DateTime Now = DateTime.Now;

            int years = (Now.Year - DOB.Year);

            if (Now.Month == DOB.Month && Now.Day < DOB.Day || (Now.Month < DOB.Month))
                years--;

            return years;
        }
        #endregion methods



    }


}
