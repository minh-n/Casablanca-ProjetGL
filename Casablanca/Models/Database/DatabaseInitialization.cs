using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Casablanca.Models {

    public class DatabaseInitialization : DropCreateDatabaseAlways<DatabaseContext> {

        protected override void Seed(DatabaseContext context) {
            base.Seed(context);
        }
    }
}