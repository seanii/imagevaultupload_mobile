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
using ImageVault.Common.Data.Query;

namespace imagevault_mobile_upload
{

    public partial class Organize : System.Web.UI.Page
    {
      
        public Client client = new Client();

        public List<MetadataDefinition> Metadata
        {
            get
            {
                var userDefinitions = client.Query<MetadataDefinition>().Where(x => x.MetadataDefinitionType == MetadataDefinitionTypes.User).ToList();
                return userDefinitions;
            }
        }

        
       


        protected void Page_Load(object sender, EventArgs e)
        {
            int vaultid = Convert.ToInt32(Session["selectedvault"]);
            var vaultmeta = client.Query<Vault>().Where(v => v.Id == vaultid).Include(v => v.MetadataDefinitions).FirstOrDefault();
            var myCategory = client.Query<Category>()  //.Where(m => m.Categories.Contains(2))  //.Where(m => m.Categories.ContainsAll(1,2))
            .ToList();




            foreach (var metadata in vaultmeta.MetadataDefinitions)
            {
                Label metadatalbl = new Label();
                metadatalbl.Text = metadata.MetadataDefinition.Id.ToString() + metadata.MetadataDefinition.Name;
                PlaceHolder1.Controls.Add(metadatalbl);



                switch (metadata.MetadataDefinition.MetadataType)
                {

                    case MetadataTypes.String:
                        TextBox metadataInput = new TextBox();
                        metadataInput.ID = metadata.MetadataDefinition.Id.ToString();
                        PlaceHolder1.Controls.Add(metadataInput);
                        break;

                    case MetadataTypes.Decimal:
                        TextBox hello = new TextBox();
                        hello.Text = "decimal";
                        PlaceHolder1.Controls.Add(hello);
                        break;

                    //case MetadataTypes.DateTime:


                    //    DateTime datetime = new DateTime();
                    //    PlaceHolder1.Controls.Add();
                    //    break;

                }
            }
               
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            HttpPostedFile postfile = (HttpPostedFile)Session["postfile"];
            int i = Convert.ToInt32(Session["selectedvault"]);
            var uploadService = client.CreateChannel<IUploadService>();
            string id;
            using (var fs = postfile.InputStream)
            {
                id = uploadService.UploadFileContent(fs, null);
            }
            var mcs = client.CreateChannel<IMediaContentService>();
            var mediaItem = mcs.StoreContentInVault(id, postfile.FileName, "image/png", i);

            var mediaid = mediaItem.Id;
            var mi = client.Query<MediaItem>()
                .Where(u => u.Id == mediaid)
                .Include(x => x.Metadata.Where(md => md.DefinitionType == MetadataDefinitionTypes.User))
                .FirstOrDefault();
            mi.Metadata.Clear();

            foreach (Control item in PlaceHolder1.Controls)
            {
                switch (item.GetType().Name)
                {
                    case "TextBox":

                        var m = new MetadataString();

                        m.MetadataDefinitionId = Convert.ToInt32(item.ID);

                        Control mytextbox = (TextBox)PlaceHolder1.FindControl(item.ID);

                        m.StringValue = (mytextbox as TextBox).Text;

                        //m.StringValue = Request.Form[item.ID];
                        mi.Metadata.Add(m);

                        break;
                }
            }



            var ms = client.CreateChannel<IMediaService>();
            ms.Save(new List<MediaItem> { mi }, MediaServiceSaveOptions.Metadata);
            ms.Save(new List<MediaItem> { mediaItem }, MediaServiceSaveOptions.MarkAsOrganized);
            Console.WriteLine("Media item marked as organized");
            Session.RemoveAll();

            
 
        }
    }
}