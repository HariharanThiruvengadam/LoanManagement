
using LoanManagement.Exception;
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
           

            try
            {
                cmd.Parameters.Clear();
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                string status = "";
                cmd.CommandText = "Select COUNT(*) from loan where loanId = @LoanId";
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                int isExist = (int)cmd.ExecuteScalar();

                if (isExist == 0)
                {
                    throw new InvalidLoanException("Loan Id is not found!");
                }

                cmd.CommandText = "Select CreditScore from Customer c join loan l on c.customerId = l.customerId where loanId = @LoanId";
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                SqlDataReader reader = cmd.ExecuteReader();
                decimal creditScore = 0;
                
               
                creditScore = Convert.ToDecimal(reader["CreditScore"]);
                
                 if (creditScore > 650)
                    {
                       status ="Approved";
                        cmd.CommandText = "UPDATE Loan SET LoanStatus = 'Approved' WHERE loanId = @LoanId";
                       cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        status= "Rejected";
                        cmd.CommandText = "UPDATE Loans SET LoanStatus = 'Rejected' WHERE loanId = @LoanId";
                        cmd.ExecuteNonQuery();
                }

                return status;
            }
            catch(InvalidLoanException ilex) 
            {
                Console.WriteLine(ilex.Message);
            }
          
            sqlConnection.Close();
            return "pending";
        }

        public double CalculateEMI(int loanId)
        {
            cmd.Parameters.Clear();
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            cmd.CommandText = "select InterestRate,PrincipalAmount,LoanTerm from loan where loanId = @loanId";

            cmd.Parameters.AddWithValue("@loanId", loanId);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                double Rate = (Convert.ToDouble(reader["InterestRate"])/12/100);
                double Principal = Convert.ToDouble(reader["PrincipalAmount"]);
                int term = (int)reader["LoanTerm"];

                double interest = Principal * Rate * Math.Pow((1 + Rate),term)/(Math.Pow((double)(1 + Rate), term) - 1);

                return interest;
            }

            sqlConnection.Close();
            return 0.00;
        }

        public bool LoanRepayment(int loanId, decimal amount)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                string status = "";
                cmd.CommandText = "Select COUNT(*) from loan where loanId = @LoanId";
                cmd.Parameters.AddWithValue("@LoanId", loanId);
                int isExist = (int)cmd.ExecuteScalar();

                if (isExist == 0)
                {
                    throw new InvalidLoanException("Loan Id is not found!");
                }

                double emi =CalculateEMI(loanId);
                if (Convert.ToDouble(amount) < emi)
                {
                    Console.WriteLine("Payment rejected. Amount less than the EMI amount.");
                    return false;
                }
                int noOfEmis = (int)(Convert.ToDouble(amount) / emi);
                double totalPaid = noOfEmis * emi;

                cmd.CommandText = "UPDATE Loans SET PrincipalAmount = PrincipalAmount - @TotalPaid WHERE LoanId = @LoanId";
                cmd.Parameters.AddWithValue("@TotalPaid", totalPaid);
                cmd.ExecuteNonQuery();

                amount= Convert.ToDecimal(Convert.ToDouble(amount) - totalPaid);

                if (amount <= 0)
                {
                    Console.WriteLine("Loan fully paid.");
                }
                else
                {
                    Console.WriteLine($"EMIs paid: {noOfEmis}. Remaining Principal: {amount}");
                }

            }
            catch (InvalidLoanException ilex)
            {
                Console.WriteLine(ilex.Message);
            }

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

        public List<Loan> GetAllLoanById(int customerId)
        {
            List<Loan> loans = new List<Loan>();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "SELECT * FROM Loan where customerId=@customerid";
            cmd.Parameters.AddWithValue("@customeroid", customerId);
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
