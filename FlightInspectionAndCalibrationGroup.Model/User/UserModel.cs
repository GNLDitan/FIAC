using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInspectionAndCalibrationGroup.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ContactNo { get; set; }

        public string ImageLocation { get; set; }
        public byte[] Image { get; set; }


    }

   
}
