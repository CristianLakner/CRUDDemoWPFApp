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
    /// Interaction logic for StudentsPage.xaml
    /// </summary>
    public partial class StudentsPage : UserControl
    {
        private Students selectedStudent = new Students();
        private ObservableCollection<Students> students = new ObservableCollection<Students>();
        private string filter = "All Students";

        public StudentsPage()
        {
            InitializeComponent();
            GetStudents();
        }
        private void CmbFilterView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvStudents != null)
            { 
                GetStudents();
            }
        }
        private void GetStudents()
        {
            filter = (cmbFilterView.SelectedValue.ToString().Contains("All Students")) ? "All Students" : "Active Students";
            students = DBServicesGeneral.dbServicesStudents.Select(filter);
            if (students.Count > 0)
            {
                dgvStudents.ItemsSource = students;
            }  
        }

        private void BtnBackToMainPage_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = new MainWindow();
            mainPage.Show();
        }

        private void DgvStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvStudents.SelectedIndex >= 0)
            {
                selectedStudent = (Students)dgvStudents.SelectedItem;
                txtStudentID.Text = selectedStudent.StudentID;
                txtFirstName.Text = selectedStudent.FirstName;
                txtLastName.Text = selectedStudent.LastName;
                txtDateOfBirth.Text = selectedStudent.DateOfBirth.ToShortDateString();
                txtActive.Text = selectedStudent.Active.ToString();
            }
        }

        private void UpdateSelectedStudent()
        {
            selectedStudent.StudentID = txtStudentID.Text;
            selectedStudent.FirstName = txtFirstName.Text;
            selectedStudent.LastName = txtLastName.Text;
            selectedStudent.DateOfBirth = Convert.ToDateTime(txtDateOfBirth.Text);
            selectedStudent.Active = Convert.ToBoolean(txtActive.Text);
        }

        private bool CheckDataIsFilled()
        {

            if (!txtStudentID.Text.Equals("") && !txtFirstName.Text.Equals("") && !txtLastName.Text.Equals("") && !txtDateOfBirth.Text.Equals("") && !txtActive.Text.Equals(""))
            {
                return true;
            }
            else
            {
                MessageBox.Show("All fields are mandatory!");
                return false;
            }

        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            if (CheckDataIsFilled())
            {
                UpdateSelectedStudent();

                if (DBServicesGeneral.dbServicesStudents.Insert(selectedStudent))
                {
                    MessageBox.Show("Student Added!");
                    txtStudentID.Text = "";
                    txtFirstName.Text = "";
                    txtLastName.Text = "";
                    txtDateOfBirth.Text = "";
                    txtActive.Text = "";
                }
                else
                {
                    MessageBox.Show("An error occured while creating the Course.");
                }

                GetStudents();

            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (CheckDataIsFilled())
            {
                UpdateSelectedStudent();

                if (DBServicesGeneral.dbServicesStudents.Update(selectedStudent))
                {
                    MessageBox.Show("Student Updated!");
                }
                else
                {
                    MessageBox.Show("An error occured while updating the Student.");
                }

                GetStudents();
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedStudent();

            if (DBServicesGeneral.dbServicesStudents.Delete(selectedStudent))
            {
                MessageBox.Show("Student Inactive!");
            }
            else
            {
                MessageBox.Show("An error occured while inactivating the Student.");
            }

            GetStudents();
        }
    }
}
