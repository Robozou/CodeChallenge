using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallengeV2.Models;

namespace CodeChallengeV2.Services
{
    public class SensorService : ISensorService
    {
        public Task<SensorPayloadDecoded> DecodePayload(SensorPayload payload)
        {
            var res = new SensorPayloadDecoded()
            {
                DevEUI = payload.DevEUI,
                Time = payload.Time,
            };
            int index = 0;
            string completeData = payload.FPort.ToString("X") + payload.Data;
            while (index < completeData.Length)
            {
                switch (getInt16(completeData.Substring(index, 2)))
                {
                    case 20:
                        res.Battery = getInt16(completeData.Substring(index + 2, 4));
                        index += 6;
                        break;
                    case 40:
                        res.TempInternal = ((double)getInt16(completeData.Substring(index + 2, 4))) / 100;
                        index += 6;
                        break;
                    case 41:
                        res.TempRed = ((double)getInt16(completeData.Substring(index + 2, 4))) / 100;
                        index += 6;
                        break;
                    case 42:
                        res.TempBlue = ((double)getInt16(completeData.Substring(index + 2, 4))) / 100;
                        index += 6;
                        break;
                    case 43:
                        res.TempHumidity = ((double)getInt16(completeData.Substring(index + 2, 4))) / 100;
                        res.Humidity = ((double)getInt16(completeData.Substring(index + 6, 2))) / 2;
                        index += 8;
                        break;
                    default:
                        break;
                }
            }

            return Task.FromResult(res);
        }

        private string ReverseData(string data)
        {
            string res = "";
            for (int i = 0; i < data.Length; i = i + 2)
            {
                res = data.Substring(i, 2) + res;
            }
            return res;
        }

        private Int16 getInt16(string byteString)
        {
            return Int16.Parse(ReverseData(byteString), System.Globalization.NumberStyles.HexNumber);
        }
    }
}
