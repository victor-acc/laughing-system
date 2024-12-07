using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace glamping_addventure3.Migrations
{
    /// <inheritdoc />
    public partial class uno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosReserva",
                columns: table => new
                {
                    IdEstadoReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEstadoReserva = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EstadosR__3E654CA898335B91", x => x.IdEstadoReserva);
                });

            migrationBuilder.CreateTable(
                name: "MetodoPago",
                columns: table => new
                {
                    IdMetodoPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomMetodoPago = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MetodoPa__6F49A9BED37B368A", x => x.IdMetodoPago);
                });

            migrationBuilder.CreateTable(
                name: "Muebles",
                columns: table => new
                {
                    IDMueble = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreMueble = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    ImagenMueble = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<double>(type: "float", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Muebles__EAC9F7C88D2DF473", x => x.IDMueble);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    IDPermiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePermisos = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EstadoPermisos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permisos__F11D75F39691961E", x => x.IDPermiso);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IDRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__A681ACB61E786D48", x => x.IDRol);
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    IDServicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreServicio = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Duracion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CantidadMaximaPersonas = table.Column<int>(type: "int", nullable: false),
                    Costo = table.Column<double>(type: "float", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Servicio__3CCE7416F3CB208A", x => x.IDServicio);
                });

            migrationBuilder.CreateTable(
                name: "TipodeHabitacion",
                columns: table => new
                {
                    IDTipodeHabitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTipodeHabitacion = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TipodeHa__9BD4086C17A3DCE5", x => x.IDTipodeHabitacion);
                });

            migrationBuilder.CreateTable(
                name: "TokenRecuperacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime", nullable: false),
                    Usado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TokenRec__3214EC07DFC4F73E", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    NroDocumento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Apellido = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Telefono = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true),
                    IDRol = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clientes__CC62C91C118FB83F", x => x.NroDocumento);
                    table.ForeignKey(
                        name: "FK_Clientes_Roles",
                        column: x => x.IDRol,
                        principalTable: "Roles",
                        principalColumn: "IDRol");
                });

            migrationBuilder.CreateTable(
                name: "RolesPermisos",
                columns: table => new
                {
                    IDRolPermiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDRol = table.Column<int>(type: "int", nullable: true),
                    IDPermiso = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RolesPer__3F09FABFE79F4DA2", x => x.IDRolPermiso);
                    table.ForeignKey(
                        name: "FK__RolesPerm__IDPer__534D60F1",
                        column: x => x.IDPermiso,
                        principalTable: "Permisos",
                        principalColumn: "IDPermiso");
                    table.ForeignKey(
                        name: "FK__RolesPerm__IDRol__52593CB8",
                        column: x => x.IDRol,
                        principalTable: "Roles",
                        principalColumn: "IDRol");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IDUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Contrasena = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TipoDocumento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroDocumento = table.Column<int>(type: "int", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IDRol = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__52311169B5B0B2C3", x => x.IDUsuario);
                    table.ForeignKey(
                        name: "FK__Usuarios__IDRol__5629CD9C",
                        column: x => x.IDRol,
                        principalTable: "Roles",
                        principalColumn: "IDRol");
                });

            migrationBuilder.CreateTable(
                name: "Habitacion",
                columns: table => new
                {
                    IDHabitacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreHabitacion = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    ImagenHabitacion = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Costo = table.Column<double>(type: "float", nullable: false),
                    IDTipodeHabitacion = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Habitaci__6B4757DADE5D4D9D", x => x.IDHabitacion);
                    table.ForeignKey(
                        name: "FK_Habitacion_TipodeHabitacion",
                        column: x => x.IDTipodeHabitacion,
                        principalTable: "TipodeHabitacion",
                        principalColumn: "IDTipodeHabitacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    IdReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroDocumentoCliente = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FechaReserva = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: true),
                    FechaFinalizacion = table.Column<DateOnly>(type: "date", nullable: true),
                    SubTotal = table.Column<double>(type: "float", nullable: true),
                    Descuento = table.Column<double>(type: "float", nullable: true),
                    IVA = table.Column<double>(type: "float", nullable: true),
                    MontoTotal = table.Column<double>(type: "float", nullable: true),
                    MetodoPago = table.Column<int>(type: "int", nullable: true),
                    IdEstadoReserva = table.Column<int>(type: "int", nullable: true),
                    UsuarioIdusuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reserva__0E49C69D32CE0362", x => x.IdReserva);
                    table.ForeignKey(
                        name: "FK_Reserva_Usuarios_UsuarioIdusuario",
                        column: x => x.UsuarioIdusuario,
                        principalTable: "Usuarios",
                        principalColumn: "IDUsuario");
                    table.ForeignKey(
                        name: "FK__Reserva__IdEstad__6C190EBB",
                        column: x => x.IdEstadoReserva,
                        principalTable: "EstadosReserva",
                        principalColumn: "IdEstadoReserva");
                    table.ForeignKey(
                        name: "FK__Reserva__MetodoP__6D0D32F4",
                        column: x => x.MetodoPago,
                        principalTable: "MetodoPago",
                        principalColumn: "IdMetodoPago");
                    table.ForeignKey(
                        name: "FK__Reserva__NroDocu__6B24EA82",
                        column: x => x.NroDocumentoCliente,
                        principalTable: "Clientes",
                        principalColumn: "NroDocumento");
                });

            migrationBuilder.CreateTable(
                name: "HabitacionMuebles",
                columns: table => new
                {
                    IDHabitacionMuebles = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDHabitacion = table.Column<int>(type: "int", nullable: true),
                    IDMueble = table.Column<int>(type: "int", nullable: true),
                    CantidadUsada = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Habitaci__65CFCA9AC9573D6A", x => x.IDHabitacionMuebles);
                    table.ForeignKey(
                        name: "FK_HabitacionMuebles_Habitacion",
                        column: x => x.IDHabitacion,
                        principalTable: "Habitacion",
                        principalColumn: "IDHabitacion");
                    table.ForeignKey(
                        name: "FK_HabitacionMuebles_Muebles",
                        column: x => x.IDMueble,
                        principalTable: "Muebles",
                        principalColumn: "IDMueble");
                });

            migrationBuilder.CreateTable(
                name: "Paquetes",
                columns: table => new
                {
                    IDPaquete = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombrePaquete = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    ImagenPaquete = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    IDHabitacion = table.Column<int>(type: "int", nullable: false),
                    IDServicio = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Paquetes__4C29513BEC7B59E1", x => x.IDPaquete);
                    table.ForeignKey(
                        name: "FK_Habitacione_Paquetes",
                        column: x => x.IDHabitacion,
                        principalTable: "Habitacion",
                        principalColumn: "IDHabitacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Servicio_Paquetes",
                        column: x => x.IDServicio,
                        principalTable: "Servicios",
                        principalColumn: "IDServicio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Abono",
                columns: table => new
                {
                    IDAbono = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDReserva = table.Column<int>(type: "int", nullable: true),
                    FechaAbono = table.Column<DateOnly>(type: "date", nullable: true),
                    ValorDeuda = table.Column<double>(type: "float", nullable: true),
                    Porcentaje = table.Column<double>(type: "float", nullable: true),
                    Pendiente = table.Column<double>(type: "float", nullable: true),
                    CantAbono = table.Column<double>(type: "float", nullable: true),
                    Comprobante = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Abono__8647F8A95A40B07E", x => x.IDAbono);
                    table.ForeignKey(
                        name: "FK_Abono_Reserva",
                        column: x => x.IDReserva,
                        principalTable: "Reserva",
                        principalColumn: "IdReserva");
                });

            migrationBuilder.CreateTable(
                name: "DetalleReservaServicio",
                columns: table => new
                {
                    IDDetalleReservaServicio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDReserva = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: true),
                    Precio = table.Column<double>(type: "float", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true),
                    IDServicio = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DetalleR__B52B22F852F3BE00", x => x.IDDetalleReservaServicio);
                    table.ForeignKey(
                        name: "FK_DetalleReservaServicio_Reserva",
                        column: x => x.IDReserva,
                        principalTable: "Reserva",
                        principalColumn: "IdReserva");
                    table.ForeignKey(
                        name: "FK_DetalleReservaServicio_Servicio",
                        column: x => x.IDServicio,
                        principalTable: "Servicios",
                        principalColumn: "IDServicio");
                });

            migrationBuilder.CreateTable(
                name: "DetalleReservaPaquetes",
                columns: table => new
                {
                    IDDetalleReservaPaquetes = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDReserva = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: true),
                    Precio = table.Column<double>(type: "float", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true),
                    IDPaquete = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DetalleR__64F9FDAECC7F82D0", x => x.IDDetalleReservaPaquetes);
                    table.ForeignKey(
                        name: "FK_DetalleReservaPaquetes_Paquete",
                        column: x => x.IDPaquete,
                        principalTable: "Paquetes",
                        principalColumn: "IDPaquete");
                    table.ForeignKey(
                        name: "FK_DetalleReservaPaquetes_Reserva",
                        column: x => x.IDReserva,
                        principalTable: "Reserva",
                        principalColumn: "IdReserva");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abono_IDReserva",
                table: "Abono",
                column: "IDReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_IDRol",
                table: "Clientes",
                column: "IDRol");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleReservaPaquetes_IDPaquete",
                table: "DetalleReservaPaquetes",
                column: "IDPaquete");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleReservaPaquetes_IDReserva",
                table: "DetalleReservaPaquetes",
                column: "IDReserva");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleReservaServicio_IDReserva",
                table: "DetalleReservaServicio",
                column: "IDReserva");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleReservaServicio_IDServicio",
                table: "DetalleReservaServicio",
                column: "IDServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Habitacion_IDTipodeHabitacion",
                table: "Habitacion",
                column: "IDTipodeHabitacion");

            migrationBuilder.CreateIndex(
                name: "IX_HabitacionMuebles_IDHabitacion",
                table: "HabitacionMuebles",
                column: "IDHabitacion");

            migrationBuilder.CreateIndex(
                name: "IX_HabitacionMuebles_IDMueble",
                table: "HabitacionMuebles",
                column: "IDMueble");

            migrationBuilder.CreateIndex(
                name: "IX_Paquetes_IDHabitacion",
                table: "Paquetes",
                column: "IDHabitacion");

            migrationBuilder.CreateIndex(
                name: "IX_Paquetes_IDServicio",
                table: "Paquetes",
                column: "IDServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_IdEstadoReserva",
                table: "Reserva",
                column: "IdEstadoReserva");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_MetodoPago",
                table: "Reserva",
                column: "MetodoPago");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_NroDocumentoCliente",
                table: "Reserva",
                column: "NroDocumentoCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_UsuarioIdusuario",
                table: "Reserva",
                column: "UsuarioIdusuario");

            migrationBuilder.CreateIndex(
                name: "IX_RolesPermisos_IDPermiso",
                table: "RolesPermisos",
                column: "IDPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_RolesPermisos_IDRol",
                table: "RolesPermisos",
                column: "IDRol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IDRol",
                table: "Usuarios",
                column: "IDRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abono");

            migrationBuilder.DropTable(
                name: "DetalleReservaPaquetes");

            migrationBuilder.DropTable(
                name: "DetalleReservaServicio");

            migrationBuilder.DropTable(
                name: "HabitacionMuebles");

            migrationBuilder.DropTable(
                name: "RolesPermisos");

            migrationBuilder.DropTable(
                name: "TokenRecuperacion");

            migrationBuilder.DropTable(
                name: "Paquetes");

            migrationBuilder.DropTable(
                name: "Reserva");

            migrationBuilder.DropTable(
                name: "Muebles");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Habitacion");

            migrationBuilder.DropTable(
                name: "Servicios");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "EstadosReserva");

            migrationBuilder.DropTable(
                name: "MetodoPago");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "TipodeHabitacion");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
