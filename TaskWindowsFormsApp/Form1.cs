using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskWindowsFormsApp
{
    public partial class Form1 : Form
    {

        private static string dbCommand = "";
        private static BindingSource bindingSrc;

        private static string dbPath = Application.StartupPath + "\\" + "TaskManagerDB.db";
        private static string conString = "Data Source=" + dbPath + ";Version=3;New=False;Compress=True";

        private static SQLiteConnection connection = new SQLiteConnection(conString);
        private static SQLiteCommand command = new SQLiteCommand("", connection);
        private static string sql;

        public Form1()
        {
            InitializeComponent();
            this.autoIDTextBox.Enabled = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openConnection();

            updateDataBiding();

            closeConnection();
        }

        

        private void closeConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void openConnection()
        {
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        private void exitButtion_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void displayPosition()
        {
            PositionLabel.Text = "Task: " + Convert.ToString(bindingSrc.Position + 1) + "/" + bindingSrc.Count.ToString();
        }

        private void updateDataBiding(SQLiteCommand cmd = null)
        {
            try
            {

                TextBox tb;
                foreach(Control c in groupBox1.Controls)
                {
                    if(c.GetType() == typeof(TextBox))
                    {
                        tb = (TextBox)c;
                        tb.DataBindings.Clear();
                        tb.Text = "";
                    }
                }

                dbCommand = "SELECT";
                sql = "SELECT * FROM tasks ORDER BY AutoID ASC;";

                if(cmd == null)
                {
                    command.CommandText = sql;
                }
                else
                {
                    command = cmd;
                }

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataSet dataSt = new DataSet();
                adapter.Fill(dataSt, "Tasks");

                bindingSrc = new BindingSource();
                bindingSrc.DataSource = dataSt.Tables["Tasks"];


                // Data bindings
                autoIDTextBox.DataBindings.Add("Text", bindingSrc, "AutoID");
                taskNameTextBox.DataBindings.Add("Text", bindingSrc, "TaskName");
                dateTextBox.DataBindings.Add("Text", bindingSrc, "Date");
                descriptionTextBox.DataBindings.Add("Text", bindingSrc, "Description");
                firstNameTextBox.DataBindings.Add("Text", bindingSrc, "FirstName");
                lastNameTextBox.DataBindings.Add("Text", bindingSrc, "LastName");

                dataGridView1.Enabled = true;
                dataGridView1.DataSource = bindingSrc;
                dataGridView1.AutoResizeColumns((DataGridViewAutoSizeColumnsMode)DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                dataGridView1.Columns[0].Width = 70;

                // Hide the AutoID colomn because this isn't relevant for the user.
                dataGridView1.Columns["AutoID"].Visible = false;


                displayPosition();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Binding Error: " + ex.Message.ToString(), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dateTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void moveFirstButton_Click(object sender, EventArgs e)
        {
            bindingSrc.MoveFirst();
            displayPosition();
        }

        private void movePreviousButton_Click(object sender, EventArgs e)
        {
            bindingSrc.MovePrevious();
            displayPosition();
        }

        private void moveNextButton_Click(object sender, EventArgs e)
        {
            bindingSrc.MoveNext();
            displayPosition();
        }

        private void moveLastbutton_Click(object sender, EventArgs e)
        {
            bindingSrc.MoveLast();
            displayPosition();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                displayPosition();
            }
            catch (Exception)
            {
              
            }
        }

        private void refreshDataButton_Click(object sender, EventArgs e)
        {
            if (addNewButton.Text.Equals("Cancel"))
            {
                return;
            }
            updateDataBiding();
        }

        private void addNewButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(addNewButton.Text == "Add Task")
                {
                    addNewButton.Text = "Cancel";
                    PositionLabel.Text = "Task: 0/0";
                    dataGridView1.ClearSelection();
                    dataGridView1.Enabled = false;
                }
                else
                {
                    addNewButton.Text = "Add Task";
                    updateDataBiding();
                    return;
                }
                TextBox txt;
                foreach(Control c in groupBox1.Controls)
                {
                    if (c.GetType() == typeof(TextBox))
                    {
                        txt = (TextBox)c;
                        txt.DataBindings.Clear();
                        txt.Text = "";
                        if (txt.Name.Equals("firstNameTextBox"))
                        {
                            if (txt.CanFocus)
                            {
                                txt.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void addCmdParameters()
        {
            command.Parameters.Clear();
            command.CommandText = sql;

            command.Parameters.AddWithValue("TaskName", taskNameTextBox.Text.Trim());
            command.Parameters.AddWithValue("Date", dateTextBox.Text.Trim());
            command.Parameters.AddWithValue("Description", descriptionTextBox.Text.Trim());
            command.Parameters.AddWithValue("FirstName", firstNameTextBox.Text.Trim());
            command.Parameters.AddWithValue("LastName", lastNameTextBox.Text.Trim());

            if (dbCommand.ToUpper() == "UPDATE")
            {
                command.Parameters.AddWithValue("AutoID", autoIDTextBox.Text.Trim());
                

            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(taskNameTextBox.Text.Trim()) ||
                    string.IsNullOrEmpty(dateTextBox.Text.Trim()) ||
                    string.IsNullOrEmpty(descriptionTextBox.Text.Trim()) ||
                    string.IsNullOrEmpty(firstNameTextBox.Text.Trim()) ||
                    string.IsNullOrEmpty(lastNameTextBox.Text.Trim()))
            {
                MessageBox.Show("Fill in all fields to proceed!", "New task", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            openConnection();

            try
            {
                if (addNewButton.Text == "Add Task")
                {
                    if (autoIDTextBox.Text.Trim() == "" || string.IsNullOrEmpty(autoIDTextBox.Text.Trim()))
                    {
                        MessageBox.Show("Select an item!");
                        return;
                    }
                    if (MessageBox.Show("Task: " + taskNameTextBox.Text.Trim() + ". Do you want to update this task?",
                        "Update task", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }

                    dbCommand = "UPDATE";
                    sql = "UPDATE tasks SET TaskName = @TaskName, Date = @Date, Description = @Description, FirstName = @FirstName, LastName = @LastName WHERE AutoID = @AutoID";

                    addCmdParameters();
                   
                }
                else if(addNewButton.Text.Equals("Cancel"))
                {
                    DialogResult result;
                    result = MessageBox.Show("Do you want to add a new task?", "Saving task.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        dbCommand = "INSERT";
                        sql = "INSERT INTO tasks(TaskName, Date, Description, FirstName, LastName) " +
                            "VALUES(@TaskName, @Date, @Description, @FirstName, @LastName)";

                        addCmdParameters();
                    }
                    else
                    {
                        return;
                    }
                }

                int executeResult = command.ExecuteNonQuery();
                if (executeResult == -1)
                {
                    MessageBox.Show("Something went wrong.", "Task was not saved!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show("Task has been saved!","Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    updateDataBiding();

                    addNewButton.Text = "Add Task";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message.ToString(), "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbCommand = "";
                closeConnection();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (addNewButton.Text == "Cancel")
            {
                return;
            }
            {
                if(autoIDTextBox.Text.Trim() == "" ||
                    string.IsNullOrEmpty(autoIDTextBox.Text.Trim()))
                {
                    MessageBox.Show("Select a task you want to delete?", "Delete task.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                openConnection();

                try
                {
                    if(MessageBox.Show("Task: " + taskNameTextBox.Text.Trim() + 
                        ". Do you want to delete this task?", "Delete task.", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, 
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }

                    dbCommand = "DELETE";
                    sql = "DELETE FROM tasks WHERE AutoID = @AutoID";

                    command.Parameters.Clear();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("AutoID", autoIDTextBox.Text.Trim());

                    int executeResult = command.ExecuteNonQuery();
                    if (executeResult == 1)
                    {
                        MessageBox.Show("Your Task has been deleted successfully.",
                            "Task.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updateDataBiding();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message.ToString(),
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {

                }
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (addNewButton.Text == "Cancel")
            {
                return;
            }
            openConnection();

            try
            {
                if (string.IsNullOrEmpty(SearchTextBox.Text.Trim()))
                {
                    updateDataBiding();
                    return;
                }

                sql = "SELECT * FROM tasks WHERE " +
                      "FirstName LIKE @Keyword2 OR " +
                      "LastName LIKE @Keyword2 OR " +
                      "TaskName LIKE @Keyword2 OR " +
                      "Date LIKE @Keyword2 OR " +
                      "Description LIKE @Keyword2 " +
                      "ORDER BY AutoID ASC";

                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.Parameters.Clear();

                string keyword = string.Format("%{0}%", SearchTextBox.Text);
                command.Parameters.AddWithValue("@Keyword2", keyword);

                openConnection();

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Close();
                    updateDataBiding(command);
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("No results found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                closeConnection();
                SearchTextBox.Focus();
            }

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
