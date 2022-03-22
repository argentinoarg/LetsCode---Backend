using BACK.Data;
using BACK.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly KanbanDbContext _context;

        public LoginController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings, KanbanDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _context = context;

            var _task = CriarUsuarioPadrao();
        }

        private async Task CriarUsuarioPadrao()
        {
            var user = new IdentityUser
            {
                UserName = Environment.GetEnvironmentVariable("LETSCODE_USER"),
                Email = Environment.GetEnvironmentVariable("LETSCODE_USER"),
                EmailConfirmed = true,
                PasswordHash = Environment.GetEnvironmentVariable("LETSCODE_PASS")
            };

            await _userManager.CreateAsync(user, Environment.GetEnvironmentVariable("LETSCODE_PASS"));
            await _signInManager.SignInAsync(user, isPersistent: true);

            if (_context.Users.FirstOrDefault(x => x.UserName == user.UserName) == null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
            }
        }

        [HttpPost]
        public async Task<ActionResult> VerificarLogin(LoginModel loginModel)
        { 
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var user = _context.Users.FirstOrDefault(x => x.UserName == loginModel.Login);

            // validate
            if (user == null || (loginModel.Password != user.PasswordHash))
                return BadRequest("Login ou senha inválidos.");

            return Ok(await GerarJwt(loginModel.Login, loginModel.Password));
        }

        private async Task<string> GerarJwt(string usuario, string senha)
        {
            var user = await _userManager.FindByNameAsync(usuario);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            IDictionary<string, object> _claims = new Dictionary<string, object>();
            _claims.Add("login", usuario);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = _claims,
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_appSettings.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
