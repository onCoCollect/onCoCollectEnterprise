using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace ePxCollectWeb
{
    public partial class LiveUpdateRedirect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }

     

       
    }
}