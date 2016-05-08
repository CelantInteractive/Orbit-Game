using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class GameInputTest : MonoBehaviour {
    [SetUp]
    public void Init()
    {
        GameInput.UnbindAll();
    }

    [Test]
    public void GetUnassignedKeyTest()
    {
        var key = GameInput.GetBinding(Controls.SHIP_X_AXIS_POS);

        Assert.AreEqual(KeyCode.None, key);
    }

    [Test]
    public void AssignKeyTest()
    {
        GameInput.Bind(Controls.SHIP_X_AXIS_POS, KeyCode.W);

        var key = GameInput.GetBinding(Controls.SHIP_X_AXIS_POS);

        Assert.AreEqual(KeyCode.W, key);
    }
}
