using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagement.Model;

namespace LoanManagement.Repository
{
    public interface ILoanRepository
    {

        (bool,int) CreateCustomer(Customer customer);
        bool ApplyLoan(Loan loan,string propertyAddress, int propertyValue,string carModel,int carValue);

        decimal CalculateInterest(int loanid);

        string LoanStatus(int loanId);

       decimal CalculateEMI (int loanId);

        bool LoanRepayment(int loanId, decimal amount);

        List<Loan> GetAllLoan();

        List<Loan> GetAllLoanById(int loanId);

    }
}
