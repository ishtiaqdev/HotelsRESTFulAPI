using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using HotelsRESTApi.Contracts;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace HotelsRESTApi.Services
{
    public class HotelsServices : IHotelsServices
    {
        private readonly JsonTextReader _hoteJsonReader;

        public HotelsServices()
        {
            string jsonObjectHotel = @"[
                  {
                    'hotel': {
                      'classification': 5,
                      'hotelID': 7294,
                      'name': 'Kempinski Bristol Berlin',
                      'reviewscore': 8.3
                    },
                    'hotelRates': [
                      {
                                'adults': 2, 
                        'los': 1, 
                        'price': {
                                    'currency': 'EUR', 
                          'numericFloat': 116.1, 
                          'numericInteger': 11610
                        }, 
                        'rateDescription': 'Unsere Classic Zimmer bieten Ihnen allen Komfort, den Sie von einem 5-Sterne-Hotel erwarten. Helle und freundliche Farben sorgen f\u00fcr ein angenehmes Ambiente, damit Sie Ihren Aufenthalt im Herzen Berlins voll und ganz genie\u00dfen k\u00f6nnen. 20m\u00b2. Doppelbett oder zwei separate Betten. Max. Kapazit\u00e4t: 2 Erwachsene oder 1 Erwachsener und 1 Kind.      ', 
                        'rateID': '-739857498', 
                        'rateName': 'Classic Zimmer - Fr\u00fchbucher Rate', 
                        'rateTags': [
                          {
                            'name': 'breakfast',
                            'shape': false
                          }
                        ], 
                        'targetDay': '2016-03-15T00:00:00.000+01:00'
                      }, 
                      {
                        'adults': 2, 
                        'los': 1, 
                        'price': {
                          'currency': 'EUR', 
                          'numericFloat': 129, 
                          'numericInteger': 12900
                        }, 
                        'rateDescription': 'Unsere Classic Zimmer bieten Ihnen allen Komfort, den Sie von einem 5-Sterne-Hotel erwarten. Helle und freundliche Farben sorgen f\u00fcr ein angenehmes Ambiente, damit Sie Ihren Aufenthalt im Herzen Berlins voll und ganz genie\u00dfen k\u00f6nnen. 20m\u00b2. Doppelbett oder zwei separate Betten. Max. Kapazit\u00e4t: 2 Erwachsene oder 1 Erwachsener und 1 Kind.      ', 
                        'rateID': '-740789668', 
                        'rateName': 'Classic Zimmer - Beste Flexible Rate', 
                        'rateTags': [
                          {
                            'name': 'breakfast',
                            'shape': false
                          }
                        ], 
                        'targetDay': '2016-03-15T00:00:00.000+01:00'
                      }]
                    }
                ]";
            _hoteJsonReader = new JsonTextReader(new StringReader(jsonObjectHotel));
        }
        public ActionResult GetByString(int HotelID, DateTime ArrivalDate)
        {
            //Need to to be implemented. Not implementing right now due to shortage of time.
            return null;
        }
        public ActionResult GetByStream(int HotelID, DateTime ArrivalDate)
        {
            //Need to to be implemented. Not implementing right now due to shortage of time.
            return null;
        }
        public ActionResult GetByJsonObject(int HotelID, DateTime ArrivalDate)
        {
            ActionResult result = null;
            JArray hotelArray = null;
            JObject filteredHotels = null;
            JsonTextReader streamReader = null;

            hotelArray = (JArray)JToken.ReadFrom(_hoteJsonReader);

            FilterHotels(hotelArray, ref filteredHotels, streamReader, HotelID, ArrivalDate);

            if (filteredHotels == null)
            {
                filteredHotels = new JObject();
            }

            result = new ContentResult { Content = filteredHotels.ToString(), ContentType = "application/json" };
            return result;
        }

        public ActionResult GetByJsonFile(int HotelID, DateTime ArrivalDate)
        {
            string filePath = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().IndexOf("HotelsRESTApi")) + "HotelsRESTApi\\Files\\task 3 - hotelsrates.json";

            ActionResult result = null;
            JArray hotelArray = null;
            JObject filteredHotels = null;
            JsonTextReader streamReader = null;

            using (StreamReader file = File.OpenText(filePath))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    hotelArray = (JArray)JToken.ReadFrom(reader);
                }
            }

            FilterHotels(hotelArray, ref filteredHotels, streamReader, HotelID, ArrivalDate);

            if(filteredHotels == null)
            {
                filteredHotels = new JObject();
            }

            result = new ContentResult { Content = filteredHotels.ToString(), ContentType = "application/json" };
            return result;
        }



        private JObject FilterHotels(JArray hotelArray, ref JObject filteredHotels, JsonTextReader streamReader, int HotelID, DateTime ArrivalDate)
        {
            foreach (var hotel in hotelArray.Children())
            {
                var hotelId = hotel["hotel"]["hotelID"].ToString();

                if (!string.IsNullOrEmpty(hotelId))
                {
                    if (int.Parse(hotelId) == HotelID)
                    {
                        streamReader = new JsonTextReader(new StringReader(hotel.ToString()));
                        break;
                    }
                }
            }

            if(streamReader != null)
                filteredHotels = (JObject)JToken.ReadFrom(streamReader);

            List<object> lstOfDatesToRemove = new List<object>();

            if (filteredHotels != null)
            {
                foreach (var rate in filteredHotels.Children().Children())
                {
                    if (rate.Path == "hotelRates")
                    {
                        foreach (var date in rate.Children())
                        {
                            if (date["targetDay"].ToString() != null)
                            {
                                DateTime currentDate = Convert.ToDateTime(date["targetDay"].ToString());

                                if (currentDate != ArrivalDate)
                                {
                                    lstOfDatesToRemove.Add(date);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        foreach (JToken date in lstOfDatesToRemove)
                        {
                            date.Remove();
                        }
                    }
                }
            }

            return filteredHotels;
        }
    }
}
