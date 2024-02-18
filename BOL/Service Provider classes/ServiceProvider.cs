using System;

namespace BOL
{
    public class ServiceProvider
    {
        // Properties
        public int ServiceProviderID { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Skills { get; set; }  
        public string Wages { get; set; }
        public string Address { get; set; }

        // Constructors
        public ServiceProvider() { 

        }

        public ServiceProvider(int serviceProviderID, string nameFirst, string nameLast, string username, string password, string phoneNumber, string skills, string wages, string address)
        {
            ServiceProviderID = serviceProviderID;
            NameFirst = nameFirst;
            NameLast = nameLast;
            Username = username;
            Password = password;
            PhoneNumber = phoneNumber;
            Skills = skills;
            Wages = wages;
            Address = address;
        }
    }
}
