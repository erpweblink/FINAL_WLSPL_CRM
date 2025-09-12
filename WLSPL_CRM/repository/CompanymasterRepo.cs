using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using WLSPL_CRM_2.Models;
using static WLSPL_CRM_2.Models.Company;

namespace WLSPL_CRM_2.repository
{
    public class CompanymasterRepo : IcomapnymasterRepo
    {
        private readonly IConfiguration _configuration;
        public CompanymasterRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<Company>> checkcomapnies(string action, string company, string GstNo)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@Action", action);
                parameters.Add("@CompanyName", company);
                parameters.Add("@GSTNo", GstNo);
                var result = await connection.QueryAsync<Company>(
                    "SP_companymaster",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }
        public async Task<int> DeleteReord(string ID, string CreatedBy)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@id", ID);
                parameters.Add("@Action", "DeleteCompany");
                parameters.Add("@Deletedby", CreatedBy);
                parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.ExecuteAsync("SP_companymaster", parameters, commandType: CommandType.StoredProcedure);
                int isSuccess = parameters.Get<int>("@Result");
                return isSuccess;
            }
        }
        public async Task<dynamic> GetcompanybyId(string Id)
        {
            //using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
            //{
            //    await connection.OpenAsync();
            //    var parameters = new DynamicParameters();
            //    parameters.Add("@id", Id);
            //    parameters.Add("@Action", "Getcomapnybyid");
            //    var result = await connection.QueryFirstOrDefaultAsync<Company>(
            //        "SP_companymaster",
            //        parameters,
            //        commandType: CommandType.StoredProcedure);

            //    return result;
            //}

            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String"));
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@id", Id);
                parameters.Add("@Action", "Getcomapnybyid"); // Corrected spelling

                var data = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "SP_companymaster",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (data == null)
                    return null;

                var company = new Company
                {
                    Id = data.Id?.ToString(),
                    CompanyName = data.CompanyName,
                    CompanyCode = data.CompanyCode,
                    Registerfor = data.Registerfor,
                    supplytype = data.supplytype,
                    OwnerName = data.OwnerName,
                    GSTNo = data.GSTNo,
                    BillAddress = data.BillAddress,
                    ShippAddress = data.ShippAddress,
                    BillLocation = data.BillLocation,
                    ShippLocation = data.ShippLocation,
                    BillingPincode = data.BillingPincode,
                    ShippingPincode = data.ShippingPincode,
                    BillStateCode = data.BillStateCode,
                    ShippStateCode = data.ShippStateCode,
                    BillingAddress = data.BillingAddress,
                    ShippingAddress = data.ShippingAddress,
                    IsDeleted = data.IsDeleted,
                    CreatedBy = data.CreatedBy,
                    CreatedOn = data.CreatedOn,
                    UpdatedBy = data.UpdatedBy,
                    UpdatedOn = data.UpdatedOn,
                    DeletedOn = data.DeletedOn,
                    DeletedBy = data.DeletedBy,
                    AreaNAme = data.AreaNAme // spelling in DB?
                };

                void AddContact(string? name, string? mobile, string? email, string? designation)
                {
                    if (!string.IsNullOrWhiteSpace(name) ||
                        !string.IsNullOrWhiteSpace(mobile) ||
                        !string.IsNullOrWhiteSpace(email) ||
                        !string.IsNullOrWhiteSpace(designation))
                    {
                        company.Contacts.Add(new Company.ContactModel
                        {
                            Name = name,
                            MobileNo = mobile,
                            EmailID = email,
                            Designation = designation
                        });
                    }
                }

                AddContact(data.Name1, data.MobileNo1, data.EmailID1, data.Designation1);
                AddContact(data.Name2, data.MobileNo2, data.EmailID2, data.Designation2);
                AddContact(data.Name3, data.MobileNo3, data.EmailID3, data.Designation3);
                AddContact(data.Name4, data.MobileNo4, data.EmailID4, data.Designation4);
                AddContact(data.Name5, data.MobileNo5, data.EmailID5, data.Designation5);

                return company;
            }
            catch (Exception ex)
            {
                // Consider logging ex here
                throw;
            }



        }
        public async Task<List<Company>> Getcompcode(string Action)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@Action", Action);
                var result = await connection.QueryAsync<Company>(
                    "SP_Lead",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }
        public async Task<List<Company>> GetLeadlist(string Action, Company Model)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@Action", Action);
                var result = await connection.QueryAsync<Company>(
                    "SP_companymaster",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
            }
        }
        public async Task<int> SubmitDetails(Company Model, string Action)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyCode", Model.CompanyCode);
                    parameters.Add("@id", Model.Id);
                    parameters.Add("@CompanyName", Model.CompanyName);
                    parameters.Add("@Registertype", Model.Registerfor);
                    parameters.Add("@AreaNAme", Model.AreaNAme);
                    parameters.Add("@SupplyType", Model.supplytype);
                    parameters.Add("@OwnerName", Model.OwnerName);
                    parameters.Add("@GSTNo", Model.GSTNo);
                    parameters.Add("@ShippLocation", Model.ShippLocation);
                    parameters.Add("@BillLocation", Model.BillLocation);
                    parameters.Add("@Billingpincode", Model.BillingPincode);
                    parameters.Add("@shippingpincode", Model.ShippingPincode);
                    //parameters.Add("@Designation", Model.Designation1);
                    parameters.Add("@BillStateCode", Model.BillStateCode);
                    parameters.Add("@ShippStateCode", Model.ShippStateCode);
                    parameters.Add("@BillAddress", Model.BillingAddress);
                    parameters.Add("@shippaddress", Model.ShippingAddress);
                    //parameters.Add("@EmailID", Model.EmailID);
                    //parameters.Add("@MobileNo", Model.MobileNo);
                    //parameters.Add("@Designation", Model.Designation);
                    //parameters.Add("@Name", Model.Name);
                    //parameters.Add("@EmailID2", Model.EmailID1);
                    //parameters.Add("@MobileNo2", Model.MobileNo1);
                    //parameters.Add("@Designation2", Model.Designation1);
                    //parameters.Add("@Name2", Model.Designation1);

                    var contactTable = ToContactDataTable(Model.Contacts);
                    //parameters.Add("@Contacts", contactTable.AsTableValuedParameter("ContactTableType"));

                    if (Model.Contacts != null && Model.Contacts.Count > 0)
                    {
                        // First Contact (index 0)
                        if (Model.Contacts.Count >= 1)
                        {
                            var first = Model.Contacts[0];
                            parameters.Add("@EmailID", first.EmailID);
                            parameters.Add("@MobileNo", first.MobileNo);
                            parameters.Add("@Designation", first.Designation);
                            parameters.Add("Name", first.Name);
                        }

                        // Second Contact (index 1)
                        if (Model.Contacts.Count >= 2)
                        {
                            var second = Model.Contacts[1];
                            parameters.Add("@EmailID2", second.EmailID);
                            parameters.Add("@MobileNo2", second.MobileNo);
                            parameters.Add("@Designation2", second.Designation);
                            parameters.Add("Name2", second.Name);
                        }

                        // Third Contact (optional – example)
                        if (Model.Contacts.Count >= 3)
                        {
                            var third = Model.Contacts[2];
                            parameters.Add("@EmailID3", third.EmailID);
                            parameters.Add("@MobileNo3", third.MobileNo);
                            parameters.Add("@Designation3", third.Designation);
                            parameters.Add("@Name3", third.Name);
                        }
                    }





                    parameters.Add("@Action", Action);
                    //parameters.Add("@Createdby", Model.Createdby);
                    parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await connection.ExecuteAsync("SP_companymaster", parameters, commandType: CommandType.StoredProcedure);
                    int isSuccess = parameters.Get<int>("@Result");
                    return isSuccess;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<Company>> SearchCompanyAsync(string Action)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Conn_String")))
            {
                await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyName", Action);
                parameters.Add("@Action", "SearchCompany");

                var result = await connection.QueryAsync<Company>(
                    "SP_companymaster",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                    return result.ToList();
            }
        }

        private static DataTable ToContactDataTable(List<ContactModel> contacts)
        {
            var table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("MobileNo", typeof(string));
            table.Columns.Add("EmailID", typeof(string));
            table.Columns.Add("Designation", typeof(string));

            foreach (var contact in contacts)
            {
                table.Rows.Add(contact.Name, contact.MobileNo, contact.EmailID, contact.Designation);
            }

            return table;
        }
    }
}

