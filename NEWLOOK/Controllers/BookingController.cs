using Microsoft.AspNetCore.Mvc;
using NEWLOOK.Models.NewLook;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using NEWLOOK.Models;
using Newtonsoft.Json;

public class BookingController : Controller
{
    private readonly NewLookContext _context;
    BookHdr booking;

    public BookingController(NewLookContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BookingViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            // Get cart from session
            var cart = HttpContext.Session.GetString("CartItems");
            if (string.IsNullOrEmpty(cart))
            {
                return RedirectToAction("Index", "Home"); // Redirect to home if cart is empty
            }

            var services = JsonConvert.DeserializeObject<List<ServiceType>>(cart);

            if (services == null || services.Count == 0)
            {
                return RedirectToAction("Index", "Home"); // Redirect to home if cart is empty
            }

            // Save customer
            var customer = new Customer
            {
                CustmName = model.CustmName,
                CustmPhone = model.CustmPhone,
                CustmEmail = model.CustmEmail,
                CustmAddress = model.CustmAddress,
                BirthDt = model.BirthDt
            };
            _context.Customers.Add(customer);
            _context.SaveChanges();

            // Generate Invoice No
            var invNo = "INV" + DateTime.Now.Ticks;
            var totalPrice = services.Sum(c => c.Price);

            booking = new BookHdr
            {
                InvNo = invNo,
                BookDate = DateTime.Now,
                TotalPrice = totalPrice,
                Discount = 0,
                CustmId = customer.Id,
                CreatedDt = DateTime.Now
            };
            _context.BookHdrs.Add(booking);
            _context.SaveChanges();

            foreach (var item in services)
            {
                _context.BookDets.Add(new BookDet
                {
                    InvNo = invNo,
                    Price = item.Price,
                    ServTypeId = item.Id
                });
            }

            _context.SaveChanges();

            // Clear cart
            HttpContext.Session.Remove("CartItems");

            return RedirectToAction("Success");
        }
        catch (Exception)
        {
            return RedirectToAction("Error");
        }
    }

    public IActionResult Success()
    {
        TempData["SuccessMessage"] = $"🎉 Your booking was successfully made on {booking.BookDate:dddd, MMMM dd, yyyy 'at' HH:mm}! Thank you for choosing us.";
        return RedirectToAction("Index", "Home");

    }

    public IActionResult Error()
    {
        TempData["ErrorMessage"] = "An error has occurred. Please try again.";
        return RedirectToAction("Index", "Home");
    }
}
