using System;
using System.Collections.Generic;

namespace SDE_Assessment
{
    public class Employees
    {
        readonly Dictionary<string, List<string>> juniours = new Dictionary<string, List<string>>();
        private List<Employee> employees = new List<Employee>();
        public List<Employee> employeeList => employees;

        /// <summary>
        /// Constructor Takes Csv Data
        /// </summary>
        /// <param name="data">Raw Data captured from Csv</param>
        public Employees(String[] data)
        {
            ProcessCsvData(data);

            foreach (var employee in employees)
            {
                AddEmployee(employee.Manager, employee.Id);
            }
        }

        /// <summary>
        /// Checks if the Data is well formed if not well formed list of employees is zero
        /// </summary>
        /// <param name="data"></param>
        public void ProcessCsvData(string[] data)
        {
            int managers = 0;

            foreach (var line in data)
            {
                try
                {
                    var parts = line.Split(',');
                    var employee = new Employee();
                    employee.Id = parts[0];
                    if (parts[1].Equals(""))
                    {
                        employee.Manager = "";
                        managers++;
                        if (managers > 1)
                        {
                            throw new ManagerException("Wueh! Employer has more than one manager");
                        }
                    }
                    else employee.Manager = parts[1];

                    long salary;
                    var isValid = Int64.TryParse(parts[2], out salary);
                    if (isValid)
                    {
                        if (salary > 0) employee.Salary = salary;                        
                        else throw new SalaryException("Wueh! Negative salary found");
                    }
                    else throw new SalaryException("Wueh! Invalid salary found");
                    employees.Add(employee);
                }
                catch (ManagerException ex)
                {
                    employees.Clear();
                    Console.WriteLine(ex.Message);
                    return;
                }
                catch (SalaryException ex)
                {
                    employees.Clear();
                    Console.WriteLine(ex.Message);
                    return;
                }
            }

            //Verify there is a manager
            if (managers != 1)
            {
                Console.WriteLine("No Manager identified, Check again");
                employees.Clear();
            }
        }

        /// <summary>
        /// Adds an employee id into the graph
        /// </summary>
        /// <param name="employeeId">Employee Id</param>
        public void AddEmployee(string employeeId)
        {
            //if Employee ID exists do nothing
            if (juniours.ContainsKey(employeeId))
            {
                return;
            }

            juniours.Add(employeeId, new List<string>());
        }

        /// <summary>
        /// Adds a Junior employee to a list of all junior staff reporting to the senior staff
        /// </summary>
        /// <param name="manager">Senior Staff</param>
        /// <param name="employeeId">Junior Staff</param>
        public void AddEmployee(string manager, string employeeId)
        {
            AddEmployee(manager);
            AddEmployee(employeeId);
            juniours[manager].Add(employeeId);
        }

        /// <summary>
        /// returns a list of all junior staff under the senior Staff
        /// </summary>
        /// <param name="empId">ID of the Senior Staff</param>
        /// <returns>List of all Junior Staffs</returns>
        public List<String> FetchJuniours(String empId)
        {
            return juniours[empId];
        }

        /// <summary>
        /// Given a senior staff calculate all the salary of junior staff below.
        /// This method uses Depth Transversal as the algorithim to find all the junior staffs and their salary
        /// </summary>
        /// <param name="root">Senior Staff ID</param>
        /// <returns>Salary</returns>
        public long FetchSalaryBudget(String root)
        {
            long salary = 0;
            HashSet<String> visited = new HashSet<String>();
            Stack<String> stack = new Stack<String>();
            stack.Push(root);
            while (stack.Count != 0)
            {
                String empId = stack.Pop();
                if (!visited.Contains(empId))
                {
                    visited.Add(empId);
                    foreach (String v in FetchJuniours(empId))
                    {
                        stack.Push(v);
                    }
                }
            }

            if (visited.Count == 0) return salary;
            foreach (var id in visited)
            {
                salary += LookUp(id).Salary;
            }

            return salary;
        }

        /// <summary>
        /// 
        /// Given an Id returns the employee details
        /// </summary>
        /// <param name="id">employee ID to search</param>
        /// <returns>Employee Details</returns>
        public Employee LookUp(string id)
        {
            foreach (Employee employee in employees)
            {
                if (employee.Id.Equals(id))
                {
                    return employee;
                }
            }

            return null;
        }

        class ManagerException : Exception
        {
            public ManagerException(string message) : base(message)
            {
            }
        }

        class SalaryException : Exception
        {
            public SalaryException(string message) : base(message)
            {
            }
        }

    }
}
