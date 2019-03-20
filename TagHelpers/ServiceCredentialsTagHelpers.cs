using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace SqlServerWebDemo.TagHelpers
{
    /// <summary>
    /// Tag helper for generating the HTML for the service credentials in a VCAP_SERVICES entry
    /// </summary>
    [HtmlTargetElement("service-credentials", Attributes = "credentials")]
    public class ServiceCredentialsTagHelper : TagHelper
    {
        [HtmlAttributeName("credentials")] public Dictionary<string, Credential> Credentials { set; get; }

        private List<string> SecretsList = new List<string>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.AppendHtml("<ul>");
            if (Credentials != null)
            {
                foreach (KeyValuePair<string, Credential> pair in Credentials)
                {
                    GenerateCredentialHtml(pair.Key, pair.Value, output);
                }
            }

            output.Content.AppendHtml("</ul>");

            base.Process(context, output);
        }

        private void GenerateCredentialHtml(string key, Credential credential, TagHelperOutput output)
        {
            if (!string.IsNullOrEmpty(credential.Value))
            {
                // HACK: Redact secrets from credentials.
                var value = credential.Value;
                if (key.Contains("password", StringComparison.InvariantCultureIgnoreCase) ||
                    key.Contains("secret", StringComparison.InvariantCultureIgnoreCase))
                {
                    SecretsList.Add(value);
                    value = "[REDACTED]";
                }

                foreach (var secret in SecretsList)
                {
                    value = value.Replace(secret, "[REDACTED]");
                }

                output.Content.AppendHtml("<li>");
                output.Content.Append(key + "=" + value);
                output.Content.AppendHtml("</li>");
            }
            else
            {
                output.Content.AppendHtml("<li>");
                output.Content.Append(key);
                output.Content.AppendHtml("</li>");

                output.Content.AppendHtml("<ul>");
                foreach (KeyValuePair<string, Credential> pair in credential)
                {
                    GenerateCredentialHtml(pair.Key, pair.Value, output);
                }

                output.Content.AppendHtml("</ul>");
            }
        }
    }
}