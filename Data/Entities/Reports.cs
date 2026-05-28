using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDMS_App.Data.Entities
{
    internal class Reports
    {
        public string report_id;
        public int patient_id;
        public int doctor_id;
        public string result;
        public string treatment;
        public DateTime time_stamp;

        public Reports(string report_id, int patient_id, int doctor_id, string result, string treatment, DateTime time_stamp)
        {
            this.report_id = report_id;
            this.patient_id = patient_id;
            this.doctor_id = doctor_id;
            this.result = result;
            this.treatment = treatment;
            this.time_stamp = time_stamp;
        }
    }
}
