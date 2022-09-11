using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SDE_Assessment
{
    [TestClass]
    public class UnitTests
    {
        private Employees employees;

        [TestInitialize]
        public void TestInitiliaze()
        {
            var data = GetData("test_data/test_data1.csv");
            employees = new Employees(data);
        }

        /// <summary>
        /// Tests if the Employees are added to the graph
        /// </summary>
        [TestMethod]
        public void TestIfEmployeesAdded()
        {
            Assert.IsTrue(
                employees.employeeList.Contains(
                    new Employee { 
                        Id = "Employee2", 
                        Manager = "Employee1", 
                        Salary = 800 
                    }
                )
            );
            Assert.IsTrue(
                employees.employeeList.Contains(
                    new Employee { 
                        Id = "Employee4", 
                        Manager = "Employee2", 
                        Salary = 500 
                    }
                )
            );
        }

        /// <summary>
        /// As per the test data employee 5 has no juniours
        /// </summary>
        [TestMethod]
        public void TestEmployeeHasNoJuniours()
        {
            var juniours = employees.FetchJuniours("Employee5");
            Assert.AreEqual(0, juniours.Count);
        }

        /// <summary>
        /// Tests if Employee have juniours added
        /// </summary>
        [TestMethod]
        public void TestJuniours()
        {
            var juniours = employees.FetchJuniours("Employee2");
            Assert.AreEqual(2, juniours.Count);
        }

        /// <summary>
        /// Tests if the Lookup function returns a Employee given a valid Employee ID
        /// </summary>
        [TestMethod]
        public void TestLookUp()
        {
            Employee employee = employees.LookUp("Employee1");
            Assert.IsNotNull(employee);
        }

        /// <summary>
        /// Tests if lookup returns null on non existence id
        /// </summary>
        [TestMethod]
        public void TestLookupOfWrongEmployeeId()
        {
            Employee employee = employees.LookUp("Employee10");
            Assert.IsNull(employee);
        }

        string[] GetData(String path)
        {
            return File.ReadAllLines(path);
        }

        /// <summary>
        /// Tests for the correct budget
        /// </summary>
        [TestMethod]
        public void TestSalaryBudget()
        {
            Assert.AreEqual(1800, employees.FetchSalaryBudget("Employee2"));
            Assert.AreEqual(500, employees.FetchSalaryBudget("Employee3"));
            Assert.AreEqual(3800, employees.FetchSalaryBudget("Employee1"));
        }

        /// <summary>
        /// Test for existence of managers
        /// </summary>
        [TestMethod]
        public void TestManager()
        {
            Employees employees = new Employees(
                GetData("test_data/test_data5.csv")
            );
            Assert.AreEqual(0, employees.employeeList.Count);
        }

        /// <summary>
        /// Test for the number of managers
        /// </summary>
        [TestMethod]
        public void TestManagersCount()
        {
            Employees employees = new Employees(
                GetData("test_data/test_data3.csv")
            );
            Assert.IsFalse(
                employees.employeeList.Contains(
                    new Employee { Id = "Employee5" }
                )
            );
            Assert.IsFalse(
                employees.employeeList.Contains(
                    new Employee { Id = "Employee1" }
                )
            );
            Assert.AreEqual(0, employees.employeeList.Count);
        }

        /// <summary>
        /// Test for negative salary
        /// </summary>
        [TestMethod]
        public void TestNegativeSalary()
        {
            Employees employees = new Employees(
                GetData("test_data/test_data4.csv")
            );
            Assert.IsFalse(
                employees.employeeList.Contains(
                    new Employee { Id = "Employee5" }
                )
            );
            Assert.AreEqual(0, employees.employeeList.Count);
        }

        /// <summary>
        /// Test for invalid salary
        /// </summary>
        [TestMethod]
        public void TestInvalidSalary()
        {
            Employees employees = new Employees(GetData("test_data/test_data2.csv"));
            Assert.IsFalse(
                employees.employeeList.Contains(
                    new Employee { Id = "Employee5" }
                )
            );
            Assert.IsFalse(
                employees.employeeList.Contains(
                    new Employee { Id = "Employee2" }
                )
            );
            Assert.AreEqual(0, employees.employeeList.Count);
        }

    }
}