
CREATE DATABASE Veterinaria

CREATE TABLE [dbo].[Usuarios](
	[id_usuario] [int] IDENTITY(1,1),
	[usuario] [varchar](50),
	[password] [varchar](10),
	[email] [varchar](50) ,	
	[borrado] [bit],
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																						 
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([id_usuario])
)


CREATE TABLE [dbo].[Tipos](
	[id_tipo] [int] IDENTITY(1,1),
	[t_descrip] [varchar](50),	
	[borrado] [bit],
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																						 
 CONSTRAINT [PK_Tipos] PRIMARY KEY CLUSTERED 	([id_tipo]) 
)

CREATE TABLE [dbo].[Clientes](
	[id_cliente] [int] IDENTITY(1,1),
	[c_nombre] [varchar](50),	
	[sexo] [bit],
	[borrado] [bit],
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																						 
 CONSTRAINT [PK_Clientes] PRIMARY KEY CLUSTERED 	([id_cliente]) 
)



CREATE TABLE [dbo].[Mascotas](
	[id_mascota] [int] IDENTITY(1,1),
	[m_nombre] [varchar](50),	
	[edad] [int],
	[id_tipo] [int],
	[id_cliente] [int],
	[borrado] [bit],
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																						 
 CONSTRAINT [PK_Mascotas] PRIMARY KEY CLUSTERED 	([id_mascota]) ,
 CONSTRAINT [FK_Mascotas_Tipo] FOREIGN KEY  ([id_tipo])
	REFERENCES [dbo].[Tipos]  ([id_tipo]) ,
CONSTRAINT [FK_Mascotas_Cliente] FOREIGN KEY  ([id_cliente])
	REFERENCES [dbo].[Clientes]  ([id_cliente]) 
)


CREATE TABLE [dbo].[Atenciones](
	[id_atencion] [int] IDENTITY(1,1),
	[fecha] [datetime],
	[a_descrip] [varchar](50),	
	[importe] [decimal](18, 0),
	[id_mascota] [int],
	[id_usuario] [int],
	[borrado] [bit],
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																						 
 CONSTRAINT [PK_Atencion] PRIMARY KEY CLUSTERED 	([id_atencion]) ,
 CONSTRAINT [FK_Atencion_Mascota] FOREIGN KEY  ([id_mascota])
	REFERENCES [dbo].[Mascotas]  ([id_mascota]),
CONSTRAINT [FK_Atencion_Usuario] FOREIGN KEY  ([id_usuario])
	REFERENCES [dbo].[Usuarios]  ([id_usuario])
)

CREATE TABLE [dbo].[Productos](
	[id_producto] [int] IDENTITY(1,1),
	[p_nombre] [varchar](50),	
	[precio] [decimal](18, 0),
	[borrado] [bit],
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																						 
 CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED 	([id_producto]) 
)


CREATE TABLE [dbo].[Detalles](
	[id_detalle] [int] IDENTITY(1,1),
	[id_producto] [int],
	[cantidad] [int],
	[p_facturado] [decimal](18, 0),
	[id_atencion] [int],
	[borrado] [bit],
																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																						 
 CONSTRAINT [PK_Detalles] PRIMARY KEY CLUSTERED 	([id_detalle]) ,
 CONSTRAINT [FK_Detalle_Producto] FOREIGN KEY  ([id_producto])
	REFERENCES [dbo].[Productos]  ([id_producto]) ,
CONSTRAINT [FK_Detalle_Atencion] FOREIGN KEY  ([id_atencion])
	REFERENCES [dbo].[Atencion]  ([id_atencion]) 
)


insert into Usuarios (usuario,password, borrado) values ('a',1,0)
insert into Usuarios (usuario,password, borrado) values ('b',1,0)
insert into Usuarios (usuario,password, borrado) values ('c',1,1)

insert into Clientes (c_nombre,sexo,borrado) values ('cli1',1,0)

insert into Tipos (t_descrip,borrado) values ('pez',0)

insert into Productos (p_nombre, precio, borrado) values ('Vacuna', 150,0)
insert into Productos (p_nombre, precio, borrado) values ('Pipeta', 238,0)

insert into Mascotas(m_nombre, edad, id_tipo, id_cliente, borrado) values ('teo',4,2,3,0)

insert into Atenciones (fecha, a_descrip, importe, id_mascota, id_usuario, borrado) values ('1/11/2021', 'Vacunacion', 523, 1, 2,0)
insert into Atenciones (fecha, a_descrip, importe, id_mascota, id_usuario, borrado) values ('1/10/2021', 'Control', 235, 1, 2,0)

insert into Detalles (id_producto, cantidad, p_facturado, id_atencion, borrado) values (1,2,145,1,0)
insert into Detalles (id_producto, cantidad, p_facturado, id_atencion, borrado) values (2,1,233,1,0)


select a.id_atencion, FORMAT(a.fecha,'dd/MM/yyyy'), a.a_descrip, a.importe,a.borrado,m.id_mascota, m.m_nombre, u.id_usuario, u.usuario from Atencion a join Mascotas m on  a.id_mascota=m.id_mascota join Usuarios u on u.id_usuario=a.id_usuario order by a.borrado, a.fecha desc  

 set dateformat dmy 





select m.id_mascota as id, m.m_nombre as nombre, m.edad, t.t_descrip as animal, c.c_nombre as dueño, m.borrado from mascotas m join clientes c on m.id_cliente=c.id_cliente join tipos t on t.id_tipo=m.id_tipo


select m.id_mascota as id, m.m_nombre as nombre, m.edad, t.t_descrip as animal, c.c_nombre as dueño, m.borrado from mascotas m left join clientes c on m.id_cliente=c.id_cliente join tipos t on t.id_tipo=m.id_tipo


select m.id_mascota as id, m.m_nombre as macota, m.edad, m.borrado, 
t.id_tipo, t_descrip as animal, c.id_cliente, c.sexo,  c.c_nombre as dueño
from mascotas m join clientes c on m.id_cliente=c.id_cliente join tipos t on t.id_tipo=m.id_tipo


select m.id_mascota as id, m.m_nombre as mascota, m.edad, m.borrado, t.id_tipo, t_descrip as animal, c.id_cliente, c.sexo, c.c_nombre as dueño from mascotas m join clientes c on m.id_cliente=c.id_cliente join tipos t on t.id_tipo=m.id_tipo


