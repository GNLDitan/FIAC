using System;
using System.Collections.Generic;
using System.ComponentModel;
using FlightInspectionAndCalibrationGroup.Interface;

namespace FlightInspectionAndCalibrationGroup.Model
{
    public class UserModelProperties : INotifyPropertyChanged, IUserModel
    {
        public int id;
        public int Id
        {
            set
            {
                if (id != value)
                {
                    id = value;
                    onPropertyChanged("Id");
                }
            }
            get
            {
                return id;
            }
        }

        public string userName;
        public string UserName
        {
            set
            {
                if (userName != value)
                {
                    userName = value;
                    onPropertyChanged("UserName");
                }
            }
            get
            {
                return userName;
            }
        }
        public string password;
        public string Password
        {
            set
            {
                if (password != value)
                {
                    password = value;
                    onPropertyChanged("Password");
                }
            }
            get
            {
                return password;
            }
        }
        public string confirmPassword;
        public string ConfirmPassword
        {
            set
            {
                if (confirmPassword != value)
                {
                    confirmPassword = value;
                    onPropertyChanged("ConfirmPassword");
                }
            }
            get
            {
                return confirmPassword;
            }
        }

        public string promptError;
        public string PromptError
        {
            set
            {
                if (promptError != value)
                {
                    promptError = value;
                    onPropertyChanged("PromptError");
                }
            }
            get
            {
                return promptError;
            }
        }

        public string name;
        public string Name
        {
            set
            {
                if (name != value)
                {
                    name = value;
                    onPropertyChanged("Name");
                }
            }
            get
            {
                return name;
            }
        }

        public string contactNo;
        public string ContactNo {
            set
            {
                if (contactNo != value)
                {
                    contactNo = value;
                    onPropertyChanged("ContactNo");
                }
            }
            get
            {
                return contactNo;
            }
        }

        public string imageLocation { get; set; }

        public string ImageLocation
        {
            set
            {
                if (imageLocation!= value)
                {
                    imageLocation = value;
                    onPropertyChanged("ImageLocation");
                }
            }
            get
            {
                return imageLocation;
            }
        }


        

        public event PropertyChangedEventHandler PropertyChanged;
        public void onPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

        }

    }
}
