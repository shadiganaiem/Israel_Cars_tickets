using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ISRLCARS.Models;
using ISRLCARS.Database;
using MySql.Data.MySqlClient;

namespace ISRLCARS.Views
{
    public partial class TicketCheckByID : ContentPage
    {
        public ObservableCollection<ticket> tickets;
        DBConnection database = new DBConnection();

        public TicketCheckByID()
        {
            InitializeComponent();
        }

        async void checkID(object sender, System.EventArgs e)
        {
            tickets = new ObservableCollection<ticket>();
            ticketsList.ItemsSource = tickets;
            string message = "SELECT * FROM Entry_Tickets WHERE User_ID='{0}'";
            string query = string.Format(message, ID_entry.Text);
            MySqlCommand cmd = new MySqlCommand(query, database.getConn());
            cmd.ExecuteNonQuery();
            System.Data.DataTable dt = new System.Data.DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            int k = Convert.ToInt32(dt.Rows.Count.ToString());
            if (k == 0)
            {
                await DisplayAlert(null, "אין כרטיסים עבור תעודת זהות זו", "אישור");
                fullname.Text = "";
                UserID.Text = "";
                phone.Text = "";
                TicketsNum.Text = "";
                tickets = new ObservableCollection<ticket>();
                ticketsList.ItemsSource = tickets;
            }
            else {
                fullname.Text = dt.Rows[0][2].ToString();
                UserID.Text = dt.Rows[0][3].ToString();
                phone.Text = dt.Rows[0][5].ToString();
                TicketsNum.Text = "מספר כרטיסים: " + k.ToString();

                for (int i = 0; i < k; i++)
                {
                    tickets.Add(new ticket
                    {
                        barcode = dt.Rows[i][0].ToString(),
                        ticket_ID = dt.Rows[i][1].ToString(),
                        username = dt.Rows[i][2].ToString(),
                        User_ID = dt.Rows[i][3].ToString(),
                        email = dt.Rows[i][4].ToString(),
                        phone = dt.Rows[i][5].ToString(),
                        color = getColor(dt.Rows[i][0].ToString())
                    }) ;

                }
            }
        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            var menuItem = (MenuItem)sender;
            ticket tick = (ticket)menuItem.CommandParameter;
            if (ifEntered(tick.barcode))
            {
                await DisplayAlert(null, "בוצעה כניסה באמצעות כרטיס זה!", "אישור"); 
            }
            else
            {
                string message = "INSERT INTO Entered Values('{0}','{1}')";
                string query = string.Format(message, tick.barcode, tick.User_ID);
                MySqlCommand cmd = new MySqlCommand(query, database.getConn());
                cmd.ExecuteNonQuery();
                int i = tickets.IndexOf(tick);
                tick.color = "Red";
                tickets[i] = tick;
            }
            
        }

        private bool ifEntered(string barcode)
        {
            string message = "SELECT * FROM Entered WHERE barcode = '{0}'";
            string query = string.Format(message, barcode);
            MySqlCommand cmd = new MySqlCommand(query, database.getConn());
            cmd.ExecuteNonQuery();
            System.Data.DataTable dt = new System.Data.DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            int k = Convert.ToInt32(dt.Rows.Count.ToString());
            return k > 0;
        }

        private string getColor(string barcode)
        {
            string message = "SELECT * FROM Entered WHERE barcode = '{0}'";
            string query = string.Format(message, barcode);
            MySqlCommand cmd = new MySqlCommand(query, database.getConn());
            cmd.ExecuteNonQuery();
            System.Data.DataTable dt = new System.Data.DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            int k = Convert.ToInt32(dt.Rows.Count.ToString());
            if (k == 0)
            {
                return "Green";
            }

            return "Red";
            
        }
    }


}
