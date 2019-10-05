using System;
using System.Collections.Generic;
using System.Data;
using ISRLCARS.Database;
using ISRLCARS.Services;
using MySql.Data.MySqlClient;
using Xamarin.Forms;

namespace ISRLCARS.Views
{
    public partial class TicketCheck : ContentPage
    {

        DBConnection database = new DBConnection();
        string QR;
        public TicketCheck()
        {
            InitializeComponent();
        }

        async void btnScan_clicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.scanAsync();
                if (result != null)
                {
                    QR = result;
                    if (!IfQRExist(result))
                    {
                        await DisplayAlert(null, "אין פרטים עבור כרטיס זה", "אישור");
                        permission.Text = "כרטיס לא מאושר";
                        fullname.Text = "";
                        UserID.Text = "";
                        phone.Text = "";
                        ticketNum.Text = "";
                        permission.TextColor = Color.Red;
                    }
                }
                else
                {
                    await DisplayAlert(null, "סריקה נכשלה", "אישור");
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
        }

        private  bool IfQRExist(string barcode)
        {


            string message = "SELECT * FROM Entry_Tickets WHERE barcode='{0}'";
            string query = string.Format(message, barcode);
            MySqlCommand cmd = new MySqlCommand(query, database.getConn());
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            int i = Convert.ToInt32(dt.Rows.Count.ToString());
            if (i < 1)
                return false;

            
            if (checkEntry() > 0)
            {
                permission.Text = "בוצעה כניסה באמצעות כרטיס זה!";
                permission.TextColor = Color.Red;

            }
            else
            {
                permission.Text = "כרטיס מאושר";
                permission.TextColor = Color.Green;
                enter.IsVisible = true;
            }
            fullname.Text = dt.Rows[0][2].ToString();
            UserID.Text = dt.Rows[0][3].ToString();
            phone.Text = dt.Rows[0][5].ToString();
            string txt = "כרטיס מספר: " + dt.Rows[0][1].ToString();
            ticketNum.Text = txt;

            

            return true;

        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {

            string message = "INSERT INTO Entered VALUES('{0}','{1}')";
            string query = string.Format(message, QR, UserID.Text);
            MySqlCommand cmd = new MySqlCommand(query, database.getConn());
            cmd.ExecuteNonQuery();
            fullname.Text = "";
            UserID.Text = "";
            phone.Text = "";
            ticketNum.Text = "";
            permission.Text = "";
            enter.IsVisible = false;
        }

        int checkEntry()
        {
            string message = "SELECT * FROM Entered WHERE barcode='{0}'";
            string query = string.Format(message, QR);
            MySqlCommand cmd = new MySqlCommand(query, database.getConn());
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            return Convert.ToInt32(dt.Rows.Count.ToString());
        }
    }
}
