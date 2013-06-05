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
    public partial class WebForm1 : System.Web.UI.Page
    {
        public void SaveMetadataTest()
        {

            var client = ClientFactory.GetSdkClient();



            //Yuck! Metadata handling is ugly so far.



            //create or find the metadata definition that you want to store

            //here we create the one we need if it isn't found

            var template = new MetadataDefinition
            {

                MetadataDefinitionType = MetadataDefinitionTypes.User,

                Name = "Test",

                MetadataType = MetadataTypes.String

            };

            var mds = client.CreateChannel<IMetadataDefinitionService>();

            //first find all metadata definitions of the same def type and type as the one requested and that matches the name.

            var definition = mds.Find(new MetadataDefinitionQuery
            {
                Filter =
                {

                    MetadataDefinitionType = template.MetadataDefinitionType,
                    MetadataType = template.MetadataType

                }
            }).FirstOrDefault(d => d.Name == template.Name);

            if (definition == null)
            {

                //if no match was found, create the template instead

                var id = mds.Save(template);

                definition = template;

                definition.Id = id;

            }

            //create the metadata itself by setting the id of the definition and the value (important to create a metadata of 

            //the same sort as the defined MetadataType.

            var m = new MetadataString
            {

                MetadataDefinitionId = definition.Id,

                StringValue = "test value"

            };



            //get the media item to which we should attach the metadata to. (We inlcude all user metadata)

            var mi = client.Query<MediaItem>()

                     .Include(x => x.Metadata.Where(md => md.DefinitionType == MetadataDefinitionTypes.User))

                    .Take(1).FirstOrDefault();

            //when we save the metadata we only want to modify the new one

            //when we clear the metadata, this will not clear the metadata stored in the db only for this copy

            mi.Metadata.Clear();

            //add the new/modified metadata

            mi.Metadata.Add(m);

            var ms = client.CreateChannel<IMediaService>();

            //supply the Metadata save option flag to indicate that medatata should be saved as well.

            //as stated above, we cannot delete any metadata. Any metadata passed to the save function will only 

            //be added/modified for now.

            ms.Save(new List<MediaItem> { mi }, MediaServiceSaveOptions.Metadata);

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}