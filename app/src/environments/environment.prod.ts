export const environment = {
	production: false,
	intervaloExecucaoJob: 30,
	urlApiBase: "https://apontamento-api.azurewebsites.net/api/",
	urlTfs: "http://tfs.vixteam.com.br:8080/tfs/",
	urlChannel: "https://vixteam.jexperts.com.br/channel",
	urlGraphAzure: "https://graph.microsoft.com/v1.0/me/",
	chaveStorageToken: "8e294461-b28a-4221-a1c6-f1a246df5cb6",
	chaveStorageUsuario: "20c919d4-37ad-4311-9a54-c262ba5c146e",
	chaveStorageTarefasFixadas: "afd2c0d0-e22b-4995-a35f-b7dedc150134",
	chaveStorageConfiguracoes: "81684d56-48db-4317-ac66-515cf96e0a1f",
	azureAd: {
		returnUrl: "https://apontamento-vixteam.azurewebsites.net/",
		authority: "https://login.microsoftonline.com/organizations/",
		clientId: "ae7b62cf-5a6d-4f4f-9d66-f0596be9ca37"
	}
};
