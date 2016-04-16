using UnityEngine;
using NUnit.Framework;

[TestFixture]
[Category("Connection Tests")]
 class GoConnectionTest
{
    CommunicationManagerScript script = new CommunicationManagerScript();

    [Test]
    [Category("Unknown Tests")]
    public void FirstConnectionTest()
    {
        Assert.AreNotEqual(-1, script.StartGoLink(), "Link could not be established to the go software");
    }

}
