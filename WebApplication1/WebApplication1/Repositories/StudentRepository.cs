using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class StudentRepository
{
    private readonly IConfiguration _configuration;

    public StudentRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> doesStudentExist(int id) // -> przyjmuje to po czym sprawdzamy, raczej id
    {
        var query = "SELECT 1 FROM Students WHERE @id = ID";
	
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

    public async Task deleteStudent(int id)
    {
        var query1 = "DELETE FROM Students WHERE ID = @id";
        var query2 = "DELETE FROM GroupAssignments WHERE Student_ID = @id";
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query2;
        command.Parameters.AddWithValue("@id", id);
        await connection.OpenAsync();
        
        await command.ExecuteNonQueryAsync();
        command.CommandText = query1;
        await command.ExecuteNonQueryAsync();

    }
}