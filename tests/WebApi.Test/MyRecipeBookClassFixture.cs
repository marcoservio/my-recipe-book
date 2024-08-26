using MyRecipeBook.Domain.Extensions;

using System.Collections;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Xunit;

namespace WebApi.Test;

public class MyRecipeBookClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string method, object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizationRequest(token);

        return await _httpClient.PostAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoPostFormData(string method, object request, string token, string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizationRequest(token);

        var multipartContent = new MultipartFormDataContent();

        var requestProperties = request.GetType().GetProperties().ToList();

        foreach (var property in requestProperties)
        {
            var propertyValue = property.GetValue(request);

            if (string.IsNullOrWhiteSpace(propertyValue?.ToString()))
                continue;

            if(propertyValue is IList list)
                AddToMultipartContent(multipartContent, property.Name, list);
            else
                multipartContent.Add(new StringContent(propertyValue.ToString()!), property.Name);
        }

        return await _httpClient.PostAsync(method, multipartContent);
    }

    protected async Task<HttpResponseMessage> DoGet(string method, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizationRequest(token);

        return await _httpClient.GetAsync(method);
    }

    protected async Task<HttpResponseMessage> DoPut(string method, object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizationRequest(token);

        return await _httpClient.PutAsJsonAsync(method, request);
    }

    protected async Task<HttpResponseMessage> DoDelete(string method, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizationRequest(token);

        return await _httpClient.DeleteAsync(method);
    }

    private void AuthorizationRequest(string token)
    {
        if (token.Empty())
            return;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string culture)
    {
        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");

        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }

    private void AddToMultipartContent(MultipartFormDataContent multipartContent, string propertyName, IList list)
    {
        var itemType = list.GetType().GetGenericArguments().Single();

        if (itemType.IsClass && itemType != typeof(string))
        {
            AddClassListToMuiltipartContent(multipartContent, propertyName, list);
        }
        else
        {
            foreach (var item in list)
            {
                multipartContent.Add(new StringContent(item.ToString()!), propertyName);
            }
        }
    }

    private void AddClassListToMuiltipartContent(MultipartFormDataContent multipartContent, string propertyName, IList list)
    {
        var index = 0;

        foreach (var item in list)
        {
            var classPropertiesInfo = item.GetType().GetProperties().ToList();

            foreach (var prop in classPropertiesInfo)
            {
                var value = prop.GetValue(item, null);
                multipartContent.Add(new StringContent(value!.ToString()!), $"{propertyName}[{index}][{prop.Name}]");
            }

            index++;
        }
    }
}
