using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
namespace LinqAssignments
{
    class StudentDetails
    {
        public class Student
        {
            public int StudentId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FatherFirstName { get; set; }
            public string FatherLastName { get; set; }
            public string FatherMobileNumber { get; set; }
            public string MotherFirstName { get; set; }
            public string MotherLastName { get; set; }
            public string MotherMobileNumber { get; set; }
            public string Address { get; set; }
        }

        public class BusFee
        {
            public int StudentId { get; set; }
            public float AmountToPay { get; set; }
            public float FeesCollected { get; set; }
            public string DatePaid { get; set; }
            public string BusPickUppoint { get; set; }
            public int BusNumber { get; set; }
            public int BusDriverId { get; set; }

        }

        public class DriverDetail
        {
            public int DriverId { get; set; }
            public string DriverFirstName { get; set; }
            public string DriverLastName { get; set; }
            public string DriverMobileNumber { get; set; }
            public string DrivingLicenseNumber { get; set; }
            public string AddharCard { get; set; }
            public string BankAccountNumber { get; set; }
            public string IfscCode { get; set; }

        }
        static void Main(string[]  args)
        { 
            string studentDetails = @"C:\Datagrokr\csharp\LinqAssignments\LinqAssignments\Share\studentDetails.csv";
            string studentBusFee  = @"C:\Datagrokr\csharp\LinqAssignments\LinqAssignments\Share\busFee.csv";
            string driverDetails  = @"C:\Datagrokr\csharp\LinqAssignments\LinqAssignments\Share\driverDetails.csv";

            if (File.Exists(studentDetails)&& File.Exists(studentBusFee)&& File.Exists(driverDetails))
            {
                var studentInfo = new List<Student>();
                var busFeeInfo  = new List<BusFee>();
                var driverInfo  = new List<DriverDetail>();

                try
                {
                    using (StreamReader reader = new StreamReader(File.OpenRead(studentDetails)))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            if (count > 0)
                            {

                                studentInfo.Add(new Student()
                                {
                                    StudentId = int.Parse(values[0]),
                                    FirstName = values[1],
                                    LastName = values[2],
                                    FatherFirstName = values[3],
                                    FatherLastName = values[4],
                                    FatherMobileNumber = values[5],
                                    MotherFirstName = values[6],
                                    MotherLastName = values[7],
                                    MotherMobileNumber = values[8],
                                    Address = values[9]
                                });

                            }
                            else
                            {
                                count++;
                                //Console.WriteLine(count);
                            }

                        }

                    }



                    using (StreamReader reader = new StreamReader(studentBusFee))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            if (count > 0)
                            {

                                busFeeInfo.Add(new BusFee()
                                {
                                    StudentId = int.Parse(values[0]),
                                    AmountToPay = float.Parse(values[1]),
                                    FeesCollected = float.Parse(values[2]),
                                    DatePaid = values[3],
                                    BusPickUppoint = values[4],
                                    BusNumber = int.Parse(values[5]),
                                    BusDriverId = int.Parse(values[6])

                                });

                            }
                            else
                            {
                                count++;
                                //Console.WriteLine(count);
                            }
                        }

                    }
                    using (StreamReader reader = new StreamReader(driverDetails))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            if (count > 0)
                            {

                                driverInfo.Add(new DriverDetail()
                                {
                                    DriverId = int.Parse(values[0]),
                                    DriverFirstName = values[1],
                                    DriverLastName = values[2],
                                    DriverMobileNumber = values[3],
                                    DrivingLicenseNumber = values[4],
                                    AddharCard = values[5],
                                    BankAccountNumber = values[6],
                                    IfscCode = values[7]

                                });

                            }
                            else
                            {
                                count++;
                                //Console.WriteLine(count);
                            }

                        }
                    }

                    //Query the contact number of the bus driver who picks up 'Sathya Sri'.
                    //Write 2 functions as solutions where each uses Lambda expressions or Sql queries to query.

                    var driverContactNumberSqlQuery = from student in studentInfo
                                                      join busfee in busFeeInfo on student.StudentId equals busfee.StudentId
                                                      join driver in driverInfo on busfee.BusDriverId equals driver.DriverId
                                                      where student.FirstName == "Sathya" && student.LastName == "Sri"
                                                      select driver;

                    driverContactNumberSqlQuery.ToList().ForEach(s => Console.WriteLine($"{s.DriverMobileNumber}"));

        
                    var driverContactNumberByLambdaExpression = studentInfo.Join(busFeeInfo, s => s.StudentId, busf => busf.StudentId, (s, busf) => new { s, busf })
                                                                           .Join(driverInfo, bs => bs.busf.BusDriverId, d => d.DriverId, (bs, d) => new { bs, d })
                                                                           .Where(studentName => (studentName.bs.s.FirstName == "Sathya" && studentName.bs.s.LastName == "Sri"))
                                                                           .Select(contactNumber => contactNumber.d.DriverMobileNumber);
                    driverContactNumberByLambdaExpression.ToList().ForEach(number => Console.WriteLine(number));

                }
                
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("File doesn't exists");
            }


        }
       
    }
}
