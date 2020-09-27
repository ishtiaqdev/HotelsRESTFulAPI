using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace HotelsRESTApi.Contracts
{
    public interface IHotelsServices
    {
         ActionResult GetByString(int HotelID, DateTime ArrivalDate);
         ActionResult GetByStream(int HotelID, DateTime ArrivalDate);
         ActionResult GetByJsonObject(int HotelID, DateTime ArrivalDate);
         ActionResult GetByJsonFile(int HotelID, DateTime ArrivalDate);
    }
}
