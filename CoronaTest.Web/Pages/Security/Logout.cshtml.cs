using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoronaTest.Web.Pages.Security
{
	public class LogoutModel : PageModel
	{
		[BindProperty]
		public string Message { get; set; }
		[BindProperty]
		public int? LoggedUserId { get; set; }

		public IActionResult OnGet()
		{
			// ---------- request cookie ------------
			var cookieValue = Request.Cookies["MyCookieId"];
			if (cookieValue == null)
			{
				Message = "Benutzer war nicht angemeldet";
				return Page();
			}
			// --------------------------------------
			LoggedUserId = int.Parse(cookieValue);

			foreach (var cookie in HttpContext.Request.Cookies)
			{
				Response.Cookies.Delete(cookie.Key);
			}

			try
			{
				return RedirectToPage("Login", new
				{
					message = "Sie haben sich abgemeldet"
				});
			}
			catch (Exception e)
			{
				Message = e.InnerException.Message;
				return Page();
			}
		}
	}
}
