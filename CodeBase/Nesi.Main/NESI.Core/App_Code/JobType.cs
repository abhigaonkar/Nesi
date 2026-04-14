using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nesi.core
{
    //public class JobTypeStuff
    //{
    //    //
    //    //  1540 Allow for membertype selection on the timesheet.
    //    //

    //    /*
    //     * Setup:
    //     *      Define memberType settings from member profile as the Main Membertype, call all stuff calculated based on this: M.
    //     *      Define memberType settings from UI(angular/mobile) as the Shadow Membertype, call all stuff calculated based on this: S.
    //     *      Define job type line as the (M.membtype_id, M.charegoutID, M.charegoutCost, M.wage, S.membtype_id, S.chargeoutID, S.charegoutCost, allow_jobtype_selection),
    //     *             call it JobLine.
    //     *      
    //     * Goal: 
    //     *     (1) When JobLine.allow_jobtype_selection is false, we will use (M.membtype_id, M.charegoutID, M.charegoutCost, M.wage) to create the timesheet records & labor lines.
    //     *     (2) When JobLine.allow_jobtype_selection is true,  we will use (S.membtype_id, S.charegoutID, S.charegoutCost, M.wage) to create the timesheet records & labor lines.
    //     *     
    //     * Note:
    //     *     (1) Goal one is already existing in our system.
    //     */
    //}

    public class JobTypeLine
    {
        public int membertype_id { get; set; }
        public int charegeout_id { get; set; }
        public double charegeout_cost { get; set; }

        public JobTypeLine()
        {
            this.membertype_id = 0;
            this.charegeout_id = 0;
            this.charegeout_cost = 0.0;
        }
    }
}
