using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ImageVault.Client;
using ImageVault.Client.Query;
using ImageVault.Common.Data;
using ImageVault.Common.Services;

namespace imagevault_mobile_upload
{
    public partial class _Default : System.Web.UI.Page
    {
        
        public Client client = new Client();
      
        public List<Vault> Vaults
        {
            get
            {

                //var allVaults = client.Query<Vault>().Include(v => v.AccessList).Include(v => v.MetadataDefinitions).ToList();
                var vaults = client.Query<Vault>().Where(v => v.CurrentUserRole == VaultRoles.Contribute).ToList();
                return vaults;

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
        
            foreach (var vault in Vaults)
            {

                DropDownList1.Items.Add(new ListItem(vault.Name, vault.Id.ToString()));

            }
            DropDownList1.Items.RemoveAt(0);
            DropDownList1.Items.Insert(0, new ListItem("Please select a vault", ""));
            

        
            
   

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
           
            HttpPostedFile postfile = Request.Files["myFile"];
            if (postfile.ContentLength > 0)
            {

                Session["selectedvault"] = DropDownList1.SelectedItem.Value;
                Session["postfile"] = postfile;

                Server.Transfer("Organize.aspx");
           
            }
            else
            {
                Response.Write(@"<script language='javascript'>alert('Please select a file to upload');</script>");
            }

        }
    }
}
