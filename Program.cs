using System.ComponentModel.Design;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using HDMS_App.Data.Entities;

namespace HDMS_App
{
    internal class Program
    {
        static List<Doctors> doctors_list = new List<Doctors>();
        static List<Patients> patients_list = new List<Patients>();
        static List<Reports> reports_catalog = new List<Reports>();
        static List<Appointments> appointments_catalog = new List<Appointments>();

        static string datasource = @"YOUR-SERVER-NAME";
        static string database = "YOUR-DATABASE-NAME";

        static string connString = @"Data Source=" + datasource +
            ";Initial Catalog=" + database + "; Trusted_Connection=True;TrustServerCertificate=True;";
        static void Main(string[] args)
        {
            Console.WriteLine("loading database...");
            load_database();

            while(true)
            {
                 Console.Clear();
                Console.WriteLine("====== Welcome to Vintage Hospitals ======");
                Console.WriteLine("\n1. Management portal");
                Console.WriteLine("2. Visitors Portal");
                Console.Write("select the page: ");
                int page = Convert.ToInt32(Console.ReadLine());
                if (page == 1)
                {
                    Console.Write("\nEnter the access code: ");
                    if (Console.ReadLine() == "YOUR-ACCESS-CODE")
                    {
                        int next = 1;
                        while (next != 0)
                        {
                            
                            Console.WriteLine("\n====== MANAGEMENT'S PORTAL ======");
                            Console.WriteLine("1. Add Doctors");
                            Console.WriteLine("2 Add Report");
                            Console.WriteLine("3. View Patients");
                            Console.WriteLine("4. View Report Catalog");
                            Console.WriteLine("5. Exit");
                            Console.Write("Enter your choice: ");
                            int choice = Convert.ToInt32(Console.ReadLine());
                            switch (choice)
                            {
                                case 1:
                                    add_doctor();
                                    break;
                                case 2:
                                    add_report();
                                    break;
                                case 3:
                                    view_patients();
                                    break;
                                case 4:
                                    view_reports();
                                    break;
                                case 5:
                                    break;

                            }

                            Console.WriteLine("\nEnter 0 to exit / 1 to continue");
                            next = Convert.ToInt32(Console.ReadLine());

                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong Access code!");
                    }

                }

                else
                {
                    int next = 1;
                    while (next != 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("====== VISITOR'S PORTAL ======");
                        Console.WriteLine("1. Add Patient");
                        Console.WriteLine("2 View Doctors");
                        Console.WriteLine("3. Book appointment");
                        Console.WriteLine("4. Exit");
                        Console.Write("Enter your choice: ");
                        int choice = Convert.ToInt32(Console.ReadLine());
                        switch (choice)
                        {
                            case 1:
                                Add_patient();
                                break;
                            case 2:
                                view_doctors();
                                break;
                            case 3:
                                book_appointment();
                                break;
                            case 4:
                                break;

                        }

                        Console.WriteLine("\nEnter 0 to exit / 1 to continue");
                        next = Convert.ToInt32(Console.ReadLine());

                    }
                }
            }
            


        }

        static void view_doctors()
        {
            Console.WriteLine();
            Console.WriteLine("--- Doctor's list ---");
            Console.WriteLine("{0,-15} {1,-15} {2,-10} {3,-5} {4, -15}", "ID", "Name", "Specialization", "Experience", "Contact");
            foreach (var i in doctors_list)
            {
                Console.WriteLine("{0,-15} {1,-15} {2,-10} {3,-5} {4, -15}", i.doctor_id, i.doctor_name, i.specialization, i.experience, i.contact);
            }
        }

        static void view_patients()
        {
            Console.WriteLine();
            Console.WriteLine("--- Patient's list ---");
            Console.WriteLine("{0,-15} {1,-15} {2,-10} {3,-5} {4, -15}", "ID", "Name", "Age", "Symptoms", "Contact");
            foreach (var i in patients_list)
            {
                Console.WriteLine("{0,-15} {1,-15} {2,-10} {3,-5} {4, -15}", i.patient_id, i.patient_name, i.age, i.symptoms, i.contact);
            }
        }

        static void Add_patient()
        {
            Console.Write("Patient Details (Quick/Detailed)? ");
            string mode = Console.ReadLine();

            Console.Write("Patient's Name: ");
            string name = Console.ReadLine();

            Console.Write("Patient's Symptoms: ");
            string symptoms = Console.ReadLine();

            if(mode == "Detailed")
            {
                Console.Write("Patient's Age: ");
                int age = Convert.ToInt32(Console.ReadLine());

                Console.Write("Patient's Contact: ");
                long contact = Convert.ToInt64(Console.ReadLine());

                Patients p = new Patients(name, age, symptoms, contact);
                patients_list.Add(p);
                

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        string insert_query = "insert into patients values(@name, @age, @symptoms, @contact);" +
                            "select scope_identity();";
                        SqlCommand cm = new SqlCommand(insert_query, conn);


                        cm.Parameters.AddWithValue("@name", name);
                        cm.Parameters.AddWithValue("@age", age);
                        cm.Parameters.AddWithValue("@symptoms", symptoms);
                        cm.Parameters.AddWithValue("@contact", contact);

                        var result = cm.ExecuteScalar();
                        
                        if (result != null)
                        {
                            Console.WriteLine("Patient registered succesfully");
                        }

                        p.patient_id = Convert.ToInt32(result);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }               

            }
            else
            {
                Patients p = new Patients(name, symptoms);
                patients_list.Add(p);
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        string insert_query = "insert into patients values(@name, @age, @symptoms, @contact);" +
                            "select scope_identity();";
                        SqlCommand cm = new SqlCommand(insert_query, conn);


                        cm.Parameters.AddWithValue("@name", name);
                        cm.Parameters.AddWithValue("@symptoms", symptoms);

                        var result = cm.ExecuteScalar();

                        if (result != null)
                        {
                            Console.WriteLine("Patient registered succesfully");
                        }

                        p.patient_id = Convert.ToInt32(result);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            

        }


        static void view_reports()
        {
            Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4, -10}", "Report ID", "Patient ID", "Doctor ID", "Results", "Treatment");
            foreach (Reports report in reports_catalog)
            {
                Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4, -10}", report.report_id, report.patient_id, report.doctor_id, report.result, report.treatment);
            }
        }

        static void add_doctor()
        {
            Console.Write("Doctor's Details (Quick/Detailed)? ");
            string mode = Console.ReadLine();

            Console.Write("Doctor's ID: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Doctor's Name: ");
            string name = Console.ReadLine();

            Console.Write("Doctor's Specialization: ");
            string spec = Console.ReadLine();

            if (mode == "Detailed")
            {
                Console.Write("Doctor's Experience: ");
                int exp = Convert.ToInt32(Console.ReadLine());

                Console.Write("Doctor's Contact: ");
                long contact = Convert.ToInt64(Console.ReadLine());

                Doctors d = new Doctors(id, name, spec, exp , contact);
                doctors_list.Add(d);

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        string insert_query = "insert into doctors values(@id, @name, @spec, @exp, @contact);";
                        SqlCommand cm = new SqlCommand(insert_query, conn);

                        cm.Parameters.AddWithValue("@id", id);
                        cm.Parameters.AddWithValue("@name", name);
                        cm.Parameters.AddWithValue("@spec", spec);
                        cm.Parameters.AddWithValue("@exp", exp);
                        cm.Parameters.AddWithValue("@contact", contact);

                        int result = cm.ExecuteNonQuery();

                        if (result > 0)
                        {
                            Console.WriteLine($"Welcome on board {name}");
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            else
            {
                Doctors d = new Doctors(id, name, spec);
                doctors_list.Add(d);

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        string insert_query = "insert into patients values(@id, @name, @spec);";
                        SqlCommand cm = new SqlCommand(insert_query, conn);

                        cm.Parameters.AddWithValue("@id", id);
                        cm.Parameters.AddWithValue("@name", name);
                        cm.Parameters.AddWithValue("@spec", spec);

                        int result = cm.ExecuteNonQuery();
                         
                        if (result > 0)
                        {
                            Console.WriteLine($"Welcome onboard {name}");
                        }

               


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        static void add_report()
        {
            string id = "REP" + Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper();
            Console.WriteLine();
            Console.WriteLine("Enter report details");
            
            Console.Write("Patient ID: ");
            int patient_id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Doctor ID: ");
            int doctor_id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Result: ");
            string result = Console.ReadLine();
            Console.Write("Treatment");
            string treatment = Console.ReadLine();
            DateTime time_stamp = DateTime.Now;
            Reports r = new Reports(id, patient_id, doctor_id, result, treatment, time_stamp);
            reports_catalog.Add(r);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string insert_query = "insert into reports values(@id, @patient_id, @doctor_id, @result, @treatment, @time_stamp);";
                    SqlCommand cm = new SqlCommand(insert_query, conn);

                    cm.Parameters.AddWithValue("@id", id);
                    cm.Parameters.AddWithValue("@patient_id", patient_id);
                    cm.Parameters.AddWithValue("@doctor_id", doctor_id);
                    cm.Parameters.AddWithValue("@result", result);
                    cm.Parameters.AddWithValue("@treatment", treatment);
                    cm.Parameters.AddWithValue("@time_stamp", DateTime.Now);

                    int rows = cm.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        Console.WriteLine("Report Added succesfully");
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        static void book_appointment()
        {
            Console.WriteLine();
            Console.Write("Select the Doctor: ");
            string d_name = Console.ReadLine();
            var doc = doctors_list.Find(d => d.doctor_name == d_name);
            if (doc == null) { Console.WriteLine("enter an existing doctor's name"); }
            else
            {
                Console.WriteLine("Enter the required time slot");
                DateTime dateTime = DateTime.Parse(Console.ReadLine());

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();
                        string query = $"SELECT a.scheduled_time FROM appointments a join doctors d on a.doctor_id = d.doctor_id where d.doctor_name = @d_name";

                        bool slot_available = true;
                        using (SqlCommand cm = new SqlCommand(query, conn))
                        {
                            cm.Parameters.AddWithValue("@d_name", d_name);
                            using (SqlDataReader reader = cm.ExecuteReader())
                            {
                                
                                while (reader.Read())
                                {
                                    DateTime v = (DateTime)reader["scheduled_time"];
                                    if (v == dateTime)
                                    {
                                        slot_available = false;
                                        break;
                                    }
                                }

                                reader.Close();

                                
                            }
                        }

                        if (!slot_available)
                        {
                            Console.WriteLine("Slot Unavailable. try later or for other slots");
                        }

                        else
                        {
                            Console.WriteLine("Slot Available! proceeding to booking...");

                            string id = "APT" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
                            Console.WriteLine("Patient ID");
                            int patient_id = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("appointment type: ");
                            string ap_type = Console.ReadLine();
                            Appointments ap = new Appointments(id, patient_id, doc.doctor_id, ap_type, dateTime);
                            appointments_catalog.Add(ap);

                            string insertQuery = "insert into appointments values (@id, @patient_id, @doctor_id, @ap_type, @dateTime)";

                            using (SqlCommand insertCm = new SqlCommand(insertQuery, conn))
                            {
                                insertCm.Parameters.AddWithValue("@id", id);
                                insertCm.Parameters.AddWithValue("@patient_id", patient_id);
                                insertCm.Parameters.AddWithValue("@doctor_id", doc.doctor_id);
                                insertCm.Parameters.AddWithValue("@ap_type", ap_type);
                                insertCm.Parameters.AddWithValue("@dateTime", dateTime);

                                int rows = insertCm.ExecuteNonQuery();
                                if (rows > 0)
                                {
                                    Console.WriteLine("Appointment booked successfully");
                                }
                            }
                        }
                    }

                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    
                }
            }
        }

        static void load_database()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    doctors_list.Clear();
                    patients_list.Clear();
                    reports_catalog.Clear();
                    appointments_catalog.Clear();
                    
                    conn.Open();
                    string loadPatients = "select * from patients";

                    using (SqlCommand cm = new SqlCommand(loadPatients, conn))
                    {
                        using (SqlDataReader reader = cm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Patients p = new Patients(

                                    reader["patient_name"].ToString(),
                                    Convert.ToInt32(reader["age"]),
                                    reader["symptoms"].ToString(),
                                    Convert.ToInt64(reader["contact"])
                                );

                                p.patient_id = Convert.ToInt32(reader["patient_id"]);
                                patients_list.Add(p);
                            }

                            reader.Close();
                        }
                    }

                    string loadDoctors = "select * from doctors";

                    using (SqlCommand cm = new SqlCommand(loadDoctors, conn))
                    {
                        using (SqlDataReader reader = cm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Doctors d = new Doctors(

                                    Convert.ToInt32(reader["doctor_id"]),
                                    reader["doctor_name"].ToString(),
                                    reader["specialization"].ToString(),
                                    Convert.ToInt32(reader["experience"]),
                                    Convert.ToInt64(reader["contact"])
                                );

                                doctors_list.Add(d);
                            }
                            reader.Close();
                        }
                    }

                    string loadReports = "select * from reports";

                    using (SqlCommand cm = new SqlCommand(loadReports, conn))
                    {
                        using (SqlDataReader reader = cm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Reports r = new Reports(

                                    reader["report_id"].ToString(),
                                    Convert.ToInt32(reader["patient_id"]),
                                    Convert.ToInt32(reader["doctor_id"]),
                                    reader["result"].ToString(),
                                    reader["treatment"].ToString(),
                                    Convert.ToDateTime(reader["time_stamp"])
                                );

                                reports_catalog.Add(r);
                            }
                            reader.Close();
                        }
                    }

                    string loadAppointments = "select * from appointments";

                    using (SqlCommand cm = new SqlCommand(loadReports, conn))
                    {
                        using (SqlDataReader reader = cm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Appointments a = new Appointments(

                                    reader["appointment_id"].ToString(),
                                    Convert.ToInt32(reader["patient_id"]),
                                    Convert.ToInt32(reader["doctor_id"]),
                                    reader["appointment_type"].ToString(),
                                    Convert.ToDateTime(reader["scheduled_time"])
                                );

                                appointments_catalog.Add(a);
                            }
                            reader.Close();
                        }
                    }

                    
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            Console.WriteLine("Database loaded successfully, redirecting to the application");
            
        } 

    }
}
