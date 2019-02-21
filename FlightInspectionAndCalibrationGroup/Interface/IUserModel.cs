using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInspectionAndCalibrationGroup.Interface
{
    public interface IUserModel
    {
        int Id { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        string Name { get; set; }

        string ContactNo { get; set; }

        string ImageLocation { get; set; }
    }
}
