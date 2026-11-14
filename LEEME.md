# Cacao y Trigo

## Primer arranque
Abre `CacaoTrigo.slnx` en Visual Studio y ejecuta el proyecto (F5).
Al iniciar, la app **actualiza la base de datos sola** y crea los roles
(`Administrador`, `Cliente`) y un administrador por defecto:

| Dato        | Valor                     |
|-------------|---------------------------|
| Correo      | `admin@cacaoytrigo.com`   |
| Contraseña  | `Admin123!`               |

Entra en `/Cuenta/Login`. Cambia esa contraseña desde el menú de tu usuario
(**Cambiar contraseña**) y, si quieres otro correo/contraseña por defecto,
edita la sección `AdministradorInicial` de `appsettings.json` antes del primer arranque.

## Roles
- **Administrador**: catálogo, mesas, reservas, pedidos, promociones y usuarios.
  Solo un administrador puede crear otros administradores.
- **Cliente**: se registra desde "Crear cuenta". Ve el menú, hace pedidos,
  reserva mesa (terraza o interior, fecha y hora) y ve su historial.

## Imágenes de productos
En Productos > Crear/Editar se sube un archivo (PNG, JPG/JPEG, WebP, GIF o SVG, máx. 5 MB).
Se guardan en `wwwroot/uploads/productos/`.
