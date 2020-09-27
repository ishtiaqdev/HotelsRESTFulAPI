using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HotelsRESTApi.Contracts;
using System.IO;

namespace HotelsRESTApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsServices _service;

        public HotelsController(IHotelsServices service)
        {
            _service = service;
        }

        //Default Test API
        // GET api/hotels
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" }; 
        }

        // GET api/hotels/GetByStringValue/7294
        [HttpGet("GetByStringValue/{hotelid}/{arrivaldate?}")]
        public ActionResult GetByStringValue(int HotelID, DateTime ArrivalDate)
        {
            var item = _service.GetByString(HotelID, ArrivalDate);

            if(HotelID == 10)
            {
                return Ok("Success");
            }

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // GET api/hotels/GetByStreamValue/7294
        [HttpGet("GetByStreamValue/{hotelid}/{arrivaldate?}")]
        public ActionResult GetByStreamValue(int HotelID, DateTime ArrivalDate)
        {
            var item = _service.GetByStream(HotelID, ArrivalDate);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // GET api/hotels/GetByJsonObjectValue/7294/2016-03-15
        [HttpGet("GetByJsonObjectValue/{hotelid}/{arrivaldate?}")]
        public ActionResult GetByJsonObjectValue(int HotelID, DateTime ArrivalDate)
        {
            var item = _service.GetByJsonObject(HotelID, ArrivalDate);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        //// GET api/hotels/GetByFileValue/7294/2016-03-16
        [HttpGet("GetByFileValue/{hotelid}/{arrivaldate?}")]
        public ActionResult GetByFileValue(int HotelID, DateTime ArrivalDate)
        {
            var item = _service.GetByJsonFile(HotelID, ArrivalDate);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }
    }
}
