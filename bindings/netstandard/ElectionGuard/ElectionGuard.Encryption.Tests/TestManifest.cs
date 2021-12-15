﻿using System;
using System.Diagnostics;
using NUnit.Framework;

using ElectionGuard.Encryption.Tests.Utils;

namespace ElectionGuard.Encrypt.Tests
{
    [TestFixture]
    public class TestManifest
    {
        string TEST_SPEC_VERSION = "0.95.0";
        string TEST_USE_SAMPLE = "hamilton-general";
        [Test]
        public void Test_Can_Serialize_Sample_manifest()
        {
            var subject = ManifestGenerator.GetManifestFromFile(TEST_SPEC_VERSION, TEST_USE_SAMPLE);
            Assert.That(subject.IsValid);
        }

        [Test]
        public void Test_Can_Construct_Internationalized_Text()
        {
            // Arrange
            var language_1 = new Language("some words", "en");
            var language_2 = new Language("algunas palabras", "es");
            var languages = new[] { language_1, language_2 };

            // Act
            var subject = new InternationalizedText(languages);

            // Assert
            var actual = subject.GetTextAt(0);
            Assert.That(actual.Value == "some words");
        }

        [Test]
        public void Test_Can_Construct_Ballot_style()
        {
            var gpUnitIds = new[] { "gp-unit-1", "gp-unit-2" };

            var subject = new BallotStyle("some-object-id", gpUnitIds);

            var actual = subject.GetGeopoliticalUnitIdAt(0);
            Assert.That(actual == "gp-unit-1");
        }

        [Test]
        public void Test_Can_Construct_InternalManifest_From_Sample_Manifest()
        {
            // Get a simple manifest that shows the bare minimum data required
            var manifest = ManifestGenerator.GetManifestFromFile(TEST_SPEC_VERSION, TEST_USE_SAMPLE);
            var internalManifest = new InternalManifest(manifest);

            Assert.That(manifest.CryptoHash().ToHex() == internalManifest.ManifestHash.ToHex());
            Assert.That(manifest.IsValid());
        }

        [Test]
        public void Test_Can_Construct_InternalManifest_From_Manifest_MultipleBallotStyles()
        {
            // Get a slightly more complex manifest that shows including multiple ballot styles
            var manifest = ManifestGenerator.GetJeffersonCountyManifest_MultipleBallotStyles();
            var internalManifest = new InternalManifest(manifest);

            Assert.That(manifest.CryptoHash().ToHex() == internalManifest.ManifestHash.ToHex());
            Assert.That(manifest.IsValid());
        }

        [Test]
        public void Test_Can_Serialize_Manifest_Minimal()
        {
            var manifest = ManifestGenerator.GetJeffersonCountyManifest_Minimal();
            var json = manifest.ToJson();

            var result = new Manifest(json);

            Assert.That(manifest.CryptoHash().ToHex() == result.CryptoHash().ToHex());
            Assert.That(result.IsValid());
        }

        [Test]
        public void Test_Can_Serialize_Manifest_MultipleBallotStyles()
        {
            var manifest = ManifestGenerator.GetJeffersonCountyManifest_MultipleBallotStyles();
            var json = manifest.ToJson();

            var result = new Manifest(json);

            Assert.That(manifest.CryptoHash().ToHex() == result.CryptoHash().ToHex());
            Assert.That(result.IsValid());
        }
    }
}
