using CRUDDemoWPFApp.Models;
using CRUDDemoWPFApp.Services;
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

namespace CRUDDemoWPFApp.UserInterface
{
    /// <summary>
    /// Interaction logic for CoursesPage.xaml
    /// </summary>
    public partial class CoursesPage : UserControl
    {

        private Courses selectedCourse = new Courses();
        private ObservableCollection<Courses> courses = new ObservableCollection<Courses>();
        private string filter = "All Courses";

        public CoursesPage()
        {
            InitializeComponent();
            //filter = cmbFilter.Text;
            filter = (cmbFilter.SelectedValue.ToString().Contains("All Courses")) ? "All Courses" : "Available Courses";
            GetCourses(filter);
        }

        private void GetCourses(string filter)
        {
            courses = DBServicesGeneral.dBServicesCourses.Select(filter);
            CoursesDataGrid.ItemsSource = courses;
        }

        private void CoursesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            if (CoursesDataGrid.SelectedIndex >= 0)
            {
                selectedCourse = (Courses)CoursesDataGrid.SelectedItem;
                txtID.Text = selectedCourse.CourseID;
                txtCourseName.Text = selectedCourse.CourseName;
                txtStartDate.Text = selectedCourse.StartDate.ToShortDateString();
                txtEndDate.Text = selectedCourse.EndDate.ToShortDateString();
                txtPlaces.Text = selectedCourse.Places.ToString();
            }      
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = new MainWindow(); 
            mainPage.Show(); 
        }

        private bool CheckDataIsFilled()
        {

            if (!txtID.Text.Equals("") && !txtCourseName.Text.Equals("") && !txtStartDate.Text.Equals("") && !txtEndDate.Text.Equals(""))
            {
                return true;
            }
            else
            {
                MessageBox.Show("All fields are mandatory!");
                return false;
            }
            
        }

        private void UpdateSelectedCourse()
        {
            selectedCourse.CourseID = txtID.Text;
            selectedCourse.CourseName = txtCourseName.Text;
            selectedCourse.StartDate = Convert.ToDateTime(txtStartDate.Text);
            selectedCourse.EndDate = Convert.ToDateTime(txtEndDate.Text);
            selectedCourse.Places = Convert.ToInt32(txtPlaces.Text);
        }
        
        //New Course
        private void Button_New(object sender, RoutedEventArgs e)
        {
            if (CheckDataIsFilled())
            {
                UpdateSelectedCourse();

                if (DBServicesGeneral.dBServicesCourses.Insert(selectedCourse))
                {
                    MessageBox.Show("Course Created!");
                    txtID.Text = "";
                    txtCourseName.Text = "";
                    txtStartDate.Text = "";
                    txtEndDate.Text = "";
                    txtPlaces.Text = "";
                }
                else
                {
                    MessageBox.Show("An error occured while creating the Course.");
                }

                GetCourses(filter);
            }

        }
        //Update Course
        private void Button_Update(object sender, RoutedEventArgs e)
        {
            if (CheckDataIsFilled())
            {
                UpdateSelectedCourse();

                if (DBServicesGeneral.dBServicesCourses.Update(selectedCourse))
                {
                    MessageBox.Show("Course Updated!");
                }
                else
                {
                    MessageBox.Show("An error occured while updating your Course.");
                }

                GetCourses(filter);
                //UpdateSelectedCourse();
            }          

        }
        //Remove the Course
        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            UpdateSelectedCourse();

            if (DBServicesGeneral.dBServicesCourses.Delete(selectedCourse))
            {
                MessageBox.Show("Course Removed!");
            }
            else
            {
                MessageBox.Show("An error occured while deleting your Course.");
            }

            GetCourses(filter);

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter = (cmbFilter.SelectedValue.ToString().Contains("All Courses")) ? "All Courses" : "Available Courses";    
            GetCourses(filter);
        }
    }
}
