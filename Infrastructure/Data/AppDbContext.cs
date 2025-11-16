using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketSeat> TicketSeats { get; set; }
        public DbSet<TicketSector> TicketSectors { get; set; }
        public DbSet<TicketStatus> TicketStatuses { get; set; }
        public DbSet<EventSeat> EventSeats { get; set; }
        public DbSet<EventSector> EventSectors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ticket
            modelBuilder.Entity<Ticket>(entity => 
            {
                entity.ToTable("Ticket");
                entity.HasKey(t => t.TicketId);     
                entity.Property(t => t.UserId)
                    .IsRequired();
                entity.Property(t => t.EventId)
                    .IsRequired();
                entity.Property(t => t.StatusId)
                    .IsRequired();               
                entity.Property(t => t.Created)
                    .IsRequired();
                entity.Property(t => t.Updated);
            });

            // TicketSeat
            modelBuilder.Entity<TicketSeat>(entity =>
            {
                entity.ToTable("TicketSeat");
                entity.HasKey(t => t.TicketSeatId);
                entity.Property(t => t.TicketId)
                    .IsRequired();
                entity.Property(t => t.EventSeatId)
                    .IsRequired();
            });

            // TicketStatus
            modelBuilder.Entity<TicketStatus>(entity =>
            {
                entity.ToTable("TicketStatus");
                entity.HasKey(s => s.StatusID);
                entity.Property(s => s.Name)
                    .IsRequired()
                    .HasColumnType("varchar(25)");
                entity.HasData(
                    new TicketStatus { StatusID = 1, Name = "Available"},
                    new TicketStatus { StatusID = 2, Name = "Reserved"},
                    new TicketStatus { StatusID = 3, Name = "Sold"},
                    new TicketStatus { StatusID = 4, Name = "Expired"}
                    );
            });

            // EventSeat
            modelBuilder.Entity<EventSeat>(entity =>
            {
                entity.ToTable("EventSeat");
                entity.HasKey(e => e.EventSeatId);
                entity.Property(t => t.EventId)
                    .IsRequired();
                entity.Property(e => e.EventSectorId)
                    .IsRequired();
                entity.Property(e => e.SeatId)
                    .IsRequired();
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();
                entity.Property(e => e.StatusId)
                    .IsRequired();
                entity.Property(e => e.ReservedByUserId)
                    .IsRequired(false);
            });

            // EventSector
            modelBuilder.Entity<EventSector>(entity =>
            {
                entity.ToTable("EventSector");
                entity.HasKey(es => es.EventSectorId);
                entity.Property(es => es.EventId)
                    .IsRequired();
                entity.Property(es => es.SectorId)
                    .IsRequired();
                entity.Property(es => es.Name)
                    .HasColumnType("varchar(50)")
                    .IsRequired();           
                entity.Property(es => es.IsControlled)
                    .IsRequired();
                entity.Property(es => es.Capacity);
                entity.Property(es => es.SoldCount);
                entity.Property(es => es.ReservedCount);
                entity.Property(es => es.Price)
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();
            });

            // Relationships

            // uno a muchos - EventSector EventSeat
            modelBuilder.Entity<EventSeat>()
                .HasOne(ES => ES.EventSectorRef)
                .WithMany(ESector => ESector.EventSeats)
                .HasForeignKey(ES => ES.EventSectorId)
                .OnDelete(DeleteBehavior.Cascade);


            // uno a muchos - TicketStatus  Ticket
            modelBuilder.Entity<Ticket>()
                .HasOne(ticket => ticket.StatusRef)
                .WithMany(TStatus => TStatus.Tickets)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // uno a muchos - Ticket TicketSeat
            modelBuilder.Entity<TicketSeat>()
                .HasOne(ts => ts.TicketRef)
                .WithMany(t => t.TicketSeats)
                .HasForeignKey(ts => ts.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // uno a uno - EventSeat TicketSeat
            modelBuilder.Entity<TicketSeat>()
                .HasOne(ts => ts.EventSeatRef)
                .WithMany()
                .HasForeignKey(ts => ts.EventSeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // uno a muchos Ticket TicketSector
            modelBuilder.Entity<TicketSector>()
                .HasOne(ts => ts.TicketRef)
                .WithMany(t => t.TicketSectors)
                .HasForeignKey(ts => ts.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            // uno a uno EventSector TicketSector
            modelBuilder.Entity<TicketSector>()
                .HasOne(ts => ts.EventSectorRef)
                .WithMany()
                .HasForeignKey(ts => ts.EventSectorId)
                .OnDelete(DeleteBehavior.Restrict);

            // uno a muchos - TicketStatus EventSeat
            modelBuilder.Entity<EventSeat>()
                .HasOne(ES => ES.StatusRef)
                .WithMany(TStatus => TStatus.EventSeats)
                .HasForeignKey(ES => ES.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            
        }
    }
}
