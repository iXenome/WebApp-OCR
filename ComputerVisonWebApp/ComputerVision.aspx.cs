﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ComputerVisonWebApp
{
    public partial class ComputerVision : System.Web.UI.Page
    {
        const string subscriptionKey = "6640d38c6e564b93b42e778e72f8cda9";
        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/ocr";
        string fileName;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ImageUpload.HasFile)
            {
                fileName = ImageUpload.FileName;
                MyImage.ImageUrl = "~/images/" + fileName;
                ImageUpload.SaveAs(Server.MapPath("~/images/" + fileName));
                MakeOCRRequest(Server.MapPath("~/images/" + fileName));
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('Image is required!')", true);
            }
        }

        static void getTextOnly(string filePath, string filePath1)
        {
            List<string> lines = new List<string>();
            using(StreamReader sr = new StreamReader(filePath))
            {
                using(StreamWriter sw = new StreamWriter(filePath1))
                {
                    string line;

                    while((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("text"))
                        {
                            if (line.Contains("textAngle"))
                            {
                                //do nothing
                            }
                            else
                            {
                                lines.Add(line);
                                sw.WriteLine(line);
                            }
                        }
                    }
                }
            }
        }

        async void MakeOCRRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters.
            string requestParameters = "language=unk&detectOrientation=true";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                MyTextBox.Text = JsonPrettyPrint(contentString);

                //Save JSON response to a text file with the same name as the image
                string textFilePath = Path.ChangeExtension(Server.MapPath(@"~/images/") + fileName, ".txt");
                File.WriteAllText(textFilePath, JsonPrettyPrint(contentString));

                //Save JSON response (text only) to a text file with the same name as the image
                string textFilePath1 = Server.MapPath(@"~/images/") + Path.GetFileNameWithoutExtension(textFilePath) + "text.txt";
                getTextOnly(textFilePath, textFilePath1);
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }


        /// <summary>
        /// Formats the given JSON string by adding line breaks and indents.
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }
    }
}