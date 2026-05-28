using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HDMS_App.Data.Entities
{
    internal class Patients
    {
        public int patient_id;
        public string patient_name;
        public int? age;
        public string symptoms;
        public long? contact; 

        public Patients(string patient_name, int age, string symptoms, long contact)
        {
            this.patient_name = patient_name;
            this.age = age;
            this.symptoms = symptoms;
            this.contact = contact;
        }

        public Patients(string patient_name, string symptoms)
        {

            this.patient_name = patient_name;
            this.symptoms = symptoms;
        }
    }
}
