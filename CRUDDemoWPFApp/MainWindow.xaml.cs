using CRUDDemoWPFApp.Models;
using CRUDDemoWPFApp.Services;
using CRUDDemoWPFApp.UserInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace CRUDDemoWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Students selectedStudent = new Students();
        private ObservableCollection<Students> students = new ObservableCollection<Students>();

        private Courses selectedCourse = new Courses();
        private ObservableCollection<Courses> courses = new ObservableCollection<Courses>();

        public MainWindow()
        {
            InitializeComponent();
            GetStudents();
            GetCourses();
            GetLinkedCourses();
        }

        private void GetStudents()
        {
            students = DBServicesGeneral.dbServicesStudents.Select("All Students");
            dgvStudents.ItemsSource = students;
        }

        private void GetCourses()
        {
            courses = DBServicesGeneral.dBServicesCourses.Select("All Courses");
            dgvCourses.ItemsSource = courses;
        }

        private void GetLinkedCourses()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("select s.FirstName, s.LastName, c.CourseName, c.StartDate, c.Places from StudentsCoursesJunction sc join Courses c on c.CourseID = sc.courseID join Students s on s.StudentID = sc.StudentID order by sc.courseID, sc.StudentID", connection);
            DataTable dt = new DataTable("LinkedData");
            da.Fill(dt);
            dgvLinked.ItemsSource = dt.DefaultView;
            connection.Close();
        }

        private void BtnCourses_Click(object sender, RoutedEventArgs e)
        {
            CoursesPage coursesPage = new CoursesPage();
            this.Content = coursesPage;
        }

        private void BtnStudents_Click(object sender, RoutedEventArgs e)
        {
            StudentsPage studentsPage = new StudentsPage();
            this.Content = studentsPage;
        }

        private void DgvCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvCourses.SelectedIndex >= 0)
            {
                selectedCourse = (Courses)dgvCourses.SelectedItem;
                txtSelectedCourse.Text = selectedCourse.CourseName;
            }
        }

        private void DgvStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvStudents.SelectedIndex >= 0)
            {
                selectedStudent = (Students)dgvStudents.SelectedItem;
                txtSelectedStudent.Text = selectedStudent.FirstName.Trim() + ", " + selectedStudent.LastName.Trim();
            }
        }

        private void BtnLink_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder strErr = new StringBuilder();
            
            //facem validari inainte de a insera datele
       
            //verificam daca cursul a inceput deja
            if (selectedCourse.StartDate <= DateTime.Today)
            {
                strErr.Append("Course has already started!\n");
            }
            
            //verificam daca cursul mai are locuri libere
            if (selectedCourse.Places < 1)
            {
                strErr.Append("Course is full, there are no free places!\n");
            }

            //verificam daca userul este activ
            if (selectedStudent.Active == false)
            {
                strErr.Append("Only active Students can join the Course");
            }

            if (strErr.Length != 0)
            {
                MessageBox.Show("Errors found!\n\n" + strErr.ToString());
            }
            else
            {
                DBServicesGeneral.InsertToJoinTable(selectedCourse.CourseID, selectedStudent.StudentID);
                dgvLinked.Items.Clear();
                GetLinkedCourses();
            }
        }
    }
}
