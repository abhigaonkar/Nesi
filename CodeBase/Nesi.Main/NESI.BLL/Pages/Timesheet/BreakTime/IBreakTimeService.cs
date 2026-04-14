using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Timesheet.BreakTime
{
    public interface IBreakTimeService
    {
        BreakTimeRecordRequirementRecord GetBranchBreakTimeRequirement(BranchBreakTimeRequirementInputParameter inputParameter);
        BreakTimeRecordResult AddBreakTimeRecord(BreakTimeRecord recordParameter);
        BreakTimeRecordResult UpdateBreakTimeRecord(BreakTimeRecord recordParameter);
        List<BreakTimeRecord> GetBreakTimeRecord(BreakTimeQueryParameter inputParameter);

        BreakTimeRecordOnGivenDate GetBreakTimeRequirementOnSpecificDate(BreakTimeQueryParameter inputParmeter);
    }
}
