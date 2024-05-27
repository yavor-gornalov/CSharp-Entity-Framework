using SoftUni.Data;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new();

            //Test SoftUniContext
            var firstEmpolyee = context.Employees.Find(1);

            Console.WriteLine(firstEmpolyee!.FirstName + ' ' + firstEmpolyee.LastName);
        }
    }
}
