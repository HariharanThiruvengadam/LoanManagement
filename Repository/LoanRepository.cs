
using LoanManagement.Model;
using LoanManagement.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LoanManagement.Repository
{
    public class LoanRepository:ILoanRepository
    {

        SqlConnection sqlConnection;
        SqlCommand cmd;

        public LoanRepository() { 
            sqlConnection = new SqlConnection(ConnectionUtil.GetConnectionString());
         cmd = new SqlCommand();
        }
        
        public (bool,int) CreateCustomer(Customer customer)
        {
            cmd.Parameters.Clear();
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            cmd.CommandText = "Insert into Customer OUTPUT INSERTED.customerId values(@name,@email,@phoneNumber,@address,@creditScore)";
            cmd.Parameters.AddWithValue("@name", customer.CustomerName);
            cmd.Parameters.AddWithValue("@email", customer.Email);
            cmd.Parameters.AddWithValue("@address", customer.Address);
            cmd.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
            cmd.Parameters.AddWithValue("@creditScore", customer.CreditScore);
            int newId = (int) cmd.ExecuteScalar();
            sqlConnection.Close();
            return (newId>0,newId);
        }
       public bool ApplyLoan(Loan loan,string propertyAddress,int propertyValue, string carModel,int carValue)
        {
            cmd.Parameters.Clear();
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            cmd.CommandText = "INSERT INTO Loan VALUES (@CustomerId, @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus,@propertyAddress,@propertyValue,@carModel,@carValue)";
            cmd.Parameters.AddWithValue("@CustomerId", loan.CustomerId);
            cmd.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
            cmd.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
            cmd.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
            cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
            cmd.Parameters.AddWithValue("@LoanStatus", loan.LoanStatus);
            cmd.Parameters.AddWithValue("@propertyAddress", propertyAddress);
            cmd.Parameters.AddWithValue("@propertyValue", propertyValue);
            cmd.Parameters.AddWithValue("@carModel", carModel);
            cmd.Parameters.AddWithValue("@carValue", carValue);
            int status = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return status>0?true:false;
        }

        public decimal CalculateInterest(int loanid)
        {
            cmd.Parameters.Clear();
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            cmd.CommandText = "select InterestRate,PrincipalAmount,LoanTerm from loan where loanId = @loanId";

            cmd.Parameters.AddWithValue("@loanId", loanid);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                decimal Rate = (decimal)reader["InterestRate"];
                decimal Principal = (decimal)reader["PrincipalAmount"];
                int term = (int)reader["LoanTerm"];

                decimal interest = (Rate * Principal * term)/12;

                return interest;
             }
            
            sqlConnection.Close();
            return 0.00m;
        }

        public string LoanStatus(int loanId)
        {
            return "pending";
        }

        public decimal CalculateEMI(int loanId)
        {
            return 5.55m;
        }

        public bool LoanRepayment(int loanId, decimal amount)
        {
            return true;
        }

        public List<Loan> GetAllLoan()
        {
            List<Loan> loans = new List<Loan>();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "SELECT * FROM Loan";
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Loan loan = new Loan
                {
                    LoanId = (int)reader["LoanId"],
                    PrincipalAmount = (decimal)reader["PrincipalAmount"],
                    InterestRate = (decimal)reader["InterestRate"],
                    LoanTerm = (int)reader["LoanTerm"],
                    LoanType = reader["LoanType"].ToString(),
                    LoanStatus = reader["LoanStatus"].ToString()
                };
                loans.Add(loan);
            }
            sqlConnection.Close();

            return loans;
        }

        public List<Loan> GetAllLoanById(int loanId)
        {
            List<Loan> loans = new List<Loan>();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "SELECT * FROM Loan where loanId=@loanId";
            cmd.Parameters.AddWithValue("@loanId", loanId);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Loan loan = new Loan
                {
                    LoanId = (int)reader["LoanId"],
                    PrincipalAmount = (decimal)reader["PrincipalAmount"],
                    InterestRate = (decimal)reader["InterestRate"],
                    LoanTerm = (int)reader["LoanTerm"],
                    LoanType = reader["LoanType"].ToString(),
                    LoanStatus = reader["LoanStatus"].ToString()
                };
                loans.Add(loan);
            }
            sqlConnection.Close();

            return loans;
        }
    }
}
