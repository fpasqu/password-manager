using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManagerAspNet.Models.Entities;
using PasswordManagerAspNet.Models.Repositories;

namespace PasswordManagerAspNet.Controllers
{
    [Authorize]
    public class PasswordController : Controller
    {
        private readonly IPasswordRepository _passwordsRepository;
        private readonly IFunctions _functions;

        public PasswordController(IPasswordRepository passwordsRepository, IFunctions functions)
        {
            _passwordsRepository = passwordsRepository;
            _functions = functions;
        }

        //get
        [HttpGet]
        public async Task<IActionResult> List()
        {            
            string? userEmail = _functions.GetCurrentUserEmail(User);
            var encryptedUserMail = _functions.Encrypt(userEmail, userEmail);
            var passwordsList = await _passwordsRepository.GetPasswordsForUserAsync(encryptedUserMail);
            var decryptedPasswordsList = new List<Password>();
            foreach (var pass in passwordsList)
            {
                decryptedPasswordsList.Add(_functions.EncryptDecryptPassword(pass, userEmail, false));
            }

            return View(decryptedPasswordsList);
        }

        //create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Password p)
        {
            string? userEmail = _functions.GetCurrentUserEmail(User);
            p.UserMail = userEmail;
            if (!ModelState.IsValid)
                return View(p);
            

            var encryptedModel = _functions.EncryptDecryptPassword(p, userEmail, true);
            await _passwordsRepository.CreatePasswordAsync(encryptedModel);
            return RedirectToAction("List");
        }

        //edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            string? userEmail = _functions.GetCurrentUserEmail(User);
            var model = await _passwordsRepository.GetPasswordByIdAsync(id);
            var decryptedModel = _functions.EncryptDecryptPassword(model, userEmail, false);
            return View(decryptedModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Password p)
        {
            string? userEmail = _functions.GetCurrentUserEmail(User);
            if (!ModelState.IsValid)
                return View(p);
            var encryptedPassword = _functions.EncryptDecryptPassword(p, userEmail, true);
            await _passwordsRepository.UpdatePasswordAsync(encryptedPassword);
            return RedirectToAction("List");
        }

        //delete
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            string? userEmail = _functions.GetCurrentUserEmail(User);
            var model = await _passwordsRepository.GetPasswordByIdAsync(id);
            var decryptedModel = _functions.EncryptDecryptPassword(model, userEmail, false);
            return View(decryptedModel);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Password p)
        {
            await _passwordsRepository.DeletePasswordAsync(p);
            return RedirectToAction("List");
        }

        //generator
        [HttpGet]
        public IActionResult Generator()
        {
            return View();
        }
    }
}
