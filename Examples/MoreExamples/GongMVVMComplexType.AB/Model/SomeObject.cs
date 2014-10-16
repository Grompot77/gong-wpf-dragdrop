using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GongComplexType.Model
{
    public class SomeObject : INotifyPropertyChanged
    {
        private string _firstName;
        private string _surname;
        private DateTime _doB;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }

        public DateTime DoB
        {
            get { return _doB; }
            set
            {
                _doB = value; 
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1} [{2}]", Surname, FirstName, DoB);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
