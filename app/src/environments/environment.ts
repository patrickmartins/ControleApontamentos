// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
	production: false,
	urlApiBase: "https://localhost:5001/api/",
	urlTfs: "http://tfs.vixteam.com.br:8080/tfs/",
	urlChannel: "https://vixteam.jexperts.com.br/channel",
	urlGraphAzure: "https://graph.microsoft.com/v1.0/me/",
	chaveStorageToken: "8e294461-b28a-4221-a1c6-f1a246df5cb6",
	chaveStorageUsuario: "20c919d4-37ad-4311-9a54-c262ba5c146e",
	chaveStorageTarefasFixadas: "afd2c0d0-e22b-4995-a35f-b7dedc150134",
	chaveStorageConfiguracoes: "81684d56-48db-4317-ac66-515cf96e0a1f",
	azureAd: {
		returnUrl: "http://localhost:4200/",
		authority: "https://login.microsoftonline.com/organizations/",
		clientId: "ae7b62cf-5a6d-4f4f-9d66-f0596be9ca37"
	}
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
