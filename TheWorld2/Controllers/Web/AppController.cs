using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;
using TheWorld2;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
  {
    private IMailService _mailService;
    private IWorldRepository _repository;

    public AppController(IMailService service, IWorldRepository repository)
    {
      _mailService = service;
      _repository = repository;
    }

    public IActionResult Index()
    {
      return View();
    }

    [Authorize]
    public IActionResult Trips()
    {
      var trips = _repository.GetAllTrips();

      return View(trips);
    }

    public IActionResult About()
    {
      return View();
    }

    public IActionResult Contact()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Contact(ContactViewModel model)
    {
      if (ModelState.IsValid)
      {
                var email = "ricardo.goes@email.com";// Configuration["AppSettings:SiteEmailAddress"];

        if (string.IsNullOrWhiteSpace(email))
        {
          ModelState.AddModelError("", "Could not send email, configuration problem.");
        }
        
        if (_mailService.SendMail(email,
          email,
          $"Contact Page from {model.Name} ({model.Email})",
          model.Message))
        {
          ModelState.Clear();

          ViewBag.Message = "Mail Sent. Thanks!";

        }
      }

      return View();
    }
  }
}
