using System;
using System.IO;
using System.Linq;
namespace LinqAssignments
{
    class ConvertIntToFloat
    {
        //Using Linq, read a local csv file and convert the integer values to float.
        static void Main(string[] args)
        {   
            var filePath = @"C:\Datagrokr\csharp\LinqAssignments\LinqAssignments\local.csv";
            
            if (File.Exists(filePath))
            {
                try
                {       

                    using (StreamReader reader = new StreamReader(File.OpenRead(filePath)))
                    {
                        var csv = reader.ReadToEnd()
                                        .Split('\n').Skip(1)
                                        .SelectMany(x => x.Split(',').Skip(1))
                                        .Select(i=>(float)int.Parse(i))
                                        .ToList();
                        
                       foreach(var i in csv)
                       {
                            Console.WriteLine(i);
                       }

                    }
                   
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("File doesn't exist");
            }
            Console.ReadLine();

        }

       
    }
}
