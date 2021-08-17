using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
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

        public class BillStatement
        {
            public int StudentId { get; set; }
            public float AmountToPay { get; set; }
            public float FeesCollected { get; set; }
            public string DatePaid { get; set; }
            public string BusPickUppoint { get; set; }
            public int BusNumber { get; set; }
            public int BusDriverId { get; set; }

        }

        public class BillReport
        {
            public int StudentId { get; set; }
            public float AmountToPay { get; set; }
            public string FatherMobileNumber { get; set; }
            public string MotherMobileNumber { get; set; }
        }
        static void Main(string[] args)
        {
            string studentDetailsCsvFile = @"C:\Datagrokr\csharp\LinqAssignments\LinqAssignments\Share\studentDetails.csv";
            string studentBillStatementCsvFile = @"C:\Datagrokr\csharp\LinqAssignments\LinqAssignments\Share\billStatement.csv";
            
            if (File.Exists(studentBillStatementCsvFile) && File.Exists(studentDetailsCsvFile) )
            {
                var studentInfo = new List<Student>();
                var billInfo = new List<BillStatement>();         
                try
                {
                    using (StreamReader reader = new StreamReader(File.OpenRead(studentDetailsCsvFile)))
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
                    using (StreamReader reader = new StreamReader(studentBillStatementCsvFile))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            if (count > 0)
                            {
                                billInfo.Add(new BillStatement()
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
                    //Using Linq, query the list of students, who have paid the bus fee and to the
                    //students who have not paid the bus fee, add Rs.100 / -to the amountToPay column.
                    //Store the list of students name who have not paid fees along with this new
                    //amountToPay and the contact number of both parents value in a new csv file.
                    //(Write 2 functions as solutions where each uses Lambda expressions or Sql queries to query)

                    var ListOfStudentBySqlQuery = from student in studentInfo
                                                  join bill in billInfo on student.StudentId equals bill.StudentId
                                                  where bill.AmountToPay != bill.FeesCollected
                                                  select new BillReport{
                                                      StudentId = student.StudentId,
                                                      AmountToPay = bill.AmountToPay - bill.FeesCollected,
                                                      FatherMobileNumber = student.FatherMobileNumber,
                                                      MotherMobileNumber = student.MotherMobileNumber
                                                  };

                    var StudentNotPaidBusFee = ListOfStudentBySqlQuery.ToList();

                    var ListOfStudentByLambdaExpresion = studentInfo.Join(billInfo, s => s.StudentId, bill => bill.StudentId, (s, bill) => new { s, bill })
                                                                     .Where(student => student.bill.AmountToPay != student.bill.FeesCollected)
                                                                     .Select(detail=>
                                                                            new BillReport 
                                                                            {       
                                                                                StudentId = detail.s.StudentId,
                                                                                AmountToPay = detail.bill.AmountToPay -  detail.bill.FeesCollected,
                                                                                FatherMobileNumber = detail.s.FatherMobileNumber,
                                                                                MotherMobileNumber = detail.s.MotherMobileNumber
                                                                            });

                    ListOfStudentByLambdaExpresion.ToList().ForEach(detail => Console.WriteLine(detail.StudentId));

                    string dir = @"C:\Datagrokr\csharp\LinqAssignments\LinqAssignments\Share\";
                    if (Directory.Exists(dir))
                    {
                        string fileName = "newBillReport.csv";
                        string delimiter = ",";
                        string[] header = { "StudentId", "Amount To Pay", "Father's Mobile Number", "Mother's Mobile Number" };                             
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(header.Aggregate((x, y) => x + delimiter + y));
                        foreach (var student in StudentNotPaidBusFee)
                        {
                           sb.AppendLine($"{student.StudentId},{student.AmountToPay+100},{student.FatherMobileNumber},{student.MotherMobileNumber}");
                            
                        }
                        File.WriteAllText(dir+fileName, sb.ToString());

                    }
                    else
                    {
                        Console.WriteLine("Directory doesn't exists");
                    }


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
