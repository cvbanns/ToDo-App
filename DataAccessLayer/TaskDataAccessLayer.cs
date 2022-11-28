using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using ToDoApp.Models;
using System.Data;

namespace ToDoApp.DataAccessLayer
{
    public class TaskDataAccessLayer
    {
        string conString = ConfigurationManager.ConnectionStrings["todoConnectionstring"].ToString();

        //Get All The Tasks
        public List<MyTask> GetAllTheTasks()
        {
            List<MyTask> taskList = new List<MyTask>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetAllTheTasks";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                DataTable dtTasks = new DataTable();

                connection.Open();
                sqlDataAdapter.Fill(dtTasks);
                connection.Close();

                foreach (DataRow dataRow in dtTasks.Rows)
                {
                    taskList.Add(new MyTask
                    {
                        TaskID = Convert.ToInt32(dataRow["TaskID"]),
                        Task = dataRow["Task"].ToString(),
                        Details = dataRow["Details"].ToString(),
                        Due = dataRow["Due"].ToString(),
                        Completed = dataRow["Completed"].ToString()
                    });
                }
            }

            return taskList;
        }

        //Insert Task
        public bool InsertTask(MyTask myTask)
        {
            int id = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InsertTasks", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Task", myTask.Task);
                command.Parameters.AddWithValue("@Details", myTask.Details);
                command.Parameters.AddWithValue("@Due", myTask.Due);
                command.Parameters.AddWithValue("@Completed", myTask.Completed);

                connection.Open();
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            return id > 0;
        }

        //Get Tasks by Task ID
        public List<MyTask> GetTasksByID(int TaskID)
        {
            List<MyTask> taskList = new List<MyTask>();

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetTaskByID";
                command.Parameters.AddWithValue("@TaskID", TaskID);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                DataTable dtTasks = new DataTable();

                connection.Open();
                sqlDataAdapter.Fill(dtTasks);
                connection.Close();

                foreach (DataRow dataRow in dtTasks.Rows)
                {
                    taskList.Add(new MyTask
                    {
                        TaskID = Convert.ToInt32(dataRow["TaskID"]),
                        Task = dataRow["Task"].ToString(),
                        Details = dataRow["Details"].ToString(),
                        Due = dataRow["Due"].ToString(),
                        Completed = dataRow["Completed"].ToString()
                    });
                }
            }

            return taskList;
        }

        //InsUpdateert Task
        public bool UpdateTask(MyTask myTask)
        {
            int i = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateTasks", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TaskID", myTask.TaskID);
                command.Parameters.AddWithValue("@Task", myTask.Task);
                command.Parameters.AddWithValue("@Details", myTask.Details);
                command.Parameters.AddWithValue("@Due", myTask.Due);
                command.Parameters.AddWithValue("@Completed", myTask.Completed);

                connection.Open();
                i = command.ExecuteNonQuery();
                connection.Close();
            }
            return i > 0;
        }

        //Delete One Task
        public string DeleteTask(int taskid)
        {
            string result = "";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteTask", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@taskid", taskid);
                command.Parameters.Add("@OutputMessage", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                connection.Open();
                command.ExecuteNonQuery();
                result = command.Parameters["@OutputMessage"].Value.ToString();
                connection.Close();
            }
            return result;
        }

        public bool DeleteAllTasks()
        {
            int result = 0;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteAllTask", connection);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                result = command.ExecuteNonQuery();
                connection.Close();
            }
            return result != 0;
        }
    }
}