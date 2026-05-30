CREATE DATABASE db_domicilios
GO
USE db_domicilios
GO

CREATE TABLE [Vehiculos] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Placa] NVARCHAR(10) NOT NULL,
	[Tipo] NVARCHAR(20) NOT NULL,
	[Modelo] NVARCHAR(50) NOT NULL,
	[Activo] BIT NOT NULL,
);

CREATE TABLE [Zonas] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Nombre] NVARCHAR(50) NOT NULL,
	[Tarifa] DECIMAL(10, 2) NOT NULL,
);

CREATE TABLE [Roles] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Nombre] NVARCHAR(50) NOT NULL,
	[Descripcion] NVARCHAR(200) NOT NULL,
);

CREATE TABLE [MetodoPago] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Nombre] NVARCHAR(50) NOT NULL,
	[Descripcion] NVARCHAR(200) NOT NULL,
	[Comision] DECIMAL(10, 2) NOT NULL,
	[Activo] BIT NOT NULL,
);

CREATE TABLE [Usuarios] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Cedula] NVARCHAR(50) NOT NULL,
	[Nombre] NVARCHAR(200) NOT NULL,
	[Email] NVARCHAR(300) NOT NULL,
	[Contrasena] NVARCHAR(100) NOT NULL,
	[FechaNacimiento] SMALLDATETIME NOT NULL,
	[Rol] INT NOT NULL REFERENCES [Roles]([Id]),
);

CREATE TABLE [Clientes] (
	[Id] INT NOT NULL PRIMARY KEY REFERENCES [Usuarios]([Id]),
	[Direccion] NVARCHAR(200) NOT NULL,
	[Telefono] NVARCHAR(100) NOT NULL,
	[FechaRegistro] SMALLDATETIME NOT NULL,
	[MetodoPagoFav] INT NOT NULL REFERENCES [MetodoPago]([Id]),
	[Activo] BIT NOT NULL,
);

CREATE TABLE [Repartidores] (
	[Id] INT NOT NULL PRIMARY KEY REFERENCES [Usuarios]([Id]),
	[VehiculoId] INT NOT NULL REFERENCES [Vehiculos]([Id]),
	[ZonaId] INT NOT NULL REFERENCES [Zonas]([Id]),
	[Disponible] BIT NOT NULL,
	[CalificacionPromedio] DECIMAL(10, 2) NOT NULL,
	[Activo] BIT NOT NULL,
);

CREATE TABLE [EstadoPedido] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Nombre] NVARCHAR(50) NOT NULL,
	[Descripcion] NVARCHAR(200) NOT NULL,
	[Notificar] BIT NOT NULL,
	[Activo] BIT NOT NULL,
);

CREATE TABLE [Pedidos] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[ClienteId] INT NOT NULL REFERENCES [Clientes]([Id]),
	[RepartidorId] INT NOT NULL REFERENCES [Repartidores]([Id]),
	[EstadoPedidoId] INT NOT NULL REFERENCES [EstadoPedido]([Id]),
	[FechaCreacion] SMALLDATETIME NOT NULL,
	[Total] DECIMAL(10, 2) NOT NULL,
);

CREATE TABLE [Categorias] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Nombre] NVARCHAR(50) NOT NULL,
	[Descripcion] NVARCHAR(200) NOT NULL,
	[Creacion] SMALLDATETIME NOT NULL,
	[Activo] BIT NOT NULL,
);

CREATE TABLE [Productos] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Nombre] NVARCHAR(50) NOT NULL,
	[Descripcion] NVARCHAR(200) NOT NULL,
	[Precio] DECIMAL(10,2) NOT NULL,
	[Stock] INT NOT NULL,
	[Urlimagen] NVARCHAR(100) NOT NULL,
	[CategoriaId] INT NOT NULL REFERENCES [Categorias]([Id]),
);

CREATE TABLE [DetallePedido] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[PedidoId] INT NOT NULL REFERENCES [Pedidos]([Id]),
	[ProductoId] INT NOT NULL REFERENCES [Productos]([Id]),
	[Cantidad] INT NOT NULL,
	[PrecioUnitario] DECIMAL(10,2) NOT NULL,
	[Subtotal] DECIMAL(10,2) NOT NULL,
);

CREATE TABLE [EstadoPago] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Nombre] NVARCHAR(50) NOT NULL,
	[Activo] BIT NOT NULL,
);
	
CREATE TABLE [Pagos] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[PedidoId] INT NOT NULL REFERENCES [Pedidos]([Id]),
	[MetodoPagoId] INT NOT NULL REFERENCES [MetodoPago]([Id]),
	[EstadoPagoId] INT NOT NULL REFERENCES [EstadoPago]([Id]),
	[Monto] DECIMAL(10,2) NOT NULL,
	[FechaPago] SMALLDATETIME NOT NULL,
	[Comision] DECIMAL(10,2) NOT NULL,
);

CREATE TABLE [Facturas] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[PedidoId] INT NOT NULL REFERENCES [Pedidos]([Id]),
	[PagoId] INT NOT NULL REFERENCES [Pagos]([Id]),
	[FechaEmision] SMALLDATETIME NOT NULL,
	[Impuesto] DECIMAL(10,2) NOT NULL,
	[Total] DECIMAL(10,2) NOT NULL
);

CREATE TABLE [Calificaciones] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[PedidoId] INT NOT NULL REFERENCES [Pedidos]([Id]),
	[Puntaje] INT NOT NULL,
	[Comentario] NVARCHAR(200) NOT NULL,
	[Fecha] SMALLDATETIME NOT NULL,
);

CREATE TABLE [RutaEntrega] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[PedidoId] INT NOT NULL REFERENCES [Pedidos]([Id]),
	[DistanciaKM] DECIMAL(10,2) NOT NULL,
	[TiempoEstimado] DECIMAL(10,2) NOT NULL,
	[Fecha] SMALLDATETIME NOT NULL,
);

CREATE TABLE [RastreoPedido] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[PedidoId] INT NOT NULL REFERENCES [Pedidos]([Id]),
	[Latitud] DECIMAL(10,2) NOT NULL,
	[Longitud] DECIMAL(10,2) NOT NULL,
	[FechaActualizacion] SMALLDATETIME NOT NULL,
);

CREATE TABLE [RastreoRepartidor] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[RepartidorId] INT NOT NULL REFERENCES [Repartidores]([Id]),
	[Latitud] DECIMAL(10,2) NOT NULL,
	[Longitud] DECIMAL(10,2) NOT NULL,
	[FechaActualizacion] SMALLDATETIME NOT NULL,
);

CREATE TABLE [Notificaciones] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[UsuarioId] INT NOT NULL REFERENCES [Usuarios]([Id]),
	[Mensaje] NVARCHAR(200) NOT NULL,
	[FechaEnvio] SMALLDATETIME NOT NULL,
	[Leida] BIT NOT NULL,
);

CREATE TABLE [Historicos] (
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[EntidadAfectada] NVARCHAR(200) NOT NULL,
	[Descripcion] NVARCHAR(200) NOT NULL,
	[Fecha] SMALLDATETIME NOT NULL
);

INSERT INTO Roles (Nombre, Descripcion) VALUES 
('Administrador', 'Usuario Administrador, Gestionara el sistema'),
('Cliente', 'Usuario comprador, realizara los pedidos'),
('Repartidor', 'Usuario empleado, entregara los pedidos');

INSERT INTO Zonas (Nombre, Tarifa) VALUES 
('Centro', 3000),
('Norte', 3500),
('Sur', 4000),
('Oriente', 4500),
('Occidente', 3800);

INSERT INTO EstadoPedido (Nombre, Descripcion, notificar, Activo) VALUES 
('Pendiente', 'El pedido esta en lista de espera', 0, 1),
('En preparación', 'El pedido se esta preparando', 0, 1),
('En camino', 'El pedido ya esta en ruta', 1, 1),
('Completado', 'El pedido se ha entregado con exito', 1, 1);

INSERT INTO MetodoPago (Nombre, descripcion, comision, Activo) VALUES 
('Efectivo', 'Cancela con el monto de dinero fisico - efectivo', 1, 1),
('Tarjeta', 'Cancela con tarjeta, sea credito o debito', 5, 1),
('Transferencia', 'Cancela con una transferencia bancaria', 2, 1);

INSERT INTO Vehiculos (Tipo, Placa, Modelo, Activo) VALUES 
('Moto', 'ABC123', '2015', 1),
('Moto', 'BIC001', '2010', 1),
('Moto', 'XYZ789', '2019', 1);

INSERT INTO Usuarios (Cedula, Nombre, Email, Contrasena, FechaNacimiento, Rol) VALUES 
('1234', 'Juan Perez', 'juan@email.com', 'juanp12', '2000-12-08', 1),
('2234', 'Maria Gomez', 'maria@email.com', 'mariag34', '2005-09-23', 1),
('4369', 'Carlos Ruiz', 'carlos@email.com', 'carlosr56', '1980-01-12', 2),
('5812', 'Ana Torres', 'ana@email.com', 'anat78', '1999-06-30', 2),
('7943', 'Pedro Lopez', 'pedro@email.com', 'pedrol90', '2001-07-20', 2),
('5798', 'Javier Ochoa', 'javier@email.com', 'javiero13', '1986-10-12', 3),
('8907', 'Camila Fernandez', 'camila@email.com', 'camilaf35', '2001-09-22', 3),
('1356', 'Cesar Ortiz', 'cesar@email.com', 'cesaro68', '1995-07-17', 3);

INSERT INTO Clientes (Id, Direccion, telefono, FechaRegistro, MetodoPagoFav, Activo) VALUES 
(3, 'Calle 12', '302134', '2025-10-10', 1, 0),
(4, 'Calle 50', '125398', '2026-01-21', 3, 1),
(5, 'Carrera 23', '683421', '2025-12-15', 2, 1);

INSERT INTO Repartidores (Id, VehiculoId, ZonaId, CalificacionPromedio, Disponible, Activo) VALUES 
(6, 1, 1, 0, 1, 1),
(7, 2, 2, 0, 1, 1),
(8, 3, 3, 0, 1, 1);

INSERT INTO Categorias (Nombre, Descripcion, creacion, Activo) VALUES 
('Hamburguesas', 'Todo tipo de hamburguesas', '2025-01-01', 1),
('Perros', 'Todo tipo de hamburguesas', '2025-01-01', 1),
('Salchipapas', 'Todo tipo de hamburguesas', '2025-01-01', 0),
('Bebidas', 'Todo tipo de bebidas', '2025-01-01', 1);

INSERT INTO Productos (Nombre,Descripcion, Precio, Stock, Urlimagen, CategoriaId) VALUES 
('Hamburguesa Sencilla','Pan, Carne, Mucho queso y Vegetales', 15000, 50,'/images/HamburguesaSencilla.webp', 1),
('Hamburguesa Especial','Pan, Doble Carne, Tocineta, Mucho queso y Vegetales', 25000, 50,'/images/HamburguesaEspecial.jpg', 1),
('Perro Pequeño','Pan, Salchicha, Ensalada y Queso', 15000, 40,'/images/PerroPequeño.webp', 2),
('Perro Grande','Pan, Salchicha, Ensalada, Huevos de Codrniz, Queso y Tocineta', 23000, 40,'/images/PerroGrande.webp', 2),
('Salchipapa Sencilla','Papas, Salchicha, Queso y Huevos de Codorniz', 12000, 40,'/images/SalchipapaSencilla.jpg', 3),
('Salchipapa Mega','Papas, Salchicha, Chorizos, Tocineta, Carne, Queso y Huevos de Codorniz', 24000, 40,'/images/SalchipapaMega.jfif', 3),
('Coca cola','Productos Coca Cola', 6000, 40,'/images/Cocacola.jpg', 4),
('Limonada Natural','Limonada artesanal de la casa', 8000, 40,'/images/Limonada.jfif', 4)
;

INSERT INTO Pedidos (ClienteId, RepartidorId, EstadoPedidoId, FechaCreacion, Total) VALUES 
(3, 6, 4, '2026-03-04', 42000),
(4, 7, 2, GETDATE(), 31000),
(5, 8, 3, GETDATE(), 48000);

INSERT INTO DetallePedido (PedidoId, ProductoId, Cantidad, PrecioUnitario, Subtotal) VALUES 
(1, 1, 2, 15000, 30000),
(1, 7, 2, 6000, 12000),
(2, 4, 1, 23000, 23000),
(2, 8, 1, 8000, 8000),
(3, 6, 2, 24000, 48000);

INSERT INTO RutaEntrega (PedidoId, distanciaKM, TiempoEstimado, fecha) VALUES 
(1, 5, 10, '2026-03-04'),
(2, 3, 6, GETDATE()),
(3, 4, 8, GETDATE());

INSERT INTO RastreoPedido (PedidoId, Latitud, Longitud, FechaActualizacion) VALUES 
(1, 6.251, -75.563, '2026-03-04'),
(2, 6.252, -75.564, GETDATE()),
(3, 6.253, -75.565, GETDATE());

INSERT INTO RastreoRepartidor (RepartidorId, Latitud, Longitud, FechaActualizacion) VALUES 
(6, 6.261, -75.573, GETDATE()),
(7, 6.252, -75.564, GETDATE()),
(8, 6.263, -75.575, GETDATE());

INSERT INTO Calificaciones (PedidoId, Puntaje, Comentario, Fecha) VALUES 
(1, 5, 'Excelente servicio', '2026-03-04'),
(2, 4, 'Buena atencion', GETDATE());

INSERT INTO Notificaciones (UsuarioId, Mensaje, FechaEnvio, Leida) VALUES 
(3, 'Tu pedido ha sido entregado', '2026-03-04', 1),
(7, 'Nuevo Pedido asignado', GETDATE(), 0),
(5, 'Tu pedido esta en camino', GETDATE(), 0);

INSERT INTO EstadoPago (Nombre, Activo) VALUES 
('Pendiente', 1),
('Pagado', 1),
('Rechazado', 1);

INSERT INTO Pagos (PedidoId, MetodoPagoId, EstadoPagoId, Monto, Comision, FechaPago) VALUES 
(1, 1, 2, 42000, 420, '2026-03-04'),
(2, 2, 1, 31000, 1550, GETDATE()),
(3, 3, 2, 48000, 960, GETDATE());

INSERT INTO Facturas (PedidoId, PagoId, FechaEmision, impuesto, Total) VALUES 
(1, 1, '2026-03-04', 7980, 49980),
(3, 3, GETDATE(), 9120, 57120);
