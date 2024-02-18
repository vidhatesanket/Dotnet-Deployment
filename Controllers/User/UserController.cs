using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BOL;
using BLL;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Asn1.Iana;
namespace WebAPI.Controllers;
[ApiController]
[Route("api/[controller]")]

public class User : Controller{
    private readonly ILogger<User> _logger;

    public User(ILogger<User> logger){
        _logger = logger;
    }

    /* http://localhost:5020/api/User/login
    body-> form-data  
    Username: 
    Password:
    */
    [HttpPost("login")] 
    public IActionResult userlogin([FromForm] string Username, [FromForm] string Password){
        Console.WriteLine("User login");

        // dotnet add package Microsoft.AspNetCore.Session

        BOL.User userExists = UserManager.ValidateUser(Username, Password);
        if (userExists != null)
        {
            HttpContext.Session.SetInt32("UserID", userExists.UserID);
            return Ok(new { message = "Login successful", user = userExists });
        }
        else
        {
            return Unauthorized(new { message = "Invalid Credentials" });
        }
    }

    /* http://localhost:5020/api/User/register
    {
    "NameFirst": "bhupesh",
    "NameLast": "verma",
    "Username": "bhupeshVerma",
    "Password": "securePass123",
    "PhoneNumber": "5551234567",
    "Address": "789 Oak St"
    }

    */
    [HttpPost("registration")]
    public IActionResult RegisterUser([FromBody] BOL.User userData){
        
        bool registrationSuccess = UserManager.RegisterUser(userData);
        if (registrationSuccess)
        {
            return Ok(new { message = "User registered successfully!" });
        }
        else
        {
            return BadRequest(new { message = "Failed to register user. Username already exists." });
        }
    }

    [HttpPost("adduserrequirement")]
    public ActionResult AddUserRequirement([FromForm]int userID,[FromForm] string skills,[FromForm] string wages,[FromForm] string address,[FromForm] string date){
            // Retrieve the user ID from session
            Console.WriteLine("userID====>"+ userID);

        if (userID > 0){
            // Call BLL method to add user requirement
           
            int insertedId = UserManager.AddUserRequirement(userID, skills, wages, address, date);
            // You can handle further logic or return a response as needed
            return Ok(new { message = "User requirement added successfully", insertedId });
        }
        else{
            // Handle case where user is not logged in or session expired
            return Unauthorized(new { message = "User not logged in" });
        }
    }
    [HttpPost("giveFeedback")]
    public IActionResult GiveFeedback([FromBody] Feedback feedback){
        UserManager.AddFeedback(feedback);
        return Ok(new { message = "Feedback added successfully!" });
    }
    

}