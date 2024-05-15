using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Exception
{
    internal class InvalidLoanException:IOException
    {
        public InvalidLoanException(string? message):base(message)
        { 
        }
    }
}
