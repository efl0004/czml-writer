﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Xml.Linq;
using KmlToCesiumLanguage;

namespace KmlToCesiumLanguageTests
{
    [TestFixture]
    class TestPolygon
    {
        [Test]
        public void PolygonWithTimeSpanProducesAvailability()
        {
            XElement placemark = new XElement("Placemark",
                                    new XElement("TimeSpan",
                                        new XElement("begin", "2007-12-06T16:38:22.920"),
                                        new XElement("end", "2007-12-06T16:38:32.920")),
                                    new XElement("Polygon",
                                        new XElement("altitudeMode", "absolute"),
                                        new XElement("outerBoundaryIs",
                                            new XElement("LinearRing",
                                                new XElement("coordinates", "-74.42333000000001,39.364163,1 -74.1855216770938,33.4296610235422,105622.226606304")))));
            CzmlDocument document = new CzmlDocument();
            var polygon = new Polygon(placemark.Element("Polygon"), document, placemark);
            polygon.WritePacket();
            string result = document.StringWriter.ToString();
            Assert.That(result.Contains("\"availability\":\"2007-12-06T16:38:22.9199999999983Z/2007-12-06T16:38:32.9199999999983Z\""));
        }

        [Test]
        public void PolygonOuterBoundaryProducesVertexPositions()
        {
            XElement placemark = new XElement("Placemark",
                                    new XElement("name", "Access Polygon"),
                                    new XElement("Polygon",
                                        new XElement("altitudeMode", "absolute"),
                                        new XElement("outerBoundaryIs",
                                            new XElement("LinearRing",
                                                new XElement("coordinates", "-74.42333000000001,39.364163,1 -74.1855216770938,33.4296610235422,105622.226606304")))));
            CzmlDocument document = new CzmlDocument();
            document.Namespace = "";
            var polygon = new Polygon(placemark.Element("Polygon"), document, placemark);
            polygon.WritePacket();
            string result = document.StringWriter.ToString();
            Assert.That(result.Contains("\"vertexPositions\":{\"cartographicRadians\":[-1.2989321487982717,0.6870342516417286,1.0,-1.2947816105749124,0.583457652686429,105622.226606304]}"));
        }

        [Test]
        public void PolygonWithPolyStyleProducesPolygonGraphics()
        {
            XElement placemark = new XElement("Placemark",
                                    new XElement("Style",
                                        new XElement("PolyStyle",
                                            new XElement("color", "ffffffff"))),
                                    new XElement("Polygon",
                                        new XElement("altitudeMode", "absolute"),
                                        new XElement("outerBoundaryIs",
                                            new XElement("LinearRing",
                                                new XElement("coordinates", "-74.42333000000001,39.364163,1 -74.1855216770938,33.4296610235422,105622.226606304")))));
            CzmlDocument document = new CzmlDocument();
            document.Namespace = "";
            var polygon = new Polygon(placemark.Element("Polygon"), document, placemark);
            polygon.WritePacket();
            string result = document.StringWriter.ToString();
            Assert.That(result.Contains("\"polygon\":{\"material\":{\"solidColor\":{\"color\":{\"rgba\":[255,255,255,255]}}}}"));
        }

        [Test]
        public void PolygonClampToGroundProducesCartographicsWithZeroHeight()
        {
            XElement placemark = new XElement("Placemark",
                                                new XElement("name", "Access Polygon"),
                                                new XElement("Polygon",
                                                    new XElement("altitudeMode", "clampToGround"),
                                                    new XElement("outerBoundaryIs",
                                                        new XElement("LinearRing",
                                                            new XElement("coordinates", "-74.42333000000001,39.364163,1 -74.1855216770938,33.4296610235422,105622.226606304")))));
            CzmlDocument document = new CzmlDocument();
            document.Namespace = "";
            var polygon = new Polygon(placemark.Element("Polygon"), document, placemark);
            polygon.WritePacket();
            string result = document.StringWriter.ToString();
            Assert.That(result.Contains("\"vertexPositions\":{\"cartographicRadians\":[-1.2989321487982717,0.6870342516417286,0.0,-1.2947816105749124,0.583457652686429,0.0]}"));
        }
    }
}
