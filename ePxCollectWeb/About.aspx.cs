using System;
namespace ePxCollectWeb
{
    public partial class About : System.Web.UI.Page
    {
        OncoEncrypt.OncoEncrypt objEnc = new OncoEncrypt.OncoEncrypt();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Session["Login"]) == "")
            {
                Response.Redirect("login.Aspx");
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TextBox2.Text = objEnc.SecureEncrypt(TextBox1.Text);

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Label1.Text = objEnc.SecureDecrypt(TextBox2.Text);
        }
        public void test()
        {
            this.Title = "Test";
        }
    }
}
