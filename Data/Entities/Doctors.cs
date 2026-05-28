using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDMS_App.Data.Entities
{
    internal class Doctors
    {
        public int doctor_id;
        public string doctor_name;
        public string? specialization;
        public int? experience;
        public long? contact;

        public Doctors(int doctor_id, string doctor_name, string specialization, int experience, long contact)
        {
            this.doctor_id = doctor_id;
            this.doctor_name = doctor_name;
            this.specialization = specialization;
            this.experience = experience;
            this.contact = contact;
        }

        public Doctors(int doctor_id, string doctor_name, string specialization)
        {
            this.doctor_id = doctor_id;
            this.doctor_name = doctor_name;
            this.specialization = specialization;
        }
    }
}
