# dinerware

		bLoyal Dinerware Pos Integration:

			This Readme contains information about the bLoyal Dinerware Integration. This repository contains both the bLoyal frontend and backend integrations for Dinerware. 
		
		bLoyal Dinerware Frontend Integration: 
			
			The Frontend integration works by hooking into Dinerware's functions and launching WinForms that launch to bLoyal POSSnippets. By using the Dinerware.AddInBase dlls, we are able to inject our processes into Dinerware. 
			In order to use the Frontend integration, the user must have access to a bLoyal Director account so that a connection can be made between Dinerware and bLoyal. There is a Configuration Form located within the Frontend Integration that is used for saving off a bLoyal API Key, Dinerware Virtual Client Url, and Dinerware Database Credentials.
		
		
		bLoyal Dinerware Backend Integration:
		
			The Backend integration works through bLoyal's integration batches that imports and exports data between bLoyal and Dinerware. 
			The Backend Integration uses bLoyal's Grid APIs to communicate between Dinerware and bLoyal. Attached to the Backend Integration is a Configuration app that is used for saving off a bLoyal API key, Dinerware Virtual Client URL, and Dinerware database credentials. 
			The user must have access to a bLoyal Director account so that a connection can be made between Dinerware and bLoyal. Settings for what data can be imported and exported, as well as, the frequency at which the backend will check in to sync data can be found within Director. 
