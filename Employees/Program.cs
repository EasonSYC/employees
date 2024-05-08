namespace Employees;

class Program
{
    static void Main()
    {
        Employee[] employeeArray = new Employee[8];
        string[] fileInfo = File.ReadAllLines("Employees.txt");

        int employeeCount = 0;

        int numberCount = 0;
        string[] tempInfos = new string[3];

        foreach(string str in fileInfo)
        {
            if (char.IsLetter(str[0])) // we find a job title, a whole employee is found
            {
                decimal hourlyPay = decimal.Parse(tempInfos[0]);
                string employeeNumber = tempInfos[1];
                string jobTitle = str;

                if (numberCount == 2) // an employee
                {
                    employeeArray[employeeCount++] = new Employee(hourlyPay, employeeNumber, jobTitle);
                }
                else // a manager
                {
                    decimal bonusValue = decimal.Parse(tempInfos[2]);
                    employeeArray[employeeCount++] = new Manager(bonusValue, hourlyPay, employeeNumber, jobTitle);
                }

                numberCount = 0;
                tempInfos = new string[3];
            }
            else
            {
                tempInfos[numberCount++] = str;
            }
        }

        EnterHours(employeeArray);

        foreach(Employee e in employeeArray)
        {
            Console.WriteLine($"{e.GetEmployeeNumber()} {e.GetTotalPay()}");
        }
    }

    static void EnterHours(Employee[] employeeArray)
    {
        string[] fileInfo = File.ReadAllLines("HoursWeek1.txt");
        int lineNum = fileInfo.Length;
        
        for(int i = 0; i < lineNum; i += 2)
        {
            string employeeNumber = fileInfo[i];
            decimal numberOfHours = decimal.Parse(fileInfo[i + 1]);

            foreach(Employee e in employeeArray)
            {
                if(e.GetEmployeeNumber() == employeeNumber)
                {
                    e.SetPay(1, numberOfHours);
                }
            }
        }
    }
}

public class Employee
{
    private readonly decimal _hourlyPay;
    private readonly string _employeeNumber;
    private readonly string _jobTitle;
    private readonly decimal[] _payYear2022; // array of index 0 ~ 51

    public Employee(decimal hourlyPay, string employeeNumber, string jobTitle)
    {
        _hourlyPay = hourlyPay;
        _employeeNumber = employeeNumber;
        _jobTitle = jobTitle;
        _payYear2022 = new decimal[51];
    }

    public string GetEmployeeNumber()
    {
        return _employeeNumber;
    }

    public virtual void SetPay(int weekNumber, decimal numberOfHours)
        // virtual for future override
    {
        _payYear2022[weekNumber - 1] = _hourlyPay * numberOfHours;
    }

    public decimal GetTotalPay()
    {
        decimal s = 0;
        foreach (decimal i in _payYear2022)
        {
            s += i;
        }

        return s;
    }
}

public class Manager : Employee
{
    private readonly decimal _bonusValue;

    public Manager(decimal bonusValue, decimal hourlyPay, string employeeNumber, string jobTitle) : base(hourlyPay, employeeNumber, jobTitle)
    {
        _bonusValue = bonusValue;
    }

    public override void SetPay(int weekNumber, decimal numberOfHours)
    {
        base.SetPay(weekNumber, numberOfHours * (1 + _bonusValue / 100));
    }
}