using System.Reflection.Metadata.Ecma335;
using BOL;
namespace DAL;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
public class DBManager{
    
    public static string connString=@"server=roundhouse.proxy.rlwy.net;port=13719;user=root;password=a4hE3BhD4dA4CH65e3CbghE1g4gAdB13;database=railway";
    public static User IsUserPresent(string username, string password){
            try
            {
                
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    // Prepare the SQL query to fetch the user details
                    string query = @"SELECT UserID, NameFirst, NameLast, Username, Password, PhoneNumber, Address 
                                    FROM Users 
                                    WHERE Username = @Username AND Password = @Password";

                    

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameters to the command to prevent SQL injection
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        
                        // Execute the command to fetch the user details
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if a user with the provided credentials exists
                            if (reader.Read())
                            {
                                
                                // User found, create a new User object and populate its properties
                                User user = new User
                                {
                                    UserID = reader.GetInt32("UserID"),
                                    NameFirst = reader.GetString("NameFirst"),
                                    NameLast = reader.GetString("NameLast"),
                                    Username = reader.GetString("Username"),
                                    Password = reader.GetString("Password"),
                                    PhoneNumber = reader.GetString("PhoneNumber"),
                                    Address = reader.GetString("Address")
                                };
                                
                                return user;
                            }
                            else
                            {
                                // User not found
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the database operation
                Console.WriteLine("Error validating service provider login: " + ex.Message);
                return null; // Return null in case of an error
            }
        }



    public static bool RegisterUser(User userData){
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    
                    // Check if username already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                    MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Username", userData.Username);
                    int existingUserCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    
                    // If username already exists, return false
                    if (existingUserCount > 0)
                    {
                        Console.WriteLine("User with the same username already exists.");
                        return false;
                    }
                    
                    // If username does not exist, proceed with user registration
                    string insertQuery = "INSERT INTO Users (NameFirst, NameLast, Username, Password, PhoneNumber, Address) VALUES (@NameFirst, @NameLast, @Username, @Password, @PhoneNumber, @Address)";
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@NameFirst", userData.NameFirst);
                    cmd.Parameters.AddWithValue("@NameLast", userData.NameLast);
                    cmd.Parameters.AddWithValue("@Username", userData.Username);
                    cmd.Parameters.AddWithValue("@Password", userData.Password);
                    cmd.Parameters.AddWithValue("@PhoneNumber", userData.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", userData.Address);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error registering user: " + ex.Message);
                return false;
            }
        }


    public static int InsertUserRequirement(int userId, string skills, string wages, string address, string date){
        int insertedId = 0; 
        // Example using MySqlCommand (for MySQL):
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            string query = "INSERT INTO UserRequirements (UserID, Skills, Wages, Address, Date) VALUES (@UserID, @Skills, @Wages, @Address, @Date); SELECT LAST_INSERT_ID();";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@Skills", skills);
                cmd.Parameters.AddWithValue("@Wages", wages);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Date", date);
                insertedId = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        Console.WriteLine("user requirement inserted successfully");
        return insertedId;
    }

    public static bool RegisterServiceProvider(BOL.ServiceProvider serviceProvider){
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connString))
            {
                conn.Open();

                // Prepare the SQL query to insert the service provider into the database
                string insertQuery = "INSERT INTO ServiceProviders (NameFirst, NameLast, UserName, Password, PhoneNumber, Skills, Wages, Address) VALUES (@NameFirst, @NameLast, @UserName, @Password, @PhoneNumber, @Skills, @Wages, @Address)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                {
                    // Add parameters to the command to prevent SQL injection
                    cmd.Parameters.AddWithValue("@NameFirst", serviceProvider.NameFirst);
                    cmd.Parameters.AddWithValue("@NameLast", serviceProvider.NameLast);
                    cmd.Parameters.AddWithValue("@UserName", serviceProvider.Username);
                    cmd.Parameters.AddWithValue("@Password", serviceProvider.Password);
                    cmd.Parameters.AddWithValue("@PhoneNumber", serviceProvider.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Skills", serviceProvider.Skills); // Add Skills parameter
                    cmd.Parameters.AddWithValue("@Wages", serviceProvider.Wages);
                    cmd.Parameters.AddWithValue("@Address", serviceProvider.Address);

                    // Execute the command to insert the service provider into the database
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if the insert operation was successful
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the database operation
            Console.WriteLine("Error registering service provider: " + ex.Message);
            return false;
        }
    }


    public static BOL.ServiceProvider ValidateServiceProvider(string username, string password){
        Console.WriteLine("username: "+username+" password: "+ password);
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    // Prepare the SQL query to fetch the service provider details
                    string query = "SELECT * FROM ServiceProviders WHERE UserName = @UserName AND Password = @Password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Add parameters to the command to prevent SQL injection
                        cmd.Parameters.AddWithValue("@UserName", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        // Execute the command to fetch the service provider details
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if a service provider with the provided credentials exists
                            if (reader.Read())
                            {
                                // Service provider found, create a new ServiceProvider object and populate its properties
                                BOL.ServiceProvider serviceProvider = new BOL.ServiceProvider
                                {
                                    ServiceProviderID = reader.GetInt32("ServiceProviderID"),
                                    NameFirst = reader.GetString("NameFirst"),
                                    NameLast = reader.GetString("NameLast"),
                                    Username = reader.GetString("UserName"),
                                    Password = reader.GetString("Password"),
                                    PhoneNumber = reader.GetString("PhoneNumber"),
                                    Skills=reader.GetString("Skills"),
                                    Wages = reader.GetString("Wages"),
                                    Address = reader.GetString("Address")
                                };

                                return serviceProvider;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error validating service provider login: " + ex.Message);
            }
            return null; 
        }
    public static List<UserRequirementWithUserData> GetUserRequirementsWithUserInfo(string skills)
        {
            List<UserRequirementWithUserData> userRequirements = new List<UserRequirementWithUserData>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                     string query = @"SELECT DISTINCT u.UserID, u.NameFirst, u.NameLast, u.PhoneNumber, ur.Skills, ur.Wages, ur.Address, ur.Date
                                    FROM Users u
                                    INNER JOIN UserRequirements ur ON u.UserID = ur.UserID
                                    INNER JOIN ServiceProviders sp ON ur.Skills = sp.Skills
                                    WHERE ur.Skills = @skills;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                         cmd.Parameters.AddWithValue("@skills", skills);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserRequirementWithUserData userRequirement = new UserRequirementWithUserData
                                {
                                    UserID = reader.GetInt32("UserID"),
                                    NameFirst = reader.GetString("NameFirst"),
                                    NameLast = reader.GetString("NameLast"),
                                    PhoneNumber = reader.GetString("PhoneNumber"),
                                    Skills = reader.GetString("Skills"),
                                    Wages = reader.GetString("Wages"),
                                    Address = reader.GetString("Address"),
                                    Date = reader.GetString("Date")
                                };

                                userRequirements.Add(userRequirement);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine("Error fetching user requirements with user info: " + ex.Message);
            }

            return userRequirements;
        }


        public static BOL.ServiceProvider GetServiceProviderByID(int serviceProviderID)
        {
            BOL.ServiceProvider serviceProvider = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    string query = "SELECT NameFirst, NameLast, PhoneNumber, Skills, Wages " +
                                   "FROM ServiceProviders " +
                                   "WHERE ServiceProviderID = @ServiceProviderID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ServiceProviderID", serviceProviderID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                serviceProvider = new BOL.ServiceProvider
                                {
                                    ServiceProviderID=serviceProviderID,
                                    NameFirst = reader.GetString("NameFirst"),
                                    NameLast = reader.GetString("NameLast"),
                                    PhoneNumber = reader.GetString("PhoneNumber"),
                                    Skills = reader.GetString("Skills"),
                                    Wages = reader.GetString("Wages")

                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving service provider: " + ex.Message);
                // You may want to log the exception or handle it appropriately
            }

            return serviceProvider;
        }


        public static bool AddBookingEntry(int userID, BOL.ServiceProvider serviceProvider)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    string query = @"INSERT INTO BookingList (UserID,Username,NameFirst, NameLast, PhoneNumber, Skills, Wages) 
                                    VALUES (@UserID,@Username, @NameFirst, @NameLast, @PhoneNumber, @Skills, @Wages)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@Username", serviceProvider.Username);
                        // cmd.Parameters.AddWithValue("@ServiceProviderID", serviceProvider.ServiceProviderID);
                        cmd.Parameters.AddWithValue("@NameFirst",serviceProvider.NameFirst);
                        cmd.Parameters.AddWithValue("@NameLast", serviceProvider.NameLast);
                        cmd.Parameters.AddWithValue("@PhoneNumber", serviceProvider.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Skills", serviceProvider.Skills);
                        cmd.Parameters.AddWithValue("@Wages",serviceProvider.Wages);
                        // cmd.Parameters.AddWithValue("@Ratings", serviceProvider.ratings);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        Console.WriteLine("Added into booking list");
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine("Error adding booking entry: " + ex.Message);
                return false;
            }
        }

    public static List<BookingList> GetBookingListByUserId(int userId){
        Console.WriteLine("in get booking list");
        List<BookingList> bookingLists = new List<BookingList>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

        string query = @" SELECT b1.UserID,b1.Username,b1.NameFirst, b1.NameLast,b1.PhoneNumber,b1.Skills,
        b1.Wages,COALESCE(AVG(bl.Rating), 0) AS Rating     
                FROM 
                    BookingList b1
                LEFT JOIN 
                    Feedback bl ON b1.Username = bl.ServiceProviderUsername
                WHERE 
                    b1.UserID = @UserID
                GROUP BY 
                    b1.Username";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BookingList bookingList = new BookingList
                                {
                                    // ServiceProviderID = reader.GetInt32("ServiceProviderID"),
                                    UserID=reader.GetInt32("UserID"),
                                    Username = reader.GetString("Username"),
                                    NameFirst = reader.GetString("NameFirst"),
                                    NameLast = reader.GetString("NameLast"),
                                    PhoneNumber = reader.GetString("PhoneNumber"),
                                    Skills = reader.GetString("Skills"),
                                    Wages = reader.GetString("Wages"),
                                    Rating = (float)reader.GetDouble("Rating")
                                };

                                bookingLists.Add(bookingList);
                            }
                        }
                    }
                }
            }
            catch (Exception ex){
                // Log or handle the exception
                Console.WriteLine("Error fetching booking lists: " + ex.Message);
            }

            return bookingLists;
        }

    public static bool SelectServiceProvider(int userId, string serviceProviderUsername, bool isSelected){
        using (MySqlConnection connection = new MySqlConnection(connString))
        {
            try
            {
                connection.Open();

                string query = "INSERT INTO Status (UserID, ServiceProviderUserName, IsSelected) VALUES (@UserID, @ServiceProviderUserName, @IsSelected)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@ServiceProviderUserName", serviceProviderUsername);
                command.Parameters.AddWithValue("@IsSelected", isSelected);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error selecting service provider: " + ex.Message);
                return false;
            }
            
        }
    }



    public static List<ServiceProviderViewStatus> GetServiceProviderData(string serviceProviderUsername){
            List<ServiceProviderViewStatus> serviceProviderDataList = new List<ServiceProviderViewStatus>();
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                string query = @"SELECT distinct u.NameFirst, u.NameLast, u.PhoneNumber, ur.Wages, ur.Address, ur.Date
                                 FROM users u
                                 INNER JOIN userrequirements ur ON u.UserID = ur.UserID
                                 INNER JOIN status st ON u.UserID = st.UserID
                                 WHERE st.ServiceProviderUserName = @ServiceProviderUsername";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ServiceProviderUsername", serviceProviderUsername);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServiceProviderViewStatus serviceProviderData = new ServiceProviderViewStatus
                            {
                                NameFirst = reader["NameFirst"].ToString(),
                                NameLast = reader["NameLast"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                Address = reader["Address"].ToString(),
                                Wages = reader["Wages"].ToString(),
                                Date = reader["Date"].ToString()
                            };
                            serviceProviderDataList.Add(serviceProviderData);
                        }
                    }
                }
            }
            return serviceProviderDataList;
        }



        public static bool GiveFeedback(Feedback feedback)
        {
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                string query = @"INSERT INTO Feedback (ServiceProviderUsername, FeedbackMessage, Rating) 
                                 VALUES (@ServiceProviderUsername, @FeedbackMessage, @Rating)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ServiceProviderUsername", feedback.ServiceProviderUsername);
                    command.Parameters.AddWithValue("@FeedbackMessage", feedback.FeedbackMessage);
                    command.Parameters.AddWithValue("@Rating", feedback.Rating);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Log the exception
                        throw new Exception("Error giving feedback: " + ex.Message);
                    }
                }
            }
        }
}