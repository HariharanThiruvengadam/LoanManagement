using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Model
{
    public class CarLoan:Loan
    {
        public string CarModel { get; set; } = string.Empty;
        public int CarValue { get; set; } = 0;
        public CarLoan() { }

        public CarLoan(string carModel, int carValue)
        {
            CarModel = carModel;
            CarValue = carValue;
        }

    }
}
