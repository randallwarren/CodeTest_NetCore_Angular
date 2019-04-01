using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DoctorRatingsServer.Database;

namespace DoctorRatingsServer.Controllers
{
	public class DoctorDisplayRow
	{
		public string Id { get; set; } = "0";
		public string Name { get; set; } = "Unknown";
		public string Gender { get; set; } = "Unknown";
		public string Language { get; set; } = "Unknown";
		public string School { get; set; } = "Unknown";
		public string Specialties { get; set; } = "";
		public string AvgRating { get; set; } = "0";
		public string SuperStar { get; set; } = "false";
	}

	[Route("api/[controller]")]
	[ApiController]
	public class DoctorsController : ControllerBase
	{
		// GET api/doctors
		[HttpGet]
		public ActionResult<IEnumerable<DoctorDisplayRow>> Get()
		{
			var doctorRows = new List<DoctorDisplayRow>();
			using (var context = new DoctorRatingContext())
			{
				var doctors = (
				from d in context.Doctors
				join ln in context.Languages on d.LanguageId equals ln.Id
				join md in context.MedicalSchools on d.MedicalSchoolId equals md.Id
				join dxs in context.DoctorSpecialties on d.Id equals dxs.DoctorId
				join sp in context.Specialties on dxs.SpecialtyId equals sp.Id
				group sp.Name by new
				{
					d.Id,
					d.Name,
					d.Gender,
					Language = ln.Name,
					School = md.Name
				} into zrx
				select new
				{
					Id = zrx.Key.Id,
					Name = zrx.Key.Name,
					Gender = zrx.Key.Gender,
					Language = zrx.Key.Language,
					School = zrx.Key.School,
					Specialties = string.Join(", ", zrx.ToArray())
				}).ToList();

				foreach (var doctor in doctors)
				{
					DoctorDisplayRow docRow = new DoctorDisplayRow();
					docRow.Id = doctor.Id.ToString();
					docRow.Name = doctor.Name;
					docRow.Gender = doctor.Gender;
					docRow.Language = doctor.Language;
					docRow.School = doctor.School;
					docRow.Specialties = doctor.Specialties;
					double avgScore = DoctorReviews.GetAverageRating(doctor.Id);
					docRow.AvgRating = avgScore.ToString();
					docRow.SuperStar = DoctorReviews.IsSuperStar(avgScore).ToString();
					doctorRows.Add(docRow);
				}
			}
			return doctorRows.ToArray();
		}

		// GET api/doctors/id
		[HttpGet("{id}")]
		public ActionResult<DoctorDisplayRow> Get(int id)
		{
			var doctorRow = new DoctorDisplayRow();
			using (var context = new DoctorRatingContext())
			{
				var doctor = (
				from d in context.Doctors
				join ln in context.Languages on d.LanguageId equals ln.Id
				join md in context.MedicalSchools on d.MedicalSchoolId equals md.Id
				join dxs in context.DoctorSpecialties on d.Id equals dxs.DoctorId
				join sp in context.Specialties on dxs.SpecialtyId equals sp.Id
				group sp.Name by new
				{
					d.Id,
					d.Name,
					d.Gender,
					Language = ln.Name,
					School = md.Name
				} into zrx
				where zrx.Key.Id == id
				select new
				{
					Id = zrx.Key.Id,
					Name = zrx.Key.Name,
					Gender = zrx.Key.Gender,
					Language = zrx.Key.Language,
					School = zrx.Key.School,
					Specialties = string.Join(",", zrx.ToArray())
				}).FirstOrDefault();

				if (doctor != null)
				{
					doctorRow.Id = doctor.Id.ToString();
					doctorRow.Name = doctor.Name;
					doctorRow.Gender = doctor.Gender;
					doctorRow.Language = doctor.Language;
					doctorRow.School = doctor.School;
					doctorRow.Specialties = doctor.Specialties;
					double avgScore = DoctorReviews.GetAverageRating(doctor.Id);
					doctorRow.AvgRating = avgScore.ToString();
					doctorRow.SuperStar = DoctorReviews.IsSuperStar(avgScore).ToString();


				}
			}
			return doctorRow;
		}
	}

	public class DoctorReviews
	{
		public const double MIN_SUPER_STAR = 4.25; // 85% of 5 stars...

		public static bool IsSuperStar(double score) { return (score > MIN_SUPER_STAR); }

		public static double GetAverageRating(int doctorId)
		{
			double result = 0;
			using (var context = new DoctorRatingContext())
			{
				var avgRating = (
				from d in context.Doctors
				join rv in context.PatientRatings on d.Id equals rv.DoctorId
				where rv.DoctorId == doctorId
				group rv.Rating by rv.DoctorId into avr
				select new
				{
					AvgRating = avr.Average()
				}).FirstOrDefault();

				if (avgRating != null)
				{
					result = avgRating.AvgRating;
				}
			}
			return result;
		}
	}
}
