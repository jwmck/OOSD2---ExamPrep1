using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;

namespace S00190761_JamesMcKeon_CA1
{
    /*  ***************
     *  Author: James McKeon
     *  Student Number: S00190761
     *  Submitted: 02 APR 2019
     *  
     *  Although the MainWindow Colour Scheme is not exactly the same as the example
     *  I aimed to show use of gradients as in the example. The main focus was on layout.
     *  ****************
     */

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region variables
        public ObservableCollection<Ward> Wards;

        public string selectedFile;

        //this bool is changed to true if any radio button is checked (see region: Code to Add Patient)
        public bool isChecked = false;

        public object JsonConvert { get; private set; }

        public static int wardCount;

        public string userChoice;
        #endregion variables

        #region Initialise Main Window
        public MainWindow()
        {
            InitializeComponent();
            Wards = new ObservableCollection<Ward>();

            //the following can not be set simply in the xaml page. Assume average age of patient is 40, this makes choosing the DOB easier in the datepicker.
            datepick.DisplayDate = DateTime.Today.AddYears(-40);
            //sets calander for over 18s only
            datepick.DisplayDateEnd = DateTime.Today.AddYears(-18);
        }
        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //create two wards
            Ward W1 = new Ward() {Name = "Marx Brothers Ward", Capacity = 3};
            Ward W2 = new Ward() {Name = "Addams Family Ward", Capacity = 7};

            //add wards to the observable collection
            Wards.Add(W1);
            //updates wardCount
            wardCount = Wards.Count();
            tblkWard.Text = $"Ward List ({wardCount})";

            //as above
            Wards.Add(W2);
            wardCount = Wards.Count();
            tblkWard.Text = $"Ward List ({wardCount})";

            //create four patients
            Patient P1 = new Patient("John Reilly", Convert.ToDateTime("1990/11/18"), BloodType.O);
            Patient P2 = new Patient("Bill Reilly", Convert.ToDateTime("1990/11/17"), BloodType.A);
            Patient P3 = new Patient("Pat Reilly", Convert.ToDateTime("1990/11/16"), BloodType.B);
            Patient P4 = new Patient("Rick Reilly", Convert.ToDateTime("1990/11/15"), BloodType.AB);

            //add patients to wards
            W1.Patients.Add(P1);
            W1.Patients.Add(P2);
            W2.Patients.Add(P3);
            W2.Patients.Add(P4);

            //display
            lbxWards.ItemsSource = Wards;

        }
        #endregion Initialise Main Window

        #region addWard / Patient Event Handlers
        private void lbxWards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //enables save button once a ward is chosen
            btnSave.IsEnabled = true;

            //determine what ward is selected
            Ward selectedWard = lbxWards.SelectedItem as Ward;

            if (selectedWard != null)
            {
                //displays patients in the ward
                lbxPatients.ItemsSource = selectedWard.Patients;
            }
        }

        private void lbxPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //determine what patient is selected
            Patient selectedPatient = lbxPatients.SelectedItem as Patient;

            if (selectedPatient != null)
            {
                //displays patient's name in text block
                patient_NameBlock.Text = selectedPatient.PName;

                //displays blood type image in image block
                patient_BloodPic.Source = new BitmapImage(new Uri($"{selectedPatient.BloodImage}", UriKind.Relative));
            }
        }
        #endregion addWard / Patient Event Handlers

        #region display AddWard Button
        //clears text in box
        private void tblk_name_GotFocus(object sender, RoutedEventArgs e)
        {
            tblk_name.Clear();
        }

        //if text changes, tests if name and capacity have been entered by user
        private void tblk_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            //check if patient name has not been entered or capacity is at 0
            if (slider_ward.Value == 0 || tblk_name.Text == "")
            {
                btn_addWard.IsEnabled = false;
            }

            //enables addWard button
            else
            {
                btn_addWard.IsEnabled = true;
            }
        }

        //if capacity changes, tests if name and capacity have been entered by user
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //change text beside slider to reflect value (note maximum value set to 10 in mainwindow.xaml)
            tblk_num.Text = string.Format("{0:F0}", slider_ward.Value);

            //check if patient name has not been entered or capacity is at 0
            if (tblk_name.Text == "" || slider_ward.Value == 0)
            {
                btn_addWard.IsEnabled = false;
            }

            //enables addWard button
            else
            {
                btn_addWard.IsEnabled = true;
            }
        }
        #endregion display AddWard Button

        #region display AddPatient button
        //clears text block
        private void tblk_patientName_GotFocus(object sender, RoutedEventArgs e)
        {
            tblk_patientName.Clear();
        }

        //if datepicker is changed, checks all other inputs have values and enables addpatient button
        private void datepick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //check if any other relevant inputs don't have values
            if (tblk_patientName.Text == "" || datepick.SelectedDate == null || isChecked == false)
            {
                btn_addPatient.IsEnabled = false;
            }

            //otherwise enable add patient button
            else btn_addPatient.IsEnabled = true;
        }

        //if patient name is changed, checks all other inputs have values and enables addpatient button
        private void tblk_patientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tblk_patientName.Text == "" || datepick.SelectedDate == null || isChecked == false)
            {
                btn_addPatient.IsEnabled = false;
            }

            //otherwise enable add patient button
            else btn_addPatient.IsEnabled = true;
        }

        //if radio button is checked, checks all other inputs have values and enables addpatient button
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            isChecked = true;

            if (tblk_patientName.Text == "" || datepick.SelectedDate == null || isChecked == false)
            {
                btn_addPatient.IsEnabled = false;
            }

            //otherwise enable add patient button
            else btn_addPatient.IsEnabled = true;
        }
        #endregion display AddPatient button

        #region Code to Add Ward
        private void btn_addWard_Click(object sender, RoutedEventArgs e)
        {


            //read info from screen
            string name = tblk_name.Text;

            //check if string is correct length for display
            if (name.Length < 10 || name.Length > 22)
            {
                MessageBox.Show("Please enter a Ward Name between 10 and 22 characters in length.");
            }

            else
            {
                //error check not required on the below as user can only input int via the slider
                int capacity = Convert.ToInt16(tblk_num.Text);

                //create object
                Ward newWard = new Ward(name, capacity);

                //add object to collection
                Wards.Add(newWard);
                //updates wardCount
                wardCount = Wards.Count();
                tblkWard.Text = $"Ward List ({wardCount})";

                //resets values
                tblk_name.Clear();
                slider_ward.Value = 0;
            }
        }
        #endregion Code to Add Ward

        #region Code to Add Patients
        private void btn_addPatient_Click(object sender, RoutedEventArgs e)
        {
            //determine the ward
            Ward selectedWard = lbxWards.SelectedItem as Ward;

            //read info from screen
            string name = tblk_patientName.Text;

            //check if string is correct length for display
            if (name.Length < 10 || name.Length > 22)
            {
                MessageBox.Show("Please enter a Patient Name between 10 and 22 characters in length.");
            }

            else
            {
                //no error possible as user can not input text (focusable = false) and input must be entered to focus addPatient button
                DateTime dob = Convert.ToDateTime(datepick.SelectedDate);

                //checks which blootype is selected
                BloodType blood;

                if (rbnO.IsChecked == true)
                {
                    blood = BloodType.O;
                }

                else if (rbnA.IsChecked == true)
                {
                    blood = BloodType.A;
                }

                else if (rbnB.IsChecked == true)
                {
                    blood = BloodType.B;
                }

                else
                {
                    blood = BloodType.AB;
                }

                //makes sure a ward is selected
                if (selectedWard == null)
                {
                    MessageBox.Show("You must select a Ward for this patient first");
                }

                //check ward has not exceeded capacity
                else if (selectedWard.Patients.Count >= selectedWard.Capacity)
                {
                    MessageBox.Show("New patient denied. Ward at capacity.");
                }

                else
                {
                    //create patient object
                    Patient patient = new Patient(name, dob, blood);

                    //add patient to collection
                    selectedWard.Patients.Add(patient);
                }
            }
            //clear values from inputs
            tblk_patientName.Clear();
            datepick.SelectedDate = null;
            rbnO.IsChecked = false;
            rbnA.IsChecked = false;
            rbnB.IsChecked = false;
            rbnAB.IsChecked = false;

        }
        #endregion  Code to Add Patients

        #region saveJSON
        //saves all information in JSON format
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //get string of objects, JSON formattted
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(Wards, Formatting.Indented);

            //determine if file already exists, see saveFile() method
            string saveLocation = saveFile();

            //check if file exists and write
            if (File.Exists($@"{saveLocation}") && saveLocation.EndsWith("json"))
            {
                using (StreamWriter sw = new StreamWriter($@"{saveLocation}"))
                {
                    sw.Write(json);
                    MessageBox.Show("File Updated.");
                }
            }
            else if (!File.Exists($@"{saveLocation}") && saveLocation.EndsWith(".json"))
            {
                using (StreamWriter sw = new StreamWriter($@"{saveLocation}"))
                {
                    sw.Write(json);
                    MessageBox.Show("New File Saved.");
                }
            }
            else if (saveLocation.Contains("cancelled"))
            {
                MessageBox.Show("User Cancelled");
            }
        }

        public string saveFile()
        {
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            SaveFileDialog1.Filter = "json files | *.json";
            SaveFileDialog1.ShowDialog();

            if (SaveFileDialog1.FileName.EndsWith(".json"))
            {
                return SaveFileDialog1.FileName;
            }
            else if (!SaveFileDialog1.FileName.EndsWith(".json") && SaveFileDialog1.FileName != "")
            {
                MessageBox.Show("Invalid filename, please use .json extension.");
                return saveFile();
            }
            else
            {
                return "cancelled";
            }
        }
        #endregion saveJSON

        #region loadJSON
        //loads JSON file

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            //user decides which file to load
            getFile();

            if (userChoice.EndsWith(".json"))
            {
                using (StreamReader sr = new StreamReader($@"{userChoice}"))
                {
                    //read text
                    string json = sr.ReadToEnd();

                    //clear wards (also removes unsaved data)
                    Wards.Clear();

                    //convert from json to objects
                    Wards = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Ward>>(json);

                    //refresh the display
                    lbxWards.ItemsSource = Wards;

                    //update wartCount
                    wardCount = Wards.Count();
                    tblkWard.Text = $"Ward List ({wardCount})";

                    //empty userChoice so it does not affect next addition
                    userChoice = "";
                }
            }
            else if (userChoice.Contains("cancelled"))
            {
                MessageBox.Show("User Cancelled");
                //empty userChoice so it does not affect next addition
                userChoice = "";
            }

        }

        public void getFile()
        {
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();

            //warn user about losing unsaved data
            MessageBox.Show("All unsaved data will be lost. Press 'Cancel' at next screen if you are unsure.");

            OpenFileDialog1.ShowDialog();

            //checks for json file
            if (OpenFileDialog1.FileName.EndsWith(".json"))
            {
                userChoice = OpenFileDialog1.FileName;
            }
            //if user chooses bad file
            else if (!OpenFileDialog1.FileName.EndsWith(".json") && OpenFileDialog1.FileName != "")
            {
                MessageBox.Show("Invalid file selected, please select .json file or cancel operation.");
                getFile();
            }
            else //if user cancels
            {
                userChoice = "cancelled";
            }

        }
        #endregion loadJSON

    }
}
