using LoanManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagement.Repository;

namespace LoanManagement.Service
{
    internal class LoanServices:ILoneServices
    {
        ILoanRepository loanRepo;
        public LoanServices() {
            loanRepo = new LoanRepository();
        }

        public void CreateCustomer()
        {
            Customer newCustomer = new Customer();
            Console.Write("Enter customer name: ");
            newCustomer.CustomerName = Console.ReadLine();
            Console.Write("Enter customer email id: ");
            newCustomer.Email = Console.ReadLine();
            Console.Write("Enter the phone number: ");
            newCustomer.PhoneNumber = Console.ReadLine();
            Console.Write("Enter the address: ");
            newCustomer.Address = Console.ReadLine();
            Console.Write("Enter the credit score: ");
            newCustomer.CreditScore = Convert.ToDecimal(Console.ReadLine());

            (bool status,int newId) = loanRepo.CreateCustomer(newCustomer);
            if(status)
            {
                Console.WriteLine($"Successfully created! Your customer id is: {newId}");
            }
            else
            {
                Console.WriteLine("Failed to create new customer");
            }

        }

        public void ApplyLoan()
        {


            Loan loan = new Loan();
            Console.Write("Enter the customer Id:");
            loan.CustomerId = int.Parse(Console.ReadLine());
            Console.Write("Enter the principal amount:");
            loan.PrincipalAmount = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Enter the interest rate");
            loan.InterestRate = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Enter the loan term in months");
            loan.LoanTerm = int.Parse(Console.ReadLine());
            Console.Write("Enter the loan type:(CarLoan/HomeLoan)");
            loan.LoanType = Console.ReadLine().ToLower();
            string PropertyAddress="";
            int propertyValue=0;
            string CarModel="";
            int CarValue=0;

            if (loan.LoanType.Equals("homeloan"))
            {
                Console.Write("Enter the property address:");
                PropertyAddress = Console.ReadLine();
                Console.Write("Enter the property value");
                propertyValue = int.Parse(Console.ReadLine());


            }else if (loan.LoanType.Equals("carloan"))
            {
              
                Console.Write("Enter the car model:");
                CarModel = Console.ReadLine();
                Console.Write("Enter the car value");
                CarValue = int.Parse(Console.ReadLine());
              
            }


            Console.WriteLine("Do you want to confirm the loan application? (Yes/No)");
            string confirmation = Console.ReadLine().ToLower();
            if (confirmation.Equals("yes"))
            {
                loan.LoanStatus = "Pending";
                bool status = loanRepo.ApplyLoan(loan,PropertyAddress,propertyValue,CarModel,CarValue);
                if (status)
                {
                    Console.WriteLine("Loan application stored in database with status pending.");
                }
                else
                {
                    Console.WriteLine("Failed to apply.");
                }
            }
            else
            {
                Console.WriteLine("Loan application cancelled.");
            }

        }

       public void CalculateInterest()
        {
            Console.Write("Enter the loan id:");
            int loanId = int.Parse(Console.ReadLine());


            decimal calculatedInterest = loanRepo.CalculateInterest(loanId);
            Console.WriteLine($"Calculated interest is{calculatedInterest}");

        }

       public void LoanStatus()
        {
            Console.Write("Enter the loan id:");
            int loanId = int.Parse(Console.ReadLine());
            string status = Console.ReadLine();
            Console.WriteLine($"Your current status: {status}");
        }

       public void CalculateEMI()
        {
            Console.Write("Enter the loan id:");
            int loanId = int.Parse(Console.ReadLine());
            decimal calculatedEMI = loanRepo.CalculateEMI(loanId);
            Console.WriteLine($"Calculated interest is{calculatedEMI}");
        }

       public void LoanRepayment()
        {
            Console.Write("Enter the loan id:");
            int loanId = int.Parse(Console.ReadLine());
            Console.Write("Enter the amount:");
            decimal amount = decimal.Parse(Console.ReadLine());
            bool status = loanRepo.LoanRepayment(loanId, amount);   
        }

       public void GetAllLoan()
        {
            List<Loan> loans = new List<Loan>();

            loans = loanRepo.GetAllLoan();
            foreach (var loan in loans)
            {
                Console.Write($"Loan Id: {loan.LoanId},\n Customer id:{loan.CustomerId},\n Loan type:{loan.LoanType},\n Principle Amount: {loan.PrincipalAmount},\n Loan Status :{loan.LoanStatus}\n______________\n");
            }
        }

      public  void GetAllLoanById()
        {
            List<Loan> loans = new List<Loan>();
            Console.Write("Enter the loan id:");
            int loanId = int.Parse(Console.ReadLine());
            loans = loanRepo.GetAllLoanById(loanId);
            foreach (var loan in loans)
            {
                Console.Write($"Loan Id: {loan.LoanId},\n Customer id:{loan.CustomerId},\n Loan type:{loan.LoanType},\n Principle Amount: {loan.PrincipalAmount},\n Loan Status :{loan.LoanStatus}\n______________\n");
            }
        }
    }
}
