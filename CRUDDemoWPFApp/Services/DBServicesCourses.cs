using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CRUDDemoWPFApp.Models;
using System.Collections.ObjectModel;

namespace CRUDDemoWPFApp.Services
{
    public class DBServicesCourses
    {
        private SqlConnection connection;
        public DBServicesCourses()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString);
        }
        
        //Open Connection
        private bool OpenConnection()
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
        
        //Close connection
        private bool CloseConnection()
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
        
        //Insert New Course
        public bool Insert(Courses course)
        {

            try
            {
                if (this.OpenConnection() == true)
                { 
                    SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Courses (CourseID, CourseName, StartDate, EndDate, Places) VALUES (@courseId, @courseName, @startDate, @endDate, @places)", connection);

                    cmd.Parameters.AddWithValue("@courseId", course.CourseName);
                    cmd.Parameters.AddWithValue("@courseName", course.CourseName);
                    cmd.Parameters.AddWithValue("@startDate", course.StartDate);
                    cmd.Parameters.AddWithValue("@endDate", course.EndDate);
                    cmd.Parameters.AddWithValue("@places", course.Places);

                    cmd.ExecuteNonQuery();
                    
                    this.CloseConnection();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Update Course
        public bool Update(Courses course)
        {
            try
            {
                
                if (this.OpenConnection() == true)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE dbo.Courses SET CourseName = @courseName, StartDate = @startDate, EndDate = @endDate, Places = @places WHERE CourseID = @courseId", connection);
                    cmd.Parameters.AddWithValue("@courseName", course.CourseName);
                    cmd.Parameters.AddWithValue("@startDate", course.StartDate);
                    cmd.Parameters.AddWithValue("@endDate", course.EndDate);
                    cmd.Parameters.AddWithValue("@places", course.Places);
                    cmd.Parameters.AddWithValue("@courseId", course.CourseID);
                    
                    cmd.ExecuteNonQuery();
                    
                    this.CloseConnection();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Delete Course - set StartDate to Null value, instead of removing information from Database
        public bool Delete(Courses course) 
        {
            try
            {
                if (this.OpenConnection() == true)
                {
                    //SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Courses WHERE CourseID = ?courseId", connection);
                    SqlCommand cmd = new SqlCommand("UPDATE dbo.Courses set StartDate = 'Null' WHERE CourseID = @courseId", connection);
                    cmd.Parameters.AddWithValue("@courseId", course.CourseID);
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Select Courses
        public ObservableCollection<Courses> Select(string filter)
        {
            string query = "";

            if (filter.Equals("All Courses")) //select all Courses
            {
                query = "SELECT * FROM dbo.Courses ORDER BY StartDate";
            }
            else //select only available Courses (by date) and also with places available
            {
                query = "SELECT c.* FROM dbo.Courses c ";
                query += " LEFT JOIN (SELECT courseID, COUNT(courseID) PlacesReserved FROM dbo.StudentsCoursesJunction GROUP BY courseID) j ON j.courseID = c.CourseID";
                query += " WHERE StartDate is not null AND c.Places > isnull(j.PlacesReserved, 0) AND StartDate > '" + DateTime.Now.ToString("M/d/yyyy") + "'";
            }

            //list of Courses
            ObservableCollection<Courses> courses = new ObservableCollection<Courses>();
            
            if (this.OpenConnection() == true)
            {
                
                SqlCommand cmd = new SqlCommand(query, connection);
                
                SqlDataReader dataReader = cmd.ExecuteReader();
                
                while (dataReader.Read())
                {
                    Courses course = new Courses();
       
                    course.CourseID = Convert.ToString(dataReader["CourseID"]);
                    course.CourseName = Convert.ToString(dataReader["CourseName"]);
                    course.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                    course.EndDate = Convert.ToDateTime(dataReader["EndDate"]);
                    course.Places = Convert.ToInt32(dataReader["Places"]);
                    course.RecNo = Convert.ToInt32(dataReader["RecNo"]);
                    
                    courses.Add(course);
                }
                
                dataReader.Close();
                
                this.CloseConnection();
                
                return courses;
            }
            else
            {
                return null;
            }
        }
    }
}