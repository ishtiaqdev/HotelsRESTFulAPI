using System;
using System.Collections.Generic;
using System.Text;
using HotelsRESTApi.Contracts;
using HotelsRESTApi.Controllers;
using NUnit.Framework;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace NUnitHotelsApi
{
    [TestFixture]
    class HotelsControllerTest
    {
        HotelsController _controller;
        IHotelsServices _service;

        public HotelsControllerTest()
        {
            _service = new HotelServiceFake();
            _controller = new HotelsController(_service);
        }

        [TestCase]
        public void Get_WhenCalled_ReturnsOkResultWithNull()
        {
            // Act
            var okNullResult = _controller.GetByStringValue(1, DateTime.Now);

            // Assert
            Assert.AreNotEqual(1, okNullResult);
        }

        [TestCase]
        public void Get_WhenCalled_ReturnsNull_TypesMatch()
        {
            // Act
            var nullResult = _controller.GetByStreamValue(1, DateTime.Now);

            // Assert
            Assert.AreEqual(typeof (NotFoundResult), nullResult.GetType());
        }

        [TestCase]
        public void GetBy_HotelIDAndDate_ReturnsNotNullResult()
        {
            // Act
            var okFoundResult = _controller.GetByJsonObjectValue(7294, Convert.ToDateTime("2016-03-15"));

            // Assert
            Assert.IsNotNull(okFoundResult);
        }

        [TestCase]
        public void GetBy_HotelIDAndDate_FoundFromFile_ReturnsOkResult()
        {
            //Act
            var okFoundResult = _controller.GetByFileValue(7294, DateTime.Now);

            // Assert
            Assert.AreEqual(typeof (ContentResult), okFoundResult.GetType());
        }

        [TestCase]
        public void GetBy_HotelIDAndDate_ReturnsNotFoundResult()
        {
            // Act
            var notFoundResult = _controller.GetByJsonObjectValue(010, Convert.ToDateTime("0001-01-01"));

            // Assert
            Assert.IsNotNull(notFoundResult);
        }

    }
}
