using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using WLSPL_CRM_2.Models;

namespace WLSPL_CRM_2.repository
{
    public class ChatRepo : IchatRepo
    {
        private readonly IConfiguration _configuration;

        public ChatRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<dynamic> Getchatbyid(string Id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@SenderId", Id);
                parameters.Add("@Action", "Getsendermessage");
                var result = await connection.QueryFirstOrDefaultAsync<Message>(
                    "SP_chat",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result;

            }
        }

        public async Task<List<Message>> GetMessagesBetweenUsers(string senderId, string receiverId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@SenderId", senderId);
                parameters.Add("@ReceiverId", receiverId);
                parameters.Add("@Action", "GetChatHistory"); 

                var result = await connection.QueryAsync<Message>(
                    "SP_chat",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public async Task<int> submitchat(Message model, string parameters)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
            {
                await connection.OpenAsync();
                var param = new DynamicParameters();
                param.Add("@SenderId", model.SenderId);
                param.Add("@ReceiverId", model.ReceiverId);
                param.Add("@MessageText", model.MessageText);
                param.Add("@Timestamp", model.Timestamp);
                param.Add("@Action", "Insert");
                param.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync("SP_chat", param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("@Result");
            }
        }

    }
}
