using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System;

[TestFixture]
[Category("Sample Tests")]
class NetworkUnitTest
{
    [Test]
    [Category("Failing Tests")]
    public void ExceptionTest()
    {
        throw new Exception("Exception throwing test");
    }
}
