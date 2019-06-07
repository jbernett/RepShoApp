using RecipesWebApplication.Models;
using RecipesWebApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RecipesWebApplication.Controllers
{
    public class LoginController : Controller
    {
        private int saltLengthLimit;

        private static byte[] Get_SALT()
        {
            int saltLengthLimit = 64;
            return Get_SALT(saltLengthLimit);
        }

        private static byte[] Get_SALT(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];

            //Require NameSpace: using System.Security.Cryptography;
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
        public static string Get_HASH_SHA512(string password, string username, byte[] salt)
        {
            try
            {
                //required NameSpace: using System.Text;
                //Plain Text in Byte
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(password + username);

                //Plain Text + SALT Key in Byte
                byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + salt.Length];

                for (int i = 0; i < plainTextBytes.Length; i++)
                {
                    plainTextWithSaltBytes[i] = plainTextBytes[i];
                }

                for (int i = 0; i < salt.Length; i++)
                {
                    plainTextWithSaltBytes[plainTextBytes.Length + i] = salt[i];
                }

                HashAlgorithm hash = new SHA512Managed();
                byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
                byte[] hashWithSaltBytes = new byte[hashBytes.Length + salt.Length];

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hashWithSaltBytes[i] = hashBytes[i];
                }

                for (int i = 0; i < salt.Length; i++)
                {
                    hashWithSaltBytes[hashBytes.Length + i] = salt[i];
                }

                return Convert.ToBase64String(hashWithSaltBytes);
            }
            catch
            {
                return string.Empty;
            }
        }
        [HttpGet]
        public ActionResult Login(string returnURL)
        {
            var userinfo = new LoginVm();

            try
            {
                // We do not want to use any existing identity information
                EnsureLoggedOut();

                // Store the originating URL so we can attach it to a form field
                userinfo.ReturnURL = returnURL;

                return View(userinfo);
            }
            catch
            {
                throw;
            }
        }
        //GET: EnsureLoggedOut
        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }

        //POST: Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            try
            {
                // First we clean the authentication ticket like always
                //required NameSpace: using System.Web.Security;
                FormsAuthentication.SignOut();

                // Second we clear the principal to ensure the user does not retain any authentication
                //required NameSpace: using System.Security.Principal;
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();

                // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
                // this clears the Request.IsAuthenticated flag since this triggers a new request
                return RedirectToLocal();
            }
            catch
            {
                throw;
            }
        }
        //GET: SignInAsync
        private void SignInRemember(string userName, bool isPersistent = false)
        {
            // Clear any lingering authencation data
            FormsAuthentication.SignOut();

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie(userName, isPersistent);
        }

        //GET: RedirectToLocal
        private ActionResult RedirectToLocal(string returnURL = "")
        {
            try
            {
                // If the return url starts with a slash "/" we assume it belongs to our site
                // so we will redirect to this "action"
                if (!string.IsNullOrWhiteSpace(returnURL) && Url.IsLocalUrl(returnURL))
                    return Redirect(returnURL);

                // If we cannot verify if the url is local to our host we redirect to a default location
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVm entity)
        {
            string OldHASHValue = string.Empty;
            byte[] SALT = new byte[saltLengthLimit];

            try
            {
                using (RepshoDBE db = new RepshoDBE())
                {
                    // Ensure we have a valid viewModel to work with
                    if (!ModelState.IsValid)
                        return View(entity);

                    //Retrive Stored HASH Value From Database According To Username (one unique field)
                    var userInfo = db.UserMasters.Where(s => s.Username == entity.Username.Trim()).FirstOrDefault();

                    //Assign HASH Value
                    if (userInfo != null)
                    {
                        OldHASHValue = userInfo.HASH;
                        SALT = userInfo.SALT;
                    }

                    bool isLogin = CompareHashValue(entity.Password, entity.Username, OldHASHValue, SALT);

                    if (isLogin)
                    {
                        //Login Success
                        //For Set Authentication in Cookie (Remeber ME Option)
                        SignInRemember(entity.Username, entity.isRemember);

                        //Set A Unique ID in session
                        Session["UserID"] = userInfo.UserID;

                        // If we got this far, something failed, redisplay form
                        // return RedirectToAction("Index", "Dashboard");
                        return RedirectToLocal(entity.ReturnURL);
                    }
                    else
                    {
                        //Login Fail
                        TempData["ErrorMSG"] = "Access Denied! Wrong Credential";
                        return View(entity);
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        public static bool CompareHashValue(string password, string username, string OldHASHValue, byte[] SALT)
        {
            try
            {
                string expectedHashString = Get_HASH_SHA512(password, username, SALT);

                return (OldHASHValue == expectedHashString);
            }
            catch
            {
                return false;
            }
        }
        [HttpGet]
        public ActionResult Registration(string returnURL)
        {
            var userinfo = new RegisterVM();

            try
            {
                // We do not want to use any existing identity information
                EnsureLoggedOut();

                // Store the originating URL so we can attach it to a form field
                userinfo.ReturnURL = returnURL;

                return View(userinfo);
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(RegisterVM entity)
        {
            string HASHValue = string.Empty;
            byte[] SALT = new byte[saltLengthLimit];

            try
            {
                using (RepshoDBE db = new RepshoDBE())
                {
                    // Ensure we have a valid viewModel to work with
                    if (!ModelState.IsValid)
                        return View(entity);

                    //Retrive Stored HASH Value From Database According To Username (one unique field)
                    var userInfo = db.UserMasters.Where(s => s.Username == entity.Username.Trim()).FirstOrDefault();

                    //Assign HASH Value
                    if (userInfo == null)
                    {
                        UserMaster user = new UserMaster();
                        SALT = Get_SALT();
                        HASHValue = Get_HASH_SHA512(entity.Password, entity.Username, SALT);
                        user.SALT = SALT;
                        user.HASH = HASHValue;
                        user.Username = entity.Username;
                        user.FirstName = entity.FirstName;
                        user.LastName = entity.LastName;
                        user.UserEmail = entity.UserEmail;
                        db.UserMasters.Add(user);
                        db.SaveChanges();
                        ModelState.Clear();
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        //Login Fail
                        TempData["ErrorMSG"] = "User Exists";
                        return View(entity);
                    }
                                      
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
    }
}