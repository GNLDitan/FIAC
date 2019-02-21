using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInspectionAndCalibrationGroup.Model
{
    public class EquipmentModel
    {
        public int Id { get; set; }
        public string StationDesc { get; set; }
        public string EquipmentDesc { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Remarks { get; set; }
    }
}
