using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Model
{
    public class HomeLoan:Loan
    {

        public string PropertyAddress { get; set; } = string.Empty;
        public int PropertyValue { get; set; } = 0;

        public HomeLoan() { }

        public HomeLoan(string propertyAddress, int propertyValue)
        {
            PropertyAddress= propertyAddress;
            PropertyValue= propertyValue;
        }
    }
}
