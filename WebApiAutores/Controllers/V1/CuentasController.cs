using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAutores.DTO;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly HashService hashService;
        private readonly IDataProtector dataProtector;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            SignInManager<IdentityUser> signInManager, IDataProtectionProvider dataProtectionProvider, HashService hashService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.hashService = hashService;
            dataProtector = dataProtectionProvider.CreateProtector("valor_unico");
        }

        //[HttpGet("hash/{textoplano}")]
        //public ActionResult RealizarHash(string textoplano)
        //{
        //    var resultado1 = hashService.Hash(textoplano);
        //    var resultado2 = hashService.Hash(textoplano);

        //    return Ok (new
        //    {

        //        textoplano= textoplano,
        //        hash1 = resultado1,
        //        hash2 = resultado2
        //      });  
        //}


        //[HttpGet("Encriptar")]
        //public ActionResult Encriptar() {
        //    var textoplano = "Niara Emes";
        //    var textocifrado = dataProtector.Protect (textoplano);  
        //    var textoDesifrado = dataProtector .Unprotect (textocifrado);

        //    return Ok(new
        //    {
        //        textoplano = textoplano,
        //        textocifrado = textocifrado,
        //        textoDesifrado = textoDesifrado 
        //    });

        //}

        //[HttpGet("EncriptarporTiempo")]
        //public ActionResult EncriptarporTiempo()
        //{
        //    var encriptartiempoLimitado = dataProtector.ToTimeLimitedDataProtector();
        //    var textoplano = "Niara Emes";
        //    var textocifrado = encriptartiempoLimitado.Protect(textoplano, lifetime :TimeSpan.FromSeconds(5));

        //    Thread .Sleep (6000); 
        //    var textoDesifrado = encriptartiempoLimitado.Unprotect(textocifrado);

        //    return Ok(new
        //    {
        //        textoplano = textoplano,
        //        textocifrado = textocifrado,
        //        textoDesifrado = textoDesifrado
        //    });

        //}

        [HttpPost("registar", Name = "RegistarUsuario")] //api/ceuntas/registar
        public async Task<ActionResult<RespuestaAutenticacion>> Registar(CredencialesUsuario credencialesUsuario)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuario.Email,
                Email = credencialesUsuario.Email
            };
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Pasword);

            if (resultado.Succeeded)
            {
                return await ContruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }

        }

        [HttpPost("login", Name = "")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email,
                credencialesUsuario.Pasword, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ContruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }

        }
        [HttpGet("RenovarToken", Name = "RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacion>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var credencialesusuario = new CredencialesUsuario()
            {
                Email = email,
            };
            return await ContruirToken(credencialesusuario);
        }

        private async Task<RespuestaAutenticacion> ContruirToken(CredencialesUsuario credencialesUsuario)
        {
            //los cliam son informacion de usuario en la cual podemos confiar, los usuarios tambien la piueden ver por lo que no se puede poner inforemcacion comprometida
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario .Email ),
                new Claim("prueba", "esto e suna prueba")
            };
            var usuario = await userManager.FindByEmailAsync(credencialesUsuario.Email);
            var claimDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));

            var cred = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: cred);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };
        }
        [HttpPost("HacerAdmin", Name = "HacerAdmin")]
        public async Task<ActionResult> HcerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);

            await userManager.AddClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

        [HttpPost("RemoveAdmin", Name = "RemoverAdmin")]
        public async Task<ActionResult> RemoveAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);

            await userManager.RemoveClaimAsync(usuario, new Claim("esAdmin", "1"));
            return NoContent();
        }

    }
}
