using RecipesWebApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipesWebApplication.Controllers
{
    public class BaseController : Controller
    {
        private UserMaster _userObject;
        protected UserMaster userObject
        {
            get
            {
                if (Session["UserID"] == null)
                    throw new Exception("User Object accessed with no valid User ID in session.");

                if (_userObject == null)
                    _userObject = new UserMaster(); // fill from db

                return _userObject;
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if(Session["UserID"] == null)
            {
                filterContext.Result = RedirectToAction("Login", "Login", new { ReturnURL = Request.Url});
            }
        }
    }
}