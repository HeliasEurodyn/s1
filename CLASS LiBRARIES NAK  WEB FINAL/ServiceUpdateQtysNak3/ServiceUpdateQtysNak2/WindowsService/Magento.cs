using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace WindowsService
{
    class Magento
    {

        public String AccessToken = "";

        public String Authenticate(string url, string postData)
        {
            string ret = string.Empty;

            StreamWriter requestWriter;

            var webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.Method = "POST";
                webRequest.ServicePoint.Expect100Continue = false;
                webRequest.Timeout = 20000;

                webRequest.ContentType = "application/json";
                //POST the data.
                using (requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(postData);
                }
            }

            HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
            Stream resStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            AccessToken = reader.ReadToEnd();

            return AccessToken;
        }


        public String GetProductBySKU(String url)
        {
            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );

            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }

        public String GetProductBySKUParseResponce(String url)
        {
            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );

            // IRestResponse response = client.Execute(request);
            var response = client.Execute<dynamic>(request);

            //  var data = ((JObject)json.data).Children();
            //  Object sku = response.Data.Children().sku;

            JObject joResponse = JObject.Parse(response.Content);
            String sku = (String)joResponse["sku"].ToString();

            return sku;
        }


        public String UpdateProductQty(String url, String Qty)
        {
            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.PUT);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );

            //  request.AddHeader("Accept", "application/json");
            request.RequestFormat = DataFormat.Json;

            //request.Parameters.Clear();
            // request.AddParameter("application/json", "{\"stockItem\":{\"qty:\":22}}", ParameterType.RequestBody);
            //  request.AddBody("{\"stockItem\":{\"qty:\":22}}");

            request.AddParameter("application/json",
               "{\r\n\"stockItem\":{\r\n                                " +
               "\"qty\":" + Qty + "\r\n                               \r\n                " +
               "}\r\n}", ParameterType.RequestBody);
            //  request.AddBody("{\"stockItem\":{\"qty:\":22}}");
            // request.AddParameter("stockItem", "{\"qty:\":22}");

            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }

        public String InsertUpdateProduct(String url)
        {
            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.PUT);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );

            //  request.AddHeader("Accept", "application/json");
            request.RequestFormat = DataFormat.Json;

            //request.Parameters.Clear();
            // request.AddParameter("application/json", "{\"stockItem\":{\"qty:\":22}}", ParameterType.RequestBody);
            //  request.AddBody("{\"stockItem\":{\"qty:\":22}}");

            request.AddParameter("application/json",
               "{\r\n \r\n  " +
                "\"product\" : {\r\n                                " +
                        "\"sku\": \"testSKU\",\r\n        " +
                        "\"name\" : \"TEST CREATE PRODUCT\",\r\n        " +
                        "\"status\" : 2,\r\n        " +
                        "\"attribute_set_id\": 9,\r\n        " +
                        "\"price\": 99,\r\n        " +
                        "\"visibility\": 1,\r\n        " +
                        "\"type_id\": \"simple\",\r\n        " +
                        "\"weight\": 1,\r\n                " +

                        "\"extension_attributes\": {\r\n                                                " +
                            "\"stock_item\": {\r\n                                                  " +
                                    "\"qty\": 10,\r\n                                                  " +
                                    "\"is_in_stock\": true\r\n                                " +
                            "}\r\n                                " +
                        "},\r\n        " +

                        "\"custom_attributes\": [\r\n                " +

                            "{\r\n                                                " +
                            "\"attribute_code\": \"description\",\r\n                                                " +
                            "\"value\": \"test description\"          \r\n                " +
                            "},\r\n                " +

                           "{\r\n                                                " +
                           "\"attribute_code\": \"short_description\",\r\n                                                " +
                           "\"value\": \"test short description\"              \r\n                " +
                           "},\r\n                                    " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"category_ids\",\r\n                                      " +
                           "\"value\": [\r\n                                        \"254\"\r\n                                      " +
                           "]\r\n                                    " +
                           "},\r\n                        " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"url_key\",\r\n                                      " +
                           "\"value\": \"test-test-simple-product-url\"\r\n                                    " +
                           "},\r\n                        " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"tax_class_id\",\r\n                                      " +
                           "\"value\": \"2\"\r\n                                    " +
                           "},\r\n                        " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"brand\",\r\n                                      " +
                           "\"value\": \"6960\"\r\n                                    " +
                           "},\r\n                        " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"group_code\",\r\n                                      " +
                           "\"value\": \"test_group_code\"\r\n                                    " +
                           "},\r\n                        {\r\n                                      " +


                           "\"attribute_code\": \"internal_id\",\r\n                                      " +
                           "\"value\": \"testInternalId\"\r\n                                    " +
                           "},\r\n                        " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"nak_color\",\r\n                                      " +
                           "\"value\": \"7910\"\r\n                                    " +
                           "},\r\n                        " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"nak_size\",\r\n                                      " +
                           "\"value\": \"13041\"\r\n                                    " +
                           "},\r\n                        " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"has_barcode\",\r\n                                      " +
                           "\"value\": \"1\"\r\n                                    " +
                           "},\r\n                        " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"new_erp_product_data\",\r\n                                      " +
                           "\"value\": \"some test data\"\r\n                                    " +
                           "},\r\n                        " +

                           "{\r\n                                      " +
                           "\"attribute_code\": \"erp_color\",\r\n                                      " +
                           "\"value\": \"BLUSH\"\r\n                                    " +
                           "}\r\n                                                                               \r\n        " +

                        "]\r\n \r\n  " +
                    "}\r\n" +
                "}",

                ParameterType.RequestBody);

            //  request.AddBody("{\"stockItem\":{\"qty:\":22}}");
            // request.AddParameter("stockItem", "{\"qty:\":22}");

            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }


        public String updateProductQtys(String url, List<PopulateItem> items)
        {
            String ItemsInJson = "";
            bool FirstLoop = true;


            if (items.Count > 0) ItemsInJson +=
                                        "    { \r\n " +
                                        "     \"items\":[ \r\n";

            foreach (PopulateItem item in items)
            {
                if (!FirstLoop) ItemsInJson += ",";
                ItemsInJson += " { \r\n" +
                                            "   \"itemSubCode\" : \"" + item.itemSubCode + "\",\r\n " +
                                            "   \"itemBalance\" : \"" + item.itemBalance + "\" \r\n" +
                              "    } \r\n";


                FirstLoop = false;
            }

            if (items.Count > 0) ItemsInJson +=
                                      "     ] \r\n" +
                                      " }\r\n ";



            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.POST);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );

            request.RequestFormat = DataFormat.Json;

            request.AddParameter("application/json",
                       ItemsInJson, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }



    }
}
