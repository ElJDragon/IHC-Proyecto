# 🐘 Guía de Instalación y Configuración de PostgreSQL

## 📋 Requisitos
- Windows 10/11
- PowerShell
- 500 MB de espacio en disco

---

## 🚀 Paso 1: Descargar e Instalar PostgreSQL

### Opción A: Instalador Oficial (Recomendado)

1. **Descargar PostgreSQL**
   - Ve a: https://www.postgresql.org/download/windows/
   - Descarga el instalador para Windows (versión 15 o superior)

2. **Ejecutar el Instalador**
   - Ejecuta el archivo `.exe` descargado
   - Sigue el asistente de instalación:
     - **Componentes**: Selecciona PostgreSQL Server, pgAdmin 4, Command Line Tools
     - **Directorio**: Mantén el predeterminado (`C:\Program Files\PostgreSQL\15`)
     - **Contraseña**: Establece la contraseña del superusuario `postgres` (ejemplo: `postgres`)
     - **Puerto**: Mantén el puerto predeterminado `5432`
     - **Locale**: Selecciona tu configuración regional

3. **Verificar la Instalación**
   Abre PowerShell y ejecuta:
   ```powershell
   psql --version
   ```
   Deberías ver algo como: `psql (PostgreSQL) 15.x`

### Opción B: Usando Chocolatey (Avanzado)

Si tienes Chocolatey instalado:
```powershell
choco install postgresql
```

---

## 🔧 Paso 2: Configurar Variables de Entorno (Si es necesario)

Si el comando `psql` no funciona, agrega PostgreSQL al PATH:

1. Abre **Configuración del Sistema** → **Variables de entorno**
2. En **Variables del sistema**, selecciona `Path` → **Editar**
3. Agrega la ruta: `C:\Program Files\PostgreSQL\15\bin`
4. **Reinicia PowerShell**

---

## 🗄️ Paso 3: Crear la Base de Datos

### Método 1: Usando pgAdmin 4 (Interfaz Gráfica)

1. Abre **pgAdmin 4** desde el menú de inicio
2. Conéctate al servidor local (usa la contraseña que estableciste)
3. Click derecho en **Databases** → **Create** → **Database**
4. Nombre: `GestionIncidentesDb`
5. Click **Save**

### Método 2: Usando PowerShell (Línea de Comandos)

```powershell
# Conectarse a PostgreSQL como superusuario
psql -U postgres

# Dentro de psql, ejecuta:
CREATE DATABASE "GestionIncidentesDb";

# Verificar que se creó
\l

# Salir
\q
```

---

## 📊 Paso 4: Ejecutar el Script SQL

Ahora ejecuta el script de creación de tablas:

```powershell
# Navega al directorio del proyecto
cd C:\IHC-Proyecto

# Ejecuta el script SQL
psql -U postgres -d GestionIncidentesDb -f GestionIncidentes.Infrastructure\Scripts\init-database.sql
```

**Contraseña**: Cuando se solicite, ingresa la contraseña que estableciste para el usuario `postgres`.

---

## ✅ Paso 5: Verificar la Instalación

Conéctate a la base de datos y verifica las tablas:

```powershell
psql -U postgres -d GestionIncidentesDb
```

Dentro de `psql`, ejecuta:

```sql
-- Ver todas las tablas
\dt

-- Consultar datos de ejemplo
SELECT * FROM "Users";
SELECT * FROM "Departments";
SELECT * FROM "Roles";

-- Salir
\q
```

---

## 🔗 Paso 6: Verificar la Cadena de Conexión

Tu cadena de conexión en `appsettings.json` debe ser:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=GestionIncidentesDb;Username=postgres;Password=postgres"
  }
}
```

**IMPORTANTE**: Cambia `Password=postgres` por la contraseña que estableciste.

---

## 🏃 Paso 7: Ejecutar Migraciones de Entity Framework (Opcional)

Si prefieres usar migraciones de EF Core en lugar del script SQL:

```powershell
# Instalar EF Core CLI (si no está instalado)
dotnet tool install --global dotnet-ef

# Navega al proyecto Infrastructure
cd GestionIncidentes.Infrastructure

# Crear migración inicial
dotnet ef migrations add InitialCreate --startup-project ..\GestionIncidentes.Web

# Aplicar migración
dotnet ef database update --startup-project ..\GestionIncidentes.Web
```

---

## 🧪 Paso 8: Probar la Conexión desde la Aplicación

```powershell
# Navega al proyecto Web
cd C:\IHC-Proyecto\GestionIncidentes.Web

# Ejecuta la aplicación
dotnet run
```

La aplicación debería conectarse exitosamente a PostgreSQL.

---

## 🐛 Solución de Problemas Comunes

### Problema: "psql: command not found"
**Solución**: Agrega PostgreSQL al PATH (ver Paso 2).

### Problema: "password authentication failed"
**Solución**: Verifica la contraseña en `appsettings.json` y `GestionIncidentesDbContextFactory.cs`.

### Problema: "database does not exist"
**Solución**: Crea la base de datos usando pgAdmin o el comando `CREATE DATABASE`.

### Problema: Puerto 5432 en uso
**Solución**: 
```powershell
# Ver qué proceso usa el puerto
netstat -ano | findstr :5432

# Detener el servicio PostgreSQL
Stop-Service postgresql-x64-15

# Reiniciar el servicio
Start-Service postgresql-x64-15
```

---

## 📚 Recursos Adicionales

- **Documentación Oficial**: https://www.postgresql.org/docs/
- **pgAdmin 4**: https://www.pgadmin.org/
- **Tutorial PostgreSQL**: https://www.postgresqltutorial.com/

---

## 🎯 Próximos Pasos

Una vez que PostgreSQL esté funcionando:

1. ✅ Ejecuta el script SQL para crear las tablas
2. ✅ Verifica que los datos de ejemplo se hayan insertado
3. ✅ Ejecuta la aplicación con `dotnet run`
4. ✅ Prueba los endpoints de la API con Postman
5. ✅ Navega a las páginas Razor en el navegador

---

¡Listo! Ahora tienes PostgreSQL configurado correctamente. 🎉
