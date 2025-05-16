using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser(UserDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                CreationDate = DateTime.Now,
                
            };
            var result = await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
            {
                return StatusCode(201);
            }

            return BadRequest(result.Errors);
        }

        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return BadRequest(new {message = "Kullanıcı bulunamadı" });
            }

            var result= await _signInManager.CheckPasswordSignInAsync(user, model.Password,true);
            if (result.Succeeded)
            {
                return Ok(new {token="token"});
            }
            return Unauthorized();
        }

    }

}
