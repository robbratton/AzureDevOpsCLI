using System;
using System.Text;

namespace DevOpsTools
{
    public static class UriHelper
    {
        public static string CombineUriPath(params string[] inputs)
        {
            var output = "";

            foreach (var input in inputs)
            {
                if (
                    !string.IsNullOrEmpty(output)
                    &&
                    !output.EndsWith("/"))
                {
                    output += "/";
                }

                output += input;
            }

            return output;
        }

        public static string CombineUriQuery(params string[] inputs)
        {
            var output = "";

            foreach (var input in inputs)
            {
                if (
                    !string.IsNullOrEmpty(output)
                    &&
                    !output.EndsWith("&"))
                {
                    output += "&";
                }

                output += input;
            }

            return output;
        }

        public static string CombineUriParts(string basePath, string queryString)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                throw new ArgumentException("Value must ot be null or whitespace", nameof(basePath));
            }

            var output = new StringBuilder();
            output.Append(basePath);

            if (!string.IsNullOrEmpty(queryString))
            {
                output.Append("?").Append(queryString);
            }

            return output.ToString();
        }
    }
}