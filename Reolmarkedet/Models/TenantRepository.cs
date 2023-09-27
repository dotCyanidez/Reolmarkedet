using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarkedet.Models
{
    public partial class TenantRepository : ObservableObject, BaseRepositoryInterface
    {
        private ObservableCollection<Tenant> _tenants = new();

        public TenantRepository() { }

        public void AddTenant(string name, string contactNr, string email)
        {
            Tenant tenant = new(name,contactNr,email);

            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO TENANT(Name, ContactNr, Email)"
                        + "VALUES(@name,@contactNr,@email) SELECT SCOPE_IDENTITY()", con);
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 200).Value = tenant.Name;
                    cmd.Parameters.Add("@contactNr", SqlDbType.NVarChar, 200).Value = tenant.ContactNr;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 200).Value = tenant.Email;
                    tenant.ID = Convert.ToInt32(cmd.ExecuteScalar());
                    _tenants.Add(tenant);
                    con.Close();

                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public void RemoveTenant(int id) 
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE TENANT WHERE ID  = @id", con);
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public async void UpdateTenant(Tenant tenant)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE SALE SET"
                        + " Name = @name, ContactNr = @contactNr, Email = @email"
                        + " WHERE ID = @id", con);
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = tenant.Name;
                    cmd.Parameters.Add("@contactNr", SqlDbType.NVarChar).Value = tenant.ContactNr;
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = tenant.Email;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = tenant.ID;
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();
                }
            }
            catch (Exception e )
            {

                throw new Exception(e.Message);
            }
        }
        
        public async void GetAllTenants()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseRepositoryInterface._connectionString))
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand("SELECT ID, Name, ContactNr, Email FROM TENANT", con);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {                   // skal muligvis lave en constructor der tager færre parameters
                            Tenant tenant = new( 
                                reader[nameof(tenant.Name)].ToString(),
                                reader[nameof(tenant.ContactNr)].ToString(),
                                reader[nameof(tenant.Email)].ToString()
                                );
                            tenant.ID = Convert.ToInt32(reader[nameof(tenant.ID)]);
                            _tenants.Add(tenant);
                        }
                    }
                    await con.CloseAsync();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public Tenant GetTenant(int id)
        {
            Tenant result = _tenants.FirstOrDefault(x => x.ID == id);

            return result == null ? throw new Exception("Lejer ikke fundet") : result;
        }
    }
}
