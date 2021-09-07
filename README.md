# DjustConnect PartnerAPIClient
## Consumerclient  
The sample code in the ConsumerClient addresses the DjustConnect partnerAPI from a data-consumer perspective.
Following paragraphs illustrate how to set-up the client, how to talk to the api's endpoints and how to process the responses.  

**Choose appropriate constructor**  
Talking to the partnerapi requires a client certificate to be presented at any request. Depending on your workflow, 
this consumerclient offers the following HttpClient-options:  

**Consumerclient with your ready-made Httpclient**  
<pre><code>
        public ConsumerClient(HttpClient httpClient) : base(httpClient)
        {
        } 
</code></pre>
**Consumerclient with HttpClient presenting a certificate from your Local Machine's personal certificate store**   
<pre><code>
        public ConsumerClient(string thumbprint, string subscriptionkey)
        {
            _httpClient = Client.GetHttpClientWithLocalCertificate(thumbprint, subscriptionkey);
        } 
        public static HttpClient GetHttpClientWithLocalCertificate(string thumbprint, string subscriptionkey)
        {
            var certificate = GetCertificateFromLocalCertStore(thumbprint);
            return GetHttpClient(certificate, subscriptionkey);
        }
        private static X509Certificate2 GetCertificateFromLocalCertStore(string thumbprint)
        {
            var store = new X509Store("My", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            if (certificates.Count == 0)
            {
                throw new InvalidOperationException($"Certificate not found in LocalMachine/My.");
            }
            return certificates[0];
        }
        </code></pre>
**Consumerclient with HttpClient presenting a certificate from your Azure key vault**  
<pre><code>
        public ConsumerClient(string thumbprint, string subscriptionkey, string keyvaultname, string tenantId, string certSecretname)
        {
            _httpClient = Client.GetHttpClientWithAzureKeyvaultCertificate(thumbprint, subscriptionkey, keyvaultname, tenantId, certSecretname);
        }
        public static HttpClient GetHttpClientWithAzureKeyvaultCertificate(string thumbprint, string subscriptionkey, string keyvaultname, string tenantId, string certSecretname)
        {
            var certificate = GetCertificateFromKeyVault(thumbprint, keyvaultname, tenantId, certSecretname);
            return GetHttpClient(certificate, subscriptionkey);
        }
        private static X509Certificate2 GetCertificateFromKeyVault(string thumbprint, string keyvaultname, string tenantId, string certSecretname)
        {
            var ascs = new AzureSecretClientService(keyvaultname,tenantId);
            return ascs.GetClientCertificateFromKeyVault(certSecretname);
        }
        public AzureSecretClientService(string keyvaultname, string tenantId)
        {
            DefaultAzureCredentialOptions options = new DefaultAzureCredentialOptions()
            {
                SharedTokenCacheTenantId = tenantId,
                VisualStudioTenantId = tenantId
            };
            _client = new SecretClient(new Uri($"https://{keyvaultname}.vault.azure.net/"),
                                        new DefaultAzureCredential(options));
        }
        public X509Certificate2 GetClientCertificateFromKeyVault(string secretname)
        {
            // Get full certificate + private key object from keyvault with secretclient
            // see --> https://github.com/Azure/azure-sdk-for-js/issues/7647
            var PKCS12 = _client.GetSecret(secretname).Value;
            var PKCS12bytes = Convert.FromBase64String(PKCS12.Value);
            //specify StorageFlags, otherwise WindowsCryptographicException when deploying to Azure
            return new X509Certificate2(
                PKCS12bytes,
                String.Empty, // omit pw
                X509KeyStorageFlags.MachineKeySet |
                X509KeyStorageFlags.PersistKeySet |
                X509KeyStorageFlags.Exportable);
        }
        </code></pre>

### Abstractions  
Except for one endpoint (FarmMapping, see further) GET and POST calls are all redirected to the same generic methods.   
**GET**  
A typical GET call will have the following structure:  
<pre><code>
        public Task&lt;EndpointDTO> GetEndpointAsync()
        {
            return GetEndpointAsync(CancellationToken.None);
        }

        public async Task&lt;EndpointDTO> GetEndpointAsync(CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Endpoint?");

            return await CallAPI(urlBuilder_, GetResult&lt;EndpointDTO>, cancellationToken);
        }  
</code></pre>
**GetResult&lt;T>**  
Generic method for deserializing response jsonObject to a desired DTO.  
<pre><code>
		public static T GetResult&lt;T>(Dictionary&lt;string, IEnumerable&lt;string>> headers, string json)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject&lt;T>(json);
		}
</code></pre>
**CallAPI**  
Generic method calling the PartnerApi on the specified endpoint and returning the response as the specified DTO-type  
<pre><code>
        protected async Task&lt;T> CallAPI&lt;T>(StringBuilder urlBuilder, Func&lt;Dictionary&lt;string, IEnumerable&lt;string>>, string, T> getResult, CancellationToken cancellationToken) where T : class
        {
            try
            {
                using (var request_ = GetRequestMessage(urlBuilder))
                {
                    var response_ = await _httpClient.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.GetResponseHeaders();
                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            try
                            {
                                var result_ = getResult(headers_, responseData_);
                                return result_;
                            }
                            catch (Exception exception_)
                            {
                                throw new DjustConnectException("Could not deserialize the response body.", (int)response_.StatusCode, responseData_, headers_, exception_);
                            }
                        }
                        else if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new DjustConnectException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }
                        return null;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }
    }
</code></pre>

**GetRequestMessage**  
Straightforward helpermethod returning a standard GET HttpRequestMessage for the requested url
<pre><code>
        protected static HttpRequestMessage GetRequestMessage(StringBuilder urlbuilder)
        {
            var request = new HttpRequestMessage();
            request.Method = new HttpMethod("GET");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            var url_ = urlbuilder.ToString();
            request.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
            return request;
        }
</code></pre>

**POST**  
A typical POST call will have the following structure:  
<pre><code>
        public Task PostEndpointAsync(EndpointDTO endpointDTO)
        {
            return PostEndpointAsync(endpointDTO, CancellationToken.None);
        }

        public async Task PostEndpointAsync(EndpointDTO endpointDTO, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Endpoint?");
            await PostAPI&lt;EndpointDTO>(urlBuilder_, cancellationToken, endpointDTO);
        }
</code></pre>

**PostAPI**  
Generic method for sending data to the PartnerAPI (e.g. to configure settings or add consumeraccess subjects, see further)  
<pre><code>        
    protected async Task PostAPI&lt;T>(StringBuilder urlBuilder, CancellationToken cancellationToken, T dto)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            using (var request_ = PostRequestMessage(urlBuilder))
            {
                var response_ = await _httpClient.PostAsync(request_.RequestUri, stringContent, cancellationToken).ConfigureAwait(false);
                try
                {
                    var headers_ = response_.GetResponseHeaders();
                    var status_ = ((int)response_.StatusCode).ToString();
                    if (status_ != "200" && status_ != "204")
                    {
                        throw new DjustConnectException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, null, headers_, null);
                    }
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }
        }
        </code></pre>

**PostRequestMessage**  
Straightforward helpermethod returning a standard Post HttpRequestMessage for the requested url
<pre><code>
        protected static HttpRequestMessage PostRequestMessage(StringBuilder urlbuilder)
        {
            var request = new HttpRequestMessage();
            request.Method = new HttpMethod("POST");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            var url_ = urlbuilder.ToString();
            request.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
            return request;
        }
</code></pre>

**GetResponseHeaders**  
Extension method mapping headers from an HttpResponseMessage to a Dictionary  
<pre><code>
        public static Dictionary&lt;string, IEnumerable&lt;string>> GetResponseHeaders(this HttpResponseMessage response_)
        {
            var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null)
            {
                foreach (var item_ in response_.Content.Headers)
                    headers_[item_.Key] = item_.Value;
            }
            return headers_;
        }
        </code></pre>

##Some Highlighted cases
**GET/POST ConsumerAccess**  
Managing ConsumerAccess in the portal is a tedious and time-consuming task as subscribed farmIDS might run in the hundreds if not thousands. The ConsumerAccess endpoint(s) 
of the PartnerAPI allow you to automate this work to any degree that fits your workflow.  

First of all, GET your ConsumerAccess details (see ConsumerAccessDTO) by calling the GET endpoint **_public Task&lt;ConsumerAccessDTO> GetConsumerAccessAsync()_**  
Next, update(/delete/extend/...) this same ConsumerAccessDTO's collection of FARMIDS to your current requirements and send it back to the POST endpoint 
**_public Task PostConsumerAccessAsync(ConsumerAccessDTO consumerAccessDTO)_**.  
A rudimentary example can be found in the Tests-project:  
<pre><code>
        [Fact]
        public async Task UpdateConsumerAccess()
        {

            // Arrange

            var newId = "test-ilvo" + Guid.NewGuid();
            // Act
            // Get
            var consumerAccess = await _client.GetConsumerAccessAsync();
            var farmIdsCount = consumerAccess.FarmsIds.Count;
            consumerAccess.FarmsIds.Add(newId);
            // Post
            await _client.PostConsumerAccessAsync(consumerAccess);
            // Get again
            var okResult = await _client.GetConsumerAccessAsync();

            // Assert
            Assert.Contains(newId, okResult.FarmsIds);
            Assert.Equal(okResult.FarmsIds.Count, farmIdsCount + 1);
        }
</code></pre>

**DarStatus/RarStatus/FarmStatus**  
Get the resp. status of your **D**ata**A**ccess**R**equests, **R**esource**A**ccess**R**equests and Farms using these GET endpoints. All three endpoints can be 
called with or without filtering. Refer to the Dar-, Rar-, and FarmStatusFilters in the Filters-folder to read about their full specifications.  
Each filter extends the same **PagingFilter** allowing you to specify the desired number of records per page (**PageSize**) and the number of the page to view (**PageNumber**).
Furthermore, each separate filter will then add it's own specific properties allowing you to finetune the way your requested data is returned/displayed: the API responds with four keys in the **header** -
Xpages (the number of pages returned), X-PageNumber (the pagenumber requested in the filter for display), X-PageSize (the amount of records per page as requested in filter) and X-TotalCount (the total 
amount of records in the response). This information is delivered to the user in a PagedResult including the header's paging information and the deserialized response object.
<pre><code>
        public static PagedResult<T> GetPagedResult<T>(Dictionary<string, IEnumerable<string>> headers, string json)
        {
            return new PagedResult<T>
            {
                PageNumber = headers["X-PageNumber"].Select(x => Convert.ToInt32(x)).Single(),
                Pages = headers["X-Pages"].Select(x => Convert.ToInt32(x)).Single(),
                PageSize = headers["X-PageSize"].Select(x => Convert.ToInt32(x)).Single(),
                TotalCount = headers["X-TotalCount"].Select(x => Convert.ToInt32(x)).Single(),
                Result = Newtonsoft.Json.JsonConvert.DeserializeObject<T[]>(json)
            };
        }
</code></pre>  
An indication on how to call these endpoints with filtering can be found in the Tests-project:   
<pre><code>
        [Fact]
        public void GetDarStatusWithFilter()
        {
            // Arrange
            DarStatusFilter filter = new DarStatusFilter
            {
                PageSize = 10,
                PageNumber = 1,
                DarStatus = "Approved",
                FarmStatus = "HasUser",
                ResourceName = "prov-2-test"
            };
            // Act
            var okResult = _client.GetDarStatusAsyncWithFilter(filter, CancellationToken.None).Result;
            // Assert
            Assert.Equal(1, okResult.PageNumber);
            Assert.Equal(10, okResult.PageSize);
            Assert.True(okResult.Result.Length > 0);
            Assert.Equal("HasUser",okResult.Result[0].FarmStatus);
            Assert.Equal("Approved", okResult.Result[0].DarStatus);
        }

        [Fact]
        public void GetRarStatusWithFilter()
        {
            // Arrange
            RarStatusFilter filter = new RarStatusFilter
            {
                ResourceName = "prov-2-test",
                PageSize = 10,
                PageNumber = 1,
                ProviderName = "Test Provider 2",
                Status = "Approved"
                };

            // Act
            var okResult = _client.GetRarStatusAsyncWithFilter(filter, CancellationToken.None).Result;
            // Assert
            Assert.Equal(1, okResult.PageNumber);
            Assert.Equal(10, okResult.PageSize);
            Assert.True(okResult.Result.Length > 0);
            Assert.Equal("Test Provider 2", okResult.Result[0].ProviderName);
            Assert.Equal("Approved",okResult.Result[0].Status);
        }

        [Fact]
        public void GetFarmStatusWithFilter()
        {
            // Arrange
            FarmStatusFilter filter = new FarmStatusFilter
            {
                PageSize = 10,
                PageNumber = 1,
                Status = FarmStatus.HasUser,
                FarmNumber = "0262172489"
            };

            // Act
            var okResult = _client.GetFarmStatusAsyncWithFilter(filter, CancellationToken.None).Result;

            // Assert
            Assert.True(okResult.PageNumber == 1);
            Assert.True(okResult.Result.Length > 0);
            Assert.Equal(FarmStatus.HasUser.ToString(), okResult.Result[0].Status);
        }
</code></pre>

**Consumer/Resource-Health**  
Get the health-details on any resource you have access to by providing it's ID to the Consumer/Resource-Health endpoint.
<pre><code>
        [Fact]
        public async void GetResourceHealthStatus()
        {
            // Arrange - Get all Consumer Access results
            var access = await _client.GetConsumerAccessAsync();

            // Act
            if(access.Resources.Count > 0)
            {
                // check health of first item
                var okResult = _client.GetResourceHealthAsync(access.Resources.First().Id).Result;
                // Assert - a health status should be available
                Assert.True(okResult.Length > 0);
            }
        }
</pre></code>
   
**Consumer/push, push/activate, push/deactivate**  
Get your pushnotifications endpoint calling the consumer/push endpoint, (de)activate it calling resp push/activate or
push/deactivate endpoints providing the notificationendpoint. Simply put: 
<pre><code>
        [Fact]
        public async void GetPushNotificationDetails()
        {
            // asserts will fail when endpoint is deactivated! 
            var result = await _client.GetPushNotificationsEndpointAsync();
            Assert.Contains("samples.djustconnect.be/Consumercsharp/Events", result.Endpoint);
            Assert.NotNull(result.SubscriptionStatus);
        }

        [Fact]
        public async void DectivatePushNotificationEndpoint()
        {
            // Possible StatusCodes (200 = ok, 403 not a consumer (forbidden))
            // Toggle between activate and deactivate then check status with GetPushNotificationDetails
            var result = await _client.DeactivatePushNotificationsEndpointAsync("https://samples.djustconnect.be/Consumercsharp/Events");
            Assert.Equal("OK", result);
        }
        [Fact]
        public async void ActivatePushNotificationEndpoint()
        {   // Possible StatusCodes (200 = ok, 400 = activation endpoint not valid (bad request), 403 not a consumer (forbidden))
            // Toggle between activate and deactivate then check status with GetPushNotificationDetails
            var result = await _client.ActivatePushNotificationsEndpointAsync("https://samples.djustconnect.be/Consumercsharp/Events");
            Assert.Equal("OK", result);
        }
</pre></code>

**Resource**
Get all available resources. A very straightforward use-case could be to check if a specific resource is available by name:  
<pre><code>
        [Fact]
        public void GetResources()
        {
            // Act
            var resources = _client.GetResourcesAsync().Result;
            bool hasWaterAnalysesInagro = false;
            foreach(var resourceDTO in resources)
            {
                if(resourceDTO.Name.Equals("Water analyse rapporten Inagro"))
                {
                    hasWaterAnalysesInagro = true;
                    break;
                }
            }
            // Assert
            Assert.True(hasWaterAnalysesInagro);
        }
</pre></code>

**FarmIdType**  
Gets an overview of all supported farmIdTypes. Useful for FarmMapping (see next), or verifying if an IdType is supported:    
<pre><code>
       [Fact]
        public void GetFarmIdTypes()
        {
            // Arrange
            bool isBeslagNummerSupported = false;
            // Act
            var farmIdTypes = _client.GetFarmIdTypesAsync().Result;
            foreach (var farmIdType in farmIdTypes)
            {
                if (farmIdType.Name.ToLower().Equals("beslagnummer"))
                {
                    isBeslagNummerSupported = true;
                    break;
                }
            }
            // Assert
            Assert.True(isBeslagNummerSupported);
        }
</pre></code>
**FarmMapping** 
Returns a mapping of associated IDs for every requested (farm)ID. This GET method <span style="color:purple;">does not take url-query parameters but the exceptional requestbody</span> parameters.  
Typically you'd call this endpoint after acquiring the array of KBO numbers associated with your AADB2C login and pass this array into the 
FarmMapping endpoint requestbody's requestIDs property. Yet, any type of identifier can in fact be used, as long as it is supported by DjustConnect and you provide 
the correct farmIdType (use the farmIDType endpoint to know what they are). Briefly put, any of these supported IdTypes can be mapped to one or more of the other IdTypes.
<pre><code>
        public async Task<IEnumerable<FarmMappingResultDTO>> GetFarmMappingAsync(string[] requestIDs, string[] responseIDs, string farmIDType)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/farmMapping");
            JObject o = JObject.FromObject(new
            {
                farmIds = requestIDs,
                farmIdType = farmIDType,
                resultFarmIdTypes = responseIDs
            });
            var jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(o);
            var request = GetRequestMessage(urlBuilder_);
            request.Content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<FarmMappingResultDTO>>(responseData).ToList();
        }

        // simple example from tests:
        [Fact]
        public async void FarmMapping()
        {
            var farmmappings = await _client.GetFarmMappingAsync(new string[] { "0262172489" }, new string[] { "4c17a3f2-c03d-4d65-8440-3a896b245753", "324a23eb-b4bc-4de1-a01b-0e478afac252", "dd03e71c-d114-4cce-a5fe-6843f1fc8878", "d55fe787-6ea0-46e8-9f00-d9e5e86bad2b" }, "4c17a3f2-c03d-4d65-8440-3a896b245753");
            Assert.Equal("4402300237", farmmappings.ToArray()[0].FarmMappings.ToArray()[1].Value);
        }

</pre></code>
  
Further information on the partnerAPI's endpoints and schema's can be found in the open spec documentation at "https://partnerapi.djustconnect.be/index.html"   


	

