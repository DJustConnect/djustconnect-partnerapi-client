# DjustConnect PartnerAPIClient
##Consumerclient
* Addresses the partnerapi from a data-consumer perspective
* Choose appropriate constructor 
	- parameter: HttpClient  
		initiates Consumerclient with your ready-made httpclient
	- parameters: thumbprint, subscriptionkey  
		initiates Consumerclient with an HttpClient presenting a certificate from your Local Machine's personal certificate store.
	- parameters: thumbprint, subscriptionkey, keyvaultname, tenantID, certSecretname   
		initiates Consumerclient with an HttpClient presenting a certificate from your Azure key vault.  
* Catered endpoints:
	- FarmMapping (GET)
	GetFarmMappingAsync(string[] requestIDs, string[] responseIDs, string farmIDType)  
		+ requestIDs: the farmIds for which mappings are requested  
		+ responseIds: specify the Id's to be included in response  
	        KBO: "4c17a3f2-c03d-4d65-8440-3a896b245753"
            Beslag nummer: "324a23eb-b4bc-4de1-a01b-0e478afac252"
            Keuring Spuittoestellen: "dd03e71c-d114-4cce-a5fe-6843f1fc8878"
            PE nummer: "d55fe787-6ea0-46e8-9f00-d9e5e86bad2b"
		+ farmIdType: specifies the type (cfr responseIds) of the ID used as requestId, e.g. if KBO then farmIdType = "4c17a3f2-c03d-4d65-8440-3a896b245753"
	- ConsumerAccess (GET, POST)  
		+ GET: get your ConsumerAccess, params = no, response ConsumerAccessDTO
		+ POST: create/update your ConsumerAccess.  
		params: no  
		requestbody = ConsumerAccessDTO  
		response: HttpStatusCode   
		To update, first GET, apply changes to response ConsumerAccessDTO, add changed DTO to POST requestbody  
	- FarmIdType (GET)  
		params: no  
		response: all farmIdTypes (farmIdTypeDTO)
	- Resource (GET)   
		params: no  
		response: all resources (ResourceDTO)  
	- ResourceHalth (GET)  
		params: string resourcetypeID  
		response: resourceHealth object for requested resource (ResourceHealthDTO)
	- Rarstatus (GET) - Returns the statuses of your Resource Access Requests  
		params: resourceNameFilter, StatusFilter, apiNameFilter, ProviderNAmeFilter, PageNumber, PageSize, Sort  
		response: RarStatusDTO
	- DarStatus (GET) - Returns the current status of your Data Access Requests  
		params: FarmNumberFilter, ResourceNameFilter, ResourceIdFilter, FarmstatusFilter, DarStatusFilter, PageNumber, PageSize, Sort  
		response: DarStatusDTO



