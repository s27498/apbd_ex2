using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;
using Group = WebApplication1.Models.Group;

namespace WebApplication1.Repositories;

public class GroupRepository
{
    private readonly IConfiguration _configuration;

    public GroupRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> doesGroupExist(int id) // -> przyjmuje to po czym sprawdzamy, raczej id
    {
        var query = "SELECT 1 FROM Groups WHERE @id = ID";
	
        //// sql connection ////
	
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);
        await connection.OpenAsync();		
        var reader = await command.ExecuteScalarAsync();
		
        if (reader == null)
        {
            return false;
        }

        return true;
    }
    public async Task<Group> getGroup(int id)
    {
        var query = "SELECT G.ID AS ID, G.NAME AS Name, GA.Student_ID as Students FROM Groups G JOIN dbo.GroupAssignments GA on G.ID = GA.Group_ID WHERE G.ID = @id";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);
        await connection.OpenAsync();
        
        var reader = await command.ExecuteReaderAsync();
        var groupIdOrdinal = reader.GetOrdinal("ID"); // -> nazwy w nawiasie takie same jak nazwy tabel
        var groupNameOrdinal = reader.GetOrdinal("Name");
        var studentIdOrdinal = reader.GetOrdinal("Students");

        Group group = null;
        while (await reader.ReadAsync())
        {
            if (group == null)
            {
                group = new Group()
                {
                    GroupID = reader.GetInt32(groupIdOrdinal),
                    Name = reader.GetString(groupNameOrdinal),
                    StudentsIDs = new List<int>()
                    {
                        
                        reader.GetInt32(studentIdOrdinal)
                        
                    }
                };
            }
            else
            {
                group.StudentsIDs.Add(reader.GetInt32(studentIdOrdinal));

            }    
            
        }

        return group;
    }
    
}