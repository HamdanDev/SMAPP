using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMAPP.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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

            IWebDriver driver = new ChromeDriver();

            driver.Manage().Window.Minimize();
            driver.Navigate().GoToUrl(sponsor.Url);

            // Get the page elements
            try
            {
                var userNameField = driver.FindElement(By.Id(sponsor.UsernameTag));
                var PasswordField = driver.FindElement(By.Id(sponsor.PasswordTag));
                var loginButton   = driver.FindElement(By.Id(sponsor.SubmitTag));

                // Type user name and password
                userNameField.SendKeys(sponsor.Username);
                PasswordField.SendKeys(sponsor.Password);

                // and click the login button
                loginButton.Click();

                driver.Manage().Window.Maximize();
            }
            catch {}


            return View("Index");
        }
    }
}