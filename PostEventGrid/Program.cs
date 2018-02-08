using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace PostEventGrid
{
    class Program
    {
        // Event Grid Object
        public class GridEvent
        {
            public string Id { get; set; }
            public string Subject { get; set; }
            public string EventType { get; set; }
            public string Data { get; set; }
            public string EventTime { get; set; }
        }

        static void Main(string[] args)
        {
            string KEY = "St1U1jE6E4Pz5QLmCDpipV3+Dlt9iaQ1b3uSUrVxnCc=";
            string TOPIC_ENDPOINT = "https://eventgridtopicfp.eastus-1.eventgrid.azure.net/api/events";

            // Create a HTTP client which we will use to post to the Event Grid Topic
            var httpClient = new HttpClient();

            // Add key in the request headers
            httpClient.DefaultRequestHeaders.Add("aeg-sas-key", KEY);

            var ediEvent = new GridEvent();
            ediEvent.EventTime = DateTime.UtcNow.ToString("o");
            // Embed the EDI into the json
            ediEvent.EventType = "ISA*00*          *00*          *01*123456789      *01*987654321      *140829*1506*U*00400*004200369*0*P*>~GS*PO*123456789*987654321*20140829*1506*9405568*X*004010~ST*850*055680001~BEG*00*NE*4523184348**20140829~CUR*VN*USD~PER*CN*Med Team 03~N9*ME*MED PRODS SRVS~N1*BT*MED PRODS AND SRVCS~N3*SUPPLY PROD*P.O. Box 2279~N4*EL PASO*TX*79998~N1*DB*Montgomery , KY DC*6*D0OC~N1*MF*PACKAGING*92*000055~N1*SF*PACKAGING*92*00055~N3*4 CORPORATE DR 304~N4*LATITZ*PA*19047~N1*ST*HEALTH MED PRODS AND SRVCS~N3*Montgomery , KY DC*500 Nee Road~N4*Montgomery*KY*12345~PO1*00010*20*CA*55**CB*AD6X10LAWH*VC*AD6X10LAWH***IN*AD6X10LAWH*UK*50885380066458~PID*F****6 X 10 LAWRENCE HOSPITAL CENTER SPECIM~DTM*002*20140829~CTT*1*20~SE*21*055680001~GE*1*9405568~IEA*1*004200369~";
            ediEvent.Id = Guid.NewGuid().ToString();
            ediEvent.Subject = "EDI850";

            // Event grid expects event data as JSON
            var json = JsonConvert.SerializeObject(ediEvent);
                        
            // Create request which will be sent to the topic
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send request
            Console.WriteLine("Sending event to Event Grid...");
            var result = httpClient.PostAsync(TOPIC_ENDPOINT, content);

            // Show result
            Console.WriteLine($"Event sent with result: {result.ToString()}");
            Console.ReadLine();
        }
     }
}
