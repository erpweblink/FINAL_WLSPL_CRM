using WLSPL_CRM_2.Models;

namespace WLSPL_CRM_2.repository
{
    public interface IchatRepo
    {
        Task<int> submitchat(Message Model, string parametrs);
        Task<dynamic> Getchatbyid(string Id);
        Task<List<Message>> GetMessagesBetweenUsers(string senderId, string receiverId);
    }
}
