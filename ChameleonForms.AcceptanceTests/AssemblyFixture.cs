﻿using System;
using NUnit.Framework;
using TestStack.Seleno.Configuration;

namespace ChameleonForms.AcceptanceTests
{
    [SetUpFixture]
    public class AssemblyFixture
    {
        [SetUp]
        public void SetUp()
        {
            SelenoHost.Run("ChameleonForms.Example", 12345, c => c
                .WithMinimumWaitTimeoutOf(TimeSpan.FromSeconds(1))
            );
        }
    }
}
