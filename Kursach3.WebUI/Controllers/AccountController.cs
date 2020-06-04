using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kursach3.WebUI.Infrastructure.Abstract;
using Kursach3.WebUI.Models;
using Kursach3Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Data.Entity;
using Kursach3Domain.Abstract;
using Kursach3Domain.Concrete;
using System.Net.Mail;
namespace Kursach3.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        private IStatsRepository statsRepository;
        public AccountController(IStatsRepository stats)
        {
            statsRepository = stats;
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name, EmailConfirmed = false };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                var Role = RoleManager.Roles.First(x => x.Name == "Користувач");
                if (result.Succeeded)
                {
                    if (Role != null)
                    {
                        UserManager.AddToRole(user.Id, Role.Name);
                    }
                    // наш email с заголовком письма
                    MailAddress from = new MailAddress("cybertestcorpmail@gmail.com", "Реєстрація");
                    // кому отправляем
                    MailAddress to = new MailAddress(user.Email);
                    // создаем объект сообщения
                    MailMessage m = new MailMessage(from, to);
                    // тема письма
                    m.Subject = "Підтвердження реєстрації";
                    // текст письма - включаем в него ссылку
                    m.Body = string.Format("Для завершення реєстрації перейдіть за посиланням:" +
                                    "<a href=\"{0}\" title=\"Підтвердити реєстрацію\">{0}</a>",
                        Url.Action("ConfirmEmail", "Account", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
                    m.IsBodyHtml = true;
                    // адрес smtp-сервера, с которого мы и будем отправлять письмо
                    SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com",587);
                    smtp.EnableSsl = true;
                    // логин и пароль
                    smtp.Credentials = new System.Net.NetworkCredential("cybertestcorpmail@gmail.com", "AleZZeYkA12_45_12");
                    smtp.Send(m);
                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            TempData["message"] = string.Format("На поштову адресу \"{0}\" Вам надіслани подальщі інструкції по завершенню реєстрації", Email);
            return RedirectToAction("Index","Default");
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            ApplicationUser user = this.UserManager.FindById(Token);
            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.EmailConfirmed = true;
                    await UserManager.UpdateAsync(user);
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);
                    TempData["message"] = string.Format("Вітаємо, {0} ви завершили реєстрацію та підтвердили свою пошту, ласкаво просимо до нашого ресурсу!", user.Name);
                    return RedirectToAction("Index", "Default", new { ConfirmedEmail = user.Email });
                }
                else
                {
                    return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { Email = "" });
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    if (user.EmailConfirmed == true)
                    {
                        ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);
                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Default");
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ви не завершили реєстрацію, будь ласка підтвердіть свій поштовий адрес.");
                    }
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index","Default");
        }
        [HttpGet]
        [Authorize(Roles = "Користувач")]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Користувач")]
        public async Task<ActionResult> DeleteConfirmed()
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Logout", "Account");
                }
            }
            return RedirectToAction("List", "Test");
        }
        [Authorize(Roles = "Користувач")]
        public async Task<ActionResult> Edit()
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                EditModel model = new EditModel { Name = user.Name };
                return View(model);
            }
            return RedirectToAction("Index", "Default");
        }
        [HttpPost]
        [Authorize(Roles = "Користувач")]
        public async Task<ActionResult> Edit(EditModel model)
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                user.Name = model.Name;
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["message"] = string.Format("Зміни збережені");
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return View(model);
        }
        [Authorize(Roles = "Користувач")]
        public async Task<ActionResult> Index()
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);
            string roles = null;
            foreach (var a in user.Roles)
            {
                var role = RoleManager.Roles.First(x => x.Id == a.RoleId);
                roles += role.Name + " ";
            }
            UserInfoModel userInfo = new UserInfoModel
            {
                Name = user.Name,
                Email = user.Email,
                Role = roles,
                EmailConfirmed=user.EmailConfirmed,
                Stats = statsRepository.Stats.Where(x=>x.userId==user.Id)
            };
            return View(userInfo);
        }
        [Authorize(Roles = "admin")]
        public ActionResult UsersIndex()
        {
            List<UserIndexModel> list = new List<UserIndexModel>();
            List<ApplicationUser> users = UserManager.Users.ToList();
            foreach (var a in users)
            {
                string roles = null;
                foreach (var b in a.Roles)
                {
                    var role = RoleManager.Roles.First(x => x.Id == b.RoleId);
                    roles += role.Name + " ";
                }
                UserIndexModel userIndex = new UserIndexModel { Name = a.Name, Email = a.Email, Role = roles, Id = a.Id };
                list.Add(userIndex);
            }
            return View(list);
        }
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddAdminRole(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var Role = RoleManager.Roles.First(x => x.Name == "admin");
                if (Role != null)
                {
                    UserManager.AddToRole(user.Id, Role.Name);
                    TempData["message"] = string.Format("Пользователь \"{0}\" теперь имеет права администратора", user.Email);
                    return RedirectToAction("UsersIndex");
                }
                else
                {
                    TempData["error"] = string.Format("Роль неверно указана, проверте данные в разделе ролей");
                    return RedirectToAction("UsersIndex");
                }
            }
            else 
            {
                TempData["error"] = string.Format("Возникла ошибка \"{0}\"",id);
                return RedirectToAction("UsersIndex");
            }
        }
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> RemoveAdminRole(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user.Email != User.Identity.Name)
            {
                if (user != null)
                {
                    var Role = RoleManager.Roles.First(x => x.Name == "admin");
                    if (Role != null)
                    {
                        UserManager.RemoveFromRole(id, Role.Name);
                        TempData["message"] = string.Format("Пользователь \"{0}\" теперь не имеет прав администратора", user.Email);
                       
                    }
                    else
                    {
                        TempData["error"] = string.Format("Роль неверно указана, проверте данные в разделе ролей", user.Email);
                        
                    }
                }
                else
                {
                    TempData["error"] = string.Format("Возникла ошибка \"{0}\"", id);
                    
                }
            }
            else
            {
                TempData["error"] = string.Format("Нельзя заблокировать себя");
            }
            return RedirectToAction("UsersIndex");

        }
        [Authorize(Roles = "admin")]
        public ActionResult BlockUser(string id)
        {
            ApplicationUser user = UserManager.FindById(id);
            if (user.Email != User.Identity.Name)
            {
                IList<string> roles = UserManager.GetRoles(id);
                foreach (var a in roles)
                {
                    UserManager.RemoveFromRole(id, a);
                }
                var Role = RoleManager.Roles.First(x => x.Name == "blocked");
                if (Role != null)
                {
                    UserManager.RemoveFromRole(user.Id, Role.Name);
                    TempData["message"] = string.Format("Пользователь \"{0}\" заблокирован", user.Email);
                    
                }
                else
                {
                    TempData["error"] = string.Format("Роль неверно указана, проверте данные в разделе ролей", user.Email);
                    
                }
            }
            else 
            {
            TempData["error"] = string.Format("Нельзя заблокировать себя");
            }
            return RedirectToAction("UsersIndex");
        }
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> EditByAdmin(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(new EditModel { Id = user.Id, Name = user.Name });
            }
            
            return RedirectToAction("UsersIndex");
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> EditByAdmin(EditModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
                if (user != null)
                {

                    user.Name = model.Name;
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["message"] = string.Format("Изменения у пользователя \"{0}\" были сохранены", user.Email);
                        return RedirectToAction("UsersIndex");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
            }
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteByAdmin(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                if (user.Email != User.Identity.Name)
                {
                    IdentityResult result = await UserManager.DeleteAsync(user);
                    TempData["message"] = string.Format("Пользователь \"{0}\" был удален", user.Email);
                }
                else
                {
                    TempData["error"] = string.Format("Нельзя удалить себя");
                }
            }
            return RedirectToAction("UsersIndex");
        }
    }
}