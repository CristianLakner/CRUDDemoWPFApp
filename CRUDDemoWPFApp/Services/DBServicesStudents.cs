using CRUDDemoWPFApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CRUDDemoWPFApp.Services
{
    public class DBServicesStudents
    {
        private SqlConnection connection;
        public DBServicesStudents()
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

        //Insert New Student
        public bool Insert(Students student)
        {

            try
            {
                if (this.OpenConnection() == true)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Students (StudentID, FirstName, LastName, DateOfBirth, Active) VALUES (@studentId, @firstName, @lastName, @dateOfBirth, @active)", connection);

                    cmd.Parameters.AddWithValue("@studentId", student.StudentID);
                    cmd.Parameters.AddWithValue("@firstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", student.LastName);
                    cmd.Parameters.AddWithValue("@dateOfBirth", student.DateOfBirth);
                    cmd.Parameters.AddWithValue("@active", student.Active);

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

        //Update Student
        public bool Update(Students student)
        {
            try
            {

                if (this.OpenConnection() == true)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE dbo.Students SET FirstName = @firstName, LastName = @lastName, DateOfBirth = @dateOfBirth, Active = @active WHERE StudentID = @studentId", connection);

                    cmd.Parameters.AddWithValue("@firstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", student.LastName);
                    cmd.Parameters.AddWithValue("@dateOfBirth", student.DateOfBirth);
                    cmd.Parameters.AddWithValue("@active", student.Active);
                    cmd.Parameters.AddWithValue("@studentId", student.StudentID);

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

        //Delete Student - set Active to 0, instead of removing information from Database
        public bool Delete(Students student)
        {
            try
            {
                if (this.OpenConnection() == true)
                {
                    //SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Students WHERE studentID = ?studentId", connection);
                    SqlCommand cmd = new SqlCommand("UPDATE dbo.Students set Active = 0 WHERE StudentID = @studentId", connection);
                    cmd.Parameters.AddWithValue("@studentId", student.StudentID);
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

        //Select Students
        public ObservableCollection<Students> Select(string filter)
        {
            string query = "";

            if (filter.Equals("All Students")) //select all Students
            {
                query = "SELECT * FROM dbo.Students ORDER BY FirstName";
            }
            else //select only Active students
            {
                query = "SELECT * FROM dbo.Students WHERE Active = 1 ORDER BY FirstName ";
            }

            //list of Students
            ObservableCollection<Students> students = new ObservableCollection<Students>();
            
            if (this.OpenConnection() == true)
            {
                
                SqlCommand cmd = new SqlCommand(query, connection);
                
                SqlDataReader dataReader = cmd.ExecuteReader();
                
                while (dataReader.Read())
                {
                    Students student = new Students();

                    student.RecNo = Convert.ToInt32(dataReader["RecNo"]);
                    student.StudentID = Convert.ToString(dataReader["StudentID"]);
                    student.FirstName = Convert.ToString(dataReader["FirstName"]);
                    student.LastName = Convert.ToString(dataReader["LastName"]);
                    student.DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]);
                    student.Active = Convert.ToBoolean(dataReader["Active"]);

                    students.Add(student);
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                //return the collection
                return students;
            }
            else
            {
                return null;
            }
        }

    }
}
