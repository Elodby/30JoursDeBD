using _30JoursDeBD.Common.testmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _30JoursDeBD.testmodel
{
    public class BD
    {
        public string Titre { get; set; }
        public string Image { get; set; }
        public List<string> ImagesAttachees { get; set; }
        public string Auteur { get; set; }
        public string Rubrique { get; set; }
        public string Excerpt { get; set; }

        public List<Commentaire> Commentaires { get; set; }
    }
}
