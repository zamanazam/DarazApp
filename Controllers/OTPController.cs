using DarazApp.Entities;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Net;
using System.Net.Mail;
using static System.Net.WebRequestMethods;
using Microsoft.EntityFrameworkCore;
using DarazApp.Services;

public class OTPController : Controller
{
    public IActionResult GenerateOTP(string email)
    {
        CommerceDbContext context = new CommerceDbContext();
        List<User> user = context.Users.Where(x => x.Email == email).ToList();
        if (user.Count != 0)
        {
        char[] charArr = "0123456789".ToCharArray();
        string OTP = string.Empty;
        Random objran = new Random();
        for (int i = 0; i < 4; i++)
        {
            //It will not allow Repetation of Characters
            int pos = objran.Next(1, charArr.Length);
            if (!OTP.Contains(charArr.GetValue(pos).ToString())) OTP += charArr.GetValue(pos);
            else i--;
        }
            // OTP generation code
            Execute(email,OTP);
            return Json(OTP);
        }
        else
        {
            return BadRequest();
        }
    }

    static async Task Execute(string email,string OTP)
    {
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("zaid.s.dev@gmail.com", "Zaid Test");
        var subject = "ZaCommerceComapany";
        var to = new EmailAddress(email, "Zaman Azam Zaman");
        var plainTextContent = "Your OTP is" + OTP;
        var htmlContent = "<strong>Your OTP is</strong>" + OTP;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);

    CommerceDbContext commerceDb = new CommerceDbContext();
    var Update = commerceDb.Users.Where(y => y.Email == email).Select(z => new User()
    {
        Age = z.Age,
        Email = z.Email,
        Cnic = z.Cnic,
        Address = z.Address,
        City = z.City,
        Date = z.Date,
        FatherName = z.FatherName,
        UserName = z.UserName,
        Gender = z.Gender,
        Nationality = z.Nationality,
        Password = z.Password,
        PhoneNo = z.PhoneNo,
        PostalCode = z.PostalCode,
        UserId = z.UserId,
        UserImage = z.UserImage,
        OTP = z.OTP,
    }).FirstOrDefault();
    List<User> users = commerceDb.Users.Where(x => x.Email == email).ToList();
    foreach (User user in users)
    {
        user.Age = Update.Age;
        user.Email = Update.Email;
        user.Cnic = Update.Cnic;
        user.Address = Update.Address;
        user.City = Update.City;
        user.Date = Update.Date;
        user.FatherName = Update.FatherName;
        user.UserName = Update.UserName;
        user.Gender = Update.Gender;
        user.Nationality = Update.Nationality;
        user.Password = Update.Password;
        user.PhoneNo = Update.PhoneNo;
        user.PostalCode = Update.PostalCode;
        user.UserId = Update.UserId;
        user.UserImage = Update.UserImage;
        user.OTP = OTP;
        commerceDb.Users.Update(user);
        commerceDb.SaveChanges();
    }
    }

    public IActionResult VerifyOTP(string OTP)
    {
        CommerceDbContext commerceDb = new CommerceDbContext();
        List<User> user = commerceDb.Users.Where(x => x.OTP == OTP).ToList();
        if (user.Count != null)
        {
            return Json(user);
        }
            return BadRequest();
    }
    public IActionResult ChangePassword(string OTP, string Password )
    {
        CommerceDbContext commerceDb = new CommerceDbContext();
        var Update = commerceDb.Users.Where(y => y.OTP == OTP).Select(z => new User()
        {
            Age = z.Age,
            Email = z.Email,
            Cnic = z.Cnic,
            Address = z.Address,
            City = z.City,
            Date = z.Date,
            FatherName = z.FatherName,
            UserName = z.UserName,
            Gender = z.Gender,
            Nationality = z.Nationality,
            Password = z.Password,
            PhoneNo = z.PhoneNo,
            PostalCode = z.PostalCode,
            UserId = z.UserId,
            UserImage = z.UserImage,
            OTP = z.OTP,
        }).FirstOrDefault();
        var latestPassword = "";
        List<User> users = commerceDb.Users.Where(x => x.OTP == OTP).ToList();
        foreach (User user in users)
        {
            user.Age = Update.Age;
            user.Email = Update.Email;
            user.Cnic = Update.Cnic;
            user.Address = Update.Address;
            user.City = Update.City;
            user.Date = Update.Date;
            user.FatherName = Update.FatherName;
            user.UserName = Update.UserName;
            user.Gender = Update.Gender;
            user.Nationality = Update.Nationality;
            user.Password = Password;
            user.PhoneNo = Update.PhoneNo;
            user.PostalCode = Update.PostalCode;
            user.UserId = Update.UserId;
            user.UserImage = Update.UserImage;
            user.OTP = OTP;
            commerceDb.Users.Update(user);
            commerceDb.SaveChanges();
            latestPassword = user.Password;
        }
        return Json(latestPassword);
    }
}