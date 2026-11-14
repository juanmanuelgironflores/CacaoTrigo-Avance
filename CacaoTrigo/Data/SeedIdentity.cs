using CacaoTrigo.Models;
using Microsoft.AspNetCore.Identity;

namespace CacaoTrigo.Data;

public static class SeedIdentity
{
    /// <summary>
    /// Crea los roles y, si todavía no existe ningún administrador,
    /// el administrador inicial definido en appsettings ("AdministradorInicial").
    /// </summary>
    public static async Task InicializarAsync(IServiceProvider servicios, IConfiguration configuracion)
    {
        var roles = servicios.GetRequiredService<RoleManager<IdentityRole>>();
        var usuarios = servicios.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var rol in new[] { RolesApp.Administrador, RolesApp.Cliente })
        {
            if (!await roles.RoleExistsAsync(rol))
            {
                await roles.CreateAsync(new IdentityRole(rol));
            }
        }

        if ((await usuarios.GetUsersInRoleAsync(RolesApp.Administrador)).Any())
        {
            return;
        }

        var correo = configuracion["AdministradorInicial:Correo"];
        var contrasena = configuracion["AdministradorInicial:Contrasena"];
        var nombre = configuracion["AdministradorInicial:Nombre"] ?? "Administrador";

        if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
        {
            return;
        }

        var admin = await usuarios.FindByEmailAsync(correo);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = correo,
                Email = correo,
                EmailConfirmed = true,
                NombreCompleto = nombre
            };

            var creado = await usuarios.CreateAsync(admin, contrasena);

            if (!creado.Succeeded)
            {
                throw new InvalidOperationException(
                    "No se pudo crear el administrador inicial: " +
                    string.Join(" ", creado.Errors.Select(e => e.Description)));
            }
        }

        await usuarios.AddToRoleAsync(admin, RolesApp.Administrador);
    }
}
