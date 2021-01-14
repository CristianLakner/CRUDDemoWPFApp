using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CRUDDemoWPFApp.Services
{
    public static class DBServicesGeneral
    {
        public static DBServicesCourses dBServicesCourses = new DBServicesCourses();
        public static DBServicesStudents dbServicesStudents = new DBServicesStudents();
        private static SqlConnection connection;

        public static void InsertToJoinTable(string CourseId, string StudentId)
        {  
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);

            try
            {
                if (OpenConnection() == true)
                {
                    //inseram datele
                    SqlCommand cmd = new SqlCommand("INSERT INTO dbo.StudentsCoursesJunction (studentID, courseID) VALUES (@studentId, @courseId)", connection);

                    cmd.Parameters.AddWithValue("@studentId", StudentId);
                    cmd.Parameters.AddWithValue("@courseId", CourseId);
                    cmd.ExecuteNonQuery();
                    
                    //ajustam numarul de locuri libere in tabela Course
                    SqlCommand cmdUpdate = new SqlCommand("UPDATE dbo.Courses SET Places = Places - 1 WHERE CourseID = @courseId", connection);
                    cmdUpdate.Parameters.AddWithValue("@courseId", CourseId);
                    cmdUpdate.ExecuteNonQuery();

                    CloseConnection();
                }

                MessageBox.Show("Student has joined the Course!");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static bool OpenConnection()
        {
            try
            {
                CloseConnection();
                connection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server!");
                        break;
                    case 1045:
                        MessageBox.Show("Invalid credentials!");
                        break;
                }
                return false;
            }
        }

        private static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

    }
}