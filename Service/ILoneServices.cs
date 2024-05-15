using LoanManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Service
{
    internal interface ILoneServices
    {

       void CreateCustomer();
        void ApplyLoan();

        void CalculateInterest();

        void LoanStatus();

        void CalculateEMI();

        void LoanRepayment();

        void GetAllLoan();

        void GetAllLoanById();
    }
}
