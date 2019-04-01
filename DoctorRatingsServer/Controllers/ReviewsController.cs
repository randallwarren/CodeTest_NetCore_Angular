using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DoctorRatingsServer.Database;

namespace DoctorRatingsServer.Controllers
{
	public class ReviewDisplayRow
	{
		public string Id { get; set; } = "0";
		public string DoctorId { get; set; } = "0";
		public string Comments { get; set; } = "";
		public string Rating { get; set; } = "0";
	}

	[Route("api/[controller]")]
	[ApiController]
	public class ReviewsController : ControllerBase
	{
		// GET api/reviews
		[HttpGet]
		public ActionResult<IEnumerable<ReviewDisplayRow>> Get()
		{
			var reviewRows = new List<ReviewDisplayRow>();
			using (var context = new DoctorRatingContext())
			{
				var reviews = (
				from d in context.Doctors
				join rv in context.PatientRatings on d.Id equals rv.DoctorId
				select new
				{
					Id = rv.Id,
					DoctorId = rv.DoctorId,
					Comments = rv.Comments,
					Rating = rv.Rating
				}).ToList();

				foreach (var review in reviews)
				{
					ReviewDisplayRow revRow = new ReviewDisplayRow();
					revRow.Id = review.Id.ToString();
					revRow.DoctorId = review.DoctorId.ToString();
					revRow.Comments = review.Comments;
					revRow.Rating = review.Rating.ToString();
					reviewRows.Add(revRow);
				}
			}
			return reviewRows.ToArray();
		}

		// GET api/reviews/id
		[HttpGet("{id}")]
		public ActionResult<IEnumerable<ReviewDisplayRow>> Get(int id)
		{
			var reviewRows = new List<ReviewDisplayRow>();
			using (var context = new DoctorRatingContext())
			{
				var reviews = (
				from d in context.Doctors
				join rv in context.PatientRatings on d.Id equals rv.DoctorId
				where rv.DoctorId == id
				select new
				{
					Id = rv.Id,
					DoctorId = rv.DoctorId,
					Comments = rv.Comments,
					Rating = rv.Rating
				}).ToList();

				foreach (var review in reviews)
				{
					ReviewDisplayRow revRow = new ReviewDisplayRow();
					revRow.Id = review.Id.ToString();
					revRow.DoctorId = review.DoctorId.ToString();
					revRow.Comments = review.Comments;
					revRow.Rating = review.Rating.ToString();
					reviewRows.Add(revRow);
				}
			}
			return reviewRows.ToArray();
		}
	}
}
