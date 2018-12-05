using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casablanca.Models.Database
{
    interface IDal : IDisposable
    {
        List<Collaborator> GetCollaborators();

        void CreateCollaborator(string firstname, string lastname);
    }
}
