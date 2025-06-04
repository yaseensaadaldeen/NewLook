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
        // Get cart from session
        var cart = HttpContext.Session.GetString("CartItems");

        if (string.IsNullOrEmpty(cart))
        {
            // Redirect to Services page if no cart
            TempData["ErrorMessage"] = "Please select at least one service before booking.";
            return RedirectToAction("Index", "Services");
        }
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
            if (model.BookDate <= DateTime.Now)
            {
                ModelState.AddModelError("BookDate", "Booking date must be in the future.");
                return View(model);
            }
            if (model.BookDate > DateTime.Now.AddMonths(2))
            {
                ModelState.AddModelError("BookDate", "Booking date cannot be more than two months from today.");
                return View(model);
            }

            // Birth date should not be more recent than 2 years ago
            if (model.BirthDt > DateTime.Now.AddYears(-2))
            {
                ModelState.AddModelError("BirthDt", "Birth date must be more than 2 years ago.");
                return View(model);
            }
            if (services == null || services.Count == 0)
            {
                return RedirectToAction("Index", "Home"); // Redirect to home if cart is empty
            }
            var bookingDate = model.BookDate.Date;
            bool alreadyBooked = _context.BookHdrs.Any(b => b.BookDate.Date == bookingDate);

            if (alreadyBooked)
            {
                TempData["ErrorMessage"] = "There is already a booking on the selected date. Please choose a different date.";
                return View(model);
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
            var invNo = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
            var message = $@"*New Booking Request*
                            *Name:* {model.CustmName}
                            *Email:* {model.CustmEmail}
                            *Phone:* {model.CustmPhone}
                            *Booking Date:* {model.BookDate:yyyy-MM-dd HH:mm}
                            *Selected Services:*
                            {string.Join("%0A", services.Select(s => $"• {s.SerTypeName} - ${s.Price}"))}";

            var whatsappNumber = "+971586807722"; // Change to your number
            var whatsappUrl = $"https://wa.me/{whatsappNumber}?text={Uri.EscapeDataString(message)}";
            TempData["SuccessMessage"] = $"🎉 Your booking was successfully made , Thank you for choosing us.";
            return Redirect(whatsappUrl);
        }
        catch (Exception)
        {
            return RedirectToAction("Error");
        }
    }

    public IActionResult Success()
    {
        TempData["SuccessMessage"] = $"🎉 Your booking was successfully made , Thank you for choosing us.";
        return RedirectToAction("Index", "Home");

    }

    public IActionResult Error()
    {
        TempData["ErrorMessage"] = "An error has occurred. Please try again.";
        return RedirectToAction("Index", "Home");
    }
}
