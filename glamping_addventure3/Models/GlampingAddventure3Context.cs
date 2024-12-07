using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace glamping_addventure3.Models;

public partial class GlampingAddventure3Context : DbContext
{
    public GlampingAddventure3Context()
    {
    }

    public GlampingAddventure3Context(DbContextOptions<GlampingAddventure3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Abono> Abonos { get; set; }

    public DbSet<CodigoRecuperacion> CodigosRecuperacion { get; set; }  // Agrega esta línea

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetalleReservaPaquete> DetalleReservaPaquetes { get; set; }

    public virtual DbSet<DetalleReservaServicio> DetalleReservaServicios { get; set; }

    public virtual DbSet<EstadosReserva> EstadosReservas { get; set; }

    public virtual DbSet<Habitacion> Habitacions { get; set; }

    public virtual DbSet<HabitacionMueble> HabitacionMuebles { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<Mueble> Muebles { get; set; }

    public virtual DbSet<Paquete> Paquetes { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolesPermiso> RolesPermisos { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TipodeHabitacion> TipodeHabitacions { get; set; }

    public virtual DbSet<TokenRecuperacion> TokenRecuperacion { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=MEDAPRCSGFSP688\\SQLEXPRESS;Initial Catalog=ola;integrated security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Abono>(entity =>
        {
            entity.HasKey(e => e.Idabono).HasName("PK__Abono__8647F8A95A40B07E");

            entity.ToTable("Abono");

            entity.Property(e => e.Idabono).HasColumnName("IDAbono");
            entity.Property(e => e.Idreserva).HasColumnName("IDReserva");

            entity.HasOne(d => d.IdreservaNavigation).WithMany(p => p.Abonos)
                .HasForeignKey(d => d.Idreserva)
                .HasConstraintName("FK_Abono_Reserva");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.NroDocumento).HasName("PK__Clientes__CC62C91C118FB83F");

            entity.Property(e => e.NroDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Idrol).HasColumnName("IDRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.Idrol)
                .HasConstraintName("FK_Clientes_Roles");
        });

        modelBuilder.Entity<DetalleReservaPaquete>(entity =>
        {
            entity.HasKey(e => e.IddetalleReservaPaquetes).HasName("PK__DetalleR__64F9FDAECC7F82D0");

            entity.Property(e => e.IddetalleReservaPaquetes).HasColumnName("IDDetalleReservaPaquetes");
            entity.Property(e => e.Idpaquete).HasColumnName("IDPaquete");
            entity.Property(e => e.Idreserva).HasColumnName("IDReserva");

            entity.HasOne(d => d.IdpaqueteNavigation).WithMany(p => p.DetalleReservaPaquetes)
                .HasForeignKey(d => d.Idpaquete)
                .HasConstraintName("FK_DetalleReservaPaquetes_Paquete");

            entity.HasOne(d => d.IdreservaNavigation).WithMany(p => p.DetalleReservaPaquetes)
                .HasForeignKey(d => d.Idreserva)
                .HasConstraintName("FK_DetalleReservaPaquetes_Reserva");
        });

        modelBuilder.Entity<DetalleReservaServicio>(entity =>
        {
            entity.HasKey(e => e.IddetalleReservaServicio).HasName("PK__DetalleR__B52B22F852F3BE00");

            entity.ToTable("DetalleReservaServicio");

            entity.Property(e => e.IddetalleReservaServicio).HasColumnName("IDDetalleReservaServicio");
            entity.Property(e => e.Idreserva).HasColumnName("IDReserva");
            entity.Property(e => e.Idservicio).HasColumnName("IDServicio");

            entity.HasOne(d => d.IdreservaNavigation).WithMany(p => p.DetalleReservaServicios)
                .HasForeignKey(d => d.Idreserva)
                .HasConstraintName("FK_DetalleReservaServicio_Reserva");

            entity.HasOne(d => d.IdservicioNavigation).WithMany(p => p.DetalleReservaServicios)
                .HasForeignKey(d => d.Idservicio)
                .HasConstraintName("FK_DetalleReservaServicio_Servicio");
        });

        modelBuilder.Entity<EstadosReserva>(entity =>
        {
            entity.HasKey(e => e.IdEstadoReserva).HasName("PK__EstadosR__3E654CA898335B91");

            entity.ToTable("EstadosReserva");

            entity.Property(e => e.NombreEstadoReserva)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Habitacion>(entity =>
        {
            entity.HasKey(e => e.Idhabitacion).HasName("PK__Habitaci__6B4757DADE5D4D9D");

            entity.ToTable("Habitacion");

            entity.Property(e => e.Idhabitacion).HasColumnName("IDHabitacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdtipodeHabitacion).HasColumnName("IDTipodeHabitacion");
            entity.Property(e => e.NombreHabitacion)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdtipodeHabitacionNavigation).WithMany(p => p.Habitacions)
                .HasForeignKey(d => d.IdtipodeHabitacion)
                .HasConstraintName("FK_Habitacion_TipodeHabitacion");
        });

        modelBuilder.Entity<HabitacionMueble>(entity =>
        {
            entity.HasKey(e => e.IdhabitacionMuebles).HasName("PK__Habitaci__65CFCA9AC9573D6A");

            entity.Property(e => e.IdhabitacionMuebles).HasColumnName("IDHabitacionMuebles");
            entity.Property(e => e.Idhabitacion).HasColumnName("IDHabitacion");
            entity.Property(e => e.Idmueble).HasColumnName("IDMueble");

            entity.HasOne(d => d.IdhabitacionNavigation).WithMany(p => p.HabitacionMuebles)
                .HasForeignKey(d => d.Idhabitacion)
                .HasConstraintName("FK_HabitacionMuebles_Habitacion");

            entity.HasOne(d => d.IdmuebleNavigation).WithMany(p => p.HabitacionMuebles)
                .HasForeignKey(d => d.Idmueble)
                .HasConstraintName("FK_HabitacionMuebles_Muebles");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__MetodoPa__6F49A9BED37B368A");

            entity.ToTable("MetodoPago");

            entity.Property(e => e.NomMetodoPago)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Mueble>(entity =>
        {
            entity.HasKey(e => e.Idmueble).HasName("PK__Muebles__EAC9F7C88D2DF473");

            entity.Property(e => e.Idmueble).HasColumnName("IDMueble");
            entity.Property(e => e.NombreMueble)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Paquete>(entity =>
        {
            entity.HasKey(e => e.Idpaquete).HasName("PK__Paquetes__4C29513BEC7B59E1");

            entity.Property(e => e.Idpaquete).HasColumnName("IDPaquete");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.Idhabitacion).HasColumnName("IDHabitacion");
            entity.Property(e => e.Idservicio).HasColumnName("IDServicio");
            entity.Property(e => e.NombrePaquete)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdhabitacionNavigation).WithMany(p => p.Paquetes)
                .HasForeignKey(d => d.Idhabitacion)
                .HasConstraintName("FK_Habitacione_Paquetes");

            entity.HasOne(d => d.IdservicioNavigation).WithMany(p => p.Paquetes)
                .HasForeignKey(d => d.Idservicio)
                .HasConstraintName("FK_Servicio_Paquetes");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.Idpermiso).HasName("PK__Permisos__F11D75F39691961E");

            entity.Property(e => e.Idpermiso).HasColumnName("IDPermiso");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.EstadoPermisos).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.NombrePermisos).HasMaxLength(255);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__Reserva__0E49C69D32CE0362");

            entity.ToTable("Reserva");

            entity.Property(e => e.FechaReserva).HasColumnType("datetime");
            entity.Property(e => e.Iva).HasColumnName("IVA");
            entity.Property(e => e.NroDocumentoCliente)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstadoReservaNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdEstadoReserva)
                .HasConstraintName("FK__Reserva__IdEstad__6C190EBB");

            entity.HasOne(d => d.MetodoPagoNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MetodoPago)
                .HasConstraintName("FK__Reserva__MetodoP__6D0D32F4");

            entity.HasOne(d => d.NroDocumentoClienteNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.NroDocumentoCliente)
                .HasConstraintName("FK__Reserva__NroDocu__6B24EA82");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrol).HasName("PK__Roles__A681ACB61E786D48");

            entity.Property(e => e.Idrol).HasColumnName("IDRol");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<RolesPermiso>(entity =>
        {
            entity.HasKey(e => e.IdrolPermiso).HasName("PK__RolesPer__3F09FABFE79F4DA2");

            entity.Property(e => e.IdrolPermiso).HasColumnName("IDRolPermiso");
            entity.Property(e => e.Idpermiso).HasColumnName("IDPermiso");
            entity.Property(e => e.Idrol).HasColumnName("IDRol");

            entity.HasOne(d => d.IdpermisoNavigation).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.Idpermiso)
                .HasConstraintName("FK__RolesPerm__IDPer__534D60F1");

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.Idrol)
                .HasConstraintName("FK__RolesPerm__IDRol__52593CB8");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Idservicio).HasName("PK__Servicio__3CCE7416F3CB208A");

            entity.Property(e => e.Idservicio).HasColumnName("IDServicio");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Duracion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreServicio)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipodeHabitacion>(entity =>
        {
            entity.HasKey(e => e.IdtipodeHabitacion).HasName("PK__TipodeHa__9BD4086C17A3DCE5");

            entity.ToTable("TipodeHabitacion");

            entity.Property(e => e.IdtipodeHabitacion).HasColumnName("IDTipodeHabitacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreTipodeHabitacion)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TokenRecuperacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TokenRec__3214EC07DFC4F73E");

            entity.ToTable("TokenRecuperacion");

            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(255);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario).HasName("PK__Usuarios__52311169B5B0B2C3");

            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");
            entity.Property(e => e.Apellido).HasMaxLength(255);
            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Idrol).HasColumnName("IDRol");
            entity.Property(e => e.NombreUsuario).HasMaxLength(255);
            entity.Property(e => e.Pais).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(50);
            entity.Property(e => e.TipoDocumento).HasMaxLength(50);

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Idrol)
                .HasConstraintName("FK__Usuarios__IDRol__5629CD9C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}