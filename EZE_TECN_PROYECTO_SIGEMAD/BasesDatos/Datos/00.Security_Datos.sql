SET QUOTED_IDENTIFIER ON;

-- ====================================================================
-- ROLES
-- ====================================================================
-- Insertar el rol de Administrador
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('E6248C8C-C043-4D11-B711-A4F8E34A940E', 'Administrador', 'ADMINISTRADOR', NULL);

-- Insertar el rol de Usuario Interno
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('FDAD9EC8-0720-4C10-9A07-739D20D1E42B', 'UsuarioInterno', 'USUARIOINTERNO', NULL);

-- Insertar el rol de Usuario Externo
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES ('FC99E2FD-8544-4CDC-8B52-17C75DB077E1', 'UsuarioExterno', 'USUARIOEXTERNO', NULL);

-- select * from AspNetRoles


-- ====================================================================
-- USUARIOS
-- ====================================================================

-- Insertar un usuario administrador
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('B3C14CF0-58C8-4781-94D0-F9EAB4296C7A', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECaWXRR0KIKEpE++JFanlo7bjiotVXQTzrloTEa6vmD1u9CNvAElAo49x4zpuwSDgQ==', NULL, NULL, 0, 0, 0, 0);

-- Insertar un usuario interno
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('C596AD0C-17D1-48D0-B08B-F635B83D14C2', 'interno@example.com', 'INTERNO@EXAMPLE.COM', 'interno@example.com', 'INTERNO@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECaWXRR0KIKEpE++JFanlo7bjiotVXQTzrloTEa6vmD1u9CNvAElAo49x4zpuwSDgQ==',  NULL, NULL, 0, 0, 0, 0);

-- Insertar un usuario externo
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
VALUES ('FB161AB0-96C6-4549-9395-2C1EC5FD9B46', 'externo@example.com', 'EXTERNO@EXAMPLE.COM', 'externo@example.com', 'EXTERNO@EXAMPLE.COM', 1, 'AQAAAAEAACcQAAAAECaWXRR0KIKEpE++JFanlo7bjiotVXQTzrloTEa6vmD1u9CNvAElAo49x4zpuwSDgQ==',  NULL, NULL, 0, 0, 0, 0);

-- ============================
-- Para tabla ApplicationUsers
-- ============================

-- Insertar un usuario administrador en la tabla ApplicationUsers
INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
VALUES ('AC41965C-7F9E-48C0-BBC1-C4A0487DFB2D', 'B3C14CF0-58C8-4781-94D0-F9EAB4296C7A', 'Admin', 'Administrador', 'admin@example.com', '555-1234', GETDATE(), NULL, NULL, NULL, 0);

-- Insertar un usuario interno en la tabla ApplicationUsers
INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
VALUES ('550E683E-0458-43E8-A6E6-20887DC2BDDD', 'C596AD0C-17D1-48D0-B08B-F635B83D14C2', 'Interno', 'Empleado', 'interno@example.com', '555-5678', GETDATE(), NULL, NULL, NULL, 0);

-- Insertar un usuario externo en la tabla ApplicationUsers
INSERT INTO ApplicationUsers (Id, IdentityId, Nombre, Apellidos, Email, Telefono, FechaCreacion, CreadoPor, FechaModificacion, ModificadoPor, Borrado)
VALUES ('D3813C04-4EEE-4D37-84B7-49EC293F92D2', 'FB161AB0-96C6-4549-9395-2C1EC5FD9B46', 'Externo', 'Proveedor', 'externo@example.com', '555-9876', GETDATE(), NULL, NULL, NULL, 0);


-- ====================================================================
-- USUARIOS - ROLES
-- ====================================================================

-- Obtener el Id del rol Administrador
DECLARE @RoleIdAdministrador NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'ADMINISTRADOR');

-- Obtener el Id del usuario administrador
DECLARE @UserIdAdministrador NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'ADMIN@EXAMPLE.COM');

-- Asignar el rol Administrador al usuario administrador
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserIdAdministrador, @RoleIdAdministrador);


-- Obtener el Id del rol UsuarioInterno
DECLARE @RoleIdInterno NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'USUARIOINTERNO');

-- Obtener el Id del usuario interno
DECLARE @UserIdInterno NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'INTERNO@EXAMPLE.COM');

-- Asignar el rol Usuario Interno al usuario
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserIdInterno, @RoleIdInterno);


-- Obtener el Id del rol UsuarioExterno
DECLARE @RoleIdExterno NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE NormalizedName = 'USUARIOEXTERNO');

-- Obtener el Id del usuario externo
DECLARE @UserIdExterno NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE NormalizedUserName = 'EXTERNO@EXAMPLE.COM');

-- Asignar el rol Usuario Externo al usuario
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserIdExterno, @RoleIdExterno);


--select NEWID()