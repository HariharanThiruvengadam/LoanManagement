using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Model
{
    public class Customer
    {
        public int CustomerId { get; set; } = 0;
        public string CustomerName { get; set; } = string.Empty;

        public string Email {  get; set; } = string.Empty ;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal CreditScore { get; set; } =  decimal.Zero;


        public Customer() { }

        public Customer(int customerId, string customerName, string email, string phoneNumber, string address, decimal creditScore)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            CreditScore = creditScore;
        }
    }
}
