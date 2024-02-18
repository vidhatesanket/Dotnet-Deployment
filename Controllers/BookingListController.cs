using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BOL;
using BLL;
using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Asn1.Iana;
using DAL;
namespace WebAPI.Controllers;
[ApiController]
[Route("api/[controller]")]

    public class BookingList : Controller{

        [HttpGet("user/bookingList")]
        public IActionResult GetAllBookingList(int userID)
        {
            Console.WriteLine("Getting booking list");
            if (userID <= 0)
            {
                return BadRequest("Invalid user ID");
            }

            // Call the BLL method to get booking lists for the specified user ID
            List<BOL.BookingList> bookingLists = BookingListManager.GetBookingListByUserId(userID);

            if (bookingLists == null)
            {
                return NotFound("No booking lists found for the specified user ID");
            }
            foreach (var item in bookingLists)
            {
                Console.WriteLine(item);
            }
            
            return Ok(bookingLists);
        }

        [HttpPost("serviceProvider/bookingList")]
        public IActionResult AddIntoBookingList([FromBody] BookingListViewModel model){

            foreach (int userId in model.UserIDs){
                    
                BookingListManager.AddIntoBookingList(userId, model.serviceProviderData);
                    
            }
            
            return Ok("Added service provider into booking list for all users.");
        }




        [HttpPost("user/bookServiceProvider")]
        public IActionResult SelectServiceProvider([FromBody] SelectServiceProviderRequest request)
        {
            Console.WriteLine("You were booked: userid ==>" + request.UserID+" username==> "+ request.ServiceProviderUsername+" status==> " +request.IsSelected);
            // Assuming you have a service to handle the selection logic
            bool selectionSuccess = BookingListManager.SelectServiceProvider(request.UserID, request.ServiceProviderUsername,request.IsSelected);

            if (selectionSuccess)
            {
                return Ok(new { message = "Service provider selected successfully" });
            }
            else
            {
                return BadRequest(new { message = "Failed to select service provider" });
            }
        }

    
    }