namespace Pal.Core.Enums.Account
{
    public enum PalClaimType : int
    {
        UserType,
        CompanyId,
        CustomerId,
        SalesId,
        AdvisorId,
        LawyerId,
        EmployeeId,
        Password
      
    }
    public enum UserType : int
    {
        Admins = 0,
        Companies = 1,
        RealStateAgents = 2,
        Customers = 3,
        Adviser = 4,
        Unknown = 5,
        Lawyer = 6,
        Sales= 7,
   
    }
    
    public enum ReelStatus : int
    {
        Pending = 0,
        Accepted = 1,
        NotAccepted = 2,
       
    }
}
