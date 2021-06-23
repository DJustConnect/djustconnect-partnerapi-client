# DjustConnect PartnerAPIClient
## Consumerclient
* Addresses the partnerapi from a data-consumer perspective  
* Choose appropriate **constructor**  
	- params: HttpClient  
		initiates Consumerclient with your ready-made httpclient  
	- params: thumbprint, subscriptionkey   
		initiates Consumerclient with an HttpClient presenting a certificate from your Local Machine's personal certificate store.  
	- params: thumbprint, subscriptionkey, keyvaultname, tenantID, certSecretname     
		initiates Consumerclient with an HttpClient presenting a certificate from your Azure key vault.   
* **Endpoints** called by ConsumerClient sample-code:  
	- **api/farmer (GetFarmsAsync) (GET)** - returns array of KBO numbers associated with AzureAdb2c userId
		+ params: AzureAdB2CUserId
		+ response: string[] of KBO numbers
	- **api/farmmapping (GetFarmMappingAsync) (GET)** -returns a mapping of associated IDs for every requested (farm)ID  
		+ params: no
		+ requestbody: FarmMappingDTO
			* requestIDs: the farmIds for which mappings are requested  
			* responseIds: specify the Id's to be included in response  
				KBO: 4c17a3f2-c03d-4d65-8440-3a896b245753
				Beslag nummer: 324a23eb-b4bc-4de1-a01b-0e478afac252
				Keuring Spuittoestellen: dd03e71c-d114-4cce-a5fe-6843f1fc8878
				PE nummer: d55fe787-6ea0-46e8-9f00-d9e5e86bad2b
			* farmIdType: specifies the type (cfr responseIds) of the ID used as requestId, e.g. if KBO then farmIdType = "4c17a3f2-c03d-4d65-8440-3a896b245753"
		+ response: FarmMappingResultDTO
	- **api/consumeraccess (GET, POST)**   
		+ GET: get your ConsumerAccess
			* params: no
			* response: ConsumerAccessDTO
		+ POST: create/update your ConsumerAccess.  
			* params: no  
			* requestbody: ConsumerAccessDTO  
			* response: HttpStatusCode   
			To update, first GET ConsumeraccessDTO apply changes, then POST same (updated) DTO in requestbody  
	- **api/farmidtype (GET)**  
		+ params: no  
		+ response: all farmIdTypes (farmIdTypeDTO)
	- **api/resource (GET)**   
		+ params: no  
		+ response: all resources (ResourceDTO)  
	- **api/rarstatus (GET)** - Returns the statuses of your Resource Access Requests  
		+ params: resourceNameFilter[, StatusFilter, apiNameFilter, ProviderNAmeFilter, PageNumber, PageSize, Sort]  
		+ response: RarStatusDTO
	- **api/darStatus (GET)** - Returns the current status of your Data Access Requests  
		+ params: FarmNumberFilter[, ResourceNameFilter, ResourceIdFilter, FarmstatusFilter, DarStatusFilter, PageNumber, PageSize, Sort]    
		+ response: DarStatusDTO  
	- **api/farmstatus (GET)**  
		+ params: farmnumberfilter("no" filter, get all farms and their statuses) || FarmStatusFilter (filter farms on specified status)
		+ response: FarmstatusDTO (farmnumber, Status (HasUser, NotFound, HasNoUser))
	- **api/consumer/resource-health (GET)**  
		+ params: string resourcetypeID  
		+ response: resourceHealth object for requested resource (ResourceHealthDTO)
	- **api/Consumer/push (GET)**  -get your consumer notification endpoint details
		+ params: no
		+ response: NotificationResultDTO
	- **api/Consumer/push/activate (POST)** -activate your pushnotifications endpoint
		+ params: no
		+ requestbody: string(endpoint)
		+ response: statuscode(200 - OK,400 - endpoint not valid ,403 - not a consumer)
	- **api/Consumer/push/deactivate (POST)** -deactivate your pushnotifications endpoint
		+ params: no
		+ requestbody: string(endpoint)
		+ response: statuscode(200 - OK, 403 - not a consumer)

	



