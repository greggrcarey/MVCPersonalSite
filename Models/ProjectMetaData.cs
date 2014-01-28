using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace MvcPersonalSite//.Models
{

    [MetadataType(typeof(ProjectMetaData))]
    public partial class PSProject
    {
    }

    public class ProjectMetaData
    {

        [UIHint("MultilineText")]
        public string description { get; set; }




    }

}