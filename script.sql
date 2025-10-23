USE [BD]
GO
/****** Object:  User [alumno]    Script Date: 23/10/2025 14:12:59 ******/
CREATE USER [alumno] FOR LOGIN [alumno] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Compras]    Script Date: 23/10/2025 14:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compras](
	[IdCompra] [int] IDENTITY(1,1) NOT NULL,
	[IdVendedor] [int] NOT NULL,
	[IdComprador] [int] NOT NULL,
	[FechaCompra] [date] NOT NULL,
 CONSTRAINT [PK_Compras] PRIMARY KEY CLUSTERED 
(
	[IdCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Etiquetas]    Script Date: 23/10/2025 14:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Etiquetas](
	[IdEtiqueta] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Etiquetas] PRIMARY KEY CLUSTERED 
(
	[IdEtiqueta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Listas]    Script Date: 23/10/2025 14:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Listas](
	[IdLista] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[NombreLista] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Listas] PRIMARY KEY CLUSTERED 
(
	[IdLista] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Publicaciones]    Script Date: 23/10/2025 14:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Publicaciones](
	[IdPublicacion] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
	[Foto] [nvarchar](50) NOT NULL,
	[Precio] [float] NOT NULL,
 CONSTRAINT [PK_Publicaciones] PRIMARY KEY CLUSTERED 
(
	[IdPublicacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PublicacionEtiqueta]    Script Date: 23/10/2025 14:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PublicacionEtiqueta](
	[IdPublicacionEtiqueta] [int] IDENTITY(1,1) NOT NULL,
	[IdPublicacion] [int] NOT NULL,
	[IdEtiqueta] [int] NULL,
 CONSTRAINT [PK_PublicacionEtiqueta] PRIMARY KEY CLUSTERED 
(
	[IdPublicacionEtiqueta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PublicacionLista]    Script Date: 23/10/2025 14:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PublicacionLista](
	[IdPublicacionLista] [int] IDENTITY(1,1) NOT NULL,
	[IdPublicacion] [int] NOT NULL,
	[IdLista] [int] NOT NULL,
 CONSTRAINT [PK_PublicacionLista] PRIMARY KEY CLUSTERED 
(
	[IdPublicacionLista] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 23/10/2025 14:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Contraseña] [nvarchar](50) NOT NULL,
	[Usuario] [nvarchar](50) NOT NULL,
	[NombreCompleto] [nvarchar](50) NOT NULL,
	[Pais] [nvarchar](50) NOT NULL,
	[Telefono] [int] NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuariosLista]    Script Date: 23/10/2025 14:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuariosLista](
	[IdUsuariosLista] [int] IDENTITY(1,1) NOT NULL,
	[IdLista] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
 CONSTRAINT [PK_UsuariosLista] PRIMARY KEY CLUSTERED 
(
	[IdUsuariosLista] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Compras]  WITH CHECK ADD  CONSTRAINT [FK_Compras_Usuarios] FOREIGN KEY([IdComprador])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Compras] CHECK CONSTRAINT [FK_Compras_Usuarios]
GO
ALTER TABLE [dbo].[Compras]  WITH CHECK ADD  CONSTRAINT [FK_Compras_Usuarios1] FOREIGN KEY([IdVendedor])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Compras] CHECK CONSTRAINT [FK_Compras_Usuarios1]
GO
ALTER TABLE [dbo].[Listas]  WITH CHECK ADD  CONSTRAINT [FK_Listas_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Listas] CHECK CONSTRAINT [FK_Listas_Usuarios]
GO
ALTER TABLE [dbo].[Publicaciones]  WITH CHECK ADD  CONSTRAINT [FK_Publicaciones_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Publicaciones] CHECK CONSTRAINT [FK_Publicaciones_Usuarios]
GO
ALTER TABLE [dbo].[PublicacionEtiqueta]  WITH CHECK ADD  CONSTRAINT [FK_PublicacionEtiqueta_Etiquetas] FOREIGN KEY([IdEtiqueta])
REFERENCES [dbo].[Etiquetas] ([IdEtiqueta])
GO
ALTER TABLE [dbo].[PublicacionEtiqueta] CHECK CONSTRAINT [FK_PublicacionEtiqueta_Etiquetas]
GO
ALTER TABLE [dbo].[PublicacionEtiqueta]  WITH CHECK ADD  CONSTRAINT [FK_PublicacionEtiqueta_Publicaciones] FOREIGN KEY([IdPublicacion])
REFERENCES [dbo].[Publicaciones] ([IdPublicacion])
GO
ALTER TABLE [dbo].[PublicacionEtiqueta] CHECK CONSTRAINT [FK_PublicacionEtiqueta_Publicaciones]
GO
ALTER TABLE [dbo].[PublicacionLista]  WITH CHECK ADD  CONSTRAINT [FK_PublicacionLista_Publicaciones] FOREIGN KEY([IdPublicacion])
REFERENCES [dbo].[Publicaciones] ([IdPublicacion])
GO
ALTER TABLE [dbo].[PublicacionLista] CHECK CONSTRAINT [FK_PublicacionLista_Publicaciones]
GO
ALTER TABLE [dbo].[UsuariosLista]  WITH CHECK ADD  CONSTRAINT [FK_UsuariosLista_Listas] FOREIGN KEY([IdLista])
REFERENCES [dbo].[Listas] ([IdLista])
GO
ALTER TABLE [dbo].[UsuariosLista] CHECK CONSTRAINT [FK_UsuariosLista_Listas]
GO
ALTER TABLE [dbo].[UsuariosLista]  WITH CHECK ADD  CONSTRAINT [FK_UsuariosLista_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[UsuariosLista] CHECK CONSTRAINT [FK_UsuariosLista_Usuarios]
GO
