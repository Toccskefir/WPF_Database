using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Database
{
    public class EmployeeService
    {
        MySqlConnection connection;
        public EmployeeService()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.Port = 3306;
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "dolgozok";
            connection = new MySqlConnection(builder.ConnectionString);
        }

        //CRUD

        public List<Employee> GetAll()
        {
            List<Employee> employees = new List<Employee>();

            OpenConnection();
            string sql = "SELECT * FROM dolgozok";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.Id = reader.GetInt32("id");
                    employee.Name = reader.GetString("nev");
                    employee.Gender = reader.GetString("nem");
                    employee.Age = reader.GetInt32("kor");
                    employee.Salary = reader.GetInt32("fizetes");
                    employees.Add(employee);
                }
            }
            CloseConnection();

            return employees;
        }

        public bool Create(Employee employee)
        {
            OpenConnection();
            string sql = "INSERT INTO dolgozok (nev, nem, kor, fizetes) VALUES (@name, @gender, @age, @salary)";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddWithValue("@name", employee.Name);
            command.Parameters.AddWithValue("@gender", employee.Gender);
            command.Parameters.AddWithValue("@age", employee.Age);
            command.Parameters.AddWithValue("@salary", employee.Salary);

            int affectedRows = command.ExecuteNonQuery();

            CloseConnection();
            //INSERT utasítás akkor sikeres ha legalább 1 sort érintett
            return affectedRows == 1;
        }

        public bool Update(int id, Employee newEmployee)
        {
            OpenConnection();
            string sql = "UPDATE dolgozok SET nev = @name, nem = @gender, kor = @age, fizetes = @salary WHERE id = @id";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddWithValue("@name", newEmployee.Name);
            command.Parameters.AddWithValue("@gender", newEmployee.Gender);
            command.Parameters.AddWithValue("@age", newEmployee.Age);
            command.Parameters.AddWithValue("@salary", newEmployee.Salary);
            command.Parameters.AddWithValue("@id", id);

            int affectedRows = command.ExecuteNonQuery();

            CloseConnection();

            return affectedRows == 1;
        }

        public bool Delete(int id)
        {
            OpenConnection();
            string sql = "DELETE FROM dolgozok WHERE id = @id";
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddWithValue("@id", id);

            int affectedRows = command.ExecuteNonQuery();

            CloseConnection();

            return affectedRows == 1;
        }

        private void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }
    }
}
