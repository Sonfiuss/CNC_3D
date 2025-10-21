using Microsoft.AspNetCore.Mvc;
using WebServer.ModelDTO;

namespace WebServer.Controllers;

public class AccountController : Microsoft.AspNetCore.Mvc.Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // TODO: replace with real authentication
        if (model.Username == "admin" && model.Password == "admin")
        {
            // Redirect to Swagger or a home page for now
            return Redirect("/swagger");
        }

        ModelState.AddModelError(string.Empty, "Invalid username or password");
        return View(model);
    }
}
