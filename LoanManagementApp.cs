using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagement.Service;
namespace LoanManagement
{
    internal class LoanManagementApp
    {
        ILoneServices loanServices;
        public LoanManagementApp()
        {
            loanServices = new LoanServices();
        }

        public void Menu()
        {
            while (true)
            {
                Console.WriteLine("WELCOME TO LOAN MANAGEMENT APPLICATION \n Here are the options from which you can choose to perform");
                Console.WriteLine("1.Create Customer");
                Console.WriteLine("2.Apply loan");
                Console.WriteLine("3.Calculate Interest");
                Console.WriteLine("4.Loan Staus");
                Console.WriteLine("5.Calculate EMI");
                Console.WriteLine("6.Loan Repayment");
                Console.WriteLine("7.Get all loans");
                Console.WriteLine("8.Get all loans by specific customer id");
                Console.Write("9.EXIT");
                Console.Write("Select an Option: ");
                int userOption = int.Parse(Console.ReadLine());
                switch (userOption)
                {
                    case 1:
                        loanServices.CreateCustomer();
                        break;
                    case 2:
                        loanServices.ApplyLoan();
                        break;
                    case 3:
                        loanServices.CalculateInterest();
                        break;
                    case 4:
                        loanServices.LoanStatus();
                        break;
                    case 5:
                        loanServices.CalculateEMI();
                        break;
                    case 6:
                        loanServices.LoanRepayment();
                        break;
                    case 7:
                        loanServices.GetAllLoan();
                        break;
                    case 8:
                        loanServices.GetAllLoanById();
                        break;
                    case 9:
                        Console.WriteLine("Thank you!!!");
                        break;
                }

                if (userOption == 9)
                {
                    break;
                }
                else if (userOption > 10)
                {
                    Console.WriteLine("Choose the correct given option");
                }
            }
        }
    }
}
