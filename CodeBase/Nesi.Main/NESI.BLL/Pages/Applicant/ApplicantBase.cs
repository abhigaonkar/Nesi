using NESI.BLL.Base;
using NESI.BLL.Core.Employee;

namespace NESI.BLL.Pages.Applicant
{
	public class ApplicantBase : BLLBase
	{
		public int applicant_id { get; set; }

		public ApplicantBase(Employee user) : base(user)
		{

		}

		public ApplicantBase(Employee user, int applicantId) : base(user)
		{
			applicant_id = applicantId;
		}

	

		public virtual object Profile()
		{
			return this;
		}
	}
}