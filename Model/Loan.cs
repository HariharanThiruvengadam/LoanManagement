using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Model
{
    public class Loan
    {
        public int LoanId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public decimal PrincipalAmount { get; set; }  = decimal.Zero;

        public decimal InterestRate { get; set; } = decimal.Zero;

        public int LoanTerm { get; set; } = 0;

        public string LoanType { get; set; } = string.Empty;

        public string LoanStatus { get; set; } = string.Empty;

       public Loan() { }

        public Loan(int loanId,int customerId, decimal principalAmount, decimal interestRate, int loanTerm , string loanType, string loanStatus)
        {
            LoanId = loanId;
            CustomerId = customerId;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = loanStatus;
        }
    }
}
