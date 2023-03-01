using DarazApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System.Net.Http;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using DarazApp.Services;
using System.Net.Mail;
using DarazApp.Helpers;
using System.Net;
using Example;

public class LoginController : Controller
{
    public ActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult AdminPagesView()
    {
        return View();
    }
    public IActionResult SellerPagesView()
    {
        return View();
    }
    [HttpPost]
    public ActionResult ForgotPassword(string email)
    {
        // check if email is valid and exists in the database
        // if yes, then generate OTP and send it to the email
        // if not, return error message
        if (IsValidEmail(email))
        {
            CommerceDbContext commerceDb = new CommerceDbContext();
            var Session = HttpContext.Session;
            //var otp = GenerateOTP();
            
            //SendOTP(otp, email);
            // store OTP in the session
            //HttpContext.Session.Equals(HttpContext.User);
            //HttpContent content = new StringContent(otp);
            //HttpContent content1 = new StringContent(email);
            //HttpContext.Session.SetString("OTP",otp);
            //HttpContext.Session.SetString("Email",email);
            //Session["OTP"] = otp;
            //Session["Email"] = email;
            return RedirectToAction("VerifyOTP");
        }
        else
        {
            ModelState.AddModelError("", "Invalid email");
            return View();
        }
    }
    public IActionResult UserRequestsForAdmin()
    {
        return View();
    }

    public ActionResult VerifyOTP()
    {
        return View();
    }

    [HttpPost]
    public ActionResult VerifyOTP(string otp)
    {
        // check if the OTP in the session matches the one entered by the user
        // if yes, redirect to reset password page
        // if not, return error message
        //var sessionOTP = HttpContext.Session.Equals(otp);
        CommerceDbContext commerceDb = new CommerceDbContext();
        List<User> users = commerceDb.Users.Where(x=>x.OTP == otp).ToList();
        //var sessionOTP = (string)HttpContext.Session.GetString("OTP");

        if (users.Count != 0)
        {
            return RedirectToAction("ResetPassword");
        }
        else
        {
            ModelState.AddModelError("", "Invalid OTP");
            return View();
        }
    }

    public ActionResult ResetPassword()
    {
        return View();
    }

    //[HttpPost]
    //public ActionResult ResetPassword(string newPassword)
    //{
    //    // update the password in the database
    //    var email = (string)HttpContext.Session.GetString("Email");
    //    UpdatePassword(email, newPassword);
    //    SendConfirmation(email);
    //    return RedirectToAction("Login");
    //}
    //[HttpPost]
    public void ValidEmail(string email)
    {
        CommerceDbContext context = new CommerceDbContext();
        List<User> user = context.Users.Where(x => x.Email == email).ToList();
        if (user.Count != 0)
        {
            //return Json (user);
            GenerateOTP(email);
        }
        else
        {
             BadRequest();
        }
    }
    private bool IsValidEmail(string email)
    {
        CommerceDbContext context = new CommerceDbContext();
        List<User> user = context.Users.Where(x => x.Email == email).ToList();
        if (user.Count != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        // code to check if email exists in the database
        // return true if exists, else false
    }

    public void GenerateOTP(string email)
    {
        // code to generate OTP
        char[] charArr = "0123456789".ToCharArray();
        string strrandom = string.Empty;
        Random objran = new Random();
        for (int i = 0; i < 4; i++)
        {
            //It will not allow Repetation of Characters
            int pos = objran.Next(1, charArr.Length);
            if (!strrandom.Contains(charArr.GetValue(pos).ToString())) strrandom += charArr.GetValue(pos);
            else i--;
        }
      
        /*eturn strrandom;*/
        //SendOTP(email, strrandom);
    }
    public void SendOTPs(string email, string OTP)
    {
  
        //SendOTP(email,OTP).Wait();
    }

    // code to send OTP to the email
    //[HttpPost]
    //public ActionResult SendOTP(string email, string OTP)
    //{

        
    //}
    //xvvjtdyxonrdvqrq
    //s1749zaman @gmail.com

        //jccrrhlghpjzfmzi
        //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
//    var fromAddress = new MailAddress("zamanmalik1749@gmail.com", "zaman malik");
//    var toAddress = new MailAddress("zamanazam1749@gmail.com", "Zaman Azam Zaman");
//    const string fromPassword = "malikazam";
//    const string subject = "Your OTP is";
//    const string body = "Body of the email.''OTP'";

    //    var smtp = new SmtpClient
    //    {
    //        Host = "smtp.gmail.com",
    //        Port = 587,
    //        EnableSsl = true,
    //        DeliveryMethod = SmtpDeliveryMethod.Network,
    //        UseDefaultCredentials = false,
    //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
    //    };
    //        using (var message = new MailMessage(fromAddress, toAddress)
    //        {
    //            Subject = subject,
    //            Body = body
    //        })
    //{
    //    smtp.Send(message);
    //}

    //CommerceDbContext commerceDb = new CommerceDbContext();
    //var Update = commerceDb.Users.Where(y => y.Email == email).Select(z => new User()
    //{
    //    Age = z.Age,
    //    Email = z.Email,
    //    Cnic = z.Cnic,
    //    Address = z.Address,
    //    City = z.City,
    //    Date = z.Date,
    //    FatherName = z.FatherName,
    //    UserName = z.UserName,
    //    Gender = z.Gender,
    //    Nationality = z.Nationality,
    //    Password = z.Password,
    //    PhoneNo = z.PhoneNo,
    //    PostalCode = z.PostalCode,
    //    UserId = z.UserId,
    //    UserImage = z.UserImage,
    //    OTP = z.OTP,
    //}).FirstOrDefault();
    //List<User> users = commerceDb.Users.Where(x => x.Email == email).ToList();
    //foreach (User user in users)
    //{
    //    user.Age = Update.Age;
    //    user.Email = Update.Email;
    //    user.Cnic = Update.Cnic;
    //    user.Address = Update.Address;
    //    user.City = Update.City;
    //    user.Date = Update.Date;
    //    user.FatherName = Update.FatherName;
    //    user.UserName = Update.UserName;
    //    user.Gender = Update.Gender;
    //    user.Nationality = Update.Nationality;
    //    user.Password = Update.Password;
    //    user.PhoneNo = Update.PhoneNo;
    //    user.PostalCode = Update.PostalCode;
    //    user.UserId = Update.UserId;
    //    user.UserImage = Update.UserImage;
    //    user.OTP = OTP;
    //    commerceDb.Users.Update(user);
    //    commerceDb.SaveChanges();
    //}


}