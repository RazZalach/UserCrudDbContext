


using System;
using System.Data.SqlClient;

namespace losseon
{
    class SchoolDbContext
    {

       public SqlConnection sqlConnection;
       public  string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\Documents\\school.mdf;Integrated Security=True;Connect Timeout=30";
        public SchoolDbContext()
        {
            try
            {
                do
                {
                    sqlConnection = new SqlConnection(ConnectionString);
                    sqlConnection.Open();
                    Console.WriteLine("Connection Established successfully");
                    // call crud function according to input
                    Console.WriteLine("Choose 1 for Pupils Crud, 2 for Teachers Crud, or any other key to exit");
                    if (int.TryParse(Console.ReadLine(), out int crudChoose))
                    {
                        if (crudChoose == 1)
                        {
                            pupilCrud();
                        }
                        else if (crudChoose == 2)
                        {
                            teacherCrud();
                        }
                        else
                        {
                            Console.WriteLine("Exiting program...");
                            break; // Exit the loop if the input is neither 1 nor 2
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Exiting program...");
                        break; // Exit the loop for invalid input
                    }

                    Console.WriteLine("Do you want to perform another operation? Press Y to continue, any other key to exit.");
                } while (Console.ReadKey().Key == ConsoleKey.Y);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
            finally
            {
                // Close the connection in the finally block to ensure proper cleanup
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
            }
        }

        public void teacherCrud()
        {
            Console.WriteLine("choose action: \n " +
                  "1 for Insert new Teacher \n" +
                  "2 for Update Teacher \n" +
                  "3 for delete Teacher \n" +
                  "4 for get Teacher by name \n" +
                  "5 for get all Teacher \n" +
                  "6 for get Pupils with Teacher Details \n" +
                  "7 for get Average Grades by Subject");
            int choose = int.Parse(Console.ReadLine());
            switch (choose)
            {
                case 1:
                    insertNewTeacher();
                    break;
                case 2:
                    updateTeacher();
                    break;
                case 3:
                    deleteTeacher();
                    break;
                case 4:
                    getTeacherById();
                    break;
                case 5:
                    getAllTeachers();
                    break;
                case 6:
                    GetPupilsWithTeachersDetails();
                    break;
                case 7:
                    GetAverageGradesBySubject();
                    break;

            }
        }
        public void getAllTeachers()
        {
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                string query = "SELECT * FROM Teachers";

                command = new SqlCommand(query, sqlConnection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    string name = reader["name"].ToString();
                    string subject = reader["subject"].ToString();
                    Teacher teacher = new Teacher(id, name, subject);
                    Console.WriteLine($"Teacher ID: {teacher.id}, Name: {teacher.name}, Subject: {teacher.subject}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving teachers: {ex.Message}");
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (command != null)
                {
                    command.Dispose();
                }
            }
        }



        public void getTeacherById()
        {

            SqlCommand command = null;
            try
            {
                Console.WriteLine("Enter teacher's ID:");
                int teacherId = int.Parse(Console.ReadLine());
                string query = $"SELECT * FROM Teachers WHERE id = {teacherId}";

                command = new SqlCommand(query, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader["id"]);
                    string name = reader["name"].ToString();
                    string subject = reader["subject"].ToString();

                    // Create a Teacher object with retrieved values
                    Teacher teacher = new Teacher(id, name, subject);

                    // Display the details of the teacher
                    Console.WriteLine($"Teacher ID: {teacher.id}, Name: {teacher.name}, Subject: {teacher.subject}");
                }
                else
                {
                    Console.WriteLine("Teacher not found with the provided ID.");
                }

                // Close the reader manually
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving teacher by ID: {ex.Message}");
            }
            finally
            {
                // Dispose of the command
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }



        public void deleteTeacher()
        {
            SqlCommand command = null;

            try
            {
                Console.WriteLine("Enter teacher's ID to delete:");
                int teacherId = int.Parse(Console.ReadLine());

                // Construct the SQL query
                string query = $"DELETE FROM Teachers WHERE id = {teacherId}";

                command = new SqlCommand(query, sqlConnection);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Teacher successfully deleted from the database.");
                }
                else
                {
                    Console.WriteLine("No teacher found with the provided ID. Deletion failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting teacher: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }


        public void updateTeacher()
        {
            SqlCommand command = null;

            try
            {
                Console.WriteLine("Enter teacher's ID to update:");
                int teacherId = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter new name for the teacher:");
                string newName = Console.ReadLine();

                Console.WriteLine("Enter new subject for the teacher:");
                string newSubject = Console.ReadLine();

                // Construct the SQL query
                string query = $"UPDATE Teachers SET name = '{newName}', subject = '{newSubject}' WHERE id = {teacherId}";

                command = new SqlCommand(query, sqlConnection);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Teacher details successfully updated in the database.");
                }
                else
                {
                    Console.WriteLine("No teacher found with the provided ID. Update failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating teacher: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }



        public void insertNewTeacher()
        {
            SqlCommand command = null;

            try
            {
                Console.WriteLine("Enter teacher's name:");
                string teacherName = Console.ReadLine();

                Console.WriteLine("Enter teacher's subject:");
                string teacherSubject = Console.ReadLine();

                // Construct the SQL query
                string query = $"INSERT INTO Teachers (name, subject) VALUES ('{teacherName}', '{teacherSubject}')";

                command = new SqlCommand(query, sqlConnection);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Teacher successfully inserted into the database.");
                }
                else
                {
                    Console.WriteLine("Failed to insert the teacher into the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting new teacher: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }



        public void pupilCrud()
        {
            Console.WriteLine("choose action: \n " +
                  "1 for Insert new Pupil \n" +
                  "2 for Update Pupil \n" +
                  "3 for delete Pupil \n" +
                  "4 for get pupil by name \n" +
                  "5 for get all pupils");
            int choose = int.Parse(Console.ReadLine());
            switch (choose)
            {
                case 1:
                    insertNewPupil();
                    break;
                case 2:
                    updatePupil();
                    break;
                case 3:
                    deletePupil();
                    break;
                case 4:
                    getPupilById();
                    break;
                case 5:
                    getAllPupils();
                    break;
            }
        }

        public void insertNewPupil()
        {
            SqlCommand command = null;

            try
            {
                Console.WriteLine("Enter pupil's name:");
                string pupilName = Console.ReadLine();

                Console.WriteLine("Enter teacher's ID for the pupil:");
                int teacherId = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter pupil's grade:");
                int pupilGrade = int.Parse(Console.ReadLine());

                // Construct the SQL query
                string query = $"INSERT INTO Pupils (name, teacher_id, grade) VALUES ('{pupilName}', {teacherId}, {pupilGrade})";

                command = new SqlCommand(query, sqlConnection);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Pupil successfully inserted into the database.");
                }
                else
                {
                    Console.WriteLine("Failed to insert the pupil into the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting new pupil: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }


        public void updatePupil()
        {
            SqlCommand command = null;

            try
            {
                getAllPupils();
                Console.WriteLine("Enter pupil's ID to update:");
                int pupilId = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter new name for the pupil:");
                string newName = Console.ReadLine();

                getAllTeachers();
                Console.WriteLine("Enter new teacher's ID for the pupil:");
                int newTeacherId = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter new grade for the pupil:");
                int newGrade = int.Parse(Console.ReadLine());

                // Construct the SQL query
                string query = $"UPDATE Pupils SET name = '{newName}', teacher_id = {newTeacherId}, grade = {newGrade} WHERE id = {pupilId}";

                command = new SqlCommand(query, sqlConnection);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Pupil details successfully updated in the database.");
                }
                else
                {
                    Console.WriteLine("No pupil found with the provided ID. Update failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating pupil: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }


        public void deletePupil()
        {
            SqlCommand command = null;

            try
            {
                Console.WriteLine("Enter pupil's ID to delete:");
                int pupilId = int.Parse(Console.ReadLine());

                // Construct the SQL query
                string query = $"DELETE FROM Pupils WHERE id = {pupilId}";

                command = new SqlCommand(query, sqlConnection);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("Pupil successfully deleted from the database.");
                }
                else
                {
                    Console.WriteLine("No pupil found with the provided ID. Deletion failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting pupil: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }


        public void getPupilById()
        {
            SqlCommand command = null;

            try
            {
                Console.WriteLine("Enter pupil's ID:");
                int pupilId = int.Parse(Console.ReadLine());

                // Construct the SQL query
                string query = $"SELECT * FROM Pupils WHERE id = {pupilId}";

                command = new SqlCommand(query, sqlConnection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string name = reader["name"].ToString();
                        int teacherId = Convert.ToInt32(reader["teacher_id"]);
                        int grade = Convert.ToInt32(reader["grade"]);

                        // Create a Pupil object with retrieved values
                        Pupil pupil = new Pupil(id, name, teacherId, grade);

                        // Display the details of the pupil
                        Console.WriteLine($"Pupil ID: {pupil.id}, Name: {pupil.name}, Pupil ID: {pupil.teacher_id}, Grade: {pupil.grade}");
                    }
                    else
                    {
                        Console.WriteLine("Pupil not found with the provided ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving pupil by ID: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }


        public void getAllPupils()
        {
            SqlCommand command = null;

            try
            {
                string query = "SELECT Pupils.*, Teachers.name AS teacher_name, Teachers.subject AS teacher_subject " +
                               "FROM Pupils " +
                               "INNER JOIN Teachers ON Pupils.teacher_id = Teachers.id";

                command = new SqlCommand(query, sqlConnection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string name = reader["name"].ToString();
                        int teacherId = Convert.ToInt32(reader["teacher_id"]);
                        int grade = Convert.ToInt32(reader["grade"]);
                        string teacherName = reader["teacher_name"].ToString();
                        string teacherSubject = reader["teacher_subject"].ToString();

                        // Display the details of the pupil and teacher
                        Console.WriteLine($"Pupil ID: {id}, Name: {name}, Teacher ID: {teacherId}, Grade: {grade}");
                        Console.WriteLine($"Teacher Name: {teacherName}, Subject: {teacherSubject}");
                        Console.WriteLine("----------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all pupils: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }


        ////////////////////////////
        ///
        public void GetPupilsWithTeachersDetails()
        {
            SqlCommand command = null;

            try
            {
                string query = "SELECT Pupils.*, Teachers.name AS teacher_name, Teachers.subject AS teacher_subject " +
                               "FROM Pupils " +
                               "INNER JOIN Teachers ON Pupils.Teacher_id = Teachers.id";

                command = new SqlCommand(query, sqlConnection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string name = reader["name"].ToString();
                        int teacherId = Convert.ToInt32(reader["teacher_id"]);
                        int grade = Convert.ToInt32(reader["grade"]);
                        string teacherName = reader["teacher_name"].ToString();
                        string teacherSubject = reader["teacher_subject"].ToString();

                        // Display the details of the pupil and teacher
                        Console.WriteLine($"Pupil ID: {id}, Name: {name}, Teacher ID: {teacherId}, Grade: {grade}");
                        Console.WriteLine($"Teacher Name: {teacherName}, Subject: {teacherSubject}");
                        Console.WriteLine("----------------------------");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving pupils with teacher details: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }


        public void GetAverageGradesBySubject()
        {
            SqlCommand command = null;

            try
            {
                string query = "SELECT Teachers.subject, AVG(Pupils.grade) AS average_grade, Teachers.name AS teacher_name " +
                               "FROM Pupils " +
                               "INNER JOIN Teachers ON Pupils.teacher_id = Teachers.id " +
                               "GROUP BY Teachers.subject, Teachers.name";

                command = new SqlCommand(query, sqlConnection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string subject = reader["subject"].ToString();
                        double averageGrade = Convert.ToDouble(reader["average_grade"]);
                        string teacherName = reader["teacher_name"].ToString();

                        // Display the average grade for each subject and teacher
                        Console.WriteLine($"Subject: {subject}, Average Grade: {averageGrade}, Teacher Name: {teacherName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving average grades by subject: {ex.Message}");
            }
            finally
            {
                // Dispose of the command manually
                if (command != null)
                {
                    command.Dispose();
                }
            }
        }

    }
}
