using System;
using Casablanca.Models.Database;
using Casablanca.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTest_Casablanca
{
    [TestClass]
    public class DalTest
    {
        [TestMethod]
        public void GetCollaborators()
        {
            using (Dal dal = new Dal())
            {
                // TODO : Modify
                //dal.ResetCollaborators();
                //dal.CreateCollaborator("Minh", "Nguyen");
                List<Collaborator> collaborators = dal.GetCollaborators();

                Assert.IsNotNull(collaborators);
                Assert.AreEqual(10, collaborators.Count);
                Assert.AreEqual("Minh", collaborators[0].FirstName);
                Assert.AreEqual("NGUYEN", collaborators[0].LastName);
                


            }
        }
    }
}
