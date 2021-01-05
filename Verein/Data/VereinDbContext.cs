using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Verein.Models;

namespace Verein.Data
{
    public class VereinDbContext : IdentityDbContext<HundevereinUser>
    {
        public VereinDbContext(DbContextOptions<VereinDbContext> options) : base(options)
        {

        }

        public DbSet<Mitglied> Mitglieder { get; set; }

        public DbSet<Hund> Hunde { get; set; }

        public DbSet<Kurs> Kurse { get; set; }

        public DbSet<KursTeilnehmer> KursTeilnehmer { get; set; }

        public DbSet<Trainer> Trainer { get; set; }

        public DbSet<StammdatenEintrag> Stammdaten { get; set; }

        public DbSet<Tarif> Tarife { get; set; }

        public DbSet<Gegenstand> Inventar { get; set; }

        public DbSet<BankInformation> Zahlungsinformationen { get; set; }

        public DbSet<Arbeitseinsatz> Arbeitseinsaetze { get; set; }

        public DbSet<Helfer> Helfer { get; set; }

        public DbSet<Familie> Familien { get; set; }

        public DbSet<TrainerBudget> TrainerBudget { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mitglied>()
                .HasMany(m => m.Hunde)
                .WithOne(h => h.Besitzer);

            modelBuilder.Entity<Hund>()
                .HasOne(h => h.Besitzer)
                .WithMany(m => m.Hunde);

            modelBuilder.Entity<Mitglied>()
                .HasOne(m => m.ZahlungsInfo)
                .WithMany(b => b.Besitzer);

            modelBuilder.Entity<Mitglied>()
                .HasOne(m => m.Familie)
                .WithMany(f => f.Mitglieder);


            modelBuilder.Entity<KursTeilnehmer>()
                .HasOne(kt => kt.Teilnehmer)
                .WithMany(m => m.Kurse);

            modelBuilder.Entity<KursTeilnehmer>()
                .HasOne(kt => kt.Kurse)
                .WithMany(k => k.Teilnehmer);

            modelBuilder.Entity<Helfer>()
                .HasOne(h => h.Arbeitseinsatz)
                .WithMany(a => a.Helfer);

            modelBuilder.Entity<Helfer>()
                .HasOne(h => h.Teilnehmer)
                .WithMany(m => m.Arbeitsstunden);

        }
    }
}