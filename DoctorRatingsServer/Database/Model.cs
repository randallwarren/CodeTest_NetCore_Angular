using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorRatingsServer.Database
{
	public class DoctorRatingContext : DbContext
	{
		public DbSet<Doctor> Doctors { get; set; }
		public DbSet<Specialty> Specialties { get; set; }
		public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }
		public DbSet<Language> Languages { get; set; }
		public DbSet<MedicalSchool> MedicalSchools { get; set; }
		public DbSet<PatientRating> PatientRatings { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=.\\Database\\DoctorRatings.sqlite3");
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DoctorSpecialty>()
				.HasKey(c => new { c.DoctorId, c.SpecialtyId });
		}
	}

	public class Doctor
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Gender { get; set; }
		public int MedicalSchoolId { get; set; }
		public int LanguageId { get; set; }
	}

	public class Specialty
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class DoctorSpecialty
	{
		// Composite Key via fluent API above in OnModelCreating()
		[Required]
		[ForeignKey("Doctor")]
		public int DoctorId { get; set; }
		[Required]
		[ForeignKey("Specialty")]
		public int SpecialtyId { get; set; }
	}

	public class Language
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class MedicalSchool
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class PatientRating
	{
		public int Id { get; set; }
		public int DoctorId { get; set; }
		public string Comments { get; set; }
		public int Rating { get; set; }
	}
}
