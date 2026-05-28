using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDMS_App.Data.Entities
{
    internal class Appointments
    {
        public string appointment_id;
        public int patient_id;
        public int doctor_id;
        public string? appointment_type;
        public DateTime scheduled_time;

        public Appointments(string appointment_id, int patient_id, int doctor_id, string appointment_type, DateTime scheduled_time)
        {
            this.appointment_id = appointment_id;
            this.patient_id = patient_id;
            this.doctor_id = doctor_id;
            this.appointment_type = appointment_type;
            this.scheduled_time = scheduled_time;
        }

        public Appointments(string appointment_id, int patient_id, int doctor_id, string scheduled_time)
        {
            this.appointment_id = appointment_id;
            this.patient_id = patient_id;
            this.doctor_id = doctor_id;
            this.scheduled_time = DateTime.Parse(scheduled_time);
        }
    }
}
