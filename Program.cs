using System;
using System.Linq;
using System.Collections.Generic;

public class Program
{
    IList<Employee> employeeList;
    IList<Salary> salaryList;

    public Program()
    {
        employeeList = new List<Employee>() {
            new Employee(){ EmployeeID = 1, EmployeeFirstName = "Rajiv", EmployeeLastName = "Desai", Age = 49},
            new Employee(){ EmployeeID = 2, EmployeeFirstName = "Karan", EmployeeLastName = "Patel", Age = 32},
            new Employee(){ EmployeeID = 3, EmployeeFirstName = "Sujit", EmployeeLastName = "Dixit", Age = 28},
            new Employee(){ EmployeeID = 4, EmployeeFirstName = "Mahendra", EmployeeLastName = "Suri", Age = 26},
            new Employee(){ EmployeeID = 5, EmployeeFirstName = "Divya", EmployeeLastName = "Das", Age = 20},
            new Employee(){ EmployeeID = 6, EmployeeFirstName = "Ridhi", EmployeeLastName = "Shah", Age = 60},
            new Employee(){ EmployeeID = 7, EmployeeFirstName = "Dimple", EmployeeLastName = "Bhatt", Age = 53}
        };

        salaryList = new List<Salary>() {
            new Salary(){ EmployeeID = 1, Amount = 1000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 1, Amount = 500, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 1, Amount = 100, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 2, Amount = 3000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 2, Amount = 1000, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 3, Amount = 1500, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 4, Amount = 2100, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 5, Amount = 2800, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 5, Amount = 600, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 5, Amount = 500, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 6, Amount = 3000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 6, Amount = 400, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 7, Amount = 4700, Type = SalaryType.Monthly}
        };
    }

    public static void Main()
    {
        Program program = new Program();

        program.Task1();

        program.Task2();

        program.Task3();
    }

    public void Task1()
    {
       
        var totalSalaries = from emp in employeeList                // Iterate through each employee in the employeeList
                            join sal in salaryList on emp.EmployeeID equals sal.EmployeeID // Join with salaryList based on EmployeeID
                            group sal.Amount by new { emp.EmployeeFirstName, emp.EmployeeLastName } into g // Group by employee's first and last name
                            orderby g.Sum()                           // Order groups by sum of salaries
                            select new                                // Select new anonymous type
                            {
                                Name = $"{g.Key.EmployeeFirstName} {g.Key.EmployeeLastName}", // Concatenate first and last name
                                TotalSalary = g.Sum()                  // Calculate total salary for the group
                            };


        Console.WriteLine("Total Salary of all employees with their corresponding names in ascending order of salary:");
                            foreach (var item in totalSalaries)
                            {
                                Console.WriteLine($"Name: {item.Name}, Total Salary: {item.TotalSalary}");
                            }
                            Console.WriteLine();
                            Console.WriteLine("-----------------------------------------------------------------------------------------");
                            Console.WriteLine();
    }

    public void Task2()
    {
                        var secondOldestEmployee = (from emp in employeeList
                                    orderby emp.Age descending
                                    select emp).Skip(1).FirstOrDefault();

        if (secondOldestEmployee != null)
        {
            var totalMonthlySalary = (from sal in salaryList                              // Iterate through each salary in the salaryList
                                      where sal.EmployeeID == secondOldestEmployee.EmployeeID // Filter salaries where EmployeeID matches the second oldest employee's EmployeeID
                                         && sal.Type == SalaryType.Monthly               // Filter salaries of type Monthly
                                      select sal.Amount)                                 // Select the Amount of each salary
                          .Sum();                                            // Calculate the sum of all selected amounts


            Console.WriteLine($"Employee details of 2nd oldest employee ({secondOldestEmployee.EmployeeFirstName} {secondOldestEmployee.EmployeeLastName}) including total monthly salary:");
            Console.WriteLine($"Name: {secondOldestEmployee.EmployeeFirstName} {secondOldestEmployee.EmployeeLastName}, Age: {secondOldestEmployee.Age}, Total Monthly Salary: {totalMonthlySalary}");
        }
        Console.WriteLine();
        Console.WriteLine("----------------------------------------------------------------------------------------");
        Console.WriteLine();
    }

    public void Task3()
    {
        var meanSalaries = from emp in employeeList                    // Iterate through each employee in the employeeList
                           where emp.Age > 30                          // Filter employees whose age is greater than 30
                           join sal in salaryList on emp.EmployeeID equals sal.EmployeeID // Join with salaryList based on EmployeeID
                           group sal.Amount by new { Type = sal.Type } into g // Group salaries by the salary type (Monthly, Performance, or Bonus)
                           select new                                 // Select new anonymous type
                           {
                               SalaryType = g.Key.Type,               // Store the salary type
                               MeanSalary = g.Average()                // Calculate the average salary for each group
                           };


        Console.WriteLine("Task 3: Mean of Monthly, Performance, and Bonus salary of employees whose age is greater than 30:");
        foreach (var item in meanSalaries)
        {
            Console.WriteLine($"Salary Type: {item.SalaryType}, Mean Salary: {item.MeanSalary}");
        }
    }
}

public enum SalaryType
{
    Monthly,
    Performance,
    Bonus
}

public class Employee
{
    public int EmployeeID { get; set; }
    public string EmployeeFirstName { get; set; }
    public string EmployeeLastName { get; set; }
    public int Age { get; set; }
}

public class Salary
{
    public int EmployeeID { get; set; }
    public int Amount { get; set; }
    public SalaryType Type { get; set; }
}
