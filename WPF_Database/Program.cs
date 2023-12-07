using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Database
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            EmployeeService employeeService = new EmployeeService();
            if (args.Contains("--count"))
            {
                Console.WriteLine("A dolgozók száma: " + employeeService.GetAll().Count);
            }
            else
            {
                Application application = new Application();
                application.Run(new MainWindow(employeeService));
            }
        }
    }
}
