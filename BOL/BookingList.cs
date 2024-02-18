namespace BOL;
    public class BookingList
    {
        public int UserID { get; set; }
        public string Username{get;set;}
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string PhoneNumber { get; set; }
        public string Skills { get; set; }
        public string Wages { get; set; }
        public float Rating { get; set; }

        public BookingList()
        {
            // Default constructor
        }

        public BookingList(int userId,string username,string firstName, string lastName, string phoneNumber, string skills, string wages, float ratings)
        {
            UserID = userId;
            Username=username;
            NameFirst = firstName;
            NameLast = lastName;
            PhoneNumber = phoneNumber;
            Skills = skills;
            Wages = wages;
            Rating = ratings;
        }
        public override string ToString()
        {
            return $"UserID: {UserID}, Username: {Username}, NameFirst: {NameFirst}, NameLast: {NameLast}, PhoneNumber: {PhoneNumber}, Skills: {Skills}, Wages: {Wages}, Rating: {Rating}";
        }

    }

