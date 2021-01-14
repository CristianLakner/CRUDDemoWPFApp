using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDDemoWPFApp.Models
{
    public class Students : INotifyPropertyChanged
    {
        public int RecNo { get; set; }
        private string _studentID;
        private string _firstName;
        private string _lastName;
        private DateTime _dateOfBirth;
        private bool _active;

        public event PropertyChangedEventHandler PropertyChanged;  
        public Students(){}
        public Students(string studentID, string firstName, string lastName, DateTime dateOfBirth, bool active)
        {
            _studentID = studentID;
            _firstName = firstName;
            _lastName = lastName;
            _dateOfBirth = dateOfBirth;
            _active = active;
        }
        public string StudentID
        {
            get { return _studentID; }
            set
            {
                _studentID = value;
                OnPropertyChanged("StudentID");
            }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        }
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                OnPropertyChanged("Active");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}