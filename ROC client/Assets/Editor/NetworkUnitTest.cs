using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;

[TestFixture]
[Category("Network Script Tests")]
class NetworkUnitTest
{
    NetworkScript _script = new NetworkScript();

    [Test]
    [ExpectedException(typeof(Exception))]
    [Category("Failing Tests")]
    public void ExceptionTest()
    {
        throw new Exception("Exception throwing test");
    }

    [Test]
    [Category("Fail Setup Test")]
    public void SetupNetworkFailTest()
    {
        Assert.AreEqual(-1, _script.SetUpNetwork("toto", 10));
    }

    [Test]
    [Category("Success Setup Test")]
    public void SetupNetworkSuccessTest()
    {
        Assert.AreEqual(0, _script.SetUpNetwork("0", 10));
    }

    [Test]
    [Category("rtsp Addr Test null")]
    public void GetRtspAddrTest()
    {
        Assert.AreNotEqual(null, _script.GetRtspAddr());
    }

    [Test]
    [Category("level changing Test")]
    public void LoadLevelTest()
    {
        Application.LoadLevel("MenuScene");
        Assert.AreEqual(false, _script.GetMainLevel());
    }
}
