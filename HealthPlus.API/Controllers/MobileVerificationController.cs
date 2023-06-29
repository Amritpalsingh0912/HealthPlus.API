using HealthPlus.Domain.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Net;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace HealthPlus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileVerificationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public string AccountSid;
        public string AuthToken;
        public string FromMessage;
        public MobileVerificationController(IConfiguration configuration)
        {
            _configuration = configuration;
            AccountSid = _configuration.GetValue<string>("AccountSid")!;
            AuthToken = _configuration.GetValue<string>("AuthToken")!;
            FromMessage = _configuration.GetValue<string>("FromNumber")!;
        }
        [HttpPost("SendOtp")]
        public  IActionResult SendOtp([FromForm] Verification verification)
        {
            if (verification == null)
                return BadRequest();
            verification.Otp = GenerateOtp();
            TwilioClient.Init(AccountSid, AccountSid);
            var message = MessageResource.Create(
                body: $"{verification.Otp} is your one time password to login to your HealthPlus account.It is valid for 1 minute.Do not share your OTP with anyone",
                from: new Twilio.Types.PhoneNumber(FromMessage),
                to: new PhoneNumber("+91" + verification.PhoneNumber)
                );
            HttpContext.Response.Cookies.Append("Access", $"{verification.Otp}", new CookieOptions() { HttpOnly = true, Expires = DateAndTime.Now.AddMinutes(1) });
            return Ok(message);
        }
        [NonAction]
        public string GenerateOtp()
        {
            Random rnd = new Random();
            string otp = (rnd.Next(100000, 999999)).ToString();
            return otp;
        }
        [HttpPost("VerifyOtp")]
        public IActionResult VerifyOtp([FromForm] Verification verification)
        {
            var data = Request.Cookies["Access"];
            if (data==null)
                return BadRequest("Otp is expired");
            if (data == verification.Otp)
                return Ok("Otp is verified");
            else
                return BadRequest("Invalid otp");
        }
    }
}
