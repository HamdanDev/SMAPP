using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMAPP.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

namespace SMAPP.Controllers
{
    public class SponsorsController : Controller
    {
        private ApplicationDbContext _context;

        public SponsorsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        // GET: Sponsors
        public ViewResult Index()
        {
            if (User.IsInRole(RoleName.CanManageAPP))
                return View("Index");

            return View("ReadOnlyList");
        }



        [Authorize(Roles = RoleName.CanManageAPP)]
        public ActionResult New()
        {
            var model = new Sponsor();

            return View("SponsorsForm", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageAPP)]
        public ActionResult Save(Sponsor sponsor)
        {
            if (!ModelState.IsValid)
            {
                var model = new Sponsor();
                return View("SponsorForm", model);
            }

            if (sponsor.Id == 0)
                _context.Sponsors.Add(sponsor);
            else
            {
                var sponsorInDb = _context.Sponsors.Single(c => c.Id == sponsor.Id);

                sponsorInDb.Name = sponsor.Name;
                sponsorInDb.Url = sponsor.Url;
                sponsorInDb.Username = sponsor.Username;
                sponsorInDb.Password = sponsor.Password;
                sponsorInDb.UsernameTag = sponsor.UsernameTag;
                sponsorInDb.PasswordTag = sponsor.PasswordTag;
                sponsorInDb.SubmitTag = sponsor.SubmitTag;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Sponsors");
        }



        [Authorize(Roles = RoleName.CanManageAPP)]
        public ActionResult Details(int id)
        {
            var sponsor = _context.Sponsors.SingleOrDefault(c => c.Id == id);

            if (sponsor == null)
                return HttpNotFound();

            return View(sponsor);
        }



        [Authorize(Roles = RoleName.CanManageAPP)]
        public ActionResult Edit(int id)
        {
            var sponsor = _context.Sponsors.SingleOrDefault(c => c.Id == id);

            if (sponsor == null)
                return HttpNotFound();

            var model = new Sponsor();

            return View("SponsorsForm", model);

        }

        public ActionResult GoTo(int id)
        {
            var sponsor = _context.Sponsors.SingleOrDefault(c => c.Id == id);

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            //Hide Browser
            var options = new ChromeOptions();
            //options.AddArgument("headless");
            options.AddArgument("--window-position=-32000,-32000");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--incognito");


            var driver = new ChromeDriver(service, options);
            driver.Navigate().GoToUrl(sponsor.Url);

            // Get the page elements
            try
            {
                IWebElement userNameField = null;
                IWebElement PasswordField = null;
                IWebElement loginButton = null;

                switch (sponsor.UsernameTagType)
                {
                    case "id":
                        userNameField = driver.FindElement(By.Id(sponsor.UsernameTag));
                        break;

                    case "name":
                        userNameField = driver.FindElement(By.Name(sponsor.UsernameTag));
                        break;   
                }

                switch (sponsor.PasswordTagType)
                {
                    case "id":
                        PasswordField = driver.FindElement(By.Id(sponsor.PasswordTag));
                        break;

                    case "name":
                        PasswordField = driver.FindElement(By.Name(sponsor.PasswordTag));
                        break;

                    case "type":
                        PasswordField = driver.FindElement(By.TagName(sponsor.PasswordTag));
                        break;
                }

                switch (sponsor.SubmitTagType)
                {
                    case "id":
                        loginButton = driver.FindElement(By.Id(sponsor.SubmitTag));
                        break;

                    case "name":
                        loginButton = driver.FindElement(By.Name(sponsor.SubmitTag));
                        break;

                    case "type":
                        loginButton = driver.FindElement(By.TagName(sponsor.SubmitTag));
                        break;
                }

               // Type user name and password
                userNameField.Clear();
                userNameField.Clear();
                userNameField.SendKeys(sponsor.Username);
                PasswordField.SendKeys(sponsor.Password);

                // and click the login button
                try
                {
                    Actions builder = new Actions(driver);
                    builder.SendKeys(Keys.Enter);
                }
                catch
                {
                    loginButton.Click();
                }
                                
                driver.Manage().Window.Maximize();
            }
            catch
            {

            }

            if (User.IsInRole(RoleName.CanManageAPP))
                return View("Index");
            else
                return View("ReadOnlyList");
        }
    }
}