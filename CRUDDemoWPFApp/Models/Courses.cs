using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDDemoWPFApp.Models
{
    public class Courses : INotifyPropertyChanged
    {
        public int RecNo { get; set; }
        private string _courseID;
        private string _courseName;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _places;

        public event PropertyChangedEventHandler PropertyChanged;

        public Courses() { }
        public Courses(string courseID, string courseName, DateTime startDate, DateTime endDate, int places)
        {
            _courseID = courseID;
            _courseName = courseName;
            _startDate = startDate;
            _endDate = endDate;
            _places = places;
        }
        public string CourseID
        {
            get { return _courseID; }
            set
            {
                _courseID = value;
                OnPropertyChanged("CourseID");
            }
        }
        public string CourseName
        {
            get { return _courseName; }
            set
            {
                _courseName = value;
                OnPropertyChanged("CourseName");
            }
        }
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
            }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }
        public int Places
        {
            get { return _places; }
            set
            {
                _places = value;
                OnPropertyChanged("Places");
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
