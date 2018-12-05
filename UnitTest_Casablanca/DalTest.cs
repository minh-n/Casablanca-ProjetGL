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
                dal.ResetCollaborators();
                dal.CreateCollaborator("Minh", "Nguyen");
                List<Collaborator> collaborators = dal.GetCollaborators();

                Assert.IsNotNull(collaborators);
                Assert.AreEqual(1, collaborators.Count);
                Assert.AreEqual("Minh", collaborators[0].firstName);
                Assert.AreEqual("Nguyen", collaborators[0].lastName);
            }
        }
    }
}
