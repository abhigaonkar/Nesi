using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;

namespace NESI.BLL.Layout.Banner
{
    public class Message: BLLBase
    {

        public Message(Employee user): base(user)
        {
        }
        

        public DTO.ViewModels.CurrentUser.Layout.Message[] GetMessages()
        {

            var messagesdt = _db.Database.SqlQuery<DTO.ViewModels.CurrentUser.Layout.Message>(
                @"SELECT
  MType.MessageType AS Type,
  member_name (MTo.MessageTo_ID) ToName,
  MTo.MessageTo_ID ToId,
  member_name (mem.member_id) FromName,
  mem.member_id FromId,
  M.Message_Subject Subject,
  M.Date AS Date,
  MTo.Message_Status AS Status  
FROM
  messageto MTo
  INNER JOIN Message M
    ON M.Message_ID = MTo.MessageTo_Message_ID
  INNER JOIN Member Mem
    ON Mem.Member_ID = M.Message_LeftBy_Member_ID
  INNER JOIN MessageType MType
    ON MType.MessageType_ID = M.MessageType_ID
WHERE MTo.MessageTo_Member_ID_To = @p0
  AND MTo.Message_Status != 'Deleted'
  AND MTo.Message_Status != 'Read' 
ORDER BY MessageTo_ID DESC",
                CurrentUser.Id).ToList();

            return messagesdt.ToArray();
        }
        
    }




}