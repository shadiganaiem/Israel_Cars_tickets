using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using ISRLCARS.Database;
using ISRLCARS.Services;
using MySql.Data.MySqlClient;
using Xamarin.Forms;

namespace ISRLCARS.Views
{
    public partial class NewTicket : ContentPage
    {
        DBConnection database = new DBConnection();
        string QR;
        public NewTicket()
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
                    QRentry.Text = QR;
                    CheckTicketAsync();
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

        async void CheckTicketAsync()
        {
            string message = "SELECT * from Entry_Tickets WHERE barcode='{0}'";
            string query = string.Format(message, QRentry.Text);
            MySqlCommand cmd = new MySqlCommand(query, database.getConn());
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            int i = Convert.ToInt32(dt.Rows.Count.ToString());
            if (i > 0)
            {
                await DisplayAlert(null, "כרטיס זה נמכר!" + "\n" + "נא ליצור כרטיס חדש", "אישור");
                QRentry.Text = "";
            }
            else
                generateTicketNumber();
        }

        void generateTicketNumber()
        {
            string query = "SELECT * FROM Entry_Tickets";
            MySqlCommand cmd = new MySqlCommand(query, database.getConn());
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            int i = Convert.ToInt32(dt.Rows.Count.ToString());
            i++;
            TicketNum.Text = i.ToString();
        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            if (isAnyFieldEmpty())
            {
                await DisplayAlert(null, "שדה חובה חסר!", "אישור");
            }
            else if (!validateEmail())
            {
                await DisplayAlert(null, "דואר אלקטרוני לא תקין", "אישור");
                email.Text = "";
            }
            else if (phone.Text.Length != 10)
            {
                await DisplayAlert(null, "מספר נייד לא נכון", "אישור");
                phone.Text = "";
            }
            else
            {
                string message = "INSERT INTO Entry_Tickets VALUES('{0}','{1}','{2}','{3}','{4}','{5}')";
                string query = string.Format(message, QRentry.Text, TicketNum.Text, fullname.Text, UserID.Text, email.Text, phone.Text);
                MySqlCommand cmd = new MySqlCommand(query, database.getConn());
                cmd.ExecuteNonQuery();

                var navpage = (NavigationPage)Application.Current.MainPage;
                await navpage.PopAsync();
            }
            
        }

        bool isAnyFieldEmpty()
        {
            return QRentry.Text.Equals("") || TicketNum.Text.Equals("") || fullname.Text.Equals("") || UserID.Text.Equals("")
                    || email.Text.Equals("") || phone.Text.Equals("");
         }

        bool validateEmail()
        {
            return Regex.Match(email.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success;
        }

        async void CheckButtonAsync(object sender, System.EventArgs e)
        {
            if (UserID.Text.Equals(""))
            {
                await DisplayAlert(null, "נא למלא תעודת זהות", "אישור");
            }
            else
            {
                string message = "SELECT * FROM Entry_Tickets WHERE User_ID='{0}'";
                string query = string.Format(message, UserID.Text);
                MySqlCommand cmd = new MySqlCommand(query, database.getConn());
                cmd.ExecuteNonQuery();

                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                int i = Convert.ToInt32(dt.Rows.Count.ToString());
                if(i > 0)
                {
                    fullname.Text = dt.Rows[0][2].ToString();
                    email.Text = dt.Rows[0][4].ToString();
                    phone.Text = dt.Rows[0][5].ToString();

                }
                else
                {
                    await DisplayAlert(null, "תעודת זהות לא רשומה במערכת", "אישור");
                }
            }
            
        }
    }
}
