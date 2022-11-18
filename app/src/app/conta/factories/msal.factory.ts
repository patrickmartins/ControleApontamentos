import { MsalGuardConfiguration, MsalInterceptorConfiguration, ProtectedResourceScopes } from "@azure/msal-angular";
import {
  BrowserCacheLocation,
  InteractionType,
  IPublicClientApplication,
  PublicClientApplication,
} from "@azure/msal-browser";
import { environment } from "src/environments/environment";

export class MsalFactory {

	public static MSALInstanceFactory(): IPublicClientApplication {
		return new PublicClientApplication({
			auth: {
				clientId: `${environment.azureAd.clientId}`,
				authority: `${environment.azureAd.authority}`,
				redirectUri: `${environment.azureAd.returnUrl}`,
			},
			cache: {
				cacheLocation: BrowserCacheLocation.LocalStorage,
				storeAuthStateInCookie: false,
			},
		});
	}

	public static MSALGuardConfigFactory(): MsalGuardConfiguration {
		return {
			interactionType: InteractionType.Popup,
			authRequest: {
				scopes: [
					"user.read",
					`api://${environment.azureAd.clientId}/Apontamento.Signin`
				],
			},
		};
	} 

	public static MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
		const protectedResourceMap = new Map<string, Array<string | ProtectedResourceScopes> | null>();
	
		protectedResourceMap.set(`${environment.urlApiBase}conta/login`, [
			{
				httpMethod: 'POST',
				scopes: [`api://${environment.azureAd.clientId}/Apontamento.Signin`]
			}
		]);

		protectedResourceMap.set("https://graph.microsoft.com/v1.0/me", ["user.read"]);
		
		return {
			interactionType: InteractionType.Popup,
			protectedResourceMap,
		}
	}
}
