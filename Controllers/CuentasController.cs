using CursoIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CursoIdentity.Controllers
{
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailSender emailsender;

        public CuentasController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailsender
            )
        {
            _userManager = userManager;
            this.signInManager = signInManager;
            this.emailsender = emailsender;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro(string returnurl = null)
        {
            ViewData["RetrunUrl"] = returnurl;
            RegistroViewModel registroVM = new();
            return View(registroVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel registroViewModel, string returnurl = null)
        {
            ViewData["RetrunUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var usuario = new AppUsuario
                {
                    UserName = registroViewModel.Email,
                    Email = registroViewModel.Email,
                    Nombre = registroViewModel.Nombre,
                    Url = registroViewModel.Url,
                    CodigoPais = registroViewModel.CodigoPais,
                    Telefono = registroViewModel.Telefono,
                    Pais = registroViewModel.Pais,
                    Ciudad = registroViewModel.Ciudad,
                    Direccion = registroViewModel.Direccion,
                    FechaNacimiento = registroViewModel.FechaNacimiento,
                    Estado = registroViewModel.Estado
                };

                var resultado = await _userManager.CreateAsync(usuario, registroViewModel.Password);

                if (resultado.Succeeded)
                {
                    //Implementación de confirmación de email en el registro
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
                    var urlRetorno = Url.Action("ConfirmarEmail", "Cuentas",
                    new { userId = usuario.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await emailsender.SendEmailAsync(registroViewModel.Email, "Confirmar su Cuenta - NeighborFood",
                   "Por favor confirme su cuenta dando click aquí: <a href=\"" + urlRetorno + "\">enlace</a>");

                    await signInManager.SignInAsync(usuario, isPersistent: false);

                    return LocalRedirect(returnurl);
                }
                ValidarErrores(resultado);
            }

            return View(registroViewModel);
        }

        //Metodo mostrar formulario de acceso
        [HttpGet]
        public IActionResult Acceso(string returnurl = null)
        {
            ViewData["RetrunUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acceso(AccesoViewModel accesoViewModel, string returnurl = null)
        {
            ViewData["RetrunUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var resultado = await signInManager.PasswordSignInAsync(accesoViewModel.Email, accesoViewModel.Password, accesoViewModel.RememberMe,

                lockoutOnFailure: true);

                if (resultado.Succeeded)
                {
                    return LocalRedirect(returnurl);
                }
                if (resultado.IsLockedOut)
                {
                    return View("Bloqueado");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Acceso Invalido");
                    return View(accesoViewModel);
                }
            }

            return View(accesoViewModel);
        }

        //Manejador de Errores

        private void ValidarErrores(IdentityResult resultado)
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(String.Empty, error.Description);
            }
        }
        //salir o cerrar sesión method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalirAplication()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //metodo par olvido de contraseña

        [HttpGet]
        public IActionResult OlvidoPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OlvidoPassword(OlvidoPasswordViewModel opViewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(opViewModel.Email);
                if (usuario == null)
                {
                    return RedirectToAction("ConfirmacionOlvidoPassword");
                }

                var codigo = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                var urlRetorno = Url.Action("ResetPassword", "Cuentas",
                    new { userId = usuario.Id, code = codigo }, protocol: HttpContext.Request.Scheme);
                await emailsender.SendEmailAsync(opViewModel.Email, "Recuperar contraseña - NeighborFood",
                    "Por favor recupere su contraseña dando click aquí: <a href=\"" + urlRetorno + "\">enlace</a>");

                return RedirectToAction("ConfirmacionOlvidoPassword");
            }

            return View(opViewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmacionOlvidoPassword()
        {
            return View();
        }

        //funcion para recuperar contraseña
        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(RecuperaPasswordViewModel recupera)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(recupera.Email);
                if (usuario == null)
                {
                    return RedirectToAction("ConfirmacionRecuperaPassword");
                }
                var resultado = await _userManager.ResetPasswordAsync(usuario, recupera.Code, recupera.Password);
                if (resultado.Succeeded)
                {
                    return RedirectToAction("ConfirmacionRecuperaPassword");
                }
                ValidarErrores(resultado);
            }

            return View(recupera);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmacionRecuperaPassword()
        {
            return View();
        }

        //metodo para confirmacion de email
        [HttpGet]
        public async Task<IActionResult> ConfirmarEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null)
            {
                return View("Error");
            }
            var resultado = await _userManager.ConfirmEmailAsync(usuario, code);

            return View(resultado.Succeeded ? "ConfirmarEmail" : "Error");
        }

        //Configuración acceso externo : facebook, google, twitter, etc
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult AccesoExterno(string proveedor, string retunrurl = null)
        {
            var urlRedireccion = Url.Action("AccesoExternoCallback", "Cuentas",
                    new { ReturnUrl = retunrurl },
                    protocol: HttpContext.Request.Scheme);

            var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            return Challenge(propiedades, proveedor);

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AccesoExternoCallback(string returnurl = null, string error = null)
        {
            returnurl = returnurl ?? Url.Content("~/");
            if(error != null)
            {
                ModelState.AddModelError(string.Empty, $"Error en el acceso externo {error}");
                return View(nameof(Acceso));

            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if(info == null)
            {
                return RedirectToAction(nameof(Acceso));
            }

            //Acceder con el usuario en el proveedor externo

            var resultado = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (resultado.Succeeded)
            {
                //Actualizar los tokens de acceso
                await signInManager.UpdateExternalAuthenticationTokensAsync(info);
                return LocalRedirect(returnurl);
            }
            else
            {
                //Sie el usuario no tiene cuenta, pregunta si quiere crear una
                ViewData["ReturnUrl"] = returnurl;
                ViewData["NombreAMostrarProveedor"] = info.ProviderDisplayName;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var nombre = info.Principal.FindFirstValue(ClaimTypes.Name);
                return View("ConfirmacionAccesoExterno", new ConfirmacionAccesoExterno { Email = email, Name = nombre});
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmacionAccesoExterno(ConfirmacionAccesoExterno caeViewModel, string returnurl = null)
        {
            returnurl = returnurl??Url.Content("~/");

            if (ModelState.IsValid)
            {
                //obtener la información del usuario del proveedor externo
                var info = await signInManager.GetExternalLoginInfoAsync();
                if(info == null)
                {
                    return View("Error");
                }

                var usuario = new AppUsuario { UserName = caeViewModel.Email, Email = caeViewModel.Email, Nombre = caeViewModel.Name };
                var resultado = await _userManager.CreateAsync(usuario);
                if (resultado.Succeeded)
                {
                    resultado = await _userManager.AddLoginAsync(usuario, info);
                    if (resultado.Succeeded)
                    {
                        await signInManager.SignInAsync(usuario, isPersistent: false);
                        await signInManager.UpdateExternalAuthenticationTokensAsync(info);
                        return LocalRedirect(returnurl);
                    }
                }

                ValidarErrores(resultado);
            }
            ViewData["ReturnUrl"] = returnurl;
            return View(caeViewModel);
        }


    }
}
