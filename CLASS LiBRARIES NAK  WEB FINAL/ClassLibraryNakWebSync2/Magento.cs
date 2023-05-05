using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryNakWebSync
{
    class Magento
    {

        public String AccessToken = "";

        public static bool Web1 = true;
        public static bool Web2 = true;

        public Magento()
        {
            if (Properties.Settings1.Default["Web1"].ToString() == "1" ) Magento.Web1 = true; 
            else Magento.Web1 = false;

            if (Properties.Settings1.Default["Web2"].ToString() == "1") Magento.Web2 = true;
            else Magento.Web2 = false;
        }


        public Boolean runThisTime(int web)
        {
            if (web == 1)
            {
                if (Web1 == false) return false;
            }
            else if (web == 2)
            {
                if (Web2 == false) return false;
            }

            return true;
        }


        public String truncate(String url,int web)
        {
            if (!runThisTime(web)) return "";

            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );

            request.RequestFormat = DataFormat.Json;


            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }



        public String importTriger(String url, int web)
        {
            if (!runThisTime(web)) return "";

            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );

            request.RequestFormat = DataFormat.Json;


            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }


        public String populate(String url, PopulateItem populateItem, int web)
        {
            if (!runThisTime(web)) return "";

            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.POST);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );

            request.RequestFormat = DataFormat.Json;



            request.AddParameter("application/json", "{\r\n               " +
                " \"items\" : [\r\n                               " +
                " {\r\n                                              " +
                "  \"itemId\"                               : \""+ populateItem.itemId+ "\",\r\n                                               " +
                " \"itemCode\"                        : \"" + populateItem.itemCode + "\",\r\n                                                " +
                "\"itemName\"                      : \"" + populateItem.itemName + "\",\r\n                                                " +
                "\"active\"                                : \"" + populateItem.active + "\",\r\n                                                " +
                "\"groupCode\"     : \"" + populateItem.groupCode + "\",\r\n                                                " +
                "\"groupName\"    : \"" + populateItem.groupName + "\",\r\n                                                " +
                "\"markCode\"                       : \"" + populateItem.markCode + "\",\r\n                                                " +
                "\"markName\"                     : \"" + populateItem.markName + "\",\r\n                                                " +
                "\"retailPrice\"        : \"" + populateItem.retailPrice.Replace(",", ".") + "\",\r\n                                                " +
                "\"retailPriceUK\"        : \"" + populateItem.retailPriceUK.Replace(",", ".") + "\",\r\n                                                " +
                "\"specialPrice\"        : \"" + populateItem.specialPrice.Replace(",", ".") + "\",\r\n                                                " +
                "\"specialPriceUK\"        : \"" + populateItem.specialPriceUK.Replace(",", ".") + "\",\r\n                                                " +
                "\"fromDate\" : \"" + populateItem.fromDate + " 00:00:00\",\r\n " +
                "\"finalDate\" : \"" + populateItem.finalDate + " 23:59:59\" \r\n" +
                "\"colorName\"      : \"" + populateItem.colorName + "\",\r\n                                                " +
                "\"sizeName\"                        : \"" + populateItem.sizeName + "\",\r\n                                                " +
                "\"seasonName\" : \"" + populateItem.seasonName + "\",\r\n                                                " +
                "\"itemBalance\"   : \"" + populateItem.itemBalance + "\",\r\n                                                " +
                "\"itemSubCode\"                : \"" + populateItem.itemSubCode + "\",\r\n                                                " +
                "\"itemSubCode1\"              : \"" + populateItem.itemSubCode1 + "\",\r\n                                                " +
                "\"itemSubCode2\"              : \"" + populateItem.itemSubCode2 + "\",\r\n                                                " +
                "\"itemSubCode3\"              : \"" + populateItem.itemSubCode3 + "\",\r\n                                                " +
                "\"itemSubCode4\"              : \"" + populateItem.itemSubCode4 + "\",\r\n                                                " +
                "\"colorName2\"   : \"" + populateItem.colorName2 + "\",\r\n                                                " +
                "\"sizeName2\"      : \"" + populateItem.sizeName2 + "\",\r\n                                                " +
                "\"itemCode2\"      : \"" + populateItem.itemCode2 + "\",\r\n                                                " +
                "\"onWeb\"                             : \"" + populateItem.onWeb + "\",\r\n                                                " +
                "\"sizeWebUK\"     : \"" + populateItem.sizeWebUK + "\",\r\n                                                " +
                "\"sizeWebUS\"     : \"" + populateItem.sizeWebUS + "\",\r\n                                                " +
                "\"sizeWebEU\"     : \"" + populateItem.sizeWebEU + "\",\r\n                                                " +
                "\"sizeWebFR\"      : \"" + populateItem.sizeWebFR + "\",\r\n                                                " +
                "\"sizeWebJPN\"   : \"" + populateItem.sizeWebJPN + "\"\r\n                                " +
                "}\r\n" +
                "   ]\r\n}", ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }



        public String populate(String url, List<PopulateItem> populateItems, int web)
        {
            if (!runThisTime(web)) return "";

            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.POST);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );


            bool firstTime = true;
            request.RequestFormat = DataFormat.Json;
            String parameter = "";

            parameter += "{\r\n               " +
               " \"items\" : ";
            
            parameter += "[\r\n                               ";

            foreach (PopulateItem populateItem in populateItems)
            {
                if (firstTime) parameter += " {\r\n                                              ";
                else parameter += ",{\r\n                                              ";


                parameter += "  \"itemId\"                               : \"" + populateItem.itemId + "\",\r\n                                               " +
              " \"itemCode\"                        : \"" + populateItem.itemCode + "\",\r\n                                                " +
              "\"itemName\"                      : \"" + populateItem.itemName + "\",\r\n                                                " +
              "\"active\"                                : \"" + populateItem.active + "\",\r\n                                                " +
              "\"groupCode\"     : \"" + populateItem.groupCode + "\",\r\n                                                " +
              "\"groupName\"    : \"" + populateItem.groupName + "\",\r\n                                                " +
              "\"markCode\"                       : \"" + populateItem.markCode + "\",\r\n                                                " +
              "\"markName\"                     : \"" + populateItem.markName + "\",\r\n                                                " +
              "\"retailPrice\"        : \"" + populateItem.retailPrice.Replace(",", ".") + "\",\r\n                                                " +
              "\"retailPriceUK\"        : \"" + populateItem.retailPriceUK.Replace(",", ".") + "\",\r\n                                                " +
              "\"specialPrice\"        : \"" + populateItem.specialPrice.Replace(",", ".") + "\",\r\n                                                " +
              "\"specialPriceUK\"        : \"" + populateItem.specialPriceUK.Replace(",", ".") + "\",\r\n                                                " +
              "\"fromDate\" : \"" + populateItem.fromDate + " 00:00:00\",\r\n " +
              "\"finalDate\" : \"" + populateItem.finalDate + " 23:59:59\", \r\n" +
              "\"colorName\"      : \"" + populateItem.colorName + "\",\r\n                                                " +
              "\"sizeName\"                        : \"" + populateItem.sizeName + "\",\r\n                                                " +
              "\"seasonName\" : \"" + populateItem.seasonName + "\",\r\n                                                " +
              "\"itemBalance\"   : \"" + populateItem.itemBalance + "\",\r\n                                                " +
              "\"itemSubCode\"                : \"" + populateItem.itemSubCode + "\",\r\n                                                " +
              "\"itemSubCode1\"              : \"" + populateItem.itemSubCode1 + "\",\r\n                                                " +
              "\"itemSubCode2\"              : \"" + populateItem.itemSubCode2 + "\",\r\n                                                " +
              "\"itemSubCode3\"              : \"" + populateItem.itemSubCode3 + "\",\r\n                                                " +
              "\"itemSubCode4\"              : \"" + populateItem.itemSubCode4 + "\",\r\n                                                " +
              "\"colorName2\"   : \"" + populateItem.colorName2 + "\",\r\n                                                " +
              "\"sizeName2\"      : \"" + populateItem.sizeName2 + "\",\r\n                                                " +
              "\"itemCode2\"      : \"" + populateItem.itemCode2 + "\",\r\n                                                " +
              "\"onWeb\"                             : \"" + populateItem.onWeb + "\",\r\n                                                " +
              "\"sizeWebUK\"     : \"" + populateItem.sizeWebUK + "\",\r\n                                                " +
              "\"sizeWebUS\"     : \"" + populateItem.sizeWebUS + "\",\r\n                                                " +
              "\"sizeWebEU\"     : \"" + populateItem.sizeWebEU + "\",\r\n                                                " +
              "\"sizeWebFR\"      : \"" + populateItem.sizeWebFR + "\",\r\n                                                " +
              "\"sizeWebJPN\"   : \"" + populateItem.sizeWebJPN + "\"\r\n                                " +
              "}\r\n";
             

                firstTime = false;

            }

               parameter += "   ]\r\n";
               parameter +=  "}";

            request.AddParameter("application/json", parameter, ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }





        public String Authenticate(string url, string postData, int web)
        {
            if (!runThisTime(web)) return "";
           
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


        public String GetProductBySKU(String url, int web)
        {
            if (!runThisTime(web)) return "";

            RestClient client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            AccessToken = AccessToken.Replace("\"", "");
            request.AddHeader("authorization",
            "Bearer " + AccessToken
                );

            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }

        public String GetProductBySKUParseResponce(String url, int web)
        {
            if (!runThisTime(web)) return "";

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


        public String UpdateProductQty(String url, String Qty, int web)
        {
            if (!runThisTime(web)) return "";

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

        public String InsertUpdateProduct(String url, int web)
        {
            if (!runThisTime(web)) return "";

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


        public String updateProductQtys(String url, List<PopulateItem> items, int web)
        {
            if (!runThisTime(web)) return "";

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



        public String updateProductPrices(String url, List<PopulateItem> items, int web)
        {
            if (!runThisTime(web)) return "";

            String ItemsInJson = "";
            bool FirstLoop = true;


            if (items.Count > 0) ItemsInJson +=
                                        "    { \r\n " +
                                        "     \"items\":[ \r\n";

            foreach (PopulateItem item in items)
            {
                if (!FirstLoop) ItemsInJson += ",";
                ItemsInJson += " { \r\n" +
                                            "   \"itemId\" : \"" + item.itemId + "\",\r\n " +
                                            "   \"itemCode\" : \"" + item.itemCode + "\",\r\n " +
                                            "   \"retailPrice\" : \"" + item.retailPrice.Replace(",", ".") + "\",\r\n " +
                                            "   \"retailPriceUK\" : \"" + item.retailPriceUK.Replace(",", ".") + "\",\r\n " +
                                            "   \"specialPrice\" : \"" + item.specialPrice.Replace(",", ".") + "\",\r\n " +
                                            "   \"specialPriceUK\" : \"" + item.specialPriceUK.Replace(",", ".") + "\",\r\n " +
                                            "   \"fromDate\" : \"" + item.fromDate + " 00:00:00\",\r\n " +
                                            "   \"finalDate\" : \"" + item.finalDate + " 23:59:59\" \r\n" +
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
