using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCMModels.MPRMasterModels
{
   public class MPRDocumentationDescriptionModel
    {
        
        public MPRDocumentationDescriptionModel()
        {
            //this.MPRDocumentations = new HashSet<MPRDocumentation>();
        }

        public byte DocumentationDescriptionId { get; set; }
        public string DocumentationDescription { get; set; }
        public bool BoolInUse { get; set; }

       
       // public virtual ICollection<MPRDocumentation> MPRDocumentations { get; set; }
    }
}
