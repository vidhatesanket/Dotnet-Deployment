using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BOL;
using BLL;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Asn1.Iana;
namespace WebAPI.Controllers;
[ApiController]
[Route("api/[controller]")]

public class ServiceProvider : Controller{
    private readonly ILogger<ServiceProvider> _logger;

    public ServiceProvider(ILogger<ServiceProvider> logger){
        _logger = logger;
    }

     
    [HttpPost("login")] 
    public IActionResult serviceproviderlogin([FromForm] string Username,[FromForm] string Password){
        Console.WriteLine("service provider login");
        BOL.ServiceProvider serviceProviderExists = ServiceProviderManager.ValidateServiceProvider(Username, Password);
        if (serviceProviderExists!=null){
            HttpContext.Session.SetInt32("ServiceProviderID", serviceProviderExists.ServiceProviderID);
            return Ok(new { message = "Login successful" ,serviceProviderExists=serviceProviderExists});
        }
        else{
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }

    [HttpPost("registration")]
    public IActionResult RegisterServiceProvider([FromBody] BOL.ServiceProvider serviceProvider){
        if (serviceProvider == null){
            return BadRequest("Invalid request body");
        }

         bool registrationSuccess = ServiceProviderManager.RegisterServiceProvider(serviceProvider);

        if (registrationSuccess){
            return Ok(new { message = "ServiceProvider registered successfully!" });
        }
        else{
            return BadRequest(new { message = "Failed to register service provider. Username already exists." });
        }
    }

    [HttpGet("userrequirements")]
    public IActionResult GetUserRequirements(string skills){
        Console.WriteLine("in get user requirement");
            try
            {
                List<UserRequirementWithUserData> userRequirements = ServiceProviderManager.GetUserRequirements(skills);
                return Ok(userRequirements);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error retrieving user requirements: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to retrieve user requirements" });
            }
        }
        [HttpGet("getStatus")]
        public IActionResult GetServiceProviderData(string serviceProviderUsername)
        {
            List<ServiceProviderViewStatus> serviceProviderData = ServiceProviderManager.GetServiceProviderData(serviceProviderUsername);
            if (serviceProviderData != null && serviceProviderData.Count > 0)
            {
                return Ok(serviceProviderData);
            }
            else
            {
                return NotFound();
            }
        }
    
}


    
