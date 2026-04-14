using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;

namespace NESI.BLL.Layout.Tickets
{
    public class Tickets : BLLBase
    {

        public Tickets(Employee user): base(user)
        {
        }


        public DTO.ViewModels.CurrentUser.Layout.Tickets[] GetTickets()
        {


            var tickets = _db.Database.SqlQuery<DTO.ViewModels.CurrentUser.Layout.Tickets>(@"call GETTICKETS( @p0, @p1)", CurrentUser.Id, "GetMyCourt").ToList(); 


            
            return tickets.ToArray();
        }
    }




}